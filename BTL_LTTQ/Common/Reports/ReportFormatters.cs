using BTL_LTTQ.Models.Reports;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BTL_LTTQ.Common.Reports
{
    /// <summary>
    /// Định dạng / phân tích giá trị theo kiểu dữ liệu logic của trường.
    /// Tách khỏi giao diện để dùng chung cho lưới, xem trước và xuất tệp.
    /// </summary>
    public static class ReportFormatters
    {
        public static readonly CultureInfo Vi = CultureInfo.GetCultureInfo("vi-VN");

        /// <summary>Định dạng mặc định gợi ý theo kiểu dữ liệu.</summary>
        public static string DinhDangMacDinh(KieuDuLieu kieu) => kieu switch
        {
            KieuDuLieu.SoNguyen => "0",
            KieuDuLieu.SoThuc => "0.##",
            KieuDuLieu.TienTe => "#,##0",
            KieuDuLieu.PhanTram => "0.##",
            KieuDuLieu.Ngay => "dd/MM/yyyy",
            KieuDuLieu.Gio => "HH:mm",
            _ => ""
        };

        /// <summary>Căn lề mặc định gợi ý theo kiểu dữ liệu.</summary>
        public static CanLeCot CanLeMacDinh(KieuDuLieu kieu) => kieu switch
        {
            KieuDuLieu.SoNguyen or KieuDuLieu.SoThuc or KieuDuLieu.TienTe or KieuDuLieu.PhanTram
                => CanLeCot.Phai,
            KieuDuLieu.Ngay or KieuDuLieu.Gio or KieuDuLieu.Logic => CanLeCot.Giua,
            _ => CanLeCot.Trai
        };

        /// <summary>Định dạng một giá trị để hiển thị.</summary>
        public static string Hien(object? giaTri, KieuDuLieu kieu, string dinhDang,
            string tienTo = "", string hauTo = "")
        {
            string than = HienThan(giaTri, kieu, dinhDang);
            if (than.Length == 0)
                return "";

            return (tienTo ?? "") + than + (hauTo ?? "");
        }

        private static string HienThan(object? giaTri, KieuDuLieu kieu, string dinhDang)
        {
            if (giaTri == null || giaTri == DBNull.Value)
                return "";

            switch (kieu)
            {
                case KieuDuLieu.SoNguyen:
                    if (LaSo(giaTri, out double sng))
                        return Math.Round(sng).ToString(string.IsNullOrEmpty(dinhDang) ? "0" : dinhDang, Vi);
                    break;

                case KieuDuLieu.SoThuc:
                    if (LaSo(giaTri, out double st))
                        return st.ToString(string.IsNullOrEmpty(dinhDang) ? "0.##" : dinhDang, Vi);
                    break;

                case KieuDuLieu.TienTe:
                    if (LaSo(giaTri, out double tt))
                        return tt.ToString(string.IsNullOrEmpty(dinhDang) ? "#,##0" : dinhDang, Vi);
                    break;

                case KieuDuLieu.PhanTram:
                    if (LaSo(giaTri, out double pt))
                        return pt.ToString(string.IsNullOrEmpty(dinhDang) ? "0.##" : dinhDang, Vi) + "%";
                    break;

                case KieuDuLieu.Ngay:
                    if (LaNgay(giaTri, out DateTime ngay))
                        return ngay.ToString(string.IsNullOrEmpty(dinhDang) ? "dd/MM/yyyy" : dinhDang, Vi);
                    break;

                case KieuDuLieu.Gio:
                    if (LaNgay(giaTri, out DateTime gio))
                        return gio.ToString(string.IsNullOrEmpty(dinhDang) ? "HH:mm" : dinhDang, Vi);
                    break;

                case KieuDuLieu.Logic:
                    if (LaBool(giaTri, out bool b))
                        return b ? "Đúng" : "Sai";
                    break;
            }

            return Convert.ToString(giaTri, Vi) ?? "";
        }

        /// <summary>Chuyển giá trị về chuỗi "thô" để lọc / nhóm / sắp xếp.</summary>
        public static string Khoa(object? giaTri)
        {
            if (giaTri == null || giaTri == DBNull.Value)
                return "";

            if (giaTri is DateTime dt)
                return dt.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

            if (giaTri is IFormattable f)
                return f.ToString(null, CultureInfo.InvariantCulture) ?? "";

            return giaTri.ToString() ?? "";
        }

        public static bool LaSo(object? giaTri, out double so)
        {
            so = 0;
            if (giaTri == null || giaTri == DBNull.Value) return false;

            if (giaTri is double d) { so = d; return true; }
            if (giaTri is float f2) { so = f2; return true; }
            if (giaTri is decimal m) { so = (double)m; return true; }
            if (giaTri is int i) { so = i; return true; }
            if (giaTri is long l) { so = l; return true; }
            if (giaTri is short sh) { so = sh; return true; }
            if (giaTri is byte by) { so = by; return true; }

            return double.TryParse(Convert.ToString(giaTri, CultureInfo.InvariantCulture),
                NumberStyles.Any, CultureInfo.InvariantCulture, out so);
        }

        public static bool LaNgay(object? giaTri, out DateTime ngay)
        {
            ngay = default;
            if (giaTri == null || giaTri == DBNull.Value) return false;
            if (giaTri is DateTime dt) { ngay = dt; return true; }
            if (giaTri is DateTimeOffset dto) { ngay = dto.DateTime; return true; }

            return DateTime.TryParse(Convert.ToString(giaTri, CultureInfo.InvariantCulture),
                CultureInfo.InvariantCulture, DateTimeStyles.None, out ngay);
        }

        public static bool LaBool(object? giaTri, out bool b)
        {
            b = false;
            if (giaTri == null || giaTri == DBNull.Value) return false;
            if (giaTri is bool bb) { b = bb; return true; }

            string s = Convert.ToString(giaTri, CultureInfo.InvariantCulture) ?? "";
            if (s == "1") { b = true; return true; }
            if (s == "0") { b = false; return true; }

            return bool.TryParse(s, out b);
        }

        /// <summary>Phân tích một giá trị người dùng nhập thành tham số an toàn theo kiểu dữ liệu.</summary>
        public static bool ThuPhanTich(string? chuoi, KieuDuLieu kieu, out object? giaTri)
        {
            giaTri = null;
            chuoi = (chuoi ?? "").Trim();
            if (chuoi.Length == 0)
                return false;

            switch (kieu)
            {
                case KieuDuLieu.SoNguyen:
                    if (int.TryParse(chuoi, NumberStyles.Integer, Vi, out int si) ||
                        int.TryParse(chuoi, NumberStyles.Integer, CultureInfo.InvariantCulture, out si))
                    {
                        giaTri = si;
                        return true;
                    }
                    return false;

                case KieuDuLieu.SoThuc:
                case KieuDuLieu.TienTe:
                case KieuDuLieu.PhanTram:
                    if (double.TryParse(chuoi, NumberStyles.Any, Vi, out double sd) ||
                        double.TryParse(chuoi, NumberStyles.Any, CultureInfo.InvariantCulture, out sd))
                    {
                        giaTri = sd;
                        return true;
                    }
                    return false;

                case KieuDuLieu.Ngay:
                case KieuDuLieu.Gio:
                    if (DateTime.TryParse(chuoi, Vi, DateTimeStyles.None, out DateTime nd) ||
                        DateTime.TryParse(chuoi, CultureInfo.InvariantCulture, DateTimeStyles.None, out nd))
                    {
                        giaTri = nd;
                        return true;
                    }
                    return false;

                case KieuDuLieu.Logic:
                    if (LaBool(chuoi, out bool bl))
                    {
                        giaTri = bl;
                        return true;
                    }
                    return false;

                default:
                    giaTri = chuoi;
                    return true;
            }
        }

        /// <summary>Tách danh sách giá trị người dùng nhập (phân tách bằng dấu phẩy hoặc chấm phẩy).</summary>
        public static List<string> TachDanhSach(string? chuoi) =>
            (chuoi ?? "")
                .Split(new[] { ',', ';', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => x.Length > 0)
                .ToList();
    }
}
