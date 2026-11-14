namespace BTL_LTTQ.Models.Import
{
    /// <summary>Một lỗi của một dòng dữ liệu (hoặc lỗi cấp tệp với SoDong = 0).</summary>
    public class ImportLoi
    {
        public int SoDong { get; set; }
        public string Cot { get; set; } = "";
        public string? GiaTri { get; set; }
        public string ThongDiep { get; set; } = "";
    }
}
