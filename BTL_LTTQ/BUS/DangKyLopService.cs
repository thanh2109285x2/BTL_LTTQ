using BTL_LTTQ.Common;
using BTL_LTTQ.DAL;
using System.Data;

namespace BTL_LTTQ.BUS
{
    public class DangKyLopService
    {
        private readonly DangKyRepository _repo = new DangKyRepository();

        public DataTable LayLopChoHocVien(string maHocVien, string tuKhoa = "")
        {
            if (!CurrentUser.IsHocVien || string.IsNullOrWhiteSpace(maHocVien))
                return new DataTable();
            return _repo.LayLopChoHocVien(maHocVien, tuKhoa ?? "");
        }

        public string DangKy(string maHocVien, string maLop)
        {
            if (!PermissionService.Can(ChucNangConst.DANGKYLOP))
                return "Bạn không có quyền sử dụng chức năng này.";
            if (!CurrentUser.IsHocVien || string.IsNullOrWhiteSpace(maHocVien))
                return "Chức năng đăng ký lớp chỉ dành cho học viên.";
            if (string.IsNullOrWhiteSpace(maLop))
                return "Vui lòng chọn một lớp trong danh sách để đăng ký.";
            return _repo.Them(maHocVien, maLop);
        }

        public string HuyDangKy(string maHocVien, string maLop)
        {
            if (!PermissionService.Can(ChucNangConst.DANGKYLOP))
                return "Bạn không có quyền sử dụng chức năng này.";
            if (!CurrentUser.IsHocVien || string.IsNullOrWhiteSpace(CurrentUser.MaHocVien))
                return "Chức năng đăng ký lớp chỉ dành cho học viên.";
            if (string.IsNullOrWhiteSpace(maLop))
                return "Vui lòng chọn một lớp trong danh sách để hủy đăng ký.";
            if (!string.Equals(CurrentUser.MaHocVien, maHocVien, StringComparison.OrdinalIgnoreCase))
                return "Bạn chỉ có thể hủy đăng ký của chính mình.";
            return _repo.Huy(CurrentUser.MaHocVien, maLop);
        }
    }
}
