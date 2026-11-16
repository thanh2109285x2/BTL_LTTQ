/* =============================================================
   TRÌNH TẠO BÁO CÁO ĐỘNG (chỉ THÊM, không xóa gì)
   DB   : QuanLyLopTinHocVaNgoaiNguNganHan
   Chạy : sqlcmd -S "localhost\SQLEXPRESS" -d "QuanLyLopTinHocVaNgoaiNguNganHan" -E -C -f 65001 -b -i Database\ReportBuilder.sql

   Nội dung:
     1. Bảng dbo.ReportDefinition lưu cấu hình báo cáo (JSON).
     2. Chức năng BAOCAO_TAO + quyền mặc định cho Giáo viên / Học viên.
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

/* ---------- 1. Bảng định nghĩa báo cáo ---------- */
IF OBJECT_ID(N'dbo.ReportDefinition', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ReportDefinition
    (
        MaBaoCao     int            IDENTITY(1,1) NOT NULL,
        Ten          nvarchar(200)  NOT NULL,
        MoTa         nvarchar(500)  NULL,
        MaNguon      varchar(50)    NOT NULL,
        NguoiTao     varchar(50)    NOT NULL,
        CongKhai     bit            NOT NULL CONSTRAINT DF_ReportDefinition_CongKhai DEFAULT (1),
        SoCot        int            NOT NULL CONSTRAINT DF_ReportDefinition_SoCot DEFAULT (0),
        SoBoLoc      int            NOT NULL CONSTRAINT DF_ReportDefinition_SoBoLoc DEFAULT (0),
        CauHinh      nvarchar(max)  NOT NULL,
        NgayTao      datetime2(0)   NOT NULL CONSTRAINT DF_ReportDefinition_NgayTao DEFAULT (SYSDATETIME()),
        NgayCapNhat  datetime2(0)   NOT NULL CONSTRAINT DF_ReportDefinition_NgayCapNhat DEFAULT (SYSDATETIME()),
        CONSTRAINT PK_ReportDefinition PRIMARY KEY (MaBaoCao)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes
               WHERE object_id = OBJECT_ID(N'dbo.ReportDefinition') AND name = N'IX_ReportDefinition_NguoiTao')
    CREATE INDEX IX_ReportDefinition_NguoiTao ON dbo.ReportDefinition (NguoiTao, NgayCapNhat DESC);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes
               WHERE object_id = OBJECT_ID(N'dbo.ReportDefinition') AND name = N'IX_ReportDefinition_Nguon')
    CREATE INDEX IX_ReportDefinition_Nguon ON dbo.ReportDefinition (MaNguon);
GO

/* ---------- 2. Chức năng mới ---------- */
IF OBJECT_ID(N'dbo.ChucNang', N'U') IS NOT NULL
BEGIN
    MERGE dbo.ChucNang AS target
    USING (VALUES
        ('BAOCAO_TAO', N'Trình tạo báo cáo', N'Báo cáo', 0, 0, 28)
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
END
GO

/* Quyền mặc định: Giáo viên và Học viên được tạo báo cáo nhưng chỉ trong phạm vi của mình. */
IF OBJECT_ID(N'dbo.PhanQuyen', N'U') IS NOT NULL
BEGIN
    INSERT INTO dbo.PhanQuyen (MaChucNang, VaiTro, CoQuyen, ChiCuaMinh)
    SELECT m.MaChucNang, m.VaiTro, m.CoQuyen, m.ChiCuaMinh
    FROM (VALUES
        ('BAOCAO_TAO', 'GiaoVien', 1, 1),
        ('BAOCAO_TAO', 'HocVien',  1, 1)
    ) AS m (MaChucNang, VaiTro, CoQuyen, ChiCuaMinh)
    WHERE NOT EXISTS
    (
        SELECT 1 FROM dbo.PhanQuyen p
        WHERE p.MaChucNang = m.MaChucNang AND p.VaiTro = m.VaiTro
    );
END
GO

PRINT N'Đã thêm bảng ReportDefinition + chức năng BAOCAO_TAO.';
GO
