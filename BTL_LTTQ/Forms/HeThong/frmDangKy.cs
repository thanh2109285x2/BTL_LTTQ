using BTL_LTTQ.BUS;
using BTL_LTTQ.Common;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.HeThong
{
    public partial class frmDangKy : Form
    {
        private readonly TaiKhoanService _taiKhoanService = new TaiKhoanService();

        public string TenDangNhapMoi { get; private set; } = "";

        public frmDangKy()
        {
            InitializeComponent();
            Load += frmDangKy_Load;
            GiaoDien.ApDung(this);
            btnDangKy.BackColor = GiaoDien.MauChinh;
            btnDangKy.ForeColor = System.Drawing.Color.White;
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
                dong["HoSoHienThi"] = $"{dong[cotMa]} - {dong[cotTen]}";

            cboHoSo.DisplayMember = "HoSoHienThi";
            cboHoSo.ValueMember = cotMa;
            cboHoSo.DataSource = hoSo;
            cboHoSo.SelectedIndex = hoSo.Rows.Count == 0 ? -1 : 0;
            lblHoSo.Text = vaiTro == VaiTroConst.GiaoVien ? "Hồ sơ giáo viên" : "Hồ sơ học viên";
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            string tenDangNhap = txtTenDangNhap.Text;
            string vaiTro = cboVaiTro.SelectedValue?.ToString() ?? "";
            string? maHoSo = cboHoSo.SelectedValue?.ToString();

            string loi = _taiKhoanService.DangKy(
                tenDangNhap,
                txtMatKhau.Text,
                txtNhapLaiMatKhau.Text,
                vaiTro,
                maHoSo,
                txtSoDienThoai.Text);

            if (loi != "")
            {
                BaoLoi(loi);
                // giữ focus đơn giản
                return;
            }

            TenDangNhapMoi = tenDangNhap;
            MessageBox.Show("Đăng ký tài khoản thành công.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
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
