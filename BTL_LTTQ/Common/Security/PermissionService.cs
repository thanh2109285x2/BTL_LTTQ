using BTL_LTTQ.DAL;
using BTL_LTTQ.Models;
using System.Data;

namespace BTL_LTTQ.Common
{
    public static class PermissionService
    {
        private static readonly PhanQuyenDAL _phanQuyenDAL = new PhanQuyenDAL();
        private static readonly Dictionary<string, (bool CoQuyen, bool ChiCuaMinh)> _quyen =
            new Dictionary<string, (bool CoQuyen, bool ChiCuaMinh)>(StringComparer.OrdinalIgnoreCase);

        public static void Luu(IEnumerable<PhanQuyenChucNang> danhSach) => _phanQuyenDAL.Luu(danhSach);

        //Nạp lại quyền theo vai trò hiện tại; không có dòng = không có quyền.
        public static void Load()
        {
            _quyen.Clear();

            if (CurrentUser.IsAdmin || (!CurrentUser.IsGiaoVien && !CurrentUser.IsHocVien))
                return;

            DataTable duLieu = _phanQuyenDAL.LayTheoVaiTro(CurrentUser.VaiTro);
            var quyenMoi = new Dictionary<string, (bool CoQuyen, bool ChiCuaMinh)>(StringComparer.OrdinalIgnoreCase);
            foreach (DataRow dong in duLieu.Rows)
            {
                string maChucNang = dong["MaChucNang"].ToString() ?? "";
                if (maChucNang == "") continue;

                quyenMoi[maChucNang] = (
                    Convert.ToBoolean(dong["CoQuyen"]),
                    Convert.ToBoolean(dong["ChiCuaMinh"]));
            }

            foreach (var quyen in quyenMoi)
                _quyen[quyen.Key] = quyen.Value;
        }
         
        //Xóa các quyền đã nạp; không có dòng = không có quyền.
        public static void Clear()
        {
            _quyen.Clear();
        }

        //Kiểm tra quyền theo mã chức năng; không có dòng = không có quyền.
        public static bool Can(string maChucNang)
        {
            if (string.IsNullOrWhiteSpace(CurrentUser.TenDangNhap) || string.IsNullOrWhiteSpace(CurrentUser.VaiTro))
                return false;

            if (CurrentUser.IsAdmin)
                return true;

            if (string.Equals(maChucNang, ChucNangConst.PHANQUYEN, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(maChucNang, ChucNangConst.TAIKHOAN, StringComparison.OrdinalIgnoreCase))
                return false;

            if (string.Equals(maChucNang, ChucNangConst.BAOCAO_TAO, StringComparison.OrdinalIgnoreCase))
            {
                if (_quyen.TryGetValue(maChucNang, out var quyenBc))
                    return quyenBc.CoQuyen;
                return true;
            }

            if (string.IsNullOrEmpty(maChucNang) || !_quyen.TryGetValue(maChucNang, out var quyen))
                return false;

            return quyen.CoQuyen;
        }

        //Chặn thao tác khi vai trò hiện tại không có quyền.
        //Dùng ở tầng repository làm lớp bảo vệ cuối, không phụ thuộc vào việc ẩn nút trên giao diện.
        public static void YeuCau(string maChucNang, string thongBao)
        {
            if (!Can(maChucNang))
                throw new UnauthorizedAccessException(thongBao);
        }

        //Kiểm tra giới hạn dữ liệu của chính mình; không có dòng = không có quyền.
        public static bool ChiCuaMinh(string maChucNang)
        {
            if (CurrentUser.IsAdmin || !Can(maChucNang))
                return false;

            return _quyen.TryGetValue(maChucNang, out var quyen) && quyen.ChiCuaMinh;
        }

        public static List<PhanQuyenChucNang> LayDanhSachChucNang()
        {
            YeuCau(ChucNangConst.PHANQUYEN, "Bạn không có quyền sử dụng chức năng này.");
            return _phanQuyenDAL.LayDanhSachChucNang();
        }

        public static void LuuPhanQuyen(IEnumerable<PhanQuyenChucNang> danhSach)
        {
            YeuCau(ChucNangConst.PHANQUYEN, "Bạn không có quyền sử dụng chức năng này.");
            _phanQuyenDAL.Luu(danhSach);
        }
    }
}
