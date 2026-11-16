using BTL_LTTQ.DAL;
using BTL_LTTQ.Models.Reports;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BTL_LTTQ.DAL.Repositories.Reports
{
    /// <summary>
    /// Lưu trữ định nghĩa báo cáo vào dbo.ReportDefinition.
    /// Cấu hình chi tiết (cột/lọc/sắp xếp/nhóm/tổng/bố cục) lưu dạng JSON trong cột CauHinh.
    /// </summary>
    public sealed class ReportRepository
    {
        private static readonly JsonSerializerOptions TuyChonJson = new()
        {
            Converters = { new JsonStringEnumConverter() },
            WriteIndented = false
        };

        public static string MaHoaJson(ReportDefinition dinhNghia) =>
            JsonSerializer.Serialize(dinhNghia, TuyChonJson);

        public static ReportDefinition? GiaiMaJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return null;

            try
            {
                return JsonSerializer.Deserialize<ReportDefinition>(json, TuyChonJson);
            }
            catch (JsonException)
            {
                return null;
            }
        }

        /// <summary>Thêm mới hoặc cập nhật; trả về mã báo cáo.</summary>
        public int Luu(ReportDefinition dinhNghia, string nguoiTao)
        {
            ArgumentNullException.ThrowIfNull(dinhNghia);

            string json = MaHoaJson(dinhNghia);
            int soCot = dinhNghia.Cot.Count(c => c.HienThi);
            int soLoc = dinhNghia.Loc.Count(l => l.Bat);

            if (dinhNghia.MaBaoCao > 0 && TonTai(dinhNghia.MaBaoCao))
            {
                const string sql = @"
                    UPDATE dbo.ReportDefinition
                       SET Ten = @Ten,
                           MoTa = @MoTa,
                           MaNguon = @MaNguon,
                           CongKhai = @CongKhai,
                           SoCot = @SoCot,
                           SoBoLoc = @SoBoLoc,
                           CauHinh = @CauHinh,
                           NgayCapNhat = SYSDATETIME()
                     WHERE MaBaoCao = @MaBaoCao";

                DbHelper.ExecuteNonQuery(sql,
                    new SqlParameter("@Ten", SqlDbType.NVarChar, 200) { Value = dinhNghia.Ten },
                    new SqlParameter("@MoTa", SqlDbType.NVarChar, 500) { Value = (object?)dinhNghia.MoTa ?? DBNull.Value },
                    new SqlParameter("@MaNguon", SqlDbType.VarChar, 50) { Value = dinhNghia.MaNguon },
                    new SqlParameter("@CongKhai", SqlDbType.Bit) { Value = dinhNghia.CongKhai },
                    new SqlParameter("@SoCot", SqlDbType.Int) { Value = soCot },
                    new SqlParameter("@SoBoLoc", SqlDbType.Int) { Value = soLoc },
                    new SqlParameter("@CauHinh", SqlDbType.NVarChar, -1) { Value = json },
                    new SqlParameter("@MaBaoCao", SqlDbType.Int) { Value = dinhNghia.MaBaoCao });

                return dinhNghia.MaBaoCao;
            }

            const string sqlInsert = @"
                INSERT INTO dbo.ReportDefinition
                    (Ten, MoTa, MaNguon, NguoiTao, CongKhai, SoCot, SoBoLoc, CauHinh, NgayTao, NgayCapNhat)
                VALUES
                    (@Ten, @MoTa, @MaNguon, @NguoiTao, @CongKhai, @SoCot, @SoBoLoc, @CauHinh, SYSDATETIME(), SYSDATETIME());
                SELECT CAST(SCOPE_IDENTITY() AS int);";

            object? ketQua = DbHelper.ExecuteScalar(sqlInsert,
                new SqlParameter("@Ten", SqlDbType.NVarChar, 200) { Value = dinhNghia.Ten },
                new SqlParameter("@MoTa", SqlDbType.NVarChar, 500) { Value = (object?)dinhNghia.MoTa ?? DBNull.Value },
                new SqlParameter("@MaNguon", SqlDbType.VarChar, 50) { Value = dinhNghia.MaNguon },
                new SqlParameter("@NguoiTao", SqlDbType.VarChar, 50) { Value = nguoiTao },
                new SqlParameter("@CongKhai", SqlDbType.Bit) { Value = dinhNghia.CongKhai },
                new SqlParameter("@SoCot", SqlDbType.Int) { Value = soCot },
                new SqlParameter("@SoBoLoc", SqlDbType.Int) { Value = soLoc },
                new SqlParameter("@CauHinh", SqlDbType.NVarChar, -1) { Value = json });

            return Convert.ToInt32(ketQua);
        }

        public bool TonTai(int maBaoCao)
        {
            object? kq = DbHelper.ExecuteScalar(
                "SELECT TOP 1 1 FROM dbo.ReportDefinition WHERE MaBaoCao = @MaBaoCao",
                new SqlParameter("@MaBaoCao", SqlDbType.Int) { Value = maBaoCao });
            return kq != null && kq != DBNull.Value;
        }

        public ReportDefinition? LayTheoMa(int maBaoCao)
        {
            DataTable dt = DbHelper.ExecuteQuery(
                "SELECT CauHinh FROM dbo.ReportDefinition WHERE MaBaoCao = @MaBaoCao",
                new SqlParameter("@MaBaoCao", SqlDbType.Int) { Value = maBaoCao });

            if (dt.Rows.Count == 0)
                return null;

            var dinhNghia = GiaiMaJson(Convert.ToString(dt.Rows[0]["CauHinh"]) ?? "");
            if (dinhNghia == null)
                return null;

            dinhNghia.MaBaoCao = maBaoCao;
            return dinhNghia;
        }

        /// <summary>
        /// Danh sách báo cáo đã lưu. <paramref name="chiCuaMinh"/> = true chỉ trả báo cáo do
        /// chính người dùng tạo; ngược lại trả báo cáo công khai hoặc của chính người dùng.
        /// </summary>
        public List<ReportListItem> LayDanhSach(string tuKhoa, bool chiCuaMinh, string nguoiTao)
        {
            string sql = chiCuaMinh
                ? @"SELECT MaBaoCao, Ten, MoTa, MaNguon, NguoiTao, CongKhai, SoCot, SoBoLoc, NgayCapNhat
                      FROM dbo.ReportDefinition
                     WHERE NguoiTao = @NguoiTao
                       AND (@TuKhoa = '' OR Ten COLLATE Vietnamese_CI_AI LIKE '%' + @TuKhoa + '%')
                     ORDER BY NgayCapNhat DESC, Ten"
                : @"SELECT MaBaoCao, Ten, MoTa, MaNguon, NguoiTao, CongKhai, SoCot, SoBoLoc, NgayCapNhat
                      FROM dbo.ReportDefinition
                     WHERE (CongKhai = 1 OR NguoiTao = @NguoiTao)
                       AND (@TuKhoa = '' OR Ten COLLATE Vietnamese_CI_AI LIKE '%' + @TuKhoa + '%')
                     ORDER BY NgayCapNhat DESC, Ten";

            DataTable dt = DbHelper.ExecuteQuery(sql,
                new SqlParameter("@TuKhoa", SqlDbType.NVarChar, 200) { Value = tuKhoa ?? "" },
                new SqlParameter("@NguoiTao", SqlDbType.VarChar, 50) { Value = nguoiTao ?? "" });

            var danhSach = new List<ReportListItem>();
            foreach (DataRow r in dt.Rows)
            {
                danhSach.Add(new ReportListItem
                {
                    MaBaoCao = Convert.ToInt32(r["MaBaoCao"]),
                    Ten = Convert.ToString(r["Ten"]) ?? "",
                    MoTa = Convert.ToString(r["MoTa"]) ?? "",
                    MaNguon = Convert.ToString(r["MaNguon"]) ?? "",
                    NguoiTao = Convert.ToString(r["NguoiTao"]) ?? "",
                    CongKhai = r["CongKhai"] != DBNull.Value && Convert.ToBoolean(r["CongKhai"]),
                    SoCot = r["SoCot"] == DBNull.Value ? 0 : Convert.ToInt32(r["SoCot"]),
                    SoBoLoc = r["SoBoLoc"] == DBNull.Value ? 0 : Convert.ToInt32(r["SoBoLoc"]),
                    NgayCapNhat = r["NgayCapNhat"] == DBNull.Value
                        ? DateTime.MinValue
                        : Convert.ToDateTime(r["NgayCapNhat"])
                });
            }

            return danhSach;
        }

        public void Xoa(int maBaoCao)
        {
            DbHelper.ExecuteNonQuery(
                "DELETE FROM dbo.ReportDefinition WHERE MaBaoCao = @MaBaoCao",
                new SqlParameter("@MaBaoCao", SqlDbType.Int) { Value = maBaoCao });
        }

        /// <summary>Đếm nhanh số báo cáo (dùng cho dashboard).</summary>
        public int Dem(string nguoiTao, bool chiCuaMinh)
        {
            string sql = chiCuaMinh
                ? "SELECT COUNT(*) FROM dbo.ReportDefinition WHERE NguoiTao = @NguoiTao"
                : "SELECT COUNT(*) FROM dbo.ReportDefinition WHERE CongKhai = 1 OR NguoiTao = @NguoiTao";

            object? kq = DbHelper.ExecuteScalar(sql,
                new SqlParameter("@NguoiTao", SqlDbType.VarChar, 50) { Value = nguoiTao ?? "" });
            return kq == null || kq == DBNull.Value ? 0 : Convert.ToInt32(kq);
        }
    }
}
