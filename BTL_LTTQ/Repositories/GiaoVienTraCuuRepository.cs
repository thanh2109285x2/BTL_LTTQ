using BTL_LTTQ.DataAccessLayer;
using BTL_LTTQ.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BTL_LTTQ.Repositories
{
    public class GiaoVienTraCuuRepository
    {
        public DataTable TimKiem(TimKiemGiaoVien tieuChi)
        {
            string sql = @"SELECT g.MaGV, g.TenGV, g.DienThoai,t.TenTrinhDo, c.TenChuyenMon,
                            (SELECT STRING_AGG(m.TenMon, ', ')
                            FROM GiaoVien_MonHoc gm
                            JOIN MonHoc m ON gm.MaMon = m.MaMon
                            WHERE gm.MaGV = g.MaGV) AS MonDay
                            FROM GiaoVien g
                            LEFT JOIN TrinhDo t   ON g.MaTrinhDo = t.MaTrinhDo
                            LEFT JOIN ChuyenMon c ON g.MaChuyenMon = c.MaChuyenMon
                            WHERE (@TenGV = '' OR g.TenGV COLLATE Vietnamese_CI_AI LIKE '%' + @TenGV + '%')
                                AND (@MaTrinhDo   IS NULL OR g.MaTrinhDo   = @MaTrinhDo)
                                AND (@MaChuyenMon IS NULL OR g.MaChuyenMon = @MaChuyenMon)
                                AND (@MaMon IS NULL OR EXISTS(
                                    SELECT 1 FROM GiaoVien_MonHoc x
                                    WHERE x.MaGV = g.MaGV AND x.MaMon = @MaMon))
                            ORDER BY g.TenGV";

            return DbHelper.ExecuteQuery(sql,
                new SqlParameter("@TenGV", tieuChi.TenGV),
                new SqlParameter("@MaTrinhDo", (object?)tieuChi.MaTrinhDo ?? DBNull.Value),
                new SqlParameter("@MaChuyenMon", (object?)tieuChi.MaChuyenMon ?? DBNull.Value),
                new SqlParameter("@MaMon", (object?)tieuChi.MaMon ?? DBNull.Value));
        }
    }
}
