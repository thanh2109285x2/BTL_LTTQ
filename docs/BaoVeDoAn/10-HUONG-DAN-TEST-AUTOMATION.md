# 10 — HƯỚNG DẪN TEST THỬ MODULE WORKFLOW AUTOMATION

> Hướng dẫn này bám đúng code hiện tại (`Services/Automation/**`, `Forms/Automation/**`, `Database/Automation.sql`).
> Có 2 mức test: **Test nhanh trên UI** (không cần dữ liệu thật) và **Test end-to-end** (rule thật sự chạy action).

---

## 0. CHUẨN BỊ

| Việc | Ghi chú |
|---|---|
| Đăng nhập `admin / Admin@123` | Cần quyền `AUTOMATION_QUANLY` (chỉ Admin) để tạo/sửa rule |
| Menu | **Hệ thống → Tự động hóa** (hiện nếu có `AUTOMATION_XEM`; giáo viên có, học viên không) |
| Nút "Quản lý quy tắc" | Chỉ hiện với Admin (`AUTOMATION_QUANLY.ChiAdmin = 1`) |
| SQL kiểm tra | `sqlcmd -S "localhost\SQLEXPRESS" -d "QuanLyLopTinHocVaNgoaiNguNganHan" -E -C -h -1 -W -Q "..."` |

---

## 1. TEST NHANH TRÊN UI (2 phút) — chỉ kiểm tra điều kiện

> **Cách nhanh nhất để test hoàn toàn từ giao diện** (không cần import gì):
> **Hệ thống → Tự động hóa → nút "Phát sự kiện thử"** → chọn lớp + loại sự kiện → **Phát sự kiện**.
> Sự kiện được phát qua đúng `AutomationEventPublisher.PhatSuKienAsync` như khi chạy thật,
> nên Rule được đánh giá và **hành động chạy thật**; xong quay lại tab *"Lần chạy gần đây"* để xem kết quả.
> (Nút này chỉ hiện với Admin — quyền `AUTOMATION_QUANLY`.)

Bấm **Hệ thống → Tự động hóa → Quản lý quy tắc → Thêm**.

1. **Tên rule**: `TEST_DEMO`.
2. **Trigger**: chọn `Thay đổi số học viên đăng ký` (`CLASS_ENROLLMENT_CHANGED`).
   > ⚠️ **Đừng chọn "Tạo lớp học" (`CLASS_CREATED`)** nếu bạn chỉ muốn rule chạy tự động:
   > hiện chỉ Bulk Import mới phát sự kiện thật, nên rule `CLASS_CREATED` sẽ không bao giờ chạy.
   > (Muốn test rule `CLASS_CREATED` ngay trên UI thì dùng nút **"Phát sự kiện thử"** ở Dashboard.)
3. **Thêm điều kiện** → chọn trường **"Số học viên hiện tại"**, toán tử **"Lớn hơn"**, giá trị `0` → OK.
4. **Thêm hành động** → `Ghi nhật ký` (`WRITE_AUDIT_LOG`), nội dung `demo audit` → OK.
5. Bấm **Kiểm tra** ⇒ thông báo *"Cấu hình Rule hợp lệ."*
6. Bấm **Test Rule** ⇒ hộp thoại kết quả:
   - `Số học viên hiện tại > 0: **Đúng**`
   - *(Đây là context **mẫu** mà form dùng: `L01`, CurrentEnrollment = 5, ClassCapacity = 30, MinimumEnrollment = 4, HasTeacher = true, HasSchedule = false, DaysUntilStart = 7, DaysUntilEnd = 60 — xem `FrmRuleEditor.BtnTestRule_Click`.)*
7. **Thử sai để thấy "Sai"**: sửa điều kiện thành `Số học viên hiện tại > 10` → Test Rule ⇒ *"Một số điều kiện sai."* → sửa lại `> 0`.

> **Lưu ý quan trọng**: **Test Rule KHÔNG chạy hành động** và **không ghi vào DB**.
> Nó chỉ gọi `RuleEvaluator.DanhGiaAsync` (đánh giá điều kiện) — không gọi handler, không tạo `AutomationExecution`.
> Muốn test action thật → làm theo mục 2.

### Bảng tra cứu nhanh

| Trường (FieldCode) | Ý nghĩa | Kiểu | Toán tử cho phép |
|---|---|---|---|
| `CurrentEnrollment` | Số học viên hiện tại | số | =, ≠, >, ≥, <, ≤ |
| `ClassCapacity` | Sức chứa tối đa | số | như trên |
| `MinimumEnrollment` | Sĩ số tối thiểu | số | như trên |
| `HasTeacher` | Đã có giáo viên | logic | Đúng / Sai |
| `HasSchedule` | Đã có lịch học | logic | Đúng / Sai |
| `DaysUntilStart` | Số ngày đến ngày bắt đầu | số | như trên |
| `DaysUntilEnd` | Số ngày đến ngày kết thúc | số | như trên |

| Hành động (ActionType) | Ghi vào | Tham số JSON bắt buộc |
|---|---|---|
| `CREATE_NOTIFICATION` | bảng `ThongBao` | `TieuDe`, `NoiDung`, `MucDo` (INFO/WARNING/ERROR) |
| `CREATE_TASK` | bảng `AutomationTask` (gán `AssignedToRole = Admin`) | `TieuDe`, `UuTien` (LOW/NORMAL/HIGH), `HanXuLy` (YYYY-MM-DD, mặc định +3 ngày) |
| `UPDATE_CLASS_STATUS` | `UPDATE LopHoc.TrangThai` | `TrangThai` (SapKhaiGiang/DangHoc/DaKetThuc/Huy) |
| `WRITE_AUDIT_LOG` | bảng `AutomationExecutionLog` | `NoiDung`, `Loai` |

---

## 2. TEST END-TO-END (10 phút) — rule thật sự chạy action

> **Điều kiện tiên quyết**: hiện tại **chỉ có một nơi phát sự kiện** là Bulk Import
> (`FrmBulkImport.PhatSuKienSauCommitAsync`) với trigger `CLASS_ENROLLMENT_CHANGED`.
> ⇒ Muốn rule chạy, phải **nhập hàng loạt đăng ký lớp** (đăng ký bằng form `Đăng ký lớp` **không** phát trigger).

### Bước 1 — Tạo rule
Trong form soạn rule:
- Tên: `DEMO_RULE_AUDIT`
- Trigger: `CLASS_ENROLLMENT_CHANGED`
- Điều kiện: `Số học viên hiện tại` **lớn hơn** `0`
- Hành động: `Ghi nhật ký` với nội dung `Nhập hàng loạt đăng ký lớp`
- Bấm **Lưu** ⇒ rule được ghi vào `AutomationRule` + `AutomationCondition` + `AutomationAction` trong **1 transaction**.

### Bước 2 — Đảm bảo rule đang BẬT
Trong `FrmRuleManagement`, cột trạng thái phải là **"Hoạt động"** (`IsActive = 1`, `IsDeleted = 0`).
Nếu không, sửa ở cột trạng thái hoặc bằng SQL:
```sql
UPDATE dbo.AutomationRule SET IsActive = 1, IsDeleted = 0 WHERE RuleName = 'DEMO_RULE_AUDIT';
```

### Bước 3 — Chuẩn bị dữ liệu để import
Tạo file `DangKy_DEMO.csv` (UTF-8, phải có header tiếng Việt):
```
Mã học viên,Mã lớp,Ngày đăng ký
HV01,L01,2026-01-20
HV02,L01,2026-01-20
```
Lưu ý: lớp `L01` hiện có 5 học viên, `SiSoToiDa = 30` ⇒ còn chỗ.
Nếu import báo "Lớp chỉ còn nhận N học viên" thì lớp đã đầy ⇒ dùng lớp khác hoặc nâng `SiSoToiDa`.

### Bước 4 — Nhập hàng loạt (chính là nơi phát sự kiện)
1. **Quản lý → Nhập dữ liệu hàng loạt** → chọn loại **Đăng ký lớp** → **kéo thả** `DangKy_DEMO.csv` vào cửa sổ.
2. Preview phải **xanh** (không lỗi) → **Xác nhận nhập** ⇒ *"Đã nhập thành công 2 dòng đăng ký lớp."*
3. **Sự kiện được phát ngay sau khi transaction đã COMMIT** (nếu rollback ⇒ không phát gì).

### Bước 5 — Xem kết quả trên UI
1. **Hệ thống → Tự động hóa** ⇒ tab **"Lần chạy gần đây"** ⇒ thấy 1 dòng:
   `Rule = DEMO_RULE_AUDIT | Lớp/Mã = L01 | Trạng thái = SUCCESS | Thời gian = ...`
2. **Double click** dòng đó ⇒ `FrmAutomationExecutionDetail` xem chi tiết từng bước:
   - `CONDITION` — điều kiện có đúng không, giá trị mong đợi vs thực tế.
   - `ACTION` — hành động có chạy không, thông điệp kết quả.
3. Thống kê ở đầu Dashboard: *"Chạy thành công: 1"*.

> ✅ **Chuỗi này đã được kiểm chứng tự động**: harness test "E2E" mô phỏng đúng `FrmBulkImport.PhatSuKienSauCommitAsync`
> (import đăng ký → `KetQuaImport.LopBiAnhHuong` → `PhatSuKienAsync` với token `BULK_IMPORT:{OperationId}:{MaLop}`)
> và kiểm tra execution `SUCCESS` + log `CONDITION` + log `ACTION` ⇒ **74/74 assertion đạt**.

### Bước 6 — Kiểm chứng bằng SQL
```sql
SET NOCOUNT ON;

-- 1) Lần thực thi
SELECT e.ExecutionID, r.RuleName, e.TriggerType, e.EntityID, e.Status,
       e.ConditionsMatched, e.ActionSuccessCount, e.ActionFailCount,
       e.StartedAt, e.DurationMs, e.TriggeredBy
FROM dbo.AutomationExecution e
JOIN dbo.AutomationRule r ON r.RuleID = e.RuleID
ORDER BY e.ExecutionID DESC;

-- 2) Log từng bước (CONDITION có ConditionID, ACTION có ActionID)
SELECT l.LogID, l.StepType, l.StepOrder, l.ConditionID, l.ActionID,
       l.StepName, l.IsSuccess, l.ExpectedValue, l.ActualValue, l.Message
FROM dbo.AutomationExecutionLog l
WHERE l.ExecutionID = <ExecutionID vừa thấy>
ORDER BY l.StepOrder;

-- 3) Tác dụng của action tùy loại
SELECT * FROM dbo.ThongBao ORDER BY ThongBaoID DESC;      -- CREATE_NOTIFICATION
SELECT TaskID, Title, AssignedToRole, Status, DueDate FROM dbo.AutomationTask ORDER BY TaskID DESC;  -- CREATE_TASK
SELECT MaLop, TrangThai FROM dbo.LopHoc;                    -- UPDATE_CLASS_STATUS
```

### Bước 7 — Test chống chạy trùng (dedup)
Import **lại đúng file đó** (cùng học viên, cùng lớp) ⇒ import sẽ bị chặn "đã tồn tại" ở tầng preview ⇒ **không có execution mới**.
Muốn test dedup đúng nghĩa thì cần 2 lần phát cùng token (hiện chỉ xảy ra khi 2 phiên import cùng lúc) — cơ chế:
`DedupKey = SHA256(RuleID|TriggerType|EntityType|EntityID|EventToken)` + unique index `UX_AutomationExecution_Dedup`.

---

## 3. CÁC TÌNH HUỐNG LỖI ĐỂ BIẾT (và cách xử lý)

| Tình huống | Hiện tượng | Nguyên nhân & cách xử lý |
|---|---|---|
| Rule không chạy khi import | Không có dòng trong "Lần chạy gần đây" | (1) Rule đang **tắt** (`IsActive = 0`) hoặc đã bị xoá mềm (`IsDeleted = 1`); (2) Rule dùng trigger khác `CLASS_ENROLLMENT_CHANGED` ⇒ **không có nơi phát**; (3) Điều kiện sai ⇒ có execution `SKIPPED` (kiểm tra tab Lần chạy gần đây, tìm dòng `SKIPPED`) |
| Rule lưu không được | Báo "Lỗi cấu hình" | Cần **tên rule** + **≥ 1 điều kiện** + **≥ 1 hành động**; điều kiện phải đúng kiểu (trường số ⇒ toán tử so sánh + số; trường logic ⇒ Đúng/Sai) |
| Tạo rule với trigger "Nhập dữ liệu hàng loạt hoàn tất" | Báo lỗi SQL (CHECK 547) | `CK_AutomationRule_TriggerType` chỉ cho `CLASS_CREATED / CLASS_ENROLLMENT_CHANGED / CLASS_STATUS_CHANGED`. Trigger `BULK_IMPORT_COMPLETED` **đã khai báo trong code nhưng DB chưa cho phép tạo rule** ⇒ publisher phát xong sẽ no-op. Muốn dùng: `ALTER TABLE dbo.AutomationRule DROP CONSTRAINT CK_AutomationRule_TriggerType;` rồi tạo lại constraint có thêm giá trị đó. |
| Rule "Chưa có giáo viên" không chạy | — | `HasTeacher` lấy từ `LopHoc.MaGV`; nếu lớp chưa gán GV thì điều kiện `HasTeacher = Sai` là **đúng** |
| Action `UPDATE_CLASS_STATUS` làm đổi lớp | `LopHoc.TrangThai` bị đổi | Đây là **hành vi đúng** của action đó — cẩn thận khi demo, dùng `SapKhaiGiang` để trả lại trạng thái ban đầu |
| Action `CREATE_NOTIFICATION` sinh thông báo cho GV khi lớp "Đang học" | `ThongBao.DoiTuongVaiTro = GiaoVien` | Handler chỉ gán đối tượng nhận khi `context.TrangThai == DangHoc`; lớp `SapKhaiGiang` ⇒ thông báo không gắn vai trò |
| Nhiệm vụ tạo ra không ai nhận | `AutomationTask.AssignedToRole = Admin` | Handler luôn gán cho Admin (chưa có cấu hình giao theo GV). Xem ở Dashboard → tab "Nhiệm vụ đang chờ" |
| Sự kiện phát nhưng rule không nhận event | `AutomationExecution` trống | Publisher chỉ nạp rule `WHERE TriggerType = @T AND IsActive = 1 AND IsDeleted = 0` |

---

## 4. DỌN DẸP SAU KHI TEST (để không để lại dữ liệu rác)

```sql
SET NOCOUNT ON;
DELETE FROM dbo.AutomationExecution WHERE RuleID IN (SELECT RuleID FROM dbo.AutomationRule WHERE RuleName LIKE 'DEMO%' OR RuleName LIKE 'TEST%');
DELETE FROM dbo.AutomationRule     WHERE RuleName LIKE 'DEMO%' OR RuleName LIKE 'TEST%';
DELETE FROM dbo.ThongBao          WHERE TieuDe LIKE N'%demo%' OR NoiDung LIKE N'%demo%';
DELETE FROM dbo.AutomationTask    WHERE Title LIKE '%demo%';
-- xoá đăng ký thử nghiệm
DELETE FROM dbo.DangKy WHERE MaLop = 'L01' AND MaHocVien IN ('HV01','HV02') AND NgayDangKy = '2026-01-20';
```
*(Lưu ý: `AutomationCondition`/`AutomationAction` tự bị xoá theo do `ON DELETE CASCADE` khi xoá Rule.)*

---

## 5. NHỮNG GIỚI HẠN PHẢI NÓI TRƯỚC KHI BẢO VỆ

1. **Chỉ `CLASS_ENROLLMENT_CHANGED` có nơi phát** (từ Bulk Import). `CLASS_CREATED` và `CLASS_STATUS_CHANGED` đã định nghĩa nhưng **chưa có code phát** ⇒ rule của 2 trigger đó không bao giờ chạy.
2. **`BULK_IMPORT_COMPLETED` không tạo được rule** do CHECK constraint của DB.
3. **Test Rule dùng context mẫu** (`L01`, 5 học viên…) — không phải dữ liệu thật của lớp bạn đang xét.
4. **MVP chỉ hỗ trợ AND** giữa các điều kiện (chưa có OR/nhóm điều kiện).
5. **Task luôn gán cho Admin**, chưa giao được theo giáo viên.
6. `AutomationContextBuilder` giả định `LopHoc.NgayBD/NgayKT` khác NULL — lớp có ngày NULL sẽ làm publisher **bỏ qua rule âm thầm** (lỗi bị nuốt để không ảnh hưởng nghiệp vụ).
