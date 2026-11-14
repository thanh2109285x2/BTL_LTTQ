using BTL_LTTQ.Common;
using BTL_LTTQ.DataAccessLayer;
using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;
using BTL_LTTQ.Models.Automation;

namespace BTL_LTTQ.Services.Automation
{
    /// <summary>
    /// Handler cho Action CREATE_NOTIFICATION.
    /// Tạo bản ghi ThongBao trong database.
    /// </summary>
    public class CreateNotificationHandler : IAutomationActionHandler
    {
        public string ActionType => AutomationConst.ActionType.CreateNotification;

        public async Task<(bool Success, string ErrorMessage)> ExecuteAsync(long executionID, AutomationAction action,
            AutomationRule rule, AutomationContext context)
        {
            string errorMessage = "";

            // Đọc ParametersJson: {"tieuDe": "...", "mucDo": "...", "noiDung": "..."}
            string? tieuDe = null;
            string? noiDung = null;
            string? mucDo = AutomationConst.MucDoThongBao.Info;

            try
            {
                if (!string.IsNullOrEmpty(action.ParametersJson))
                {
                    var doc = System.Text.Json.JsonDocument.Parse(action.ParametersJson);
                    if (doc.RootElement.TryGetProperty("TieuDe", out var tieuDeElem))
                        tieuDe = tieuDeElem.GetString();
                    if (doc.RootElement.TryGetProperty("NoiDung", out var noiDungElem))
                        noiDung = noiDungElem.GetString();
                    if (doc.RootElement.TryGetProperty("MucDo", out var mucDoElem))
                        mucDo = mucDoElem.GetString();
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Lỗi parse ParametersJson cho CREATE_NOTIFICATION: {ex.Message}";
                return (false, errorMessage);
            }

            if (string.IsNullOrEmpty(tieuDe) || string.IsNullOrEmpty(noiDung))
            {
                errorMessage = "Thiếu TieuDe hoặc NoiDung trong ParametersJson cho CREATE_NOTIFICATION.";
                return (false, errorMessage);
            }

            // Đối tượng sẽ nhận thông báo
            string? doiTuongVaiTro = null;
            string? doiTuongTaiKhoan = null;

            if (context.TrangThai != null && context.TrangThai == AutomationConst.ClassStatus.DangHoc)
            {
                doiTuongVaiTro = "GiaoVien";
                doiTuongTaiKhoan = context.MaLop;
            }

            // Lưu ThongBao vào database - Sử dụng SqlParameter chỉ với value, SQL Server tự động định kiểu
            DbHelper.ExecuteNonQuery(
                @"INSERT INTO dbo.ThongBao
                    (TieuDe, NoiDung, MucDo, DoiTuongVaiTro, DoiTuongTaiKhoan,
                     EntityType, EntityID, RuleID, ExecutionID, DaDoc, CreatedAt)
                  VALUES
                    (@TieuDe, @NoiDung, @MucDo, @DoiTuongVaiTro, @DoiTuongTaiKhoan,
                     @EntityType, @EntityID, @RuleID, @ExecutionID, @DaDoc, @CreatedAt)",
                new SqlParameter("@TieuDe", tieuDe),
                new SqlParameter("@NoiDung", noiDung),
                new SqlParameter("@MucDo", mucDo),
                new SqlParameter("@DoiTuongVaiTro", doiTuongVaiTro),
                new SqlParameter("@DoiTuongTaiKhoan", doiTuongTaiKhoan),
                new SqlParameter("@EntityType", context.EntityType),
                new SqlParameter("@EntityID", context.EntityID),
                new SqlParameter("@RuleID", rule.RuleID),
                new SqlParameter("@ExecutionID", executionID),
                new SqlParameter("@DaDoc", false),
                new SqlParameter("@CreatedAt", DateTime.UtcNow));

            // Ghi log vào ExecutionLog
            DbHelper.ExecuteNonQuery(
                @"INSERT INTO dbo.AutomationExecutionLog
                    (ExecutionID, StepType, StepOrder, ActionID, StepName, IsSuccess,
                     ExpectedValue, ActualValue, Message, DurationMs)
                  VALUES
                    (@ExecutionID, @StepType, 1, @ActionID, @StepName, 1,
                     N'CREATE_NOTIFICATION', N'Thành công', N'Đã tạo thông báo', 0)",
                new SqlParameter("@ExecutionID", executionID),
                new SqlParameter("@StepType", "ACTION"),
                new SqlParameter("@ActionID", action.ActionID),
                new SqlParameter("@StepName", "CREATE_NOTIFICATION"));

            return (true, "");
        }
    }
}