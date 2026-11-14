using System;

namespace BTL_LTTQ.Models.Automation
{
    /// <summary>Một lần thực thi Rule. Lưu snapshot tên Rule để không mất dấu khi Rule bị sửa/xóa mềm.</summary>
    public class AutomationExecution
    {
        public long ExecutionID { get; set; }
        public int RuleID { get; set; }
        public string RuleNameSnapshot { get; set; } = "";
        public string TriggerType { get; set; } = "";
        public string EntityType { get; set; } = "CLASS";
        public string EntityID { get; set; } = "";
        public string EventToken { get; set; } = "";
        public string DedupKey { get; set; } = "";
        public string Status { get; set; } = "RUNNING";
        public bool ConditionsMatched { get; set; }
        public int ActionSuccessCount { get; set; }
        public int ActionFailCount { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public int? DurationMs { get; set; }
        public string? TriggeredBy { get; set; }
        public string? ErrorMessage { get; set; }
        public string? ContextJson { get; set; }
    }
}
