#nullable enable
namespace BTL_LTTQ.Forms.QuanLy
{
    partial class frmHocVien
    {
        private System.ComponentModel.IContainer? components = null;
        private TextBox txtMaHocVien = null!;
        private TextBox txtTenHocVien = null!;
        private TextBox txtDiaChi = null!;
        private TextBox txtDienThoai = null!;
        private TextBox txtTimKiem = null!;
        private DateTimePicker dtpNgaySinh = null!;
        private DateTimePicker dtpNgayNopHocPhi = null!;
        private ComboBox cboGioiTinh = null!;
        private ComboBox cboLop = null!;
        private ComboBox cboNghe = null!;
        private ComboBox cboXepLoai = null!;
        private NumericUpDown nudDiem = null!;
        private DataGridView dgvHocVien = null!;
        private Button btnThem = null!;
        private Button btnSua = null!;
        private Button btnXoa = null!;
        private Button btnLamMoi = null!;
        private Button btnTimKiem = null!;
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
        private Label lblLop = null!;
        private Label lblNghe = null!;
        private Label lblNgayNopHocPhi = null!;
        private Label lblDiem = null!;
        private Label lblXepLoai = null!;
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
            txtDiaChi = new TextBox();
            txtDienThoai = new TextBox();
            txtTimKiem = new TextBox();
            dtpNgaySinh = new DateTimePicker();
            dtpNgayNopHocPhi = new DateTimePicker();
            cboGioiTinh = new ComboBox();
            cboLop = new ComboBox();
            cboNghe = new ComboBox();
            cboXepLoai = new ComboBox();
            nudDiem = new NumericUpDown();
            dgvHocVien = new DataGridView();
            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnLamMoi = new Button();
            btnTimKiem = new Button();
            grpThongTin = new GroupBox();
            lblMaHocVien = new Label();
            lblTenHocVien = new Label();
            lblNgaySinh = new Label();
            lblGioiTinh = new Label();
            lblDienThoai = new Label();
            lblDiaChi = new Label();
            lblLop = new Label();
            lblNghe = new Label();
            lblNgayNopHocPhi = new Label();
            lblDiem = new Label();
            lblXepLoai = new Label();
            pnlNut = new FlowLayoutPanel();
            grpDanhSach = new GroupBox();
            pnlNhap = new Panel();
            pnlTim = new Panel();
            lblTimKiem = new Label();
            ((System.ComponentModel.ISupportInitialize)nudDiem).BeginInit();
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
            // txtDiaChi
            // 
            txtDiaChi.Location = new Point(163, 218);
            txtDiaChi.Name = "txtDiaChi";
            txtDiaChi.Size = new Size(220, 27);
            txtDiaChi.TabIndex = 11;
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
            txtTimKiem.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtTimKiem.Location = new Point(210, 7);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Size = new Size(1040, 27);
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
            // dtpNgayNopHocPhi
            // 
            dtpNgayNopHocPhi.CustomFormat = "dd/MM/yyyy";
            dtpNgayNopHocPhi.Format = DateTimePickerFormat.Custom;
            dtpNgayNopHocPhi.Location = new Point(605, 104);
            dtpNgayNopHocPhi.Name = "dtpNgayNopHocPhi";
            dtpNgayNopHocPhi.Size = new Size(230, 27);
            dtpNgayNopHocPhi.TabIndex = 17;
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
            // cboLop
            // 
            cboLop.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLop.Location = new Point(605, 28);
            cboLop.Name = "cboLop";
            cboLop.Size = new Size(230, 28);
            cboLop.TabIndex = 13;
            // 
            // cboNghe
            // 
            cboNghe.DropDownStyle = ComboBoxStyle.DropDownList;
            cboNghe.Location = new Point(605, 66);
            cboNghe.Name = "cboNghe";
            cboNghe.Size = new Size(230, 28);
            cboNghe.TabIndex = 15;
            // 
            // cboXepLoai
            // 
            cboXepLoai.DropDownStyle = ComboBoxStyle.DropDownList;
            cboXepLoai.Location = new Point(605, 180);
            cboXepLoai.Name = "cboXepLoai";
            cboXepLoai.Size = new Size(230, 28);
            cboXepLoai.TabIndex = 21;
            // 
            // nudDiem
            // 
            nudDiem.DecimalPlaces = 2;
            nudDiem.Location = new Point(605, 142);
            nudDiem.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            nudDiem.Name = "nudDiem";
            nudDiem.Size = new Size(230, 27);
            nudDiem.TabIndex = 19;
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
            btnThem.Location = new Point(155, 5);
            btnThem.Margin = new Padding(5, 0, 5, 0);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(100, 36);
            btnThem.TabIndex = 0;
            btnThem.Text = "Thêm";
            btnThem.Click += btnThem_Click;
            // 
            // btnSua
            // 
            btnSua.Location = new Point(265, 5);
            btnSua.Margin = new Padding(5, 0, 5, 0);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(100, 36);
            btnSua.TabIndex = 1;
            btnSua.Text = "Sửa";
            btnSua.Click += btnSua_Click;
            // 
            // btnXoa
            // 
            btnXoa.Location = new Point(375, 5);
            btnXoa.Margin = new Padding(5, 0, 5, 0);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(100, 36);
            btnXoa.TabIndex = 2;
            btnXoa.Text = "Xóa";
            btnXoa.Click += btnXoa_Click;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Location = new Point(485, 5);
            btnLamMoi.Margin = new Padding(5, 0, 5, 0);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(100, 36);
            btnLamMoi.TabIndex = 3;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // btnTimKiem
            // 
            btnTimKiem.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnTimKiem.Location = new Point(1260, 4);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(110, 34);
            btnTimKiem.TabIndex = 2;
            btnTimKiem.Text = "Tìm kiếm";
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
            grpThongTin.Controls.Add(txtDiaChi);
            grpThongTin.Controls.Add(lblLop);
            grpThongTin.Controls.Add(cboLop);
            grpThongTin.Controls.Add(lblNghe);
            grpThongTin.Controls.Add(cboNghe);
            grpThongTin.Controls.Add(lblNgayNopHocPhi);
            grpThongTin.Controls.Add(dtpNgayNopHocPhi);
            grpThongTin.Controls.Add(lblDiem);
            grpThongTin.Controls.Add(nudDiem);
            grpThongTin.Controls.Add(lblXepLoai);
            grpThongTin.Controls.Add(cboXepLoai);
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
            lblDiaChi.Size = new Size(145, 22);
            lblDiaChi.TabIndex = 10;
            lblDiaChi.Text = "Địa chỉ";
            // 
            // lblLop
            // 
            lblLop.Location = new Point(455, 33);
            lblLop.Name = "lblLop";
            lblLop.Size = new Size(150, 22);
            lblLop.TabIndex = 12;
            lblLop.Text = "Lớp học";
            // 
            // lblNghe
            // 
            lblNghe.Location = new Point(455, 71);
            lblNghe.Name = "lblNghe";
            lblNghe.Size = new Size(150, 22);
            lblNghe.TabIndex = 14;
            lblNghe.Text = "Nghề nghiệp";
            // 
            // lblNgayNopHocPhi
            // 
            lblNgayNopHocPhi.Location = new Point(455, 109);
            lblNgayNopHocPhi.Name = "lblNgayNopHocPhi";
            lblNgayNopHocPhi.Size = new Size(150, 22);
            lblNgayNopHocPhi.TabIndex = 16;
            lblNgayNopHocPhi.Text = "Ngày nộp học phí";
            // 
            // lblDiem
            // 
            lblDiem.Location = new Point(455, 147);
            lblDiem.Name = "lblDiem";
            lblDiem.Size = new Size(150, 22);
            lblDiem.TabIndex = 18;
            lblDiem.Text = "Điểm";
            // 
            // lblXepLoai
            // 
            lblXepLoai.Location = new Point(455, 185);
            lblXepLoai.Name = "lblXepLoai";
            lblXepLoai.Size = new Size(150, 22);
            lblXepLoai.TabIndex = 20;
            lblXepLoai.Text = "Xếp loại";
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
            ((System.ComponentModel.ISupportInitialize)nudDiem).EndInit();
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
