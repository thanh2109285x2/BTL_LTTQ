using BTL_LTTQ.Common;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BTL_LTTQ.DataAccessLayer
{
    public class TaiKhoanDAL
    {
        public bool CoAdmin()
        {
            object? ketQua = DbHelper.ExecuteScalar(
                "SELECT CASE WHEN EXISTS (SELECT 1 FROM TaiKhoan WHERE VaiTro = @VaiTro) THEN 1 ELSE 0 END",
                new SqlParameter("@VaiTro", SqlDbType.VarChar, 20) { Value = VaiTroConst.Admin });

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

        public string? LayDienThoai(string vaiTro, string ma)
        {
            string? sql = vaiTro switch
            {
                VaiTroConst.GiaoVien => "SELECT DienThoai FROM GiaoVien WHERE MaGV = @Ma",
                VaiTroConst.HocVien => "SELECT DienThoai FROM HocVien WHERE MaHocVien = @Ma",
                _ => null
            };

            if (sql == null) return null;

            return DbHelper.ExecuteScalar(sql,
                new SqlParameter("@Ma", SqlDbType.VarChar, 20) { Value = ma }) as string;
        }

        public bool TenDangNhapDaTonTai(string tenDangNhap)
        {
            object? ketQua = DbHelper.ExecuteScalar(
                "SELECT CASE WHEN EXISTS (SELECT 1 FROM TaiKhoan WHERE TenDangNhap = @TenDangNhap) THEN 1 ELSE 0 END",
                new SqlParameter("@TenDangNhap", SqlDbType.VarChar, 50) { Value = tenDangNhap });

            return Convert.ToInt32(ketQua) == 1;
        }

        public string ThemTaiKhoan(string tenDangNhap, string hash, string salt, string vaiTro, string? maGV, string? maHocVien)
        {
            try
            {
                int soDong;
                if (vaiTro == VaiTroConst.Admin)
                {
                    const string sqlAdmin = @"INSERT INTO TaiKhoan (TenDangNhap, MatKhauHash, Salt, VaiTro, MaGV, MaHocVien)
                                              SELECT @TenDangNhap, @MatKhauHash, @Salt, @VaiTro, @MaGV, @MaHocVien
                                              WHERE NOT EXISTS (
                                                  SELECT 1 FROM TaiKhoan WITH (UPDLOCK, HOLDLOCK) WHERE VaiTro = @VaiTro)";
                    soDong = DbHelper.ExecuteNonQuery(sqlAdmin, ThamSoTaiKhoan(tenDangNhap, hash, salt, vaiTro, maGV, maHocVien));
                    if (soDong == 0) return "Tài khoản Admin đã tồn tại.";
                }
                else
                {
                    const string sql = @"INSERT INTO TaiKhoan (TenDangNhap, MatKhauHash, Salt, VaiTro, MaGV, MaHocVien)
                                         VALUES (@TenDangNhap, @MatKhauHash, @Salt, @VaiTro, @MaGV, @MaHocVien)";
                    soDong = DbHelper.ExecuteNonQuery(sql, ThamSoTaiKhoan(tenDangNhap, hash, salt, vaiTro, maGV, maHocVien));
                }

                return soDong == 1 ? "" : "Không thể tạo tài khoản.";
            }
            catch (SqlException ex) when (ex.Number is 2627 or 2601)
            {
                return "Tên đăng nhập hoặc hồ sơ đã được đăng ký tài khoản.";
            }
            catch (SqlException)
            {
                return "Không thể tạo tài khoản do lỗi cơ sở dữ liệu.";
            }
        }

        public DataTable LayTheoTenDangNhap(string tenDangNhap)
        {
            const string sql = @"SELECT MatKhauHash, Salt, VaiTro, MaGV, MaHocVien, TrangThai
                                 FROM TaiKhoan
                                 WHERE TenDangNhap = @TenDangNhap";
            return DbHelper.ExecuteQuery(sql,
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
