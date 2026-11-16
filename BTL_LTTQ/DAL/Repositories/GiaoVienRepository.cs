using BTL_LTTQ.DAL;
using BTL_LTTQ.Common;
using BTL_LTTQ.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BTL_LTTQ.DAL
{
    public class GiaoVienRepository
    {
        public DataTable LayDanhSach(string tuKhoa = "")
        {
            bool chiCuaMinh = CurrentUser.IsGiaoVien &&
                ((PermissionService.Can(ChucNangConst.GIAOVIEN_SUA) && PermissionService.ChiCuaMinh(ChucNangConst.GIAOVIEN_SUA)) ||
                 PermissionService.ChiCuaMinh(ChucNangConst.GIAOVIEN_XEM));
            string dieuKienCaNhan = chiCuaMinh ? " AND g.MaGV = @MaGVHienTai" : "";
            const string sql = @"SELECT g.MaGV, g.TenGV, g.NgaySinh, g.GioiTinh, g.DiaChi, g.DienThoai,
                                        g.MaChucDanh, cd.TenChucDanh,
                                        g.MaChuyenMon, cm.TenChuyenMon,
                                        g.MaCQ, cq.TenCQ,
                                        g.MaTrinhDo, td.TenTrinhDo, g.Anh,
                                        STRING_AGG(m.TenMon, N', ') AS MonDay
                                 FROM dbo.GiaoVien g
                                 LEFT JOIN dbo.ChucDanh cd ON cd.MaChucDanh = g.MaChucDanh
                                 LEFT JOIN dbo.ChuyenMon cm ON cm.MaChuyenMon = g.MaChuyenMon
                                 LEFT JOIN dbo.CoQuan cq ON cq.MaCQ = g.MaCQ
                                 LEFT JOIN dbo.TrinhDo td ON td.MaTrinhDo = g.MaTrinhDo
                                 LEFT JOIN dbo.GiaoVien_MonHoc gm ON gm.MaGV = g.MaGV
                                 LEFT JOIN dbo.MonHoc m ON m.MaMon = gm.MaMon
                                 WHERE (@TuKhoa = '' OR g.MaGV COLLATE Vietnamese_CI_AI LIKE '%' + @TuKhoa + '%'
                                    OR g.TenGV COLLATE Vietnamese_CI_AI LIKE '%' + @TuKhoa + '%'
                                    OR g.DienThoai LIKE '%' + @TuKhoa + '%')";
            string sqlCoPhamVi = sql + dieuKienCaNhan + @"
                                 GROUP BY g.MaGV, g.TenGV, g.NgaySinh, g.GioiTinh, g.DiaChi, g.DienThoai,
                                          g.MaChucDanh, cd.TenChucDanh, g.MaChuyenMon, cm.TenChuyenMon,
                                          g.MaCQ, cq.TenCQ, g.MaTrinhDo, td.TenTrinhDo, g.Anh
                                 ORDER BY g.TenGV, g.MaGV";
            var thamSo = new List<SqlParameter> { new("@TuKhoa", tuKhoa) };
            if (chiCuaMinh)
                thamSo.Add(new SqlParameter("@MaGVHienTai", CurrentUser.MaGV ?? ""));
            return DbHelper.ExecuteQuery(sqlCoPhamVi, thamSo.ToArray());
        }

        public DataTable LayDanhMuc(string tenBang, string cotMa, string cotTen)
        {
            var allow = new Dictionary<string, (string Ma, string Ten)>(StringComparer.OrdinalIgnoreCase)
            {
                ["ChucDanh"] = ("MaChucDanh", "TenChucDanh"),
                ["ChuyenMon"] = ("MaChuyenMon", "TenChuyenMon"),
                ["CoQuan"] = ("MaCQ", "TenCQ"),
                ["TrinhDo"] = ("MaTrinhDo", "TenTrinhDo"),
                ["MonHoc"] = ("MaMon", "TenMon")
            };
            if (!allow.TryGetValue(tenBang, out var columns) || columns.Ma != cotMa || columns.Ten != cotTen)
                throw new ArgumentException("Danh mục không hợp lệ.");

            return DbHelper.ExecuteQuery($"SELECT {columns.Ma}, {columns.Ten} FROM dbo.{tenBang} ORDER BY {columns.Ten}");
        }

        public List<string> LayMonDay(string maGV)
        {
            DataTable dt = DbHelper.ExecuteQuery(
                "SELECT MaMon FROM dbo.GiaoVien_MonHoc WHERE MaGV = @MaGV",
                new SqlParameter("@MaGV", maGV));
            return dt.Rows.Cast<DataRow>().Select(row => row["MaMon"].ToString() ?? "").ToList();
        }

        public bool TonTai(string maGV)
        {
            DataTable dt = DbHelper.ExecuteQuery("SELECT 1 FROM dbo.GiaoVien WHERE MaGV = @MaGV", new SqlParameter("@MaGV", maGV));
            return dt.Rows.Count > 0;
        }

        public void Them(GiaoVien g)
        {
            PermissionService.YeuCau(ChucNangConst.GIAOVIEN_THEM, "Bạn không có quyền thêm giáo viên.");
            DbHelper.ExecuteTransaction((conn, tran) =>
            {
                using var cmd = new SqlCommand(@"INSERT dbo.GiaoVien
                    (MaGV, TenGV, NgaySinh, GioiTinh, DiaChi, DienThoai, MaChucDanh, MaChuyenMon, MaCQ, MaTrinhDo, Anh)
                    VALUES (@MaGV, @TenGV, @NgaySinh, @GioiTinh, @DiaChi, @DienThoai, @MaChucDanh, @MaChuyenMon, @MaCQ, @MaTrinhDo, @Anh)", conn, tran);
                AddTeacherParameters(cmd, g);
                cmd.ExecuteNonQuery();
                LuuMonDay(conn, tran, g.MaGV, g.MaMonDay);
            });
        }

        public void Sua(GiaoVien g)
        {
            if (CurrentUser.IsGiaoVien && PermissionService.ChiCuaMinh(ChucNangConst.GIAOVIEN_SUA))
            {
                SuaHoSoCaNhan(g.MaGV, g.DienThoai, g.DiaChi, g.Anh);
                return;
            }
            PermissionService.YeuCau(ChucNangConst.GIAOVIEN_SUA, "Bạn không có quyền sửa giáo viên.");
            DbHelper.ExecuteTransaction((conn, tran) =>
            {
                using var cmd = new SqlCommand(@"UPDATE dbo.GiaoVien SET TenGV=@TenGV, NgaySinh=@NgaySinh,
                    GioiTinh=@GioiTinh, DiaChi=@DiaChi, DienThoai=@DienThoai, MaChucDanh=@MaChucDanh,
                    MaChuyenMon=@MaChuyenMon, MaCQ=@MaCQ, MaTrinhDo=@MaTrinhDo, Anh=@Anh
                    WHERE MaGV=@MaGV", conn, tran);
                AddTeacherParameters(cmd, g);
                if (cmd.ExecuteNonQuery() != 1) throw new InvalidOperationException("Không tìm thấy giáo viên cần sửa.");
                using var del = new SqlCommand("DELETE FROM dbo.GiaoVien_MonHoc WHERE MaGV=@MaGV", conn, tran);
                del.Parameters.AddWithValue("@MaGV", g.MaGV);
                del.ExecuteNonQuery();
                LuuMonDay(conn, tran, g.MaGV, g.MaMonDay);
            });
        }

        public void SuaHoSoCaNhan(string maGV, string? dienThoai, string? diaChi, string? anh)
        {
            PermissionService.YeuCau(ChucNangConst.GIAOVIEN_SUA, "Bạn không có quyền sửa hồ sơ cá nhân.");
            if (!CurrentUser.IsGiaoVien || !PermissionService.ChiCuaMinh(ChucNangConst.GIAOVIEN_SUA) ||
                string.IsNullOrWhiteSpace(CurrentUser.MaGV) ||
                !string.Equals(maGV, CurrentUser.MaGV, StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException("Bạn chỉ được sửa hồ sơ của chính mình.");

            const string sql = @"UPDATE dbo.GiaoVien SET DienThoai=@DienThoai, DiaChi=@DiaChi, Anh=@Anh
                                 WHERE MaGV=@MaGV";
            int affected = DbHelper.ExecuteNonQuery(sql,
                new SqlParameter("@DienThoai", (object?)dienThoai ?? DBNull.Value),
                new SqlParameter("@DiaChi", (object?)diaChi ?? DBNull.Value),
                new SqlParameter("@Anh", (object?)anh ?? DBNull.Value),
                new SqlParameter("@MaGV", maGV));
            if (affected != 1) throw new InvalidOperationException("Không tìm thấy hồ sơ giáo viên.");
        }

        public void Xoa(string maGV)
        {
            PermissionService.YeuCau(ChucNangConst.GIAOVIEN_XOA, "Bạn không có quyền xóa giáo viên.");
            DbHelper.ExecuteTransaction((conn, tran) =>
            {
                using var delMon = new SqlCommand("DELETE FROM dbo.GiaoVien_MonHoc WHERE MaGV=@MaGV", conn, tran);
                delMon.Parameters.AddWithValue("@MaGV", maGV);
                delMon.ExecuteNonQuery();
                using var del = new SqlCommand("DELETE FROM dbo.GiaoVien WHERE MaGV=@MaGV", conn, tran);
                del.Parameters.AddWithValue("@MaGV", maGV);
                if (del.ExecuteNonQuery() != 1) throw new InvalidOperationException("Không tìm thấy giáo viên cần xóa.");
            });
        }

        private static void LuuMonDay(SqlConnection conn, SqlTransaction tran, string maGV, IEnumerable<string> maMons)
        {
            foreach (string maMon in maMons.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                using var cmd = new SqlCommand(
                    "INSERT dbo.GiaoVien_MonHoc (MaGV, MaMon) VALUES (@MaGV, @MaMon)", conn, tran);
                cmd.Parameters.AddWithValue("@MaGV", maGV);
                cmd.Parameters.AddWithValue("@MaMon", maMon);
                cmd.ExecuteNonQuery();
            }
        }

        private static void AddTeacherParameters(SqlCommand cmd, GiaoVien g)
        {
            cmd.Parameters.AddWithValue("@MaGV", g.MaGV);
            cmd.Parameters.AddWithValue("@TenGV", g.TenGV);
            cmd.Parameters.AddWithValue("@NgaySinh", (object?)g.NgaySinh ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@GioiTinh", (object?)g.GioiTinh ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@DiaChi", (object?)g.DiaChi ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@DienThoai", (object?)g.DienThoai ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@MaChucDanh", (object?)g.MaChucDanh ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@MaChuyenMon", (object?)g.MaChuyenMon ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@MaCQ", (object?)g.MaCQ ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@MaTrinhDo", (object?)g.MaTrinhDo ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Anh", (object?)g.Anh ?? DBNull.Value);
        }
    }
}
