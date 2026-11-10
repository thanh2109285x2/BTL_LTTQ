namespace BTL_LTTQ.Forms.HeThong
{
    partial class frmDangNhap
    {
        private System.ComponentModel.IContainer? components = null;
        private Label lblTieuDe;
        private Label lblTenDangNhap;
        private Label lblMatKhau;
        private TextBox txtTenDangNhap;
        private TextBox txtMatKhau;
        private CheckBox chkHienMatKhau;
        private Button btnDangNhap;
        private Button btnDangKy;
        private Button btnThoat;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTieuDe = new Label();
            lblTenDangNhap = new Label();
            lblMatKhau = new Label();
            txtTenDangNhap = new TextBox();
            txtMatKhau = new TextBox();
            chkHienMatKhau = new CheckBox();
            btnDangNhap = new Button();
            btnDangKy = new Button();
            btnThoat = new Button();
            SuspendLayout();
            // 
            // lblTieuDe
            // 
            lblTieuDe.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
            lblTieuDe.Location = new Point(32, 24);
            lblTieuDe.Name = "lblTieuDe";
            lblTieuDe.Size = new Size(376, 36);
            lblTieuDe.TabIndex = 0;
            lblTieuDe.Text = "ĐĂNG NHẬP HỆ THỐNG";
            lblTieuDe.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTenDangNhap
            // 
            lblTenDangNhap.AutoSize = true;
            lblTenDangNhap.Location = new Point(42, 91);
            lblTenDangNhap.Name = "lblTenDangNhap";
            lblTenDangNhap.Size = new Size(95, 19);
            lblTenDangNhap.TabIndex = 1;
            lblTenDangNhap.Text = "Tên đăng nhập";
            // 
            // lblMatKhau
            // 
            lblMatKhau.AutoSize = true;
            lblMatKhau.Location = new Point(42, 135);
            lblMatKhau.Name = "lblMatKhau";
            lblMatKhau.Size = new Size(62, 19);
            lblMatKhau.TabIndex = 2;
            lblMatKhau.Text = "Mật khẩu";
            // 
            // txtTenDangNhap
            // 
            txtTenDangNhap.Location = new Point(157, 87);
            txtTenDangNhap.MaxLength = 50;
            txtTenDangNhap.Name = "txtTenDangNhap";
            txtTenDangNhap.Size = new Size(248, 25);
            txtTenDangNhap.TabIndex = 0;
            // 
            // txtMatKhau
            // 
            txtMatKhau.Location = new Point(157, 131);
            txtMatKhau.Name = "txtMatKhau";
            txtMatKhau.Size = new Size(248, 25);
            txtMatKhau.TabIndex = 1;
            txtMatKhau.UseSystemPasswordChar = true;
            // 
            // chkHienMatKhau
            // 
            chkHienMatKhau.AutoSize = true;
            chkHienMatKhau.Location = new Point(157, 166);
            chkHienMatKhau.Name = "chkHienMatKhau";
            chkHienMatKhau.Size = new Size(108, 23);
            chkHienMatKhau.TabIndex = 2;
            chkHienMatKhau.Text = "Hiện mật khẩu";
            chkHienMatKhau.UseVisualStyleBackColor = true;
            chkHienMatKhau.CheckedChanged += chkHienMatKhau_CheckedChanged;
            // 
            // btnDangNhap
            // 
            btnDangNhap.Location = new Point(42, 220);
            btnDangNhap.Name = "btnDangNhap";
            btnDangNhap.Size = new Size(112, 36);
            btnDangNhap.TabIndex = 3;
            btnDangNhap.Text = "Đăng nhập";
            btnDangNhap.UseVisualStyleBackColor = true;
            btnDangNhap.Click += btnDangNhap_Click;
            // 
            // btnDangKy
            // 
            btnDangKy.Location = new Point(164, 220);
            btnDangKy.Name = "btnDangKy";
            btnDangKy.Size = new Size(112, 36);
            btnDangKy.TabIndex = 4;
            btnDangKy.Text = "Đăng ký";
            btnDangKy.UseVisualStyleBackColor = true;
            btnDangKy.Click += btnDangKy_Click;
            // 
            // btnThoat
            // 
            btnThoat.DialogResult = DialogResult.Cancel;
            btnThoat.Location = new Point(293, 220);
            btnThoat.Name = "btnThoat";
            btnThoat.Size = new Size(112, 36);
            btnThoat.TabIndex = 5;
            btnThoat.Text = "Thoát";
            btnThoat.UseVisualStyleBackColor = true;
            btnThoat.Click += btnThoat_Click;
            // 
            // frmDangNhap
            // 
            AcceptButton = btnDangNhap;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnThoat;
            ClientSize = new Size(440, 290);
            Controls.Add(btnThoat);
            Controls.Add(btnDangKy);
            Controls.Add(btnDangNhap);
            Controls.Add(chkHienMatKhau);
            Controls.Add(txtMatKhau);
            Controls.Add(txtTenDangNhap);
            Controls.Add(lblMatKhau);
            Controls.Add(lblTenDangNhap);
            Controls.Add(lblTieuDe);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmDangNhap";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đăng nhập";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
