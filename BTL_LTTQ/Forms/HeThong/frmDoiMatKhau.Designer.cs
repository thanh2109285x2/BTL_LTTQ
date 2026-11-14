using BTL_LTTQ;

namespace BTL_LTTQ.Forms.HeThong
{
    partial class frmDoiMatKhau
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTieuDe;
        private Label lblMatKhauCu;
        private Label lblMatKhauMoi;
        private Label lblNhapLaiMatKhauMoi;
        private TextBox txtMatKhauCu;
        private TextBox txtMatKhauMoi;
        private TextBox txtNhapLaiMatKhauMoi;
        private AppButton btnDoiMatKhau;
        private AppButton btnHuy;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTieuDe = new Label();
            lblMatKhauCu = new Label();
            lblMatKhauMoi = new Label();
            lblNhapLaiMatKhauMoi = new Label();
            txtMatKhauCu = new TextBox();
            txtMatKhauMoi = new TextBox();
            txtNhapLaiMatKhauMoi = new TextBox();
            btnDoiMatKhau = new AppButton { Kieu = KieuNut.Chinh };
            btnHuy = new AppButton { Kieu = KieuNut.Phu };
            SuspendLayout();
            // 
            // lblTieuDe
            // 
            lblTieuDe.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
            lblTieuDe.Location = new Point(32, 20);
            lblTieuDe.Name = "lblTieuDe";
            lblTieuDe.Size = new Size(396, 36);
            lblTieuDe.TabIndex = 0;
            lblTieuDe.Text = "ĐỔI MẬT KHẨU";
            lblTieuDe.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblMatKhauCu
            // 
            lblMatKhauCu.AutoSize = true;
            lblMatKhauCu.Location = new Point(38, 82);
            lblMatKhauCu.Name = "lblMatKhauCu";
            lblMatKhauCu.Size = new Size(92, 20);
            lblMatKhauCu.TabIndex = 1;
            lblMatKhauCu.Text = "Mật khẩu hiện tại";
            // 
            // lblMatKhauMoi
            // 
            lblMatKhauMoi.AutoSize = true;
            lblMatKhauMoi.Location = new Point(38, 126);
            lblMatKhauMoi.Name = "lblMatKhauMoi";
            lblMatKhauMoi.Size = new Size(86, 20);
            lblMatKhauMoi.TabIndex = 2;
            lblMatKhauMoi.Text = "Mật khẩu mới";
            // 
            // lblNhapLaiMatKhauMoi
            // 
            lblNhapLaiMatKhauMoi.AutoSize = true;
            lblNhapLaiMatKhauMoi.Location = new Point(38, 170);
            lblNhapLaiMatKhauMoi.Name = "lblNhapLaiMatKhauMoi";
            lblNhapLaiMatKhauMoi.Size = new Size(112, 20);
            lblNhapLaiMatKhauMoi.TabIndex = 3;
            lblNhapLaiMatKhauMoi.Text = "Nhập lại mật khẩu";
            // 
            // txtMatKhauCu
            // 
            txtMatKhauCu.Location = new Point(172, 78);
            txtMatKhauCu.Name = "txtMatKhauCu";
            txtMatKhauCu.Size = new Size(236, 27);
            txtMatKhauCu.TabIndex = 0;
            txtMatKhauCu.UseSystemPasswordChar = true;
            // 
            // txtMatKhauMoi
            // 
            txtMatKhauMoi.Location = new Point(172, 122);
            txtMatKhauMoi.Name = "txtMatKhauMoi";
            txtMatKhauMoi.Size = new Size(236, 27);
            txtMatKhauMoi.TabIndex = 1;
            txtMatKhauMoi.UseSystemPasswordChar = true;
            // 
            // txtNhapLaiMatKhauMoi
            // 
            txtNhapLaiMatKhauMoi.Location = new Point(172, 166);
            txtNhapLaiMatKhauMoi.Name = "txtNhapLaiMatKhauMoi";
            txtNhapLaiMatKhauMoi.Size = new Size(236, 27);
            txtNhapLaiMatKhauMoi.TabIndex = 2;
            txtNhapLaiMatKhauMoi.UseSystemPasswordChar = true;
            // 
            // btnDoiMatKhau
            // 
            btnDoiMatKhau.Location = new Point(172, 220);
            btnDoiMatKhau.Name = "btnDoiMatKhau";
            btnDoiMatKhau.Size = new Size(132, 36);
            btnDoiMatKhau.TabIndex = 3;
            btnDoiMatKhau.Text = "Đổi mật khẩu";
            btnDoiMatKhau.UseVisualStyleBackColor = true;
            btnDoiMatKhau.Click += btnDoiMatKhau_Click;
            // 
            // btnHuy
            // 
            btnHuy.DialogResult = DialogResult.Cancel;
            btnHuy.Location = new Point(322, 220);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new Size(86, 36);
            btnHuy.TabIndex = 4;
            btnHuy.Text = "Hủy";
            btnHuy.UseVisualStyleBackColor = true;
            btnHuy.Click += btnHuy_Click;
            // 
            // frmDoiMatKhau
            // 
            AcceptButton = btnDoiMatKhau;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(460, 284);
            Controls.Add(btnHuy);
            Controls.Add(btnDoiMatKhau);
            Controls.Add(txtNhapLaiMatKhauMoi);
            Controls.Add(txtMatKhauMoi);
            Controls.Add(txtMatKhauCu);
            Controls.Add(lblNhapLaiMatKhauMoi);
            Controls.Add(lblMatKhauMoi);
            Controls.Add(lblMatKhauCu);
            Controls.Add(lblTieuDe);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmDoiMatKhau";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Đổi mật khẩu";
            CancelButton = btnHuy;
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
