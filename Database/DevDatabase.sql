/*
  Database thử nghiệm riêng cho BTL_LTTQ.
  Chỉ tạo database hậu tố _Dev; không tác động database gốc.
  Chạy bằng SSMS trên SQL Server local. Sau đó đăng ký Admin trong ứng dụng.
*/
USE [master];
GO
IF DB_ID(N'QuanLyLopTinHocVaNgoaiNguNganHan_Dev') IS NULL
    CREATE DATABASE [QuanLyLopTinHocVaNgoaiNguNganHan_Dev];
GO
USE [QuanLyLopTinHocVaNgoaiNguNganHan_Dev];
GO

IF OBJECT_ID(N'dbo.MonHoc', N'U') IS NULL
CREATE TABLE dbo.MonHoc (
    MaMon varchar(20) NOT NULL PRIMARY KEY,
    TenMon nvarchar(150) NOT NULL,
    HocPhi decimal(18,2) NULL
);

IF OBJECT_ID(N'dbo.TrinhDo', N'U') IS NULL
CREATE TABLE dbo.TrinhDo (
    MaTrinhDo varchar(20) NOT NULL PRIMARY KEY,
    TenTrinhDo nvarchar(100) NOT NULL
);

IF OBJECT_ID(N'dbo.ChuyenMon', N'U') IS NULL
CREATE TABLE dbo.ChuyenMon (
    MaChuyenMon varchar(20) NOT NULL PRIMARY KEY,
    TenChuyenMon nvarchar(100) NOT NULL
);

IF OBJECT_ID(N'dbo.GiaoVien', N'U') IS NULL
CREATE TABLE dbo.GiaoVien (
    MaGV varchar(20) NOT NULL PRIMARY KEY,
    TenGV nvarchar(150) NOT NULL,
    DienThoai varchar(30) NULL,
    MaTrinhDo varchar(20) NULL REFERENCES dbo.TrinhDo(MaTrinhDo),
    MaChuyenMon varchar(20) NULL REFERENCES dbo.ChuyenMon(MaChuyenMon)
);

IF OBJECT_ID(N'dbo.GiaoVien_MonHoc', N'U') IS NULL
CREATE TABLE dbo.GiaoVien_MonHoc (
    MaGV varchar(20) NOT NULL REFERENCES dbo.GiaoVien(MaGV),
    MaMon varchar(20) NOT NULL REFERENCES dbo.MonHoc(MaMon),
    CONSTRAINT PK_GiaoVien_MonHoc PRIMARY KEY (MaGV, MaMon)
);

IF OBJECT_ID(N'dbo.CaHoc', N'U') IS NULL
CREATE TABLE dbo.CaHoc (
    MaCa varchar(20) NOT NULL PRIMARY KEY,
    TenCa nvarchar(100) NOT NULL
);

IF OBJECT_ID(N'dbo.ThoiGian', N'U') IS NULL
CREATE TABLE dbo.ThoiGian (
    MaThoiGian varchar(20) NOT NULL PRIMARY KEY,
    TenThoiGian nvarchar(100) NOT NULL
);

IF OBJECT_ID(N'dbo.PhongHoc', N'U') IS NULL
CREATE TABLE dbo.PhongHoc (
    MaPhong varchar(20) NOT NULL PRIMARY KEY,
    TenPhong nvarchar(100) NOT NULL
);

IF OBJECT_ID(N'dbo.LopHoc', N'U') IS NULL
CREATE TABLE dbo.LopHoc (
    MaLop varchar(20) NOT NULL PRIMARY KEY,
    TenLop nvarchar(150) NOT NULL,
    MaMon varchar(20) NOT NULL REFERENCES dbo.MonHoc(MaMon),
    MaGV varchar(20) NOT NULL REFERENCES dbo.GiaoVien(MaGV),
    NgayBD date NOT NULL,
    NgayKT date NOT NULL,
    MaCa varchar(20) NOT NULL REFERENCES dbo.CaHoc(MaCa),
    MaThoiGian varchar(20) NOT NULL REFERENCES dbo.ThoiGian(MaThoiGian),
    MaPhong varchar(20) NOT NULL REFERENCES dbo.PhongHoc(MaPhong),
    CONSTRAINT CK_LopHoc_Ngay CHECK (NgayKT >= NgayBD)
);

IF OBJECT_ID(N'dbo.HocVien', N'U') IS NULL
CREATE TABLE dbo.HocVien (
    MaHocVien varchar(20) NOT NULL PRIMARY KEY,
    TenHocVien nvarchar(150) NOT NULL,
    DienThoai varchar(30) NULL,
    MaLop varchar(20) NULL REFERENCES dbo.LopHoc(MaLop)
);

IF OBJECT_ID(N'dbo.TaiKhoan', N'U') IS NULL
CREATE TABLE dbo.TaiKhoan (
    TenDangNhap varchar(50) NOT NULL PRIMARY KEY,
    MatKhauHash varchar(256) NOT NULL,
    Salt varchar(64) NOT NULL,
    VaiTro varchar(20) NOT NULL,
    MaGV varchar(20) NULL REFERENCES dbo.GiaoVien(MaGV),
    MaHocVien varchar(20) NULL REFERENCES dbo.HocVien(MaHocVien),
    TrangThai bit NOT NULL CONSTRAINT DF_TaiKhoan_TrangThai DEFAULT (1),
    CONSTRAINT CK_TaiKhoan_HoSo CHECK (
        (VaiTro = 'Admin' AND MaGV IS NULL AND MaHocVien IS NULL) OR
        (VaiTro = 'GiaoVien' AND MaGV IS NOT NULL AND MaHocVien IS NULL) OR
        (VaiTro = 'HocVien' AND MaGV IS NULL AND MaHocVien IS NOT NULL)
    )
);

IF OBJECT_ID(N'dbo.ChucNang', N'U') IS NULL
CREATE TABLE dbo.ChucNang (
    MaChucNang varchar(50) NOT NULL PRIMARY KEY,
    TenChucNang nvarchar(150) NOT NULL,
    Nhom nvarchar(50) NOT NULL,
    HoTroLocTheoChu bit NOT NULL DEFAULT (0),
    ThuTu int NOT NULL,
    ChiAdmin bit NOT NULL DEFAULT (0)
);

IF OBJECT_ID(N'dbo.PhanQuyen', N'U') IS NULL
CREATE TABLE dbo.PhanQuyen (
    MaChucNang varchar(50) NOT NULL REFERENCES dbo.ChucNang(MaChucNang),
    VaiTro varchar(20) NOT NULL,
    CoQuyen bit NOT NULL DEFAULT (0),
    ChiCuaMinh bit NOT NULL DEFAULT (0),
    CONSTRAINT PK_PhanQuyen PRIMARY KEY (MaChucNang, VaiTro),
    CONSTRAINT CK_PhanQuyen_ChiCuaMinh CHECK (ChiCuaMinh = 0 OR CoQuyen = 1)
);
GO

/* Dữ liệu giả để thử màn hình tra cứu, quản lý và đăng ký tài khoản. */
IF NOT EXISTS (SELECT 1 FROM dbo.MonHoc WHERE MaMon='MH001')
    INSERT dbo.MonHoc VALUES ('MH001', N'Tin học văn phòng', 1500000);
IF NOT EXISTS (SELECT 1 FROM dbo.MonHoc WHERE MaMon='MH002')
    INSERT dbo.MonHoc VALUES ('MH002', N'Tiếng Anh giao tiếp', 1800000);
IF NOT EXISTS (SELECT 1 FROM dbo.TrinhDo WHERE MaTrinhDo='TD01')
    INSERT dbo.TrinhDo VALUES ('TD01', N'Cử nhân');
IF NOT EXISTS (SELECT 1 FROM dbo.ChuyenMon WHERE MaChuyenMon='CM01')
    INSERT dbo.ChuyenMon VALUES ('CM01', N'Tin học');
IF NOT EXISTS (SELECT 1 FROM dbo.ChuyenMon WHERE MaChuyenMon='CM02')
    INSERT dbo.ChuyenMon VALUES ('CM02', N'Ngoại ngữ');
IF NOT EXISTS (SELECT 1 FROM dbo.GiaoVien WHERE MaGV='GV001')
    INSERT dbo.GiaoVien VALUES ('GV001', N'Giáo viên mẫu', '0000000000', 'TD01', 'CM01');
IF NOT EXISTS (SELECT 1 FROM dbo.GiaoVien_MonHoc WHERE MaGV='GV001' AND MaMon='MH001')
    INSERT dbo.GiaoVien_MonHoc VALUES ('GV001', 'MH001');
IF NOT EXISTS (SELECT 1 FROM dbo.CaHoc WHERE MaCa='CA01')
    INSERT dbo.CaHoc VALUES ('CA01', N'Ca sáng');
IF NOT EXISTS (SELECT 1 FROM dbo.ThoiGian WHERE MaThoiGian='TG01')
    INSERT dbo.ThoiGian VALUES ('TG01', N'Thứ 2 - 4 - 6');
IF NOT EXISTS (SELECT 1 FROM dbo.PhongHoc WHERE MaPhong='P001')
    INSERT dbo.PhongHoc VALUES ('P001', N'Phòng 01');
IF NOT EXISTS (SELECT 1 FROM dbo.LopHoc WHERE MaLop='LH001')
    INSERT dbo.LopHoc VALUES ('LH001', N'Lớp mẫu', 'MH001', 'GV001', '2026-01-01', '2027-01-01', 'CA01', 'TG01', 'P001');
IF NOT EXISTS (SELECT 1 FROM dbo.HocVien WHERE MaHocVien='HV001')
    INSERT dbo.HocVien VALUES ('HV001', N'Học viên mẫu', '0000000001', 'LH001');

/* Tài khoản mẫu dùng đúng PBKDF2-SHA256, 600.000 vòng như MatKhauHelper.cs. */
IF NOT EXISTS (SELECT 1 FROM dbo.TaiKhoan WHERE TenDangNhap='admin')
    INSERT dbo.TaiKhoan (TenDangNhap, MatKhauHash, Salt, VaiTro, MaGV, MaHocVien)
    VALUES ('admin', 'BRasfHE/q6vEeb2vX+26cAM5lCCO73TsQa1sp63oTMU=',
            'AQIDBAUGBwgJCgsMDQ4PEA==', 'Admin', NULL, NULL);
IF NOT EXISTS (SELECT 1 FROM dbo.TaiKhoan WHERE TenDangNhap='gv001')
    INSERT dbo.TaiKhoan (TenDangNhap, MatKhauHash, Salt, VaiTro, MaGV, MaHocVien)
    VALUES ('gv001', 'Q0xDjTGfTFwr260Zz5LLNpA43cCDGvYvHldA+e8osCc=',
            'ERITFBUWFxgZGhscHR4fIA==', 'GiaoVien', 'GV001', NULL);
IF NOT EXISTS (SELECT 1 FROM dbo.TaiKhoan WHERE TenDangNhap='hv001')
    INSERT dbo.TaiKhoan (TenDangNhap, MatKhauHash, Salt, VaiTro, MaGV, MaHocVien)
    VALUES ('hv001', 'UqAj9oaMjUt812Y2tEj7RlqSd971oc+VZUc6JF7SlGw=',
            'ISIjJCUmJygpKissLS4vMA==', 'HocVien', NULL, 'HV001');

/* Các mã chức năng được khai báo trong Common/ChucNangConst.cs. */
INSERT dbo.ChucNang (MaChucNang, TenChucNang, Nhom, HoTroLocTheoChu, ThuTu, ChiAdmin)
SELECT s.Ma, s.Ten, s.Nhom, s.Loc, s.ThuTu, s.AdminOnly
FROM (VALUES
 ('LOPHOC_XEM', N'Xem lớp học', N'Quản lý', 1, 1, 0),
 ('LOPHOC_THEM', N'Thêm lớp học', N'Quản lý', 0, 2, 0),
 ('LOPHOC_SUA', N'Sửa lớp học', N'Quản lý', 1, 3, 0),
 ('LOPHOC_XOA', N'Xóa lớp học', N'Quản lý', 0, 4, 0),
 ('HOCVIEN_XEM', N'Xem học viên', N'Quản lý', 1, 5, 0),
 ('HOCVIEN_THEM', N'Thêm học viên', N'Quản lý', 0, 6, 0),
 ('HOCVIEN_SUA', N'Sửa học viên', N'Quản lý', 0, 7, 0),
 ('HOCVIEN_XOA', N'Xóa học viên', N'Quản lý', 0, 8, 0),
 ('GIAOVIEN_XEM', N'Xem giáo viên', N'Quản lý', 0, 9, 0),
 ('GIAOVIEN_THEM', N'Thêm giáo viên', N'Quản lý', 0, 10, 0),
 ('GIAOVIEN_SUA', N'Sửa giáo viên', N'Quản lý', 0, 11, 0),
 ('GIAOVIEN_XOA', N'Xóa giáo viên', N'Quản lý', 0, 12, 0),
 ('MONHOC_XEM', N'Xem môn học', N'Quản lý', 0, 13, 0),
 ('MONHOC_THEM', N'Thêm môn học', N'Quản lý', 0, 14, 0),
 ('MONHOC_SUA', N'Sửa môn học', N'Quản lý', 0, 15, 0),
 ('MONHOC_XOA', N'Xóa môn học', N'Quản lý', 0, 16, 0),
 ('DANHMUC_KHAC', N'Quản lý danh mục khác', N'Quản lý', 0, 17, 0),
 ('TIM_LOP', N'Tìm lớp học', N'Tra cứu', 1, 18, 0),
 ('TIM_GIAOVIEN', N'Tìm giáo viên', N'Tra cứu', 0, 19, 0),
 ('BC_DIEM', N'Báo cáo điểm', N'Báo cáo', 1, 20, 0),
 ('BC_DOANHTHU', N'Báo cáo doanh thu', N'Báo cáo', 0, 21, 0),
 ('BC_XEPHANG', N'Báo cáo xếp hạng', N'Báo cáo', 0, 22, 0),
 ('BC_LOP_PHONG', N'Thống kê lớp theo phòng', N'Báo cáo', 0, 23, 0),
 ('XUAT_EXCEL', N'Xuất Excel', N'Khác', 0, 24, 0),
 ('PHANQUYEN', N'Phân quyền', N'Khác', 0, 25, 1),
 ('TAIKHOAN', N'Quản lý tài khoản', N'Khác', 0, 26, 1)
) s (Ma, Ten, Nhom, Loc, ThuTu, AdminOnly)
WHERE NOT EXISTS (SELECT 1 FROM dbo.ChucNang c WHERE c.MaChucNang=s.Ma);
GO
PRINT N'Database _Dev đã sẵn sàng. Mở ứng dụng và đăng ký tài khoản Admin đầu tiên.';
GO
