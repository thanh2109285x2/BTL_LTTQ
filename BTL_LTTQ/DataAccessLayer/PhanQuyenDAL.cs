using System.Data;
using Microsoft.Data.SqlClient;

namespace BTL_LTTQ.DataAccessLayer
{
    public class PhanQuyenDAL
    {
        public DataTable LayTheoVaiTro(string vaiTro)
        {
            const string sql = @"SELECT MaChucNang, CoQuyen, ChiCuaMinh
                                 FROM PhanQuyen
                                 WHERE VaiTro = @VaiTro";

            return DbHelper.ExecuteQuery(sql,
                new SqlParameter("@VaiTro", SqlDbType.VarChar, 20) { Value = vaiTro });
        }
    }
}
