# 06 — BULK IMPORT (NHẬP HÀNG LOẠT)

> Nguồn: `BTL_LTTQ/Forms/Import/FrmBulkImport.cs`, `BTL_LTTQ/Services/Import/**`,
> `BTL_LTTQ/Repositories/Import/ImportLookupRepository.cs`, `BTL_LTTQ/Models/Import/**`.
> Package: `ClosedXML 0.105.1`, `CsvHelper 33.1.0` (đã hạ từ 34.0.0 vì bản 34 đổi API, mất namespace `CsvHelper.Configuration`).

---

## 1. Bài toán và vì sao cần module này

**Bài toán**: nhập hàng loạt Học viên / Giáo viên / Đăng ký lớp từ file Excel hoặc CSV.

**Vì sao không nhập từng bản ghi bằng form thường**: mỗi lần thêm 1 học viên là 1 vòng: mở form → gõ → chọn danh mục → lưu. Với 200 học viên là 200 vòng và 200 lần gõ sai. Nhập hàng loạt: 1 lần chọn file ⇒ preview toàn bộ ⇒ xác nhận 1 lần.

**Vì sao hỗ trợ cả Excel và CSV**:
- **Excel (`.xlsx`)**: phổ biến nhất với người dùng nghiệp vụ, có định dạng ngày/số, nhiều sheet.
- **CSV**: nhẹ, mở được bằng Notepad/Google Sheets, dễ sinh từ hệ thống khác.
- Thư viện dùng: **ClosedXML** (đọc/ghi `.xlsx`) và **CsvHelper** (đọc `.csv`) — cả hai đều xử lý mã hoá và escape đúng chuẩn.

**Vì sao có cả tệp mẫu**: người dùng không nhớ tên cột tiếng Việt chuẩn ⇒ `ImportTemplateWriter.Luu()` tạo file `.xlsx` mẫu (sheet `DuLieu` chỉ có tiêu đề + sheet `HuongDan`), tải về điền rồi kéo thả lại vào form.

---

## 2. Kiến trúc & DTO

```
FrmBulkImport
 └─ ImportService
      ├─ XemTruoc(path, loai)  → KetQuaPreview
      │    ├─ BangTinhReader.Doc(path, loai)        → KetQuaDocTep { List<ImportDong>, LoiFile, CotBatBuocThieu, CotKhongNhanDien }
      │    ├─ ImportLookupRepository.Nap()           → mã HocVien/GiaoVien/danh mục, Dictionary lớp + sĩ số, HashSet cặp đăng ký
      │    └─ ImportValidator.KiemTra(preview)       → gắn ImportLoi vào từng ImportDong
      └─ ThucHien(preview) → KetQuaImport
           ├─ PermissionService.YeuCau(...)          (HOCVIEN_THEM / GIAOVIEN_THEM / DANGKYLOP)
           └─ DbHelper.ExecuteTransaction(...)       (tất cả trong 1 transaction)
```

**DTO (Models/Import)** — đây là ví dụ **tách Model/DTO rõ ràng** trong dự án:
| DTO | Vai trò |
|---|---|
| `ImportDong` | 1 dòng dữ liệu đọc từ file: `SoDong` + `Dictionary<string,string> DuLieu` (key = tên cột chuẩn DB) + `List<ImportLoi> DanhSachLoi`; có `LayChuoi/LayNgay/LaySo` để parse ngày (nhiều định dạng) và số (8,5 và 8.5) |
| `ImportLoi` | 1 lỗi: `SoDong`, `Cot`, `GiaTri`, `ThongDiep` |
| `KetQuaPreview` | Kết quả preview: `OperationId` (Guid trong bộ nhớ), `DanhSachDong`, `LoiFile`, `CotBatBuocThieu`, `CotKhongNhanDien`, `CoTheImport` |
| `KetQuaImport` | Kết quả ghi: `ThanhCong`, `SoDongDaThem`, `ThongDiep`, `DanhSachLoi`, **`LopBiAnhHuong`** |
| `LopThongTinImport` | Snapshot lớp: `MaLop, TenLop, SiSoToiDa, TrangThai, SiSoHienTai` |

**Nhận diện cột**: `BangTinhReader.ChuanHoa()` bỏ dấu, bỏ khoảng trắng, hạ chữ thường ⇒ "Mã học viên", "MA HOC VIEN", "MaHocVien" đều ra `mahocvien`; kèm danh sách bí danh (`mahv`, `masinhvien`, `hoten`…). Cột không nhận diện được sẽ vào `CotKhongNhanDien` (cảnh báo, không chặn).

---

## 3. Quy trình chi tiết (2 bước: Preview → Confirm)

### Bước 1 — Preview (`ImportService.XemTruoc`)
1. Đọc file (`.xlsx` bằng ClosedXML, `.csv` bằng CsvHelper với `DetectDelimiterValues = {",", ";", "\t", "|"}`, fallback mã hoá CP1258 nếu không phải UTF-8).
2. Xác định cột bắt buộc; thiếu ⇒ dừng, báo lỗi cấp tệp.
3. Nạp dữ liệu tham chiếu từ DB (1 lần): mã học viên/giáo viên, mã danh mục, danh sách lớp + sĩ số, tập cặp đăng ký đã có.
4. `ImportValidator.KiemTra` — các nhóm kiểm tra:
   - Bắt buộc/độ dài (mã ≤ 20 ký tự, chỉ chữ/số/`_ . -`; tên ≤ 100; địa chỉ ≤ 200; SĐT ≤ 20).
   - Ngày hợp lệ (`yyyy-MM-dd`, `dd/MM/yyyy`, …), số hợp lệ.
   - Trùng **trong file** và trùng **với DB**.
   - Mã tham chiếu phải tồn tại (nghề, xếp loại, chức danh, chuyên môn, cơ quan, trình độ, lớp).
   - Lớp phải ở trạng thái `SapKhaiGiang`/`DangHoc` mới nhận đăng ký.
   - **Sức chứa tính theo TỔNG số dòng trong file cho lớp đó** (không kiểm từng dòng riêng lẻ).
5. Form hiển thị grid, tô đỏ dòng lỗi, đếm tổng/hợp lệ/lỗi; **nút "Xác nhận nhập" bị khoá** khi còn lỗi.

### Bước 2 — Confirm (`ImportService.ThucHien`)
1. Kiểm tra quyền theo loại dữ liệu.
2. Mở transaction (`DbHelper.ExecuteTransaction`).
3. **Kiểm tra lại toàn bộ ràng buộc trong transaction** (`KiemTraLai`):
   - Trùng mã (lần này truy vấn trực tiếp DB với `WITH (HOLDLOCK)`).
   - Tồn tại mã tham chiếu (`ImportLookupRepository.TonTai`).
   - Cặp đăng ký đã có (`DaTonTaiCapDangKy` với `UPDLOCK, HOLDLOCK`).
   - Với đăng ký: **khóa dòng lớp** `SELECT … FROM LopHoc WITH (UPDLOCK, HOLDLOCK)` để chặn 2 phiên cùng vượt sức chứa; đếm `COUNT(DangKy)` và so với `SiSoToiDa`.
4. Nếu có lỗi ⇒ ném `ImportValidationException` ⇒ **rollback toàn bộ**, trả lỗi theo dòng.
5. Nếu hợp lệ ⇒ INSERT từng dòng (parameter hóa).
6. Commit. Trả `KetQuaImport` kèm `LopBiAnhHuong` (các lớp bị ảnh hưởng).

### Chính sách All-or-nothing
> "Toàn bộ tệp được nhập trong một lần: nếu có bất kỳ dòng lỗi nào, không dòng nào được ghi."

Cơ chế: **mọi dòng nằm trong cùng 1 transaction**; bất kỳ exception nào ⇒ `DbHelper.ExecuteTransaction` rollback rồi re-throw ⇒ `SoDongDaThem = 0`.

**Đã kiểm chứng**: harness tạo trigger tạm trên `HocVien` ném lỗi khi insert dòng thứ 2 ⇒ sau khi chạy, **dòng thứ 1 cũng không tồn tại** trong DB.

---

## 4. ADO.NET: INSERT từng dòng vs SqlBulkCopy vs Staging Table

| Cách | Ưu | Nhược | Trong dự án |
|---|---|---|---|
| **INSERT từng dòng (hiện tại)** | Đơn giản, đúng transaction, dễ bắt lỗi từng dòng, không cần quyền `BULK INSERT` | Chậm với file rất lớn (hàng nghìn dòng) | ✅ **đang dùng** (`GhiHocVien/GhiGiaoVien/GhiDangKy`) |
| `SqlBulkCopy` | Rất nhanh (hàng loạt, copy theo lô) | Cần đường dẫn file staging, khó gắn "giá trị mặc định/constraint phức tạp", xử lý lỗi từng dòng khó hơn | ❌ chưa dùng |
| **Staging Table** | Load nhanh vào bảng tạm → kiểm tra/đối chiếu → `INSERT … SELECT` | Cần thêm bảng, thêm bước dọn dẹp, phức tạp hóa vòng đời | ❌ chưa dùng |

**Quyết định và lý do**: quy mô dữ liệu của bài toán (vài trăm tới vài nghìn dòng) ⇒ INSERT có tham số hóa là đủ; ưu tiên **tính đúng đắn và trả lỗi chi tiết theo dòng** hơn là tốc độ. Đây là trade-off có chủ ý: nếu sau này cần vài trăm nghìn dòng, bước nâng cấp là **staging table + `INSERT…SELECT`** trong cùng transaction (giữ nguyên nguyên tắc all-or-nothing).

---

## 5. Cách tránh dữ liệu trùng & kiểm tra FK

**Trùng**: 3 tầng
1. Preview: so mã với HashSet mã đang có trong DB + HashSet mã trong file (báo "trùng với dòng N trong tệp" / "đã tồn tại trong hệ thống").
2. Trong transaction: `DaTonTai(...)` với `WITH (HOLDLOCK)` ⇒ chặn cả trường hợp dữ liệu đổi sau preview.
3. DB: PK `HocVien(MaHocVien)`, `GiaoVien(MaGV)`, `DangKy(MaHocVien, MaLop)`; lỗi 2627/2601 được bắt riêng ⇒ thông báo "Dữ liệu đã tồn tại hoặc bị trùng khóa".

**FK**: mã tham chiếu được kiểm tra tồn tại (`ImportLookupRepository.TonTai`) trước khi ghi; ngoài ra DB có FK thật (`HocVien.MaNghe → NgheNghiep`, `DangKy.MaHocVien → HocVien`…) và lỗi 547 được bắt riêng ⇒ "Dữ liệu tham chiếu không hợp lệ".

---

## 6. Nhiều dòng đăng ký vào cùng một lớp & tránh vượt sức chứa

- **Tổng số dòng trong file cho lớp**: validator gom theo `soDongTheoLop[maLop]` và so sánh với `SiSoToiDa - SiSoHienTai`. Nếu tệp có 2 dòng mà lớp chỉ còn 1 chỗ ⇒ **cả 2 dòng** cùng bị báo lỗi (đúng nguyên tắc all-or-nothing, không "cho lọt" một dòng).
- **Trong transaction**: `ImportLookupRepository.DocLopDangKhoa(conn, tran, maLop)` dùng `WITH (UPDLOCK, HOLDLOCK)` ⇒ **xếp hàng** giữa 2 phiên import ⇒ không thể cùng vượt sức chứa.
- **Thứ tự khóa cố định**: các lớp được khóa theo thứ tự alphabet (`OrderBy(x => x, StringComparer.OrdinalIgnoreCase)`) ⇒ giảm khả năng **deadlock** khi 2 tệp import cùng lúc.
- **Trạng thái lớp**: chỉ nhận vào lớp `SapKhaiGiang`/`DangHoc`; lớp `DaKetThuc`/`Huy` bị từ chối.

---

## 7. Module Workflow Automation đã gỡ

- Module Workflow Automation (RulesEngine, bảng `Automation*`, `AutomationEventPublisher`, `BULK_IMPORT_COMPLETED`, `CLASS_ENROLLMENT_CHANGED`, …) đã **bị gỡ** khỏi ứng dụng: không còn code, không còn bảng, không còn nút phát sự kiện, không còn package `RulesEngine`/`Google.OrTools`.
- Vì vậy luồng nhập hàng loạt **không còn có bước phát sự kiện sau commit** nữa; nghiệp vụ import (dữ liệu + transaction + kết quả) đã đủ và tự chứa audit/đối chiếu.

---

## 8. Vì sao KHÔNG có Import History? (phạm vi có chủ ý)

- **Không có** bảng `ImportBatch`/`ImportBatchDetail`, **không có** `FrmImportHistory`, **không** lưu lịch sử import vào DB.
- Thay vào đó dùng `KetQuaPreview.OperationId` (Guid, **chỉ trong bộ nhớ**) để đối chiếu kết quả với lần xem trước trong cùng phiên làm việc.
- **Lý do**: (1) đúng phạm vi đồ án đã thống nhất (không làm thừa); (2) lịch sử import đòi hỏi bảng + màn hình + chính sách giữ dữ liệu (bao lâu?), tăng diện tích bảo trì; (3) audit/đối chiếu nghiệp vụ nằm trong transaction import (`KetQuaImport` trả `LopBiAnhHuong` các lớp bị ảnh hưởng) cho phần kiểm soát lại.
- **Nói thẳng khi bị hỏi**: "Phần lịch sử nhập hàng loạt em cố ý **không** làm trong phạm vi hiện tại; nếu cần, thiết kế tối thiểu là bảng `ImportBatch(OperationId UNIQUE, Loai, TenFile, SoDong, ThanhCong, NguoiThucHien, ThoiGian)` + `ImportBatchDetail(ImportID, SoDong, Cot, GiaTri, ThongDiep)`, ghi trong **cùng transaction** với dữ liệu để nó cũng all-or-nothing."

---

## 9. Tính năng bổ sung đã có
- **Kéo thả tệp**: `FrmBulkImport` có `AllowDrop = true`, nối `DragEnter/DragDrop/DragLeave`; `LayTepTha` chỉ nhận `.xlsx`/`.csv` tồn tại; thả nhiều file ⇒ lấy file đầu và báo rõ; thả xong **tự chạy Preview**.
- **Xuất báo cáo lỗi**: `ImportErrorReportWriter.Luu(...)` xuất `.xlsx` (ClosedXML) hoặc `.csv` (CsvHelper, UTF-8 BOM để Excel mở đúng tiếng Việt) gồm lỗi cấp tệp + bảng chi tiết (Dòng, Cột, Giá trị, Thông điệp).
- **Tệp mẫu**: `ImportTemplateWriter.Luu(loai, path)` — sheet `DuLieu` (tiêu đề, cột bắt buộc tô hồng, có comment "Bắt buộc", filter, freeze dòng 1) + sheet `HuongDan`.

## 9.1 Phân quyền của module nhập hàng loạt (đã sửa)

| Tầng | Quy tắc | Vị trí code |
|---|---|---|
| Menu | Nút "Nhập dữ liệu hàng loạt" chỉ hiện nếu **không phải học viên** và có `HOCVIEN_THEM` hoặc `GIAOVIEN_THEM` hoặc `DANGKYLOP` | `frmMain.ApDungQuyenMenu` |
| Giao diện form | `CoQuyenLoai(loai)`: học viên ⇒ **false** cho mọi loại; còn lại theo quyền của loại | `FrmBulkImport.CoQuyenLoai` |
| Service (lớp bảo vệ cuối) | `KiemTraQuyen(loai)`: học viên ⇒ ném `UnauthorizedAccessException("Học viên không có quyền nhập dữ liệu hàng loạt.")`; còn lại `PermissionService.YeuCau(...)` theo loại | `ImportService.KiemTraQuyen` |

**Vì sao chặn học viên**: quyền `DANGKYLOP` được cấp cho **học viên** (để học viên tự đăng ký lớp bằng form "Đăng ký lớp"). Nhập hàng loạt là thao tác **quản trị dữ liệu** (ghi hàng trăm dòng vào `HocVien`/`GiaoVien`/`DangKy`), không phải thao tác cá nhân ⇒ tách riêng để học viên không thấy và không thể gọi (kể cả khi gọi thẳng Service).

> Đã kiểm chứng bằng harness: đăng nhập `hv01` ⇒ vẫn có `DANGKYLOP` (đăng ký lớp bình thường), nhưng `ThucHien` bị từ chối, **không ghi được dòng nào**, và form chặn cả 3 loại import.

---

## 10. Hạn chế đã biết (nói trước)

1. **Tốc độ**: INSERT từng dòng; file vài nghìn dòng vẫn OK, hàng chục nghìn dòng sẽ chậm.
2. **Bộ nhớ**: đọc **toàn bộ** file vào RAM (`List<ImportDong>`) — file rất lớn có thể tốn bộ nhớ.
3. **Không hỗ trợ `.xls`** (cũ) — chỉ `.xlsx`/`.xlsm`/`.csv`.
4. **Không có thanh tiến trình theo dòng** — `ProgressBar` hiện kiểu marquee trong lúc ghi.
5. **Không có báo cáo kết quả từng dòng thành công** (chỉ có báo cáo lỗi).
6. **Mã lớp/nhân sự giới hạn 20 ký tự, chỉ chữ/số/`_ . -`** — quy tắc riêng của module, không phải của DB.
7. `DangKy.NgayDangKy` để trống ⇒ dùng `CONVERT([date], getdate())` (mặc định của DB).
8. **Module Workflow Automation đã gỡ** (xem mục 7) — không còn bước phát sự kiện sau commit.
