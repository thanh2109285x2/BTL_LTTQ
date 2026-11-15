using BTL_LTTQ.Common;
using BTL_LTTQ.BUS;
using System.Data;
using System.Linq;

namespace BTL_LTTQ.Forms.HeThong
{
    public partial class frmDangKy : Form
    {
        private readonly TaiKhoanService _taiKhoanService = new();

        public string TenDangNhapMoi { get; private set; } = "";

        public frmDangKy()
        {
            InitializeComponent();
            cboHoSo.SelectedIndexChanged += cboHoSo_SelectedIndexChanged;
            Load += frmDangKy_Load;
            GiaoDien.ApDung(this);
            btnDangKy.BackColor = GiaoDien.MauChinh;
            btnDangKy.ForeColor = Color.White;
            btnDangKy.FlatStyle = FlatStyle.Flat;
            btnDangKy.FlatAppearance.BorderColor = GiaoDien.MauChinh;
        }

        private void frmDangKy_Load(object? sender, EventArgs e)
        {
            var vaiTro = new DataTable();
            vaiTro.Columns.Add("MaVaiTro", typeof(string));
            vaiTro.Columns.Add("TenVaiTro", typeof(string));
            vaiTro.Rows.Add(VaiTroConst.GiaoVien, "Giáo viên");
            vaiTro.Rows.Add(VaiTroConst.HocVien, "Học viên");
            if (!_taiKhoanService.CoAdmin())
                vaiTro.Rows.Add(VaiTroConst.Admin, "Admin");

            cboVaiTro.DisplayMember = "TenVaiTro";
            cboVaiTro.ValueMember = "MaVaiTro";
            cboVaiTro.DataSource = vaiTro;
            cboVaiTro.SelectedIndex = 0;
        }

        private void cboVaiTro_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cboVaiTro.SelectedValue == null) return;

            string vaiTro = cboVaiTro.SelectedValue.ToString() ?? "";
            bool canChonHoSo = vaiTro == VaiTroConst.GiaoVien || vaiTro == VaiTroConst.HocVien;
            lblHoSo.Visible = canChonHoSo;
            cboHoSo.Visible = canChonHoSo;
            lblSoDienThoai.Visible = canChonHoSo;
            txtSoDienThoai.Visible = canChonHoSo;
            cboHoSo.DataSource = null;
            txtSoDienThoai.Clear();

            if (!canChonHoSo) return;

            DataTable hoSo = vaiTro == VaiTroConst.GiaoVien
                ? _taiKhoanService.LayGiaoVienChuaCoTaiKhoan()
                : _taiKhoanService.LayHocVienChuaCoTaiKhoan();
            string cotMa = vaiTro == VaiTroConst.GiaoVien ? "MaGV" : "MaHocVien";
            string cotTen = vaiTro == VaiTroConst.GiaoVien ? "TenGV" : "TenHocVien";
            hoSo.Columns.Add("HoSoHienThi", typeof(string));
            foreach (DataRow dong in hoSo.Rows)
            {
                bool coDienThoai = !string.IsNullOrWhiteSpace(Convert.ToString(dong["DienThoai"]));
                dong["HoSoHienThi"] = $"{dong[cotMa]} - {dong[cotTen]} {(coDienThoai ? "(đã có SĐT)" : "(chưa có SĐT)")}";
            }

            cboHoSo.DisplayMember = "HoSoHienThi";
            cboHoSo.ValueMember = cotMa;
            cboHoSo.DataSource = hoSo;
            cboHoSo.SelectedIndex = hoSo.Rows.Count == 0 ? -1 : 0;
            lblHoSo.Text = vaiTro == VaiTroConst.GiaoVien ? "Hồ sơ giáo viên" : "Hồ sơ học viên";
        }

        private void cboHoSo_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cboHoSo.SelectedItem is not DataRowView hoSo) return;
            bool coDienThoai = !string.IsNullOrWhiteSpace(Convert.ToString(hoSo["DienThoai"]));
            txtSoDienThoai.Enabled = coDienThoai;
            lblSoDienThoai.Text = coDienThoai ? "Xác nhận SĐT hồ sơ" : "Cần bổ sung SĐT";
            txtSoDienThoai.PlaceholderText = coDienThoai
                ? "Nhập số điện thoại đã khai báo trong hồ sơ"
                : "Cập nhật hồ sơ trước khi đăng ký";
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            string tenDangNhap = txtTenDangNhap.Text;
            if (tenDangNhap.Length < 4 || tenDangNhap.Length > 50 || tenDangNhap.Any(char.IsWhiteSpace))
            {
                BaoLoi("Tên đăng nhập phải từ 4 đến 50 ký tự và không chứa khoảng trắng.");
                txtTenDangNhap.Focus();
                return;
            }

            if (_taiKhoanService.TenDangNhapDaTonTai(tenDangNhap))
            {
                BaoLoi("Tên đăng nhập đã tồn tại.");
                txtTenDangNhap.Focus();
                return;
            }

            if (txtMatKhau.Text.Length < 6)
            {
                BaoLoi("Mật khẩu phải có ít nhất 6 ký tự.");
                txtMatKhau.Focus();
                return;
            }

            if (txtMatKhau.Text != txtNhapLaiMatKhau.Text)
            {
                BaoLoi("Mật khẩu nhập lại không khớp.");
                txtNhapLaiMatKhau.Focus();
                return;
            }

            string vaiTro = cboVaiTro.SelectedValue?.ToString() ?? "";
            string? maGV = null;
            string? maHocVien = null;
            if (vaiTro == VaiTroConst.GiaoVien || vaiTro == VaiTroConst.HocVien)
            {
                string? maHoSo = cboHoSo.SelectedValue?.ToString();
                if (string.IsNullOrEmpty(maHoSo))
                {
                    BaoLoi("Vui lòng chọn hồ sơ đăng ký.");
                    cboHoSo.Focus();
                    return;
                }

                string? dienThoai = _taiKhoanService.LayDienThoai(vaiTro, maHoSo)?.Trim();
                if (string.IsNullOrWhiteSpace(dienThoai))
                {
                    BaoLoi("Hồ sơ này chưa có số điện thoại. Hãy cập nhật số điện thoại trong Quản lý giáo viên/học viên rồi đăng ký lại.");
                    cboHoSo.Focus();
                    return;
                }

                if (ChuanHoaDienThoai(txtSoDienThoai.Text) != ChuanHoaDienThoai(dienThoai))
                {
                    BaoLoi("Số điện thoại không khớp với hồ sơ.");
                    txtSoDienThoai.Focus();
                    return;
                }

                if (vaiTro == VaiTroConst.GiaoVien)
                    maGV = maHoSo;
                else
                    maHocVien = maHoSo;
            }
            else if (vaiTro != VaiTroConst.Admin)
            {
                BaoLoi("Vui lòng chọn vai trò hợp lệ.");
                cboVaiTro.Focus();
                return;
            }

            string loi = _taiKhoanService.DangKy(tenDangNhap, txtMatKhau.Text, vaiTro, maGV, maHocVien);
            if (loi != "")
            {
                BaoLoi(loi);
                return;
            }

            TenDangNhapMoi = tenDangNhap;
            MessageBox.Show("Đăng ký tài khoản thành công.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
        }

        private static string ChuanHoaDienThoai(string giaTri)
        {
            string chuSo = new(giaTri.Where(char.IsDigit).ToArray());
            return chuSo.StartsWith("84", StringComparison.Ordinal) && chuSo.Length == 11
                ? "0" + chuSo[2..]
                : chuSo;
        }

        private void BaoLoi(string noiDung)
        {
            MessageBox.Show(noiDung, "Thông tin đăng ký",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
