using BTL_LTTQ.Common;
using BTL_LTTQ.DAL;
using System;

namespace BTL_LTTQ.BUS
{
    // Khai báo các chức năng sao lưu và khôi phục cơ sở dữ liệu.

    public interface IDatabaseBackupService
    {
        string ThucBackup(string duongDanBackup);
        string ThucRestore(string duongDanBackup);
    }

    // Điều phối sao lưu và khôi phục database.
    public class DatabaseBackupService : IDatabaseBackupService
    {
        private readonly DatabaseBackupDAL _dal = new DatabaseBackupDAL();

        public string ThucBackup(string duongDanBackup)
        {
            if (!PermissionService.Can(ChucNangConst.DATABASE_BACKUP))
                return "Bạn không có quyền sao lưu database.";

            try
            {
                _dal.ThucBackup(duongDanBackup, out string loi);
                return loi;
            }
            catch (Exception ex)
            {
                return "Không sao lưu được database: " + ex.Message;
            }
        }

        public string ThucRestore(string duongDanBackup)
        {
            try
            {
                _dal.ThucRestore(duongDanBackup, out string loi);
                return loi;
            }
            catch (Exception ex)
            {
                return "Không khôi phục được database: " + ex.Message;
            }
        }
    }
}
