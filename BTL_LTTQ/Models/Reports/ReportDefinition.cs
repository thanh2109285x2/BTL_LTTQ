using BTL_LTTQ.Common.Reports;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BTL_LTTQ.Models.Reports
{
    /// <summary>
    /// Định nghĩa một cột hiển thị trong báo cáo.
    /// </summary>
    public sealed class ReportColumnDef
    {
        public string MaTruong { get; set; } = "";
        public string TieuDe { get; set; } = "";
        public int DoRong { get; set; } = 120;
        public CanLeCot CanLe { get; set; } = CanLeCot.Trai;
        public bool HienThi { get; set; } = true;
        public bool InDam { get; set; }
        public string DinhDang { get; set; } = "";
        public string TienTo { get; set; } = "";
        public string HauTo { get; set; } = "";
        public int ThuTu { get; set; }

        public ReportColumnDef Clone() => (ReportColumnDef)MemberwiseClone();
    }

    /// <summary>Một điều kiện lọc.</summary>
    public sealed class ReportFilterDef
    {
        public string MaTruong { get; set; } = "";
        public ToanTuLoc ToanTu { get; set; } = ToanTuLoc.Bang;
        public string GiaTri1 { get; set; } = "";
        public string GiaTri2 { get; set; } = "";
        public LienKetDieuKien LienKet { get; set; } = LienKetDieuKien.Va;
        public bool Bat { get; set; } = true;

        public ReportFilterDef Clone() => (ReportFilterDef)MemberwiseClone();
    }

    /// <summary>Một tiêu chí sắp xếp.</summary>
    public sealed class ReportSortDef
    {
        public string MaTruong { get; set; } = "";
        public HuongSapXep Huong { get; set; } = HuongSapXep.TangDan;

        public ReportSortDef Clone() => (ReportSortDef)MemberwiseClone();
    }

    /// <summary>Một cấp nhóm dữ liệu.</summary>
    public sealed class ReportGroupDef
    {
        public string MaTruong { get; set; } = "";
        public bool HienThiChanNhom { get; set; } = true;
        public bool HienThiTongNhom { get; set; } = true;

        public ReportGroupDef Clone() => (ReportGroupDef)MemberwiseClone();
    }

    /// <summary>Một dòng tổng hợp.</summary>
    public sealed class ReportSummaryDef
    {
        public string MaTruong { get; set; } = "";
        public HamTongHop Ham { get; set; } = HamTongHop.Khong;
        public PhamViTongHop PhamVi { get; set; } = PhamViTongHop.ToanBaoCao;
        public string TieuDe { get; set; } = "";
        public bool HienThi { get; set; } = true;

        public ReportSummaryDef Clone() => (ReportSummaryDef)MemberwiseClone();
    }

    /// <summary>Bố cục trang in và định dạng chung của báo cáo.</summary>
    public sealed class ReportLayoutDef
    {
        public KhoGiay KhoGiay { get; set; } = KhoGiay.A4;
        public HuongGiay HuongGiay { get; set; } = HuongGiay.Doc;
        public int LeTren { get; set; } = 15;
        public int LeDuoi { get; set; } = 15;
        public int LeTrai { get; set; } = 15;
        public int LePhai { get; set; } = 15;
        public bool HienDuongKe { get; set; } = true;
        public bool HienTongCong { get; set; } = true;
        public bool HienSoTrang { get; set; } = true;
        public bool LapLaiTieuDe { get; set; } = true;
        public bool NenXenKe { get; set; } = true;
        public string KieuChu { get; set; } = "Segoe UI";
        public float CoChu { get; set; } = 9.5f;

        public ReportLayoutDef Clone() => (ReportLayoutDef)MemberwiseClone();
    }

    /// <summary>
    /// Toàn bộ định nghĩa báo cáo do người dùng dựng.
    /// Đối tượng này được tuần tự hóa JSON để lưu vào dbo.ReportDefinition.
    /// </summary>
    public sealed class ReportDefinition
    {
        public int MaBaoCao { get; set; }
        public string Ten { get; set; } = "";
        public string MoTa { get; set; } = "";
        public string MaNguon { get; set; } = "";
        public string TieuDe { get; set; } = "";
        public string PhuDe { get; set; } = "";
        public string NguoiTao { get; set; } = "";
        public bool CongKhai { get; set; } = true;
        public int GioiHanDong { get; set; } = 1000;
        public DateTime NgayTao { get; set; }
        public DateTime NgayCapNhat { get; set; }

        public List<ReportColumnDef> Cot { get; set; } = new();
        public List<ReportFilterDef> Loc { get; set; } = new();
        public List<ReportSortDef> SapXep { get; set; } = new();
        public List<ReportGroupDef> Nhom { get; set; } = new();
        public List<ReportSummaryDef> TongHop { get; set; } = new();
        public ReportLayoutDef BoCuc { get; set; } = new();

        /// <summary>Các cột đang bật, theo đúng thứ tự hiển thị.</summary>
        public IEnumerable<ReportColumnDef> CotHienThi =>
            Cot.Where(c => c.HienThi).OrderBy(c => c.ThuTu);

        public ReportDefinition Clone()
        {
            var ban = (ReportDefinition)MemberwiseClone();
            ban.Cot = Cot.Select(c => c.Clone()).ToList();
            ban.Loc = Loc.Select(c => c.Clone()).ToList();
            ban.SapXep = SapXep.Select(c => c.Clone()).ToList();
            ban.Nhom = Nhom.Select(c => c.Clone()).ToList();
            ban.TongHop = TongHop.Select(c => c.Clone()).ToList();
            ban.BoCuc = BoCuc.Clone();
            return ban;
        }

        /// <summary>Đếm nhanh số thành phần cấu hình (dùng cho dashboard/badge).</summary>
        public int SoThanhPhan => Cot.Count + Loc.Count + SapXep.Count + Nhom.Count + TongHop.Count;

        public void SapXepLaiThuTuCot()
        {
            int i = 0;
            foreach (var cot in Cot.OrderBy(c => c.ThuTu))
                cot.ThuTu = i++;
        }
    }
}
