using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace BTL_LTTQ.DAL
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
                BACKUP DATABASE [QuanLyLopTinHocVaNgoaiNguNganHan]
                TO DISK = @DuongDanBackup
                WITH INIT;", conn);

            cmd.Parameters.Add(new SqlParameter("@DuongDanBackup", SqlDbType.NVarChar, 260) { Value = duongDanBackup });
            cmd.CommandTimeout = 0;
            conn.Open();
            cmd.ExecuteNonQuery();
            loi = "";
        }

        public void ThucRestore(string duongDanBackup, out string loi)
        {
            var csb = new SqlConnectionStringBuilder(DbHelper.ConnectionString)
            {
                InitialCatalog = "master"
            };
            using var conn = new SqlConnection(csb.ConnectionString);
            using var cmd = new SqlCommand(@"
                BEGIN TRY
                    ALTER DATABASE [QuanLyLopTinHocVaNgoaiNguNganHan]
                        SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                    RESTORE DATABASE [QuanLyLopTinHocVaNgoaiNguNganHan]
                        FROM DISK = @DuongDanBackup WITH REPLACE;
                    ALTER DATABASE [QuanLyLopTinHocVaNgoaiNguNganHan] SET MULTI_USER;
                END TRY
                BEGIN CATCH
                    IF DB_ID(N'QuanLyLopTinHocVaNgoaiNguNganHan') IS NOT NULL
                        ALTER DATABASE [QuanLyLopTinHocVaNgoaiNguNganHan] SET MULTI_USER;
                    THROW;
                END CATCH;", conn);

            cmd.Parameters.Add(new SqlParameter("@DuongDanBackup", SqlDbType.NVarChar, 260) { Value = duongDanBackup });
            cmd.CommandTimeout = 0;
            conn.Open();
            cmd.ExecuteNonQuery();
            loi = "";
        }
    }
}
