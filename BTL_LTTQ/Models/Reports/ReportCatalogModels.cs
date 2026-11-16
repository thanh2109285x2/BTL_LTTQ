using BTL_LTTQ.Common.Reports;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BTL_LTTQ.Models.Reports
{
    /// <summary>
    /// Loại phạm vi dữ liệu (bảo mật) mà một nguồn dữ liệu áp dụng.
    /// Nguồn dữ liệu khai báo sẵn, KHÔNG do người dùng nhập, nên luôn nằm trong danh sách trắng.
    /// </summary>
    public enum PhamViNguon
    {
        Khong = 0,
        ChoHocVien = 1,
        ChoLopHoc = 2
    }

    /// <summary>
    /// Một trường dữ liệu trong danh mục (danh sách trắng).
    /// <see cref="BieuThuc"/> là biểu thức SQL cố định do hệ thống cung cấp,
    /// người dùng chỉ chọn theo <see cref="Ma"/> chứ không bao giờ nhập SQL.
    /// </summary>
    public sealed class FieldDefinition
    {
        public string Ma { get; set; } = "";
        public string BieuThuc { get; set; } = "";
        public string TieuDe { get; set; } = "";
        public KieuDuLieu Kieu { get; set; } = KieuDuLieu.Chuoi;
        public string Nhom { get; set; } = "Khác";
        public bool CoTheNhom { get; set; }
        public bool CoTheTongHop { get; set; }
        public bool MacDinh { get; set; }
        public int DoRong { get; set; } = 120;
        public CanLeCot CanLeMacDinh { get; set; } = CanLeCot.Trai;
        public string DinhDangMacDinh { get; set; } = "";
        public string GhiChu { get; set; } = "";
    }

    /// <summary>
    /// Một nguồn dữ liệu (bảng/khung nhìn) được phép dùng để dựng báo cáo.
    /// </summary>
    public sealed class DataSourceDefinition
    {
        public string Ma { get; set; } = "";
        public string Ten { get; set; } = "";
        public string MoTa { get; set; } = "";
        public string BieuTuong { get; set; } = "📄";
        public string Nhom { get; set; } = "Khác";

        /// <summary>Mệnh đề FROM + JOIN cố định (đã kiểm duyệt, không chứa tham số người dùng).</summary>
        public string FromSql { get; set; } = "";
        public string AliasChinh { get; set; } = "";
        public string KhoaChinh { get; set; } = "";
        public PhamViNguon PhamVi { get; set; } = PhamViNguon.Khong;

        public List<FieldDefinition> Truong { get; set; } = new();

        public FieldDefinition? TimTruong(string ma) =>
            Truong.FirstOrDefault(t => string.Equals(t.Ma, ma, StringComparison.OrdinalIgnoreCase));

        public bool CoTruong(string ma) => TimTruong(ma) != null;

        public IEnumerable<string> CacNhomTruong() =>
            Truong.Select(t => t.Nhom).Distinct(StringComparer.OrdinalIgnoreCase);

        public int SoTruongTongHop => Truong.Count(t => t.CoTheTongHop);
        public int SoTruongNhom => Truong.Count(t => t.CoTheNhom);
    }

    /// <summary>Một mẫu báo cáo dựng sẵn (cấu hình mẫu, không phải tệp RDLC giả).</summary>
    public sealed class ReportTemplate
    {
        public string Ma { get; set; } = "";
        public string Ten { get; set; } = "";
        public string MoTa { get; set; } = "";
        public string BieuTuong { get; set; } = "🧩";
        public string MaNguon { get; set; } = "";
        public string Nhom { get; set; } = "Phổ biến";

        /// <summary>Hàm dựng cấu hình mẫu. Trả về ReportDefinition mới.</summary>
        public Func<ReportDefinition> Tao { get; set; } = () => new ReportDefinition();
    }
}
