using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace BTL_LTTQ.DataAccessLayer
{
    /// <summary>
    /// Thao tác sao lưu / khôi phục database bằng SQL Server BACKUP / RESTORE.
    /// </summary>
    public class DatabaseBackupDAL
    {
        private const string DatabaseName = "QuanLyLopTinHocVaNgoaiNguNganHan";

        public void ThucBackup(string duongDanBackup, out string loi)
        {
            using var conn = new SqlConnection(DbHelper.ConnectionString);
            using var cmd = new SqlCommand(@"
                BACKUP DATABASE QuanLyLopTinHocVaNgoaiNguNganHan
                TO DATABASE = @DuongDanBackup
                WITH INIT, COMPRESSION;", conn);

            cmd.Parameters.Add(new SqlParameter("@DuongDanBackup", SqlDbType.NVarChar, 260) { Value = duongDanBackup });
            conn.Open();
            cmd.ExecuteNonQuery();
            loi = "";
        }

        public void ThucRestore(string duongDanBackup, out string loi)
        {
            using var conn = new SqlConnection(DbHelper.ConnectionString);
            using var cmd = new SqlCommand(@"
                RESTORE DATABASE QuanLyLopTinHocVaNgoaiNguNganHan
                FROM DATABASE = @DuongDanBackup
                WITH REPLACE;", conn);

            cmd.Parameters.Add(new SqlParameter("@DuongDanBackup", SqlDbType.NVarChar, 260) { Value = duongDanBackup });
            conn.Open();
            cmd.ExecuteNonQuery();
            loi = "";
        }
    }
}
