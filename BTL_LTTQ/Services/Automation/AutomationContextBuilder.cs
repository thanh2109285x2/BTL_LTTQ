using BTL_LTTQ.Common;
using BTL_LTTQ.DataAccessLayer;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace BTL_LTTQ.Services.Automation
{
    /// <summary>Lấy dữ liệu lớp học tại thời điểm đánh giá và chuyển thành AutomationContext.</summary>
    public class AutomationContextBuilder
    {
        public AutomationContext? XayDung(string maLop)
        {
            if (string.IsNullOrWhiteSpace(maLop)) return null;

            DataTable dt = DbHelper.ExecuteQuery(@"
                SELECT l.MaLop, l.TenLop, l.MaGV, l.NgayBD, l.NgayKT,
                       l.SiSoToiDa, l.SiSoToiThieu, l.TrangThai,
                       (SELECT COUNT(*) FROM dbo.DangKy d WHERE d.MaLop = l.MaLop) AS SiSo,
                       CASE WHEN EXISTS (SELECT 1 FROM dbo.BuoiHoc b WHERE b.MaLop = l.MaLop)
                            THEN 1 ELSE 0 END AS CoLich
                FROM dbo.LopHoc l
                WHERE l.MaLop = @MaLop",
                new SqlParameter("@MaLop", SqlDbType.VarChar, 20) { Value = maLop });

            if (dt.Rows.Count == 0) return null;

            DataRow r = dt.Rows[0];
            DateTime ngayBD = Convert.ToDateTime(r["NgayBD"]).Date;
            DateTime ngayKT = Convert.ToDateTime(r["NgayKT"]).Date;
            DateTime homNay = DateTime.Today;

            return new AutomationContext
            {
                MaLop = Convert.ToString(r["MaLop"]) ?? "",
                TenLop = Convert.ToString(r["TenLop"]) ?? "",
                TrangThai = r["TrangThai"] == DBNull.Value ? null : Convert.ToString(r["TrangThai"]),
                NgayBD = ngayBD,
                NgayKT = ngayKT,
                CurrentEnrollment = Convert.ToInt32(r["SiSo"]),
                ClassCapacity = Convert.ToInt32(r["SiSoToiDa"]),
                MinimumEnrollment = Convert.ToInt32(r["SiSoToiThieu"]),
                HasTeacher = r["MaGV"] != DBNull.Value && !string.IsNullOrWhiteSpace(Convert.ToString(r["MaGV"])),
                HasSchedule = Convert.ToInt32(r["CoLich"]) > 0,
                DaysUntilStart = (ngayBD - homNay).Days,
                DaysUntilEnd = (ngayKT - homNay).Days
            };
        }
    }
}
