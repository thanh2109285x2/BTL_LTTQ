using System;
using System.Collections.Generic;

namespace BTL_LTTQ.Models.Automation
{
    /// <summary>Cấu hình một quy tắc tự động hóa (AutomationRule).</summary>
    public class AutomationRule
    {
        public int RuleID { get; set; }
        public string RuleName { get; set; } = "";
        public string? Description { get; set; }
        public string TriggerType { get; set; } = "";
        public int Priority { get; set; } = 100;
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; }
        public int? CooldownMinutes { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public byte[]? RowVersion { get; set; }

        public List<AutomationCondition> Conditions { get; set; } = new();
        public List<AutomationAction> Actions { get; set; } = new();
    }
}
