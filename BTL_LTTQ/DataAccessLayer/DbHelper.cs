using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;

namespace BTL_LTTQ.DataAccessLayer
{
    public static class DbHelper
    {
        private static readonly string _connectDBStr = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

        //dugn cho select
        public static DataTable ExecuteQuery(string sql, params SqlParameter[] parameters)
        {
            using (var conn = new SqlConnection(_connectDBStr))
            using (var cmd = new SqlCommand(sql, conn))
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                var dt = new DataTable();
                new SqlDataAdapter(cmd).Fill(dt);
                return dt;
            }
        }

        //dung cho cac lenh khac
        public static int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            using (var conn = new SqlConnection(_connectDBStr))
            using (var cmd = new SqlCommand(sql, conn))
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public static bool TestConnection(out string message)
        {
            try
            {
                using (var conn = new SqlConnection(_connectDBStr))
                {
                    conn.Open();
                    message = "ket noi thanh cong";
                    return true;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }
    }
}
