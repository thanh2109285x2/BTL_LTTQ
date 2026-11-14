using BTL_LTTQ.Common;
using BTL_LTTQ.Services;
using System;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.HeThong
{
    public partial class frmBackup : Form
    {
        private readonly DatabaseBackupService _service = new DatabaseBackupService();

        public frmBackup()
        {
            InitializeComponent();
            GiaoDien.ApDung(this);
        }

        private void btnSaoLuu_Click(object? sender, EventArgs e)
        {
            using var dlg = new SaveFileDialog
            {
                Title = "Lưu file sao lưu database",
                Filter = "File sao lưu Database (*.bak)|*.bak",
                FileName = "QuanLyLopTinHocVaNgoaiNguNganHan_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".bak",
                OverwritePrompt = true
            };

            if (dlg.ShowDialog(this) != DialogResult.OK)
                return;

            string loi = _service.ThucBackup(dlg.FileName);
            if (loi != "")
            {
                MessageBox.Show(loi, "Sao lưu thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Đã lưu database thành công:\n" + dlg.FileName, "Sao lưu database",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnHuy_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void lblTieuDe_Click(object sender, EventArgs e)
        {

        }
    }
}
