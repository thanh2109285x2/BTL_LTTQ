using BTL_LTTQ.Common;
using BTL_LTTQ.DataAccessLayer;
using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;
using BTL_LTTQ.Models.Automation;

namespace BTL_LTTQ.Services.Automation
{
    /// <summary>
    /// Handler cho Action UPDATE_CLASS_STATUS.
    /// Cập nhật trạng thái lớp học (LopHoc.TrangThai).
    /// </summary>
    public class UpdateClassStatusHandler : IAutomationActionHandler
    {
        public string ActionType => AutomationConst.ActionType.UpdateClassStatus;

        public async Task<(bool Success, string ErrorMessage)> ExecuteAsync(long executionID, AutomationAction action,
            AutomationRule rule, AutomationContext context)
        {
            string errorMessage = "";

            // Đọc ParametersJson: {"trangThai": "SapKhaiGiang|DangHoc|DaKetThuc|Huy"}
            string? trangThaiMoi = null;

            try
            {
                if (!string.IsNullOrEmpty(action.ParametersJson))
                {
                    var doc = System.Text.Json.JsonDocument.Parse(action.ParametersJson);
                    if (doc.RootElement.TryGetProperty("TrangThai", out var trangThaiElem))
                        trangThaiMoi = trangThaiElem.GetString();
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Lỗi parse ParametersJson cho UPDATE_CLASS_STATUS: {ex.Message}";
                return (false, errorMessage);
            }

            if (string.IsNullOrEmpty(trangThaiMoi))
            {
                errorMessage = "Thiếu TrangThai trong ParametersJson cho UPDATE_CLASS_STATUS.";
                return (false, errorMessage);
            }

            // Kiểm tra TrangThai hợp lệ
            if (Array.IndexOf(AutomationConst.ClassStatus.TatCa, trangThaiMoi) < 0)
            {
                errorMessage = $"Trạng thái '{trangThaiMoi}' không hợp lệ. Phải là một trong: " +
                    string.Join(", ", AutomationConst.ClassStatus.TatCa);
                return (false, errorMessage);
            }

            // Cập nhật LopHoc.TrangThai - Sử dụng SqlParameter chỉ với value
            int soDong = DbHelper.ExecuteNonQuery(
                @"UPDATE dbo.LopHoc
                   SET TrangThai = @TrangThaiMoi
                 WHERE MaLop = @MaLop",
                new SqlParameter("@TrangThaiMoi", trangThaiMoi),
                new SqlParameter("@MaLop", context.EntityID));

            if (soDong == 0)
            {
                errorMessage = $"Không tìm thấy lớp {context.EntityID} để cập nhật trạng thái.";
                return (false, errorMessage);
            }

            // Ghi log vào ExecutionLog
            DbHelper.ExecuteNonQuery(
                @"INSERT INTO dbo.AutomationExecutionLog
                    (ExecutionID, StepType, StepOrder, ActionID, StepName, IsSuccess,
                     ExpectedValue, ActualValue, Message, DurationMs)
                  VALUES
                    (@ExecutionID, @StepType, 1, @ActionID, @StepName, 1,
                     N'UPDATE_CLASS_STATUS', @TrangThaiMoi, N'Đã cập nhật trạng thái', 0)",
                new SqlParameter("@ExecutionID", executionID),
                new SqlParameter("@StepType", "ACTION"),
                new SqlParameter("@ActionID", action.ActionID),
                new SqlParameter("@StepName", "UPDATE_CLASS_STATUS"),
                new SqlParameter("@TrangThaiMoi", trangThaiMoi));

            return (true, "");
        }
    }
}