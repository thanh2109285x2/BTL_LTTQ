using BTL_LTTQ.DataAccessLayer;
using BTL_LTTQ.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BTL_LTTQ.Repositories
{
    public class LopHocTraCuuRepository
    {
        public DataTable TimKiem(TimKiemLopHoc tieuChi)
        {
            string sql = @"SELECT * FROM(
                            SELECT l.MaLop, l.TenLop, m.TenMon, g.TenGV, l.NgayBD, l.NgayKT,
                                   (SELECT COUNT(*) FROM HocVien d WHERE d.MaLop = l.MaLop) AS SiSo
                            FROM LopHoc l
                            LEFT JOIN MonHoc m ON l.MaMon = m.MaMon
                            LEFT JOIN GiaoVien g ON l.MaGV = g.MaGV
                            WHERE (@MaMon IS NULL OR l.MaMon = @MaMon) AND (@MaGV IS NULL OR l.MaGV = @MaGV)) x
                            WHERE x.SiSo BETWEEN @SiSoTu AND @SiSoDen
                            ORDER BY x.NgayBD DESC";

            return DbHelper.ExecuteQuery(sql,
                new SqlParameter("@MaMon", (object?)tieuChi.MaMon ?? DBNull.Value),
                new SqlParameter("@MaGV", (object?)tieuChi.MaGV ?? DBNull.Value),
                new SqlParameter("@SiSoTu", tieuChi.SiSoTu),
                new SqlParameter("@SiSoDen", tieuChi.SiSoDen));
        }
    }
}
