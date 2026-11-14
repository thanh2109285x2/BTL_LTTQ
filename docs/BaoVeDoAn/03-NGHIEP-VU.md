# 03 — NGHIỆP VỤ TỪNG MODULE (mục đích · luồng · ràng buộc · lỗi · Q&A)

> Mỗi module: mục đích → luồng thật (class/method) → DB → validation → lỗi → quyết định thiết kế → ưu/nhược → câu hỏi GV + trả lời mẫu 3 mức.

---

## 3.1 Đăng nhập & tài khoản

**Mục đích**: xác thực người dùng, nạp quyền theo vai trò, đưa `CurrentUser` vào ngữ cảnh toàn hệ thống.

**Luồng thật**
```
Program.Main → frmDangNhap.ShowDialog() → OK → frmMain
frmDangNhap.btnDangNhap_Click (frmDangNhap.cs:36)
  → TaiKhoanDAL.LayTheoTenDangNhap(tenDangNhap)   // SELECT MatKhauHash, Salt, VaiTro, MaGV, MaHocVien, TrangThai
  → MatKhauHelper.KiemTra(mk, salt, hash)          // PBKDF2 + FixedTimeEquals
  → CurrentUser.Dat(ten, vaiTro, maGV, maHocVien)  // Common/CurrentUser.cs
  → PermissionService.Load()                       // nạp bảng PhanQuyen theo vai trò
```

**DB**: `TaiKhoan(TenDangNhap PK, MatKhauHash, Salt, VaiTro, MaGV, MaHocVien, TrangThai)`.
Ràng buộc: `CK_TaiKhoan_VaiTro`, `CK_TaiKhoan_HoSo`, `UX_TaiKhoan_Admin`, `UX_TaiKhoan_MaGV`, `UX_TaiKhoan_MaHocVien`.

**Validation**: băm mật khẩu 600.000 vòng PBKDF2-SHA256 + salt ngẫu nhiên 16 byte; so sánh constant-time. `TaiKhoanDAL.ThemTaiKhoan` chặn tạo Admin thứ hai bằng `SELECT 1 FROM TaiKhoan WITH (UPDLOCK, HOLDLOCK) WHERE VaiTro = @VaiTro`.

**Lỗi**: sai tài khoản/mật khẩu ⇒ thông báo chung "Sai tên đăng nhập hoặc mật khẩu" (không tiết lộ tài khoản tồn tại); tài khoản bị khoá ⇒ "Tài khoản đã bị khóa"; lỗi nạp quyền ⇒ `PermissionService.Clear()` + `CurrentUser.Xoa()` và báo lỗi, **không cho vào hệ thống**.

**Ưu/nhược**: mật khẩu không lưu dạng rõ; nhưng chưa có rate-limit đăng nhập thất bại, chưa có khóa theo số lần thử.

**Q&A**
- *Vì sao dùng salt?* → Hai tài khoản cùng mật khẩu sẽ có hash khác nhau ⇒ không lộ được "người này dùng chung mật khẩu"; muối lấy ngẫu nhiên 16 byte lưu cùng dòng tài khoản.
- *Mức 3 (đào sâu)*: vì sao PBKDF2 600k? → 600.000 vòng SHA-256 làm cho mỗi lần đoán mật khẩu tốn nhiều CPU ⇒ tăng chi phí cho kẻ tấn công offline (rainbow table trở nên vô dụng vì salt khác nhau). `FixedTimeEquals` chống timing attack (thời gian so sánh không phụ thuộc vị trí byte khác nhau).

---

## 3.2 Phân quyền (RBAC + phạm vi dữ liệu)

**Mục đích**: mỗi vai trò thấy/khác được những gì; giáo viên chỉ thấy lớp của mình, học viên chỉ thấy lớp mình đăng ký.

**Ba lớp kiểm soát**
1. **UI** — `frmMain.ApDungQuyenMenu()` ẩn/hiện nút; `PhanQuyenUi.AnNeuKhongCoQuyen(button, maChucNang)`.
2. **Service/Repository** — `PermissionService.YeuCau(maChucNang, thongBao)` ném `UnauthorizedAccessException` (lớp bảo vệ cuối): `LopHocRepository.cs:54/93`, `MonHocRepository.cs:38/47/57`, `ImportService.cs:147/151/155`.
3. **SQL** — `Common/PhamViDuLieu.cs` sinh điều kiện lọc:
   - Giáo viên: `l.MaGV = @PV_MaGV`
   - Học viên: `EXISTS (SELECT 1 FROM DangKy pv_dk WHERE pv_dk.MaHocVien = @PV_MaHocVien AND pv_dk.MaLop = l.MaLop)`
   - Không có quyền ⇒ điều kiện `1 = 0` (chặn chứ không phải lỗi).
   Alias bảng được kiểm tra bằng `KiemTraAlias` (chỉ chữ/số/gạch dưới).

**DB**: `ChucNang` (32 mã: `MaChucNang, TenChucNang, Nhom, ChiAdmin, HoTroLocTheoChu, ThuTu`), `PhanQuyen` (54 dòng: `MaChucNang, VaiTro, CoQuyen, ChiCuaMinh`).

**Lỗi**: `PhanQuyenDAL.Luu` dùng transaction + `UPDLOCK, HOLDLOCK` khi cập nhật quyền để không mất cập nhật khi 2 Admin sửa đồng thời.

**Quyết định thiết kế**: `ChiCuaMinh` được lưu **cùng bảng `PhanQuyen`** (không tách bảng) vì đây là thuộc tính của *cặp* (chức năng, vai trò).

**Ưu/nhược**: mô hình RBAC đơn giản, dễ quản trị; nhưng chưa hỗ trợ quyền theo *đối tượng dữ liệu* tùy ý (chỉ "chỉ của mình" theo vai trò), và `PhanQuyen` chỉ có 2 vai trò (Admin được đặc biệt ưu tiên trong code: `PermissionService.Can` trả `true` cho Admin).

**Q&A**
- *"Kiểm tra quyền ở UI có đủ không?"* → Không, vì UI có thể bị bỏ qua; code kiểm tra lại ở Service/Repository và SQL lọc theo phạm vi ⇒ **3 lớp**.
- *Mức 3*: `PermissionService.Can` trả `true` ngay với Admin ⇒ Admin không cần dòng trong `PhanQuyen`; nhưng `PermissionService.Can` cũng **chặn sẵn** `PHANQUYEN`/`TAIKHOAN` cho non-Admin ⇒ 2 chức năng này không thể cấp cho GV/HV.

---

## 3.3 Quản lý môn học (CRUD hoàn chỉnh)

**Luồng**: `frmMonHoc` → `MonHocService.Them/Sua/Xoa` → `MonHocRepository.Them/Sua/Xoa` → `DbHelper`.
**Validation** (`MonHocService.KiemTra`): mã môn bắt buộc, tên môn bắt buộc, học phí ≥ 0; `MonHocRepository.TonTai` chặn trùng mã.
**Lỗi**: `SqlException 547` khi xoá môn đang được dùng ⇒ "Môn học này đang được sử dụng (lớp học hoặc giáo viên), không thể xóa".
**DB**: `MonHoc(MaMon PK, TenMon, HocPhi)`.
**Ưu/nhược**: đơn giản, hoàn chỉnh; nhưng `HocPhi` chưa được dùng vào báo cáo doanh thu (báo cáo chưa có UI).

---

## 3.4 Quản lý lớp học (CRUD hoàn chỉnh + phạm vi dữ liệu)

**Luồng**: `frmLopHoc` → `LopHocService` → `LopHocRepository` → `DbHelper`.
**Danh mục chọn**: `LookupService.LayMonHoc()/LayGiaoVien()/LayCaHoc()/LayThoiGian()/LayPhongHoc()` (`Repositories/LookupRepository.cs`).

**Validation (`LopHocService.KiemTra`)**: mã/tên lớp bắt buộc; bắt buộc chọn môn, GV, ca, thời gian, phòng; `NgayKT ≥ NgayBD`; `SiSoToiDa ≥ 1`; `1 ≤ SoBuoiTuan ≤ 7`.
**Chống trùng**: `LopHocRepository.TonTai(MaLop)` trước khi INSERT (và PK ở DB).
**Phạm vi dữ liệu khi sửa**: `LopHocRepository.Sua` dựng `UPDATE ... FROM LopHoc l WHERE l.MaLop = @MaLop{dieuKienPhamVi}`; nếu `ChiCuaMinh` thì **không cho sửa `MaGV`** (cột `MaGV` bị loại khỏi SET) ⇒ giáo viên không thể đổi giáo viên phụ trách; nếu update 0 dòng ⇒ ném `InvalidOperationException("Không thể cập nhật: lớp không tồn tại hoặc không thuộc quyền của bạn")`.

**Lỗi**: xoá lớp có học viên ⇒ `547` ⇒ "Lớp này đang có học viên (hoặc dữ liệu liên quan), không thể xóa".

**DB**: `LopHoc(MaLop PK, TenLop, MaMon, MaGV, NgayBD, NgayKT, MaCa, MaThoiGian, MaPhong, SiSo, SiSoToiDa, SoBuoiTuan, TrangThai, SiSoToiThieu)` + `CK_LopHoc_TrangThai`, `CK_LopHoc_SiSo`.

**Ưu/nhược**: đúng luồng CRUD chuẩn, có phạm vi dữ liệu; nhưng cột `SiSo` trong bảng **không được dùng** (app tính `COUNT(DangKy)`) và `TrangThai` **chưa có UI để đổi** (chỉ Automation action `UPDATE_CLASS_STATUS` gán được) ⇒ trạng thái lớp hiện gần như đứng yên ở `SapKhaiGiang`.

---

## 3.5 Đăng ký / hủy đăng ký lớp

**Mục đích**: học viên tự đăng ký lớp còn chỗ; hệ thống phải **không bao giờ** vượt sức chứa.

**Luồng thật** (`Forms/QuanLy/frmDangKyLop.cs`)
```
Form: _repo.LayLopChoHocVien(CurrentUser.MaHocVien, tuKhoa)  // danh sách lớp + cờ DaDangKy
      _repo.LayLopCuaHocVien(maHocVien)                    // lớp đã đăng ký
      _repo.Them(maHocVien, maLop)   → trả "" hoặc thông báo lỗi
      _repo.Huy(maHocVien, maLop)
```
`DangKyRepository.Them` (dòng 60) là nơi quan trọng nhất:
```csharp
DbHelper.ExecuteTransaction((conn, tran) => {
    SELECT SiSoToiDa FROM LopHoc WITH (UPDLOCK, HOLDLOCK) WHERE MaLop = @MaLop   // khóa dòng lớp
    SELECT COUNT(*) FROM DangKy WHERE MaLop = @MaLop                              // sĩ số hiện tại
    if (siSo >= siSoToiDa) throw new InvalidOperationException($"Lớp đã đủ sĩ số ({siSo}/{siSoToiDa}).");
    INSERT INTO DangKy (MaHocVien, MaLop) VALUES (@MaHocVien, @MaLop)
});
```

**Ba tầng chống trùng/vượt số**
1. **Code trong transaction**: `UPDLOCK, HOLDLOCK` ⇒ hai phiên cùng đăng ký sẽ **xếp hàng** trên cùng khóa dòng lớp ⇒ phiên sau đọc sĩ số đã cập nhật ⇒ chặn đúng.
2. **PK composite `PK_DangKy(MaHocVien, MaLop)`** ⇒ đăng ký trùng cùng cặp bị DB từ chối (lỗi 2627 → "Bạn đã đăng ký lớp này rồi.").
3. **FK** ⇒ học viên/lớp không hợp lệ bị chặn (547).

**Sĩ số**: luôn tính động bằng `COUNT(*) FROM DangKy WHERE MaLop = @MaLop` (`DangKyRepository.DemSiSo`, `LopHocRepository.LayDanhSach` subquery). Cột `LopHoc.SiSo` không dùng.

**Hủy đăng ký**: `DELETE FROM DangKy WHERE MaHocVien=@ AND MaLop=@`; nếu 0 dòng ⇒ "Bạn chưa đăng ký lớp này."

**Ưu/nhược**: đúng yêu cầu nghiệp vụ, an toàn khi nhiều người dùng; hạn chế: số lượt đăng ký không giới hạn theo thời gian/khóa học, chưa có "đăng ký có điều kiện trả học phí".

**Q&A**
- *"Hai máy khác cùng đăng ký 1 học viên vào 1 lớp thì sao?"* → Cả hai đều mở transaction, `UPDLOCK, HOLDLOCK` trên dòng `LopHoc` ⇒ một phiên giữ khóa, phiên kia chờ ⇒ khi phiên kia đếm sĩ số đã thấy số mới ⇒ chặn đúng. Ngoài ra PK composite chặn trùng cặp.
- *Mức 3*: vì sao `UPDLOCK` mà không chỉ `HOLDLOCK`? → `HOLDLOCK` giữ khóa đọc đến hết transaction (serializable), `UPDLOCK` buộc khóa **ghi** ngay ⇒ chặn cả hai chiều (đọc-ghi và ghi-ghi). Kết hợp = "khóa dòng lớp theo kiểu cập nhật" ⇒ mọi ghi vào `DangKy` của lớp đó phải xếp hàng.

---

## 3.6 Tra cứu (lớp học, giáo viên) và xuất Excel

- `frmTimKiemLopHoc` → `LopHocTraCuuService.TimKiem(TimKiemLopHoc, out DataTable)` → `LopHocTraCuuRepository.TimKiem` (SQL có điều kiện phạm vi từ `PhamViDuLieu`).
- `frmTimKiemGiaoVien` → `GiaoVienTraCuuService.TimKiem(TimKiemGiaoVien)` → `GiaoVienTraCuuRepository`.
- Xuất Excel: `ExcelHelper.XuatTrangHienTai(form)` (COM Interop `Microsoft.Office.Interop.Excel`) — `frmMain.btnXuatExcel_Click` có kiểm tra `PermissionService.Can(XUAT_EXCEL)`.
- **Lưu ý phòng vấn**: `dotnet build` **không** build được project vì COM reference ⇒ phải dùng `MSBuild.exe` của Visual Studio (lỗi `MSB4803`). Nói rõ điều này.

---

## 3.7 Quản lý Học viên / Giáo viên / Danh mục khác / Báo cáo — **chưa có UI**

- Không có `frmHocVien`, `frmGiaoVien`, form danh mục hay form báo cáo.
- Nút menu tồn tại nhưng **không có handler** ⇒ bấm không mở gì.
- Dữ liệu vẫn được dùng đầy đủ ở tầng DB: `HocVien` (9), `GiaoVien` (3), `NgheNghiep`, `XepLoai`, `ChucDanh`, `ChuyenMon`, `CoQuan`, `TrinhDo`; `DanhMucKhacRepository` tồn tại; Bulk Import ghi được vào `HocVien`/`GiaoVien`.
- `PhamViDuLieu.ChoHocVien` đã viết sẵn logic lọc theo vai trò cho màn hình học viên (chứng tỏ phần tầng dưới đã sẵn sàng, thiếu phần UI).

**Cách trả lời**: nói thẳng "Đây là phần em ưu tiên cho module khác, hiện trạng là **tầng dữ liệu đã có, tầng giao diện chưa làm**; em đã ưu tiên Bulk Import để vẫn nhập được dữ liệu học viên/giáo viên từ Excel". Không nhận công cho phần chưa làm.

---

## 3.7.1 Quyền của chức năng nhập hàng loạt
Nhập hàng loạt là **chức năng quản trị dữ liệu**, nên học viên **không** được dùng (kể cả khi học viên có quyền `DANGKYLOP` để tự đăng ký lớp):
- **Menu**: `frmMain.ApDungQuyenMenu` chỉ hiện nút khi **không phải học viên** và có `HOCVIEN_THEM`/`GIAOVIEN_THEM`/`DANGKYLOP`.
- **Form**: `FrmBulkImport.CoQuyenLoai` trả `false` với học viên cho mọi loại import.
- **Service (lớp bảo vệ cuối)**: `ImportService.KiemTraQuyen` ném `UnauthorizedAccessException("Học viên không có quyền nhập dữ liệu hàng loạt.")` ⇒ không phụ thuộc UI.

## 3.8 Danh mục phục vụ xếp lịch

`CaHoc(MaCa, TenCa, GioBatDau, GioKetThuc)` — giờ học trong ngày; `PhongHoc(MaPhong, TenPhong, SucChua)` — sức chứa phòng; `ThoiGian(MaThoiGian, TenThoiGian)` + `ThoiGian_Thu(MaThoiGian, Thu)` — mẫu thời gian theo thứ.
Script `Database/ThoiKhoaBieu.sql` bổ sung `SucChua`, `GioBatDau/GioKetThuc` và seed `ThoiGian_Thu` (TG01: T2/T4/T6; TG02: T3/T5/T7; TG03: T7/CN).
**Điểm cần nói**: **chưa có form quản lý** các danh mục này (chỉ có lookup + seed script) ⇒ giáo viên/phòng/ca phải chỉnh bằng SQL hoặc bằng script.

---

## 3.9 Tổng hợp bảng trạng thái nghiệp vụ

| Nghiệp vụ | Trạng thái | Bằng chứng |
|---|---|---|
| Đăng nhập, băm mật khẩu | Completed | `frmDangNhap.cs:36-47`, `MatKhauHelper.cs` |
| Phân quyền + phạm vi dữ liệu | Completed | `PermissionService.cs`, `PhamViDuLieu.cs` |
| Môn học CRUD | Completed | `frmMonHoc`, `MonHocService`, `MonHocRepository` |
| Lớp học CRUD | Completed | `frmLopHoc`, `LopHocService`, `LopHocRepository` |
| Đăng ký / hủy đăng ký | Completed | `frmDangKyLop`, `DangKyRepository` |
| Xếp thời khóa biểu | Completed | `XepLichService`, `ThoiKhoaBieuRepository` |
| Tra cứu + xuất Excel | Completed | `frmTimKiem*`, `ExcelHelper` |
| Học viên / Giáo viên (UI) | **Not Implemented** | không có form, nút không có handler |
| Danh mục khác (UI) | **Not Implemented** | `DanhMucKhacRepository` tồn tại, chưa có UI |
| Báo cáo (UI) | **Not Implemented** | 4 nút không có handler |
| Phòng/Ca/Mẫu thời gian (UI) | **Not Implemented** | chỉ có lookup + script seed |
| Đổi trạng thái lớp (UI) | **Not Implemented** | `LopHoc.TrangThai` không có form sửa; chỉ Automation action gán được |
