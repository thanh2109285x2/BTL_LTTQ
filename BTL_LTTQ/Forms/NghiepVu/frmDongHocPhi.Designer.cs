using BTL_LTTQ;

namespace BTL_LTTQ.Forms.NghiepVu
{
    partial class frmDongHocPhi
    {
        private System.ComponentModel.IContainer? components = null;
        private Panel panelTieuDe;
        private Label lblMoTa;
        private Label lblTieuDe;
        private Panel panelNoiDung;
        private Label lblNoiDung;
        private Panel panelThaoTac;
        private AppButton btnLamMoi;
        private AppButton btnXacNhanThu;

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
            btnXacNhanThu = new AppButton { Kieu = KieuNut.Chinh, Enabled = false };
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
            lblMoTa.Size = new Size(285, 20);
            lblMoTa.TabIndex = 1;
            lblMoTa.Text = "Theo dõi và xác nhận thu học phí học viên.";
            // 
            // lblTieuDe
            // 
            lblTieuDe.AutoSize = true;
            lblTieuDe.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
            lblTieuDe.ForeColor = GiaoDien.MauChinhDam;
            lblTieuDe.Location = new Point(32, 12);
            lblTieuDe.Name = "lblTieuDe";
            lblTieuDe.Size = new Size(205, 31);
            lblTieuDe.TabIndex = 0;
            lblTieuDe.Text = "Đóng học phí";
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
            lblNoiDung.Text = "Giao diện đóng học phí sẽ được bổ sung sau.";
            lblNoiDung.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelThaoTac
            // 
            panelThaoTac.Controls.Add(btnLamMoi);
            panelThaoTac.Controls.Add(btnXacNhanThu);
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
            // btnXacNhanThu
            // 
            btnXacNhanThu.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnXacNhanThu.Location = new Point(1048, 12);
            btnXacNhanThu.Name = "btnXacNhanThu";
            btnXacNhanThu.Size = new Size(120, 40);
            btnXacNhanThu.TabIndex = 0;
            btnXacNhanThu.Text = "Xác nhận thu";
            // 
            // frmDongHocPhi
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = GiaoDien.MauNen;
            ClientSize = new Size(1200, 560);
            Controls.Add(panelNoiDung);
            Controls.Add(panelThaoTac);
            Controls.Add(panelTieuDe);
            Name = "frmDongHocPhi";
            Text = "Đóng học phí";
            panelTieuDe.ResumeLayout(false);
            panelTieuDe.PerformLayout();
            panelNoiDung.ResumeLayout(false);
            panelThaoTac.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
