/* =============================================================
   BỔ SUNG TỪ DB BAN ĐẦU (Qlloptinhoc.sql) -> DB HIỆN TẠI ỨNG DỤNG
   DB   : QuanLyLopTinHocVaNgoaiNguNganHan
   Chạy : sqlcmd -S "localhost\SQLEXPRESS" -d "QuanLyLopTinHocVaNgoaiNguNganHan" -E -C -f 65001 -b -i Database\BoSung_Tu_Qlloptinhoc.sql

   Script idempotent (chỉ THÊM, không xóa dữ liệu/cột cũ):
   - Bổ sung cột còn thiếu cho LopHoc, PhongHoc, CaHoc (theo code/DAL hiện tại)
   - Tạo bảng còn thiếu: ThoiGian_Thu, DangKy, BuoiHoc, ChucNang, PhanQuyen, TaiKhoan
   - Tạo khóa/constraint/chỉ mục/FK cần thiết
   - Seed dữ liệu chức năng, phân quyền mẫu, tài khoản mẫu, mẫu ThoiGian_Thu
   Ghi chú: DB ban đầu có LopHoc.SiSo, PhongHoc.DiaChi, HocVien.MaLop -> giữ nguyên để tương thích.
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

PRINT N'=== 1. BỔ SUNG CỘT CHO BẢNG HIỆN CÓ ===';

/* LopHoc: phục vụ quản lý lớp, đăng ký, xếp thời khóa biểu, import */
IF COL_LENGTH('dbo.LopHoc', 'SiSoToiDa') IS NULL
BEGIN
    ALTER TABLE dbo.LopHoc ADD SiSoToiDa smallint NOT NULL
        CONSTRAINT DF_LopHoc_SiSoToiDa DEFAULT (30);
    PRINT N'  + LopHoc.SiSoToiDa';
END

IF COL_LENGTH('dbo.LopHoc', 'SoBuoiTuan') IS NULL
BEGIN
    ALTER TABLE dbo.LopHoc ADD SoBuoiTuan tinyint NOT NULL
        CONSTRAINT DF_LopHoc_SoBuoiTuan DEFAULT (3);
    PRINT N'  + LopHoc.SoBuoiTuan';
END

IF COL_LENGTH('dbo.LopHoc', 'TrangThai') IS NULL
BEGIN
    ALTER TABLE dbo.LopHoc ADD TrangThai nvarchar(50) NULL
        CONSTRAINT DF_LopHoc_TrangThai DEFAULT (N'DangHoc');
    PRINT N'  + LopHoc.TrangThai (dùng cho import/luồng lớp)';
END

IF COL_LENGTH('dbo.LopHoc', 'SiSoToiThieu') IS NULL
BEGIN
    ALTER TABLE dbo.LopHoc ADD SiSoToiThieu smallint NOT NULL
        CONSTRAINT DF_LopHoc_SiSoToiThieu DEFAULT (0);
    PRINT N'  + LopHoc.SiSoToiThieu';
END

/* PhongHoc: app hiện dùng SucChua (DB ban đầu có DiaChi -> giữ nguyên, thêm SucChua) */
IF COL_LENGTH('dbo.PhongHoc', 'SucChua') IS NULL
BEGIN
    ALTER TABLE dbo.PhongHoc ADD SucChua int NULL;
    PRINT N'  + PhongHoc.SucChua';
END

/* CaHoc: cần giờ bắt đầu/kết thúc để hiển thị/xếp buổi học */
IF COL_LENGTH('dbo.CaHoc', 'GioBatDau') IS NULL
BEGIN
    ALTER TABLE dbo.CaHoc ADD GioBatDau time(0) NULL;
    PRINT N'  + CaHoc.GioBatDau';
END

IF COL_LENGTH('dbo.CaHoc', 'GioKetThuc') IS NULL
BEGIN
    ALTER TABLE dbo.CaHoc ADD GioKetThuc time(0) NULL;
    PRINT N'  + CaHoc.GioKetThuc';
END
GO

PRINT N'=== 2. BẢNG THỜI GIAN - THỨ (lịch học cố định) ===';
IF OBJECT_ID(N'dbo.ThoiGian_Thu', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ThoiGian_Thu
    (
        MaThoiGian varchar(20) NOT NULL,
        Thu        tinyint     NOT NULL,   -- 0=Thứ 2 ... 5=Thứ 7, 6=Chủ nhật
        CONSTRAINT PK_ThoiGian_Thu PRIMARY KEY (MaThoiGian, Thu),
        CONSTRAINT FK_ThoiGian_Thu_ThoiGian FOREIGN KEY (MaThoiGian)
            REFERENCES dbo.ThoiGian(MaThoiGian) ON DELETE CASCADE,
        CONSTRAINT CK_ThoiGian_Thu_Thu CHECK (Thu BETWEEN 0 AND 6)
    );
    PRINT N'  + Tạo bảng ThoiGian_Thu';
END
GO

/* Seed mẫu nếu bảng đang rỗng (không ghi đè dữ liệu đã có) */
IF OBJECT_ID(N'dbo.ThoiGian_Thu', N'U') IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM dbo.ThoiGian_Thu)
BEGIN
    IF EXISTS (SELECT 1 FROM dbo.ThoiGian WHERE MaThoiGian = '2-4-6')
        INSERT INTO dbo.ThoiGian_Thu (MaThoiGian, Thu) VALUES ('2-4-6', 0), ('2-4-6', 2), ('2-4-6', 4);
    IF EXISTS (SELECT 1 FROM dbo.ThoiGian WHERE MaThoiGian = '3-5-7')
        INSERT INTO dbo.ThoiGian_Thu (MaThoiGian, Thu) VALUES ('3-5-7', 1), ('3-5-7', 3), ('3-5-7', 5);
    PRINT N'  + Seed mẫu ThoiGian_Thu (nếu có MaThoiGian 2-4-6, 3-5-7)';
END
GO

PRINT N'=== 3. BẢNG ĐĂNG KÝ LỚP (nhiều học viên - nhiều lớp) ===';
IF OBJECT_ID(N'dbo.DangKy', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.DangKy
    (
        MaHocVien  varchar(20) NOT NULL,
        MaLop      varchar(20) NOT NULL,
        NgayDangKy date        NOT NULL CONSTRAINT DF_DangKy_Ngay DEFAULT (CAST(GETDATE() AS date)),
        CONSTRAINT PK_DangKy PRIMARY KEY (MaHocVien, MaLop),
        CONSTRAINT FK_DangKy_HocVien FOREIGN KEY (MaHocVien)
            REFERENCES dbo.HocVien(MaHocVien) ON DELETE CASCADE,
        CONSTRAINT FK_DangKy_LopHoc FOREIGN KEY (MaLop)
            REFERENCES dbo.LopHoc(MaLop) ON DELETE CASCADE
    );
    PRINT N'  + Tạo bảng DangKy';
END

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_DangKy_MaLop' AND object_id = OBJECT_ID('dbo.DangKy'))
   AND OBJECT_ID(N'dbo.DangKy', N'U') IS NOT NULL
BEGIN
    CREATE INDEX IX_DangKy_MaLop ON dbo.DangKy(MaLop);
    PRINT N'  + Index IX_DangKy_MaLop';
END
GO

PRINT N'=== 4. BẢNG BUỔI HỌC (thời khóa biểu đã xếp) ===';
IF OBJECT_ID(N'dbo.BuoiHoc', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.BuoiHoc
    (
        MaBuoi  int         IDENTITY(1,1) NOT NULL,
        MaLop   varchar(20) NOT NULL,
        MaGV    varchar(20) NOT NULL,
        Ngay    date        NOT NULL,
        MaCa    varchar(20) NOT NULL,
        MaPhong varchar(20) NOT NULL,
        CONSTRAINT PK_BuoiHoc PRIMARY KEY (MaBuoi),
        CONSTRAINT FK_BuoiHoc_LopHoc   FOREIGN KEY (MaLop)   REFERENCES dbo.LopHoc(MaLop)     ON DELETE CASCADE,
        CONSTRAINT FK_BuoiHoc_GiaoVien FOREIGN KEY (MaGV)    REFERENCES dbo.GiaoVien(MaGV),
        CONSTRAINT FK_BuoiHoc_CaHoc    FOREIGN KEY (MaCa)    REFERENCES dbo.CaHoc(MaCa),
        CONSTRAINT FK_BuoiHoc_PhongHoc FOREIGN KEY (MaPhong) REFERENCES dbo.PhongHoc(MaPhong)
    );
    PRINT N'  + Tạo bảng BuoiHoc';
END

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_BuoiHoc_Phong' AND object_id = OBJECT_ID('dbo.BuoiHoc'))
   AND OBJECT_ID(N'dbo.BuoiHoc', N'U') IS NOT NULL
    CREATE UNIQUE INDEX UX_BuoiHoc_Phong ON dbo.BuoiHoc(Ngay, MaCa, MaPhong);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_BuoiHoc_GV' AND object_id = OBJECT_ID('dbo.BuoiHoc'))
   AND OBJECT_ID(N'dbo.BuoiHoc', N'U') IS NOT NULL
    CREATE UNIQUE INDEX UX_BuoiHoc_GV    ON dbo.BuoiHoc(Ngay, MaCa, MaGV);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_BuoiHoc_Lop' AND object_id = OBJECT_ID('dbo.BuoiHoc'))
   AND OBJECT_ID(N'dbo.BuoiHoc', N'U') IS NOT NULL
    CREATE UNIQUE INDEX UX_BuoiHoc_Lop   ON dbo.BuoiHoc(Ngay, MaCa, MaLop);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_BuoiHoc_Ngay' AND object_id = OBJECT_ID('dbo.BuoiHoc'))
   AND OBJECT_ID(N'dbo.BuoiHoc', N'U') IS NOT NULL
    CREATE INDEX IX_BuoiHoc_Ngay ON dbo.BuoiHoc(Ngay);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_BuoiHoc_MaLop' AND object_id = OBJECT_ID('dbo.BuoiHoc'))
   AND OBJECT_ID(N'dbo.BuoiHoc', N'U') IS NOT NULL
    CREATE INDEX IX_BuoiHoc_MaLop ON dbo.BuoiHoc(MaLop);
GO

PRINT N'=== 5. PHÂN QUYỀN: ChucNang, PhanQuyen, TaiKhoan ===';
IF OBJECT_ID(N'dbo.ChucNang', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ChucNang
    (
        MaChucNang      varchar(50)   NOT NULL,
        TenChucNang     nvarchar(200) NOT NULL,
        Nhom            nvarchar(50)  NOT NULL CONSTRAINT DF_ChucNang_Nhom DEFAULT (N''),
        HoTroLocTheoChu bit           NOT NULL CONSTRAINT DF_ChucNang_HoTroLocTheoChu DEFAULT (0),
        ThuTu           int           NOT NULL CONSTRAINT DF_ChucNang_ThuTu DEFAULT (0),
        CONSTRAINT PK_ChucNang PRIMARY KEY (MaChucNang)
    );
    PRINT N'  + Tạo bảng ChucNang';
END

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
            REFERENCES dbo.ChucNang(MaChucNang) ON DELETE CASCADE,
        CONSTRAINT CK_PhanQuyen_VaiTro CHECK (VaiTro IN ('Admin', 'GiaoVien', 'HocVien')),
        CONSTRAINT CK_PhanQuyen_ChiCuaMinh CHECK (ChiCuaMinh = 0 OR CoQuyen = 1)
    );
    PRINT N'  + Tạo bảng PhanQuyen';
END

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
        CONSTRAINT FK_TaiKhoan_GiaoVien FOREIGN KEY (MaGV) REFERENCES dbo.GiaoVien(MaGV),
        CONSTRAINT FK_TaiKhoan_HocVien  FOREIGN KEY (MaHocVien) REFERENCES dbo.HocVien(MaHocVien),
        CONSTRAINT CK_TaiKhoan_VaiTro CHECK (VaiTro IN ('Admin', 'GiaoVien', 'HocVien'))
    );
    PRINT N'  + Tạo bảng TaiKhoan';
END

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_TaiKhoan_MaGV' AND object_id = OBJECT_ID('dbo.TaiKhoan'))
   AND OBJECT_ID(N'dbo.TaiKhoan', N'U') IS NOT NULL
    CREATE UNIQUE INDEX UQ_TaiKhoan_MaGV ON dbo.TaiKhoan(MaGV) WHERE MaGV IS NOT NULL;
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_TaiKhoan_MaHocVien' AND object_id = OBJECT_ID('dbo.TaiKhoan'))
   AND OBJECT_ID(N'dbo.TaiKhoan', N'U') IS NOT NULL
    CREATE UNIQUE INDEX UQ_TaiKhoan_MaHocVien ON dbo.TaiKhoan(MaHocVien) WHERE MaHocVien IS NOT NULL;
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_TaiKhoan_Admin' AND object_id = OBJECT_ID('dbo.TaiKhoan'))
   AND OBJECT_ID(N'dbo.TaiKhoan', N'U') IS NOT NULL
    CREATE UNIQUE INDEX UX_TaiKhoan_Admin ON dbo.TaiKhoan(VaiTro) WHERE VaiTro = 'Admin';
GO

PRINT N'=== 6. SEED CHỨC NĂNG (MERGE theo MaChucNang) ===';
IF OBJECT_ID(N'dbo.ChucNang', N'U') IS NOT NULL
BEGIN
    MERGE dbo.ChucNang AS target
    USING (VALUES
        ('LOPHOC_XEM',      N'Quản lý lớp học - Xem',            N'QuanLy',  1,  1),
        ('LOPHOC_THEM',     N'Quản lý lớp học - Thêm',           N'QuanLy',  0,  2),
        ('LOPHOC_SUA',      N'Quản lý lớp học - Sửa',            N'QuanLy',  1,  3),
        ('LOPHOC_XOA',      N'Quản lý lớp học - Xóa',            N'QuanLy',  0,  4),
        ('HOCVIEN_XEM',     N'Quản lý học viên - Xem',           N'QuanLy',  1,  5),
        ('HOCVIEN_THEM',    N'Quản lý học viên - Thêm',          N'QuanLy',  0,  6),
        ('HOCVIEN_SUA',     N'Quản lý học viên - Sửa',           N'QuanLy',  1,  7),
        ('HOCVIEN_XOA',     N'Quản lý học viên - Xóa',           N'QuanLy',  0,  8),
        ('GIAOVIEN_XEM',    N'Quản lý giáo viên - Xem',          N'QuanLy',  1,  9),
        ('GIAOVIEN_THEM',   N'Quản lý giáo viên - Thêm',         N'QuanLy',  0, 10),
        ('GIAOVIEN_SUA',    N'Quản lý giáo viên - Sửa',          N'QuanLy',  1, 11),
        ('GIAOVIEN_XOA',    N'Quản lý giáo viên - Xóa',          N'QuanLy',  0, 12),
        ('MONHOC_XEM',      N'Danh mục môn học - Xem',           N'DanhMuc', 0, 13),
        ('MONHOC_THEM',     N'Danh mục môn học - Thêm',          N'DanhMuc', 0, 14),
        ('MONHOC_SUA',      N'Danh mục môn học - Sửa',           N'DanhMuc', 0, 15),
        ('MONHOC_XOA',      N'Danh mục môn học - Xóa',           N'DanhMuc', 0, 16),
        ('DANHMUC_KHAC',    N'Danh mục khác (Nghề, Cơ quan, ...)', N'DanhMuc', 0, 17),
        ('TIM_LOP',         N'Tra cứu lớp học',                  N'TraCuu',  1, 18),
        ('TIM_GIAOVIEN',    N'Tra cứu giáo viên',                N'TraCuu',  0, 19),
        ('BC_DIEM',         N'Báo cáo điểm',                     N'BaoCao',  1, 20),
        ('BC_HOCPHI',       N'Báo cáo học phí',                  N'BaoCao',  1, 21),
        ('BC_SISO',         N'Báo cáo sĩ số',                    N'BaoCao',  1, 22),
        ('PHANQUYEN',       N'Phân quyền',                       N'HeThong', 0, 23),
        ('TAIKHOAN',        N'Quản lý tài khoản',                N'HeThong', 0, 24),
        ('DANGKYLOP',       N'Đăng ký lớp',                      N'QuanLy',  0, 25),
        ('THOIKHOA_XEM',    N'Thời khóa biểu - Xem',             N'TraCuu',  1, 26),
        ('THOIKHOA_XEP',    N'Thời khóa biểu - Tạo/xếp lịch',    N'QuanLy',  0, 27)
    ) AS src(MaChucNang, TenChucNang, Nhom, HoTroLocTheoChu, ThuTu)
    ON target.MaChucNang = src.MaChucNang
    WHEN MATCHED THEN
        UPDATE SET
            TenChucNang     = src.TenChucNang,
            Nhom            = src.Nhom,
            HoTroLocTheoChu = src.HoTroLocTheoChu,
            ThuTu           = src.ThuTu
    WHEN NOT MATCHED BY TARGET THEN
        INSERT (MaChucNang, TenChucNang, Nhom, HoTroLocTheoChu, ThuTu)
        VALUES (src.MaChucNang, src.TenChucNang, src.Nhom, src.HoTroLocTheoChu, src.ThuTu);
END
GO

PRINT N'=== 7. SEED PHÂN QUYỀN MẪU (nếu chưa có) ===';
IF OBJECT_ID(N'dbo.PhanQuyen', N'U') IS NOT NULL AND OBJECT_ID(N'dbo.ChucNang', N'U') IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM dbo.PhanQuyen)
    BEGIN
        INSERT INTO dbo.PhanQuyen (MaChucNang, VaiTro, CoQuyen, ChiCuaMinh)
        SELECT MaChucNang, 'GiaoVien', 1, 1
        FROM dbo.ChucNang
        WHERE MaChucNang IN ('LOPHOC_XEM', 'LOPHOC_SUA', 'HOCVIEN_XEM', 'GIAOVIEN_XEM',
                             'MONHOC_XEM', 'TIM_LOP', 'TIM_GIAOVIEN', 'BC_DIEM', 'BC_HOCPHI', 'BC_SISO');

        INSERT INTO dbo.PhanQuyen (MaChucNang, VaiTro, CoQuyen, ChiCuaMinh)
        SELECT MaChucNang, 'HocVien', 1, 1
        FROM dbo.ChucNang
        WHERE MaChucNang IN ('LOPHOC_XEM', 'HOCVIEN_XEM', 'TIM_LOP', 'BC_DIEM', 'BC_HOCPHI');

        /* Quyền bổ sung cho module đăng ký lớp/thời khóa biểu (có thể điều chỉnh trong Phân quyền) */
        INSERT INTO dbo.PhanQuyen (MaChucNang, VaiTro, CoQuyen, ChiCuaMinh)
        SELECT MaChucNang, 'HocVien', 1, 0
        FROM dbo.ChucNang
        WHERE MaChucNang IN ('DANGKYLOP', 'THOIKHOA_XEM')
          AND NOT EXISTS (SELECT 1 FROM dbo.PhanQuyen p WHERE p.MaChucNang = dbo.ChucNang.MaChucNang AND p.VaiTro = 'HocVien');

        INSERT INTO dbo.PhanQuyen (MaChucNang, VaiTro, CoQuyen, ChiCuaMinh)
        SELECT MaChucNang, 'GiaoVien', 1, 1
        FROM dbo.ChucNang
        WHERE MaChucNang IN ('THOIKHOA_XEM')
          AND NOT EXISTS (SELECT 1 FROM dbo.PhanQuyen p WHERE p.MaChucNang = dbo.ChucNang.MaChucNang AND p.VaiTro = 'GiaoVien');

        PRINT N'  + Seed phân quyền mẫu';
    END
    ELSE
        PRINT N'  (Bỏ qua seed phân quyền: bảng PhanQuyen đã có dữ liệu)';
END
GO

PRINT N'=== 8. SEED TÀI KHOẢN MẪU (nếu chưa có) ===';
IF OBJECT_ID(N'dbo.TaiKhoan', N'U') IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.TaiKhoan)
BEGIN
    /* Mật khẩu mẫu: admin / Admin@123, gv01 & hv01 / Demo@123 (hash demo sẵn) */
    INSERT INTO dbo.TaiKhoan (TenDangNhap, MatKhauHash, Salt, VaiTro, MaGV, MaHocVien)
    VALUES ('admin', 's8bW6iIvQpUtLNg7ZkA51G8/f+ePx/QbQTs54AoflTo=', 'sRIRpvn5JWa3F6tFPxWVlg==', 'Admin', NULL, NULL);

    IF OBJECT_ID(N'dbo.GiaoVien', N'U') IS NOT NULL AND COL_LENGTH('dbo.GiaoVien','MaGV') IS NOT NULL
       AND EXISTS (SELECT 1 FROM dbo.GiaoVien WHERE MaGV = 'GV01')
       AND NOT EXISTS (SELECT 1 FROM dbo.TaiKhoan WHERE MaGV = 'GV01' OR TenDangNhap = 'gv01')
    BEGIN
        INSERT INTO dbo.TaiKhoan (TenDangNhap, MatKhauHash, Salt, VaiTro, MaGV, MaHocVien)
        VALUES ('gv01', 'LGSUDn9VSvNTI4mDC4BzQAkFgNgVxWH2oS14ZfN58xw=', 'uGmhRd2CK/sgj+YROXT9yw==', 'GiaoVien', 'GV01', NULL);
    END

    IF OBJECT_ID(N'dbo.HocVien', N'U') IS NOT NULL AND COL_LENGTH('dbo.HocVien','MaHocVien') IS NOT NULL
       AND EXISTS (SELECT 1 FROM dbo.HocVien WHERE MaHocVien = 'HV01')
       AND NOT EXISTS (SELECT 1 FROM dbo.TaiKhoan WHERE MaHocVien = 'HV01' OR TenDangNhap = 'hv01')
    BEGIN
        INSERT INTO dbo.TaiKhoan (TenDangNhap, MatKhauHash, Salt, VaiTro, MaGV, MaHocVien)
        VALUES ('hv01', 'LGSUDn9VSvNTI4mDC4BzQAkFgNgVxWH2oS14ZfN58xw=', 'uGmhRd2CK/sgj+YROXT9yw==', 'HocVien', NULL, 'HV01');
    END

    PRINT N'  + Seed tài khoản mẫu (admin; gv01/hv01 nếu có hồ sơ tương ứng)';
END
ELSE
    PRINT N'  (Bỏ qua seed tài khoản: bảng TaiKhoan đã có dữ liệu hoặc chưa tồn tại)';
GO

PRINT N'=== HOÀN TẤT BỔ SUNG DB ===';
GO
