using System;

namespace BTL_LTTQ.Models.Automation
{
    /// <summary>Kết quả từng bước (điều kiện / hành động) của một lần thực thi.</summary>
    public class AutomationExecutionLog
    {
        public long LogID { get; set; }
        public long ExecutionID { get; set; }
        public string StepType { get; set; } = "CONDITION";
        public int StepOrder { get; set; }
        public int? ConditionID { get; set; }
        public int? ActionID { get; set; }
        public string StepName { get; set; } = "";
        public bool IsSuccess { get; set; }
        public string? ExpectedValue { get; set; }
        public string? ActualValue { get; set; }
        public string? Message { get; set; }
        public int? DurationMs { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
