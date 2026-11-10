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
        private static string ConnectDBStr => ConfigurationManager.ConnectionStrings["DB"]?.ConnectionString
            ?? throw new InvalidOperationException("Chưa cấu hình connection string 'DB' trong App.config.");

        public static string DataSource => new SqlConnectionStringBuilder(ConnectDBStr).DataSource;

        //dugn cho select
        public static DataTable ExecuteQuery(string sql, params SqlParameter[] parameters)
        {
            using (var conn = new SqlConnection(ConnectDBStr))
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
            using (var conn = new SqlConnection(ConnectDBStr))
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
                using (var conn = new SqlConnection(ConnectDBStr))
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
