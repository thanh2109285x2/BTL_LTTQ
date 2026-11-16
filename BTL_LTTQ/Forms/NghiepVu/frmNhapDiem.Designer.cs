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
        private ComboBox cboLop;
        private GroupBox grpThongTinDiem;
        private Label lblMaHocVien;
        private Label lblTenHocVien;
        private Label lblDiem;
        private Label lblXepLoai;
        private TextBox txtMaHocVien;
        private TextBox txtTenHocVien;
        private TextBox txtDiem;
        private TextBox txtXepLoai;
        private AppButton btnSuaDiem;
        private AppButton btnLamMoi;
        private TextBox txtTimMaHocVien;
        private TextBox txtTimTenHocVien;
        private AppButton btnTimKiem;
        private DataGridView dgvHocVien;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelTieuDe = new Panel();
            lblMoTa = new Label();
            lblTieuDe = new Label();
            panelNoiDung = new Panel();
            dgvHocVien = new DataGridView();
            btnTimKiem = new AppButton { Kieu = KieuNut.Chinh };
            txtTimTenHocVien = new TextBox();
            txtTimMaHocVien = new TextBox();
            btnLamMoi = new AppButton { Kieu = KieuNut.Phu };
            btnSuaDiem = new AppButton { Kieu = KieuNut.Chinh };
            grpThongTinDiem = new GroupBox();
            lblMaHocVien = new Label();
            lblTenHocVien = new Label();
            lblDiem = new Label();
            lblXepLoai = new Label();
            txtMaHocVien = new TextBox();
            txtTenHocVien = new TextBox();
            txtDiem = new TextBox();
            txtXepLoai = new TextBox();
            cboLop = new ComboBox();
            panelTieuDe.SuspendLayout();
            panelNoiDung.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHocVien).BeginInit();
            grpThongTinDiem.SuspendLayout();
            SuspendLayout();
            // 
            // panelTieuDe
            // 
            panelTieuDe.BackColor = Color.FromArgb(43, 98, 164);
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
            lblMoTa.ForeColor = Color.FromArgb(225, 235, 248);
            lblMoTa.Location = new Point(35, 54);
            lblMoTa.Name = "lblMoTa";
            lblMoTa.Size = new Size(247, 20);
            lblMoTa.TabIndex = 1;
            lblMoTa.Text = "Quản lý điểm học viên theo lớp học.";
            // 
            // lblTieuDe
            // 
            lblTieuDe.AutoSize = true;
            lblTieuDe.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
            lblTieuDe.ForeColor = Color.White;
            lblTieuDe.Location = new Point(32, 12);
            lblTieuDe.Name = "lblTieuDe";
            lblTieuDe.Size = new Size(168, 40);
            lblTieuDe.TabIndex = 0;
            lblTieuDe.Text = "Nhập điểm";
            // 
            // panelNoiDung
            // 
            panelNoiDung.Controls.Add(dgvHocVien);
            panelNoiDung.Controls.Add(btnTimKiem);
            panelNoiDung.Controls.Add(txtTimTenHocVien);
            panelNoiDung.Controls.Add(txtTimMaHocVien);
            panelNoiDung.Controls.Add(btnLamMoi);
            panelNoiDung.Controls.Add(btnSuaDiem);
            panelNoiDung.Controls.Add(grpThongTinDiem);
            panelNoiDung.Controls.Add(cboLop);
            panelNoiDung.Dock = DockStyle.Fill;
            panelNoiDung.Location = new Point(0, 88);
            panelNoiDung.Name = "panelNoiDung";
            panelNoiDung.Padding = new Padding(32, 16, 32, 16);
            panelNoiDung.Size = new Size(1200, 472);
            panelNoiDung.TabIndex = 1;
            // 
            // dgvHocVien
            // 
            dgvHocVien.AllowUserToAddRows = false;
            dgvHocVien.AllowUserToDeleteRows = false;
            dgvHocVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHocVien.BackgroundColor = Color.White;
            dgvHocVien.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHocVien.Location = new Point(32, 284);
            dgvHocVien.MultiSelect = false;
            dgvHocVien.Name = "dgvHocVien";
            dgvHocVien.ReadOnly = true;
            dgvHocVien.RowHeadersVisible = false;
            dgvHocVien.RowHeadersWidth = 51;
            dgvHocVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHocVien.Size = new Size(1136, 170);
            dgvHocVien.TabIndex = 6;
            // 
            // btnTimKiem
            // 
            btnTimKiem.Location = new Point(462, 238);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(100, 36);
            btnTimKiem.TabIndex = 5;
            btnTimKiem.Text = "Tìm kiếm";
            // 
            // txtTimTenHocVien
            // 
            txtTimTenHocVien.Location = new Point(242, 242);
            txtTimTenHocVien.Name = "txtTimTenHocVien";
            txtTimTenHocVien.PlaceholderText = "Tìm theo tên học viên";
            txtTimTenHocVien.Size = new Size(210, 27);
            txtTimTenHocVien.TabIndex = 7;
            // 
            // txtTimMaHocVien
            // 
            txtTimMaHocVien.Location = new Point(32, 242);
            txtTimMaHocVien.Name = "txtTimMaHocVien";
            txtTimMaHocVien.PlaceholderText = "Tìm theo mã học viên";
            txtTimMaHocVien.Size = new Size(200, 27);
            txtTimMaHocVien.TabIndex = 8;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Location = new Point(172, 157);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(110, 38);
            btnLamMoi.TabIndex = 3;
            btnLamMoi.Text = "Làm mới";
            // 
            // btnSuaDiem
            // 
            btnSuaDiem.Location = new Point(32, 157);
            btnSuaDiem.Name = "btnSuaDiem";
            btnSuaDiem.Size = new Size(130, 38);
            btnSuaDiem.TabIndex = 2;
            btnSuaDiem.Text = "Sửa điểm";
            // 
            // grpThongTinDiem
            // 
            grpThongTinDiem.Controls.Add(lblMaHocVien);
            grpThongTinDiem.Controls.Add(lblTenHocVien);
            grpThongTinDiem.Controls.Add(lblDiem);
            grpThongTinDiem.Controls.Add(lblXepLoai);
            grpThongTinDiem.Controls.Add(txtMaHocVien);
            grpThongTinDiem.Controls.Add(txtTenHocVien);
            grpThongTinDiem.Controls.Add(txtDiem);
            grpThongTinDiem.Controls.Add(txtXepLoai);
            grpThongTinDiem.Location = new Point(35, 19);
            grpThongTinDiem.Name = "grpThongTinDiem";
            grpThongTinDiem.Size = new Size(1136, 132);
            grpThongTinDiem.TabIndex = 1;
            grpThongTinDiem.TabStop = false;
            grpThongTinDiem.Text = "Thông tin điểm";
            // 
            // lblMaHocVien
            // 
            lblMaHocVien.AutoSize = true;
            lblMaHocVien.Location = new Point(16, 30);
            lblMaHocVien.Name = "lblMaHocVien";
            lblMaHocVien.Size = new Size(89, 20);
            lblMaHocVien.TabIndex = 0;
            lblMaHocVien.Text = "Mã học viên";
            // 
            // lblTenHocVien
            // 
            lblTenHocVien.AutoSize = true;
            lblTenHocVien.Location = new Point(430, 30);
            lblTenHocVien.Name = "lblTenHocVien";
            lblTenHocVien.Size = new Size(73, 20);
            lblTenHocVien.TabIndex = 1;
            lblTenHocVien.Text = "Họ và tên";
            // 
            // lblDiem
            // 
            lblDiem.AutoSize = true;
            lblDiem.Location = new Point(16, 78);
            lblDiem.Name = "lblDiem";
            lblDiem.Size = new Size(45, 20);
            lblDiem.TabIndex = 2;
            lblDiem.Text = "Điểm";
            // 
            // lblXepLoai
            // 
            lblXepLoai.AutoSize = true;
            lblXepLoai.Location = new Point(430, 78);
            lblXepLoai.Name = "lblXepLoai";
            lblXepLoai.Size = new Size(64, 20);
            lblXepLoai.TabIndex = 3;
            lblXepLoai.Text = "Xếp loại";
            // 
            // txtMaHocVien
            // 
            txtMaHocVien.Location = new Point(128, 27);
            txtMaHocVien.Name = "txtMaHocVien";
            txtMaHocVien.ReadOnly = true;
            txtMaHocVien.Size = new Size(260, 27);
            txtMaHocVien.TabIndex = 4;
            // 
            // txtTenHocVien
            // 
            txtTenHocVien.Location = new Point(530, 27);
            txtTenHocVien.Name = "txtTenHocVien";
            txtTenHocVien.ReadOnly = true;
            txtTenHocVien.Size = new Size(570, 27);
            txtTenHocVien.TabIndex = 5;
            // 
            // txtDiem
            // 
            txtDiem.Location = new Point(128, 74);
            txtDiem.Name = "txtDiem";
            txtDiem.ReadOnly = true;
            txtDiem.Size = new Size(260, 27);
            txtDiem.TabIndex = 6;
            // 
            // txtXepLoai
            // 
            txtXepLoai.Location = new Point(530, 74);
            txtXepLoai.Name = "txtXepLoai";
            txtXepLoai.ReadOnly = true;
            txtXepLoai.Size = new Size(570, 27);
            txtXepLoai.TabIndex = 7;
            // 
            // cboLop
            // 
            cboLop.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLop.FormattingEnabled = true;
            cboLop.Location = new Point(32, 204);
            cboLop.Name = "cboLop";
            cboLop.Size = new Size(480, 28);
            cboLop.TabIndex = 0;
            // 
            // frmNhapDiem
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 247, 251);
            ClientSize = new Size(1200, 560);
            Controls.Add(panelNoiDung);
            Controls.Add(panelTieuDe);
            Name = "frmNhapDiem";
            Text = "Nhập điểm";
            panelTieuDe.ResumeLayout(false);
            panelTieuDe.PerformLayout();
            panelNoiDung.ResumeLayout(false);
            panelNoiDung.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHocVien).EndInit();
            grpThongTinDiem.ResumeLayout(false);
            grpThongTinDiem.PerformLayout();
            ResumeLayout(false);
        }
    }
}
