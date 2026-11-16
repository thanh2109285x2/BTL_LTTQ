namespace BTL_LTTQ.Models
{
    public class CaHoc
    {
        public string MaCa { get; set; } = "";
        public string TenCa { get; set; } = "";
        public TimeSpan? GioBatDau { get; set; }
        public TimeSpan? GioKetThuc { get; set; }

        public string KhungGio => GioBatDau.HasValue && GioKetThuc.HasValue
            ? $"{GioBatDau.Value:hh\\:mm}-{GioKetThuc.Value:hh\\:mm}"
            : "";
    }
}
