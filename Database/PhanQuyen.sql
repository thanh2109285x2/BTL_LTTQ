/* =============================================================
   PHÂN QUYỀN HỆ THỐNG - TẠO SCHEMA + DỮ LIỆU MẶC ĐỊNH
   DB     : QuanLyLopTinHocVaNgoaiNguNganHan
   Chạy   : sqlcmd -S "localhost\SQLEXPRESS" -d "QuanLyLopTinHocVaNgoaiNguNganHan" -E -C -f 65001 -i Database\PhanQuyen.sql

   Tài khoản mặc định sau khi chạy:
     admin / Admin@123   (Admin)
     gv01  / Demo@123    (Giáo viên - gắn hồ sơ GV01)
     hv01  / Demo@123    (Học viên  - gắn hồ sơ HV01)
   ============================================================= */

SET NOCOUNT ON;
SET XACT_ABORT ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
SET ANSI_PADDING ON;
SET ANSI_WARNINGS ON;
SET ARITHABORT ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET NUMERIC_ROUNDABORT OFF;
GO

/* ---------- 1. Bảng ChucNang ---------- */
IF OBJECT_ID(N'dbo.ChucNang', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ChucNang
    (
        MaChucNang      varchar(50)   NOT NULL,
        TenChucNang     nvarchar(200) NOT NULL,
        Nhom            nvarchar(50)  NOT NULL CONSTRAINT DF_ChucNang_Nhom DEFAULT (N'Khác'),
        ChiAdmin        bit           NOT NULL CONSTRAINT DF_ChucNang_ChiAdmin DEFAULT (0),
        HoTroLocTheoChu bit           NOT NULL CONSTRAINT DF_ChucNang_HoTroLocTheoChu DEFAULT (0),
        ThuTu           int           NOT NULL CONSTRAINT DF_ChucNang_ThuTu DEFAULT (0),
        CONSTRAINT PK_ChucNang PRIMARY KEY (MaChucNang)
    );
END
GO

/* ---------- 2. Bảng PhanQuyen ---------- */
IF OBJECT_ID(N'dbo.PhanQuyen', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.PhanQuyen
    (
        MaChucNang varchar(50) NOT NULL,
        VaiTro     varchar(20) NOT NULL,
        CoQuyen    bit         NOT NULL CONSTRAINT DF_PhanQuyen_CoQuyen DEFAULT (0),
        ChiCuaMinh bit         NOT NULL CONSTRAINT DF_PhanQuyen_ChiCuaMinh DEFAULT (0),
        CONSTRAINT PK_PhanQuyen PRIMARY KEY (MaChucNang, VaiTro),
        CONSTRAINT FK_PhanQuyen_ChucNang FOREIGN KEY (MaChucNang)
            REFERENCES dbo.ChucNang (MaChucNang) ON DELETE CASCADE,
        CONSTRAINT CK_PhanQuyen_VaiTro CHECK (VaiTro IN ('GiaoVien', 'HocVien')),
        CONSTRAINT CK_PhanQuyen_ChiCuaMinh CHECK (ChiCuaMinh = 0 OR CoQuyen = 1)
    );
END
GO

/* ---------- 3. Bảng TaiKhoan ---------- */
IF OBJECT_ID(N'dbo.TaiKhoan', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.TaiKhoan
    (
        TenDangNhap varchar(50)  NOT NULL,
        MatKhauHash varchar(256) NOT NULL,
        Salt        varchar(64)  NOT NULL,
        VaiTro      varchar(20)  NOT NULL,
        MaGV        varchar(20)  NULL,
        MaHocVien   varchar(20)  NULL,
        TrangThai   bit          NOT NULL CONSTRAINT DF_TaiKhoan_TrangThai DEFAULT (1),
        CONSTRAINT PK_TaiKhoan PRIMARY KEY (TenDangNhap),
        CONSTRAINT CK_TaiKhoan_VaiTro CHECK (VaiTro IN ('Admin', 'GiaoVien', 'HocVien')),
        CONSTRAINT FK_TaiKhoan_GiaoVien FOREIGN KEY (MaGV) REFERENCES dbo.GiaoVien (MaGV),
        CONSTRAINT FK_TaiKhoan_HocVien FOREIGN KEY (MaHocVien) REFERENCES dbo.HocVien (MaHocVien),
        CONSTRAINT CK_TaiKhoan_HoSo CHECK
        (
            (VaiTro = 'Admin'    AND MaGV IS NULL     AND MaHocVien IS NULL) OR
            (VaiTro = 'GiaoVien' AND MaGV IS NOT NULL AND MaHocVien IS NULL) OR
            (VaiTro = 'HocVien'  AND MaHocVien IS NOT NULL AND MaGV IS NULL)
        )
    );
END
GO

/* ---------- 4. Index duy nhất ---------- */
IF NOT EXISTS (SELECT 1 FROM sys.indexes
               WHERE object_id = OBJECT_ID(N'dbo.TaiKhoan') AND name = N'UX_TaiKhoan_MaGV')
    CREATE UNIQUE INDEX UX_TaiKhoan_MaGV ON dbo.TaiKhoan (MaGV) WHERE MaGV IS NOT NULL;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes
               WHERE object_id = OBJECT_ID(N'dbo.TaiKhoan') AND name = N'UX_TaiKhoan_MaHocVien')
    CREATE UNIQUE INDEX UX_TaiKhoan_MaHocVien ON dbo.TaiKhoan (MaHocVien) WHERE MaHocVien IS NOT NULL;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes
               WHERE object_id = OBJECT_ID(N'dbo.TaiKhoan') AND name = N'UX_TaiKhoan_Admin')
    CREATE UNIQUE INDEX UX_TaiKhoan_Admin ON dbo.TaiKhoan (VaiTro) WHERE VaiTro = 'Admin';
GO

/* ---------- 5. Seed danh mục chức năng ---------- */
MERGE dbo.ChucNang AS target
USING (VALUES
    ('LOPHOC_XEM',      N'Xem lớp học',              N'Quản lý', 0, 1,  1),
    ('LOPHOC_THEM',     N'Thêm lớp học',             N'Quản lý', 0, 0,  2),
    ('LOPHOC_SUA',      N'Sửa lớp học',              N'Quản lý', 0, 1,  3),
    ('LOPHOC_XOA',      N'Xóa lớp học',              N'Quản lý', 0, 0,  4),
    ('HOCVIEN_XEM',     N'Xem học viên',             N'Quản lý', 0, 1,  5),
    ('HOCVIEN_THEM',    N'Thêm học viên',            N'Quản lý', 0, 0,  6),
    ('HOCVIEN_SUA',     N'Sửa học viên',             N'Quản lý', 0, 0,  7),
    ('HOCVIEN_XOA',     N'Xóa học viên',             N'Quản lý', 0, 0,  8),
    ('GIAOVIEN_XEM',    N'Xem giáo viên',            N'Quản lý', 0, 0,  9),
    ('GIAOVIEN_THEM',   N'Thêm giáo viên',           N'Quản lý', 0, 0, 10),
    ('GIAOVIEN_SUA',    N'Sửa giáo viên',            N'Quản lý', 0, 0, 11),
    ('GIAOVIEN_XOA',    N'Xóa giáo viên',            N'Quản lý', 0, 0, 12),
    ('MONHOC_XEM',      N'Xem môn học',              N'Quản lý', 0, 0, 13),
    ('MONHOC_THEM',     N'Thêm môn học',             N'Quản lý', 0, 0, 14),
    ('MONHOC_SUA',      N'Sửa môn học',              N'Quản lý', 0, 0, 15),
    ('MONHOC_XOA',      N'Xóa môn học',              N'Quản lý', 0, 0, 16),
    ('TIM_LOP',         N'Tra cứu lớp học',          N'Tra cứu', 0, 1, 17),
    ('TIM_GIAOVIEN',    N'Tra cứu giáo viên',        N'Tra cứu', 0, 0, 18),
    ('BC_DIEM',         N'Báo cáo điểm',             N'Báo cáo', 0, 1, 19),
    ('BC_DOANHTHU',     N'Báo cáo doanh thu',        N'Báo cáo', 0, 0, 20),
    ('BC_XEPHANG',      N'Báo cáo xếp hạng',         N'Báo cáo', 0, 0, 21),
    ('BC_LOP_PHONG',    N'Báo cáo lớp theo phòng',   N'Báo cáo', 0, 0, 22),
    ('DANHMUC_KHAC',    N'Danh mục khác',            N'Khác',    0, 0, 23),
    ('XUAT_EXCEL',      N'Xuất Excel',               N'Khác',    0, 0, 24),
    ('PHANQUYEN',       N'Phân quyền',               N'Hệ thống',1, 0, 25),
    ('TAIKHOAN',        N'Quản lý tài khoản',        N'Hệ thống',1, 0, 26),
    ('THOIKHOA_XEP',    N'Tạo thời khóa biểu',      N'Hệ thống',1, 0, 27),
    ('AUTOMATION_XEM',  N'Xem tự động hóa',          N'Hệ thống',0, 0, 28),
    ('AUTOMATION_QUANLY',N'Quản lý quy tắc tự động',N'Hệ thống',1, 0, 29),
    ('AUTOMATION_TASK', N'Xử lý nhiệm vụ tự động',   N'Hệ thống',0, 0, 30)
) AS source (MaChucNang, TenChucNang, Nhom, ChiAdmin, HoTroLocTheoChu, ThuTu)
ON target.MaChucNang = source.MaChucNang
WHEN MATCHED THEN
    UPDATE SET TenChucNang      = source.TenChucNang,
               Nhom             = source.Nhom,
               ChiAdmin         = source.ChiAdmin,
               HoTroLocTheoChu  = source.HoTroLocTheoChu,
               ThuTu            = source.ThuTu
WHEN NOT MATCHED THEN
    INSERT (MaChucNang, TenChucNang, Nhom, ChiAdmin, HoTroLocTheoChu, ThuTu)
    VALUES (source.MaChucNang, source.TenChucNang, source.Nhom,
            source.ChiAdmin, source.HoTroLocTheoChu, source.ThuTu);
GO

/* ---------- 6. Seed quyền mặc định cho Giáo viên / Học viên ----------
   Cột: MaChucNang, GV_CoQuyen, GV_ChiCuaMinh, HV_CoQuyen, HV_ChiCuaMinh
   Chỉ chèn dòng còn thiếu, không ghi đè quyền admin đã chỉnh sau này. */
INSERT INTO dbo.PhanQuyen (MaChucNang, VaiTro, CoQuyen, ChiCuaMinh)
SELECT m.MaChucNang, 'GiaoVien', m.GV_CoQuyen, m.GV_ChiCuaMinh
FROM (VALUES
    ('LOPHOC_XEM',    1, 1, 1, 1),
    ('LOPHOC_THEM',   0, 0, 0, 0),
    ('LOPHOC_SUA',    1, 1, 0, 0),
    ('LOPHOC_XOA',    0, 0, 0, 0),
    ('HOCVIEN_XEM',   1, 1, 1, 1),
    ('HOCVIEN_THEM',  0, 0, 0, 0),
    ('HOCVIEN_SUA',   0, 0, 0, 0),
    ('HOCVIEN_XOA',   0, 0, 0, 0),
    ('GIAOVIEN_XEM',  0, 0, 0, 0),
    ('GIAOVIEN_THEM', 0, 0, 0, 0),
    ('GIAOVIEN_SUA',  0, 0, 0, 0),
    ('GIAOVIEN_XOA',  0, 0, 0, 0),
    ('MONHOC_XEM',    1, 0, 1, 0),
    ('MONHOC_THEM',   0, 0, 0, 0),
    ('MONHOC_SUA',    0, 0, 0, 0),
    ('MONHOC_XOA',    0, 0, 0, 0),
    ('TIM_LOP',       1, 1, 1, 1),
    ('TIM_GIAOVIEN',  1, 0, 0, 0),
    ('BC_DIEM',       1, 1, 1, 1),
    ('BC_DOANHTHU',   0, 0, 0, 0),
    ('BC_XEPHANG',    0, 0, 0, 0),
    ('BC_LOP_PHONG',  0, 0, 0, 0),
    ('DANHMUC_KHAC',  0, 0, 0, 0),
    ('XUAT_EXCEL',    1, 0, 1, 0),
    ('DANGKYLOP',     0, 0, 1, 1),
    ('THOIKHOA_XEM',  1, 1, 1, 1),
    ('AUTOMATION_XEM',1, 0, 0, 0),
    ('AUTOMATION_TASK',1, 0, 0, 0)
) AS m (MaChucNang, GV_CoQuyen, GV_ChiCuaMinh, HV_CoQuyen, HV_ChiCuaMinh)
WHERE NOT EXISTS
(
    SELECT 1 FROM dbo.PhanQuyen p
    WHERE p.MaChucNang = m.MaChucNang AND p.VaiTro = 'GiaoVien'
);

INSERT INTO dbo.PhanQuyen (MaChucNang, VaiTro, CoQuyen, ChiCuaMinh)
SELECT m.MaChucNang, 'HocVien', m.HV_CoQuyen, m.HV_ChiCuaMinh
FROM (VALUES
    ('LOPHOC_XEM',    1, 1, 1, 1),
    ('LOPHOC_THEM',   0, 0, 0, 0),
    ('LOPHOC_SUA',    1, 1, 0, 0),
    ('LOPHOC_XOA',    0, 0, 0, 0),
    ('HOCVIEN_XEM',   1, 1, 1, 1),
    ('HOCVIEN_THEM',  0, 0, 0, 0),
    ('HOCVIEN_SUA',   0, 0, 0, 0),
    ('HOCVIEN_XOA',   0, 0, 0, 0),
    ('GIAOVIEN_XEM',  0, 0, 0, 0),
    ('GIAOVIEN_THEM', 0, 0, 0, 0),
    ('GIAOVIEN_SUA',  0, 0, 0, 0),
    ('GIAOVIEN_XOA',  0, 0, 0, 0),
    ('MONHOC_XEM',    1, 0, 1, 0),
    ('MONHOC_THEM',   0, 0, 0, 0),
    ('MONHOC_SUA',    0, 0, 0, 0),
    ('MONHOC_XOA',    0, 0, 0, 0),
    ('TIM_LOP',       1, 1, 1, 1),
    ('TIM_GIAOVIEN',  1, 0, 0, 0),
    ('BC_DIEM',       1, 1, 1, 1),
    ('BC_DOANHTHU',   0, 0, 0, 0),
    ('BC_XEPHANG',    0, 0, 0, 0),
    ('BC_LOP_PHONG',  0, 0, 0, 0),
    ('DANHMUC_KHAC',  0, 0, 0, 0),
    ('XUAT_EXCEL',    1, 0, 1, 0),
    ('DANGKYLOP',     0, 0, 1, 1),
    ('THOIKHOA_XEM',  1, 1, 1, 1),
    ('AUTOMATION_XEM',1, 0, 0, 0),
    ('AUTOMATION_TASK',1, 0, 0, 0)
) AS m (MaChucNang, GV_CoQuyen, GV_ChiCuaMinh, HV_CoQuyen, HV_ChiCuaMinh)
WHERE NOT EXISTS
(
    SELECT 1 FROM dbo.PhanQuyen p
    WHERE p.MaChucNang = m.MaChucNang AND p.VaiTro = 'HocVien'
);
GO

/* ---------- 7. Seed tài khoản ---------- */
/* admin / Admin@123 */
IF NOT EXISTS (SELECT 1 FROM dbo.TaiKhoan WHERE VaiTro = N'Admin')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM dbo.TaiKhoan WHERE TenDangNhap = N'admin')
    BEGIN
        INSERT INTO dbo.TaiKhoan (TenDangNhap, MatKhauHash, Salt, VaiTro, MaGV, MaHocVien, TrangThai)
        VALUES ('admin',
                '4SptDElNMh0jSJ5bYJWT7CBI73NVIux6ftN9AeAGTmM=',
                'gtNmDqJ9klUajBL/a3OJIA==',
                'Admin', NULL, NULL, 1);
    END
END
GO

/* gv01 / Demo@123 (gắn GV01) */
IF NOT EXISTS (SELECT 1 FROM dbo.TaiKhoan WHERE TenDangNhap = N'gv01')
   AND EXISTS (SELECT 1 FROM dbo.GiaoVien WHERE MaGV = N'GV01')
   AND NOT EXISTS (SELECT 1 FROM dbo.TaiKhoan WHERE MaGV = N'GV01')
BEGIN
    INSERT INTO dbo.TaiKhoan (TenDangNhap, MatKhauHash, Salt, VaiTro, MaGV, MaHocVien, TrangThai)
    VALUES ('gv01',
            'DQG89MdzK+vhDeHR3rT77eVql26Ziuhq/D5xXUaOhns=',
            'Ou6xU/frG5I68luZuGM2xg==',
            'GiaoVien', 'GV01', NULL, 1);
END
GO

/* hv01 / Demo@123 (gắn HV01) */
IF NOT EXISTS (SELECT 1 FROM dbo.TaiKhoan WHERE TenDangNhap = N'hv01')
   AND EXISTS (SELECT 1 FROM dbo.HocVien WHERE MaHocVien = N'HV01')
   AND NOT EXISTS (SELECT 1 FROM dbo.TaiKhoan WHERE MaHocVien = N'HV01')
BEGIN
    INSERT INTO dbo.TaiKhoan (TenDangNhap, MatKhauHash, Salt, VaiTro, MaGV, MaHocVien, TrangThai)
    VALUES ('hv01',
            'DQG89MdzK+vhDeHR3rT77eVql26Ziuhq/D5xXUaOhns=',
            'Ou6xU/frG5I68luZuGM2xg==',
            'HocVien', NULL, 'HV01', 1);
END
GO

PRINT N'Đã tạo schema + seed phân quyền xong.';
GO
