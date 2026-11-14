using BTL_LTTQ.DataAccessLayer;
using BTL_LTTQ.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BTL_LTTQ.Repositories
{
    public class MonHocRepository
    {
        public DataTable LayDanhSach(string tuKhoa = "")
        {
            string sql = @"SELECT MaMon, TenMon, HocPhi
                           FROM MonHoc
                           WHERE @TuKhoa = ''
                              OR MaMon COLLATE Vietnamese_CI_AI LIKE '%' + @TuKhoa + '%'
                              OR TenMon COLLATE Vietnamese_CI_AI LIKE '%' + @TuKhoa + '%'
                           ORDER BY MaMon";

            return DbHelper.ExecuteQuery(sql, new SqlParameter("@TuKhoa", tuKhoa));
        }

        public bool TonTai(string maMon)
        {
            DataTable dt = DbHelper.ExecuteQuery(
                "SELECT 1 FROM MonHoc WHERE MaMon = @MaMon",
                new SqlParameter("@MaMon", maMon));
            return dt.Rows.Count > 0;
        }

        public void Them(MonHoc m)
        {
            string sql = @"INSERT INTO MonHoc (MaMon, TenMon, HocPhi)
                           VALUES (@MaMon, @TenMon, @HocPhi)";
            DbHelper.ExecuteNonQuery(sql, ThamSo(m));
        }

        public void Sua(MonHoc m)
        {
            string sql = @"UPDATE MonHoc
                           SET TenMon = @TenMon, HocPhi = @HocPhi
                           WHERE MaMon = @MaMon";
            DbHelper.ExecuteNonQuery(sql, ThamSo(m));
        }

        public void Xoa(string maMon)
        {
            DbHelper.ExecuteNonQuery("DELETE FROM MonHoc WHERE MaMon = @MaMon",
                                     new SqlParameter("@MaMon", maMon));
        }

        private static SqlParameter[] ThamSo(MonHoc m)
        {
            return new[]
            {
                new SqlParameter("@MaMon", m.MaMon),
                new SqlParameter("@TenMon", m.TenMon),
                new SqlParameter("@HocPhi", (object?)m.HocPhi ?? DBNull.Value)
            };
        }
    }
}
