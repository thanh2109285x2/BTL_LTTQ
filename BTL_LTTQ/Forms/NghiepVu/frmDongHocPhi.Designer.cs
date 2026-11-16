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
        private Panel panelTimHocVien;
        private TextBox txtTimHocVien;
        private AppButton btnTimHocVien;
        private DataGridView dgvHocVien;
        private Label lblLopChuaLap;
        private DataGridView dgvLopChuaLap;
        private Label lblTongTien;
        private AppButton btnTaoQR;
        private Label lblPhieu;
        private DataGridView dgvPhieuThu;
        private AppButton btnXemQR;
        private AppButton btnHuyPhieu;
        private AppButton btnXacNhanThu;
        private AppButton btnLamMoi;
        private PictureBox picQr;
        private Label lblNganHang;
        private Label lblNhacThanhToan;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                if (disposing) picQr?.Image?.Dispose();
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelTieuDe = new Panel();
            lblMoTa = new Label();
            lblTieuDe = new Label();
            panelNoiDung = new Panel();
            lblNhacThanhToan = new Label();
            lblNganHang = new Label();
            picQr = new PictureBox();
            btnLamMoi = new AppButton();
            btnXacNhanThu = new AppButton();
            btnHuyPhieu = new AppButton();
            btnXemQR = new AppButton();
            dgvPhieuThu = new DataGridView();
            lblPhieu = new Label();
            btnTaoQR = new AppButton();
            lblTongTien = new Label();
            dgvLopChuaLap = new DataGridView();
            lblLopChuaLap = new Label();
            panelTimHocVien = new Panel();
            dgvHocVien = new DataGridView();
            btnTimHocVien = new AppButton();
            txtTimHocVien = new TextBox();
            panelTieuDe.SuspendLayout();
            panelNoiDung.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picQr).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvPhieuThu).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvLopChuaLap).BeginInit();
            panelTimHocVien.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHocVien).BeginInit();
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
            panelTieuDe.Size = new Size(1391, 88);
            panelTieuDe.TabIndex = 0;
            // 
            // lblMoTa
            // 
            lblMoTa.AutoSize = true;
            lblMoTa.Location = new Point(35, 54);
            lblMoTa.Name = "lblMoTa";
            lblMoTa.Size = new Size(383, 20);
            lblMoTa.TabIndex = 1;
            lblMoTa.Text = "Lập phiếu học phí, tạo QR chuyển khoản và xác nhận thu.";
            // 
            // lblTieuDe
            // 
            lblTieuDe.AutoSize = true;
            lblTieuDe.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
            lblTieuDe.ForeColor = Color.FromArgb(30, 74, 130);
            lblTieuDe.Location = new Point(32, 12);
            lblTieuDe.Name = "lblTieuDe";
            lblTieuDe.Size = new Size(200, 40);
            lblTieuDe.TabIndex = 0;
            lblTieuDe.Text = "Đóng học phí";
            // 
            // panelNoiDung
            // 
            panelNoiDung.Controls.Add(lblNhacThanhToan);
            panelNoiDung.Controls.Add(lblNganHang);
            panelNoiDung.Controls.Add(picQr);
            panelNoiDung.Controls.Add(btnLamMoi);
            panelNoiDung.Controls.Add(btnXacNhanThu);
            panelNoiDung.Controls.Add(btnHuyPhieu);
            panelNoiDung.Controls.Add(btnXemQR);
            panelNoiDung.Controls.Add(dgvPhieuThu);
            panelNoiDung.Controls.Add(lblPhieu);
            panelNoiDung.Controls.Add(btnTaoQR);
            panelNoiDung.Controls.Add(lblTongTien);
            panelNoiDung.Controls.Add(dgvLopChuaLap);
            panelNoiDung.Controls.Add(lblLopChuaLap);
            panelNoiDung.Controls.Add(panelTimHocVien);
            panelNoiDung.Dock = DockStyle.Fill;
            panelNoiDung.Location = new Point(0, 88);
            panelNoiDung.Name = "panelNoiDung";
            panelNoiDung.Padding = new Padding(24);
            panelNoiDung.Size = new Size(1391, 645);
            panelNoiDung.TabIndex = 1;
            // 
            // lblNhacThanhToan
            // 
            lblNhacThanhToan.Location = new Point(1034, 519);
            lblNhacThanhToan.Name = "lblNhacThanhToan";
            lblNhacThanhToan.Size = new Size(330, 65);
            lblNhacThanhToan.TabIndex = 0;
            lblNhacThanhToan.Visible = false;
            // 
            // lblNganHang
            // 
            lblNganHang.Location = new Point(1034, 419);
            lblNganHang.Name = "lblNganHang";
            lblNganHang.Size = new Size(330, 100);
            lblNganHang.TabIndex = 1;
            // 
            // picQr
            // 
            picQr.BackColor = Color.White;
            picQr.BorderStyle = BorderStyle.FixedSingle;
            picQr.Location = new Point(1144, 191);
            picQr.Name = "picQr";
            picQr.Size = new Size(220, 220);
            picQr.SizeMode = PictureBoxSizeMode.Zoom;
            picQr.TabIndex = 2;
            picQr.TabStop = false;
            // 
            // btnLamMoi
            // 
            btnLamMoi.BackColor = Color.White;
            btnLamMoi.FlatAppearance.BorderColor = Color.FromArgb(200, 208, 220);
            btnLamMoi.FlatAppearance.MouseDownBackColor = Color.FromArgb(234, 241, 250);
            btnLamMoi.FlatAppearance.MouseOverBackColor = Color.FromArgb(234, 241, 250);
            btnLamMoi.FlatStyle = FlatStyle.Flat;
            btnLamMoi.ForeColor = Color.FromArgb(33, 37, 41);
            btnLamMoi.Location = new Point(418, 572);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(100, 36);
            btnLamMoi.TabIndex = 9;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = false;
            // 
            // btnXacNhanThu
            // 
            btnXacNhanThu.BackColor = Color.White;
            btnXacNhanThu.FlatAppearance.BorderColor = Color.FromArgb(200, 208, 220);
            btnXacNhanThu.FlatAppearance.MouseDownBackColor = Color.FromArgb(234, 241, 250);
            btnXacNhanThu.FlatAppearance.MouseOverBackColor = Color.FromArgb(234, 241, 250);
            btnXacNhanThu.FlatStyle = FlatStyle.Flat;
            btnXacNhanThu.ForeColor = Color.FromArgb(33, 37, 41);
            btnXacNhanThu.Location = new Point(248, 572);
            btnXacNhanThu.Name = "btnXacNhanThu";
            btnXacNhanThu.Size = new Size(160, 36);
            btnXacNhanThu.TabIndex = 8;
            btnXacNhanThu.Text = "Xác nhận đã thu";
            btnXacNhanThu.UseVisualStyleBackColor = false;
            // 
            // btnHuyPhieu
            // 
            btnHuyPhieu.BackColor = Color.White;
            btnHuyPhieu.FlatAppearance.BorderColor = Color.FromArgb(200, 208, 220);
            btnHuyPhieu.FlatAppearance.MouseDownBackColor = Color.FromArgb(234, 241, 250);
            btnHuyPhieu.FlatAppearance.MouseOverBackColor = Color.FromArgb(234, 241, 250);
            btnHuyPhieu.FlatStyle = FlatStyle.Flat;
            btnHuyPhieu.ForeColor = Color.FromArgb(33, 37, 41);
            btnHuyPhieu.Location = new Point(130, 572);
            btnHuyPhieu.Name = "btnHuyPhieu";
            btnHuyPhieu.Size = new Size(110, 36);
            btnHuyPhieu.TabIndex = 7;
            btnHuyPhieu.Text = "Hủy phiếu";
            btnHuyPhieu.UseVisualStyleBackColor = false;
            btnHuyPhieu.Click += btnHuyPhieu_Click;
            // 
            // btnXemQR
            // 
            btnXemQR.BackColor = Color.White;
            btnXemQR.FlatAppearance.BorderColor = Color.FromArgb(200, 208, 220);
            btnXemQR.FlatAppearance.MouseDownBackColor = Color.FromArgb(234, 241, 250);
            btnXemQR.FlatAppearance.MouseOverBackColor = Color.FromArgb(234, 241, 250);
            btnXemQR.FlatStyle = FlatStyle.Flat;
            btnXemQR.ForeColor = Color.FromArgb(33, 37, 41);
            btnXemQR.Location = new Point(22, 572);
            btnXemQR.Name = "btnXemQR";
            btnXemQR.Size = new Size(100, 36);
            btnXemQR.TabIndex = 6;
            btnXemQR.Text = "Xem QR";
            btnXemQR.UseVisualStyleBackColor = false;
            btnXemQR.Click += btnXemQR_Click;
            // 
            // dgvPhieuThu
            // 
            dgvPhieuThu.AllowUserToAddRows = false;
            dgvPhieuThu.AllowUserToDeleteRows = false;
            dgvPhieuThu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPhieuThu.ColumnHeadersHeight = 29;
            dgvPhieuThu.Location = new Point(24, 402);
            dgvPhieuThu.MultiSelect = false;
            dgvPhieuThu.Name = "dgvPhieuThu";
            dgvPhieuThu.ReadOnly = true;
            dgvPhieuThu.RowHeadersVisible = false;
            dgvPhieuThu.RowHeadersWidth = 51;
            dgvPhieuThu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPhieuThu.Size = new Size(1004, 150);
            dgvPhieuThu.TabIndex = 5;
            dgvPhieuThu.SelectionChanged += dgvPhieuThu_SelectionChanged;
            // 
            // lblPhieu
            // 
            lblPhieu.AutoSize = true;
            lblPhieu.Location = new Point(24, 379);
            lblPhieu.Name = "lblPhieu";
            lblPhieu.Size = new Size(70, 20);
            lblPhieu.TabIndex = 4;
            lblPhieu.Text = "Phiếu thu";
            // 
            // btnTaoQR
            // 
            btnTaoQR.BackColor = Color.White;
            btnTaoQR.FlatAppearance.BorderColor = Color.FromArgb(200, 208, 220);
            btnTaoQR.FlatAppearance.MouseDownBackColor = Color.FromArgb(234, 241, 250);
            btnTaoQR.FlatAppearance.MouseOverBackColor = Color.FromArgb(234, 241, 250);
            btnTaoQR.FlatStyle = FlatStyle.Flat;
            btnTaoQR.ForeColor = Color.FromArgb(33, 37, 41);
            btnTaoQR.Location = new Point(929, 342);
            btnTaoQR.Name = "btnTaoQR";
            btnTaoQR.Size = new Size(139, 36);
            btnTaoQR.TabIndex = 3;
            btnTaoQR.Text = "Tạo mã QR";
            btnTaoQR.UseVisualStyleBackColor = false;
            btnTaoQR.Click += btnTaoQR_Click;
            // 
            // lblTongTien
            // 
            lblTongTien.AutoSize = true;
            lblTongTien.Location = new Point(24, 339);
            lblTongTien.Name = "lblTongTien";
            lblTongTien.Size = new Size(100, 20);
            lblTongTien.TabIndex = 2;
            lblTongTien.Text = "Tổng tiền: 0 đ";
            // 
            // dgvLopChuaLap
            // 
            dgvLopChuaLap.AllowUserToAddRows = false;
            dgvLopChuaLap.AllowUserToDeleteRows = false;
            dgvLopChuaLap.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLopChuaLap.ColumnHeadersHeight = 29;
            dgvLopChuaLap.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvLopChuaLap.Location = new Point(24, 191);
            dgvLopChuaLap.MultiSelect = false;
            dgvLopChuaLap.Name = "dgvLopChuaLap";
            dgvLopChuaLap.RowHeadersVisible = false;
            dgvLopChuaLap.RowHeadersWidth = 51;
            dgvLopChuaLap.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLopChuaLap.Size = new Size(1044, 145);
            dgvLopChuaLap.TabIndex = 1;
            dgvLopChuaLap.CellValueChanged += dgvLopChuaLap_SelectionChanged;
            dgvLopChuaLap.CurrentCellDirtyStateChanged += dgvLopChuaLap_CurrentCellDirtyStateChanged;
            dgvLopChuaLap.SelectionChanged += dgvLopChuaLap_SelectionChanged;
            // 
            // lblLopChuaLap
            // 
            lblLopChuaLap.AutoSize = true;
            lblLopChuaLap.Location = new Point(24, 166);
            lblLopChuaLap.Name = "lblLopChuaLap";
            lblLopChuaLap.Size = new Size(216, 20);
            lblLopChuaLap.TabIndex = 0;
            lblLopChuaLap.Text = "Lớp đã đăng ký, chưa lập phiếu";
            // 
            // panelTimHocVien
            // 
            panelTimHocVien.Controls.Add(dgvHocVien);
            panelTimHocVien.Controls.Add(btnTimHocVien);
            panelTimHocVien.Controls.Add(txtTimHocVien);
            panelTimHocVien.Location = new Point(24, 16);
            panelTimHocVien.Name = "panelTimHocVien";
            panelTimHocVien.Size = new Size(1232, 147);
            panelTimHocVien.TabIndex = 0;
            // 
            // dgvHocVien
            // 
            dgvHocVien.AllowUserToAddRows = false;
            dgvHocVien.AllowUserToDeleteRows = false;
            dgvHocVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHocVien.ColumnHeadersHeight = 29;
            dgvHocVien.Location = new Point(0, 34);
            dgvHocVien.MultiSelect = false;
            dgvHocVien.Name = "dgvHocVien";
            dgvHocVien.ReadOnly = true;
            dgvHocVien.RowHeadersVisible = false;
            dgvHocVien.RowHeadersWidth = 51;
            dgvHocVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHocVien.Size = new Size(440, 110);
            dgvHocVien.TabIndex = 2;
            dgvHocVien.SelectionChanged += dgvHocVien_SelectionChanged;
            // 
            // btnTimHocVien
            // 
            btnTimHocVien.BackColor = Color.White;
            btnTimHocVien.FlatAppearance.BorderColor = Color.FromArgb(200, 208, 220);
            btnTimHocVien.FlatAppearance.MouseDownBackColor = Color.FromArgb(234, 241, 250);
            btnTimHocVien.FlatAppearance.MouseOverBackColor = Color.FromArgb(234, 241, 250);
            btnTimHocVien.FlatStyle = FlatStyle.Flat;
            btnTimHocVien.ForeColor = Color.FromArgb(33, 37, 41);
            btnTimHocVien.Location = new Point(316, 0);
            btnTimHocVien.Name = "btnTimHocVien";
            btnTimHocVien.Size = new Size(105, 32);
            btnTimHocVien.TabIndex = 1;
            btnTimHocVien.Text = "Tìm kiếm";
            btnTimHocVien.UseVisualStyleBackColor = false;
            btnTimHocVien.Click += btnTimHocVien_Click;
            // 
            // txtTimHocVien
            // 
            txtTimHocVien.Location = new Point(0, 3);
            txtTimHocVien.Name = "txtTimHocVien";
            txtTimHocVien.PlaceholderText = "Tìm học viên theo mã hoặc tên";
            txtTimHocVien.Size = new Size(310, 27);
            txtTimHocVien.TabIndex = 0;
            txtTimHocVien.KeyDown += txtTimHocVien_KeyDown;
            // 
            // frmDongHocPhi
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 247, 251);
            ClientSize = new Size(1391, 733);
            Controls.Add(panelNoiDung);
            Controls.Add(panelTieuDe);
            Name = "frmDongHocPhi";
            Text = "Đóng học phí";
            Load += frmDongHocPhi_Load;
            panelTieuDe.ResumeLayout(false);
            panelTieuDe.PerformLayout();
            panelNoiDung.ResumeLayout(false);
            panelNoiDung.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picQr).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvPhieuThu).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvLopChuaLap).EndInit();
            panelTimHocVien.ResumeLayout(false);
            panelTimHocVien.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHocVien).EndInit();
            ResumeLayout(false);
        }
    }
}
