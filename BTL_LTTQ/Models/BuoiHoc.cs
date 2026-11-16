using System;
using BTL_LTTQ.Common;

namespace BTL_LTTQ.Models
{
    //Một buổi học đã được xếp lịch (kết quả của bộ xếp lịch)
    public class BuoiHoc
    {
        public int MaBuoi { get; set; }
        public string MaLop { get; set; } = "";
        public string TenLop { get; set; } = "";
        public string MaGV { get; set; } = "";
        public string TenGV { get; set; } = "";
        public string TenMon { get; set; } = "";
        public DateTime Ngay { get; set; }
        public string MaCa { get; set; } = "";
        public string TenCa { get; set; } = "";
        public TimeSpan? GioBatDau { get; set; }
        public TimeSpan? GioKetThuc { get; set; }
        public string MaPhong { get; set; } = "";
        public string TenPhong { get; set; } = "";

        public int Thu => ThuTrongTuanConst.TuNgay(Ngay);

        public string TenThu => ThuTrongTuanConst.TenHienThi((byte)Thu);

        public string KhungGio => GioBatDau.HasValue && GioKetThuc.HasValue
            ? $"{GioBatDau.Value:hh\\:mm}-{GioKetThuc.Value:hh\\:mm}"
            : "";
    }
}
