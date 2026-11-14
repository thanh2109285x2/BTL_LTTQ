using BTL_LTTQ.Common;
using BTL_LTTQ.DataAccessLayer;
using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;
using BTL_LTTQ.Models.Automation;

namespace BTL_LTTQ.Services.Automation
{
    /// <summary>
    /// Interface cho Action Handler trong Automation Center.
    /// </summary>
    public interface IAutomationActionHandler
    {
        /// <summary>
        /// Mã hành động này xử lý (phải khớp ActionType trong database).
        /// </summary>
        string ActionType { get; }

        /// <summary>
        /// Thực thi action. Trả về (thành công, thongDiepLoi).
        /// </summary>
        Task<(bool Success, string ErrorMessage)> ExecuteAsync(long executionID, AutomationAction action,
            AutomationRule rule, AutomationContext context);
    }
}