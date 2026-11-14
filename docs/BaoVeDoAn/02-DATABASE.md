# 02 — THIẾT KẾ DATABASE (theo trạng thái DB thật)

> Nguồn: truy vấn trực tiếp `sys.tables`, `sys.foreign_keys`, `sys.check_constraints`, `sys.indexes` trên
> `localhost\SQLEXPRESS` / `QuanLyLopTinHocVaNgoaiNguNganHan` (29 bảng, không tính `sysdiagrams`).
> Script trong repo: `Database/ThoiKhoaBieu.sql`, `Database/PhanQuyen.sql`, `Database/Automation.sql`.

---

## 1. Bản đồ các bảng theo nhóm nghiệp vụ

| Nhóm | Bảng | Ghi chú |
|---|---|---|
| Lõi học viên | `HocVien` (9 dòng), `GiaoVien` (3 dòng) | `HocVien.MaLop` là cột **còn sót** từ trước khi tách `DangKy` |
| Lớp & đăng ký | `LopHoc` (3), `DangKy` (10), `BuoiHoc` (51) | `DangKy` = bảng trung gian N-N; `BuoiHoc` = lịch đã xếp |
| Danh mục | `MonHoc`, `PhongHoc`, `CaHoc`, `ThoiGian`, `ThoiGian_Thu` (8), `NgheNghiep`, `XepLoai`, `ChucDanh`, `ChuyenMon`, `CoQuan`, `TrinhDo` | `ThoiGian_Thu` = mẫu thời gian ↔ thứ trong tuần |
| N-N khác | `GiaoVien_MonHoc` (5) | GV dạy được những môn nào (kèm `MaXepLoai` cho GV đó) |
| Tài khoản & quyền | `TaiKhoan` (3), `PhanQuyen` (54), `ChucNang` (32) | `ChucNang.ChiAdmin` = chỉ Admin được phép (vd `PHANQUYEN`, `TAIKHOAN`) |
| Thông báo | `ThongBao` (0) | sinh bởi Action `CREATE_NOTIFICATION` |
| Automation | `AutomationRule`, `AutomationCondition`, `AutomationAction`, `AutomationExecution`, `AutomationExecutionLog`, `AutomationTask` | xem `05-AUTOMATION-RULESENGINE.md` |

---

## 2. Khóa chính, khóa ngoại, quan hệ

### 2.1 Quan hệ 1-N
```
ChucNang 1 ──< PhanQuyen            (FK MaChucNang, ON DELETE CASCADE)
MonHoc 1 ──< LopHoc >── 1 GiaoVien   (FK MaMon, MaGV)
PhongHoc 1 ──< LopHoc                (FK MaPhong)
CaHoc 1 ──< LopHoc                  (FK MaCa)
ThoiGian 1 ──< LopHoc               (FK MaThoiGian)
LopHoc 1 ──< BuoiHoc                (FK MaLop, ON DELETE CASCADE)
TaiKhoan 1 ──< AutomationRule       (FK CreatedBy)
```

### 2.2 Quan hệ N-N (bảng trung gian)
| Bảng trung gian | Ghép 2 bên | PK | Ghi chú |
|---|---|---|---|
| `DangKy` | `HocVien` × `LopHoc` | **(MaHocVien, MaLop)** | + `NgayDangKy` (default `CAST(GETDATE() AS date)`). FK cả 2 về `HocVien`/`LopHoc`, **ON DELETE CASCADE** ⇒ xoá lớp ⇒ xoá luôn đăng ký của lớp đó |
| `GiaoVien_MonHoc` | `GiaoVien` × `MonHoc` | (MaGV, MaMon) | + `MaXepLoai` FK → `XepLoai` |
| `ThoiGian_Thu` | `ThoiGian` × thứ trong tuần | **(MaThoiGian, Thu)** | `Thu` 2..8 (2=T2 … 8=CN), `CK_ThoiGian_Thu_Thu` |

**Vì sao cần bảng trung gian?** Vì quan hệ thuộc **nhiều-nhiều kèm thuộc tính riêng**:
- 1 học viên đăng ký nhiều lớp; 1 lớp có nhiều học viên ⇒ không thể đặt `MaLop` (hoặc `MaHocVien`) trực tiếp trong bảng cha mà không mất dữ liệu.
- Đăng ký còn có thuộc tính riêng `NgayDangKy` ⇒ bảng trung gian mang thuộc tính, không phải bảng "nối cứng".
- PK composite `(MaHocVien, MaLop)` **chính là** cơ chế chống đăng ký trùng ở tầng DB (không cần code check).

### 2.3 Bảng trung gian cũ và cột `HocVien.MaLop`
- `HocVien.MaLop` (FK → `LopHoc`) **vẫn còn** cùng `DangKy` ⇒ **hai nguồn sự thật** về "học viên thuộc lớp nào".
- `Database/ThoiKhoaBieu.sql` có đoạn migrate `HocVien.MaLop → DangKy` (idempotent, chạy 1 lần) và comment: *"Migrate dữ liệu lớp cũ (HocVien.MaLop -> DangKy)"*.
- Code ứng dụng **đã chuyển sang dùng `DangKy` làm nguồn sự thật**: `LopHocRepository.LayDanhSach` tính sĩ số bằng `(SELECT COUNT(*) FROM DangKy d WHERE d.MaLop = l.MaLop)`; `DangKyRepository.DemSiSo` cũng vậy; `PhamViDuLieu.ChoHocVien` lọc theo `DangKy`.
- **Trả lời phản biện**: "Đúng, đây là nợ kỹ thuật còn tồn tại. Em giữ cột để không phá vỡ dữ liệu cũ và đã dùng `DangKy` làm nguồn sự thật trong toàn bộ nghiệp vụ; bước tiếp theo là drop cột `HocVien.MaLop` sau khi migrate xong."

---

## 3. Ràng buộc toàn vẹn (CHECK) quan trọng

| Constraint | Định nghĩa | Ý nghĩa nghiệp vụ |
|---|---|---|
| `CK_LopHoc_TrangThai` | `TrangThai ∈ {SapKhaiGiang, DangHoc, DaKetThuc, Huy}` | Vòng đời lớp chỉ có 4 trạng thái |
| `CK_LopHoc_SiSo` | `SiSoToiThieu ≥ 0 AND SiSoToiThieu ≤ SiSoToiDa` | Không khai báo sĩ số tối thiểu lớn hơn tối đa |
| `CK_TaiKhoan_VaiTro` | `VaiTro ∈ {Admin, GiaoVien, HocVien}` | |
| `CK_TaiKhoan_HoSo` | Admin: không có MaGV/MaHocVien; GV: có MaGV; HV: có MaHocVien | Tài khoản phải trỏ đúng người |
| `CK_PhanQuyen_VaiTro` | `VaiTro ∈ {GiaoVien, HocVien}` | Admin luôn có toàn quyền, không cần dòng phân quyền |
| `CK_PhanQuyen_ChiCuaMinh` | `ChiCuaMinh = 1 ⇒ CoQuyen = 1` | "Chỉ của mình" mà không có quyền thì vô nghĩa |
| `CK_ThoiGian_Thu_Thu` | `Thu BETWEEN 2 AND 8` | 2..7 = T2..T7, 8 = CN |
| `CK_AutomationRule_TriggerType` | `TriggerType ∈ {CLASS_CREATED, CLASS_ENROLLMENT_CHANGED, CLASS_STATUS_CHANGED}` | **Không có `BULK_IMPORT_COMPLETED`** |
| `CK_AutomationRule_Priority` | `0 ≤ Priority ≤ 10000` | Thứ tự ưu tiên rule |
| `CK_AutomationCondition_Field` | 7 field: CurrentEnrollment, ClassCapacity, MinimumEnrollment, HasTeacher, HasSchedule, DaysUntilStart, DaysUntilEnd | Whitelist trường được phép điều kiện |
| `CK_AutomationCondition_Operator` | 8 toán tử: EQUALS…IS_FALSE | |
| `CK_AutomationCondition_TypeMatch` | Trường số ⇒ toán tử so sánh + `ValueNumber`; trường logic ⇒ `IS_TRUE/IS_FALSE` + `ValueText` | Ràng buộc kiểu dữ liệu của điều kiện |
| `CK_AutomationAction_Type` | 4 loại: CREATE_NOTIFICATION, CREATE_TASK, UPDATE_CLASS_STATUS, WRITE_AUDIT_LOG | Admin không thể tự tạo action mới |
| `CK_AutomationAction_Json` / `_JsonObject` | `ISJSON(ParametersJson)=1` và bắt đầu bằng `{` | Tham số action phải là JSON object |
| `CK_AutomationExecution_Status` | RUNNING/SUCCESS/PARTIAL/FAILED/SKIPPED | |
| `CK_AutomationExecution_Context` | `ContextJson IS NULL OR ISJSON=1` | |
| `CK_AutomationExecutionLog_Ref` | StepType=CONDITION ⇒ có `ConditionID` và không có `ActionID`; StepType=ACTION ⇒ ngược lại | Log bước luôn truy được về điều kiện/hành động |
| `CK_AutomationTask_Completed` | `Status=COMPLETED ⇔ CompletedAt IS NOT NULL` | |
| `CK_AutomationTask_Assignee` | Phải có `AssignedToAccount` hoặc `AssignedToRole` | Task phải có người nhận |
| `CK_ThongBao_MucDo` / `_VaiTro` | INFO/WARNING/ERROR; vai trò hợp lệ | |

---

## 4. Index quan trọng (đọc từ `sys.indexes`)

| Index | Bảng | Cột | Vai trò |
|---|---|---|---|
| `PK_DangKy` | DangKy | (MaHocVien, MaLop) | PK composite + chống đăng ký trùng |
| `UX_BuoiHoc_GiaoVien` | BuoiHoc | **(Ngay, MaCa, MaGV)** unique | **Chống giáo viên dạy trùng giờ ở tầng DB** |
| `UX_BuoiHoc_Phong` | BuoiHoc | **(Ngay, MaCa, MaPhong)** unique | **Chống phòng bị chiếm trùng giờ ở tầng DB** |
| `IX_BuoiHoc_Lop` | BuoiHoc | MaLop | Truy vấn lịch theo lớp |
| `IX_LopHoc_TrangThai` | LopHoc | TrangThai | Lọc lớp theo trạng thái |
| `UX_TaiKhoan_Admin` | TaiKhoan | VaiTro (filtered) | Tối đa 1 Admin |
| `UX_TaiKhoan_MaGV` / `UX_TaiKhoan_MaHocVien` | TaiKhoan | MaGV / MaHocVien | 1 giáo viên / 1 học viên chỉ có 1 tài khoản |
| `UX_AutomationExecution_Dedup` | AutomationExecution | (RuleID, DedupKey) unique | Chống chạy trùng sự kiện ở tầng DB |
| `UX_AutomationRule_RuleName` | AutomationRule | RuleName (filtered IsDeleted=0) | Tên rule là duy nhất |
| `UX_AutomationTask_Open` | AutomationTask | (RuleID, EntityType, EntityID, Title) filtered | Chống tạo trùng task đang mở |
| `IX_AutomationRule_Trigger` | AutomationRule | (TriggerType, IsActive, Priority) filtered | Truy vấn rule theo trigger khi phát sự kiện |
| `IX_AutomationExecution_Cooldown` | AutomationExecution | (RuleID, EntityType, EntityID, StartedAt) | Kiểm tra cooldown |

---

## 5. Câu hỏi phản biện về DB (trả lời theo tư duy, không học thuộc)

**"Tại sao `BuoiHoc` có unique index mà code cũng check, là thừa?"**
→ Không thừa: code check phục vụ **thông báo lỗi thân thiện** và **hiệu năng**; unique index là **lưới an toàn cuối cùng** khi hai phiên xếp lịch chạy đồng thời (race condition). Nếu chỉ dựa vào code thì khi xếp lịch song song, cả hai đều đọc "chưa có lịch" rồi cùng INSERT ⇒ DB là nơi phải phát ra lỗi (2627).

**"Xoá một lớp thì dữ liệu liên quan thế nào?"**
→ `LopHoc` có FK tới `MonHoc/GiaoVien/PhongHoc/CaHoc/ThoiGian` (NO ACTION ⇒ không xoá được nếu danh mục đang được dùng) và `BuoiHoc.MaLop` (CASCADE ⇒ xoá luôn lịch). `DangKy.MaLop` cũng CASCADE ⇒ đăng ký bị xoá theo. Nhưng `HocVien.MaLop` FK là NO ACTION ⇒ **nếu còn học viên tham chiếu `HocVien.MaLop = lớp này` thì xoá lớp sẽ bị chặn (547)** — đây chính là thông báo "Lớp này đang có học viên… không thể xóa" trong `LopHocService.Xoa`. ⇒ Dùng `DangKy` làm nguồn sự thật sẽ gỡ chặn này.

**"Có vi phạm chuẩn hóa không?"**
→ Có **một** vi phạm đáng nói: `HocVien.MaLop` (cột nhân bản của quan hệ N-N đã được chuẩn hóa vào `DangKy`) và `LopHoc.SiSo` (sĩ số được lưu nhưng app luôn tính bằng `COUNT(DangKy)`). Các trường dẫn xuất khác (`DanhKy.NgayDangKy`) vẫn chuẩn.

**"Khi hai người thao tác cùng lúc thì sao?"**
→ Ba lớp bảo vệ:
1. `UPDLOCK, HOLDLOCK` khi đọc số liệu sẽ dùng để quyết định ghi (`DangKyRepository.Them`, `ImportLookupRepository.DocLopDangKhoa`).
2. Unique index (đăng ký trùng, trùng lịch).
3. `UX_AutomationExecution_Dedup` cho Automation.
Ngoài ra `PhanQuyenDAL.Luu` dùng `UPDATE PhanQuyen WITH (UPDLOCK, HOLDLOCK)`.

**"Dữ liệu tăng gấp 10 lần thì sao?"**
→ Những truy vấn nặng: `LopHocRepository.LayDanhSach` có subquery `(SELECT COUNT(*) FROM DangKy ...)` cho **mỗi dòng** ⇒ N+1 ở mức DB. Khắc phục: view/index theo `DangKy(MaLop)` hoặc materialized column. Bảng `BuoiHoc` sẽ lớn nhanh (51 dòng/3 lớp ⇒ vài nghìn dòng/lớp) ⇒ index `(Ngay, MaCa, MaGV)` và `(Ngay, MaCa, MaPhong)` là chỗ cần index phủ; đã có.

**"Thiếu gì trong DB so với thiết kế chuẩn?"**
→ (a) Thiếu script tạo các bảng nghiệp vụ gốc (repo chỉ có script **增量** cho ThoiKhoaBieu/PhanQuyen/Automation) ⇒ cần script `CREATE TABLE` cho `HocVien/GiaoVien/LopHoc/...` để cài mới từ đầu; (b) Thiếu cột audit (`CreatedAt/UpdatedAt/CreatedBy`) trên `LopHoc/HocVien/GiaoVien` — chỉ bảng Automation có; (c) `LopHoc` không có `UpdatedAt` (từng gây lỗi cho `UpdateClassStatusHandler`).

---

## 6. Những điều KHÔNG được nói quá

- Không nói "DB đã chuẩn hóa 3NF hoàn hảo" — có `HocVien.MaLop` và `LopHoc.SiSo` còn sót.
- Không nói "đã có đầy đủ audit trail" — chỉ bảng Automation có; `LopHoc/HocVien` không có cột thời gian cập nhật.
- Không nói "mọi thao tác đều có transaction" — chỉ 6 nơi (đăng ký, xếp lịch, import, lưu rule, sửa rule, lưu phân quyền); thêm/sửa/xoá lớp, môn học là lệnh đơn lẻ.
- Không nói "tự động hóa được toàn bộ" — `CLASS_CREATED` và `CLASS_STATUS_CHANGED` hiện **không có nơi phát trigger**.
