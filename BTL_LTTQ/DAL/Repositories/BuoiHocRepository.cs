using Microsoft.Data.SqlClient;
using System.Data;

namespace BTL_LTTQ.DAL
{
    public class BuoiHocRepository
    {
        public int XoaTatCaCuaLop(SqlConnection conn, SqlTransaction tran, string maLop)
        {
            using var cmd = new SqlCommand(
                "DELETE FROM dbo.BuoiHoc WHERE MaLop = @MaLop", conn, tran);
            cmd.Parameters.Add(new SqlParameter("@MaLop", SqlDbType.VarChar, 20) { Value = maLop });
            return cmd.ExecuteNonQuery();
        }
    }
}
