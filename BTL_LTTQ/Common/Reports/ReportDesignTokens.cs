using System.Drawing;

namespace BTL_LTTQ.Common.Reports
{
    /// <summary>
    /// Token thiết kế riêng cho module Report Builder.
    /// Kế thừa bảng màu của <see cref="GiaoDien"/> để không tạo một design system thứ hai,
    /// chỉ bổ sung thang khoảng cách, cỡ chữ và kích thước vùng làm việc.
    /// </summary>
    public static class ReportDesignTokens
    {
        // ---- Màu ----
        public static readonly Color NenTrang = Color.White;
        public static readonly Color NenPanel = Color.FromArgb(248, 250, 253);
        public static readonly Color NenHeader = Color.FromArgb(240, 244, 250);
        public static readonly Color NenChon = GiaoDien.MauChon;
        public static readonly Color ChuChinh = GiaoDien.MauChu;
        public static readonly Color ChuBaoCao = Color.FromArgb(39, 54, 77);
        public static readonly Color ChuTieuDeBaoCao = GiaoDien.MauChinhDam;
        public static readonly Color ChuPhu = Color.FromArgb(108, 117, 125);
        public static readonly Color ChuMo = Color.FromArgb(150, 158, 168);
        public static readonly Color Vien = GiaoDien.MauVien;
        public static readonly Color VienNhat = Color.FromArgb(226, 232, 240);
        public static readonly Color Nhan = GiaoDien.MauChinh;
        public static readonly Color NhanDam = GiaoDien.MauChinhDam;
        public static readonly Color NhanNhat = GiaoDien.MauHangLe;
        public static readonly Color CanhBao = Color.FromArgb(191, 120, 4);
        public static readonly Color NguyHiem = GiaoDien.MauNguyHiem;
        public static readonly Color ThanhCong = Color.FromArgb(35, 134, 76);

        // ---- Khoảng cách (spacing scale) ----
        public const int Khoang4 = 4;
        public const int Khoang8 = 8;
        public const int Khoang12 = 12;
        public const int Khoang16 = 16;
        public const int Khoang20 = 20;
        public const int Khoang24 = 24;

        // ---- Kích thước vùng ----
        public const int ChieuCaoToolbar = 56;
        public const int ChieuCaoThanhTrangThai = 30;
        public const int ChieuCaoDong = 30;
        public const int ChieuCaoHeaderMuc = 34;
        public const int ChieuRongPanelTrai = 284;
        public const int ChieuRongPanelPhai = 304;
        public const int ChieuCaoNut = 34;
        public const int ChieuRongNut = 96;

        // ---- Typography ----
        public static readonly Font ChuTieuDeTrang = new("Segoe UI", 18F, FontStyle.Bold);
        public static readonly Font ChuPhuDeTrang = new("Segoe UI", 10F);
        public static readonly Font ChuTieuDeMuc = new("Segoe UI", 10.5F, FontStyle.Bold);
        public static readonly Font ChuTieuDeThe = new("Segoe UI", 10F, FontStyle.Bold);
        public static readonly Font ChuThuong = new("Segoe UI", 9.5F);
        public static readonly Font ChuNho = new("Segoe UI", 8.5F);
        public static readonly Font ChuNhan = new("Segoe UI", 9F, FontStyle.Bold);
        public static readonly Font ChuSo = new("Consolas", 9.5F);
    }
}
