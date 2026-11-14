using System;

namespace BTL_LTTQ.Services.Automation
{
    /// <summary>
    /// Dữ liệu nghiệp vụ đưa vào RulesEngine. Tên thuộc tính phải khớp chính xác với
    /// AutomationConst.FieldCode vì expression được sinh theo tên thuộc tính.
    /// Các thuộc tính nhận diện (MaLop, TenLop...) không nằm trong whitelist nên
    /// không thể được dùng trong điều kiện do Admin cấu hình.
    /// </summary>
    public class AutomationContext
    {
        public string MaLop { get; set; } = "";
        public string TenLop { get; set; } = "";
        public string? TrangThai { get; set; }
        public DateTime? NgayBD { get; set; }
        public DateTime? NgayKT { get; set; }

        public int CurrentEnrollment { get; set; }
        public int ClassCapacity { get; set; }
        public int MinimumEnrollment { get; set; }
        public bool HasTeacher { get; set; }
        public bool HasSchedule { get; set; }
        public int DaysUntilStart { get; set; }
        public int DaysUntilEnd { get; set; }

        // Thông tin nhận diện Entity (để action handler biết là lớp nào)
        public string EntityID { get; set; } = "";
        public string EntityType { get; set; } = "CLASS";
    }
}