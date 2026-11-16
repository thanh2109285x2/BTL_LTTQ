using BTL_LTTQ.Common.Reports;
using System;

namespace BTL_LTTQ.Models.Reports
{
    /// <summary>Một dòng trong danh sách báo cáo đã lưu (nhẹ, không kèm cấu hình).</summary>
    public sealed class ReportListItem
    {
        public int MaBaoCao { get; set; }
        public string Ten { get; set; } = "";
        public string MoTa { get; set; } = "";
        public string MaNguon { get; set; } = "";
        public string TenNguon { get; set; } = "";
        public string NguoiTao { get; set; } = "";
        public bool CongKhai { get; set; }
        public DateTime NgayCapNhat { get; set; }
        public int SoCot { get; set; }
        public int SoBoLoc { get; set; }
    }

    /// <summary>Một thông báo kiểm tra cấu hình (lỗi / cảnh báo / gợi ý).</summary>
    public sealed class ReportValidationMessage
    {
        public MucDoThongDiep MucDo { get; set; } = MucDoThongDiep.ThongTin;
        public string ThongDiep { get; set; } = "";
        public string MaTruong { get; set; } = "";

        public static ReportValidationMessage Loi(string thongDiep, string maTruong = "") =>
            new() { MucDo = MucDoThongDiep.Loi, ThongDiep = thongDiep, MaTruong = maTruong };

        public static ReportValidationMessage CanhBao(string thongDiep, string maTruong = "") =>
            new() { MucDo = MucDoThongDiep.CanhBao, ThongDiep = thongDiep, MaTruong = maTruong };

        public static ReportValidationMessage ThongTin(string thongDiep, string maTruong = "") =>
            new() { MucDo = MucDoThongDiep.ThongTin, ThongDiep = thongDiep, MaTruong = maTruong };
    }

    /// <summary>Thống kê tổng quan cho màn hình Dashboard.</summary>
    public sealed class ReportDashboardStats
    {
        public int SoBaoCaoDaTao { get; set; }
        public int SoNguonDuLieu { get; set; }
        public int SoTruongKhaDung { get; set; }
        public int SoMauDungSan { get; set; }
        public int SoBaoCaoCuaToi { get; set; }
        public DateTime LanCapNhatGanNhat { get; set; }
    }
}
