using BTL_LTTQ.Models.Automation;
using System;
using System.Data;

namespace BTL_LTTQ.Repositories.Automation
{
    /// <summary>Chuyển DataRow sang model. Tách riêng để các repository không lặp code đọc dữ liệu.</summary>
    internal static class AutomationMapper
    {
        public static AutomationRule DocRule(DataRow r)
        {
            return new AutomationRule
            {
                RuleID = SoNguyen(r, "RuleID"),
                RuleName = Chuoi(r, "RuleName") ?? "",
                Description = Chuoi(r, "Description"),
                TriggerType = Chuoi(r, "TriggerType") ?? "",
                Priority = SoNguyen(r, "Priority"),
                IsActive = CoKhong(r, "IsActive"),
                IsDeleted = CoKhong(r, "IsDeleted"),
                CooldownMinutes = SoNguyenNull(r, "CooldownMinutes"),
                CreatedAt = ThoiGian(r, "CreatedAt") ?? DateTime.MinValue,
                CreatedBy = Chuoi(r, "CreatedBy"),
                UpdatedAt = ThoiGian(r, "UpdatedAt"),
                UpdatedBy = Chuoi(r, "UpdatedBy"),
                RowVersion = r["RowVersion"] == DBNull.Value ? null : (byte[])r["RowVersion"]
            };
        }

        public static AutomationCondition DocCondition(DataRow r)
        {
            return new AutomationCondition
            {
                ConditionID = SoNguyen(r, "ConditionID"),
                RuleID = SoNguyen(r, "RuleID"),
                FieldCode = Chuoi(r, "FieldCode") ?? "",
                Operator = Chuoi(r, "Operator") ?? "",
                ValueText = Chuoi(r, "ValueText"),
                ValueNumber = SoThapPhanNull(r, "ValueNumber"),
                SortOrder = SoNguyen(r, "SortOrder")
            };
        }

        public static AutomationAction DocAction(DataRow r)
        {
            return new AutomationAction
            {
                ActionID = SoNguyen(r, "ActionID"),
                RuleID = SoNguyen(r, "RuleID"),
                ActionType = Chuoi(r, "ActionType") ?? "",
                SortOrder = SoNguyen(r, "SortOrder"),
                ParametersJson = Chuoi(r, "ParametersJson") ?? "{}",
                ContinueOnError = CoKhong(r, "ContinueOnError")
            };
        }

        public static AutomationExecution DocExecution(DataRow r)
        {
            return new AutomationExecution
            {
                ExecutionID = SoNguyenDai(r, "ExecutionID"),
                RuleID = SoNguyen(r, "RuleID"),
                RuleNameSnapshot = Chuoi(r, "RuleNameSnapshot") ?? "",
                TriggerType = Chuoi(r, "TriggerType") ?? "",
                EntityType = Chuoi(r, "EntityType") ?? "",
                EntityID = Chuoi(r, "EntityID") ?? "",
                EventToken = Chuoi(r, "EventToken") ?? "",
                DedupKey = Chuoi(r, "DedupKey") ?? "",
                Status = Chuoi(r, "Status") ?? "",
                ConditionsMatched = CoKhong(r, "ConditionsMatched"),
                ActionSuccessCount = SoNguyen(r, "ActionSuccessCount"),
                ActionFailCount = SoNguyen(r, "ActionFailCount"),
                StartedAt = ThoiGian(r, "StartedAt") ?? DateTime.MinValue,
                FinishedAt = ThoiGian(r, "FinishedAt"),
                DurationMs = SoNguyenNull(r, "DurationMs"),
                TriggeredBy = Chuoi(r, "TriggeredBy"),
                ErrorMessage = Chuoi(r, "ErrorMessage"),
                ContextJson = Chuoi(r, "ContextJson")
            };
        }

        public static AutomationTask DocTask(DataRow r)
        {
            return new AutomationTask
            {
                TaskID = SoNguyen(r, "TaskID"),
                ExecutionID = SoNguyenDaiNull(r, "ExecutionID"),
                RuleID = SoNguyen(r, "RuleID"),
                Title = Chuoi(r, "Title") ?? "",
                Description = Chuoi(r, "Description"),
                EntityType = Chuoi(r, "EntityType") ?? "",
                EntityID = Chuoi(r, "EntityID") ?? "",
                AssignedToAccount = Chuoi(r, "AssignedToAccount"),
                AssignedToRole = Chuoi(r, "AssignedToRole"),
                Priority = Chuoi(r, "Priority") ?? "",
                Status = Chuoi(r, "Status") ?? "",
                DueDate = ThoiGian(r, "DueDate"),
                CreatedAt = ThoiGian(r, "CreatedAt") ?? DateTime.MinValue,
                CompletedAt = ThoiGian(r, "CompletedAt"),
                CompletedBy = Chuoi(r, "CompletedBy"),
                IsOpen = CoKhong(r, "IsOpen")
            };
        }

        private static string? Chuoi(DataRow r, string cot)
        {
            object giaTri = r[cot];
            return giaTri == DBNull.Value ? null : Convert.ToString(giaTri);
        }

        private static int SoNguyen(DataRow r, string cot)
        {
            object giaTri = r[cot];
            return giaTri == DBNull.Value ? 0 : Convert.ToInt32(giaTri);
        }

        private static int? SoNguyenNull(DataRow r, string cot)
        {
            object giaTri = r[cot];
            return giaTri == DBNull.Value ? null : Convert.ToInt32(giaTri);
        }

        private static long SoNguyenDai(DataRow r, string cot)
        {
            object giaTri = r[cot];
            return giaTri == DBNull.Value ? 0 : Convert.ToInt64(giaTri);
        }

        private static long? SoNguyenDaiNull(DataRow r, string cot)
        {
            object giaTri = r[cot];
            return giaTri == DBNull.Value ? null : Convert.ToInt64(giaTri);
        }

        private static decimal? SoThapPhanNull(DataRow r, string cot)
        {
            object giaTri = r[cot];
            return giaTri == DBNull.Value ? null : Convert.ToDecimal(giaTri);
        }

        private static bool CoKhong(DataRow r, string cot)
        {
            object giaTri = r[cot];
            return giaTri != DBNull.Value && Convert.ToBoolean(giaTri);
        }

        private static DateTime? ThoiGian(DataRow r, string cot)
        {
            object giaTri = r[cot];
            return giaTri == DBNull.Value ? null : Convert.ToDateTime(giaTri);
        }
    }
}
