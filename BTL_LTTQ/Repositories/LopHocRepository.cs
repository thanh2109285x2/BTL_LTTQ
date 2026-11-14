using BTL_LTTQ.DataAccessLayer;
using BTL_LTTQ.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BTL_LTTQ.Repositories
{
    public class LopHocRepository
    {
        public DataTable LayDanhSach(string tuKhoa = "")
        {
            string sql = @"SELECT l.MaLop, l.TenLop,
                                  l.MaMon, m.TenMon,
                                  l.MaGV, g.TenGV,
                                  l.NgayBD, l.NgayKT,
                                  l.MaCa, c.TenCa,
                                  l.MaThoiGian, t.TenThoiGian,
                                  l.MaPhong, p.TenPhong,
                                  l.SiSoToiDa, l.SoBuoiTuan,
                                  (SELECT COUNT(*) FROM DangKy d WHERE d.MaLop = l.MaLop) AS SiSo
                           FROM LopHoc l
                           LEFT JOIN MonHoc m   ON l.MaMon = m.MaMon
                           LEFT JOIN GiaoVien g ON l.MaGV = g.MaGV
                           LEFT JOIN CaHoc c    ON l.MaCa = c.MaCa
                           LEFT JOIN ThoiGian t ON l.MaThoiGian = t.MaThoiGian
                           LEFT JOIN PhongHoc p ON l.MaPhong = p.MaPhong
                           WHERE @TuKhoa = '' OR l.TenLop COLLATE Vietnamese_CI_AI LIKE '%' + @TuKhoa + '%'
                           ORDER BY l.NgayBD DESC";

            return DbHelper.ExecuteQuery(sql, new SqlParameter("@TuKhoa", tuKhoa));
        }

        public bool TonTai(string maLop)
        {
            DataTable dt = DbHelper.ExecuteQuery(
                "SELECT 1 FROM LopHoc WHERE MaLop = @MaLop",
                new SqlParameter("@MaLop", maLop));
            return dt.Rows.Count > 0;
        }

        public void Them(LopHoc l)
        {
            PermissionService.YeuCau(ChucNangConst.LOPHOC_THEM, "Bạn không có quyền thêm lớp học.");

            if (CurrentUser.IsHocVien)
                throw new UnauthorizedAccessException("Học viên không được thêm lớp học.");

            string sql = @"INSERT INTO LopHoc (MaLop, TenLop, MaMon, MaGV, NgayBD, NgayKT, MaCa, MaThoiGian, MaPhong, SiSoToiDa, SoBuoiTuan)
                           VALUES (@MaLop, @TenLop, @MaMon, @MaGV, @NgayBD, @NgayKT, @MaCa, @MaThoiGian, @MaPhong, @SiSoToiDa, @SoBuoiTuan)";
            DbHelper.ExecuteNonQuery(sql, ThamSo(l));
        }

        public void Sua(LopHoc l)
        {
<<<<<<< Updated upstream
            string sql = @"UPDATE LopHoc
                           SET TenLop = @TenLop, MaMon = @MaMon, MaGV = @MaGV,
                               NgayBD = @NgayBD, NgayKT = @NgayKT,
                               MaCa = @MaCa, MaThoiGian = @MaThoiGian, MaPhong = @MaPhong
                           WHERE MaLop = @MaLop";
            DbHelper.ExecuteNonQuery(sql, ThamSo(l));
=======
            if (!PermissionService.Can(ChucNangConst.LOPHOC_SUA))
                throw new UnauthorizedAccessException("Bạn không có quyền sửa lớp học.");

            if (CurrentUser.IsHocVien)
                throw new UnauthorizedAccessException("Học viên không được sửa lớp học.");

            bool chiCuaMinh = PermissionService.ChiCuaMinh(ChucNangConst.LOPHOC_SUA);
            var phamVi = PhamViDuLieu.ChoLopHoc(ChucNangConst.LOPHOC_SUA, "l");
            string dieuKienPhamVi = phamVi.DieuKienSql == "" ? "" : " AND " + phamVi.DieuKienSql;
            string cotGiaoVien = chiCuaMinh ? "" : "MaGV = @MaGV, ";
            string sql = $@"UPDATE l
                            SET TenLop = @TenLop, MaMon = @MaMon, {cotGiaoVien}
                                NgayBD = @NgayBD, NgayKT = @NgayKT,
                                MaCa = @MaCa, MaThoiGian = @MaThoiGian, MaPhong = @MaPhong,
                                SiSoToiDa = @SiSoToiDa, SoBuoiTuan = @SoBuoiTuan
                            FROM LopHoc l
                            WHERE l.MaLop = @MaLop{dieuKienPhamVi}";

            var thamSo = new List<SqlParameter>(ThamSo(l));
            thamSo.AddRange(phamVi.ThamSo);

            if (DbHelper.ExecuteNonQuery(sql, thamSo.ToArray()) != 1)
                throw new InvalidOperationException("Không thể cập nhật: lớp không tồn tại hoặc không thuộc quyền của bạn");
>>>>>>> Stashed changes
        }

        public void Xoa(string maLop)
        {
            PermissionService.YeuCau(ChucNangConst.LOPHOC_XOA, "Bạn không có quyền xóa lớp học.");

            if (CurrentUser.IsHocVien)
                throw new UnauthorizedAccessException("Học viên không được xóa lớp học.");

            DbHelper.ExecuteNonQuery("DELETE FROM LopHoc WHERE MaLop = @MaLop",
                                     new SqlParameter("@MaLop", maLop));
        }
        private static SqlParameter[] ThamSo(LopHoc l)
        {
            return new[]
            {
                new SqlParameter("@MaLop", l.MaLop),
                new SqlParameter("@TenLop", l.TenLop),
                new SqlParameter("@MaMon", l.MaMon),
                new SqlParameter("@MaGV", l.MaGV),
                new SqlParameter("@NgayBD", l.NgayBD),
                new SqlParameter("@NgayKT", l.NgayKT),
                new SqlParameter("@MaCa", l.MaCa),
                new SqlParameter("@MaThoiGian", l.MaThoiGian),
                new SqlParameter("@MaPhong", l.MaPhong),
                new SqlParameter("@SiSoToiDa", SqlDbType.SmallInt) { Value = l.SiSoToiDa },
                new SqlParameter("@SoBuoiTuan", SqlDbType.TinyInt) { Value = l.SoBuoiTuan }
            };
        }
    }
}