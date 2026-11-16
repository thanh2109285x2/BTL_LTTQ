using BTL_LTTQ;

namespace BTL_LTTQ.Forms.HeThong
{
    partial class frmDangKy
    {
        private System.ComponentModel.IContainer? components = null;
        private Label lblTieuDe;
        private Label lblTenDangNhap;
        private Label lblVaiTro;
        private Label lblMatKhau;
        private Label lblNhapLaiMatKhau;
        private Label lblHoSo;
        private Label lblSoDienThoai;
        private TextBox txtTenDangNhap;
        private ComboBox cboVaiTro;
        private TextBox txtMatKhau;
        private TextBox txtNhapLaiMatKhau;
        private ComboBox cboHoSo;
        private TextBox txtSoDienThoai;
        private AppButton btnDangKy;
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
            lblTenDangNhap = new Label();
            lblVaiTro = new Label();
            lblMatKhau = new Label();
            lblNhapLaiMatKhau = new Label();
            lblHoSo = new Label();
            lblSoDienThoai = new Label();
            txtTenDangNhap = new TextBox();
            cboVaiTro = new ComboBox();
            txtMatKhau = new TextBox();
            txtNhapLaiMatKhau = new TextBox();
            cboHoSo = new ComboBox();
            txtSoDienThoai = new TextBox();
            btnDangKy = new AppButton();
            btnHuy = new AppButton();
            SuspendLayout();
            // 
            // lblTieuDe
            // 
            lblTieuDe.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
            lblTieuDe.Location = new Point(37, 24);
            lblTieuDe.Name = "lblTieuDe";
            lblTieuDe.Size = new Size(544, 42);
            lblTieuDe.TabIndex = 0;
            lblTieuDe.Text = "ĐĂNG KÝ TÀI KHOẢN";
            lblTieuDe.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTenDangNhap
            // 
            lblTenDangNhap.AutoSize = true;
            lblTenDangNhap.Location = new Point(48, 99);
            lblTenDangNhap.Name = "lblTenDangNhap";
            lblTenDangNhap.Size = new Size(107, 20);
            lblTenDangNhap.TabIndex = 1;
            lblTenDangNhap.Text = "Tên đăng nhập";
            // 
            // lblVaiTro
            // 
            lblVaiTro.AutoSize = true;
            lblVaiTro.Location = new Point(48, 151);
            lblVaiTro.Name = "lblVaiTro";
            lblVaiTro.Size = new Size(52, 20);
            lblVaiTro.TabIndex = 2;
            lblVaiTro.Text = "Vai trò";
            // 
            // lblMatKhau
            // 
            lblMatKhau.AutoSize = true;
            lblMatKhau.Location = new Point(48, 202);
            lblMatKhau.Name = "lblMatKhau";
            lblMatKhau.Size = new Size(70, 20);
            lblMatKhau.TabIndex = 3;
            lblMatKhau.Text = "Mật khẩu";
            // 
            // lblNhapLaiMatKhau
            // 
            lblNhapLaiMatKhau.AutoSize = true;
            lblNhapLaiMatKhau.Location = new Point(48, 254);
            lblNhapLaiMatKhau.Name = "lblNhapLaiMatKhau";
            lblNhapLaiMatKhau.Size = new Size(130, 20);
            lblNhapLaiMatKhau.TabIndex = 4;
            lblNhapLaiMatKhau.Text = "Nhập lại mật khẩu";
            // 
            // lblHoSo
            // 
            lblHoSo.AutoSize = true;
            lblHoSo.Location = new Point(48, 306);
            lblHoSo.Name = "lblHoSo";
            lblHoSo.Size = new Size(48, 20);
            lblHoSo.TabIndex = 5;
            lblHoSo.Text = "Hồ sơ";
            lblHoSo.Visible = false;
            // 
            // lblSoDienThoai
            // 
            lblSoDienThoai.AutoSize = true;
            lblSoDienThoai.Location = new Point(48, 358);
            lblSoDienThoai.Name = "lblSoDienThoai";
            lblSoDienThoai.Size = new Size(97, 20);
            lblSoDienThoai.TabIndex = 6;
            lblSoDienThoai.Text = "Số điện thoại";
            lblSoDienThoai.Visible = false;
            // 
            // txtTenDangNhap
            // 
            txtTenDangNhap.Location = new Point(217, 94);
            txtTenDangNhap.Margin = new Padding(3, 4, 3, 4);
            txtTenDangNhap.MaxLength = 50;
            txtTenDangNhap.Name = "txtTenDangNhap";
            txtTenDangNhap.Size = new Size(354, 27);
            txtTenDangNhap.TabIndex = 0;
            // 
            // cboVaiTro
            // 
            cboVaiTro.DropDownStyle = ComboBoxStyle.DropDownList;
            cboVaiTro.FormattingEnabled = true;
            cboVaiTro.Location = new Point(217, 146);
            cboVaiTro.Margin = new Padding(3, 4, 3, 4);
            cboVaiTro.Name = "cboVaiTro";
            cboVaiTro.Size = new Size(354, 28);
            cboVaiTro.TabIndex = 1;
            cboVaiTro.SelectedIndexChanged += cboVaiTro_SelectedIndexChanged;
            // 
            // txtMatKhau
            // 
            txtMatKhau.Location = new Point(217, 198);
            txtMatKhau.Margin = new Padding(3, 4, 3, 4);
            txtMatKhau.Name = "txtMatKhau";
            txtMatKhau.Size = new Size(354, 27);
            txtMatKhau.TabIndex = 2;
            txtMatKhau.UseSystemPasswordChar = true;
            // 
            // txtNhapLaiMatKhau
            // 
            txtNhapLaiMatKhau.Location = new Point(217, 249);
            txtNhapLaiMatKhau.Margin = new Padding(3, 4, 3, 4);
            txtNhapLaiMatKhau.Name = "txtNhapLaiMatKhau";
            txtNhapLaiMatKhau.Size = new Size(354, 27);
            txtNhapLaiMatKhau.TabIndex = 3;
            txtNhapLaiMatKhau.UseSystemPasswordChar = true;
            // 
            // cboHoSo
            // 
            cboHoSo.DropDownStyle = ComboBoxStyle.DropDownList;
            cboHoSo.FormattingEnabled = true;
            cboHoSo.Location = new Point(217, 301);
            cboHoSo.Margin = new Padding(3, 4, 3, 4);
            cboHoSo.Name = "cboHoSo";
            cboHoSo.Size = new Size(354, 28);
            cboHoSo.TabIndex = 4;
            cboHoSo.Visible = false;
            // 
            // txtSoDienThoai
            // 
            txtSoDienThoai.Location = new Point(217, 353);
            txtSoDienThoai.Margin = new Padding(3, 4, 3, 4);
            txtSoDienThoai.MaxLength = 30;
            txtSoDienThoai.Name = "txtSoDienThoai";
            txtSoDienThoai.PlaceholderText = "Nhập số điện thoại đã khai báo trong hồ sơ";
            txtSoDienThoai.Size = new Size(354, 27);
            txtSoDienThoai.TabIndex = 5;
            txtSoDienThoai.Visible = false;
            // 
            // btnDangKy
            // 
            btnDangKy.BackColor = Color.FromArgb(41, 98, 168);
            btnDangKy.FlatAppearance.BorderColor = Color.FromArgb(41, 98, 168);
            btnDangKy.FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 74, 130);
            btnDangKy.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 74, 130);
            btnDangKy.FlatStyle = FlatStyle.Flat;
            btnDangKy.ForeColor = Color.White;
            btnDangKy.Kieu = KieuNut.Chinh;
            btnDangKy.Location = new Point(290, 432);
            btnDangKy.Margin = new Padding(3, 4, 3, 4);
            btnDangKy.Name = "btnDangKy";
            btnDangKy.Size = new Size(133, 44);
            btnDangKy.TabIndex = 6;
            btnDangKy.Text = "Đăng ký";
            btnDangKy.UseVisualStyleBackColor = false;
            btnDangKy.Click += btnDangKy_Click;
            // 
            // btnHuy
            // 
            btnHuy.BackColor = Color.FromArgb(192, 57, 43);
            btnHuy.DialogResult = DialogResult.Cancel;
            btnHuy.FlatAppearance.BorderColor = Color.FromArgb(192, 57, 43);
            btnHuy.FlatAppearance.MouseDownBackColor = Color.FromArgb(150, 40, 30);
            btnHuy.FlatAppearance.MouseOverBackColor = Color.FromArgb(150, 40, 30);
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.ForeColor = Color.White;
            btnHuy.Kieu = KieuNut.NguyHiem;
            btnHuy.Location = new Point(439, 432);
            btnHuy.Margin = new Padding(3, 4, 3, 4);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new Size(133, 44);
            btnHuy.TabIndex = 7;
            btnHuy.Text = "Hủy";
            btnHuy.UseVisualStyleBackColor = false;
            btnHuy.Click += btnHuy_Click;
            // 
            // frmDangKy
            // 
            AcceptButton = btnDangKy;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnHuy;
            ClientSize = new Size(617, 518);
            Controls.Add(btnHuy);
            Controls.Add(btnDangKy);
            Controls.Add(txtSoDienThoai);
            Controls.Add(cboHoSo);
            Controls.Add(txtNhapLaiMatKhau);
            Controls.Add(txtMatKhau);
            Controls.Add(cboVaiTro);
            Controls.Add(txtTenDangNhap);
            Controls.Add(lblSoDienThoai);
            Controls.Add(lblHoSo);
            Controls.Add(lblNhapLaiMatKhau);
            Controls.Add(lblMatKhau);
            Controls.Add(lblVaiTro);
            Controls.Add(lblTenDangNhap);
            Controls.Add(lblTieuDe);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmDangKy";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Đăng ký tài khoản";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
