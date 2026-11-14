namespace BTL_LTTQ.Common
{
    public static class CurrentUser
    {
        public static string TenDangNhap { get; private set; } = "";
        public static string VaiTro { get; private set; } = "";
        public static string? MaGV { get; private set; }
        public static string? MaHocVien { get; private set; }

        public static bool IsAdmin => VaiTro == VaiTroConst.Admin;
        public static bool IsGiaoVien => VaiTro == VaiTroConst.GiaoVien;
        public static bool IsHocVien => VaiTro == VaiTroConst.HocVien;

        public static void Dat(string tenDangNhap, string vaiTro, string? maGV, string? maHocVien)
        {
            TenDangNhap = tenDangNhap;
            VaiTro = vaiTro;
            MaGV = maGV;
            MaHocVien = maHocVien;
        }

        public static void Xoa()
        {
            TenDangNhap = "";
            VaiTro = "";
            MaGV = null;
            MaHocVien = null;
        }
    }
}
