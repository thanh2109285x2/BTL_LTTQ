using BTL_LTTQ.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace BTL_LTTQ.DAL
{
    public class LopHocRepository
    {
        public DataTable LayDanhSach(string tuKhoa, PhamViInfo phamVi)
        {
            var (dieuKienSql, thamSoPhamVi) = PhamViSqlBuilder.ChoLopHoc(phamVi, "l");
            string dieuKienPhamVi = dieuKienSql == "" ? "" : " AND " + dieuKienSql;
            string sql = $@"SELECT l.MaLop, l.TenLop,
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
                            WHERE (@TuKhoa = '' OR l.TenLop COLLATE Vietnamese_CI_AI LIKE '%' + @TuKhoa + '%'){dieuKienPhamVi}
                           ORDER BY l.NgayBD DESC";

            var thamSo = new List<SqlParameter>
            {
                new SqlParameter("@TuKhoa", tuKhoa)
            };
            thamSo.AddRange(thamSoPhamVi);

            return DbHelper.ExecuteQuery(sql, thamSo.ToArray());
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
            string sql = @"INSERT INTO LopHoc (MaLop, TenLop, MaMon, MaGV, NgayBD, NgayKT, MaCa, MaThoiGian, MaPhong, SiSoToiDa, SoBuoiTuan)
                           VALUES (@MaLop, @TenLop, @MaMon, @MaGV, @NgayBD, @NgayKT, @MaCa, @MaThoiGian, @MaPhong, @SiSoToiDa, @SoBuoiTuan)";
            DbHelper.ExecuteNonQuery(sql, ThamSo(l));
        }

        public int Sua(LopHoc l, PhamViInfo phamVi, bool choPhepSuaGiaoVien)
        {
            var (dieuKienSql, thamSoPhamVi) = PhamViSqlBuilder.ChoLopHoc(phamVi, "l");
            string dieuKienPhamVi = dieuKienSql == "" ? "" : " AND " + dieuKienSql;
            string cotGiaoVien = choPhepSuaGiaoVien ? "MaGV = @MaGV, " : "";
            string sql = $@"UPDATE l
                            SET TenLop = @TenLop, MaMon = @MaMon, {cotGiaoVien}
                                NgayBD = @NgayBD, NgayKT = @NgayKT,
                                MaCa = @MaCa, MaThoiGian = @MaThoiGian, MaPhong = @MaPhong,
                                SiSoToiDa = @SiSoToiDa, SoBuoiTuan = @SoBuoiTuan
                            FROM LopHoc l
                            WHERE l.MaLop = @MaLop{dieuKienPhamVi}";

            var thamSo = new List<SqlParameter>(ThamSo(l));
            thamSo.AddRange(thamSoPhamVi);

            return DbHelper.ExecuteNonQuery(sql, thamSo.ToArray());
        }

        public void Xoa(string maLop)
        {
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