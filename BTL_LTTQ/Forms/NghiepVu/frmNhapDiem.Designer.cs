using BTL_LTTQ;

namespace BTL_LTTQ.Forms.NghiepVu
{
    partial class frmNhapDiem
    {
        private System.ComponentModel.IContainer? components = null;
        private Panel panelTieuDe;
        private Label lblMoTa;
        private Label lblTieuDe;
        private Panel panelNoiDung;
        private Label lblNoiDung;
        private Panel panelThaoTac;
        private AppButton btnLamMoi;
        private AppButton btnLuuDiem;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelTieuDe = new Panel();
            lblMoTa = new Label();
            lblTieuDe = new Label();
            panelNoiDung = new Panel();
            lblNoiDung = new Label();
            panelThaoTac = new Panel();
            btnLamMoi = new AppButton { Kieu = KieuNut.Phu, Enabled = false };
            btnLuuDiem = new AppButton { Kieu = KieuNut.Chinh, Enabled = false };
            panelTieuDe.SuspendLayout();
            panelNoiDung.SuspendLayout();
            panelThaoTac.SuspendLayout();
            SuspendLayout();
            // 
            // panelTieuDe
            // 
            panelTieuDe.BackColor = Color.White;
            panelTieuDe.Controls.Add(lblMoTa);
            panelTieuDe.Controls.Add(lblTieuDe);
            panelTieuDe.Dock = DockStyle.Top;
            panelTieuDe.Location = new Point(0, 0);
            panelTieuDe.Name = "panelTieuDe";
            panelTieuDe.Padding = new Padding(32, 12, 32, 8);
            panelTieuDe.Size = new Size(1200, 88);
            panelTieuDe.TabIndex = 0;
            // 
            // lblMoTa
            // 
            lblMoTa.AutoSize = true;
            lblMoTa.Location = new Point(35, 54);
            lblMoTa.Name = "lblMoTa";
            lblMoTa.Size = new Size(254, 20);
            lblMoTa.TabIndex = 1;
            lblMoTa.Text = "Quản lý điểm học viên theo lớp học.";
            // 
            // lblTieuDe
            // 
            lblTieuDe.AutoSize = true;
            lblTieuDe.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
            lblTieuDe.ForeColor = GiaoDien.MauChinhDam;
            lblTieuDe.Location = new Point(32, 12);
            lblTieuDe.Name = "lblTieuDe";
            lblTieuDe.Size = new Size(181, 31);
            lblTieuDe.TabIndex = 0;
            lblTieuDe.Text = "Nhập điểm";
            // 
            // panelNoiDung
            // 
            panelNoiDung.Controls.Add(lblNoiDung);
            panelNoiDung.Dock = DockStyle.Fill;
            panelNoiDung.Location = new Point(0, 88);
            panelNoiDung.Name = "panelNoiDung";
            panelNoiDung.Padding = new Padding(32);
            panelNoiDung.Size = new Size(1200, 404);
            panelNoiDung.TabIndex = 1;
            // 
            // lblNoiDung
            // 
            lblNoiDung.Dock = DockStyle.Fill;
            lblNoiDung.Font = new Font("Segoe UI", 11F);
            lblNoiDung.ForeColor = Color.FromArgb(110, 120, 132);
            lblNoiDung.Location = new Point(32, 32);
            lblNoiDung.Name = "lblNoiDung";
            lblNoiDung.Size = new Size(1136, 340);
            lblNoiDung.TabIndex = 0;
            lblNoiDung.Text = "Giao diện nhập điểm sẽ được bổ sung sau.";
            lblNoiDung.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelThaoTac
            // 
            panelThaoTac.Controls.Add(btnLamMoi);
            panelThaoTac.Controls.Add(btnLuuDiem);
            panelThaoTac.Dock = DockStyle.Bottom;
            panelThaoTac.Location = new Point(0, 492);
            panelThaoTac.Name = "panelThaoTac";
            panelThaoTac.Padding = new Padding(32, 12, 32, 12);
            panelThaoTac.Size = new Size(1200, 68);
            panelThaoTac.TabIndex = 2;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLamMoi.Location = new Point(936, 12);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(100, 40);
            btnLamMoi.TabIndex = 1;
            btnLamMoi.Text = "Làm mới";
            // 
            // btnLuuDiem
            // 
            btnLuuDiem.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLuuDiem.Location = new Point(1048, 12);
            btnLuuDiem.Name = "btnLuuDiem";
            btnLuuDiem.Size = new Size(120, 40);
            btnLuuDiem.TabIndex = 0;
            btnLuuDiem.Text = "Lưu điểm";
            // 
            // frmNhapDiem
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = GiaoDien.MauNen;
            ClientSize = new Size(1200, 560);
            Controls.Add(panelNoiDung);
            Controls.Add(panelThaoTac);
            Controls.Add(panelTieuDe);
            Name = "frmNhapDiem";
            Text = "Nhập điểm";
            panelTieuDe.ResumeLayout(false);
            panelTieuDe.PerformLayout();
            panelNoiDung.ResumeLayout(false);
            panelThaoTac.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
