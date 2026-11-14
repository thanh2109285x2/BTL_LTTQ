using System;

namespace BTL_LTTQ.Models.Automation
{
    /// <summary>Nhiệm vụ do Automation tạo ra và giao cho người dùng xử lý.</summary>
    public class AutomationTask
    {
        public int TaskID { get; set; }
        public long? ExecutionID { get; set; }
        public int RuleID { get; set; }
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public string EntityType { get; set; } = "CLASS";
        public string EntityID { get; set; } = "";
        public string? AssignedToAccount { get; set; }
        public string? AssignedToRole { get; set; }
        public string Priority { get; set; } = "NORMAL";
        public string Status { get; set; } = "PENDING";
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? CompletedBy { get; set; }
        public bool IsOpen { get; set; }
    }
}
