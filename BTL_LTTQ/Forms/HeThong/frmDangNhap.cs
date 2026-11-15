using BTL_LTTQ.Common;
using BTL_LTTQ.BUS;
using BTL_LTTQ.Models;

namespace BTL_LTTQ.Forms.HeThong
{
    public partial class frmDangNhap : Form
    {
        private readonly TaiKhoanService _taiKhoanService = new();

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
            DangNhap(txtTenDangNhap.Text.Trim());
        }

        private void DangNhap(string tenDangNhap)
        {
            KetQuaDangNhap ketQua = _taiKhoanService.DangNhap(tenDangNhap, txtMatKhau.Text, out string? loi);
            switch (ketQua)
            {
                case KetQuaDangNhap.SaiThongTin:
                    BaoLoiDangNhap();
                    return;
                case KetQuaDangNhap.TaiKhoanBiKhoa:
                    MessageBox.Show("Tài khoản đã bị khóa", "Đăng nhập thất bại",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                case KetQuaDangNhap.LoiTaiQuyen:
                    MessageBox.Show("Không thể tải quyền truy cập, đăng nhập bị từ chối.\n" + loi,
                        "Lỗi phân quyền", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                case KetQuaDangNhap.ThanhCong:
                    DialogResult = DialogResult.OK;
                    return;
            }
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
