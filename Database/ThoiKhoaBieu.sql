/* =============================================================
   THỜI KHÓA BIỂU + ĐĂNG KÝ LỚP  (chỉ THÊM, không xóa gì)
   DB   : QuanLyLopTinHocVaNgoaiNguNganHan
   Chạy : sqlcmd -S "localhost\SQLEXPRESS" -d "QuanLyLopTinHocVaNgoaiNguNganHan" -E -C -f 65001 -b -i Database\ThoiKhoaBieu.sql

   Luồng nghiệp vụ:
     1. Admin tạo lớp (LopHoc) + sĩ số tối đa
     2. Học viên tự đăng ký nhiều lớp           -> DangKy
     3. Admin bấm "Tạo thời khóa biểu"           -> BuoiHoc (CP-SAT xếp)
     4. GV/HV mở thời khóa biểu của mình         -> lọc theo MaGV / MaHocVien
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

/* ---------- 1. Bổ sung cột cho bảng cũ ---------- */

IF COL_LENGTH('dbo.LopHoc', 'SiSoToiDa') IS NULL
    ALTER TABLE dbo.LopHoc ADD SiSoToiDa smallint NOT NULL
        CONSTRAINT DF_LopHoc_SiSoToiDa DEFAULT (30);
GO

IF COL_LENGTH('dbo.LopHoc', 'SoBuoiTuan') IS NULL
    ALTER TABLE dbo.LopHoc ADD SoBuoiTuan tinyint NOT NULL
        CONSTRAINT DF_LopHoc_SoBuoiTuan DEFAULT (3);
GO

IF COL_LENGTH('dbo.PhongHoc', 'SucChua') IS NULL
    ALTER TABLE dbo.PhongHoc ADD SucChua smallint NULL;
GO

IF COL_LENGTH('dbo.CaHoc', 'GioBatDau') IS NULL
    ALTER TABLE dbo.CaHoc ADD GioBatDau time(0) NULL;
GO

IF COL_LENGTH('dbo.CaHoc', 'GioKetThuc') IS NULL
    ALTER TABLE dbo.CaHoc ADD GioKetThuc time(0) NULL;
GO

/* Giá trị mặc định cho dữ liệu cũ */
UPDATE dbo.CaHoc SET GioBatDau = '08:00', GioKetThuc = '11:00' WHERE MaCa = 'CA01' AND GioBatDau IS NULL;
UPDATE dbo.CaHoc SET GioBatDau = '14:00', GioKetThuc = '17:00' WHERE MaCa = 'CA02' AND GioBatDau IS NULL;
UPDATE dbo.CaHoc SET GioBatDau = '18:00', GioKetThuc = '21:00' WHERE MaCa = 'CA03' AND GioBatDau IS NULL;
UPDATE dbo.CaHoc SET GioBatDau = '08:00', GioKetThuc = '11:00' WHERE GioBatDau IS NULL;
GO

UPDATE dbo.PhongHoc SET SucChua = 30 WHERE SucChua IS NULL AND MaPhong = 'PH01';
UPDATE dbo.PhongHoc SET SucChua = 25 WHERE SucChua IS NULL AND MaPhong = 'PH02';
UPDATE dbo.PhongHoc SET SucChua = 40 WHERE SucChua IS NULL AND MaPhong = 'PH03';
UPDATE dbo.PhongHoc SET SucChua = 30 WHERE SucChua IS NULL;
GO

/* ---------- 2. Thứ trong tuần dạng số (2..7, CN = 8) ---------- */

IF OBJECT_ID(N'dbo.ThoiGian_Thu', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ThoiGian_Thu
    (
        MaThoiGian varchar(20) NOT NULL,
        Thu        tinyint     NOT NULL,
        CONSTRAINT PK_ThoiGian_Thu PRIMARY KEY (MaThoiGian, Thu),
        CONSTRAINT FK_ThoiGian_Thu_ThoiGian FOREIGN KEY (MaThoiGian)
            REFERENCES dbo.ThoiGian (MaThoiGian) ON DELETE CASCADE,
        CONSTRAINT CK_ThoiGian_Thu_Thu CHECK (Thu BETWEEN 2 AND 8)
    );
END
GO

MERGE dbo.ThoiGian_Thu AS target
USING (VALUES
    ('TG01', 2), ('TG01', 4), ('TG01', 6),
    ('TG02', 3), ('TG02', 5), ('TG02', 7),
    ('TG03', 7), ('TG03', 8)
) AS source (MaThoiGian, Thu)
ON target.MaThoiGian = source.MaThoiGian AND target.Thu = source.Thu
WHEN NOT MATCHED THEN INSERT (MaThoiGian, Thu) VALUES (source.MaThoiGian, source.Thu);
GO

/* Số buổi/tuần của lớp cũ = số thứ trong mẫu thời gian */
UPDATE l
SET l.SoBuoiTuan = x.SoThu
FROM dbo.LopHoc l
JOIN (SELECT MaThoiGian, COUNT(*) AS SoThu FROM dbo.ThoiGian_Thu GROUP BY MaThoiGian) x
    ON x.MaThoiGian = l.MaThoiGian
WHERE l.SoBuoiTuan = 3 AND x.SoThu <> 3;
GO

/* ---------- 3. Đăng ký lớp (nhiều - nhiều) ---------- */

IF OBJECT_ID(N'dbo.DangKy', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.DangKy
    (
        MaHocVien  varchar(20) NOT NULL,
        MaLop      varchar(20) NOT NULL,
        NgayDangKy date        NOT NULL CONSTRAINT DF_DangKy_Ngay DEFAULT (CAST(GETDATE() AS date)),
        CONSTRAINT PK_DangKy PRIMARY KEY (MaHocVien, MaLop),
        CONSTRAINT FK_DangKy_HocVien FOREIGN KEY (MaHocVien)
            REFERENCES dbo.HocVien (MaHocVien) ON DELETE CASCADE,
        CONSTRAINT FK_DangKy_LopHoc FOREIGN KEY (MaLop)
            REFERENCES dbo.LopHoc (MaLop) ON DELETE CASCADE
    );
END
GO

/* Migrate dữ liệu cũ: HocVien.MaLop -> DangKy (chạy 1 lần, idempotent) */
INSERT INTO dbo.DangKy (MaHocVien, MaLop)
SELECT h.MaHocVien, h.MaLop
FROM dbo.HocVien h
WHERE h.MaLop IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.DangKy d
                  WHERE d.MaHocVien = h.MaHocVien AND d.MaLop = h.MaLop);
GO

/* ---------- 4. Kết quả xếp lịch ---------- */

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
        CONSTRAINT FK_BuoiHoc_LopHoc FOREIGN KEY (MaLop)
            REFERENCES dbo.LopHoc (MaLop) ON DELETE CASCADE,
        CONSTRAINT FK_BuoiHoc_GiaoVien FOREIGN KEY (MaGV)
            REFERENCES dbo.GiaoVien (MaGV),
        CONSTRAINT FK_BuoiHoc_CaHoc FOREIGN KEY (MaCa)
            REFERENCES dbo.CaHoc (MaCa),
        CONSTRAINT FK_BuoiHoc_PhongHoc FOREIGN KEY (MaPhong)
            REFERENCES dbo.PhongHoc (MaPhong)
    );

    /* Mỗi phòng / mỗi giáo viên chỉ 1 lớp trong cùng (ngày, ca) */
    CREATE UNIQUE INDEX UX_BuoiHoc_Phong ON dbo.BuoiHoc (Ngay, MaCa, MaPhong);
    CREATE UNIQUE INDEX UX_BuoiHoc_GiaoVien ON dbo.BuoiHoc (Ngay, MaCa, MaGV);
    CREATE INDEX IX_BuoiHoc_Lop ON dbo.BuoiHoc (MaLop, Ngay);
END
GO

/* ---------- 5. Chức năng mới ---------- */

MERGE dbo.ChucNang AS target
USING (VALUES
    ('DANGKYLOP',    N'Đăng ký lớp học',      N'Quản lý',  0, 1, 5),
    ('THOIKHOA_XEM', N'Xem thời khóa biểu',   N'Tra cứu',  0, 1, 18),
    ('THOIKHOA_XEP', N'Tạo thời khóa biểu',   N'Hệ thống', 1, 0, 27)
) AS source (MaChucNang, TenChucNang, Nhom, ChiAdmin, HoTroLocTheoChu, ThuTu)
ON target.MaChucNang = source.MaChucNang
WHEN MATCHED THEN
    UPDATE SET TenChucNang = source.TenChucNang,
               Nhom = source.Nhom,
               ChiAdmin = source.ChiAdmin,
               HoTroLocTheoChu = source.HoTroLocTheoChu,
               ThuTu = source.ThuTu
WHEN NOT MATCHED THEN
    INSERT (MaChucNang, TenChucNang, Nhom, ChiAdmin, HoTroLocTheoChu, ThuTu)
    VALUES (source.MaChucNang, source.TenChucNang, source.Nhom,
            source.ChiAdmin, source.HoTroLocTheoChu, source.ThuTu);
GO

/* Quyền mặc định cho Giáo viên / Học viên (chỉ chèn dòng còn thiếu) */
INSERT INTO dbo.PhanQuyen (MaChucNang, VaiTro, CoQuyen, ChiCuaMinh)
SELECT m.MaChucNang, m.VaiTro, m.CoQuyen, m.ChiCuaMinh
FROM (VALUES
    ('DANGKYLOP',    'GiaoVien', 0, 0),
    ('DANGKYLOP',    'HocVien',  1, 1),
    ('THOIKHOA_XEM', 'GiaoVien', 1, 1),
    ('THOIKHOA_XEM', 'HocVien',  1, 1)
) AS m (MaChucNang, VaiTro, CoQuyen, ChiCuaMinh)
WHERE NOT EXISTS
(
    SELECT 1 FROM dbo.PhanQuyen p
    WHERE p.MaChucNang = m.MaChucNang AND p.VaiTro = m.VaiTro
);
GO

PRINT N'Đã thêm schema + quyền cho Thời khóa biểu.';
GO
