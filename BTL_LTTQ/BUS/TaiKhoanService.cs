using BTL_LTTQ.Common;
using BTL_LTTQ.DAL;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq;

namespace BTL_LTTQ.BUS
{
    public class TaiKhoanService
    {
        private readonly TaiKhoanDAL _dal = new TaiKhoanDAL();

        public bool CoAdmin() => _dal.CoAdmin();

        public DataTable LayGiaoVienChuaCoTaiKhoan() => _dal.LayGiaoVienChuaCoTaiKhoan();

        public DataTable LayHocVienChuaCoTaiKhoan() => _dal.LayHocVienChuaCoTaiKhoan();

        public bool TenDangNhapDaTonTai(string tenDangNhap) => _dal.TenDangNhapDaTonTai(tenDangNhap);

        public string DangNhap(string tenDangNhap, string matKhau)
        {
            tenDangNhap = (tenDangNhap ?? "").Trim();
            if (string.IsNullOrWhiteSpace(tenDangNhap))
                return "Sai tên đăng nhập hoặc mật khẩu";

            try
            {
                DataTable taiKhoan = _dal.LayTheoTenDangNhap(tenDangNhap);
                if (taiKhoan.Rows.Count == 0)
                    return "Sai tên đăng nhập hoặc mật khẩu";

                DataRow dong = taiKhoan.Rows[0];
                string hash = dong["MatKhauHash"].ToString() ?? "";
                string salt = dong["Salt"].ToString() ?? "";
                if (!MatKhauHelper.KiemTra(matKhau, salt, hash))
                    return "Sai tên đăng nhập hoặc mật khẩu";

                if (!Convert.ToBoolean(dong["TrangThai"]))
                    return "Tài khoản đã bị khóa";

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
                    return "Không thể tải quyền truy cập, đăng nhập bị từ chối.\n" + ex.Message;
                }

                return "";
            }
            catch (Exception ex)
            {
                return "Đăng nhập thất bại: " + ex.Message;
            }
        }

        public string DangKy(string tenDangNhap, string matKhau, string nhapLai, string vaiTro, string? maHoSo, string? soDienThoaiNhap)
        {
            tenDangNhap = tenDangNhap ?? "";

            if (tenDangNhap.Length < 4 || tenDangNhap.Length > 50 || tenDangNhap.Any(char.IsWhiteSpace))
                return "Tên đăng nhập phải từ 4 đến 50 ký tự và không chứa khoảng trắng.";

            if (_dal.TenDangNhapDaTonTai(tenDangNhap))
                return "Tên đăng nhập đã tồn tại.";

            if ((matKhau ?? "").Length < 6)
                return "Mật khẩu phải có ít nhất 6 ký tự.";

            if (!string.Equals(matKhau, nhapLai, StringComparison.Ordinal))
                return "Mật khẩu nhập lại không khớp.";

            vaiTro = vaiTro ?? "";
            string? maGV = null;
            string? maHocVien = null;

            if (vaiTro == VaiTroConst.GiaoVien || vaiTro == VaiTroConst.HocVien)
            {
                if (string.IsNullOrWhiteSpace(maHoSo))
                    return "Vui lòng chọn hồ sơ đăng ký.";

                string? dienThoai = vaiTro == VaiTroConst.GiaoVien
                    ? _dal.LayDienThoaiGiaoVien(maHoSo)
                    : _dal.LayDienThoaiHocVien(maHoSo);

                if (BoKhoangTrang(soDienThoaiNhap ?? "") != BoKhoangTrang(dienThoai ?? ""))
                    return "Số điện thoại không khớp với hồ sơ";

                if (vaiTro == VaiTroConst.GiaoVien) maGV = maHoSo;
                else maHocVien = maHoSo;
            }
            else if (vaiTro != VaiTroConst.Admin)
            {
                return "Vui lòng chọn vai trò hợp lệ.";
            }

            string salt = MatKhauHelper.TaoSalt();
            string hash = MatKhauHelper.BamMatKhau(matKhau ?? "", salt);

            try
            {
                int soDong;
                if (vaiTro == VaiTroConst.Admin)
                {
                    soDong = _dal.ThemTaiKhoanAdmin(tenDangNhap, hash, salt, maGV, maHocVien);
                    if (soDong == 0) return "Tài khoản Admin đã tồn tại.";
                }
                else
                {
                    soDong = _dal.ThemTaiKhoan(tenDangNhap, hash, salt, vaiTro, maGV, maHocVien);
                }

                return soDong == 1 ? "" : "Không thể tạo tài khoản.";
            }
            catch (SqlException ex) when (ex.Number is 2627 or 2601)
            {
                return "Tên đăng nhập hoặc hồ sơ đã được đăng ký tài khoản.";
            }
            catch (SqlException)
            {
                return "Không thể tạo tài khoản do lỗi cơ sở dữ liệu.";
            }
        }

        public string DoiMatKhau(string tenDangNhap, string matKhauCu, string matKhauMoi)
        {
            try
            {
                DataTable taiKhoan = _dal.LayHashSalt(tenDangNhap);
                if (taiKhoan.Rows.Count != 1)
                    return "Không thể đổi mật khẩu cho tài khoản hiện tại.";

                DataRow dong = taiKhoan.Rows[0];
                string hashCu = dong["MatKhauHash"].ToString() ?? "";
                string saltCu = dong["Salt"].ToString() ?? "";
                if (!MatKhauHelper.KiemTra(matKhauCu, saltCu, hashCu))
                    return "Mật khẩu hiện tại không đúng";

                string saltMoi = MatKhauHelper.TaoSalt();
                string hashMoi = MatKhauHelper.BamMatKhau(matKhauMoi, saltMoi);
                int soDong = _dal.CapNhatHashSalt(tenDangNhap, hashMoi, saltMoi);

                return soDong == 1 ? "" : "Không thể cập nhật mật khẩu tài khoản.";
            }
            catch
            {
                return "Không thể đổi mật khẩu do lỗi hệ thống.";
            }
        }

        private static string BoKhoangTrang(string giaTri)
        {
            return new string(giaTri.Where(c => !char.IsWhiteSpace(c)).ToArray());
        }
    }
}
