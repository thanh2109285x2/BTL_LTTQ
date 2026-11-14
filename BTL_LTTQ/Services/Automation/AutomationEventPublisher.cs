using BTL_LTTQ.Common;
using BTL_LTTQ.Models.Automation;
using BTL_LTTQ.Repositories.Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BTL_LTTQ.Services.Automation
{
    /// <summary>
    /// Phát sự kiện nghiệp vụ cho Automation Center: nạp Rule đang bật theo TriggerType,
    /// đánh giá điều kiện và thực thi hành động. Mọi lỗi Automation đều được nuốt để
    /// không ảnh hưởng giao dịch nghiệp vụ đã commit.
    /// </summary>
    public class AutomationEventPublisher
    {
        private readonly AutomationRuleRepository _ruleRepo = new();
        private readonly AutomationExecutionRepository _executionRepo = new();
        private readonly AutomationContextBuilder _contextBuilder = new();
        private readonly RuleEvaluator _evaluator = new();

        public async Task PhatSuKienAsync(string triggerType, string entityType, string entityId,
            string eventToken, AutomationContext? context = null, string? triggeredBy = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(triggerType) ||
                    string.IsNullOrWhiteSpace(entityType) ||
                    string.IsNullOrWhiteSpace(entityId))
                    return;

                // Bảo đảm handler đã đăng ký kể cả khi publisher được dùng ngoài luồng UI startup.
                ActionHandlerRegistry.EnsureInitialized();

                List<AutomationRule> rules = _ruleRepo.LayDangHoatDong(triggerType);
                if (rules.Count == 0)
                    return;

                string entityIdGioiHan = Cat(entityId, 20);
                string tokenGioiHan = Cat(eventToken, 200);
                string? nguoiThucHien = Cat(triggeredBy ?? CurrentUser.TenDangNhap, 50);

                AutomationContext? ctx = context;
                if (ctx == null && entityType == AutomationConst.EntityType.Class)
                    ctx = _contextBuilder.XayDung(entityIdGioiHan);

                if (ctx == null)
                    return;

                ctx.EntityType = entityType;
                ctx.EntityID = entityIdGioiHan;

                foreach (AutomationRule rule in rules)
                {
                    await ThucThiRuleAsync(rule, entityType, entityIdGioiHan, tokenGioiHan, ctx, nguoiThucHien);
                }
            }
            catch
            {
                // Automation không được làm hỏng luồng nghiệp vụ.
            }
        }

        private async Task ThucThiRuleAsync(AutomationRule rule, string entityType, string entityId,
            string eventToken, AutomationContext context, string? triggeredBy)
        {
            long executionId = 0;

            try
            {
                string dedupKey = TaoDedupKey(rule.RuleID, rule.TriggerType, entityType, entityId, eventToken);

                if (_executionRepo.DaTonTaiDedup(rule.RuleID, dedupKey))
                    return;

                if (rule.CooldownMinutes is int cooldown &&
                    _executionRepo.TrongThoiGianCho(rule.RuleID, entityType, entityId, cooldown))
                    return;

                var execution = new AutomationExecution
                {
                    RuleID = rule.RuleID,
                    RuleNameSnapshot = Cat(rule.RuleName, 150) ?? "",
                    TriggerType = Cat(rule.TriggerType, 40) ?? "",
                    EntityType = Cat(entityType, 40) ?? "",
                    EntityID = entityId,
                    EventToken = eventToken,
                    DedupKey = dedupKey,
                    Status = AutomationConst.ExecutionStatus.Running,
                    TriggeredBy = triggeredBy
                };

                executionId = _executionRepo.Them(execution);

                // 0 nghĩa là một sự kiện trùng vừa được ghi bởi phiên khác.
                if (executionId == 0)
                    return;

                // Them() chỉ trả về khóa, không gán ngược vào đối tượng.
                // Thiếu dòng này thì CapNhatKetQua sẽ cập nhật WHERE ExecutionID = 0.
                execution.ExecutionID = executionId;

                KetQuaDanhGiaRule danhGia = await _evaluator.DanhGiaAsync(rule, context);

                GhiLogDieuKien(executionId, danhGia);

                if (!danhGia.HopLe)
                {
                    execution.Status = AutomationConst.ExecutionStatus.Failed;
                    execution.ConditionsMatched = false;
                    execution.ErrorMessage = danhGia.Loi;
                    execution.ContextJson = TaoContextJson(context);
                    _executionRepo.CapNhatKetQua(execution);
                    return;
                }

                if (!danhGia.TatCaDieuKienDung)
                {
                    execution.Status = AutomationConst.ExecutionStatus.Skipped;
                    execution.ConditionsMatched = false;
                    execution.ContextJson = TaoContextJson(context);
                    _executionRepo.CapNhatKetQua(execution);
                    return;
                }

                execution.ConditionsMatched = true;

                int thanhCong = 0;
                int thatBai = 0;
                var loiHanhDong = new List<string>();

                foreach (AutomationAction action in danhGia.HanhDong.OrderBy(a => a.SortOrder))
                {
                    IAutomationActionHandler? handler = ActionHandlerRegistry.GetHandler(action.ActionType);

                    if (handler == null)
                    {
                        thatBai++;
                        string thongDiep = $"Không có handler cho ActionType '{action.ActionType}'.";
                        loiHanhDong.Add(thongDiep);
                        GhiLogHanhDongThatBai(executionId, action, thongDiep);
                        if (!action.ContinueOnError)
                            break;

                        continue;
                    }

                    bool ok;
                    string loi = "";

                    try
                    {
                        (ok, loi) = await handler.ExecuteAsync(executionId, action, rule, context);
                    }
                    catch (Exception ex)
                    {
                        ok = false;
                        loi = "Handler lỗi: " + ex.Message;
                    }

                    if (ok)
                    {
                        thanhCong++;
                    }
                    else
                    {
                        thatBai++;
                        if (!string.IsNullOrWhiteSpace(loi))
                            loiHanhDong.Add($"{action.ActionType}: {loi}");
                        GhiLogHanhDongThatBai(executionId, action,
                            string.IsNullOrWhiteSpace(loi) ? "Hành động thất bại." : loi);

                        if (!action.ContinueOnError)
                            break;
                    }
                }

                execution.ActionSuccessCount = thanhCong;
                execution.ActionFailCount = thatBai;
                execution.ContextJson = TaoContextJson(context);

                if (thatBai == 0)
                    execution.Status = AutomationConst.ExecutionStatus.Success;
                else if (thanhCong > 0)
                    execution.Status = AutomationConst.ExecutionStatus.Partial;
                else
                    execution.Status = AutomationConst.ExecutionStatus.Failed;

                if (loiHanhDong.Count > 0)
                    execution.ErrorMessage = string.Join(" | ", loiHanhDong);

                _executionRepo.CapNhatKetQua(execution);
            }
            catch (Exception ex)
            {
                if (executionId == 0)
                    return;

                try
                {
                    _executionRepo.CapNhatKetQua(new AutomationExecution
                    {
                        ExecutionID = executionId,
                        Status = AutomationConst.ExecutionStatus.Failed,
                        ConditionsMatched = false,
                        ErrorMessage = Cat("Lỗi Automation: " + ex.Message, 2000)
                    });
                }
                catch
                {
                    // Bỏ qua: không để lỗi ghi lịch sử ảnh hưởng nghiệp vụ.
                }
            }
        }

        private void GhiLogDieuKien(long executionId, KetQuaDanhGiaRule danhGia)
        {
            for (int i = 0; i < danhGia.DieuKien.Count; i++)
            {
                KetQuaDieuKien dieuKien = danhGia.DieuKien[i];

                try
                {
                    _executionRepo.ThemLog(new AutomationExecutionLog
                    {
                        ExecutionID = executionId,
                        StepType = AutomationConst.StepType.Condition,
                        StepOrder = i + 1,
                        ConditionID = dieuKien.ConditionID,
                        StepName = Cat($"{dieuKien.FieldCode} {dieuKien.Operator}", 200) ?? "",
                        IsSuccess = dieuKien.ThanhCong,
                        ExpectedValue = Cat(dieuKien.GiaTriMongDoi, 200),
                        ActualValue = Cat(dieuKien.GiaTriThucTe, 200),
                        Message = Cat(dieuKien.Loi ?? dieuKien.MoTa, 1000)
                    });
                }
                catch
                {
                    // Không để lỗi log ảnh hưởng việc đánh giá Rule.
                }
            }
        }

        private void GhiLogHanhDongThatBai(long executionId, AutomationAction action, string thongDiep)
        {
            try
            {
                _executionRepo.ThemLog(new AutomationExecutionLog
                {
                    ExecutionID = executionId,
                    StepType = AutomationConst.StepType.Action,
                    StepOrder = action.SortOrder,
                    ActionID = action.ActionID,
                    StepName = Cat(action.ActionType, 200) ?? "",
                    IsSuccess = false,
                    Message = Cat(thongDiep, 1000)
                });
            }
            catch
            {
                // Không để lỗi log ảnh hưởng nghiệp vụ.
            }
        }

        public static string TaoDedupKey(int ruleId, string triggerType, string entityType,
            string entityId, string eventToken)
        {
            string noiDung = string.Join("|", ruleId, triggerType, entityType, entityId, eventToken);
            byte[] bam = SHA256.HashData(Encoding.UTF8.GetBytes(noiDung));
            return Convert.ToHexString(bam).ToLowerInvariant();
        }

        private static string? TaoContextJson(AutomationContext context)
        {
            try
            {
                return JsonSerializer.Serialize(context);
            }
            catch
            {
                return null;
            }
        }

        private static string? Cat(string? giaTri, int doDai)
        {
            if (string.IsNullOrWhiteSpace(giaTri))
                return string.IsNullOrWhiteSpace(giaTri) ? giaTri : null;

            return giaTri.Length <= doDai ? giaTri : giaTri.Substring(0, doDai);
        }
    }
}
