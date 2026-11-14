using BTL_LTTQ.DataAccessLayer;
using BTL_LTTQ.Models.Automation;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace BTL_LTTQ.Repositories.Automation
{
    /// <summary>
    /// Đọc/ghi AutomationAction. Tham số hành động lưu dạng JSON, không phải expression.
    /// Không kiểm tra quyền ở đây (xem AutomationConditionRepository).
    /// </summary>
    public class AutomationActionRepository
    {
        public List<AutomationAction> LayTheoRule(int ruleId)
        {
            DataTable dt = DbHelper.ExecuteQuery(
                @"SELECT ActionID, RuleID, ActionType, SortOrder, ParametersJson, ContinueOnError
                  FROM dbo.AutomationAction
                  WHERE RuleID = @RuleID
                  ORDER BY SortOrder",
                new SqlParameter("@RuleID", SqlDbType.Int) { Value = ruleId });

            var ketQua = new List<AutomationAction>();
            foreach (DataRow dong in dt.Rows)
                ketQua.Add(AutomationMapper.DocAction(dong));
            return ketQua;
        }

        public int Them(AutomationAction a)
        {
            const string sql = @"
                INSERT INTO dbo.AutomationAction (RuleID, ActionType, SortOrder, ParametersJson, ContinueOnError)
                OUTPUT INSERTED.ActionID
                VALUES (@RuleID, @ActionType, @SortOrder, @ParametersJson, @ContinueOnError)";

            object? ketQua = DbHelper.ExecuteScalar(sql, ThamSo(a));
            return Convert.ToInt32(ketQua);
        }

        public void Sua(AutomationAction a)
        {
            const string sql = @"
                UPDATE dbo.AutomationAction
                SET ActionType = @ActionType,
                    SortOrder = @SortOrder,
                    ParametersJson = @ParametersJson,
                    ContinueOnError = @ContinueOnError
                WHERE ActionID = @ActionID";

            var thamSo = new List<SqlParameter>(ThamSo(a))
            {
                new SqlParameter("@ActionID", SqlDbType.Int) { Value = a.ActionID }
            };

            if (DbHelper.ExecuteNonQuery(sql, thamSo.ToArray()) != 1)
                throw new InvalidOperationException("Không cập nhật được hành động: hành động không tồn tại.");
        }

        public void Xoa(int actionId)
        {
            DbHelper.ExecuteNonQuery(
                "DELETE FROM dbo.AutomationAction WHERE ActionID = @ActionID",
                new SqlParameter("@ActionID", SqlDbType.Int) { Value = actionId });
        }

        /// <summary>Ghi đè toàn bộ hành động của Rule trong transaction đang mở.</summary>
        internal static void GhiDe(SqlConnection conn, SqlTransaction tran, int ruleId, IEnumerable<AutomationAction> ds)
        {
            using (var xoa = new SqlCommand("DELETE FROM dbo.AutomationAction WHERE RuleID = @RuleID", conn, tran))
            {
                xoa.Parameters.Add(new SqlParameter("@RuleID", SqlDbType.Int) { Value = ruleId });
                xoa.ExecuteNonQuery();
            }

            int thuTu = 1;
            foreach (AutomationAction a in ds)
            {
                using var them = new SqlCommand(@"
                    INSERT INTO dbo.AutomationAction (RuleID, ActionType, SortOrder, ParametersJson, ContinueOnError)
                    VALUES (@RuleID, @ActionType, @SortOrder, @ParametersJson, @ContinueOnError)", conn, tran);

                them.Parameters.Add(new SqlParameter("@RuleID", SqlDbType.Int) { Value = ruleId });
                them.Parameters.Add(new SqlParameter("@ActionType", SqlDbType.VarChar, 40) { Value = a.ActionType });
                them.Parameters.Add(new SqlParameter("@SortOrder", SqlDbType.Int) { Value = thuTu++ });
                them.Parameters.Add(new SqlParameter("@ParametersJson", SqlDbType.NVarChar, -1)
                {
                    Value = string.IsNullOrWhiteSpace(a.ParametersJson) ? "{}" : a.ParametersJson
                });
                them.Parameters.Add(new SqlParameter("@ContinueOnError", SqlDbType.Bit) { Value = a.ContinueOnError });

                them.ExecuteNonQuery();
            }
        }

        private static SqlParameter[] ThamSo(AutomationAction a)
        {
            return new[]
            {
                new SqlParameter("@RuleID", SqlDbType.Int) { Value = a.RuleID },
                new SqlParameter("@ActionType", SqlDbType.VarChar, 40) { Value = a.ActionType },
                new SqlParameter("@SortOrder", SqlDbType.Int) { Value = a.SortOrder },
                new SqlParameter("@ParametersJson", SqlDbType.NVarChar, -1)
                {
                    Value = string.IsNullOrWhiteSpace(a.ParametersJson) ? "{}" : a.ParametersJson
                },
                new SqlParameter("@ContinueOnError", SqlDbType.Bit) { Value = a.ContinueOnError }
            };
        }
    }
}
