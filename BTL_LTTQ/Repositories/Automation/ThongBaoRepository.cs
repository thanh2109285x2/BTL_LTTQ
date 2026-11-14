using BTL_LTTQ.DataAccessLayer;
using BTL_LTTQ.Models.Automation;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace BTL_LTTQ.Repositories.Automation
{
    /// <summary>
    /// Truy cập bảng ThongBao. Đây là bảng thông báo dùng chung cho module Automation
    /// (dự án chưa có hệ thống thông báo trước đó).
    /// </summary>
    public class ThongBaoRepository
    {
        public long Them(ThongBao t)
        {
            const string sql = @"
                INSERT INTO dbo.ThongBao
                    (TieuDe, NoiDung, MucDo, DoiTuongVaiTro, DoiTuongTaiKhoan,
                     EntityType, EntityID, RuleID, ExecutionID)
                OUTPUT INSERTED.ThongBaoID
                VALUES (@TieuDe, @NoiDung, @MucDo, @DoiTuongVaiTro, @DoiTuongTaiKhoan,
                        @EntityType, @EntityID, @RuleID, @ExecutionID)";

            object? ketQua = DbHelper.ExecuteScalar(sql,
                new SqlParameter("@TieuDe", SqlDbType.NVarChar, 200) { Value = t.TieuDe },
                new SqlParameter("@NoiDung", SqlDbType.NVarChar, 1000) { Value = t.NoiDung },
                new SqlParameter("@MucDo", SqlDbType.VarChar, 10) { Value = t.MucDo },
                new SqlParameter("@DoiTuongVaiTro", SqlDbType.VarChar, 20)
                {
                    Value = (object?)t.DoiTuongVaiTro ?? DBNull.Value
                },
                new SqlParameter("@DoiTuongTaiKhoan", SqlDbType.VarChar, 50)
                {
                    Value = (object?)t.DoiTuongTaiKhoan ?? DBNull.Value
                },
                new SqlParameter("@EntityType", SqlDbType.VarChar, 40)
                {
                    Value = (object?)t.EntityType ?? DBNull.Value
                },
                new SqlParameter("@EntityID", SqlDbType.VarChar, 20)
                {
                    Value = (object?)t.EntityID ?? DBNull.Value
                },
                new SqlParameter("@RuleID", SqlDbType.Int)
                {
                    Value = (object?)t.RuleID ?? DBNull.Value
                },
                new SqlParameter("@ExecutionID", SqlDbType.BigInt)
                {
                    Value = (object?)t.ExecutionID ?? DBNull.Value
                });

            return Convert.ToInt64(ketQua);
        }

        /// <summary>
        /// Thông báo gần đây theo đối tượng. locVaiTro = null nghĩa là xem tất cả (Admin).
        /// Thông báo không đặt đối tượng được coi là thông báo chung.
        /// </summary>
        public DataTable LayGanDay(string? locVaiTro, string? locTaiKhoan, int soDong = 20)
        {
            const string sql = @"
                SELECT TOP (@SoDong)
                       ThongBaoID, TieuDe, NoiDung, MucDo, DoiTuongVaiTro, DoiTuongTaiKhoan,
                       EntityType, EntityID, RuleID, ExecutionID, DaDoc, CreatedAt
                FROM dbo.ThongBao
                WHERE (@LocVaiTro IS NULL
                       OR DoiTuongVaiTro IS NULL
                       OR DoiTuongVaiTro = @LocVaiTro
                       OR DoiTuongTaiKhoan = @LocTaiKhoan)
                ORDER BY CreatedAt DESC, ThongBaoID DESC";

            return DbHelper.ExecuteQuery(sql,
                new SqlParameter("@SoDong", SqlDbType.Int) { Value = soDong <= 0 ? 20 : soDong },
                new SqlParameter("@LocVaiTro", SqlDbType.VarChar, 20)
                {
                    Value = (object?)locVaiTro ?? DBNull.Value
                },
                new SqlParameter("@LocTaiKhoan", SqlDbType.VarChar, 50)
                {
                    Value = (object?)locTaiKhoan ?? DBNull.Value
                });
        }

        public int DemChuaDoc(string? locVaiTro, string? locTaiKhoan)
        {
            object? ketQua = DbHelper.ExecuteScalar(
                @"SELECT COUNT(*) FROM dbo.ThongBao
                  WHERE DaDoc = 0
                    AND (@LocVaiTro IS NULL
                         OR DoiTuongVaiTro IS NULL
                         OR DoiTuongVaiTro = @LocVaiTro
                         OR DoiTuongTaiKhoan = @LocTaiKhoan)",
                new SqlParameter("@LocVaiTro", SqlDbType.VarChar, 20)
                {
                    Value = (object?)locVaiTro ?? DBNull.Value
                },
                new SqlParameter("@LocTaiKhoan", SqlDbType.VarChar, 50)
                {
                    Value = (object?)locTaiKhoan ?? DBNull.Value
                });

            return ketQua == null || ketQua == DBNull.Value ? 0 : Convert.ToInt32(ketQua);
        }

        public void DanhDauDaDoc(long thongBaoId)
        {
            DbHelper.ExecuteNonQuery(
                "UPDATE dbo.ThongBao SET DaDoc = 1 WHERE ThongBaoID = @ThongBaoID",
                new SqlParameter("@ThongBaoID", SqlDbType.BigInt) { Value = thongBaoId });
        }

        public void DanhDauTatCaDaDoc(string? locVaiTro, string? locTaiKhoan)
        {
            DbHelper.ExecuteNonQuery(
                @"UPDATE dbo.ThongBao
                  SET DaDoc = 1
                  WHERE DaDoc = 0
                    AND (@LocVaiTro IS NULL
                         OR DoiTuongVaiTro IS NULL
                         OR DoiTuongVaiTro = @LocVaiTro
                         OR DoiTuongTaiKhoan = @LocTaiKhoan)",
                new SqlParameter("@LocVaiTro", SqlDbType.VarChar, 20)
                {
                    Value = (object?)locVaiTro ?? DBNull.Value
                },
                new SqlParameter("@LocTaiKhoan", SqlDbType.VarChar, 50)
                {
                    Value = (object?)locTaiKhoan ?? DBNull.Value
                });
        }
    }
}
