using System;

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

        //0 = Thứ 2 ... 5 = Thứ 7, 6 = Chủ nhật
        public int Thu => Ngay.DayOfWeek == DayOfWeek.Sunday ? 6 : (int)Ngay.DayOfWeek - 1;

        public string TenThu => Thu switch
        {
            0 => "Thứ 2",
            1 => "Thứ 3",
            2 => "Thứ 4",
            3 => "Thứ 5",
            4 => "Thứ 6",
            5 => "Thứ 7",
            _ => "Chủ nhật"
        };

        public string KhungGio => GioBatDau.HasValue && GioKetThuc.HasValue
            ? $"{GioBatDau.Value:hh\\:mm}-{GioKetThuc.Value:hh\\:mm}"
            : "";
    }
}
