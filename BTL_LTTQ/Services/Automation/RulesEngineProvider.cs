using BTL_LTTQ.Models.Automation;
using RulesEngine.Models;
using System;
using System.Collections.Concurrent;
using RulesEngineClass = global::RulesEngine.RulesEngine;

namespace BTL_LTTQ.Services.Automation
{
    /// <summary>
    /// Quản lý instance RulesEngine và cache workflow đã biên dịch.
    /// AutoExecuteActions = false: RulesEngine chỉ đánh giá điều kiện, không bao giờ tự chạy action.
    /// </summary>
    public class RulesEngineProvider
    {
        private const int SoEngineToiDa = 200;
        private static readonly ConcurrentDictionary<string, RulesEngineClass> _cache =
            new(StringComparer.Ordinal);

        public (RulesEngineClass Engine, string TenWorkflow) Lay(AutomationRule rule)
        {
            Workflow workflow = RuleDefinitionBuilder.XayDung(rule);
            string chuKy = RuleDefinitionBuilder.TaoChuKy(rule);

            if (_cache.Count >= SoEngineToiDa)
                _cache.Clear();

            RulesEngineClass engine = _cache.GetOrAdd(chuKy, _ => TaoEngine(workflow));
            return (engine, workflow.WorkflowName);
        }

        /// <summary>Gọi khi Rule được lưu/xóa để không giữ engine của cấu hình cũ.</summary>
        public static void XoaCache()
        {
            _cache.Clear();
        }

        public static int SoEngineDangCache => _cache.Count;

        private static RulesEngineClass TaoEngine(Workflow workflow)
        {
            var caiDat = new ReSettings
            {
                // Bắt buộc: RulesEngine không được tự thực thi action nghiệp vụ.
                AutoExecuteActions = false,
                UseFastExpressionCompiler = false,
                EnableExceptionAsErrorMessage = true,
                IgnoreException = false,
                EnableFormattedErrorMessage = true,
                AutoRegisterInputType = true,
                IsExpressionCaseSensitive = false
            };

            return new RulesEngineClass(new[] { workflow }, caiDat);
        }
    }
}
