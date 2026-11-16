#nullable enable
namespace BTL_LTTQ.Forms.QuanLy
{
    partial class frmHocVien
    {
        private System.ComponentModel.IContainer? components = null;
        private TextBox txtMaHocVien = null!;
        private TextBox txtTenHocVien = null!;
        private ComboBox cboDiaChi = null!;
        private TextBox txtDienThoai = null!;
        private TextBox txtTimKiem = null!;
        private DateTimePicker dtpNgaySinh = null!;
        private ComboBox cboGioiTinh = null!;
        private ComboBox cboNghe = null!;
        private DataGridView dgvHocVien = null!;
        private AppButton btnThem = null!;
        private AppButton btnSua = null!;
        private AppButton btnXoa = null!;
        private AppButton btnLamMoi = null!;
        private AppButton btnTimKiem = null!;
        private GroupBox grpThongTin = null!;
        private GroupBox grpDanhSach = null!;
        private Panel pnlNhap = null!;
        private Panel pnlTim = null!;
        private FlowLayoutPanel pnlNut = null!;
        private Label lblMaHocVien = null!;
        private Label lblTenHocVien = null!;
        private Label lblNgaySinh = null!;
        private Label lblGioiTinh = null!;
        private Label lblDienThoai = null!;
        private Label lblDiaChi = null!;
        private Label lblNghe = null!;
        private Label lblTimKiem = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            txtMaHocVien = new TextBox();
            txtTenHocVien = new TextBox();
            cboDiaChi = new ComboBox();
            txtDienThoai = new TextBox();
            txtTimKiem = new TextBox();
            dtpNgaySinh = new DateTimePicker();
            cboGioiTinh = new ComboBox();
            cboNghe = new ComboBox();
            dgvHocVien = new DataGridView();
            btnThem = new AppButton();
            btnSua = new AppButton();
            btnXoa = new AppButton();
            btnLamMoi = new AppButton();
            btnTimKiem = new AppButton();
            grpThongTin = new GroupBox();
            lblMaHocVien = new Label();
            lblTenHocVien = new Label();
            lblNgaySinh = new Label();
            lblGioiTinh = new Label();
            lblDienThoai = new Label();
            lblDiaChi = new Label();
            lblNghe = new Label();
            pnlNut = new FlowLayoutPanel();
            grpDanhSach = new GroupBox();
            pnlNhap = new Panel();
            pnlTim = new Panel();
            lblTimKiem = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvHocVien).BeginInit();
            grpThongTin.SuspendLayout();
            pnlNut.SuspendLayout();
            grpDanhSach.SuspendLayout();
            pnlNhap.SuspendLayout();
            pnlTim.SuspendLayout();
            SuspendLayout();
            // 
            // txtMaHocVien
            // 
            txtMaHocVien.Location = new Point(163, 28);
            txtMaHocVien.Name = "txtMaHocVien";
            txtMaHocVien.Size = new Size(220, 27);
            txtMaHocVien.TabIndex = 1;
            // 
            // txtTenHocVien
            // 
            txtTenHocVien.Location = new Point(163, 66);
            txtTenHocVien.Name = "txtTenHocVien";
            txtTenHocVien.Size = new Size(220, 27);
            txtTenHocVien.TabIndex = 3;
            // 
            // cboDiaChi
            // 
            cboDiaChi.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboDiaChi.AutoCompleteSource = AutoCompleteSource.ListItems;
            cboDiaChi.DropDownStyle = ComboBoxStyle.DropDown;
            cboDiaChi.Location = new Point(200, 218);
            cboDiaChi.Name = "cboDiaChi";
            cboDiaChi.Size = new Size(220, 28);
            cboDiaChi.TabIndex = 11;
            // 
            // txtDienThoai
            // 
            txtDienThoai.Location = new Point(163, 180);
            txtDienThoai.Name = "txtDienThoai";
            txtDienThoai.Size = new Size(220, 27);
            txtDienThoai.TabIndex = 9;
            // 
            // txtTimKiem
            // 
            txtTimKiem.Location = new Point(210, 7);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Size = new Size(300, 27);
            txtTimKiem.TabIndex = 1;
            txtTimKiem.KeyDown += txtTimKiem_KeyDown;
            // 
            // dtpNgaySinh
            // 
            dtpNgaySinh.CustomFormat = "dd/MM/yyyy";
            dtpNgaySinh.Format = DateTimePickerFormat.Custom;
            dtpNgaySinh.Location = new Point(163, 104);
            dtpNgaySinh.Name = "dtpNgaySinh";
            dtpNgaySinh.Size = new Size(220, 27);
            dtpNgaySinh.TabIndex = 5;
            // 
            // cboGioiTinh
            // 
            cboGioiTinh.DropDownStyle = ComboBoxStyle.DropDownList;
            cboGioiTinh.Items.AddRange(new object[] { "Nam", "Nữ", "Khác" });
            cboGioiTinh.Location = new Point(163, 142);
            cboGioiTinh.Name = "cboGioiTinh";
            cboGioiTinh.Size = new Size(220, 28);
            cboGioiTinh.TabIndex = 7;
            // 
            // cboNghe
            // 
            cboNghe.DropDownStyle = ComboBoxStyle.DropDownList;
            cboNghe.Location = new Point(605, 66);
            cboNghe.Name = "cboNghe";
            cboNghe.Size = new Size(230, 28);
            cboNghe.TabIndex = 15;
            // 
            // dgvHocVien
            // 
            dgvHocVien.ColumnHeadersHeight = 29;
            dgvHocVien.Dock = DockStyle.Fill;
            dgvHocVien.Location = new Point(8, 28);
            dgvHocVien.Name = "dgvHocVien";
            dgvHocVien.RowHeadersWidth = 51;
            dgvHocVien.Size = new Size(984, 296);
            dgvHocVien.TabIndex = 0;
            dgvHocVien.CellClick += dgvHocVien_CellClick;
            // 
            // btnThem
            // 
            btnThem.BackColor = Color.White;
            btnThem.FlatAppearance.BorderColor = Color.FromArgb(200, 208, 220);
            btnThem.FlatAppearance.MouseDownBackColor = Color.FromArgb(234, 241, 250);
            btnThem.FlatAppearance.MouseOverBackColor = Color.FromArgb(234, 241, 250);
            btnThem.FlatStyle = FlatStyle.Flat;
            btnThem.ForeColor = Color.FromArgb(33, 37, 41);
            btnThem.Location = new Point(155, 5);
            btnThem.Margin = new Padding(5, 0, 5, 0);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(100, 36);
            btnThem.TabIndex = 0;
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = false;
            btnThem.Click += btnThem_Click;
            // 
            // btnSua
            // 
            btnSua.BackColor = Color.White;
            btnSua.FlatAppearance.BorderColor = Color.FromArgb(200, 208, 220);
            btnSua.FlatAppearance.MouseDownBackColor = Color.FromArgb(234, 241, 250);
            btnSua.FlatAppearance.MouseOverBackColor = Color.FromArgb(234, 241, 250);
            btnSua.FlatStyle = FlatStyle.Flat;
            btnSua.ForeColor = Color.FromArgb(33, 37, 41);
            btnSua.Location = new Point(265, 5);
            btnSua.Margin = new Padding(5, 0, 5, 0);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(100, 36);
            btnSua.TabIndex = 1;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = false;
            btnSua.Click += btnSua_Click;
            // 
            // btnXoa
            // 
            btnXoa.BackColor = Color.White;
            btnXoa.FlatAppearance.BorderColor = Color.FromArgb(200, 208, 220);
            btnXoa.FlatAppearance.MouseDownBackColor = Color.FromArgb(234, 241, 250);
            btnXoa.FlatAppearance.MouseOverBackColor = Color.FromArgb(234, 241, 250);
            btnXoa.FlatStyle = FlatStyle.Flat;
            btnXoa.ForeColor = Color.FromArgb(33, 37, 41);
            btnXoa.Location = new Point(375, 5);
            btnXoa.Margin = new Padding(5, 0, 5, 0);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(100, 36);
            btnXoa.TabIndex = 2;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = false;
            btnXoa.Click += btnXoa_Click;
            // 
            // btnLamMoi
            // 
            btnLamMoi.BackColor = Color.White;
            btnLamMoi.FlatAppearance.BorderColor = Color.FromArgb(200, 208, 220);
            btnLamMoi.FlatAppearance.MouseDownBackColor = Color.FromArgb(234, 241, 250);
            btnLamMoi.FlatAppearance.MouseOverBackColor = Color.FromArgb(234, 241, 250);
            btnLamMoi.FlatStyle = FlatStyle.Flat;
            btnLamMoi.ForeColor = Color.FromArgb(33, 37, 41);
            btnLamMoi.Location = new Point(485, 5);
            btnLamMoi.Margin = new Padding(5, 0, 5, 0);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(100, 36);
            btnLamMoi.TabIndex = 3;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = false;
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // btnTimKiem
            // 
            btnTimKiem.BackColor = Color.White;
            btnTimKiem.FlatAppearance.BorderColor = Color.FromArgb(200, 208, 220);
            btnTimKiem.FlatAppearance.MouseDownBackColor = Color.FromArgb(234, 241, 250);
            btnTimKiem.FlatAppearance.MouseOverBackColor = Color.FromArgb(234, 241, 250);
            btnTimKiem.FlatStyle = FlatStyle.Flat;
            btnTimKiem.ForeColor = Color.FromArgb(33, 37, 41);
            btnTimKiem.Location = new Point(520, 4);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(110, 34);
            btnTimKiem.TabIndex = 2;
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.UseVisualStyleBackColor = false;
            btnTimKiem.Click += btnTimKiem_Click;
            // 
            // grpThongTin
            // 
            grpThongTin.Controls.Add(lblMaHocVien);
            grpThongTin.Controls.Add(txtMaHocVien);
            grpThongTin.Controls.Add(lblTenHocVien);
            grpThongTin.Controls.Add(txtTenHocVien);
            grpThongTin.Controls.Add(lblNgaySinh);
            grpThongTin.Controls.Add(dtpNgaySinh);
            grpThongTin.Controls.Add(lblGioiTinh);
            grpThongTin.Controls.Add(cboGioiTinh);
            grpThongTin.Controls.Add(lblDienThoai);
            grpThongTin.Controls.Add(txtDienThoai);
            grpThongTin.Controls.Add(lblDiaChi);
            grpThongTin.Controls.Add(cboDiaChi);
            grpThongTin.Controls.Add(lblNghe);
            grpThongTin.Controls.Add(cboNghe);
            grpThongTin.Controls.Add(pnlNut);
            grpThongTin.Dock = DockStyle.Fill;
            grpThongTin.Location = new Point(10, 10);
            grpThongTin.Name = "grpThongTin";
            grpThongTin.Size = new Size(980, 304);
            grpThongTin.TabIndex = 0;
            grpThongTin.TabStop = false;
            grpThongTin.Text = "Thông tin học viên";
            // 
            // lblMaHocVien
            // 
            lblMaHocVien.Location = new Point(18, 33);
            lblMaHocVien.Name = "lblMaHocVien";
            lblMaHocVien.Size = new Size(145, 22);
            lblMaHocVien.TabIndex = 0;
            lblMaHocVien.Text = "Mã học viên";
            // 
            // lblTenHocVien
            // 
            lblTenHocVien.Location = new Point(18, 71);
            lblTenHocVien.Name = "lblTenHocVien";
            lblTenHocVien.Size = new Size(145, 22);
            lblTenHocVien.TabIndex = 2;
            lblTenHocVien.Text = "Tên học viên";
            // 
            // lblNgaySinh
            // 
            lblNgaySinh.Location = new Point(18, 109);
            lblNgaySinh.Name = "lblNgaySinh";
            lblNgaySinh.Size = new Size(145, 22);
            lblNgaySinh.TabIndex = 4;
            lblNgaySinh.Text = "Ngày sinh";
            // 
            // lblGioiTinh
            // 
            lblGioiTinh.Location = new Point(18, 147);
            lblGioiTinh.Name = "lblGioiTinh";
            lblGioiTinh.Size = new Size(145, 22);
            lblGioiTinh.TabIndex = 6;
            lblGioiTinh.Text = "Giới tính";
            // 
            // lblDienThoai
            // 
            lblDienThoai.Location = new Point(18, 185);
            lblDienThoai.Name = "lblDienThoai";
            lblDienThoai.Size = new Size(145, 22);
            lblDienThoai.TabIndex = 8;
            lblDienThoai.Text = "Điện thoại";
            // 
            // lblDiaChi
            // 
            lblDiaChi.Location = new Point(18, 223);
            lblDiaChi.Name = "lblDiaChi";
            lblDiaChi.Size = new Size(175, 22);
            lblDiaChi.TabIndex = 10;
            lblDiaChi.Text = "Địa chỉ (Tỉnh/Thành phố)";
            // 
            // lblNghe
            // 
            lblNghe.Location = new Point(455, 71);
            lblNghe.Name = "lblNghe";
            lblNghe.Size = new Size(150, 22);
            lblNghe.TabIndex = 14;
            lblNghe.Text = "Nghề nghiệp";
            // 
            // pnlNut
            // 
            pnlNut.Controls.Add(btnThem);
            pnlNut.Controls.Add(btnSua);
            pnlNut.Controls.Add(btnXoa);
            pnlNut.Controls.Add(btnLamMoi);
            pnlNut.Dock = DockStyle.Bottom;
            pnlNut.Location = new Point(3, 253);
            pnlNut.Name = "pnlNut";
            pnlNut.Padding = new Padding(150, 5, 0, 0);
            pnlNut.Size = new Size(974, 48);
            pnlNut.TabIndex = 22;
            pnlNut.WrapContents = false;
            // 
            // grpDanhSach
            // 
            grpDanhSach.Controls.Add(dgvHocVien);
            grpDanhSach.Dock = DockStyle.Fill;
            grpDanhSach.Location = new Point(0, 368);
            grpDanhSach.Name = "grpDanhSach";
            grpDanhSach.Padding = new Padding(8);
            grpDanhSach.Size = new Size(1000, 332);
            grpDanhSach.TabIndex = 0;
            grpDanhSach.TabStop = false;
            grpDanhSach.Text = "Danh sách học viên";
            // 
            // pnlNhap
            // 
            pnlNhap.Controls.Add(grpThongTin);
            pnlNhap.Dock = DockStyle.Top;
            pnlNhap.Location = new Point(0, 0);
            pnlNhap.Name = "pnlNhap";
            pnlNhap.Padding = new Padding(10);
            pnlNhap.Size = new Size(1000, 324);
            pnlNhap.TabIndex = 2;
            // 
            // pnlTim
            // 
            pnlTim.Controls.Add(lblTimKiem);
            pnlTim.Controls.Add(txtTimKiem);
            pnlTim.Controls.Add(btnTimKiem);
            pnlTim.Dock = DockStyle.Top;
            pnlTim.Location = new Point(0, 324);
            pnlTim.Name = "pnlTim";
            pnlTim.Size = new Size(1000, 44);
            pnlTim.TabIndex = 1;
            // 
            // lblTimKiem
            // 
            lblTimKiem.AutoSize = true;
            lblTimKiem.Location = new Point(5, 12);
            lblTimKiem.Name = "lblTimKiem";
            lblTimKiem.Size = new Size(197, 20);
            lblTimKiem.TabIndex = 0;
            lblTimKiem.Text = "Tìm mã, tên hoặc điện thoại:";
            // 
            // frmHocVien
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 247, 251);
            ClientSize = new Size(1000, 700);
            Controls.Add(grpDanhSach);
            Controls.Add(pnlTim);
            Controls.Add(pnlNhap);
            MinimumSize = new Size(850, 600);
            Name = "frmHocVien";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quản lý học viên";
            ((System.ComponentModel.ISupportInitialize)dgvHocVien).EndInit();
            grpThongTin.ResumeLayout(false);
            grpThongTin.PerformLayout();
            pnlNut.ResumeLayout(false);
            grpDanhSach.ResumeLayout(false);
            pnlNhap.ResumeLayout(false);
            pnlTim.ResumeLayout(false);
            pnlTim.PerformLayout();
            ResumeLayout(false);
        }

    }
}