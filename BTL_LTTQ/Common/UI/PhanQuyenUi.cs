using System.Windows.Forms;

namespace BTL_LTTQ.Common
{
    public static class PhanQuyenUi
    {
        public static void AnNeuKhongCoQuyen(Control c, string maChucNang)
        {
            c.Visible = PermissionService.Can(maChucNang);
        }

        public static void AnNutThemSuaXoa(Control btnThem, Control btnSua, Control btnXoa, string maThem, string maSua, string maXoa)
        {
            AnNeuKhongCoQuyen(btnThem, maThem);
            AnNeuKhongCoQuyen(btnSua, maSua);
            AnNeuKhongCoQuyen(btnXoa, maXoa);
        }

        public static bool CoQuyenMo(string maChucNang)
        {
            return PermissionService.Can(maChucNang);
        }
    }
}
