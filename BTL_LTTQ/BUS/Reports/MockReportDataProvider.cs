using BTL_LTTQ.Common.Reports;
using BTL_LTTQ.Models.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BTL_LTTQ.BUS.Reports
{
    /// <summary>
    /// Nguồn dữ liệu MẪU dùng cho chế độ thiết kế/demo.
    /// KHÔNG truy vấn cơ sở dữ liệu và KHÔNG được bật mặc định.
    /// Chỉ có tác dụng minh họa bố cục khi chưa có dữ liệu thật.
    /// </summary>
    public sealed class MockReportDataProvider : IReportDataProvider
    {
        private readonly int _soDong;

        public MockReportDataProvider(int soDong = 24)
        {
            _soDong = Math.Max(3, soDong);
        }

        public string Ten => "Dữ liệu mẫu (thiết kế)";
        public bool LaDuLieuMau => true;

        public DataTable LayDuLieu(ReportQueryBuilder.KetQua truyVan, DataSourceDefinition nguon)
        {
            var bang = new DataTable();

            var tatCa = truyVan.CotHienThi.Concat(truyVan.CotPhu).Distinct(StringComparer.OrdinalIgnoreCase).ToList();

            foreach (string ma in tatCa)
            {
                var truong = nguon.TimTruong(ma);
                Type kieu = truong == null ? typeof(string) : KieuClr(truong.Kieu);
                bang.Columns.Add(ma, kieu);
            }

            string[] ho = { "Nguyễn", "Trần", "Lê", "Phạm", "Hoàng", "Vũ", "Đặng", "Bùi", "Đỗ", "Ngô" };
            string[] dem = { "Văn", "Thị", "Minh", "Hữu", "Thanh", "Quốc", "Ngọc", "Anh", "Thu", "Hải" };
            string[] ten = { "An", "Bình", "Châu", "Dũng", "Giang", "Hà", "Khánh", "Linh", "Nam", "Phương", "Quân", "Sơn", "Trang", "Vy" };
            string[] mon = { "Tin học văn phòng", "Tiếng Anh A2", "Tiếng Anh B1", "Tiếng Nhật N5", "Tiếng Hàn sơ cấp", "Lập trình C#" };
            string[] lop = { "TH01", "TH02", "TA01", "TA02", "TN01", "LH01" };
            string[] phong = { "PH01", "PH02", "PH03" };
            string[] ca = { "Ca sáng", "Ca chiều", "Ca tối" };
            string[] trangThai = { "SapKhaiGiang", "DangHoc", "KetThuc" };
            string[] gioiTinh = { "Nam", "Nữ" };

            var rnd = new Random(20260101);

            for (int i = 0; i < _soDong; i++)
            {
                var dong = bang.NewRow();
                foreach (string ma in tatCa)
                {
                    var truong = nguon.TimTruong(ma);
                    dong[ma] = truong == null ? DBNull.Value : GiaTriMau(truong, i, rnd,
                        ho, dem, ten, mon, lop, phong, ca, trangThai, gioiTinh);
                }
                bang.Rows.Add(dong);
            }

            return bang;
        }

        private static object GiaTriMau(FieldDefinition truong, int i, Random rnd,
            string[] ho, string[] dem, string[] ten, string[] mon, string[] lop,
            string[] phong, string[] ca, string[] trangThai, string[] gioiTinh)
        {
            string ma = truong.Ma.ToLowerInvariant();
            string hoTen = $"{ho[rnd.Next(ho.Length)]} {dem[rnd.Next(dem.Length)]} {ten[rnd.Next(ten.Length)]}";

            if (ma.Contains("tenhocvien") || ma.Contains("tengv") || ma.Contains("tennguoi"))
                return hoTen;
            if (ma.Contains("tenmon"))
                return mon[rnd.Next(mon.Length)];
            if (ma.Contains("tenlop"))
                return $"Lớp {lop[rnd.Next(lop.Length)]}";
            if (ma.Contains("tenphong"))
                return $"Phòng {phong[rnd.Next(phong.Length)]}";
            if (ma.Contains("tenca"))
                return ca[rnd.Next(ca.Length)];
            if (ma.Contains("tenthoigian"))
                return new[] { "2-4-6", "3-5-7" }[rnd.Next(2)];
            if (ma.Contains("gioitinh"))
                return gioiTinh[rnd.Next(gioiTinh.Length)];
            if (ma.Contains("diachi"))
                return $"{rnd.Next(1, 300)} đường số {rnd.Next(1, 20)}, TP.HCM";
            if (ma.Contains("dienthoai"))
                return $"09{rnd.Next(10000000, 99999999)}";
            if (ma.Contains("email"))
                return $"hv{i + 1:000}@example.edu.vn";
            if (ma.Contains("trangthai") || ma.Contains("trangthaithanhtoan"))
                return ma.Contains("thanhtoan")
                    ? (i % 3 == 0 ? "Chưa nộp" : "Đã nộp")
                    : trangThai[rnd.Next(trangThai.Length)];
            if (ma.Contains("ketqua"))
                return new[] { "Đạt", "Chưa đạt", "Chưa có điểm" }[rnd.Next(3)];
            if (ma.Contains("xephang"))
                return "A" + rnd.Next(1, 4);
            if (ma.Contains("mamon"))
                return "MH" + rnd.Next(1, 8).ToString("00");
            if (ma.Contains("malop"))
                return lop[rnd.Next(lop.Length)];
            if (ma.Contains("magv"))
                return "GV" + rnd.Next(1, 12).ToString("00");
            if (ma.Contains("mahocvien"))
                return "HV" + (i + 1).ToString("000");
            if (ma.Contains("maphong"))
                return phong[rnd.Next(phong.Length)];
            if (ma.Contains("macq"))
                return "CQ" + rnd.Next(1, 5).ToString("00");
            if (ma.Contains("mannghe") || ma.Contains("manghe"))
                return "NN" + rnd.Next(1, 6).ToString("00");
            if (ma.Contains("machucdanh"))
                return "CD" + rnd.Next(1, 4).ToString("00");
            if (ma.Contains("maca"))
                return "CA" + rnd.Next(1, 4).ToString("00");
            if (ma.Contains("mathoigian"))
                return "TG" + rnd.Next(1, 4).ToString("00");

            switch (truong.Kieu)
            {
                case KieuDuLieu.Ngay:
                    return DateTime.Today.AddDays(-rnd.Next(0, 400)).AddDays(i % 7);
                case KieuDuLieu.Gio:
                    return new TimeSpan(rnd.Next(7, 20), 0, 0);
                case KieuDuLieu.SoNguyen:
                    return ma.Contains("siso") ? rnd.Next(12, 40) : rnd.Next(1, 50);
                case KieuDuLieu.SoThuc:
                    return Math.Round(3 + rnd.NextDouble() * 7, 1);
                case KieuDuLieu.TienTe:
                    return (decimal)(1_200_000 + rnd.Next(0, 40) * 100_000);
                case KieuDuLieu.PhanTram:
                    return Math.Round(rnd.NextDouble() * 100, 1);
                case KieuDuLieu.Logic:
                    return rnd.Next(2) == 0;
                default:
                    return truong.Ma;
            }
        }

        private static Type KieuClr(KieuDuLieu kieu) => kieu switch
        {
            KieuDuLieu.SoNguyen => typeof(int),
            KieuDuLieu.SoThuc or KieuDuLieu.PhanTram => typeof(double),
            KieuDuLieu.TienTe => typeof(decimal),
            KieuDuLieu.Ngay or KieuDuLieu.Gio => typeof(DateTime),
            KieuDuLieu.Logic => typeof(bool),
            _ => typeof(string)
        };
    }
}
