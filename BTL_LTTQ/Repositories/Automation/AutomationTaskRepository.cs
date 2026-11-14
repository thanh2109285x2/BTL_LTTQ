using BTL_LTTQ.DataAccessLayer;
using BTL_LTTQ.Models.Automation;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace BTL_LTTQ.Repositories.Automation
{
    /// <summary>
    /// Truy cập AutomationTask (nhiệm vụ do Automation tạo). Bộ lọc theo vai trò/tài khoản
    /// chỉ là phạm vi hiển thị, do Service truyền vào.
    /// </summary>
    public class AutomationTaskRepository
    {
        public DataTable LayDanhSach(string trangThai = "", string tuKhoa = "",
                                     string? locVaiTro = null, string? locTaiKhoan = null)
        {
            const string sql = @"
                SELECT t.TaskID, t.ExecutionID, t.RuleID, r.RuleName,
                       t.Title, t.Description, t.EntityType, t.EntityID,
                       l.TenLop,
                       t.AssignedToAccount, t.AssignedToRole, t.Priority, t.Status,
                       t.DueDate, t.CreatedAt, t.CompletedAt, t.CompletedBy
                FROM dbo.AutomationTask t
                LEFT JOIN dbo.AutomationRule r ON r.RuleID = t.RuleID
                LEFT JOIN dbo.LopHoc l
                       ON t.EntityType = 'CLASS' AND l.MaLop = t.EntityID
                WHERE (@TrangThai = '' OR t.Status = @TrangThai)
                  AND (@TuKhoa = ''
                       OR t.Title COLLATE Vietnamese_CI_AI LIKE '%' + @TuKhoa + '%'
                       OR t.EntityID LIKE '%' + @TuKhoa + '%'
                       OR ISNULL(l.TenLop, '') COLLATE Vietnamese_CI_AI LIKE '%' + @TuKhoa + '%')
                  AND (@LocVaiTro IS NULL
                       OR t.AssignedToRole = @LocVaiTro
                       OR t.AssignedToAccount = @LocTaiKhoan)
                ORDER BY CASE t.Status
                             WHEN 'PENDING' THEN 0
                             WHEN 'IN_PROGRESS' THEN 1
                             WHEN 'COMPLETED' THEN 2
                             ELSE 3
                         END,
                         t.DueDate, t.CreatedAt DESC";

            return DbHelper.ExecuteQuery(sql,
                new SqlParameter("@TrangThai", SqlDbType.VarChar, 20) { Value = trangThai ?? "" },
                new SqlParameter("@TuKhoa", SqlDbType.NVarChar, 200) { Value = (tuKhoa ?? "").Trim() },
                new SqlParameter("@LocVaiTro", SqlDbType.VarChar, 20)
                {
                    Value = (object?)locVaiTro ?? DBNull.Value
                },
                new SqlParameter("@LocTaiKhoan", SqlDbType.VarChar, 50)
                {
                    Value = (object?)locTaiKhoan ?? DBNull.Value
                });
        }

        public AutomationTask? LayTheoID(int taskId)
        {
            DataTable dt = DbHelper.ExecuteQuery(
                @"SELECT TaskID, ExecutionID, RuleID, Title, Description, EntityType, EntityID,
                         AssignedToAccount, AssignedToRole, Priority, Status, DueDate,
                         CreatedAt, CompletedAt, CompletedBy, IsOpen
                  FROM dbo.AutomationTask WHERE TaskID = @TaskID",
                new SqlParameter("@TaskID", SqlDbType.Int) { Value = taskId });

            return dt.Rows.Count == 0 ? null : AutomationMapper.DocTask(dt.Rows[0]);
        }

        public int Them(AutomationTask t)
        {
            const string sql = @"
                INSERT INTO dbo.AutomationTask
                    (ExecutionID, RuleID, Title, Description, EntityType, EntityID,
                     AssignedToAccount, AssignedToRole, Priority, Status, DueDate)
                OUTPUT INSERTED.TaskID
                VALUES (@ExecutionID, @RuleID, @Title, @Description, @EntityType, @EntityID,
                        @AssignedToAccount, @AssignedToRole, @Priority, @Status, @DueDate)";

            object? ketQua = DbHelper.ExecuteScalar(sql,
                new SqlParameter("@ExecutionID", SqlDbType.BigInt)
                {
                    Value = (object?)t.ExecutionID ?? DBNull.Value
                },
                new SqlParameter("@RuleID", SqlDbType.Int) { Value = t.RuleID },
                new SqlParameter("@Title", SqlDbType.NVarChar, 200) { Value = t.Title },
                new SqlParameter("@Description", SqlDbType.NVarChar, 1000)
                {
                    Value = (object?)t.Description ?? DBNull.Value
                },
                new SqlParameter("@EntityType", SqlDbType.VarChar, 40) { Value = t.EntityType },
                new SqlParameter("@EntityID", SqlDbType.VarChar, 20) { Value = t.EntityID },
                new SqlParameter("@AssignedToAccount", SqlDbType.VarChar, 50)
                {
                    Value = (object?)t.AssignedToAccount ?? DBNull.Value
                },
                new SqlParameter("@AssignedToRole", SqlDbType.VarChar, 20)
                {
                    Value = (object?)t.AssignedToRole ?? DBNull.Value
                },
                new SqlParameter("@Priority", SqlDbType.VarChar, 10) { Value = t.Priority },
                new SqlParameter("@Status", SqlDbType.VarChar, 20) { Value = t.Status },
                new SqlParameter("@DueDate", SqlDbType.Date)
                {
                    Value = (object?)t.DueDate?.Date ?? DBNull.Value
                });

            return Convert.ToInt32(ketQua);
        }

        /// <summary>Cập nhật người xử lý / hạn xử lý / mức ưu tiên.</summary>
        public void CapNhatXuLy(int taskId, string? taiKhoan, string? vaiTro, DateTime? hanXuLy, string uuTien)
        {
            if (DbHelper.ExecuteNonQuery(
                @"UPDATE dbo.AutomationTask
                  SET AssignedToAccount = @AssignedToAccount,
                      AssignedToRole = @AssignedToRole,
                      DueDate = @DueDate,
                      Priority = @Priority
                  WHERE TaskID = @TaskID",
                new SqlParameter("@TaskID", SqlDbType.Int) { Value = taskId },
                new SqlParameter("@AssignedToAccount", SqlDbType.VarChar, 50)
                {
                    Value = (object?)taiKhoan ?? DBNull.Value
                },
                new SqlParameter("@AssignedToRole", SqlDbType.VarChar, 20)
                {
                    Value = (object?)vaiTro ?? DBNull.Value
                },
                new SqlParameter("@DueDate", SqlDbType.Date)
                {
                    Value = (object?)hanXuLy?.Date ?? DBNull.Value
                },
                new SqlParameter("@Priority", SqlDbType.VarChar, 10) { Value = uuTien }) != 1)
            {
                throw new InvalidOperationException("Không cập nhật được nhiệm vụ: nhiệm vụ không tồn tại.");
            }
        }

        public void CapNhatTrangThai(int taskId, string trangThai, string? nguoiThucHien)
        {
            int soDong = DbHelper.ExecuteNonQuery(
                @"UPDATE dbo.AutomationTask
                  SET Status = @Status,
                      CompletedAt = CASE WHEN @Status = 'COMPLETED' THEN SYSDATETIME() ELSE NULL END,
                      CompletedBy = CASE WHEN @Status = 'COMPLETED' THEN @CompletedBy ELSE NULL END
                  WHERE TaskID = @TaskID",
                new SqlParameter("@TaskID", SqlDbType.Int) { Value = taskId },
                new SqlParameter("@Status", SqlDbType.VarChar, 20) { Value = trangThai },
                new SqlParameter("@CompletedBy", SqlDbType.VarChar, 50)
                {
                    Value = (object?)nguoiThucHien ?? DBNull.Value
                });

            if (soDong != 1)
                throw new InvalidOperationException("Không cập nhật được nhiệm vụ: nhiệm vụ không tồn tại.");
        }

        /// <summary>Danh sách tài khoản đang hoạt động để giao nhiệm vụ.</summary>
        public DataTable LayTaiKhoan()
        {
            return DbHelper.ExecuteQuery(
                @"SELECT TenDangNhap, VaiTro
                  FROM dbo.TaiKhoan
                  WHERE TrangThai = 1
                  ORDER BY CASE VaiTro WHEN 'Admin' THEN 0 WHEN 'GiaoVien' THEN 1 ELSE 2 END,
                           TenDangNhap");
        }

        public int DemDangCho(string? locVaiTro = null, string? locTaiKhoan = null)
        {
            object? ketQua = DbHelper.ExecuteScalar(
                @"SELECT COUNT(*) FROM dbo.AutomationTask
                  WHERE Status IN ('PENDING','IN_PROGRESS')
                    AND (@LocVaiTro IS NULL
                         OR AssignedToRole = @LocVaiTro
                         OR AssignedToAccount = @LocTaiKhoan)",
                new SqlParameter("@LocVaiTro", SqlDbType.VarChar, 20)
                {
                    Value = (object?)locVaiTro ?? DBNull.Value
                },
                new SqlParameter("@LocTaiKhoan", SqlDbType.VarChar, 50)
                {
                    Value = (object?)locTaiKhoan ?? DBNull.Value
                });

            return ketQua == null || ketQua == DBNull.Value ? 0 : Convert.ToInt32(ketQua);
        }
    }
}
