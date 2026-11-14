/*
  DESTRUCTIVE DATA RESET: upgrades the simplified _Dev schema if needed, clears its
  rows, then loads sample rows from the provided full SQL export. Does not drop DB/tables.
  Confirm the selected database before running.
*/
USE [QuanLyLopTinHocVaNgoaiNguNganHan_Dev];
GO

/* Add the catalog tables/columns present in the full database export. */
IF OBJECT_ID(N'dbo.ChucDanh', N'U') IS NULL
    CREATE TABLE dbo.ChucDanh (MaChucDanh varchar(20) NOT NULL PRIMARY KEY, TenChucDanh nvarchar(100) NOT NULL);
IF OBJECT_ID(N'dbo.CoQuan', N'U') IS NULL
    CREATE TABLE dbo.CoQuan (MaCQ varchar(20) NOT NULL PRIMARY KEY, TenCQ nvarchar(100) NOT NULL, DiaChi nvarchar(200) NULL, DienThoai varchar(20) NULL);
IF OBJECT_ID(N'dbo.NgheNghiep', N'U') IS NULL
    CREATE TABLE dbo.NgheNghiep (MaNghe varchar(20) NOT NULL PRIMARY KEY, TenNghe nvarchar(100) NOT NULL);
IF OBJECT_ID(N'dbo.XepLoai', N'U') IS NULL
    CREATE TABLE dbo.XepLoai (MaXepLoai varchar(20) NOT NULL PRIMARY KEY, TenLoai nvarchar(50) NOT NULL);

IF COL_LENGTH(N'dbo.GiaoVien', N'NgaySinh') IS NULL ALTER TABLE dbo.GiaoVien ADD NgaySinh date NULL;
IF COL_LENGTH(N'dbo.GiaoVien', N'GioiTinh') IS NULL ALTER TABLE dbo.GiaoVien ADD GioiTinh nvarchar(10) NULL;
IF COL_LENGTH(N'dbo.GiaoVien', N'DiaChi') IS NULL ALTER TABLE dbo.GiaoVien ADD DiaChi nvarchar(200) NULL;
IF COL_LENGTH(N'dbo.GiaoVien', N'MaChucDanh') IS NULL ALTER TABLE dbo.GiaoVien ADD MaChucDanh varchar(20) NULL;
IF COL_LENGTH(N'dbo.GiaoVien', N'MaCQ') IS NULL ALTER TABLE dbo.GiaoVien ADD MaCQ varchar(20) NULL;
IF COL_LENGTH(N'dbo.GiaoVien', N'Anh') IS NULL ALTER TABLE dbo.GiaoVien ADD Anh nvarchar(255) NULL;
IF COL_LENGTH(N'dbo.GiaoVien_MonHoc', N'MaXepLoai') IS NULL ALTER TABLE dbo.GiaoVien_MonHoc ADD MaXepLoai varchar(20) NULL;
IF COL_LENGTH(N'dbo.HocVien', N'NgaySinh') IS NULL ALTER TABLE dbo.HocVien ADD NgaySinh date NULL;
IF COL_LENGTH(N'dbo.HocVien', N'GioiTinh') IS NULL ALTER TABLE dbo.HocVien ADD GioiTinh nvarchar(10) NULL;
IF COL_LENGTH(N'dbo.HocVien', N'DiaChi') IS NULL ALTER TABLE dbo.HocVien ADD DiaChi nvarchar(200) NULL;
IF COL_LENGTH(N'dbo.HocVien', N'MaNghe') IS NULL ALTER TABLE dbo.HocVien ADD MaNghe varchar(20) NULL;
IF COL_LENGTH(N'dbo.HocVien', N'NgayNopHocPhi') IS NULL ALTER TABLE dbo.HocVien ADD NgayNopHocPhi date NULL;
IF COL_LENGTH(N'dbo.HocVien', N'Diem') IS NULL ALTER TABLE dbo.HocVien ADD Diem float NULL;
IF COL_LENGTH(N'dbo.HocVien', N'MaXepLoai') IS NULL ALTER TABLE dbo.HocVien ADD MaXepLoai varchar(20) NULL;
IF COL_LENGTH(N'dbo.LopHoc', N'SiSo') IS NULL ALTER TABLE dbo.LopHoc ADD SiSo int NULL;
IF COL_LENGTH(N'dbo.PhongHoc', N'DiaChi') IS NULL ALTER TABLE dbo.PhongHoc ADD DiaChi nvarchar(200) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name=N'FK_Dev_GiaoVien_ChucDanh')
    ALTER TABLE dbo.GiaoVien WITH CHECK ADD CONSTRAINT FK_Dev_GiaoVien_ChucDanh FOREIGN KEY (MaChucDanh) REFERENCES dbo.ChucDanh(MaChucDanh);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name=N'FK_Dev_GiaoVien_CoQuan')
    ALTER TABLE dbo.GiaoVien WITH CHECK ADD CONSTRAINT FK_Dev_GiaoVien_CoQuan FOREIGN KEY (MaCQ) REFERENCES dbo.CoQuan(MaCQ);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name=N'FK_Dev_GVMH_XepLoai')
    ALTER TABLE dbo.GiaoVien_MonHoc WITH CHECK ADD CONSTRAINT FK_Dev_GVMH_XepLoai FOREIGN KEY (MaXepLoai) REFERENCES dbo.XepLoai(MaXepLoai);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name=N'FK_Dev_HocVien_NgheNghiep')
    ALTER TABLE dbo.HocVien WITH CHECK ADD CONSTRAINT FK_Dev_HocVien_NgheNghiep FOREIGN KEY (MaNghe) REFERENCES dbo.NgheNghiep(MaNghe);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name=N'FK_Dev_HocVien_XepLoai')
    ALTER TABLE dbo.HocVien WITH CHECK ADD CONSTRAINT FK_Dev_HocVien_XepLoai FOREIGN KEY (MaXepLoai) REFERENCES dbo.XepLoai(MaXepLoai);
GO

SET XACT_ABORT ON;
BEGIN TRY
    BEGIN TRANSACTION;

    DELETE FROM dbo.TaiKhoan;
    DELETE FROM dbo.PhanQuyen;
    DELETE FROM dbo.HocVien;
    DELETE FROM dbo.GiaoVien_MonHoc;
    DELETE FROM dbo.LopHoc;
    DELETE FROM dbo.GiaoVien;
    DELETE FROM dbo.MonHoc;
    DELETE FROM dbo.CaHoc;
    DELETE FROM dbo.ChucDanh;
    DELETE FROM dbo.ChuyenMon;
    DELETE FROM dbo.CoQuan;
    DELETE FROM dbo.NgheNghiep;
    DELETE FROM dbo.PhongHoc;
    DELETE FROM dbo.ThoiGian;
    DELETE FROM dbo.TrinhDo;
    DELETE FROM dbo.XepLoai;
    DELETE FROM dbo.ChucNang;

INSERT [dbo].[CaHoc] ([MaCa], [TenCa]) VALUES (N'CA01', N'Ca Sáng (08h00 - 11h00)');
INSERT [dbo].[CaHoc] ([MaCa], [TenCa]) VALUES (N'CA02', N'Ca Chiều (14h00 - 17h00)');
INSERT [dbo].[CaHoc] ([MaCa], [TenCa]) VALUES (N'CA03', N'Ca Tối (18h00 - 21h00)');
INSERT [dbo].[ChucDanh] ([MaChucDanh], [TenChucDanh]) VALUES (N'CD01', N'Giảng viên');
INSERT [dbo].[ChucDanh] ([MaChucDanh], [TenChucDanh]) VALUES (N'CD02', N'Trợ giảng');
INSERT [dbo].[ChucDanh] ([MaChucDanh], [TenChucDanh]) VALUES (N'CD03', N'Trưởng khoa');
INSERT [dbo].[ChucNang] ([MaChucNang], [TenChucNang], [Nhom], [HoTroLocTheoChu], [ChiAdmin], [ThuTu]) VALUES (N'BC_DIEM', N'Báo cáo điểm', N'Báo cáo', 1, 0, 70);
INSERT [dbo].[ChucNang] ([MaChucNang], [TenChucNang], [Nhom], [HoTroLocTheoChu], [ChiAdmin], [ThuTu]) VALUES (N'BC_DOANHTHU', N'Doanh thu', N'Báo cáo', 0, 0, 71);
INSERT [dbo].[ChucNang] ([MaChucNang], [TenChucNang], [Nhom], [HoTroLocTheoChu], [ChiAdmin], [ThuTu]) VALUES (N'BC_LOP_PHONG', N'Lớp học theo phòng', N'Báo cáo', 0, 0, 73);
INSERT [dbo].[ChucNang] ([MaChucNang], [TenChucNang], [Nhom], [HoTroLocTheoChu], [ChiAdmin], [ThuTu]) VALUES (N'BC_XEPHANG', N'Bảng xếp hạng HV', N'Báo cáo', 0, 0, 72);
INSERT [dbo].[ChucNang] ([MaChucNang], [TenChucNang], [Nhom], [HoTroLocTheoChu], [ChiAdmin], [ThuTu]) VALUES (N'DANHMUC_KHAC', N'Danh mục khác', N'Quản lý', 0, 0, 50);
INSERT [dbo].[ChucNang] ([MaChucNang], [TenChucNang], [Nhom], [HoTroLocTheoChu], [ChiAdmin], [ThuTu]) VALUES (N'GIAOVIEN_SUA', N'Giáo viên - Sửa', N'Quản lý', 0, 0, 32);
INSERT [dbo].[ChucNang] ([MaChucNang], [TenChucNang], [Nhom], [HoTroLocTheoChu], [ChiAdmin], [ThuTu]) VALUES (N'GIAOVIEN_THEM', N'Giáo viên - Thêm', N'Quản lý', 0, 0, 31);
INSERT [dbo].[ChucNang] ([MaChucNang], [TenChucNang], [Nhom], [HoTroLocTheoChu], [ChiAdmin], [ThuTu]) VALUES (N'GIAOVIEN_XEM', N'Giáo viên - Xem', N'Quản lý', 0, 0, 30);
INSERT [dbo].[ChucNang] ([MaChucNang], [TenChucNang], [Nhom], [HoTroLocTheoChu], [ChiAdmin], [ThuTu]) VALUES (N'GIAOVIEN_XOA', N'Giáo viên - Xóa', N'Quản lý', 0, 0, 33);
INSERT [dbo].[ChucNang] ([MaChucNang], [TenChucNang], [Nhom], [HoTroLocTheoChu], [ChiAdmin], [ThuTu]) VALUES (N'HOCVIEN_SUA', N'Học viên - Sửa', N'Quản lý', 0, 0, 22);
INSERT [dbo].[ChucNang] ([MaChucNang], [TenChucNang], [Nhom], [HoTroLocTheoChu], [ChiAdmin], [ThuTu]) VALUES (N'HOCVIEN_THEM', N'Học viên - Thêm', N'Quản lý', 0, 0, 21);
INSERT [dbo].[ChucNang] ([MaChucNang], [TenChucNang], [Nhom], [HoTroLocTheoChu], [ChiAdmin], [ThuTu]) VALUES (N'HOCVIEN_XEM', N'Học viên - Xem', N'Quản lý', 1, 0, 20);
INSERT [dbo].[ChucNang] ([MaChucNang], [TenChucNang], [Nhom], [HoTroLocTheoChu], [ChiAdmin], [ThuTu]) VALUES (N'HOCVIEN_XOA', N'Học viên - Xóa', N'Quản lý', 0, 0, 23);
INSERT [dbo].[ChucNang] ([MaChucNang], [TenChucNang], [Nhom], [HoTroLocTheoChu], [ChiAdmin], [ThuTu]) VALUES (N'LOPHOC_SUA', N'Lớp học - Sửa', N'Quản lý', 0, 0, 12);
INSERT [dbo].[ChucNang] ([MaChucNang], [TenChucNang], [Nhom], [HoTroLocTheoChu], [ChiAdmin], [ThuTu]) VALUES (N'LOPHOC_THEM', N'Lớp học - Thêm', N'Quản lý', 0, 0, 11);
INSERT [dbo].[ChucNang] ([MaChucNang], [TenChucNang], [Nhom], [HoTroLocTheoChu], [ChiAdmin], [ThuTu]) VALUES (N'LOPHOC_XEM', N'Lớp học - Xem', N'Quản lý', 1, 0, 10);
INSERT [dbo].[ChucNang] ([MaChucNang], [TenChucNang], [Nhom], [HoTroLocTheoChu], [ChiAdmin], [ThuTu]) VALUES (N'LOPHOC_XOA', N'Lớp học - Xóa', N'Quản lý', 0, 0, 13);
INSERT [dbo].[ChucNang] ([MaChucNang], [TenChucNang], [Nhom], [HoTroLocTheoChu], [ChiAdmin], [ThuTu]) VALUES (N'MONHOC_SUA', N'Môn học - Sửa', N'Quản lý', 0, 0, 42);
INSERT [dbo].[ChucNang] ([MaChucNang], [TenChucNang], [Nhom], [HoTroLocTheoChu], [ChiAdmin], [ThuTu]) VALUES (N'MONHOC_THEM', N'Môn học - Thêm', N'Quản lý', 0, 0, 41);
INSERT [dbo].[ChucNang] ([MaChucNang], [TenChucNang], [Nhom], [HoTroLocTheoChu], [ChiAdmin], [ThuTu]) VALUES (N'MONHOC_XEM', N'Môn học - Xem', N'Quản lý', 0, 0, 40);
INSERT [dbo].[ChucNang] ([MaChucNang], [TenChucNang], [Nhom], [HoTroLocTheoChu], [ChiAdmin], [ThuTu]) VALUES (N'MONHOC_XOA', N'Môn học - Xóa', N'Quản lý', 0, 0, 43);
INSERT [dbo].[ChucNang] ([MaChucNang], [TenChucNang], [Nhom], [HoTroLocTheoChu], [ChiAdmin], [ThuTu]) VALUES (N'PHANQUYEN', N'Phân quyền', N'Hệ thống', 0, 1, 90);
INSERT [dbo].[ChucNang] ([MaChucNang], [TenChucNang], [Nhom], [HoTroLocTheoChu], [ChiAdmin], [ThuTu]) VALUES (N'TAIKHOAN', N'Quản lý tài khoản', N'Hệ thống', 0, 1, 91);
INSERT [dbo].[ChucNang] ([MaChucNang], [TenChucNang], [Nhom], [HoTroLocTheoChu], [ChiAdmin], [ThuTu]) VALUES (N'TIM_GIAOVIEN', N'Tìm giáo viên', N'Tra cứu', 0, 0, 61);
INSERT [dbo].[ChucNang] ([MaChucNang], [TenChucNang], [Nhom], [HoTroLocTheoChu], [ChiAdmin], [ThuTu]) VALUES (N'TIM_LOP', N'Tìm lớp', N'Tra cứu', 1, 0, 60);
INSERT [dbo].[ChucNang] ([MaChucNang], [TenChucNang], [Nhom], [HoTroLocTheoChu], [ChiAdmin], [ThuTu]) VALUES (N'XUAT_EXCEL', N'Xuất file Excel', N'Khác', 0, 0, 80);
INSERT [dbo].[ChuyenMon] ([MaChuyenMon], [TenChuyenMon]) VALUES (N'CM01', N'Công nghệ thông tin');
INSERT [dbo].[ChuyenMon] ([MaChuyenMon], [TenChuyenMon]) VALUES (N'CM02', N'Ngôn ngữ Anh');
INSERT [dbo].[CoQuan] ([MaCQ], [TenCQ], [DiaChi], [DienThoai]) VALUES (N'CQ01', N'Đại học Công Nghệ', N'Hà Nội', N'0243123456');
INSERT [dbo].[CoQuan] ([MaCQ], [TenCQ], [DiaChi], [DienThoai]) VALUES (N'CQ02', N'Trung tâm Ngoại ngữ Tin học', N'Hà Nội', N'0243987654');
INSERT [dbo].[MonHoc] ([MaMon], [TenMon], [HocPhi]) VALUES (N'MH01', N'Tin học văn phòng', CAST(1500000.00 AS Decimal(18, 2)));
INSERT [dbo].[MonHoc] ([MaMon], [TenMon], [HocPhi]) VALUES (N'MH02', N'Lập trình C# WinForms', CAST(2500000.00 AS Decimal(18, 2)));
INSERT [dbo].[MonHoc] ([MaMon], [TenMon], [HocPhi]) VALUES (N'MH03', N'Tiếng Anh Giao Tiếp', CAST(2000000.00 AS Decimal(18, 2)));
INSERT [dbo].[MonHoc] ([MaMon], [TenMon], [HocPhi]) VALUES (N'MH04', N'Lập trình Web ASP.NET', CAST(3000000.00 AS Decimal(18, 2)));
INSERT [dbo].[NgheNghiep] ([MaNghe], [TenNghe]) VALUES (N'NN01', N'Sinh viên');
INSERT [dbo].[NgheNghiep] ([MaNghe], [TenNghe]) VALUES (N'NN02', N'Nhân viên văn phòng');
INSERT [dbo].[NgheNghiep] ([MaNghe], [TenNghe]) VALUES (N'NN03', N'Tự do');
INSERT [dbo].[PhongHoc] ([MaPhong], [TenPhong], [DiaChi]) VALUES (N'PH01', N'Phòng Lab 01', N'Tầng 2 - Tòa A');
INSERT [dbo].[PhongHoc] ([MaPhong], [TenPhong], [DiaChi]) VALUES (N'PH02', N'Phòng Lab 02', N'Tầng 2 - Tòa A');
INSERT [dbo].[PhongHoc] ([MaPhong], [TenPhong], [DiaChi]) VALUES (N'PH03', N'Phòng Lý thuyết 101', N'Tầng 3 - Tòa B');
INSERT [dbo].[ThoiGian] ([MaThoiGian], [TenThoiGian]) VALUES (N'TG01', N'Thứ 2 - 4 - 6');
INSERT [dbo].[ThoiGian] ([MaThoiGian], [TenThoiGian]) VALUES (N'TG02', N'Thứ 3 - 5 - 7');
INSERT [dbo].[ThoiGian] ([MaThoiGian], [TenThoiGian]) VALUES (N'TG03', N'Thứ 7 - Chủ Nhật');
INSERT [dbo].[TrinhDo] ([MaTrinhDo], [TenTrinhDo]) VALUES (N'TD01', N'Cử nhân');
INSERT [dbo].[TrinhDo] ([MaTrinhDo], [TenTrinhDo]) VALUES (N'TD02', N'Thạc sĩ');
INSERT [dbo].[TrinhDo] ([MaTrinhDo], [TenTrinhDo]) VALUES (N'TD03', N'Tiến sĩ');
INSERT [dbo].[XepLoai] ([MaXepLoai], [TenLoai]) VALUES (N'XL01', N'Xuất sắc');
INSERT [dbo].[XepLoai] ([MaXepLoai], [TenLoai]) VALUES (N'XL02', N'Giỏi');
INSERT [dbo].[XepLoai] ([MaXepLoai], [TenLoai]) VALUES (N'XL03', N'Khá');
INSERT [dbo].[XepLoai] ([MaXepLoai], [TenLoai]) VALUES (N'XL04', N'Trung bình');
INSERT [dbo].[XepLoai] ([MaXepLoai], [TenLoai]) VALUES (N'XL05', N'Yếu');
INSERT [dbo].[GiaoVien] ([MaGV], [TenGV], [NgaySinh], [GioiTinh], [DiaChi], [DienThoai], [MaChucDanh], [MaChuyenMon], [MaCQ], [MaTrinhDo], [Anh]) VALUES (N'GV01', N'Nguyễn Văn An', CAST(N'1985-05-20' AS Date), N'Nam', N'Hà Nội', N'0912345678', N'CD01', N'CM01', N'CQ01', N'TD02', NULL);
INSERT [dbo].[GiaoVien] ([MaGV], [TenGV], [NgaySinh], [GioiTinh], [DiaChi], [DienThoai], [MaChucDanh], [MaChuyenMon], [MaCQ], [MaTrinhDo], [Anh]) VALUES (N'GV02', N'Trần Thị Bích', CAST(N'1990-08-15' AS Date), N'Nữ', N'Hà Nội', N'0987654321', N'CD01', N'CM02', N'CQ02', N'TD01', NULL);
INSERT [dbo].[GiaoVien] ([MaGV], [TenGV], [NgaySinh], [GioiTinh], [DiaChi], [DienThoai], [MaChucDanh], [MaChuyenMon], [MaCQ], [MaTrinhDo], [Anh]) VALUES (N'GV03', N'Lê Hoàng Nam', CAST(N'1982-11-10' AS Date), N'Nam', N'Hà Nội', N'0901122334', N'CD03', N'CM01', N'CQ01', N'TD03', NULL);
INSERT [dbo].[LopHoc] ([MaLop], [TenLop], [MaMon], [MaGV], [NgayBD], [NgayKT], [MaCa], [MaThoiGian], [MaPhong], [SiSo]) VALUES (N'L003', N'Lập trình C# cơ bản K03', N'MH01', N'GV03', CAST(N'2000-11-11' AS Date), CAST(N'2027-04-28' AS Date), N'CA01', N'TG02', N'PH01', NULL);
INSERT [dbo].[LopHoc] ([MaLop], [TenLop], [MaMon], [MaGV], [NgayBD], [NgayKT], [MaCa], [MaThoiGian], [MaPhong], [SiSo]) VALUES (N'L01', N'C# WinForms K01', N'MH02', N'GV01', CAST(N'2025-01-10' AS Date), CAST(N'2025-03-10' AS Date), N'CA03', N'TG02', N'PH01', 5);
INSERT [dbo].[LopHoc] ([MaLop], [TenLop], [MaMon], [MaGV], [NgayBD], [NgayKT], [MaCa], [MaThoiGian], [MaPhong], [SiSo]) VALUES (N'L02', N'Tiếng Anh K01', N'MH03', N'GV02', CAST(N'2025-01-15' AS Date), CAST(N'2025-03-15' AS Date), N'CA01', N'TG01', N'PH03', 4);
INSERT [dbo].[GiaoVien_MonHoc] ([MaGV], [MaMon], [MaXepLoai]) VALUES (N'GV01', N'MH01', N'XL01');
INSERT [dbo].[GiaoVien_MonHoc] ([MaGV], [MaMon], [MaXepLoai]) VALUES (N'GV01', N'MH02', N'XL01');
INSERT [dbo].[GiaoVien_MonHoc] ([MaGV], [MaMon], [MaXepLoai]) VALUES (N'GV02', N'MH03', N'XL01');
INSERT [dbo].[GiaoVien_MonHoc] ([MaGV], [MaMon], [MaXepLoai]) VALUES (N'GV03', N'MH02', N'XL01');
INSERT [dbo].[GiaoVien_MonHoc] ([MaGV], [MaMon], [MaXepLoai]) VALUES (N'GV03', N'MH04', N'XL01');
INSERT [dbo].[HocVien] ([MaHocVien], [TenHocVien], [MaLop], [NgaySinh], [GioiTinh], [DiaChi], [MaNghe], [DienThoai], [NgayNopHocPhi], [Diem], [MaXepLoai]) VALUES (N'HV01', N'Nguyễn Văn Cường', N'L01', CAST(N'2002-01-01' AS Date), N'Nam', N'Hà Nội', N'NN01', N'0911111111', CAST(N'2025-01-05' AS Date), 8.5, N'XL02');
INSERT [dbo].[HocVien] ([MaHocVien], [TenHocVien], [MaLop], [NgaySinh], [GioiTinh], [DiaChi], [MaNghe], [DienThoai], [NgayNopHocPhi], [Diem], [MaXepLoai]) VALUES (N'HV02', N'Phạm Thu Hà', N'L01', CAST(N'2003-02-12' AS Date), N'Nữ', N'Hà Nội', N'NN01', N'0922222222', CAST(N'2025-01-06' AS Date), 9.5, N'XL01');
INSERT [dbo].[HocVien] ([MaHocVien], [TenHocVien], [MaLop], [NgaySinh], [GioiTinh], [DiaChi], [MaNghe], [DienThoai], [NgayNopHocPhi], [Diem], [MaXepLoai]) VALUES (N'HV03', N'Đỗ Minh Quân', N'L01', CAST(N'2001-05-20' AS Date), N'Nam', N'Bắc Ninh', N'NN02', N'0933333333', CAST(N'2025-01-07' AS Date), 7, N'XL03');
INSERT [dbo].[HocVien] ([MaHocVien], [TenHocVien], [MaLop], [NgaySinh], [GioiTinh], [DiaChi], [MaNghe], [DienThoai], [NgayNopHocPhi], [Diem], [MaXepLoai]) VALUES (N'HV04', N'Hoàng Thùy Linh', N'L01', CAST(N'2002-09-15' AS Date), N'Nữ', N'Hà Nội', N'NN01', N'0944444444', CAST(N'2025-01-08' AS Date), 9, N'XL02');
INSERT [dbo].[HocVien] ([MaHocVien], [TenHocVien], [MaLop], [NgaySinh], [GioiTinh], [DiaChi], [MaNghe], [DienThoai], [NgayNopHocPhi], [Diem], [MaXepLoai]) VALUES (N'HV05', N'Vũ Quốc Anh', N'L01', CAST(N'2000-12-01' AS Date), N'Nam', N'Hải Phòng', N'NN03', N'0955555555', CAST(N'2025-01-09' AS Date), 6.5, N'XL04');
INSERT [dbo].[HocVien] ([MaHocVien], [TenHocVien], [MaLop], [NgaySinh], [GioiTinh], [DiaChi], [MaNghe], [DienThoai], [NgayNopHocPhi], [Diem], [MaXepLoai]) VALUES (N'HV06', N'Trần Minh Đức', N'L02', CAST(N'2003-03-10' AS Date), N'Nam', N'Hà Nội', N'NN01', N'0966666666', CAST(N'2025-01-10' AS Date), 9.8, N'XL01');
INSERT [dbo].[HocVien] ([MaHocVien], [TenHocVien], [MaLop], [NgaySinh], [GioiTinh], [DiaChi], [MaNghe], [DienThoai], [NgayNopHocPhi], [Diem], [MaXepLoai]) VALUES (N'HV07', N'Nguyễn Phương Anh', N'L02', CAST(N'2002-07-22' AS Date), N'Nữ', N'Hà Nội', N'NN01', N'0977777777', CAST(N'2025-01-11' AS Date), 8, N'XL02');
INSERT [dbo].[HocVien] ([MaHocVien], [TenHocVien], [MaLop], [NgaySinh], [GioiTinh], [DiaChi], [MaNghe], [DienThoai], [NgayNopHocPhi], [Diem], [MaXepLoai]) VALUES (N'HV08', N'Lê Tuấn Kiệt', N'L02', CAST(N'2001-10-05' AS Date), N'Nam', N'Hà Nội', N'NN02', N'0988888888', CAST(N'2025-01-12' AS Date), 8.8, N'XL02');
INSERT [dbo].[HocVien] ([MaHocVien], [TenHocVien], [MaLop], [NgaySinh], [GioiTinh], [DiaChi], [MaNghe], [DienThoai], [NgayNopHocPhi], [Diem], [MaXepLoai]) VALUES (N'HV09', N'Đặng Mai Phương', N'L02', CAST(N'2003-11-11' AS Date), N'Nữ', N'Hà Nội', N'NN01', N'0999999999', CAST(N'2025-01-13' AS Date), 7.5, N'XL03');
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'BC_DIEM', N'GiaoVien', 1, 1);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'BC_DIEM', N'HocVien', 1, 1);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'BC_DOANHTHU', N'GiaoVien', 0, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'BC_DOANHTHU', N'HocVien', 0, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'BC_LOP_PHONG', N'GiaoVien', 1, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'BC_LOP_PHONG', N'HocVien', 0, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'BC_XEPHANG', N'GiaoVien', 1, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'BC_XEPHANG', N'HocVien', 0, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'DANHMUC_KHAC', N'GiaoVien', 0, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'DANHMUC_KHAC', N'HocVien', 0, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'GIAOVIEN_SUA', N'GiaoVien', 0, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'GIAOVIEN_SUA', N'HocVien', 0, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'GIAOVIEN_THEM', N'GiaoVien', 0, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'GIAOVIEN_THEM', N'HocVien', 0, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'GIAOVIEN_XEM', N'GiaoVien', 1, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'GIAOVIEN_XEM', N'HocVien', 0, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'GIAOVIEN_XOA', N'GiaoVien', 0, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'GIAOVIEN_XOA', N'HocVien', 0, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'HOCVIEN_SUA', N'GiaoVien', 0, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'HOCVIEN_SUA', N'HocVien', 0, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'HOCVIEN_THEM', N'GiaoVien', 0, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'HOCVIEN_THEM', N'HocVien', 0, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'HOCVIEN_XEM', N'GiaoVien', 1, 1);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'HOCVIEN_XEM', N'HocVien', 1, 1);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'HOCVIEN_XOA', N'GiaoVien', 0, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'HOCVIEN_XOA', N'HocVien', 0, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'LOPHOC_SUA', N'GiaoVien', 0, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'LOPHOC_SUA', N'HocVien', 0, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'LOPHOC_THEM', N'GiaoVien', 0, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'LOPHOC_THEM', N'HocVien', 0, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'LOPHOC_XEM', N'GiaoVien', 1, 1);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'LOPHOC_XEM', N'HocVien', 1, 1);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'LOPHOC_XOA', N'GiaoVien', 0, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'LOPHOC_XOA', N'HocVien', 0, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'MONHOC_SUA', N'GiaoVien', 0, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'MONHOC_SUA', N'HocVien', 0, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'MONHOC_THEM', N'GiaoVien', 0, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'MONHOC_THEM', N'HocVien', 0, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'MONHOC_XEM', N'GiaoVien', 1, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'MONHOC_XEM', N'HocVien', 0, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'MONHOC_XOA', N'GiaoVien', 0, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'MONHOC_XOA', N'HocVien', 0, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'TIM_GIAOVIEN', N'GiaoVien', 1, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'TIM_GIAOVIEN', N'HocVien', 0, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'TIM_LOP', N'GiaoVien', 1, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'TIM_LOP', N'HocVien', 0, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'XUAT_EXCEL', N'GiaoVien', 1, 0);
INSERT [dbo].[PhanQuyen] ([MaChucNang], [VaiTro], [CoQuyen], [ChiCuaMinh]) VALUES (N'XUAT_EXCEL', N'HocVien', 0, 0);
INSERT [dbo].[TaiKhoan] ([TenDangNhap], [MatKhauHash], [Salt], [VaiTro], [MaGV], [MaHocVien], [TrangThai]) VALUES (N'admin', N'BRasfHE/q6vEeb2vX+26cAM5lCCO73TsQa1sp63oTMU=', N'AQIDBAUGBwgJCgsMDQ4PEA==', N'Admin', NULL, NULL, 1);

    COMMIT TRANSACTION;
    PRINT N'Dữ liệu mẫu đã được nạp lại thành công.';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
