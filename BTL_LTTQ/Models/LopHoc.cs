using System;
using System.Collections.Generic;
using System.Text;

namespace BTL_LTTQ.Models
{
    public class LopHoc
    {
        public string MaLop { get; set; } = "";
        public string TenLop { get; set; } = "";
        public string MaMon { get; set; }
        public string MaGV { get; set; }
        public DateTime NgayBD { get; set; }
        public DateTime NgayKT { get; set; }
        public string MaCa { get; set; }
        public string MaThoiGian { get; set; }
        public string MaPhong { get; set; }

        //Giới hạn đăng ký và số buổi mỗi tuần khi xếp thời khóa biểu
        public int SiSoToiDa { get; set; } = 30;
        public int SoBuoiTuan { get; set; } = 3;
    }
}
