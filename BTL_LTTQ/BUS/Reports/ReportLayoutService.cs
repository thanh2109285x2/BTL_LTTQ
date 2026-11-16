using BTL_LTTQ.Common.Reports;
using BTL_LTTQ.Models.Reports;
using System;

namespace BTL_LTTQ.BUS.Reports
{
    /// <summary>Tính toán kích thước trang in từ <see cref="ReportLayoutDef"/>.</summary>
    public static class ReportLayoutService
    {
        private const double MmSangPt = 2.834645669;

        /// <summary>Kích thước trang (mm) theo khổ và hướng giấy.</summary>
        public static (double Rong, double Cao) KichThuocTrang(KhoGiay kho, HuongGiay huong)
        {
            double rong = kho == KhoGiay.A4 ? 210 : 297;
            double cao = kho == KhoGiay.A4 ? 297 : 420;
            return huong == HuongGiay.Doc ? (rong, cao) : (cao, rong);
        }

        /// <summary>Ước lượng số dòng dữ liệu vừa một trang (trừ phần tiêu đề và chân trang).</summary>
        public static int SoDongMoiTrang(ReportLayoutDef boCuc)
        {
            if (boCuc == null) return 30;

            var (_, cao) = KichThuocTrang(boCuc.KhoGiay, boCuc.HuongGiay);
            double vung = cao - boCuc.LeTren - boCuc.LeDuoi;
            double vungPt = vung * MmSangPt;

            double coChu = boCuc.CoChu <= 0 ? 9.5 : boCuc.CoChu;
            double caoDong = coChu * 1.9;

            // Chừa chỗ cho tiêu đề báo cáo và dòng tổng.
            double chua = 60;
            int dong = (int)Math.Floor((vungPt - chua) / caoDong);
            return Math.Max(6, dong);
        }

        /// <summary>Mô tả ngắn khổ giấy để hiển thị (ví dụ "A4 • Dọc").</summary>
        public static string MoTaKho(ReportLayoutDef boCuc)
        {
            if (boCuc == null) return "";
            var (rong, cao) = KichThuocTrang(boCuc.KhoGiay, boCuc.HuongGiay);
            string huong = boCuc.HuongGiay == HuongGiay.Doc ? "Dọc" : "Ngang";
            return $"{boCuc.KhoGiay} • {huong} • {rong:0}×{cao:0} mm";
        }
    }
}
