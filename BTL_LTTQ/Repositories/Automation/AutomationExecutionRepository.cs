using BTL_LTTQ.DataAccessLayer;
using BTL_LTTQ.Models.Automation;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace BTL_LTTQ.Repositories.Automation
{
    /// <summary>
    /// Lưu lịch sử thực thi và log từng bước. Lịch sử là bất biến: không sửa/xóa khi Rule thay đổi.
    /// Không kiểm tra quyền (xem AutomationRuleRepository).
    /// </summary>
    public class AutomationExecutionRepository
    {
        /// <summary>Sự kiện đã được xử lý cho Rule này chưa (chống trùng khi bị phát lại).</summary>
        public bool DaTonTaiDedup(int ruleId, string dedupKey)
        {
            object? ketQua = DbHelper.ExecuteScalar(
                @"SELECT TOP 1 1 FROM dbo.AutomationExecution
                  WHERE RuleID = @RuleID AND DedupKey = @DedupKey",
                new SqlParameter("@RuleID", SqlDbType.Int) { Value = ruleId },
                new SqlParameter("@DedupKey", SqlDbType.Char, 64) { Value = dedupKey });

            return ketQua != null && ketQua != DBNull.Value;
        }

        /// <summary>Rule có lần chạy thành công nào trong khoảng thời gian chờ chưa.</summary>
        public bool TrongThoiGianCho(int ruleId, string entityType, string entityId, int cooldownMinutes)
        {
            if (cooldownMinutes <= 0) return false;

            object? ketQua = DbHelper.ExecuteScalar(
                @"SELECT TOP 1 1 FROM dbo.AutomationExecution
                  WHERE RuleID = @RuleID
                    AND EntityType = @EntityType
                    AND EntityID = @EntityID
                    AND Status IN ('SUCCESS','PARTIAL','RUNNING')
                    AND StartedAt >= DATEADD(MINUTE, -@CooldownMinutes, SYSDATETIME())",
                new SqlParameter("@RuleID", SqlDbType.Int) { Value = ruleId },
                new SqlParameter("@EntityType", SqlDbType.VarChar, 40) { Value = entityType },
                new SqlParameter("@EntityID", SqlDbType.VarChar, 20) { Value = entityId },
                new SqlParameter("@CooldownMinutes", SqlDbType.Int) { Value = cooldownMinutes });

            return ketQua != null && ketQua != DBNull.Value;
        }

        /// <summary>
        /// Tạo dòng thực thi. Trả về ExecutionID, hoặc 0 nếu vi phạm ràng buộc chống trùng
        /// (UX_AutomationExecution_Dedup) do hai sự kiện đến gần như đồng thời.
        /// </summary>
        public long Them(AutomationExecution e)
        {
            const string sql = @"
                INSERT INTO dbo.AutomationExecution
                    (RuleID, RuleNameSnapshot, TriggerType, EntityType, EntityID, EventToken, DedupKey,
                     Status, ConditionsMatched, StartedAt, TriggeredBy)
                OUTPUT INSERTED.ExecutionID
                VALUES (@RuleID, @RuleNameSnapshot, @TriggerType, @EntityType, @EntityID, @EventToken, @DedupKey,
                        @Status, 0, SYSDATETIME(), @TriggeredBy)";

            try
            {
                object? ketQua = DbHelper.ExecuteScalar(sql,
                    new SqlParameter("@RuleID", SqlDbType.Int) { Value = e.RuleID },
                    new SqlParameter("@RuleNameSnapshot", SqlDbType.NVarChar, 150) { Value = e.RuleNameSnapshot },
                    new SqlParameter("@TriggerType", SqlDbType.VarChar, 40) { Value = e.TriggerType },
                    new SqlParameter("@EntityType", SqlDbType.VarChar, 40) { Value = e.EntityType },
                    new SqlParameter("@EntityID", SqlDbType.VarChar, 20) { Value = e.EntityID },
                    new SqlParameter("@EventToken", SqlDbType.VarChar, 200) { Value = e.EventToken },
                    new SqlParameter("@DedupKey", SqlDbType.Char, 64) { Value = e.DedupKey },
                    new SqlParameter("@Status", SqlDbType.VarChar, 20) { Value = e.Status },
                    new SqlParameter("@TriggeredBy", SqlDbType.VarChar, 50)
                    {
                        Value = (object?)e.TriggeredBy ?? DBNull.Value
                    });

                return ketQua == null || ketQua == DBNull.Value ? 0 : Convert.ToInt64(ketQua);
            }
            catch (SqlException ex) when (ex.Number is 2601 or 2627)
            {
                return 0;
            }
        }

        /// <summary>Cập nhật kết quả cuối cùng của một lần thực thi.</summary>
        public void CapNhatKetQua(AutomationExecution e)
        {
            DbHelper.ExecuteNonQuery(
                @"UPDATE dbo.AutomationExecution
                  SET Status = @Status,
                      ConditionsMatched = @ConditionsMatched,
                      ActionSuccessCount = @ActionSuccessCount,
                      ActionFailCount = @ActionFailCount,
                      FinishedAt = SYSDATETIME(),
                      DurationMs = DATEDIFF(MILLISECOND, StartedAt, SYSDATETIME()),
                      ErrorMessage = @ErrorMessage,
                      ContextJson = @ContextJson
                  WHERE ExecutionID = @ExecutionID",
                new SqlParameter("@ExecutionID", SqlDbType.BigInt) { Value = e.ExecutionID },
                new SqlParameter("@Status", SqlDbType.VarChar, 20) { Value = e.Status },
                new SqlParameter("@ConditionsMatched", SqlDbType.Bit) { Value = e.ConditionsMatched },
                new SqlParameter("@ActionSuccessCount", SqlDbType.Int) { Value = e.ActionSuccessCount },
                new SqlParameter("@ActionFailCount", SqlDbType.Int) { Value = e.ActionFailCount },
                new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 2000)
                {
                    Value = (object?)CatChuoi(e.ErrorMessage, 2000) ?? DBNull.Value
                },
                new SqlParameter("@ContextJson", SqlDbType.NVarChar, -1)
                {
                    Value = (object?)e.ContextJson ?? DBNull.Value
                });
        }

        public void ThemLog(AutomationExecutionLog log)
        {
            DbHelper.ExecuteNonQuery(
                @"INSERT INTO dbo.AutomationExecutionLog
                    (ExecutionID, StepType, StepOrder, ConditionID, ActionID, StepName, IsSuccess,
                     ExpectedValue, ActualValue, Message, DurationMs)
                  VALUES (@ExecutionID, @StepType, @StepOrder, @ConditionID, @ActionID, @StepName, @IsSuccess,
                          @ExpectedValue, @ActualValue, @Message, @DurationMs)",
                new SqlParameter("@ExecutionID", SqlDbType.BigInt) { Value = log.ExecutionID },
                new SqlParameter("@StepType", SqlDbType.VarChar, 10) { Value = log.StepType },
                new SqlParameter("@StepOrder", SqlDbType.Int) { Value = log.StepOrder },
                new SqlParameter("@ConditionID", SqlDbType.Int)
                {
                    Value = (object?)log.ConditionID ?? DBNull.Value
                },
                new SqlParameter("@ActionID", SqlDbType.Int)
                {
                    Value = (object?)log.ActionID ?? DBNull.Value
                },
                new SqlParameter("@StepName", SqlDbType.NVarChar, 200) { Value = CatChuoi(log.StepName, 200) },
                new SqlParameter("@IsSuccess", SqlDbType.Bit) { Value = log.IsSuccess },
                new SqlParameter("@ExpectedValue", SqlDbType.NVarChar, 200)
                {
                    Value = CatChuoi(log.ExpectedValue, 200) ?? (object)DBNull.Value
                },
                new SqlParameter("@ActualValue", SqlDbType.NVarChar, 200)
                {
                    Value = CatChuoi(log.ActualValue, 200) ?? (object)DBNull.Value
                },
                new SqlParameter("@Message", SqlDbType.NVarChar, 1000)
                {
                    Value = CatChuoi(log.Message, 1000) ?? (object)DBNull.Value
                },
                new SqlParameter("@DurationMs", SqlDbType.Int)
                {
                    Value = (object?)log.DurationMs ?? DBNull.Value
                });
        }

        public AutomationExecution? LayTheoID(long executionId)
        {
            DataTable dt = DbHelper.ExecuteQuery(
                @"SELECT ExecutionID, RuleID, RuleNameSnapshot, TriggerType, EntityType, EntityID,
                         EventToken, DedupKey, Status, ConditionsMatched, ActionSuccessCount, ActionFailCount,
                         StartedAt, FinishedAt, DurationMs, TriggeredBy, ErrorMessage, ContextJson
                  FROM dbo.AutomationExecution
                  WHERE ExecutionID = @ExecutionID",
                new SqlParameter("@ExecutionID", SqlDbType.BigInt) { Value = executionId });

            return dt.Rows.Count == 0 ? null : AutomationMapper.DocExecution(dt.Rows[0]);
        }

        public DataTable LayDanhSach(DateTime? tuNgay, DateTime? denNgay,
                                     string triggerType = "", string trangThai = "",
                                     string tuKhoa = "", int soDong = 500)
        {
            const string sql = @"
                SELECT TOP (@SoDong)
                       ExecutionID, RuleID, RuleNameSnapshot, TriggerType, EntityType, EntityID,
                       Status, ConditionsMatched, ActionSuccessCount, ActionFailCount,
                       StartedAt, FinishedAt, DurationMs, TriggeredBy, ErrorMessage
                FROM dbo.AutomationExecution
                WHERE (@TuNgay IS NULL OR StartedAt >= @TuNgay)
                  AND (@DenNgay IS NULL OR StartedAt < DATEADD(DAY, 1, @DenNgay))
                  AND (@TriggerType = '' OR TriggerType = @TriggerType)
                  AND (@TrangThai = '' OR Status = @TrangThai)
                  AND (@TuKhoa = ''
                       OR RuleNameSnapshot COLLATE Vietnamese_CI_AI LIKE '%' + @TuKhoa + '%'
                       OR EntityID LIKE '%' + @TuKhoa + '%'
                       OR ISNULL(ErrorMessage, N'') COLLATE Vietnamese_CI_AI LIKE '%' + @TuKhoa + '%')
                ORDER BY StartedAt DESC";

            return DbHelper.ExecuteQuery(sql,
                new SqlParameter("@SoDong", SqlDbType.Int) { Value = soDong <= 0 ? 500 : soDong },
                new SqlParameter("@TuNgay", SqlDbType.Date)
                {
                    Value = (object?)tuNgay?.Date ?? DBNull.Value
                },
                new SqlParameter("@DenNgay", SqlDbType.Date)
                {
                    Value = (object?)denNgay?.Date ?? DBNull.Value
                },
                new SqlParameter("@TriggerType", SqlDbType.VarChar, 40) { Value = triggerType ?? "" },
                new SqlParameter("@TrangThai", SqlDbType.VarChar, 20) { Value = trangThai ?? "" },
                new SqlParameter("@TuKhoa", SqlDbType.NVarChar, 200) { Value = (tuKhoa ?? "").Trim() });
        }

        public DataTable LayLog(long executionId)
        {
            return DbHelper.ExecuteQuery(
                @"SELECT LogID, ExecutionID, StepType, StepOrder, ConditionID, ActionID, StepName,
                         IsSuccess, ExpectedValue, ActualValue, Message, DurationMs, CreatedAt
                  FROM dbo.AutomationExecutionLog
                  WHERE ExecutionID = @ExecutionID
                  ORDER BY CASE StepType WHEN 'CONDITION' THEN 0 ELSE 1 END, StepOrder",
                new SqlParameter("@ExecutionID", SqlDbType.BigInt) { Value = executionId });
        }

        public AutomationThongKe LayThongKe()
        {
            DataTable dt = DbHelper.ExecuteQuery(@"
                SELECT
                    (SELECT COUNT(*) FROM dbo.AutomationRule WHERE IsDeleted = 0) AS TongSoRule,
                    (SELECT COUNT(*) FROM dbo.AutomationRule WHERE IsDeleted = 0 AND IsActive = 1) AS RuleDangHoatDong,
                    (SELECT COUNT(*) FROM dbo.AutomationExecution WHERE Status = 'SUCCESS') AS LanChayThanhCong,
                    (SELECT COUNT(*) FROM dbo.AutomationExecution WHERE Status = 'FAILED') AS LanChayThatBai,
                    (SELECT COUNT(*) FROM dbo.AutomationExecution WHERE Status = 'PARTIAL') AS LanChayMotPhan,
                    (SELECT COUNT(*) FROM dbo.AutomationTask WHERE Status IN ('PENDING','IN_PROGRESS')) AS TaskDangCho");

            DataRow dong = dt.Rows[0];
            return new AutomationThongKe
            {
                TongSoRule = Convert.ToInt32(dong["TongSoRule"]),
                RuleDangHoatDong = Convert.ToInt32(dong["RuleDangHoatDong"]),
                LanChayThanhCong = Convert.ToInt32(dong["LanChayThanhCong"]),
                LanChayThatBai = Convert.ToInt32(dong["LanChayThatBai"]),
                LanChayMotPhan = Convert.ToInt32(dong["LanChayMotPhan"]),
                TaskDangCho = Convert.ToInt32(dong["TaskDangCho"])
            };
        }

        private static string? CatChuoi(string? giaTri, int doDaiToiDa)
        {
            if (string.IsNullOrEmpty(giaTri)) return giaTri;
            return giaTri.Length <= doDaiToiDa ? giaTri : giaTri.Substring(0, doDaiToiDa);
        }
    }
}
