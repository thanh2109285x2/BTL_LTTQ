namespace BTL_LTTQ.Models.Import
{
    /// <summary>Thông tin lớp cần cho việc kiểm tra sức chứa khi nhập đăng ký.</summary>
    public class LopThongTinImport
    {
        public string MaLop { get; set; } = "";
        public string TenLop { get; set; } = "";
        public int SiSoToiDa { get; set; }
        public int SiSoHienTai { get; set; }
        public string TrangThai { get; set; } = "";
    }
}
