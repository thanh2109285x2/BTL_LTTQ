using BTL_LTTQ.Common;
using BTL_LTTQ.DataAccessLayer;
using System.Data;

namespace BTL_LTTQ.Forms.HeThong
{
    public partial class frmDangNhap : Form
    {
        private readonly TaiKhoanDAL _taiKhoanDAL = new TaiKhoanDAL();

        public frmDangNhap()
        {
            InitializeComponent();
            GiaoDien.ApDung(this);
            btnDangNhap.BackColor = GiaoDien.MauChinh;
            btnDangNhap.ForeColor = Color.White;
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
            string tenDangNhap = txtTenDangNhap.Text.Trim();
            DataTable taiKhoan = _taiKhoanDAL.LayTheoTenDangNhap(tenDangNhap);

            if (taiKhoan.Rows.Count == 0)
            {
                BaoLoiDangNhap();
                return;
            }

            DataRow dong = taiKhoan.Rows[0];
            string hash = dong["MatKhauHash"].ToString() ?? "";
            string salt = dong["Salt"].ToString() ?? "";
            if (!MatKhauHelper.KiemTra(txtMatKhau.Text, salt, hash))
            {
                BaoLoiDangNhap();
                return;
            }

            if (!Convert.ToBoolean(dong["TrangThai"]))
            {
                MessageBox.Show("Tài khoản đã bị khóa", "Đăng nhập thất bại",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CurrentUser.Dat(
                tenDangNhap,
                dong["VaiTro"].ToString() ?? "",
                dong["MaGV"] == DBNull.Value ? null : dong["MaGV"].ToString(),
                dong["MaHocVien"] == DBNull.Value ? null : dong["MaHocVien"].ToString());

            DialogResult = DialogResult.OK;
        }

        private void BaoLoiDangNhap()
        {
            MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu", "Đăng nhập thất bại",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
