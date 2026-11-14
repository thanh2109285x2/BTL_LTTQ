using BTL_LTTQ.DataAccessLayer;
using System;

namespace BTL_LTTQ.Services
{
    /// <summary>
    /// Điều phối sao lưu và khôi phục database.
    /// </summary>
    public class DatabaseBackupService
    {
        private readonly DatabaseBackupDAL _dal = new DatabaseBackupDAL();

        public string ThucBackup(string duongDanBackup)
        {
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
