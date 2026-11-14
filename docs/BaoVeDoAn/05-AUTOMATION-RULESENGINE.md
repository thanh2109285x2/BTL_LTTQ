# 05 — WORKFLOW AUTOMATION & MICROSOFT RULESENGINE

> Phạm vi: đọc code thật trong `BTL_LTTQ/Services/Automation/**`, `BTL_LTTQ/Repositories/Automation/**`,
> `BTL_LTTQ/Models/Automation/**`, `BTL_LTTQ/Forms/Automation/**`, `Database/Automation.sql`.
> Package: `RulesEngine 6.0.1` (`BTL_LTTQ.csproj`).

---

## 1. Workflow Automation giải quyết vấn đề gì?

Trước đây, mỗi quy tắc nghiệp vụ ("lớp đủ sĩ số tối thiểu thì báo giáo viên", "học viên đăng ký lớp thì tạo task cho GV") phải viết thẳng vào code. Người quản trị **không thể tự thêm quy tắc** mà không cần sửa & build lại.

Module Automation biến các quy tắc đó thành **dữ liệu**:
`Rule (Trigger + điều kiện + hành động)` lưu trong DB ⇒ Admin tự cấu hình trên giao diện ⇒ hệ thống tự đánh giá và thực thi.

**Trigger / Condition / Action khác nhau thế nào**
| Khái niệm | Ý nghĩa | Ví dụ trong dự án |
|---|---|---|
| **Trigger** | *Khi nào* hệ thống cần xét rule | `CLASS_ENROLLMENT_CHANGED` (có học viên đăng ký lớp), `CLASS_CREATED`, `CLASS_STATUS_CHANGED`, `BULK_IMPORT_COMPLETED` (chưa dùng được) |
| **Condition** | *Điều kiện* để rule được kích hoạt, trên **7 trường** ngữ cảnh lớp | `CurrentEnrollment > 5`, `HasTeacher = true`, `DaysUntilStart <= 7` |
| **Action** | *Hành động* cần thực hiện khi điều kiện đúng | `CREATE_NOTIFICATION`, `CREATE_TASK`, `UPDATE_CLASS_STATUS`, `WRITE_AUDIT_LOG` |

---

## 2. Kiến trúc thật (ai làm gì)

```
FrmBulkImport (sau COMMIT)
   └─ AutomationEventPublisher.PhatSuKienAsync(triggerType, entityType, entityId, eventToken, context)
        1) ActionHandlerRegistry.EnsureInitialized()          // đảm bảo 4 handler đã đăng ký
        2) AutomationRuleRepository.LayDangHoatDong(trigger)  // SELECT ... WHERE TriggerType=@T AND IsActive=1 AND IsDeleted=0 ORDER BY Priority, RuleID
        3) với mỗi rule:
             a) dedupKey = SHA256(RuleID|Trigger|EntityType|EntityID|EventToken)
                - AutomationExecutionRepository.DaTonTaiDedup(...) → bỏ qua nếu đã xử lý
             b) cooldown: TrongThoiGianCho(RuleID, EntityType, EntityID, CooldownMinutes)
             c) Them(execution)  → INSERT AutomationExecution (Status=RUNNING) + OUTPUT INSERTED.ExecutionID
             d) RuleEvaluator.DanhGiaAsync(rule, context)
                   - RulesEngineProvider.Lay(rule) → build Workflow (RuleDefinitionBuilder) + cache engine theo "chuỗi cấu hình"
                   - engine.ExecuteAllRulesAsync(workflowName, new RuleParameter("ctx", context))
                   - map RuleResultTree → KetQuaDanhGiaRule (điều kiện nào đúng/sai, giá trị thực tế)
             e) GhiLogDieuKien(...)  → INSERT AutomationExecutionLog (StepType=CONDITION, ConditionID)
             f) nếu điều kiện sai → Status=SKIPPED, kết thúc rule
             g) nếu đúng → chạy từng Action theo SortOrder:
                    ActionHandlerRegistry.GetHandler(action.ActionType) → handler.ExecuteAsync(...)
                    handler tự ghi log bước thành công vào AutomationExecutionLog
                    nếu handler lỗi → GhiLogHanhDongThatBai, tính ActionFailCount, dừng nếu ContinueOnError=false
             h) CapNhatKetQua(execution) → UPDATE Status (SUCCESS/PARTIAL/FAILED), ConditionsMatched, số action, DurationMs, ContextJson
```

**Điểm mấu chốt**: `AutomationEventPublisher` bọc toàn bộ trong `try { ... } catch { }` ⇒ **lỗi Automation không bao giờ làm hỏng nghiệp vụ đã commit**.

---

## 3. Microsoft RulesEngine chịu trách nhiệm gì? (câu hỏi #1 của giảng viên)

### 3.1 Phần thư viện làm (RulesEngine 6.0.1)
- Nhận một `Workflow` gồm các `Rule` có `Expression` kiểu **LambdaExpression**.
- Biểu diễn expression (FastExpressionCompiler bị **tắt**: `UseFastExpressionCompiler = false` ⇒ dùng `System.Linq.Expressions` an toàn hơn về mặt code injection).
- Đánh giá các rule với đầu vào `RuleParameter("ctx", context)`.
- Trả về `RuleResultTree` (từng rule đúng/sai, exception message nếu có).
- Trong dự án: `AutoExecuteActions = false` ⇒ RulesEngine **chỉ đánh giá điều kiện, không tự chạy hành động**.

### 3.2 Phần do code dự án tự viết (chiếm phần lớn)
| Trách nhiệm | Class/Method trong dự án |
|---|---|
| Chuyển cấu hình rule trong DB ⇒ Workflow JSON của RulesEngine | `RuleDefinitionBuilder.XayDung(rule)` (dựng `RulesEngine.Models.Workflow`, `Rule`, `RuleExpressionType.LambdaExpression`) |
| **Sinh expression an toàn** (không nhận expression từ Admin) | `ConditionEvaluator.TaoExpression(c)` — chỉ ghép `ctx.<FieldCode> <toán tử> <số>` từ whitelist |
| Validate điều kiện (kiểu dữ liệu, toán tử, giá trị) | `ConditionEvaluator.KiemTra(c)`, `KiemTraDanhSach(ds)` |
| Whitelist trường được phép dùng | `AutomationFieldCatalog` (7 field + kiểu dữ liệu + toán tử hợp lệ) |
| Quản lý instance solver + cache theo cấu hình | `RulesEngineProvider.Lay(rule)` (cache `ConcurrentDictionary`, tối đa 200 engine rồi clear) |
| Quyết định rule nào chạy, chống chạy trùng, ghi lịch sử | `AutomationEventPublisher` |
| Thực thi hành động | 4 handler: `CreateNotificationHandler`, `CreateTaskHandler`, `UpdateClassStatusHandler`, `WriteAuditLogHandler` |

> **Nói thẳng**: RulesEngine ở đây là **bộ đánh giá biểu thức**, **không phải** workflow engine. Toàn bộ vòng đời rule (lưu trữ, chọn rule, chống trùng, thực thi action, ghi lịch sử, cooldown, phân quyền) là code của dự án.

### 3.3 Nếu bỏ Microsoft RulesEngine thì hệ thống còn chạy không?
- **Vẫn chạy**, nhưng phần đánh giá điều kiện phải tự viết (so sánh số/boolean theo 8 toán tử — tầm 100 dòng).
- Điều **không** mất: lưu trữ rule, chọn rule theo trigger, dedup/cooldown, lịch sử thực thi, 4 hành động, màn hình cấu hình.
- Điều **mất**: biểu diễn luật bằng biểu thức, `RuleResultTree`, cơ chế chuyển expression→code, và mọi tính năng sẵn có của RulesEngine (ví dụ `RulesEngine` hỗ trợ nhiều rule input, `LocalParam`, `CustomRule`...). Với bài toán 7 trường + 8 toán tử, tự viết là **đủ và dễ kiểm soát hơn**.
- Nói cách khác: **Microsoft RulesEngine là tiện ích đánh giá điều kiện, không phải xương sống của module**.

---

## 4. Rule lưu ở đâu? Chuyển cấu hình thành Workflow JSON thế nào?

**Lưu trữ** (4 bảng, `Database/Automation.sql`):
```
AutomationRule (RuleID PK, RuleName UNIQUE filtered, Description, TriggerType, Priority,
                IsActive, IsDeleted, CooldownMinutes, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy, RowVersion)
   ├──< AutomationCondition (ConditionID PK, RuleID FK CASCADE, FieldCode, Operator, ValueText, ValueNumber, SortOrder)
   └──< AutomationAction   (ActionID PK, RuleID FK CASCADE, ActionType, SortOrder, ParametersJson, ContinueOnError)
```
- `AutomationRuleRepository.Them/Sua` ghi trong **1 transaction** (`:126`, `:166`) — Rule + điều kiện + hành động vào cùng lúc.
- `AutomationRuleRepository.XoaMem(ruleId, nguoiThucHien)` — **xoá mềm** (`IsDeleted = 1`) để giữ lịch sử.
- `DoiTrangThai(ruleId, isActive, ...)` — bật/tắt rule mà không xoá.

**Chuyển thành Workflow** (`RuleDefinitionBuilder.XayDung`):
```csharp
foreach (AutomationCondition c in rule.Conditions) {
    dsDieuKien.Add(new Rule {
        RuleName = "C0", "C1", ...,
        RuleExpressionType = RuleExpressionType.LambdaExpression,
        Expression = ConditionEvaluator.TaoExpression(c),   // vd: "ctx.CurrentEnrollment > 5"
        Enabled = true,
        ErrorMessage = ConditionEvaluator.MoTa(c)          // mô tả thân thiện hiển thị trên UI
    });
}
return new Workflow { WorkflowName = "AutomationRule_" + rule.RuleID, RuleExpressionType = LambdaExpression, Rules = dsDieuKien };
```
- Mỗi điều kiện của rule ⇒ **một Rule của RulesEngine** ⇒ RulesEngine trả kết quả **cho từng điều kiện** (đó là lý do em dùng `RuleResultTree` để đánh dấu từng bước).
- **MVP chỉ hỗ trợ AND**: tất cả điều kiện phải đúng (AND). Chưa hỗ trợ OR/nhóm điều kiện.
- Cache: `RulesEngineProvider.TaoChuKy(rule)` = chuỗi `RuleID#Field|Op|Value|Text;...` ⇒ đổi cấu hình ⇒ cache key đổi ⇒ engine mới được tạo; `XoaCache()` gọi khi rule được lưu/xoá.

**Biểu thức được sinh thế nào (điểm an toàn quan trọng nhất)** — `ConditionEvaluator.TaoExpression`:
- Trường số: `ctx.CurrentEnrollment > 5` (giá trị format `0` bằng InvariantCulture).
- Trường logic: `ctx.HasTeacher == true`.
- **Không bao giờ** nhận chuỗi expression từ Admin ⇒ Admin không thể chạy C# tuỳ ý.

---

## 5. AutomationContext là gì? Context lấy ở đâu?

`AutomationContext` (DTO) = **ảnh chụp trạng thái lớp tại thời điểm phát sự kiện**:
`MaLop, TenLop, TrangThai, NgayBD, NgayKT, CurrentEnrollment, ClassCapacity, MinimumEnrollment, HasTeacher, HasSchedule, DaysUntilStart, DaysUntilEnd, EntityType, EntityID`.

- `AutomationContextBuilder.XayDung(maLop)` đọc `LopHoc` + đếm `DangKy` + kiểm tra `BuoiHoc` có bản ghi không (⇒ `HasSchedule`).
- Publisher chỉ tự dựng context khi `entityType == "CLASS"`; nếu không có context thì **bỏ qua rule** (điều kiện không thể đánh giá).
- Context được lưu lại ở `AutomationExecution.ContextJson` (JSON) để tra cứu về sau.

---

## 6. Action thực thi thế nào? Lỗi action xử lý ra sao?

| ActionType | Handler | Ghi vào | Tham số JSON |
|---|---|---|---|
| `CREATE_NOTIFICATION` | `CreateNotificationHandler` | bảng `ThongBao` | `{"TieuDe","NoiDung","MucDo"}` |
| `CREATE_TASK` | `CreateTaskHandler` | bảng `AutomationTask` | `{"TieuDe","UuTien","HanXuLy"}` |
| `UPDATE_CLASS_STATUS` | `UpdateClassStatusHandler` | `UPDATE dbo.LopHoc SET TrangThai=@...` | `{"TrangThai"}` |
| `WRITE_AUDIT_LOG` | `WriteAuditLogHandler` | `AutomationExecutionLog` | `{"NoiDung","Loai"}` |

- Handler đăng ký trong `ActionHandlerRegistry` (dictionary theo `ActionType`), khởi tạo idempotent bằng `EnsureInitialized()` (gọi ở `Program.Main` và trong publisher).
- Thứ tự: `rule.Actions.OrderBy(a => a.SortOrder)`.
- **Lỗi action**:
  - Handler trả `(false, "lỗi")` ⇒ publisher ghi log bước đó (`StepType=ACTION`, `IsSuccess=0`, `ActionID` set) và tăng `ActionFailCount`.
  - `ContinueOnError = true` (mặc định) ⇒ chạy tiếp action sau; `false` ⇒ dừng chuỗi.
  - Kết quả cuối: `SUCCESS` (không lỗi) / `PARTIAL` (có lỗi nhưng có action thành công) / `FAILED` (toàn bộ lỗi) / `SKIPPED` (điều kiện không đúng).
  - **Lỗi handler không làm hỏng nghiệp vụ** (publisher nuốt exception).
- Handler tự ghi log thành công vào `AutomationExecutionLog` ⇒ lịch sử chi tiết có cả bước điều kiện và bước hành động.

---

## 7. Làm sao tránh thực thi trùng?

**3 lớp**:
1. **Dedup key** = SHA-256 của `RuleID|TriggerType|EntityType|EntityID|EventToken` (`AutomationEventPublisher.TaoDedupKey`) — 64 ký tự hex.
   - `EventToken` là "mã" của một lần phát sự kiện; ví dụ Bulk Import dùng `BULK_IMPORT:{OperationId}:{MaLop}` ⇒ cùng một lần import ⇒ cùng token ⇒ chỉ chạy 1 lần; import khác ⇒ token khác ⇒ chạy lại.
2. **Unique index `UX_AutomationExecution_Dedup (RuleID, DedupKey)`** ⇒ nếu 2 phiên cùng phát sự kiện, chỉ 1 phiên insert được; phiên còn lại bắt `SqlException 2601/2627` ⇒ `Them()` trả `0` ⇒ publisher bỏ qua.
3. **Cooldown** (`Rule.CooldownMinutes`): `TrongThoiGianCho` kiểm tra có execution gần đây của cùng rule+entity không ⇒ bỏ qua nếu trong thời gian chờ.

---

## 8. Làm sao lưu Execution History?

- `AutomationExecution`: 1 dòng cho **một lần rule chạy** — `ExecutionID, RuleID, RuleNameSnapshot, TriggerType, EntityType, EntityID, EventToken, DedupKey, Status, ConditionsMatched, ActionSuccessCount, ActionFailCount, StartedAt, FinishedAt, DurationMs, TriggeredBy, ErrorMessage, ContextJson`.
  - `RuleNameSnapshot` giữ **tên rule tại thời điểm chạy** ⇒ đổi tên rule sau này không làm sai lịch sử.
- `AutomationExecutionLog`: 1 dòng cho **mỗi bước** — `StepType` (CONDITION/ACTION), `StepOrder`, `ConditionID`/`ActionID`, `StepName`, `IsSuccess`, `ExpectedValue`, `ActualValue`, `Message`, `DurationMs`.
  - `CK_AutomationExecutionLog_Ref` bắt buộc: bước CONDITION phải có `ConditionID`, bước ACTION phải có `ActionID` ⇒ log luôn truy được về cấu hình.
- Lịch sử là **bất biến**: không sửa/xoá khi rule thay đổi (rule xoá mềm `IsDeleted`).

---

## 9. Vì sao không cho Admin nhập C# hoặc SQL tùy ý?

1. **An toàn runtime**: Admin nhập C#/SQL ⇒ mã độc chạy trong tiến trình ứng dụng với quyền của app (đọc/ghi toàn DB) ⇒ lỗ hổng RCE. Dự án chỉ cho phép **chọn field trong whitelist + toán tử + giá trị**; expression do `ConditionEvaluator.TaoExpression` **sinh ra** ⇒ không có đường chạy mã tùy ý.
2. **Expression RulesEngine không an toàn nếu nhận chuỗi tự do**: RulesEngine có API `Compile`/`Evaluate` với biểu thức do người dùng cung cấp — đây là điểm yếu của thư viện; dự án **tránh** bằng cách không bao giờ đưa chuỗi người dùng vào expression.
3. **Kiểm soát thay đổi**: cấu hình rule nằm trong DB ⇒ có thể audit, versioning, phân quyền theo vai trò (`AUTOMATION_QUANLY`), và rollback bằng `IsDeleted`.
4. **Ràng buộc DB là lớp bảo vệ cuối**: `CK_AutomationAction_Type` chỉ cho 4 action; `CK_AutomationAction_Json` bắt buộc JSON object; `CK_AutomationCondition_Field/Operator/TypeMatch` chặn cấu hình sai.

---

## 10. Ngân hàng câu hỏi riêng (trả lời 3 mức)

**Q1. RulesEngine có phải workflow engine hoàn chỉnh không?**
- *Mức 1*: Không. Nó là thư viện đánh giá biểu thức; phần workflow (lưu rule, chọn rule, chống trùng, ghi log, thực thi action) là code của em.
- *Mức 2*: Em dùng RulesEngine ở đúng một chỗ — `RuleEvaluator.DanhGiaAsync` gọi `ExecuteAllRulesAsync` với một `Workflow` do em dựng từ DB, và `AutoExecuteActions=false` để nó không tự chạy action. Toàn bộ orchestration do `AutomationEventPublisher` đảm nhiệm.
- *Mức 3*: Nếu muốn đổi sang engine đầy đủ (Temporal, Elsa, Stateless), kiến trúc hiện tại **đã tách đúng**: chỉ cần thay `RuleEvaluator` và `RuleDefinitionBuilder`, còn lưu trữ/dedup/log/handler giữ nguyên. Đó là lý do em tách `IAutomationActionHandler` + `ActionHandlerRegistry`.

**Q2. Nếu bỏ RulesEngine thì sao?**
- *Mức 1*: Hệ thống vẫn chạy; chỉ cần tự viết phần so sánh điều kiện (7 trường, 8 toán tử).
- *Mức 2*: Toàn bộ giá trị của RulesEngine ở đây là biểu diễn & đánh giá biểu thức. Các phần còn lại (DB, dedup, cooldown, execution log, 4 action, UI cấu hình) không phụ thuộc nó.
- *Mức 3*: Thực tế với bài toán này, tự viết có lợi thế: kiểm soát hoàn toàn, không phụ thuộc API `Compile`, và tránh rủi ro "expression do người dùng nhập". Em giữ RulesEngine vì nó cho `RuleResultTree` chi tiết từng điều kiện — thuận tiện cho UI và log.

**Q3. Vì sao không dùng if/else?**
- *Mức 1*: Vì Admin phải **tự thêm được** quy tắc mà không cần sửa code.
- *Mức 2*: if/else ⇒ mỗi quy tắc mới = sửa code + build + deploy + test lại toàn hệ thống. Với RulesEngine, quy tắc là **dữ liệu** trong bảng `AutomationCondition/AutomationAction`, thêm/xoá/sửa ngay trên UI, có lịch sử thực thi riêng.
- *Mức 3*: Nhưng em **không** đẩy hết logic vào engine: điều kiện chỉ được phép dùng 7 trường ngữ cảnh lớp; hành động chỉ có 4 loại đã đăng ký. ⇒ Đây là "rule-based nhưng có kiểm soát", không phải "mở tổng quát".

**Q4. RulesEngine có tự ghi Notification/tạo Task không?**
- *Mức 1*: Không — em tắt `AutoExecuteActions`; RulesEngine chỉ trả kết quả đánh giá.
- *Mức 2*: Việc ghi `ThongBao`/`AutomationTask` do `CreateNotificationHandler`/`CreateTaskHandler` thực hiện, được publisher gọi sau khi điều kiện đúng, và tự ghi 1 dòng `AutomationExecutionLog` để có dấu vết.
- *Mức 3*: Đây là chủ ý: nếu để RulesEngine tự thực thi action, action sẽ chạy trong ngữ cảnh không có transaction của nghiệp vụ, khó ghi log thống nhất, và khó kiểm soát `ContinueOnError`.

**Q5. Hai rule cùng trigger thì sao?**
- *Mức 1*: Cả hai đều chạy, theo `Priority` rồi `RuleID` (`LayDangHoatDong` ORDER BY Priority, RuleID).
- *Mức 2*: Mỗi rule có execution + log riêng; rule này lỗi không chặn rule sau (mỗi rule được bọc try/catch riêng trong `ThucThiRuleAsync`).
- *Mức 3*: Nhưng dedup là **theo rule** (`DedupKey` chứa `RuleID`) nên 2 rule không loại trừ lẫn nhau — đúng ý nghĩa. Có thể thêm "rule loại trừ nhau" sau này.

**Q6. Nếu rule bị tắt giữa lúc xử lý?**
- *Mức 1*: Rule được nạp **một lần** khi phát sự kiện (`LayDangHoatDong`), nên lần chạy đó vẫn dùng snapshot cũ.
- *Mức 2*: Tắt rule có hiệu lực từ lần phát sự kiện **sau**; đây là hành vi chấp nhận được (tương tự "in-flight job").
- *Mức 3*: Nếu cần tắt tức thì, em có thể thêm trạng thái `IsActive` vào `AutomationExecution` và kiểm tra lại trước khi chạy action — hiện chưa làm.

**Q7. Làm sao đảm bảo rule an toàn?**
- *Mức 1*: 4 lớp: whitelist trường + sinh expression bởi code; CHECK constraint DB; 4 action handler cố định; `ParametersJson` phải là JSON object và được parse an toàn.
- *Mức 2*: Admin **không bao giờ** nhập expression/SQL; chỉ chọn field/operator/value trong UI (`FrmDieuKienThem`), `ConditionEvaluator.KiemTra` chặn cấu hình sai trước khi lưu, và CHECK constraint chặn lần nữa ở DB.
- *Mức 3*: Ngoài ra mọi lỗi trong vòng Automation đều bị nuốt để không ảnh hưởng nghiệp vụ, và `ParametersJson` được parse bằng `JsonDocument` + `TryGetProperty` (không `JsonConvert.DeserializeObject<dynamic>` ⇒ không có gadget chain).

**Q8. Người dùng cấu hình rule sai thì sao?**
- *Mức 1*: Bị chặn ở 3 nơi: validate lúc lưu (UI/Service), CHECK constraint lúc ghi DB, và lúc chạy thì `RuleEvaluator` trả `HopLe=false` với lý do ⇒ execution `FAILED` + log lỗi, không ném ra ngoài.
- *Mức 2*: Ví dụ rule có 2 điều kiện, một cái sai kiểu ⇒ `ConditionEvaluator.KiemTraDanhSach` phát hiện ⇒ `KetQuaDanhGiaRule.HopLe=false` ⇒ publisher ghi execution `FAILED` với `ErrorMessage` và log từng điều kiện sai.
- *Mức 3*: Với rule **đúng cú pháp nhưng sai ý nghĩa** (ví dụ `CurrentEnrollment > 1000`) thì hệ thống không thể biết — đó là lý do có nút **Test Rule** trong `FrmRuleEditor` (`:291`): nó gọi đúng `RuleEvaluator` với context mẫu và **không** chạy action.

**Q9. Kiểm thử rule mà không thực hiện action thật?**
- *Mức 1*: Nút "Test Rule" trong form soạn rule: đánh giá điều kiện với context mẫu, hiển thị kết quả từng điều kiện, **không** gọi handler.
- *Mức 2*: Nút gọi `new RuleEvaluator().DanhGiaAsync(rule, context)` — cùng đường code với lúc chạy thật nhưng **không đi qua** `ThucThiRuleAsync` nên không có `Them(execution)`, không có action.
- *Mức 3*: Ngoài ra `AutoExecuteActions=false` trong `ReSettings` là lớp bảo vệ thứ hai: ngay cả khi có action trong workflow, RulesEngine cũng không tự chạy.

---

## 11. Hạn chế phải nói trước (không đợi giảng viên hỏi)

1. **`CLASS_CREATED` và `CLASS_STATUS_CHANGED` chưa có nơi phát**: toàn bộ source chỉ có `FrmBulkImport` gọi `PhatSuKienAsync` ⇒ rule của 2 trigger này không bao giờ chạy.
2. **`BULK_IMPORT_COMPLETED` không tạo được Rule**: `CK_AutomationRule_TriggerType` chỉ cho 3 trigger lớp ⇒ publisher sẽ no-op.
3. **`AutomationContextBuilder` giả định `NgayBD/NgayKT` khác NULL** (`Convert.ToDateTime(r["NgayBD"])`): nếu lớp có ngày NULL sẽ ném exception ⇒ publisher nuốt lỗi ⇒ rule bị bỏ qua **âm thầm**. Cần sửa bằng `IsDBNull` check.
4. **Chưa hỗ trợ OR / nhóm điều kiện** (MVP chỉ AND).
5. **Không có gửi email/tin nhắn thật**: `CREATE_NOTIFICATION` chỉ ghi vào bảng `ThongBao` (hiện 0 dòng, và **chưa có màn hình đọc thông báo**).
6. **Chưa có UI cho `AutomationTask`** ngoài form xem chi tiết (`FrmAutomationTaskDetail`).

### Lối vào từ giao diện (đã có)
```
frmMain → nhóm "Hệ thống" → nút "Tự động hóa"  (hiện nếu có quyền AUTOMATION_XEM)
   └─ FrmAutomationDashboard   (kiểm tra AUTOMATION_XEM khi mở)
        ├─ Tab "Lần chạy gần đây"  → double click → FrmAutomationExecutionDetail
        ├─ Tab "Nhiệm vụ đang chờ"  → double click → FrmAutomationTaskDetail
        ├─ Tab "Thông báo mới"      → double click → đánh dấu đã đọc
        └─ Nút "Quản lý quy tắc"   (chỉ hiện nếu có quyền AUTOMATION_QUANLY)
             └─ FrmRuleManagement (kiểm tra AUTOMATION_QUANLY khi mở)
                  └─ FrmRuleEditor (kiểm tra AUTOMATION_QUANLY khi mở) → thêm/sửa điều kiện, hành động, Test Rule
```
Quyền trong DB: `AUTOMATION_XEM` (GiaoVien có, HocVien không), `AUTOMATION_TASK` (GiaoVien có), `AUTOMATION_QUANLY` (`ChiAdmin = 1` ⇒ chỉ Admin).
