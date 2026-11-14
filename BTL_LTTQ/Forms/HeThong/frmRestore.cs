using BTL_LTTQ.Common;
using BTL_LTTQ.Services;
using System;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.HeThong
{
    public partial class frmRestore : Form
    {
        private readonly DatabaseBackupService _service = new DatabaseBackupService();

        public frmRestore()
        {
            InitializeComponent();
            GiaoDien.ApDung(this);
        }

        private void btnKhoiPhuc_Click(object? sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog
            {
                Title = "Chọn file sao lưu database",
                Filter = "File sao lưu Database (*.bak)|*.bak",
                CheckFileExists = true
            };

            if (dlg.ShowDialog(this) != DialogResult.OK)
                return;

            DialogResult xacNhan = MessageBox.Show(
                "Bạn có chắc muốn khôi phục database?\nToàn bộ dữ liệu hiện tại sẽ bị ghi đè.",
                "Xác nhận khôi phục", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (xacNhan != DialogResult.Yes)
                return;

            string loi = _service.ThucRestore(dlg.FileName);
            if (loi != "")
            {
                MessageBox.Show(loi, "Khôi phục thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Đã khôi phục database thành công.\nLưu ý: mọi dữ liệu hiện tại sẽ bị thay thế.",
                "Khôi phục database", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }

        private void btnHuy_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
