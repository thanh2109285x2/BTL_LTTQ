using BTL_LTTQ.Common;
using BTL_LTTQ.DAL;
using BTL_LTTQ.Models;
using System.Data;

namespace BTL_LTTQ.BUS
{
    public class TaiKhoanService
    {
        private readonly TaiKhoanDAL _taiKhoanDAL = new();

        public KetQuaDangNhap DangNhap(string tenDangNhap, string matKhau, out string? loi)
        {
            loi = null;
            DataTable taiKhoan = _taiKhoanDAL.LayTheoTenDangNhap(tenDangNhap);
            if (taiKhoan.Rows.Count == 0)
                return KetQuaDangNhap.SaiThongTin;

            DataRow dong = taiKhoan.Rows[0];
            string hash = dong["MatKhauHash"].ToString() ?? "";
            string salt = dong["Salt"].ToString() ?? "";
            if (!MatKhauHelper.KiemTra(matKhau, salt, hash))
                return KetQuaDangNhap.SaiThongTin;

            if (!Convert.ToBoolean(dong["TrangThai"]))
                return KetQuaDangNhap.TaiKhoanBiKhoa;

            CurrentUser.Dat(
                tenDangNhap,
                dong["VaiTro"].ToString() ?? "",
                dong["MaGV"] == DBNull.Value ? null : dong["MaGV"].ToString(),
                dong["MaHocVien"] == DBNull.Value ? null : dong["MaHocVien"].ToString());

            try
            {
                PermissionService.Load();
            }
            catch (Exception ex)
            {
                PermissionService.Clear();
                CurrentUser.Xoa();
                loi = ex.Message;
                return KetQuaDangNhap.LoiTaiQuyen;
            }

            return KetQuaDangNhap.ThanhCong;
        }

        public bool CoAdmin() => _taiKhoanDAL.CoAdmin();

        public DataTable LayGiaoVienChuaCoTaiKhoan() => _taiKhoanDAL.LayGiaoVienChuaCoTaiKhoan();

        public DataTable LayHocVienChuaCoTaiKhoan() => _taiKhoanDAL.LayHocVienChuaCoTaiKhoan();

        public string? LayDienThoai(string vaiTro, string ma) => _taiKhoanDAL.LayDienThoai(vaiTro, ma);

        public bool TenDangNhapDaTonTai(string tenDangNhap) => _taiKhoanDAL.TenDangNhapDaTonTai(tenDangNhap);

        public string DangKy(string tenDangNhap, string matKhau, string vaiTro, string? maGV, string? maHocVien)
        {
            string salt = MatKhauHelper.TaoSalt();
            string hash = MatKhauHelper.BamMatKhau(matKhau, salt);
            return _taiKhoanDAL.ThemTaiKhoan(tenDangNhap, hash, salt, vaiTro, maGV, maHocVien);
        }

        public string DoiMatKhau(string tenDangNhap, string matKhauCu, string matKhauMoi) =>
            _taiKhoanDAL.DoiMatKhau(tenDangNhap, matKhauCu, matKhauMoi);
    }
}
