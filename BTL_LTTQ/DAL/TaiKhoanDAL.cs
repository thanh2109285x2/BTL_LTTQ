using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace BTL_LTTQ.DAL
{
    public class TaiKhoanDAL
    {
        public bool CoAdmin()
        {
            object? ketQua = DbHelper.ExecuteScalar(
                "SELECT CASE WHEN EXISTS (SELECT 1 FROM TaiKhoan WHERE VaiTro = @VaiTro) THEN 1 ELSE 0 END",
                new SqlParameter("@VaiTro", SqlDbType.VarChar, 20) { Value = "Admin" });
            return Convert.ToInt32(ketQua) == 1;
        }

        public DataTable LayGiaoVienChuaCoTaiKhoan()
        {
            const string sql = @"SELECT g.MaGV, g.TenGV
                                 FROM GiaoVien g
                                 WHERE NOT EXISTS (SELECT 1 FROM TaiKhoan t WHERE t.MaGV = g.MaGV)
                                 ORDER BY g.TenGV";
            return DbHelper.ExecuteQuery(sql);
        }

        public DataTable LayHocVienChuaCoTaiKhoan()
        {
            const string sql = @"SELECT h.MaHocVien, h.TenHocVien
                                 FROM HocVien h
                                 WHERE NOT EXISTS (SELECT 1 FROM TaiKhoan t WHERE t.MaHocVien = h.MaHocVien)
                                 ORDER BY h.TenHocVien";
            return DbHelper.ExecuteQuery(sql);
        }

        public string? LayDienThoaiGiaoVien(string maGV)
        {
            return DbHelper.ExecuteScalar("SELECT DienThoai FROM GiaoVien WHERE MaGV = @Ma",
                new SqlParameter("@Ma", SqlDbType.VarChar, 20) { Value = maGV }) as string;
        }

        public string? LayDienThoaiHocVien(string maHocVien)
        {
            return DbHelper.ExecuteScalar("SELECT DienThoai FROM HocVien WHERE MaHocVien = @Ma",
                new SqlParameter("@Ma", SqlDbType.VarChar, 20) { Value = maHocVien }) as string;
        }

        public bool TenDangNhapDaTonTai(string tenDangNhap)
        {
            object? ketQua = DbHelper.ExecuteScalar(
                "SELECT CASE WHEN EXISTS (SELECT 1 FROM TaiKhoan WHERE TenDangNhap = @TenDangNhap) THEN 1 ELSE 0 END",
                new SqlParameter("@TenDangNhap", SqlDbType.VarChar, 50) { Value = tenDangNhap });
            return Convert.ToInt32(ketQua) == 1;
        }

        public int ThemTaiKhoanAdmin(string tenDangNhap, string hash, string salt, string? maGV, string? maHocVien)
        {
            const string sqlAdmin = @"INSERT INTO TaiKhoan (TenDangNhap, MatKhauHash, Salt, VaiTro, MaGV, MaHocVien)
                                      SELECT @TenDangNhap, @MatKhauHash, @Salt, @VaiTro, @MaGV, @MaHocVien
                                      WHERE NOT EXISTS (
                                          SELECT 1 FROM TaiKhoan WITH (UPDLOCK, HOLDLOCK) WHERE VaiTro = @VaiTro)";
            return DbHelper.ExecuteNonQuery(sqlAdmin, ThamSoTaiKhoan(tenDangNhap, hash, salt, "Admin", maGV, maHocVien));
        }

        public int ThemTaiKhoan(string tenDangNhap, string hash, string salt, string vaiTro, string? maGV, string? maHocVien)
        {
            const string sql = @"INSERT INTO TaiKhoan (TenDangNhap, MatKhauHash, Salt, VaiTro, MaGV, MaHocVien)
                                 VALUES (@TenDangNhap, @MatKhauHash, @Salt, @VaiTro, @MaGV, @MaHocVien)";
            return DbHelper.ExecuteNonQuery(sql, ThamSoTaiKhoan(tenDangNhap, hash, salt, vaiTro, maGV, maHocVien));
        }

        public DataTable LayTheoTenDangNhap(string tenDangNhap)
        {
            const string sql = @"SELECT MatKhauHash, Salt, VaiTro, MaGV, MaHocVien, TrangThai
                                 FROM TaiKhoan
                                 WHERE TenDangNhap = @TenDangNhap";
            return DbHelper.ExecuteQuery(sql,
                new SqlParameter("@TenDangNhap", SqlDbType.VarChar, 50) { Value = tenDangNhap });
        }

        public DataTable LayHashSalt(string tenDangNhap)
        {
            const string sql = @"SELECT MatKhauHash, Salt
                                 FROM TaiKhoan
                                 WHERE TenDangNhap = @TenDangNhap";
            return DbHelper.ExecuteQuery(sql,
                new SqlParameter("@TenDangNhap", SqlDbType.VarChar, 50) { Value = tenDangNhap });
        }

        public int CapNhatHashSalt(string tenDangNhap, string hashMoi, string saltMoi)
        {
            const string sql = @"UPDATE TaiKhoan
                                 SET MatKhauHash = @MatKhauHash, Salt = @Salt
                                 WHERE TenDangNhap = @TenDangNhap";
            return DbHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MatKhauHash", SqlDbType.VarChar, 256) { Value = hashMoi },
                new SqlParameter("@Salt", SqlDbType.VarChar, 64) { Value = saltMoi },
                new SqlParameter("@TenDangNhap", SqlDbType.VarChar, 50) { Value = tenDangNhap });
        }

        private static SqlParameter[] ThamSoTaiKhoan(string tenDangNhap, string hash, string salt, string vaiTro, string? maGV, string? maHocVien)
        {
            return new[]
            {
                new SqlParameter("@TenDangNhap", SqlDbType.VarChar, 50) { Value = tenDangNhap },
                new SqlParameter("@MatKhauHash", SqlDbType.VarChar, 256) { Value = hash },
                new SqlParameter("@Salt", SqlDbType.VarChar, 64) { Value = salt },
                new SqlParameter("@VaiTro", SqlDbType.VarChar, 20) { Value = vaiTro },
                new SqlParameter("@MaGV", SqlDbType.VarChar, 20) { Value = (object?)maGV ?? DBNull.Value },
                new SqlParameter("@MaHocVien", SqlDbType.VarChar, 20) { Value = (object?)maHocVien ?? DBNull.Value }
            };
        }
    }
}
