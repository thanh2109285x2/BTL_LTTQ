using BTL_LTTQ.Common.Reports;
using System;
using System.Collections.Generic;

namespace BTL_LTTQ.Models.Reports
{
    /// <summary>Loại của một dòng trong kết quả báo cáo.</summary>
    public enum LoaiDongBaoCao
    {
        DuLieu = 0,
        TieuDeNhom = 1,
        TongNhom = 2,
        TongBaoCao = 3
    }

    /// <summary>Một cột trong kết quả đã dựng (đã gắn tiêu đề/định dạng).</summary>
    public sealed class ReportResultColumn
    {
        public string MaTruong { get; set; } = "";
        public string TieuDe { get; set; } = "";
        public KieuDuLieu Kieu { get; set; } = KieuDuLieu.Chuoi;
        public CanLeCot CanLe { get; set; } = CanLeCot.Trai;
        public int DoRong { get; set; } = 120;
        public string DinhDang { get; set; } = "";
        public string TienTo { get; set; } = "";
        public string HauTo { get; set; } = "";
        public bool InDam { get; set; }
    }

    /// <summary>Một dòng kết quả (dữ liệu, tiêu đề nhóm hoặc dòng tổng).</summary>
    public sealed class ReportResultRow
    {
        public LoaiDongBaoCao Loai { get; set; } = LoaiDongBaoCao.DuLieu;
        public object?[] GiaTri { get; set; } = Array.Empty<object?>();
        public int CapNhom { get; set; }
        public string? NhanNhom { get; set; }
        public bool InDam { get; set; }
        public bool LaTong => Loai is LoaiDongBaoCao.TongNhom or LoaiDongBaoCao.TongBaoCao;
    }

    /// <summary>Kết quả chạy báo cáo hoàn chỉnh.</summary>
    public sealed class ReportResult
    {
        public List<ReportResultColumn> Cot { get; set; } = new();
        public List<ReportResultRow> Dong { get; set; } = new();

        /// <summary>Số dòng dữ liệu gốc (không tính dòng nhóm/tổng).</summary>
        public int SoDongDuLieu { get; set; }

        /// <summary>Số dòng tối đa bị giới hạn (nếu truy vấn chạm trần an toàn).</summary>
        public bool ChamGioiHan { get; set; }

        public long ThoiGianChayMs { get; set; }
        public bool DungDuLieuMau { get; set; }
        public string TieuDe { get; set; } = "";
        public string PhuDe { get; set; } = "";
        public DateTime ThoiDiemChay { get; set; } = DateTime.Now;

        /// <summary>Số dòng hiển thị trên mỗi trang (ước lượng theo bố cục).</summary>
        public int SoDongMoiTrang { get; set; } = 30;

        public int SoTrangUocTinh => SoDongMoiTrang <= 0 ? 1 : (int)Math.Ceiling(Dong.Count / (double)SoDongMoiTrang);

        public static ReportResult Rong(string tieuDe = "") => new() { TieuDe = tieuDe };
    }
}
