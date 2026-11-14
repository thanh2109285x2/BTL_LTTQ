using System;

namespace BTL_LTTQ.Models.Automation
{
    /// <summary>Thông báo do Automation tạo ra, hiển thị trên Dashboard.</summary>
    public class ThongBao
    {
        public long ThongBaoID { get; set; }
        public string TieuDe { get; set; } = "";
        public string NoiDung { get; set; } = "";
        public string MucDo { get; set; } = "INFO";
        public string? DoiTuongVaiTro { get; set; }
        public string? DoiTuongTaiKhoan { get; set; }
        public string? EntityType { get; set; }
        public string? EntityID { get; set; }
        public int? RuleID { get; set; }
        public long? ExecutionID { get; set; }
        public bool DaDoc { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
