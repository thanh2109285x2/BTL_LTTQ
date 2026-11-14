using System;
using System.Collections.Generic;

namespace BTL_LTTQ.Models
{
    //Dữ liệu đầu vào cho bộ xếp lịch
    public class DuLieuXepLich
    {
        public List<LopXepLich> Lop { get; set; } = new List<LopXepLich>();
        public List<PhongXepLich> Phong { get; set; } = new List<PhongXepLich>();
        public List<string> Ca { get; set; } = new List<string>();
    }

    public class LopXepLich
    {
        public string MaLop { get; set; } = "";
        public string TenLop { get; set; } = "";
        public string MaGV { get; set; } = "";
        public string MaMon { get; set; } = "";
        public string TenMon { get; set; } = "";
        public string MaCaUuTien { get; set; } = "";
        public string MaPhongUuTien { get; set; } = "";
        public int SoBuoiTuan { get; set; } = 3;
        public DateTime NgayBD { get; set; }
        public DateTime NgayKT { get; set; }

        //Các thứ ưu tiên theo mẫu thời gian của lớp (0 = Thứ 2 ... 6 = CN)
        public List<int> ThuUuTien { get; set; } = new List<int>();

        //Danh sách học viên đã đăng ký lớp
        public List<string> HocVien { get; set; } = new List<string>();

        public int SiSo => HocVien.Count;
    }

    public class PhongXepLich
    {
        public string MaPhong { get; set; } = "";
        public string TenPhong { get; set; } = "";
        public int SucChua { get; set; }
    }
}
