using System;

namespace BTL_LTTQ.Models
{
    //Một lượt học viên đăng ký lớp
    public class DangKy
    {
        public string MaHocVien { get; set; } = "";
        public string MaLop { get; set; } = "";
        public DateTime NgayDangKy { get; set; }
    }
}
