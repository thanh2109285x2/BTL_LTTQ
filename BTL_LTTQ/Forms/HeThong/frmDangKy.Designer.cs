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
            btnDangKy = new AppButton { Kieu = KieuNut.Chinh };
            btnHuy = new AppButton { Kieu = KieuNut.Phu };
            SuspendLayout();
            // 
            // lblTieuDe
            // 
            lblTieuDe.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
            lblTieuDe.Location = new Point(32, 20);
            lblTieuDe.Name = "lblTieuDe";
            lblTieuDe.Size = new Size(476, 36);
            lblTieuDe.TabIndex = 0;
            lblTieuDe.Text = "ĐĂNG KÝ TÀI KHOẢN";
            lblTieuDe.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTenDangNhap
            // 
            lblTenDangNhap.AutoSize = true;
            lblTenDangNhap.Location = new Point(42, 84);
            lblTenDangNhap.Name = "lblTenDangNhap";
            lblTenDangNhap.Size = new Size(95, 19);
            lblTenDangNhap.TabIndex = 1;
            lblTenDangNhap.Text = "Tên đăng nhập";
            // 
            // lblVaiTro
            // 
            lblVaiTro.AutoSize = true;
            lblVaiTro.Location = new Point(42, 128);
            lblVaiTro.Name = "lblVaiTro";
            lblVaiTro.Size = new Size(48, 19);
            lblVaiTro.TabIndex = 2;
            lblVaiTro.Text = "Vai trò";
            // 
            // lblMatKhau
            // 
            lblMatKhau.AutoSize = true;
            lblMatKhau.Location = new Point(42, 172);
            lblMatKhau.Name = "lblMatKhau";
            lblMatKhau.Size = new Size(62, 19);
            lblMatKhau.TabIndex = 3;
            lblMatKhau.Text = "Mật khẩu";
            // 
            // lblNhapLaiMatKhau
            // 
            lblNhapLaiMatKhau.AutoSize = true;
            lblNhapLaiMatKhau.Location = new Point(42, 216);
            lblNhapLaiMatKhau.Name = "lblNhapLaiMatKhau";
            lblNhapLaiMatKhau.Size = new Size(105, 19);
            lblNhapLaiMatKhau.TabIndex = 4;
            lblNhapLaiMatKhau.Text = "Nhập lại mật khẩu";
            // 
            // lblHoSo
            // 
            lblHoSo.AutoSize = true;
            lblHoSo.Location = new Point(42, 260);
            lblHoSo.Name = "lblHoSo";
            lblHoSo.Size = new Size(44, 19);
            lblHoSo.TabIndex = 5;
            lblHoSo.Text = "Hồ sơ";
            lblHoSo.Visible = false;
            // 
            // lblSoDienThoai
            // 
            lblSoDienThoai.AutoSize = true;
            lblSoDienThoai.Location = new Point(42, 304);
            lblSoDienThoai.Name = "lblSoDienThoai";
            lblSoDienThoai.Size = new Size(79, 19);
            lblSoDienThoai.TabIndex = 6;
            lblSoDienThoai.Text = "Số điện thoại";
            lblSoDienThoai.Visible = false;
            // 
            // txtTenDangNhap
            // 
            txtTenDangNhap.Location = new Point(190, 80);
            txtTenDangNhap.MaxLength = 50;
            txtTenDangNhap.Name = "txtTenDangNhap";
            txtTenDangNhap.Size = new Size(310, 25);
            txtTenDangNhap.TabIndex = 0;
            // 
            // cboVaiTro
            // 
            cboVaiTro.DropDownStyle = ComboBoxStyle.DropDownList;
            cboVaiTro.FormattingEnabled = true;
            cboVaiTro.Location = new Point(190, 124);
            cboVaiTro.Name = "cboVaiTro";
            cboVaiTro.Size = new Size(310, 25);
            cboVaiTro.TabIndex = 1;
            cboVaiTro.SelectedIndexChanged += cboVaiTro_SelectedIndexChanged;
            // 
            // txtMatKhau
            // 
            txtMatKhau.Location = new Point(190, 168);
            txtMatKhau.Name = "txtMatKhau";
            txtMatKhau.Size = new Size(310, 25);
            txtMatKhau.TabIndex = 2;
            txtMatKhau.UseSystemPasswordChar = true;
            // 
            // txtNhapLaiMatKhau
            // 
            txtNhapLaiMatKhau.Location = new Point(190, 212);
            txtNhapLaiMatKhau.Name = "txtNhapLaiMatKhau";
            txtNhapLaiMatKhau.Size = new Size(310, 25);
            txtNhapLaiMatKhau.TabIndex = 3;
            txtNhapLaiMatKhau.UseSystemPasswordChar = true;
            // 
            // cboHoSo
            // 
            cboHoSo.DropDownStyle = ComboBoxStyle.DropDownList;
            cboHoSo.FormattingEnabled = true;
            cboHoSo.Location = new Point(190, 256);
            cboHoSo.Name = "cboHoSo";
            cboHoSo.Size = new Size(310, 25);
            cboHoSo.TabIndex = 4;
            cboHoSo.Visible = false;
            // 
            // txtSoDienThoai
            // 
            txtSoDienThoai.Location = new Point(190, 300);
            txtSoDienThoai.Name = "txtSoDienThoai";
            txtSoDienThoai.Size = new Size(310, 25);
            txtSoDienThoai.TabIndex = 5;
            txtSoDienThoai.Visible = false;
            // 
            // btnDangKy
            // 
            btnDangKy.Location = new Point(254, 367);
            btnDangKy.Name = "btnDangKy";
            btnDangKy.Size = new Size(116, 37);
            btnDangKy.TabIndex = 6;
            btnDangKy.Text = "Đăng ký";
            btnDangKy.UseVisualStyleBackColor = true;
            btnDangKy.Click += btnDangKy_Click;
            // 
            // btnHuy
            // 
            btnHuy.DialogResult = DialogResult.Cancel;
            btnHuy.Location = new Point(384, 367);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new Size(116, 37);
            btnHuy.TabIndex = 7;
            btnHuy.Text = "Hủy";
            btnHuy.UseVisualStyleBackColor = true;
            btnHuy.Click += btnHuy_Click;
            // 
            // frmDangKy
            // 
            AcceptButton = btnDangKy;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnHuy;
            ClientSize = new Size(540, 440);
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
