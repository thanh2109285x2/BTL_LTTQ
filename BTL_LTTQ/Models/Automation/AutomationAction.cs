namespace BTL_LTTQ.Models.Automation
{
    /// <summary>Một hành động của Rule. Tham số lưu dạng JSON có kiểm tra ISJSON ở database.</summary>
    public class AutomationAction
    {
        public int ActionID { get; set; }
        public int RuleID { get; set; }
        public string ActionType { get; set; } = "";
        public int SortOrder { get; set; }
        public string ParametersJson { get; set; } = "{}";
        public bool ContinueOnError { get; set; } = true;
    }
}
