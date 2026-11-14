namespace BTL_LTTQ.Models
{
    public class GiaoVien
    {
        public string MaGV { get; set; } = "";
        public string TenGV { get; set; } = "";
        public DateTime? NgaySinh { get; set; }
        public string? GioiTinh { get; set; }
        public string? DiaChi { get; set; }
        public string? DienThoai { get; set; }
        public string? MaChucDanh { get; set; }
        public string? MaChuyenMon { get; set; }
        public string? MaCQ { get; set; }
        public string? MaTrinhDo { get; set; }
        public string? Anh { get; set; }
        public List<string> MaMonDay { get; set; } = new();
    }
}
