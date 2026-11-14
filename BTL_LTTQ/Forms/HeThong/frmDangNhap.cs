using BTL_LTTQ.BUS;
using BTL_LTTQ.Common;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.HeThong
{
    public partial class frmDangNhap : Form
    {
        private readonly TaiKhoanService _taiKhoanService = new TaiKhoanService();

        public frmDangNhap()
        {
            InitializeComponent();
            GiaoDien.ApDung(this);
            btnDangNhap.BackColor = GiaoDien.MauChinh;
            btnDangNhap.ForeColor = System.Drawing.Color.White;
            btnDangNhap.FlatStyle = FlatStyle.Flat;
            btnDangNhap.FlatAppearance.BorderColor = GiaoDien.MauChinh;
        }

        private void chkHienMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            txtMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked;
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            using var formDangKy = new frmDangKy();
            if (formDangKy.ShowDialog(this) == DialogResult.OK)
                txtTenDangNhap.Text = formDangKy.TenDangNhapMoi;
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string loi = _taiKhoanService.DangNhap(txtTenDangNhap.Text.Trim(), txtMatKhau.Text);
            if (loi != "")
            {
                MessageBox.Show(loi, loi == "Tài khoản đã bị khóa" ? "Đăng nhập thất bại" : "Đăng nhập thất bại",
                    MessageBoxButtons.OK,
                    loi == "Tài khoản đã bị khóa" ? MessageBoxIcon.Warning : MessageBoxIcon.Warning);
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
