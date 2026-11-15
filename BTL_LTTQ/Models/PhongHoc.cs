using System;
using System.Collections.Generic;
using System.Text;

namespace BTL_LTTQ.Models
{
    //Phòng học: mã phòng, tên phòng, sức chứa
    public class PhongHoc
    {
        public string MaPhong { get; set; } = "";
        public string TenPhong { get; set; } = "";
        public int? SucChua { get; set; }
    }
}
