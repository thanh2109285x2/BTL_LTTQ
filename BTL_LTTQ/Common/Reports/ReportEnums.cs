using System;
using System.Collections.Generic;
using System.Linq;

namespace BTL_LTTQ.Common.Reports
{
    /// <summary>Căn lề nội dung của một cột trong báo cáo.</summary>
    public enum CanLeCot { Trai, Giua, Phai }

    /// <summary>Kiểu dữ liệu logic của một trường (dùng để chọn định dạng và toán tử lọc phù hợp).</summary>
    public enum KieuDuLieu { Chuoi, SoNguyen, SoThuc, Ngay, Gio, Logic, TienTe, PhanTram }

    /// <summary>Toán tử so sánh dùng trong điều kiện lọc.</summary>
    public enum ToanTuLoc
    {
        Bang, Khac, Chua, KhongChua, BatDauBang, KetThucBang,
        LonHon, LonHonBang, NhoHon, NhoHonBang, TrongKhoang, TrongDanhSach, KhongTrongDanhSach
    }

    /// <summary>Cách ghép nhiều điều kiện lọc.</summary>
    public enum LienKetDieuKien { Va, Hoac }

    /// <summary>Hướng sắp xếp.</summary>
    public enum HuongSapXep { TangDan, GiamDan }

    /// <summary>Hàm tổng hợp cho dòng tổng cộng / chân nhóm.</summary>
    public enum HamTongHop { Khong, Dem, Tong, TrungBinh, NhoNhat, LonNhat }

    /// <summary>Phạm vi áp dụng của một dòng tổng hợp.</summary>
    public enum PhamViTongHop { ToanBaoCao, TheoNhom }

    /// <summary>Khổ giấy in.</summary>
    public enum KhoGiay { A4, A3 }

    /// <summary>Hướng giấy in.</summary>
    public enum HuongGiay { Doc, Ngang }

    /// <summary>Trạng thái của khung xem trước báo cáo.</summary>
    public enum TrangThaiXemTruoc { Trong, DangTai, CoDuLieu, Loi }

    /// <summary>Mức độ của một thông báo kiểm tra cấu hình.</summary>
    public enum MucDoThongDiep { Loi, CanhBao, ThongTin }

    /// <summary>Nhãn tiếng Việt cho các enum ở trên (tách khỏi logic để tránh hard-code chuỗi rải rác).</summary>
    public static class ReportEnumText
    {
        public static string Ten(CanLeCot v) => v switch
        {
            CanLeCot.Trai => "Trái",
            CanLeCot.Giua => "Giữa",
            CanLeCot.Phai => "Phải",
            _ => v.ToString()
        };

        public static string Ten(KieuDuLieu v) => v switch
        {
            KieuDuLieu.Chuoi => "Chuỗi",
            KieuDuLieu.SoNguyen => "Số nguyên",
            KieuDuLieu.SoThuc => "Số thực",
            KieuDuLieu.Ngay => "Ngày",
            KieuDuLieu.Gio => "Giờ",
            KieuDuLieu.Logic => "Đúng/Sai",
            KieuDuLieu.TienTe => "Tiền tệ",
            KieuDuLieu.PhanTram => "Phần trăm",
            _ => v.ToString()
        };

        public static string Ten(ToanTuLoc v) => v switch
        {
            ToanTuLoc.Bang => "Bằng",
            ToanTuLoc.Khac => "Khác",
            ToanTuLoc.Chua => "Chứa",
            ToanTuLoc.KhongChua => "Không chứa",
            ToanTuLoc.BatDauBang => "Bắt đầu bằng",
            ToanTuLoc.KetThucBang => "Kết thúc bằng",
            ToanTuLoc.LonHon => "Lớn hơn",
            ToanTuLoc.LonHonBang => "Lớn hơn hoặc bằng",
            ToanTuLoc.NhoHon => "Nhỏ hơn",
            ToanTuLoc.NhoHonBang => "Nhỏ hơn hoặc bằng",
            ToanTuLoc.TrongKhoang => "Trong khoảng",
            ToanTuLoc.TrongDanhSach => "Thuộc danh sách",
            ToanTuLoc.KhongTrongDanhSach => "Không thuộc danh sách",
            _ => v.ToString()
        };

        public static string Ten(LienKetDieuKien v) => v == LienKetDieuKien.Va ? "VÀ" : "HOẶC";

        public static string Ten(HuongSapXep v) => v == HuongSapXep.TangDan ? "Tăng dần" : "Giảm dần";

        public static string Ten(HamTongHop v) => v switch
        {
            HamTongHop.Khong => "(Không)",
            HamTongHop.Dem => "Đếm (COUNT)",
            HamTongHop.Tong => "Tổng (SUM)",
            HamTongHop.TrungBinh => "Trung bình (AVG)",
            HamTongHop.NhoNhat => "Nhỏ nhất (MIN)",
            HamTongHop.LonNhat => "Lớn nhất (MAX)",
            _ => v.ToString()
        };

        public static string Ten(PhamViTongHop v) => v == PhamViTongHop.ToanBaoCao ? "Toàn báo cáo" : "Theo nhóm";

        public static string Ten(KhoGiay v) => v == KhoGiay.A4 ? "A4 (210 × 297 mm)" : "A3 (297 × 420 mm)";

        public static string Ten(HuongGiay v) => v == HuongGiay.Doc ? "Dọc (Portrait)" : "Ngang (Landscape)";

        /// <summary>Các toán tử hợp lệ theo kiểu dữ liệu của trường.</summary>
        public static IReadOnlyList<ToanTuLoc> ToanTuCho(KieuDuLieu kieu)
        {
            if (kieu == KieuDuLieu.Chuoi)
            {
                return new[]
                {
                    ToanTuLoc.Bang, ToanTuLoc.Khac, ToanTuLoc.Chua, ToanTuLoc.KhongChua,
                    ToanTuLoc.BatDauBang, ToanTuLoc.KetThucBang,
                    ToanTuLoc.TrongDanhSach, ToanTuLoc.KhongTrongDanhSach
                };
            }

            if (kieu == KieuDuLieu.Logic)
                return new[] { ToanTuLoc.Bang, ToanTuLoc.Khac };

            return new[]
            {
                ToanTuLoc.Bang, ToanTuLoc.Khac, ToanTuLoc.LonHon, ToanTuLoc.LonHonBang,
                ToanTuLoc.NhoHon, ToanTuLoc.NhoHonBang, ToanTuLoc.TrongKhoang,
                ToanTuLoc.TrongDanhSach, ToanTuLoc.KhongTrongDanhSach
            };
        }

        /// <summary>Toán tử nào cần tới hai giá trị (khoảng).</summary>
        public static bool CanHaiGiaTri(ToanTuLoc v) => v == ToanTuLoc.TrongKhoang;

        /// <summary>Toán tử nào nhận danh sách giá trị phân tách bằng dấu phẩy.</summary>
        public static bool LaDanhSach(ToanTuLoc v) =>
            v == ToanTuLoc.TrongDanhSach || v == ToanTuLoc.KhongTrongDanhSach;

        /// <summary>Toán tử nào dùng mẫu LIKE.</summary>
        public static bool LaLike(ToanTuLoc v) =>
            v is ToanTuLoc.Chua or ToanTuLoc.KhongChua or ToanTuLoc.BatDauBang or ToanTuLoc.KetThucBang;

        public static IReadOnlyList<HamTongHop> CacHamTongHop() =>
            new[] { HamTongHop.Khong, HamTongHop.Dem, HamTongHop.Tong, HamTongHop.TrungBinh, HamTongHop.NhoNhat, HamTongHop.LonNhat };
    }
}
