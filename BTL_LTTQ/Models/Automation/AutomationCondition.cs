namespace BTL_LTTQ.Models.Automation
{
    /// <summary>Một điều kiện của Rule. Chỉ lưu dữ liệu có cấu trúc, không lưu expression.</summary>
    public class AutomationCondition
    {
        public int ConditionID { get; set; }
        public int RuleID { get; set; }
        public string FieldCode { get; set; } = "";
        public string Operator { get; set; } = "";
        public string? ValueText { get; set; }
        public decimal? ValueNumber { get; set; }
        public int SortOrder { get; set; }
    }
}
