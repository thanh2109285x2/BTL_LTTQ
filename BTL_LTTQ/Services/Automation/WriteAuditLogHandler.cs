using BTL_LTTQ.Common;
using BTL_LTTQ.DataAccessLayer;
using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;
using BTL_LTTQ.Models.Automation;

namespace BTL_LTTQ.Services.Automation
{
    /// <summary>
    /// Handler cho Action WRITE_AUDIT_LOG.
    /// Ghi nhận vào bảng AutomationExecutionLog là bước ghi audit (không thay đổi dữ liệu nghiệp vụ).
    /// </summary>
    public class WriteAuditLogHandler : IAutomationActionHandler
    {
        public string ActionType => AutomationConst.ActionType.WriteAuditLog;

        public async Task<(bool Success, string ErrorMessage)> ExecuteAsync(long executionID, AutomationAction action,
            AutomationRule rule, AutomationContext context)
        {
            string errorMessage = "";

            // Đọc ParametersJson: {"noiDung": "...", "loai": "INFO|WARNING|ERROR"}
            string? noiDung = null;
            string? loai = AutomationConst.MucDoThongBao.Info;

            try
            {
                if (!string.IsNullOrEmpty(action.ParametersJson))
                {
                    var doc = System.Text.Json.JsonDocument.Parse(action.ParametersJson);
                    if (doc.RootElement.TryGetProperty("NoiDung", out var noiDungElem))
                        noiDung = noiDungElem.GetString();
                    if (doc.RootElement.TryGetProperty("Loai", out var loaiElem))
                        loai = loaiElem.GetString() ?? AutomationConst.MucDoThongBao.Info;
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Lỗi parse ParametersJson cho WRITE_AUDIT_LOG: {ex.Message}";
                return (false, errorMessage);
            }

            if (string.IsNullOrEmpty(noiDung))
            {
                errorMessage = "Thiếu NoiDung trong ParametersJson cho WRITE_AUDIT_LOG.";
                return (false, errorMessage);
            }

            // Ghi vào AutomationExecutionLog như 1 bước AUDIT - Sử dụng SqlParameter chỉ với value
            DbHelper.ExecuteNonQuery(
                @"INSERT INTO dbo.AutomationExecutionLog
                    (ExecutionID, StepType, StepOrder, ActionID, StepName, IsSuccess,
                     ExpectedValue, ActualValue, Message, DurationMs)
                  VALUES
                    (@ExecutionID, @StepType, 1, @ActionID, @StepName, 1,
                     N'WRITE_AUDIT_LOG', N'{}', @NoiDung, 0)",
                new SqlParameter("@ExecutionID", executionID),
                new SqlParameter("@StepType", "ACTION"),
                new SqlParameter("@ActionID", action.ActionID),
                new SqlParameter("@StepName", "WRITE_AUDIT_LOG"),
                new SqlParameter("@NoiDung", noiDung));

            return (true, "");
        }
    }
}