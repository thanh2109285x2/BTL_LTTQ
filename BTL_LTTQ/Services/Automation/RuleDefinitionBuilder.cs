using BTL_LTTQ.Common;
using BTL_LTTQ.Models.Automation;
using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BTL_LTTQ.Services.Automation
{
    /// <summary>
    /// Chuyển cấu hình Rule (bảng AutomationCondition) thành Workflow JSON của RulesEngine.
    /// Mỗi điều kiện là một Rule phẳng; MVP chỉ hỗ trợ AND nên tầng đánh giá yêu cầu
    /// tất cả Rule đều đúng. Không nhận expression do người dùng nhập.
    /// </summary>
    public static class RuleDefinitionBuilder
    {
        public const string TienToWorkflow = "AutomationRule_";
        public const string TienToDieuKien = "C";

        public static string TenWorkflow(int ruleId)
        {
            return TienToWorkflow + ruleId.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>Tên Rule bên trong workflow; chỉ số 0-based của điều kiện.</summary>
        public static string TenDieuKien(int chiSo)
        {
            return TienToDieuKien + chiSo.ToString(CultureInfo.InvariantCulture);
        }

        public static Workflow XayDung(AutomationRule rule)
        {
            if (rule == null) throw new ArgumentNullException(nameof(rule));

            var dsDieuKien = new List<Rule>();
            for (int i = 0; i < rule.Conditions.Count; i++)
            {
                AutomationCondition dieuKien = rule.Conditions[i];
                string? loi = ConditionEvaluator.KiemTra(dieuKien);
                if (loi != null)
                    throw new InvalidOperationException($"Điều kiện #{i + 1}: {loi}");

                dsDieuKien.Add(new Rule
                {
                    RuleName = TenDieuKien(i),
                    RuleExpressionType = RuleExpressionType.LambdaExpression,
                    Expression = ConditionEvaluator.TaoExpression(dieuKien),
                    Enabled = true,
                    ErrorMessage = ConditionEvaluator.MoTa(dieuKien)
                });
            }

            if (dsDieuKien.Count == 0)
                throw new InvalidOperationException("Rule chưa có điều kiện nào.");

            return new Workflow
            {
                WorkflowName = TenWorkflow(rule.RuleID),
                RuleExpressionType = RuleExpressionType.LambdaExpression,
                Rules = dsDieuKien
            };
        }

        /// <summary>
        /// Chuỗi đại diện nội dung đánh giá của Rule, dùng để cache engine đã biên dịch.
        /// Khi Rule đổi điều kiện, chuỗi đổi nên engine mới được tạo.
        /// </summary>
        public static string TaoChuKy(AutomationRule rule)
        {
            var sb = new StringBuilder();
            sb.Append(rule.RuleID).Append('#');

            foreach (AutomationCondition c in rule.Conditions)
            {
                sb.Append(c.FieldCode).Append('|')
                  .Append(c.Operator).Append('|')
                  .Append(c.ValueNumber?.ToString("0.############", CultureInfo.InvariantCulture) ?? "")
                  .Append('|')
                  .Append(c.ValueText ?? "")
                  .Append(';');
            }

            return sb.ToString();
        }
    }
}
