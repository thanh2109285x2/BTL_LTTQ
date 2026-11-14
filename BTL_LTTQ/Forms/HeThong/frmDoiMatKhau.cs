using BTL_LTTQ.BUS;
using BTL_LTTQ.Common;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.HeThong
{
    public partial class frmDoiMatKhau : Form
    {
        private readonly TaiKhoanService _taiKhoanService = new TaiKhoanService();

        public frmDoiMatKhau()
        {
            InitializeComponent();
            GiaoDien.ApDung(this);
        }

        private void btnDoiMatKhau_Click(object? sender, EventArgs e)
        {
            string matKhauCu = txtMatKhauCu.Text;
            string matKhauMoi = txtMatKhauMoi.Text;
            string nhapLai = txtNhapLaiMatKhauMoi.Text;

            if (string.IsNullOrWhiteSpace(matKhauCu) || string.IsNullOrWhiteSpace(matKhauMoi) ||
                string.IsNullOrWhiteSpace(nhapLai))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ cả ba ô mật khẩu.", "Thiếu thông tin",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (matKhauMoi.Length < 6)
            {
                MessageBox.Show("Mật khẩu mới phải có ít nhất 6 ký tự.", "Mật khẩu không hợp lệ",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.Equals(matKhauCu, matKhauMoi, System.StringComparison.Ordinal))
            {
                MessageBox.Show("Mật khẩu mới phải khác mật khẩu hiện tại.", "Mật khẩu không hợp lệ",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!string.Equals(matKhauMoi, nhapLai, System.StringComparison.Ordinal))
            {
                MessageBox.Show("Mật khẩu mới và ô nhập lại không khớp.", "Mật khẩu không hợp lệ",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string loi = _taiKhoanService.DoiMatKhau(CurrentUser.TenDangNhap, matKhauCu, matKhauMoi);
            if (loi != "")
            {
                MessageBox.Show(loi, "Không đổi được mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Đổi mật khẩu thành công", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnHuy_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
