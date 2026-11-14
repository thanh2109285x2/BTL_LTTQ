# 01 — KIẾN TRÚC, ADO.NET, TRANSACTION & XỬ LÝ LỖI

> Mọi ví dụ đều lấy từ source hiện tại. Số dòng có thể trôi khi bạn sửa code — hãy dùng đường dẫn + tên method khi trả lời.

---

## 1. Vì sao chia tầng? (trả lời được vòng 2)

| Tầng | Thư mục | Trách nhiệm | Không được làm gì |
|---|---|---|---|
| Presentation | `BTL_LTTQ/Forms/**` | Nhận input, hiển thị, gọi Service, xử lý thông báo | Không viết SQL, không tự kiểm tra quyền "cuối cùng" |
| Business | `BTL_LTTQ/Services/**` | Quy tắc nghiệp vụ, chuẩn hóa dữ liệu, gom lỗi thành thông điệp | Không mở connection trực tiếp (trừ chỗ cần transaction với SQL cụ thể) |
| Data Access | `BTL_LTTQ/Repositories/**` | Câu SQL tham số hóa, ánh xạ dữ liệu ↔ Model, áp dụng phạm vi dữ liệu | Không chứa quy tắc nghiệp vụ phức tạp (trừ check quyền cuối) |
| DAL | `BTL_LTTQ/DataAccessLayer/**` | Mở/đóng connection, thực thi SQL, quản lý transaction | Không biết nghiệp vụ |
| Database | SQL Server | Toàn vẹn dữ liệu, ràng buộc, index | — |

**Bằng chứng "Form không chạm DB"**: trong `Forms/`, chỉ `FrmBulkImport.cs` và `frmThoiKhoaBieu.cs` biết đến lớp `Repository` (qua Service); `Forms/HeThong/frmDangNhap.cs` dùng `TaiKhoanDAL` (lớp DAL chuyên biệt). Không có câu `SELECT/INSERT` nào nằm trong file Form.

**Bằng chứng "Service ≠ Repository"**:
- `LopHocService.Them` (Services/LopHocService.cs:18) = validate + kiểm tra trùng + **dịch exception thành thông báo** → trả về `string` (rỗng = thành công).
- `LopHocRepository.Them` (Repositories/LopHocRepository.cs:52) = `PermissionService.YeuCau(...)` + `INSERT` tham số hóa.
→ Service phụ thuộc Repository; Repository không biết Service.

**Nếu đổi SQL Server → MySQL/PostgreSQL**: phải thay `DataAccessLayer/DbHelper.cs` + các câu SQL trong `Repositories/**` (dùng `SqlParameter`, `SqlDbType`, câu SQL T-SQL). `Forms/**` và `Services/**` (trừ chỗ dùng `Microsoft.Data.SqlClient` trực tiếp: `ImportLookupRepository`, `ImportService`) gần như không phải sửa. Nhưng `Models/**` và `Services/Import/ImportService.cs` hiện **phụ thuộc trực tiếp `Microsoft.Data.SqlClient`** (kiểu `SqlConnection`, `SqlTransaction`) → đây là một nợ kỹ thuật thật, nên nói thẳng khi bị hỏi.

---

## 2. ADO.NET thực tế dùng trong project

### 2.1 Bảng API nào dùng ở đâu

| API | Dùng ở đâu | Ghi chú |
|---|---|---|
| `SqlConnection` | `DbHelper.cs:16/29/43/57`, `ImportService.cs`, `ImportLookupRepository.cs` | Mỗi lần gọi tạo connection mới; dùng `using` ⇒ tự `Dispose` ⇒ trả về connection pool |
| `SqlCommand` | khắp nơi trong Repositories | Luôn kèm `SqlParameter` |
| `SqlDataAdapter` | `DbHelper.cs:25` (`new SqlDataAdapter(cmd).Fill(dt)`) | Dùng cho truy vấn trả về `DataTable` |
| `SqlDataReader` | `ImportLookupRepository.cs:65/84/102/133` | Dùng khi cần đọc **nhiều bảng trong 1 connection** (nạp lookup + đọc `DangKy`) |
| `ExecuteNonQuery` | `DbHelper.cs:42` | INSERT/UPDATE/DELETE |
| `ExecuteScalar` | `DbHelper.cs:27` | Trả về 1 giá trị (`COUNT(*)`, `MAX(...)`) |
| `ExecuteReader` | chỉ `ImportLookupRepository` | Truy vấn dạng stream |
| `SqlTransaction` | `DbHelper.ExecuteTransaction` + 6 nơi gọi | Xem bảng dưới |
| `SqlBulkCopy` | **không dùng** | Bulk Import chọn INSERT từng dòng có tham số hóa (xem `06-BULK-IMPORT.md`) |

### 2.2 Vì sao chọn ADO.NET thuần (không ORM)?
- Bài toán cần SQL rõ ràng: khóa dòng `UPDLOCK/HOLDLOCK`, `OUTPUT INSERTED`, transaction nhiều bước, index unique phục vụ chống trùng lịch — các thứ ORM che mờ hoặc làm khó.
- Không thêm dependency nặng; package hiện có chỉ `Microsoft.Data.SqlClient 7.1.1` + `ClosedXML`, `CsvHelper`, `Google.OrTools`, `RulesEngine`.
- Đồ án học phần C# WinForms + SQL Server: mục tiêu là nắm được SQL/ADO.NET, không phải học ORM.

### 2.3 Parameterized query & SQL Injection
- **Quy tắc**: mọi giá trị từ người dùng đều đi qua `SqlParameter`. Ví dụ `LopHocRepository.Them`:
  ```csharp
  DbHelper.ExecuteNonQuery(
      "INSERT INTO LopHoc (MaLop, TenLop, ...) VALUES (@MaLop, @TenLop, ...)",
      new SqlParameter("@MaLop", l.MaLop), ...);
  ```
- SQL Injection là khi dữ liệu người dùng được **nối chuỗi** vào SQL (`"... WHERE MaLop = '" + ma + "'"`), cho phép kẻ xấu chèn `'; DROP TABLE ...` để đổi ý nghĩa câu lệnh. Với tham số hóa, giá trị được gửi **riêng biệt** qua kênh RPC của driver, engine SQL không bao giờ "diễn giải" nó như mã SQL ⇒ không thể phá vỡ cấu trúc câu lệnh.
- **Điểm phải thừa nhận**: có 4 chỗ SQL dùng nội suy chuỗi `$"..."`:
  - `LopHocRepository.cs:17, 76` — nội suy **alias bảng** và điều kiện phạm vi do `PhamViDuLieu` sinh ra.
  - `LopHocTraCuuRepository.cs:16` — nội suy điều kiện lọc từ `PhamViDuLieu`.
  - `ImportLookupRepository.cs:152` — nội suy **tên bảng/cột** (`$"SELECT TOP 1 1 FROM dbo.{bang} WHERE {cot} = @GiaTri"`), giá trị vẫn tham số hóa.
  ⇒ Giá trị người dùng **luôn** tham số hóa; phần nội suy chỉ là định danh do code sinh ra, không phải dữ liệu người dùng. `PhamViDuLieu.KiemTraAlias` còn kiểm tra alias chỉ gồm chữ/số/gạch dưới để chặn SQL injection qua alias.
  → Trả lời phản biện: "Đúng, đây là điểm cần refactor: nên dùng bảng tra cứu mapping tên bảng → SQL cố định để không còn nội suy; tôi đã giới hạn bằng whitelist alias."

### 2.4 Vòng đời Connection
```
using (var conn = new SqlConnection(_connectDBStr))  // không mở ngay
{
    conn.Open();                                        // mở khi cần
    ...                                                 // dùng
}                                                       // Dispose ⇒ trả về pool
```
- Chuỗi kết nối lấy 1 lần trong `DbHelper` (`ConfigurationManager.ConnectionStrings["DB"]`, `App.config`), công khai ra qua `DbHelper.ConnectionString` (dùng bởi `ImportLookupRepository`).
- **Không dùng `SqlDataSource`/binding tự động** ⇒ mọi truy vấn đều là SQL rõ ràng, kiểm soát được.

### 2.5 Transaction — bắt đầu ở đâu, kết thúc ở đâu
`DbHelper.ExecuteTransaction` (DataAccessLayer/DbHelper.cs:53) là **điểm duy nhất** mở transaction trong toàn hệ thống:

```csharp
using (var conn = new SqlConnection(_connectDBStr))
{
    conn.Open();
    using (SqlTransaction transaction = conn.BeginTransaction())
    {
        try { action(conn, transaction); transaction.Commit(); }
        catch { try { transaction.Rollback(); } catch { } throw; }
    }
}
```
- Bắt đầu: `conn.BeginTransaction()` ngay sau khi mở connection.
- Kết thúc: `Commit()` khi callback trả về bình thường; `Rollback()` khi callback ném exception, rồi **re-throw** để tầng trên biết đã thất bại.
- Đơn vị transaction = **1 callback** ⇒ toàn bộ công việc ghi trong callback là nguyên tử.

**6 nơi dùng transaction trong project**:

| Nơi | File:line | Tính năng |
|---|---|---|
| Đăng ký lớp | `DangKyRepository.cs:64` | khóa dòng lớp, đếm sĩ số, INSERT |
| Xếp lịch | `ThoiKhoaBieuRepository.cs:110` | `DELETE FROM BuoiHoc` + INSERT lịch mới (ghi đè toàn bộ) |
| Nhập hàng loạt | `ImportService.cs:83` | kiểm tra lại + ghi tất cả dòng (all-or-nothing) |
| Lưu Rule | `AutomationRuleRepository.cs:126` | tạo Rule + điều kiện + hành động |
| Sửa Rule | `AutomationRuleRepository.cs:166` | xoá điều kiện/hành động cũ rồi tạo mới |
| Lưu phân quyền | `PhanQuyenDAL.cs:67` | `UPDATE PhanQuyen WITH (UPDLOCK, HOLDLOCK)` + INSERT dòng mới |

### 2.6 Lỗi giữa transaction thì sao?
- Mọi `INSERT`/`UPDATE` chưa commit đều **ẩn** với các connection khác (mức đọc READ COMMITTED).
- Khi có exception: `Rollback()` ⇒ **không còn thay đổi nào** trong DB. Bộ test đã chứng minh: trigger tạm ném lỗi ở dòng thứ 2 ⇒ dòng thứ 1 cũng không được ghi (harness `BulkImportTest`, test "Rollback").
- Ràng buộc FK/CHECK/unique bị vi phạm ⇒ SQL Server ném `SqlException` với mã lỗi cụ thể:
  - `2627`/`2601` (trùng khóa/unique) → "Dữ liệu đã tồn tại hoặc bị trùng khóa" (`ImportService.cs`).
  - `547` (vi phạm FK) → "Dữ liệu tham chiếu không hợp lệ" (`ImportService.cs`) / "Lớp này đang có học viên… không thể xóa" (`LopHocService.Xoa`).
- Lỗi **deadlock** (1205) hiện **chưa được xử lý riêng** → sẽ rơi vào catch tổng quát ⇒ thông báo lỗi chung. (Nói thẳng đây là hạn chế.)

### 2.7 Exception handling — 3 tầng trong project
1. **Form**: `try/catch` bao quanh thao tác ghi, hiển thị `MessageBox` thân thiện (`frmLopHoc.cs`, `frmDangKyLop.cs`, `frmThoiKhoaBieu.cs:410`).
2. **Service**: bắt `SqlException`/`UnauthorizedAccessException`/`InvalidOperationException` và **trả về chuỗi lỗi** thay vì ném (`LopHocService.Them:32`).
3. **DAL/Repository**: để exception nổi lên, kèm thông điệp chuẩn hoá (`ImportService` bắt theo mã lỗi SQL; `AutomationEventPublisher` **nuốt toàn bộ lỗi** để Automation không làm hỏng nghiệp vụ đã commit).

> **Điểm cần tự nhận**: `ImportService.ThucHien` có `catch (Exception ex)` tổng quát cuối cùng ⇒ lỗi lạ vẫn được chặn, nhưng thông báo lộ `ex.Message` của SQL Server cho người dùng. Nên nói: "ở phiên bản hiện tại thông báo lỗi còn thô, em sẽ giấu chi tiết kỹ thuật và ghi log."

### 2.8 Model vs DTO
- Project **không tách Entity/DTO riêng** cho phần nghiệp vụ cũ: Repository trả `DataTable` cho UI (VD `LopHocRepository.LayDanhSach`), Service trả `DataTable`/`string`.
- Phần **mới** (Bulk Import, Automation) **có tách DTO rõ ràng**:
  - `Models/Import/*`: `ImportDong`, `ImportLoi`, `KetQuaPreview`, `KetQuaImport`, `LopThongTinImport`.
  - `Models/Automation/*`: `AutomationRule`, `AutomationCondition`, `AutomationAction`, `AutomationExecution`, `AutomationExecutionLog`, `AutomationTask`, `AutomationContext`, `KetQuaDanhGiaRule`.
- `AutomationContext` là **DTO ngữ cảnh** chứa snapshot trạng thái lớp tại thời điểm phát sự kiện (`CurrentEnrollment`, `ClassCapacity`, `HasTeacher`, `HasSchedule`, `DaysUntilStart`…). RulesEngine đánh giá trên DTO này, không truy cập DB ⇒ test được và không phụ thuộc trạng thái DB tại thời điểm đánh giá.

---

## 3. Bảo mật (phần cốt lõi để trả lời nhanh)

| Cơ chế | Vị trí | Ghi chú |
|---|---|---|
| Băm mật khẩu PBKDF2-SHA256 600.000 vòng + salt 16 byte | `Common/MatKhauHelper.cs` | `CryptographicOperations.FixedTimeEquals` chống timing attack |
| So sánh hợp đồng thời | `MatKhauHelper.KiemTra` | |
| Phân quyền theo mã chức năng | `Common/ChucNangConst.cs` (32 mã) + bảng `PhanQuyen` | `PermissionService.Can/YeuCau/ChiCuaMinh` |
| Kiểm tra quyền ở **tầng repository** (lớp bảo vệ cuối) | `LopHocRepository.cs:54,93`, `MonHocRepository.cs:38,47,57` | `PermissionService.YeuCau(...)` ném `UnauthorizedAccessException` |
| Lọc dữ liệu theo phạm vi người dùng | `Common/PhamViDuLieu.cs` | Giáo viên chỉ thấy lớp của mình; học viên chỉ thấy lớp mình đăng ký; Admin không lọc |
| Ràng buộc hồ sơ tài khoản ở DB | `CK_TaiKhoan_HoSo`, `UX_TaiKhoan_Admin` | |
| Expression RulesEngine do **code sinh**, không nhận trực tiếp từ Admin | `Services/Automation/ConditionEvaluator.cs: TaoExpression` | Admin chỉ chọn FieldCode + Operator + Value trong whitelist `AutomationFieldCatalog` |
| Validation đầu vào | Service/Repository + CHECK constraint | Xem `03-NGHIEP-VU.md` |

---

## 4. Bảng "câu hỏi nhanh – trả lời nhanh" cho vòng kiến trúc

| Câu hỏi | Trả lời 1 câu |
|---|---|
| Vì sao không truy cập DB trực tiếp trong Form? | Để tách UI khỏi SQL: đổi câu SQL/DB chỉ sửa tầng dưới, và để kiểm tra quyền tập trung ở một chỗ thay vì rải trong event handler. |
| Service khác Repository thế nào? | Service chứa quy tắc nghiệp vụ và trả về thông điệp lỗi thân thiện; Repository chứa SQL và bảo vệ quyền ở tầng thấp nhất. |
| Nếu RulesEngine lỗi thì nghiệp vụ chính có bị ảnh hưởng không? | Không: `AutomationEventPublisher.PhatSuKienAsync` bọc toàn bộ trong `try/catch` và nuốt lỗi; nghiệp vụ đã commit trước đó. |
| Có test tự động không? | Chưa có unit test trong project; hiện có harness console kiểm chứng 51 assertion cho Bulk Import + Automation + kéo thả (nằm ngoài repo). |
| Transaction bắt đầu/kết thúc ở đâu? | `DbHelper.ExecuteTransaction` là nơi duy nhất `BeginTransaction()`; `Commit()` khi callback trả về, `Rollback()` khi callback ném lỗi. |
