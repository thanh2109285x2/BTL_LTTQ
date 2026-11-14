using BTL_LTTQ.Common;
using BTL_LTTQ.DataAccessLayer;
using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;
using BTL_LTTQ.Models.Automation;

namespace BTL_LTTQ.Services.Automation
{
    /// <summary>
    /// Handler cho Action CREATE_TASK.
    /// Tạo AutomationTask trong database.
    /// </summary>
    public class CreateTaskHandler : IAutomationActionHandler
    {
        public string ActionType => AutomationConst.ActionType.CreateTask;

        public async Task<(bool Success, string ErrorMessage)> ExecuteAsync(long executionID, AutomationAction action,
            AutomationRule rule, AutomationContext context)
        {
            string errorMessage = "";

            // Đọc ParametersJson: {"tieuDe": "...", "uuTien": "HIGH|NORMAL|LOW", "hanXuLy": "YYYY-MM-DD"}
            string? tieuDe = null;
            string? uuTien = AutomationConst.TaskPriority.Normal;
            DateTime? hanXuLy = null;

            try
            {
                if (!string.IsNullOrEmpty(action.ParametersJson))
                {
                    var doc = System.Text.Json.JsonDocument.Parse(action.ParametersJson);
                    if (doc.RootElement.TryGetProperty("TieuDe", out var tieuDeElem))
                        tieuDe = tieuDeElem.GetString();
                    if (doc.RootElement.TryGetProperty("UuTien", out var uuTienElem))
                        uuTien = uuTienElem.GetString() ?? AutomationConst.TaskPriority.Normal;
                    if (doc.RootElement.TryGetProperty("HanXuLy", out var hanXuLyElem))
                        hanXuLy = hanXuLyElem.GetDateTime();
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Lỗi parse ParametersJson cho CREATE_TASK: {ex.Message}";
                return (false, errorMessage);
            }

            if (string.IsNullOrEmpty(tieuDe))
            {
                errorMessage = "Thiếu TieuDe trong ParametersJson cho CREATE_TASK.";
                return (false, errorMessage);
            }

            // Đảm bảo hanXuLy không quá quá khứ
            if (hanXuLy == null)
                hanXuLy = DateTime.Today.AddDays(3);

            // Tạo AutomationTask - Sử dụng SqlParameter chỉ với value
            var taskIdObj = DbHelper.ExecuteScalar(
                @"INSERT INTO dbo.AutomationTask
                    (ExecutionID, RuleID, Title, Description, EntityType, EntityID,
                     AssignedToRole, Priority, Status, DueDate)
                  OUTPUT INSERTED.TaskID
                  VALUES
                    (@ExecutionID, @RuleID, @Title, @Description, @EntityType, @EntityID,
                     @AssignedToRole, @Priority, @Status, @DueDate)",
                new SqlParameter("@ExecutionID", executionID),
                new SqlParameter("@RuleID", rule.RuleID),
                new SqlParameter("@Title", tieuDe),
                new SqlParameter("@Description", action.ParametersJson),
                new SqlParameter("@EntityType", context.EntityType),
                new SqlParameter("@EntityID", context.EntityID),
                new SqlParameter("@AssignedToRole", "Admin"),
                new SqlParameter("@Priority", uuTien),
                new SqlParameter("@Status", AutomationConst.TaskStatus.Pending),
                new SqlParameter("@DueDate", hanXuLy.Value.Date));

            int taskId = Convert.ToInt32(taskIdObj);

            // Ghi log vào ExecutionLog
            DbHelper.ExecuteNonQuery(
                @"INSERT INTO dbo.AutomationExecutionLog
                    (ExecutionID, StepType, StepOrder, ActionID, StepName, IsSuccess,
                     ExpectedValue, ActualValue, Message, DurationMs)
                  VALUES
                    (@ExecutionID, @StepType, 1, @ActionID, @StepName, 1,
                     N'CREATE_TASK', N'Thành công', N'Đã tạo nhiệm vụ', 0)",
                new SqlParameter("@ExecutionID", executionID),
                new SqlParameter("@StepType", "ACTION"),
                new SqlParameter("@ActionID", action.ActionID),
                new SqlParameter("@StepName", "CREATE_TASK"));

            return (true, "");
        }
    }
}