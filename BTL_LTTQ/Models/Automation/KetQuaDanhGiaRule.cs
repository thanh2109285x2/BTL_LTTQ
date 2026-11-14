using System.Collections.Generic;

namespace BTL_LTTQ.Models.Automation
{
    /// <summary>Kết quả đánh giá một điều kiện (không phải hành động).</summary>
    public class KetQuaDieuKien
    {
        public int ConditionID { get; set; }
        public int SortOrder { get; set; }
        public string FieldCode { get; set; } = "";
        public string Operator { get; set; } = "";
        public string MoTa { get; set; } = "";
        public string? GiaTriMongDoi { get; set; }
        public string? GiaTriThucTe { get; set; }
        public bool ThanhCong { get; set; }
        public string? Loi { get; set; }
    }

    /// <summary>Kết quả đánh giá một Rule. Không chứa kết quả thực thi hành động.</summary>
    public class KetQuaDanhGiaRule
    {
        /// <summary>Rule cấu hình hợp lệ và RulesEngine chạy được.</summary>
        public bool HopLe { get; set; }

        /// <summary>Tất cả điều kiện đều đúng (chỉ có ý nghĩa khi HopLe = true).</summary>
        public bool TatCaDieuKienDung { get; set; }

        public string? Loi { get; set; }
        public List<KetQuaDieuKien> DieuKien { get; set; } = new();

        /// <summary>Bản sao hành động của Rule để tầng thực thi dùng lại.</summary>
        public List<AutomationAction> HanhDong { get; set; } = new();
    }
}
