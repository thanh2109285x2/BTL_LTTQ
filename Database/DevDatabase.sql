/*  Database thử nghiệm riêng cho BTL_LTTQ.  Chỉ tạo database hậu tố _Dev; không tác động database gốc.  Chạy bằng SSMS trên SQL Server local. Sau đó đăng ký Admin trong ứng dụng.*/
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
    MaLop varchar(20) NULL REFERENCES dbo.LopHoc(MaLop),
    Diem float NULL
);
IF COL_LENGTH(N'dbo.HocVien', N'Diem') IS NULL
    ALTER TABLE dbo.HocVien ADD Diem float NULL;

-- Bảng quản lý phiếu thu học phí mới
IF OBJECT_ID(N'dbo.PhieuThu', N'U') IS NULL
CREATE TABLE dbo.PhieuThu (
    MaPhieu int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    MaHocVien varchar(20) NOT NULL REFERENCES dbo.HocVien(MaHocVien),
    NgayLap datetime NOT NULL CONSTRAINT DF_PhieuThu_NgayLap DEFAULT (GETDATE()),
    TongTien decimal(18,0) NOT NULL,
    TrangThai int NOT NULL CONSTRAINT DF_PhieuThu_TrangThai DEFAULT (0), -- 0: Chưa thu, 1: Đã thu
    NgayThu datetime NULL,
    NguoiThu varchar(50) NULL,
    NoiDungChuyenKhoan nvarchar(100) NULL,
    MaGiaoDichNganHang varchar(100) NULL
);

-- Bảng XepLoai nếu cần cho các ràng buộc điểm/xếp loại
IF OBJECT_ID(N'dbo.XepLoai', N'U') IS NULL
CREATE TABLE dbo.XepLoai (
    MaXepLoai varchar(20) NOT NULL PRIMARY KEY,
    TenXepLoai nvarchar(50) NOT NULL
);

-- Bảng DangKy lưu thông tin đăng ký học và liên kết phiếu thu, điểm, xếp loại
IF OBJECT_ID(N'dbo.DangKy', N'U') IS NULL
CREATE TABLE dbo.DangKy (
    MaHocVien varchar(20) NOT NULL REFERENCES dbo.HocVien(MaHocVien),
    MaLop varchar(20) NOT NULL REFERENCES dbo.LopHoc(MaLop),
    NgayDangKy datetime NOT NULL CONSTRAINT DF_DangKy_NgayDangKy DEFAULT (GETDATE()),
    MaPhieu int NULL,
    Diem float NULL,
    MaXepLoai varchar(20) NULL,
    CONSTRAINT PK_DangKy PRIMARY KEY (MaHocVien, MaLop)
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
    CONSTRAINT CK_TaiKhoan_HoSo CHECK (        (VaiTro = 'Admin' AND MaGV IS NULL AND MaHocVien IS NULL) OR        (VaiTro = 'GiaoVien' AND MaGV IS NOT NULL AND MaHocVien IS NULL) OR        (VaiTro = 'HocVien' AND MaGV IS NULL AND MaHocVien IS NOT NULL)
    ));

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
    INSERT dbo.HocVien (MaHocVien, TenHocVien, DienThoai, MaLop, Diem)
    VALUES ('HV001', N'Học viên mẫu', '0000000001', 'LH001', 9.0);

-- Thêm dữ liệu mẫu vào bảng DangKy dựa trên cột MaLop cũ của HocVien nếu chưa có
IF NOT EXISTS (SELECT 1 FROM dbo.DangKy WHERE MaHocVien='HV001' AND MaLop='LH001')
    INSERT INTO dbo.DangKy (MaHocVien, MaLop)
    SELECT MaHocVien, MaLop FROM dbo.HocVien WHERE MaLop IS NOT NULL;

/* ---------- MIGRATION DỮ LIỆU CŨ SANG DANGKY / PHIEUTHU ---------- */
BEGIN TRAN;

DECLARE @map TABLE (MaPhieu int, MaHocVien varchar(20), MaLop varchar(20));

MERGE dbo.PhieuThu AS t 
USING (
    SELECT hv.MaHocVien, hv.MaLop,
           CAST(ISNULL(hv.NgayNopHocPhi, GETDATE()) AS datetime) AS NgayNop,
           CAST(ISNULL(m.HocPhi, 0) AS decimal(18,0)) AS TongTien
    FROM dbo.HocVien hv
    JOIN dbo.DangKy dk ON dk.MaHocVien = hv.MaHocVien AND dk.MaLop = hv.MaLop AND dk.MaPhieu IS NULL
    JOIN dbo.LopHoc l ON l.MaLop = hv.MaLop
    LEFT JOIN dbo.MonHoc m ON m.MaMon = l.MaMon
    WHERE hv.NgayNopHocPhi IS NOT NULL
) AS s 
ON 1 = 0
WHEN NOT MATCHED THEN
    INSERT (MaHocVien, NgayLap, TongTien, TrangThai, NgayThu, NguoiThu)
    VALUES (s.MaHocVien, s.NgayNop, s.TongTien, 1, s.NgayNop, 'migrate')
OUTPUT inserted.MaPhieu, s.MaHocVien, s.MaLop INTO @map;

UPDATE dk
SET dk.MaPhieu = x.MaPhieu
FROM dbo.DangKy dk
JOIN @map x ON x.MaHocVien = dk.MaHocVien AND x.MaLop = dk.MaLop;

COMMIT TRAN;
PRINT N'  + Đã chuyển dữ liệu cũ sang DangKy/PhieuThu';
GO

/* ---------- 4. RÀNG BUỘC ---------- */
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_DangKy_XepLoai')
    ALTER TABLE dbo.DangKy ADD CONSTRAINT FK_DangKy_XepLoai
        FOREIGN KEY (MaXepLoai) REFERENCES dbo.XepLoai(MaXepLoai);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_DangKy_PhieuThu')
    ALTER TABLE dbo.DangKy ADD CONSTRAINT FK_DangKy_PhieuThu
        FOREIGN KEY (MaPhieu) REFERENCES dbo.PhieuThu(MaPhieu);

-- Thang điểm 10. Nếu lệnh này báo lỗi thì dữ liệu cũ có điểm ngoài 0..10, hãy kiểm tra DangKy.Diem
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_DangKy_Diem')
    ALTER TABLE dbo.DangKy ADD CONSTRAINT CK_DangKy_Diem
        CHECK (Diem IS NULL OR (Diem >= 0 AND Diem <= 10));
GO

/* ---------- 5. CHỨC NĂNG MỚI (nhóm Nghiệp vụ) ---------- */
-- Tên nhóm theo đúng kiểu đang dùng trong bảng: có dấu hay không dấu
DECLARE @nv nvarchar(50) =
    CASE WHEN EXISTS (SELECT 1 FROM dbo.ChucNang WHERE Nhom = N'QuanLy')
         THEN N'NghiepVu' ELSE N'Nghiệp vụ' END;

MERGE dbo.ChucNang AS t 
USING (VALUES    
    ('DANGKYLOP',        N'Đăng ký lớp',                                  @nv, 0, 28),    
    ('DONGHOCPHI',       N'Đóng học phí (lập phiếu, mã QR)',              @nv, 1, 29),    
    ('HOCPHI_XACNHAN',   N'Đóng học phí - Xác nhận đã thu',               @nv, 0, 30),    
    ('HOCVIEN_NHAPDIEM', N'Nhập điểm',                                    @nv, 1, 31)
) AS s(MaChucNang, TenChucNang, Nhom, HoTroLocTheoChu, ThuTu)
ON t.MaChucNang = s.MaChucNang
WHEN MATCHED THEN
    UPDATE SET TenChucNang = s.TenChucNang, Nhom = s.Nhom,
               HoTroLocTheoChu = s.HoTroLocTheoChu, ThuTu = s.ThuTu
WHEN NOT MATCHED BY TARGET THEN
    INSERT (MaChucNang, TenChucNang, Nhom, HoTroLocTheoChu, ThuTu)
    VALUES (s.MaChucNang, s.TenChucNang, s.Nhom, s.HoTroLocTheoChu, s.ThuTu);

-- Quyền mặc định (chỉ thêm dòng còn thiếu, không ghi đè quyền Admin đã chỉnh)
INSERT INTO dbo.PhanQuyen (MaChucNang, VaiTro, CoQuyen, ChiCuaMinh)
SELECT v.MaChucNang, v.VaiTro, v.CoQuyen, v.ChiCuaMinh
FROM (VALUES    
    ('DONGHOCPHI',       'HocVien',  1, 1),   -- học viên chỉ lập phiếu và xem phiếu của mình    
    ('DONGHOCPHI',       'GiaoVien', 0, 0),    
    ('HOCPHI_XACNHAN',   'HocVien',  0, 0),   -- chỉ Admin xác nhận đã thu    
    ('HOCPHI_XACNHAN',   'GiaoVien', 0, 0),    
    ('HOCVIEN_NHAPDIEM', 'GiaoVien', 1, 1),   -- giáo viên nhập điểm lớp mình dạy    
    ('HOCVIEN_NHAPDIEM', 'HocVien',  0, 0)
) AS v(MaChucNang, VaiTro, CoQuyen, ChiCuaMinh)
WHERE NOT EXISTS (SELECT 1 FROM dbo.PhanQuyen p
                  WHERE p.MaChucNang = v.MaChucNang AND p.VaiTro = v.VaiTro);
GO

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

/* Các mã chức năng cũ được khai báo trong Common/ChucNangConst.cs. */
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
    ('TIM_HOCVIEN', N'Tìm học viên', N'Tra cứu', 0, 27, 0), 
    ('BC_DIEM', N'Báo cáo điểm', N'Báo cáo', 1, 20, 0), 
    ('BC_DOANHTHU', N'Báo cáo doanh thu', N'Báo cáo', 0, 21, 0), 
    ('BC_XEPHANG', N'Báo cáo xếp hạng', N'Báo cáo', 0, 22, 0), 
    ('BC_LOP_PHONG', N'Thống kê lớp theo phòng', N'Báo cáo', 0, 23, 0), 
    ('XUAT_EXCEL', N'Xuất Excel', N'Khác', 0, 24, 0), 
    ('IMPORT_DULIEU', N'Thêm dữ liệu', N'Khác', 0, 27, 0),
    ('DATABASE_BACKUP', N'Backup database', N'Khác', 0, 28, 0),
    ('PHANQUYEN', N'Phân quyền', N'Khác', 0, 25, 1), 
    ('TAIKHOAN', N'Quản lý tài khoản', N'Khác', 0, 26, 1)
) s (Ma, Ten, Nhom, Loc, ThuTu, AdminOnly)
WHERE NOT EXISTS (SELECT 1 FROM dbo.ChucNang c WHERE c.MaChucNang=s.Ma);
UPDATE dbo.ChucNang SET HoTroLocTheoChu = 1 WHERE MaChucNang IN ('BC_XEPHANG', 'BC_LOP_PHONG');

/* Quyền tra cứu AI mặc định. Doanh thu mặc định chỉ Admin; không cấp cho giáo viên/học viên. */
INSERT dbo.PhanQuyen (MaChucNang, VaiTro, CoQuyen, ChiCuaMinh)
SELECT p.MaChucNang, p.VaiTro, p.CoQuyen, p.ChiCuaMinh
FROM (VALUES
    ('TIM_LOP', 'GiaoVien', 1, 0),
    ('TIM_LOP', 'HocVien', 0, 0),
    ('TIM_GIAOVIEN', 'GiaoVien', 1, 0),
    ('TIM_GIAOVIEN', 'HocVien', 0, 0),
    ('BC_DIEM', 'GiaoVien', 1, 1),
    ('BC_DIEM', 'HocVien', 1, 1),
    ('BC_DOANHTHU', 'GiaoVien', 0, 0),
    ('BC_DOANHTHU', 'HocVien', 0, 0),
    ('BC_XEPHANG', 'GiaoVien', 1, 0),
    ('BC_XEPHANG', 'HocVien', 0, 0),
    ('BC_LOP_PHONG', 'GiaoVien', 1, 0),
    ('BC_LOP_PHONG', 'HocVien', 0, 0),
    ('IMPORT_DULIEU', 'GiaoVien', 0, 0),
    ('IMPORT_DULIEU', 'HocVien', 0, 0),
    ('DATABASE_BACKUP', 'GiaoVien', 0, 0),
    ('DATABASE_BACKUP', 'HocVien', 0, 0)
) p (MaChucNang, VaiTro, CoQuyen, ChiCuaMinh)
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.PhanQuyen q
    WHERE q.MaChucNang = p.MaChucNang AND q.VaiTro = p.VaiTro
);
GO

PRINT N'Database _Dev đã sẵn sàng. Mở ứng dụng và đăng ký tài khoản Admin đầu tiên.';
GO
