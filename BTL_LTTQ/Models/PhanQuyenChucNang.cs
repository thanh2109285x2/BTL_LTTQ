namespace BTL_LTTQ.Models
{
    public class PhanQuyenChucNang
    {
        public string MaChucNang { get; set; } = "";
        public string TenChucNang { get; set; } = "";
        public string Nhom { get; set; } = "";
        public bool HoTroLocTheoChu { get; set; }
        public int ThuTu { get; set; }
        public bool GiaoVienCoQuyen { get; set; }
        public bool GiaoVienChiCuaMinh { get; set; }
        public bool HocVienCoQuyen { get; set; }
        public bool HocVienChiCuaMinh { get; set; }
    }
}
