using System.Globalization;

namespace BTL_LTTQ.Common
{
    public static class ThuTrongTuanConst
    {
        public const byte ThuHai = 2;
        public const byte ThuBa = 3;
        public const byte ThuTu = 4;
        public const byte ThuNam = 5;
        public const byte ThuSau = 6;
        public const byte ThuBay = 7;
        public const byte ChuNhat = 8;

        public static IReadOnlyList<byte> CacThu { get; } = Array.AsReadOnly(new[]
        {
            ThuHai, ThuBa, ThuTu, ThuNam, ThuSau, ThuBay, ChuNhat
        });

        public static byte TuNgay(DateTime ngay) => (byte)(((int)ngay.DayOfWeek + 6) % 7 + ThuHai);

        public static string TenHienThi(byte thu) => thu switch
        {
            ThuHai => "Thứ 2",
            ThuBa => "Thứ 3",
            ThuTu => "Thứ 4",
            ThuNam => "Thứ 5",
            ThuSau => "Thứ 6",
            ThuBay => "Thứ 7",
            ChuNhat => "Chủ Nhật",
            _ => ""
        };

        public static string TaoTenThoiGian(IEnumerable<byte> cacThu)
        {
            byte[] thuDaChon = cacThu.Distinct().OrderBy(thu => thu).ToArray();
            string[] cacNgayTrongTuan = thuDaChon
                .Where(thu => thu != ChuNhat)
                .Select(thu => thu.ToString(CultureInfo.InvariantCulture))
                .ToArray();
            bool coChuNhat = thuDaChon.Contains(ChuNhat);

            if (cacNgayTrongTuan.Length == 0)
                return coChuNhat ? TenHienThi(ChuNhat) : "";

            string ten = "Thứ " + string.Join(" - ", cacNgayTrongTuan);
            return coChuNhat ? ten + " - " + TenHienThi(ChuNhat) : ten;
        }
    }
}
