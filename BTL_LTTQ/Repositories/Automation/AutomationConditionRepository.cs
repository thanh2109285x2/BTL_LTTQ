using BTL_LTTQ.DataAccessLayer;
using BTL_LTTQ.Models.Automation;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace BTL_LTTQ.Repositories.Automation
{
    /// <summary>
    /// Đọc/ghi AutomationCondition. Không kiểm tra quyền: engine cần đọc điều kiện khi trigger
    /// được phát từ ngữ cảnh không phải Admin. Tầng Service/Form chịu trách nhiệm phân quyền.
    /// </summary>
    public class AutomationConditionRepository
    {
        public List<AutomationCondition> LayTheoRule(int ruleId)
        {
            DataTable dt = DbHelper.ExecuteQuery(
                @"SELECT ConditionID, RuleID, FieldCode, Operator, ValueText, ValueNumber, SortOrder
                  FROM dbo.AutomationCondition
                  WHERE RuleID = @RuleID
                  ORDER BY SortOrder",
                new SqlParameter("@RuleID", SqlDbType.Int) { Value = ruleId });

            var ketQua = new List<AutomationCondition>();
            foreach (DataRow dong in dt.Rows)
                ketQua.Add(AutomationMapper.DocCondition(dong));
            return ketQua;
        }

        public int Them(AutomationCondition c)
        {
            const string sql = @"
                INSERT INTO dbo.AutomationCondition (RuleID, FieldCode, Operator, ValueText, ValueNumber, SortOrder)
                OUTPUT INSERTED.ConditionID
                VALUES (@RuleID, @FieldCode, @Operator, @ValueText, @ValueNumber, @SortOrder)";

            object? ketQua = DbHelper.ExecuteScalar(sql, ThamSo(c));
            return Convert.ToInt32(ketQua);
        }

        public void Sua(AutomationCondition c)
        {
            const string sql = @"
                UPDATE dbo.AutomationCondition
                SET FieldCode = @FieldCode,
                    Operator = @Operator,
                    ValueText = @ValueText,
                    ValueNumber = @ValueNumber,
                    SortOrder = @SortOrder
                WHERE ConditionID = @ConditionID";

            var thamSo = new List<SqlParameter>(ThamSo(c))
            {
                new SqlParameter("@ConditionID", SqlDbType.Int) { Value = c.ConditionID }
            };

            if (DbHelper.ExecuteNonQuery(sql, thamSo.ToArray()) != 1)
                throw new InvalidOperationException("Không cập nhật được điều kiện: điều kiện không tồn tại.");
        }

        public void Xoa(int conditionId)
        {
            DbHelper.ExecuteNonQuery(
                "DELETE FROM dbo.AutomationCondition WHERE ConditionID = @ConditionID",
                new SqlParameter("@ConditionID", SqlDbType.Int) { Value = conditionId });
        }

        /// <summary>Ghi đè toàn bộ điều kiện của Rule trong transaction đang mở.</summary>
        internal static void GhiDe(SqlConnection conn, SqlTransaction tran, int ruleId, IEnumerable<AutomationCondition> ds)
        {
            using (var xoa = new SqlCommand("DELETE FROM dbo.AutomationCondition WHERE RuleID = @RuleID", conn, tran))
            {
                xoa.Parameters.Add(new SqlParameter("@RuleID", SqlDbType.Int) { Value = ruleId });
                xoa.ExecuteNonQuery();
            }

            int thuTu = 1;
            foreach (AutomationCondition c in ds)
            {
                using var them = new SqlCommand(@"
                    INSERT INTO dbo.AutomationCondition (RuleID, FieldCode, Operator, ValueText, ValueNumber, SortOrder)
                    VALUES (@RuleID, @FieldCode, @Operator, @ValueText, @ValueNumber, @SortOrder)", conn, tran);

                them.Parameters.Add(new SqlParameter("@RuleID", SqlDbType.Int) { Value = ruleId });
                them.Parameters.Add(new SqlParameter("@FieldCode", SqlDbType.VarChar, 40) { Value = c.FieldCode });
                them.Parameters.Add(new SqlParameter("@Operator", SqlDbType.VarChar, 30) { Value = c.Operator });
                them.Parameters.Add(new SqlParameter("@ValueText", SqlDbType.NVarChar, 200)
                {
                    Value = (object?)c.ValueText ?? DBNull.Value
                });
                them.Parameters.Add(new SqlParameter("@ValueNumber", SqlDbType.Decimal)
                {
                    Precision = 18,
                    Scale = 2,
                    Value = (object?)c.ValueNumber ?? DBNull.Value
                });
                them.Parameters.Add(new SqlParameter("@SortOrder", SqlDbType.Int) { Value = thuTu++ });

                them.ExecuteNonQuery();
            }
        }

        private static SqlParameter[] ThamSo(AutomationCondition c)
        {
            return new[]
            {
                new SqlParameter("@RuleID", SqlDbType.Int) { Value = c.RuleID },
                new SqlParameter("@FieldCode", SqlDbType.VarChar, 40) { Value = c.FieldCode },
                new SqlParameter("@Operator", SqlDbType.VarChar, 30) { Value = c.Operator },
                new SqlParameter("@ValueText", SqlDbType.NVarChar, 200)
                {
                    Value = (object?)c.ValueText ?? DBNull.Value
                },
                new SqlParameter("@ValueNumber", SqlDbType.Decimal)
                {
                    Precision = 18,
                    Scale = 2,
                    Value = (object?)c.ValueNumber ?? DBNull.Value
                },
                new SqlParameter("@SortOrder", SqlDbType.Int) { Value = c.SortOrder }
            };
        }
    }
}
