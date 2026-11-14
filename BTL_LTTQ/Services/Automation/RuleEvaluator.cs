using BTL_LTTQ.Models.Automation;
using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RulesEngineClass = global::RulesEngine.RulesEngine;

namespace BTL_LTTQ.Services.Automation
{
    /// <summary>
    /// Đánh giá điều kiện của một Rule bằng RulesEngine. Không thực thi hành động,
    /// nên có thể dùng cho chức năng "Test Rule" mà không làm thay đổi dữ liệu thật.
    /// </summary>
    public class RuleEvaluator
    {
        private readonly RulesEngineProvider _provider = new();

        public async Task<KetQuaDanhGiaRule> DanhGiaAsync(AutomationRule rule, AutomationContext context)
        {
            var ketQua = new KetQuaDanhGiaRule
            {
                HanhDong = new List<AutomationAction>(rule.Actions)
            };

            List<string> loiCauHinh = ConditionEvaluator.KiemTraDanhSach(rule.Conditions);
            if (loiCauHinh.Count > 0)
            {
                ketQua.HopLe = false;
                ketQua.Loi = string.Join(" ", loiCauHinh);
                GhiNhanDieuKienLoi(ketQua, rule);
                return ketQua;
            }

            List<RuleResultTree> ketQuaRule;
            try
            {
                (RulesEngineClass engine, string tenWorkflow) = _provider.Lay(rule);

                ketQuaRule = await engine.ExecuteAllRulesAsync(
                    tenWorkflow,
                    new RuleParameter("ctx", context));
            }
            catch (Exception ex)
            {
                ketQua.HopLe = false;
                ketQua.Loi = "Không đánh giá được Rule: " + ex.Message;
                GhiNhanDieuKienLoi(ketQua, rule);
                return ketQua;
            }

            if (ketQuaRule == null || ketQuaRule.Count == 0)
            {
                ketQua.HopLe = false;
                ketQua.Loi = "RulesEngine không trả về kết quả đánh giá.";
                GhiNhanDieuKienLoi(ketQua, rule);
                return ketQua;
            }

            ketQua.HopLe = true;
            ketQua.TatCaDieuKienDung = true;

            for (int i = 0; i < rule.Conditions.Count; i++)
            {
                AutomationCondition dieuKien = rule.Conditions[i];
                string tenRule = RuleDefinitionBuilder.TenDieuKien(i);

                RuleResultTree? cay = null;
                foreach (RuleResultTree item in ketQuaRule)
                {
                    if (item.Rule != null && string.Equals(item.Rule.RuleName, tenRule, StringComparison.Ordinal))
                    {
                        cay = item;
                        break;
                    }
                }

                var chiTiet = new KetQuaDieuKien
                {
                    ConditionID = dieuKien.ConditionID,
                    SortOrder = dieuKien.SortOrder,
                    FieldCode = dieuKien.FieldCode,
                    Operator = dieuKien.Operator,
                    MoTa = ConditionEvaluator.MoTa(dieuKien),
                    GiaTriMongDoi = GiaTriMongDoi(dieuKien),
                    GiaTriThucTe = AutomationFieldCatalog.LayGiaTri(context, dieuKien.FieldCode)
                };

                if (cay == null)
                {
                    chiTiet.ThanhCong = false;
                    chiTiet.Loi = "Không tìm thấy kết quả đánh giá cho điều kiện này.";
                    ketQua.HopLe = false;
                    ketQua.Loi = chiTiet.Loi;
                }
                else
                {
                    chiTiet.ThanhCong = cay.IsSuccess;

                    if (!string.IsNullOrWhiteSpace(cay.ExceptionMessage))
                    {
                        chiTiet.Loi = cay.ExceptionMessage;
                        ketQua.HopLe = false;
                        ketQua.Loi = $"Điều kiện #{i + 1}: {cay.ExceptionMessage}";
                    }
                }

                if (!chiTiet.ThanhCong)
                    ketQua.TatCaDieuKienDung = false;

                ketQua.DieuKien.Add(chiTiet);
            }

            return ketQua;
        }

        private static string? GiaTriMongDoi(AutomationCondition c)
        {
            if (c.Operator == BTL_LTTQ.Common.AutomationConst.Operator.IsTrue)
                return "true";

            if (c.Operator == BTL_LTTQ.Common.AutomationConst.Operator.IsFalse)
                return "false";

            return ConditionEvaluator.KyHieuToanTu(c.Operator) + " " +
                   ConditionEvaluator.GiaTriMongDoi(c);
        }

        private static void GhiNhanDieuKienLoi(KetQuaDanhGiaRule ketQua, AutomationRule rule)
        {
            for (int i = 0; i < rule.Conditions.Count; i++)
            {
                AutomationCondition c = rule.Conditions[i];
                ketQua.DieuKien.Add(new KetQuaDieuKien
                {
                    ConditionID = c.ConditionID,
                    SortOrder = c.SortOrder,
                    FieldCode = c.FieldCode,
                    Operator = c.Operator,
                    MoTa = ConditionEvaluator.MoTa(c),
                    GiaTriMongDoi = GiaTriMongDoi(c),
                    ThanhCong = false,
                    Loi = ketQua.Loi
                });
            }
        }
    }
}
