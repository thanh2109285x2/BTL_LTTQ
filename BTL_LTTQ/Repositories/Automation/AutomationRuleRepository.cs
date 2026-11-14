using BTL_LTTQ.DataAccessLayer;
using BTL_LTTQ.Models.Automation;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace BTL_LTTQ.Repositories.Automation
{
    /// <summary>
    /// Truy cập bảng AutomationRule (+ điều kiện/hành động đi kèm).
    /// Không kiểm tra quyền tại đây: TriggerDispatcher gọi khi người dùng hiện tại có thể là
    /// học viên/giáo viên. Phân quyền được kiểm tra ở AutomationService và trên Form.
    /// </summary>
    public class AutomationRuleRepository
    {
        private readonly AutomationConditionRepository _conditionRepo = new();
        private readonly AutomationActionRepository _actionRepo = new();

        /// <summary>Danh sách Rule cho lưới quản lý (kèm số điều kiện/hành động/lần chạy).</summary>
        public DataTable LayDanhSach(string tuKhoa = "", string triggerType = "", string trangThai = "")
        {
            const string sql = @"
                SELECT r.RuleID, r.RuleName, r.Description, r.TriggerType, r.Priority,
                       r.IsActive, r.CooldownMinutes, r.CreatedAt, r.UpdatedAt,
                       (SELECT COUNT(*) FROM dbo.AutomationCondition c WHERE c.RuleID = r.RuleID) AS SoDieuKien,
                       (SELECT COUNT(*) FROM dbo.AutomationAction a WHERE a.RuleID = r.RuleID) AS SoHanhDong,
                       (SELECT COUNT(*) FROM dbo.AutomationExecution e WHERE e.RuleID = r.RuleID) AS SoLanChay,
                       (SELECT MAX(e.StartedAt) FROM dbo.AutomationExecution e WHERE e.RuleID = r.RuleID) AS LanChayGanNhat
                FROM dbo.AutomationRule r
                WHERE r.IsDeleted = 0
                  AND (@TuKhoa = ''
                       OR r.RuleName COLLATE Vietnamese_CI_AI LIKE '%' + @TuKhoa + '%'
                       OR ISNULL(r.Description, N'') COLLATE Vietnamese_CI_AI LIKE '%' + @TuKhoa + '%')
                  AND (@TriggerType = '' OR r.TriggerType = @TriggerType)
                  AND (@TrangThai = ''
                       OR (@TrangThai = 'ACTIVE' AND r.IsActive = 1)
                       OR (@TrangThai = 'INACTIVE' AND r.IsActive = 0))
                ORDER BY r.Priority, r.RuleName";

            return DbHelper.ExecuteQuery(sql,
                new SqlParameter("@TuKhoa", SqlDbType.NVarChar, 200) { Value = (tuKhoa ?? "").Trim() },
                new SqlParameter("@TriggerType", SqlDbType.VarChar, 40) { Value = triggerType ?? "" },
                new SqlParameter("@TrangThai", SqlDbType.VarChar, 10) { Value = trangThai ?? "" });
        }

        /// <summary>Nạp đầy đủ một Rule (kèm điều kiện và hành động) để sửa hoặc kiểm thử.</summary>
        public AutomationRule? LayTheoID(int ruleId)
        {
            DataTable dt = DbHelper.ExecuteQuery(
                @"SELECT RuleID, RuleName, Description, TriggerType, Priority, IsActive, IsDeleted,
                         CooldownMinutes, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy, RowVersion
                  FROM dbo.AutomationRule
                  WHERE RuleID = @RuleID AND IsDeleted = 0",
                new SqlParameter("@RuleID", SqlDbType.Int) { Value = ruleId });

            if (dt.Rows.Count == 0) return null;

            AutomationRule rule = AutomationMapper.DocRule(dt.Rows[0]);
            rule.Conditions = _conditionRepo.LayTheoRule(ruleId);
            rule.Actions = _actionRepo.LayTheoRule(ruleId);
            return rule;
        }

        /// <summary>Nạp các Rule đang bật của một Trigger, sắp theo Priority tăng dần.</summary>
        public List<AutomationRule> LayDangHoatDong(string triggerType)
        {
            DataTable dtRule = DbHelper.ExecuteQuery(
                @"SELECT RuleID, RuleName, Description, TriggerType, Priority, IsActive, IsDeleted,
                         CooldownMinutes, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy, RowVersion
                  FROM dbo.AutomationRule
                  WHERE TriggerType = @TriggerType AND IsActive = 1 AND IsDeleted = 0
                  ORDER BY Priority, RuleID",
                new SqlParameter("@TriggerType", SqlDbType.VarChar, 40) { Value = triggerType });

            var dsRule = new List<AutomationRule>();
            var theoId = new Dictionary<int, AutomationRule>();

            foreach (DataRow dong in dtRule.Rows)
            {
                AutomationRule rule = AutomationMapper.DocRule(dong);
                dsRule.Add(rule);
                theoId[rule.RuleID] = rule;
            }

            if (dsRule.Count == 0) return dsRule;

            DataTable dtDieuKien = DbHelper.ExecuteQuery(
                @"SELECT c.ConditionID, c.RuleID, c.FieldCode, c.Operator, c.ValueText, c.ValueNumber, c.SortOrder
                  FROM dbo.AutomationCondition c
                  JOIN dbo.AutomationRule r ON r.RuleID = c.RuleID
                  WHERE r.TriggerType = @TriggerType AND r.IsActive = 1 AND r.IsDeleted = 0
                  ORDER BY c.RuleID, c.SortOrder",
                new SqlParameter("@TriggerType", SqlDbType.VarChar, 40) { Value = triggerType });

            foreach (DataRow dong in dtDieuKien.Rows)
            {
                AutomationCondition c = AutomationMapper.DocCondition(dong);
                if (theoId.TryGetValue(c.RuleID, out AutomationRule? rule))
                    rule.Conditions.Add(c);
            }

            DataTable dtHanhDong = DbHelper.ExecuteQuery(
                @"SELECT a.ActionID, a.RuleID, a.ActionType, a.SortOrder, a.ParametersJson, a.ContinueOnError
                  FROM dbo.AutomationAction a
                  JOIN dbo.AutomationRule r ON r.RuleID = a.RuleID
                  WHERE r.TriggerType = @TriggerType AND r.IsActive = 1 AND r.IsDeleted = 0
                  ORDER BY a.RuleID, a.SortOrder",
                new SqlParameter("@TriggerType", SqlDbType.VarChar, 40) { Value = triggerType });

            foreach (DataRow dong in dtHanhDong.Rows)
            {
                AutomationAction a = AutomationMapper.DocAction(dong);
                if (theoId.TryGetValue(a.RuleID, out AutomationRule? rule))
                    rule.Actions.Add(a);
            }

            return dsRule;
        }

        /// <summary>Thêm Rule kèm điều kiện/hành động trong một transaction. Trả về RuleID mới.</summary>
        public int Them(AutomationRule rule)
        {
            int ruleId = 0;

            DbHelper.ExecuteTransaction((conn, tran) =>
            {
                using var cmd = new SqlCommand(@"
                    INSERT INTO dbo.AutomationRule
                        (RuleName, Description, TriggerType, Priority, IsActive, CooldownMinutes, CreatedBy)
                    OUTPUT INSERTED.RuleID
                    VALUES (@RuleName, @Description, @TriggerType, @Priority, @IsActive, @CooldownMinutes, @CreatedBy)",
                    conn, tran);

                cmd.Parameters.Add(new SqlParameter("@RuleName", SqlDbType.NVarChar, 150) { Value = rule.RuleName });
                cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 500)
                {
                    Value = (object?)rule.Description ?? DBNull.Value
                });
                cmd.Parameters.Add(new SqlParameter("@TriggerType", SqlDbType.VarChar, 40) { Value = rule.TriggerType });
                cmd.Parameters.Add(new SqlParameter("@Priority", SqlDbType.Int) { Value = rule.Priority });
                cmd.Parameters.Add(new SqlParameter("@IsActive", SqlDbType.Bit) { Value = rule.IsActive });
                cmd.Parameters.Add(new SqlParameter("@CooldownMinutes", SqlDbType.Int)
                {
                    Value = (object?)rule.CooldownMinutes ?? DBNull.Value
                });
                cmd.Parameters.Add(new SqlParameter("@CreatedBy", SqlDbType.VarChar, 50)
                {
                    Value = (object?)rule.CreatedBy ?? DBNull.Value
                });

                ruleId = Convert.ToInt32(cmd.ExecuteScalar());
                AutomationConditionRepository.GhiDe(conn, tran, ruleId, rule.Conditions);
                AutomationActionRepository.GhiDe(conn, tran, ruleId, rule.Actions);
            });

            return ruleId;
        }

        /// <summary>
        /// Sửa Rule. Kiểm tra RowVersion để tránh ghi đè khi người khác đã lưu trước.
        /// Điều kiện/hành động được ghi đè toàn bộ nhưng lịch sử thực thi cũ vẫn giữ nguyên.
        /// </summary>
        public void Sua(AutomationRule rule)
        {
            DbHelper.ExecuteTransaction((conn, tran) =>
            {
                using var cmd = new SqlCommand(@"
                    UPDATE dbo.AutomationRule
                    SET RuleName = @RuleName,
                        Description = @Description,
                        TriggerType = @TriggerType,
                        Priority = @Priority,
                        IsActive = @IsActive,
                        CooldownMinutes = @CooldownMinutes,
                        UpdatedAt = SYSDATETIME(),
                        UpdatedBy = @UpdatedBy
                    WHERE RuleID = @RuleID AND IsDeleted = 0 AND RowVersion = @RowVersion", conn, tran);

                cmd.Parameters.Add(new SqlParameter("@RuleID", SqlDbType.Int) { Value = rule.RuleID });
                cmd.Parameters.Add(new SqlParameter("@RuleName", SqlDbType.NVarChar, 150) { Value = rule.RuleName });
                cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 500)
                {
                    Value = (object?)rule.Description ?? DBNull.Value
                });
                cmd.Parameters.Add(new SqlParameter("@TriggerType", SqlDbType.VarChar, 40) { Value = rule.TriggerType });
                cmd.Parameters.Add(new SqlParameter("@Priority", SqlDbType.Int) { Value = rule.Priority });
                cmd.Parameters.Add(new SqlParameter("@IsActive", SqlDbType.Bit) { Value = rule.IsActive });
                cmd.Parameters.Add(new SqlParameter("@CooldownMinutes", SqlDbType.Int)
                {
                    Value = (object?)rule.CooldownMinutes ?? DBNull.Value
                });
                cmd.Parameters.Add(new SqlParameter("@UpdatedBy", SqlDbType.VarChar, 50)
                {
                    Value = (object?)rule.UpdatedBy ?? DBNull.Value
                });
                cmd.Parameters.Add(new SqlParameter("@RowVersion", SqlDbType.Timestamp)
                {
                    Value = (object?)rule.RowVersion ?? DBNull.Value
                });

                if (cmd.ExecuteNonQuery() != 1)
                    throw new InvalidOperationException(
                        "Rule đã bị người khác thay đổi hoặc đã bị xóa. Vui lòng tải lại trước khi lưu.");

                AutomationConditionRepository.GhiDe(conn, tran, rule.RuleID, rule.Conditions);
                AutomationActionRepository.GhiDe(conn, tran, rule.RuleID, rule.Actions);
            });
        }

        /// <summary>Xóa mềm: giữ lại Rule và toàn bộ lịch sử thực thi.</summary>
        public void XoaMem(int ruleId, string? nguoiThucHien)
        {
            DbHelper.ExecuteNonQuery(
                @"UPDATE dbo.AutomationRule
                  SET IsDeleted = 1, IsActive = 0, UpdatedAt = SYSDATETIME(), UpdatedBy = @UpdatedBy
                  WHERE RuleID = @RuleID AND IsDeleted = 0",
                new SqlParameter("@RuleID", SqlDbType.Int) { Value = ruleId },
                new SqlParameter("@UpdatedBy", SqlDbType.VarChar, 50)
                {
                    Value = (object?)nguoiThucHien ?? DBNull.Value
                });
        }

        public void DoiTrangThai(int ruleId, bool isActive, string? nguoiThucHien)
        {
            DbHelper.ExecuteNonQuery(
                @"UPDATE dbo.AutomationRule
                  SET IsActive = @IsActive, UpdatedAt = SYSDATETIME(), UpdatedBy = @UpdatedBy
                  WHERE RuleID = @RuleID AND IsDeleted = 0",
                new SqlParameter("@RuleID", SqlDbType.Int) { Value = ruleId },
                new SqlParameter("@IsActive", SqlDbType.Bit) { Value = isActive },
                new SqlParameter("@UpdatedBy", SqlDbType.VarChar, 50)
                {
                    Value = (object?)nguoiThucHien ?? DBNull.Value
                });
        }

        /// <summary>Kiểm tra trùng tên Rule (bỏ qua chính Rule đang sửa).</summary>
        public bool TonTaiTen(string ruleName, int? boQuaRuleId = null)
        {
            object? ketQua = DbHelper.ExecuteScalar(
                @"SELECT TOP 1 1 FROM dbo.AutomationRule
                  WHERE IsDeleted = 0 AND RuleName = @RuleName
                    AND (@BoQuaRuleID IS NULL OR RuleID <> @BoQuaRuleID)",
                new SqlParameter("@RuleName", SqlDbType.NVarChar, 150) { Value = (ruleName ?? "").Trim() },
                new SqlParameter("@BoQuaRuleID", SqlDbType.Int)
                {
                    Value = (object?)boQuaRuleId ?? DBNull.Value
                });

            return ketQua != null && ketQua != DBNull.Value;
        }
    }
}
