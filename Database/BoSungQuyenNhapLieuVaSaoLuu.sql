/* Add the bulk-import and database-backup permissions to an existing database. */
IF NOT EXISTS (SELECT 1 FROM dbo.ChucNang WHERE MaChucNang = 'IMPORT_DULIEU')
BEGIN
    INSERT dbo.ChucNang (MaChucNang, TenChucNang, Nhom, HoTroLocTheoChu, ChiAdmin, ThuTu)
    VALUES ('IMPORT_DULIEU', N'Thêm dữ liệu', N'Khác', 0, 0, 81);
END;

IF NOT EXISTS (SELECT 1 FROM dbo.ChucNang WHERE MaChucNang = 'DATABASE_BACKUP')
BEGIN
    INSERT dbo.ChucNang (MaChucNang, TenChucNang, Nhom, HoTroLocTheoChu, ChiAdmin, ThuTu)
    VALUES ('DATABASE_BACKUP', N'Backup database', N'Khác', 0, 0, 82);
END;

/* Keep the existing bulk-import access where the role already has an import right. */
INSERT dbo.PhanQuyen (MaChucNang, VaiTro, CoQuyen, ChiCuaMinh)
SELECT 'IMPORT_DULIEU', 'GiaoVien',
       CASE WHEN EXISTS (
           SELECT 1 FROM dbo.PhanQuyen
           WHERE VaiTro = 'GiaoVien'
             AND MaChucNang IN ('HOCVIEN_THEM', 'GIAOVIEN_THEM', 'DANGKYLOP')
             AND CoQuyen = 1
       ) THEN 1 ELSE 0 END,
       0
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.PhanQuyen
    WHERE MaChucNang = 'IMPORT_DULIEU' AND VaiTro = 'GiaoVien'
);

INSERT dbo.PhanQuyen (MaChucNang, VaiTro, CoQuyen, ChiCuaMinh)
SELECT 'IMPORT_DULIEU', 'HocVien', 0, 0
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.PhanQuyen
    WHERE MaChucNang = 'IMPORT_DULIEU' AND VaiTro = 'HocVien'
);

/* Backup permissions are opt-in from the permission screen. */
INSERT dbo.PhanQuyen (MaChucNang, VaiTro, CoQuyen, ChiCuaMinh)
SELECT 'DATABASE_BACKUP', p.VaiTro, 0, 0
FROM (VALUES ('GiaoVien'), ('HocVien')) p (VaiTro)
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.PhanQuyen q
    WHERE q.MaChucNang = 'DATABASE_BACKUP' AND q.VaiTro = p.VaiTro
);
GO
