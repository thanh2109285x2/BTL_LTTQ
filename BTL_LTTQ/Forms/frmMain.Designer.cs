namespace BTL_LTTQ
{
    partial class frmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            flowLayoutPanel1 = new FlowLayoutPanel();
            btnTrangChu = new AppButton();
            panelCRUD = new Panel();
            btnDanhMucKhac = new AppButton();
            btnPhongHoc = new AppButton();
            btnMonHoc = new AppButton();
            btnGiaoVien = new AppButton();
            btnHocVien = new AppButton();
            btnLopHoc = new AppButton();
            btnDangKyLop = new AppButton();
            btnNhapDuLieu = new AppButton();
            btnQuanLyMenu = new AppButton();
            panelTraCuu = new Panel();
            btnTimGiaoVien = new AppButton();
            btnTimLop = new AppButton();
            btnThoiKhoaBieu = new AppButton();
            btnTraCuuMenu = new AppButton();
            panelBaoCao = new Panel();
            btnLopTheoPhong = new AppButton();
            btnTop3 = new AppButton();
            btnDoanhThu = new AppButton();
            btnBaoCaoDiem = new AppButton();
            btnBaoCaoMenu = new AppButton();
            panelHeThong = new Panel();
            btnPhanQuyen = new AppButton();
            btnBackup = new AppButton();
            btnRestore = new AppButton();
            btnHeThong = new AppButton();
            panelContent = new Panel();
            lblChao = new Label();
            panelThoat = new Panel();
            btnXuatExcel = new AppButton();
            btnDangXuat = new AppButton();
            btnDoiMatKhau = new AppButton();
            btnThoat = new AppButton();
            panelSidebar = new Panel();
            flowLayoutPanel1.SuspendLayout();
            panelCRUD.SuspendLayout();
            panelTraCuu.SuspendLayout();
            panelBaoCao.SuspendLayout();
            panelHeThong.SuspendLayout();
            panelContent.SuspendLayout();
            panelThoat.SuspendLayout();
            panelSidebar.SuspendLayout();
            SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Controls.Add(btnTrangChu);
            flowLayoutPanel1.Controls.Add(panelCRUD);
            flowLayoutPanel1.Controls.Add(panelTraCuu);
            flowLayoutPanel1.Controls.Add(panelBaoCao);
            flowLayoutPanel1.Controls.Add(panelHeThong);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(284, 354);
            flowLayoutPanel1.TabIndex = 0;
            flowLayoutPanel1.WrapContents = false;
            // 
            // btnTrangChu
            // 
            btnTrangChu.BackColor = Color.FromArgb(210, 225, 246);
            btnTrangChu.FlatAppearance.BorderColor = Color.FromArgb(210, 225, 246);
            btnTrangChu.FlatAppearance.BorderSize = 0;
            btnTrangChu.FlatAppearance.MouseDownBackColor = Color.FromArgb(193, 214, 242);
            btnTrangChu.FlatAppearance.MouseOverBackColor = Color.FromArgb(193, 214, 242);
            btnTrangChu.FlatStyle = FlatStyle.Flat;
            btnTrangChu.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnTrangChu.ForeColor = Color.FromArgb(30, 74, 130);
            btnTrangChu.ImageAlign = ContentAlignment.MiddleLeft;
            btnTrangChu.Kieu = KieuNut.MenuCha;
            btnTrangChu.Location = new Point(3, 3);
            btnTrangChu.Name = "btnTrangChu";
            btnTrangChu.Padding = new Padding(8, 0, 0, 0);
            btnTrangChu.Size = new Size(152, 38);
            btnTrangChu.TabIndex = 4;
            btnTrangChu.Text = "🏠Trang chủ";
            btnTrangChu.TextAlign = ContentAlignment.MiddleLeft;
            btnTrangChu.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnTrangChu.UseVisualStyleBackColor = false;
            btnTrangChu.Click += frmMain_Load;
            // 
            // panelCRUD
            // 
            panelCRUD.Controls.Add(btnDanhMucKhac);
            panelCRUD.Controls.Add(btnPhongHoc);
            panelCRUD.Controls.Add(btnMonHoc);
            panelCRUD.Controls.Add(btnGiaoVien);
            panelCRUD.Controls.Add(btnHocVien);
            panelCRUD.Controls.Add(btnLopHoc);
            panelCRUD.Controls.Add(btnDangKyLop);
            panelCRUD.Controls.Add(btnNhapDuLieu);
            panelCRUD.Controls.Add(btnQuanLyMenu);
            panelCRUD.Location = new Point(3, 47);
            panelCRUD.Name = "panelCRUD";
            panelCRUD.Size = new Size(271, 37);
            panelCRUD.TabIndex = 1;
            panelCRUD.Paint += panelCRUD_Paint;
            // 
            // btnDanhMucKhac
            // 
            btnDanhMucKhac.BackColor = Color.FromArgb(232, 240, 251);
            btnDanhMucKhac.FlatAppearance.BorderColor = Color.FromArgb(232, 240, 251);
            btnDanhMucKhac.FlatAppearance.BorderSize = 0;
            btnDanhMucKhac.FlatAppearance.MouseDownBackColor = Color.FromArgb(193, 214, 242);
            btnDanhMucKhac.FlatAppearance.MouseOverBackColor = Color.FromArgb(193, 214, 242);
            btnDanhMucKhac.FlatStyle = FlatStyle.Flat;
            btnDanhMucKhac.Font = new Font("Segoe UI", 9F);
            btnDanhMucKhac.ForeColor = Color.FromArgb(33, 37, 41);
            btnDanhMucKhac.Kieu = KieuNut.MenuCon;
            btnDanhMucKhac.Location = new Point(52, 178);
            btnDanhMucKhac.Name = "btnDanhMucKhac";
            btnDanhMucKhac.Padding = new Padding(48, 0, 0, 0);
            btnDanhMucKhac.Size = new Size(120, 29);
            btnDanhMucKhac.TabIndex = 5;
            btnDanhMucKhac.Text = "Danh mục khác";
            btnDanhMucKhac.TextAlign = ContentAlignment.MiddleLeft;
            btnDanhMucKhac.UseVisualStyleBackColor = false;
            // 
            // btnPhongHoc
            // 
            btnPhongHoc.BackColor = Color.FromArgb(232, 240, 251);
            btnPhongHoc.FlatAppearance.BorderColor = Color.FromArgb(232, 240, 251);
            btnPhongHoc.FlatAppearance.BorderSize = 0;
            btnPhongHoc.FlatAppearance.MouseDownBackColor = Color.FromArgb(193, 214, 242);
            btnPhongHoc.FlatAppearance.MouseOverBackColor = Color.FromArgb(193, 214, 242);
            btnPhongHoc.FlatStyle = FlatStyle.Flat;
            btnPhongHoc.Font = new Font("Segoe UI", 9F);
            btnPhongHoc.ForeColor = Color.FromArgb(33, 37, 41);
            btnPhongHoc.Kieu = KieuNut.MenuCon;
            btnPhongHoc.Location = new Point(52, 283);
            btnPhongHoc.Name = "btnPhongHoc";
            btnPhongHoc.Padding = new Padding(48, 0, 0, 0);
            btnPhongHoc.Size = new Size(120, 29);
            btnPhongHoc.TabIndex = 8;
            btnPhongHoc.Text = "Phòng học";
            btnPhongHoc.TextAlign = ContentAlignment.MiddleLeft;
            btnPhongHoc.UseVisualStyleBackColor = false;
            btnPhongHoc.Visible = false;
            btnPhongHoc.Click += btnPhongHoc_Click;
            // 
            // btnMonHoc
            // 
            btnMonHoc.BackColor = Color.FromArgb(232, 240, 251);
            btnMonHoc.FlatAppearance.BorderColor = Color.FromArgb(232, 240, 251);
            btnMonHoc.FlatAppearance.BorderSize = 0;
            btnMonHoc.FlatAppearance.MouseDownBackColor = Color.FromArgb(193, 214, 242);
            btnMonHoc.FlatAppearance.MouseOverBackColor = Color.FromArgb(193, 214, 242);
            btnMonHoc.FlatStyle = FlatStyle.Flat;
            btnMonHoc.Font = new Font("Segoe UI", 9F);
            btnMonHoc.ForeColor = Color.FromArgb(33, 37, 41);
            btnMonHoc.Kieu = KieuNut.MenuCon;
            btnMonHoc.Location = new Point(52, 143);
            btnMonHoc.Name = "btnMonHoc";
            btnMonHoc.Padding = new Padding(48, 0, 0, 0);
            btnMonHoc.Size = new Size(86, 29);
            btnMonHoc.TabIndex = 4;
            btnMonHoc.Text = "Môn học";
            btnMonHoc.TextAlign = ContentAlignment.MiddleLeft;
            btnMonHoc.UseVisualStyleBackColor = false;
            btnMonHoc.Click += btnMonHoc_Click;
            // 
            // btnGiaoVien
            // 
            btnGiaoVien.BackColor = Color.FromArgb(232, 240, 251);
            btnGiaoVien.FlatAppearance.BorderColor = Color.FromArgb(232, 240, 251);
            btnGiaoVien.FlatAppearance.BorderSize = 0;
            btnGiaoVien.FlatAppearance.MouseDownBackColor = Color.FromArgb(193, 214, 242);
            btnGiaoVien.FlatAppearance.MouseOverBackColor = Color.FromArgb(193, 214, 242);
            btnGiaoVien.FlatStyle = FlatStyle.Flat;
            btnGiaoVien.Font = new Font("Segoe UI", 9F);
            btnGiaoVien.ForeColor = Color.FromArgb(33, 37, 41);
            btnGiaoVien.Kieu = KieuNut.MenuCon;
            btnGiaoVien.Location = new Point(52, 108);
            btnGiaoVien.Name = "btnGiaoVien";
            btnGiaoVien.Padding = new Padding(48, 0, 0, 0);
            btnGiaoVien.Size = new Size(86, 29);
            btnGiaoVien.TabIndex = 3;
            btnGiaoVien.Text = "Giáo viên";
            btnGiaoVien.TextAlign = ContentAlignment.MiddleLeft;
            btnGiaoVien.UseVisualStyleBackColor = false;
            // 
            // btnHocVien
            // 
            btnHocVien.BackColor = Color.FromArgb(232, 240, 251);
            btnHocVien.FlatAppearance.BorderColor = Color.FromArgb(232, 240, 251);
            btnHocVien.FlatAppearance.BorderSize = 0;
            btnHocVien.FlatAppearance.MouseDownBackColor = Color.FromArgb(193, 214, 242);
            btnHocVien.FlatAppearance.MouseOverBackColor = Color.FromArgb(193, 214, 242);
            btnHocVien.FlatStyle = FlatStyle.Flat;
            btnHocVien.Font = new Font("Segoe UI", 9F);
            btnHocVien.ForeColor = Color.FromArgb(33, 37, 41);
            btnHocVien.Kieu = KieuNut.MenuCon;
            btnHocVien.Location = new Point(52, 73);
            btnHocVien.Name = "btnHocVien";
            btnHocVien.Padding = new Padding(48, 0, 0, 0);
            btnHocVien.Size = new Size(86, 29);
            btnHocVien.TabIndex = 2;
            btnHocVien.Text = "Học viên";
            btnHocVien.TextAlign = ContentAlignment.MiddleLeft;
            btnHocVien.UseVisualStyleBackColor = false;
            // 
            // btnLopHoc
            // 
            btnLopHoc.BackColor = Color.FromArgb(232, 240, 251);
            btnLopHoc.FlatAppearance.BorderColor = Color.FromArgb(232, 240, 251);
            btnLopHoc.FlatAppearance.BorderSize = 0;
            btnLopHoc.FlatAppearance.MouseDownBackColor = Color.FromArgb(193, 214, 242);
            btnLopHoc.FlatAppearance.MouseOverBackColor = Color.FromArgb(193, 214, 242);
            btnLopHoc.FlatStyle = FlatStyle.Flat;
            btnLopHoc.Font = new Font("Segoe UI", 9F);
            btnLopHoc.ForeColor = Color.FromArgb(33, 37, 41);
            btnLopHoc.Kieu = KieuNut.MenuCon;
            btnLopHoc.Location = new Point(52, 38);
            btnLopHoc.Name = "btnLopHoc";
            btnLopHoc.Padding = new Padding(48, 0, 0, 0);
            btnLopHoc.Size = new Size(86, 29);
            btnLopHoc.TabIndex = 1;
            btnLopHoc.Text = "Lớp học";
            btnLopHoc.TextAlign = ContentAlignment.MiddleLeft;
            btnLopHoc.UseVisualStyleBackColor = false;
            btnLopHoc.Click += btnLopHoc_Click;
            // 
            // btnDangKyLop
            // 
            btnDangKyLop.BackColor = Color.FromArgb(232, 240, 251);
            btnDangKyLop.FlatAppearance.BorderColor = Color.FromArgb(232, 240, 251);
            btnDangKyLop.FlatAppearance.BorderSize = 0;
            btnDangKyLop.FlatAppearance.MouseDownBackColor = Color.FromArgb(193, 214, 242);
            btnDangKyLop.FlatAppearance.MouseOverBackColor = Color.FromArgb(193, 214, 242);
            btnDangKyLop.FlatStyle = FlatStyle.Flat;
            btnDangKyLop.Font = new Font("Segoe UI", 9F);
            btnDangKyLop.ForeColor = Color.FromArgb(33, 37, 41);
            btnDangKyLop.Kieu = KieuNut.MenuCon;
            btnDangKyLop.Location = new Point(52, 213);
            btnDangKyLop.Name = "btnDangKyLop";
            btnDangKyLop.Padding = new Padding(48, 0, 0, 0);
            btnDangKyLop.Size = new Size(120, 29);
            btnDangKyLop.TabIndex = 6;
            btnDangKyLop.Text = "Đăng ký lớp";
            btnDangKyLop.TextAlign = ContentAlignment.MiddleLeft;
            btnDangKyLop.UseVisualStyleBackColor = false;
            btnDangKyLop.Visible = false;
            btnDangKyLop.Click += btnDangKyLop_Click;
            // 
            // btnNhapDuLieu
            // 
            btnNhapDuLieu.BackColor = Color.FromArgb(232, 240, 251);
            btnNhapDuLieu.FlatAppearance.BorderColor = Color.FromArgb(232, 240, 251);
            btnNhapDuLieu.FlatAppearance.BorderSize = 0;
            btnNhapDuLieu.FlatAppearance.MouseDownBackColor = Color.FromArgb(193, 214, 242);
            btnNhapDuLieu.FlatAppearance.MouseOverBackColor = Color.FromArgb(193, 214, 242);
            btnNhapDuLieu.FlatStyle = FlatStyle.Flat;
            btnNhapDuLieu.Font = new Font("Segoe UI", 9F);
            btnNhapDuLieu.ForeColor = Color.FromArgb(33, 37, 41);
            btnNhapDuLieu.Kieu = KieuNut.MenuCon;
            btnNhapDuLieu.Location = new Point(52, 248);
            btnNhapDuLieu.Name = "btnNhapDuLieu";
            btnNhapDuLieu.Padding = new Padding(48, 0, 0, 0);
            btnNhapDuLieu.Size = new Size(150, 29);
            btnNhapDuLieu.TabIndex = 7;
            btnNhapDuLieu.Text = "Nhập dữ liệu hàng loạt";
            btnNhapDuLieu.TextAlign = ContentAlignment.MiddleLeft;
            btnNhapDuLieu.UseVisualStyleBackColor = false;
            btnNhapDuLieu.Visible = false;
            btnNhapDuLieu.Click += btnNhapDuLieu_Click;
            // 
            // btnQuanLyMenu
            // 
            btnQuanLyMenu.BackColor = Color.FromArgb(210, 225, 246);
            btnQuanLyMenu.FlatAppearance.BorderColor = Color.FromArgb(210, 225, 246);
            btnQuanLyMenu.FlatAppearance.BorderSize = 0;
            btnQuanLyMenu.FlatAppearance.MouseDownBackColor = Color.FromArgb(193, 214, 242);
            btnQuanLyMenu.FlatAppearance.MouseOverBackColor = Color.FromArgb(193, 214, 242);
            btnQuanLyMenu.FlatStyle = FlatStyle.Flat;
            btnQuanLyMenu.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnQuanLyMenu.ForeColor = Color.FromArgb(30, 74, 130);
            btnQuanLyMenu.ImageAlign = ContentAlignment.MiddleLeft;
            btnQuanLyMenu.Kieu = KieuNut.MenuCha;
            btnQuanLyMenu.Location = new Point(9, 3);
            btnQuanLyMenu.Name = "btnQuanLyMenu";
            btnQuanLyMenu.Padding = new Padding(8, 0, 0, 0);
            btnQuanLyMenu.Size = new Size(107, 29);
            btnQuanLyMenu.TabIndex = 0;
            btnQuanLyMenu.Text = "📁Quản lý";
            btnQuanLyMenu.TextAlign = ContentAlignment.MiddleLeft;
            btnQuanLyMenu.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnQuanLyMenu.UseVisualStyleBackColor = false;
            btnQuanLyMenu.Click += btnQuanLyMenu_Click;
            // 
            // panelTraCuu
            // 
            panelTraCuu.Controls.Add(btnTimGiaoVien);
            panelTraCuu.Controls.Add(btnTimLop);
            panelTraCuu.Controls.Add(btnThoiKhoaBieu);
            panelTraCuu.Controls.Add(btnTraCuuMenu);
            panelTraCuu.Location = new Point(3, 90);
            panelTraCuu.Name = "panelTraCuu";
            panelTraCuu.Size = new Size(271, 40);
            panelTraCuu.TabIndex = 2;
            // 
            // btnTimGiaoVien
            // 
            btnTimGiaoVien.BackColor = Color.FromArgb(232, 240, 251);
            btnTimGiaoVien.FlatAppearance.BorderColor = Color.FromArgb(232, 240, 251);
            btnTimGiaoVien.FlatAppearance.BorderSize = 0;
            btnTimGiaoVien.FlatAppearance.MouseDownBackColor = Color.FromArgb(193, 214, 242);
            btnTimGiaoVien.FlatAppearance.MouseOverBackColor = Color.FromArgb(193, 214, 242);
            btnTimGiaoVien.FlatStyle = FlatStyle.Flat;
            btnTimGiaoVien.Font = new Font("Segoe UI", 9F);
            btnTimGiaoVien.ForeColor = Color.FromArgb(33, 37, 41);
            btnTimGiaoVien.Kieu = KieuNut.MenuCon;
            btnTimGiaoVien.Location = new Point(52, 73);
            btnTimGiaoVien.Name = "btnTimGiaoVien";
            btnTimGiaoVien.Padding = new Padding(48, 0, 0, 0);
            btnTimGiaoVien.Size = new Size(120, 29);
            btnTimGiaoVien.TabIndex = 3;
            btnTimGiaoVien.Text = "Tìm giáo viên";
            btnTimGiaoVien.TextAlign = ContentAlignment.MiddleLeft;
            btnTimGiaoVien.UseVisualStyleBackColor = false;
            btnTimGiaoVien.Click += btnTimGiaoVien_Click;
            // 
            // btnTimLop
            // 
            btnTimLop.BackColor = Color.FromArgb(232, 240, 251);
            btnTimLop.FlatAppearance.BorderColor = Color.FromArgb(232, 240, 251);
            btnTimLop.FlatAppearance.BorderSize = 0;
            btnTimLop.FlatAppearance.MouseDownBackColor = Color.FromArgb(193, 214, 242);
            btnTimLop.FlatAppearance.MouseOverBackColor = Color.FromArgb(193, 214, 242);
            btnTimLop.FlatStyle = FlatStyle.Flat;
            btnTimLop.Font = new Font("Segoe UI", 9F);
            btnTimLop.ForeColor = Color.FromArgb(33, 37, 41);
            btnTimLop.Kieu = KieuNut.MenuCon;
            btnTimLop.Location = new Point(52, 38);
            btnTimLop.Name = "btnTimLop";
            btnTimLop.Padding = new Padding(48, 0, 0, 0);
            btnTimLop.Size = new Size(86, 29);
            btnTimLop.TabIndex = 2;
            btnTimLop.Text = "Tìm lớp";
            btnTimLop.TextAlign = ContentAlignment.MiddleLeft;
            btnTimLop.UseVisualStyleBackColor = false;
            btnTimLop.Click += btnTimLop_Click;
            // 
            // btnThoiKhoaBieu
            // 
            btnThoiKhoaBieu.BackColor = Color.FromArgb(232, 240, 251);
            btnThoiKhoaBieu.FlatAppearance.BorderColor = Color.FromArgb(232, 240, 251);
            btnThoiKhoaBieu.FlatAppearance.BorderSize = 0;
            btnThoiKhoaBieu.FlatAppearance.MouseDownBackColor = Color.FromArgb(193, 214, 242);
            btnThoiKhoaBieu.FlatAppearance.MouseOverBackColor = Color.FromArgb(193, 214, 242);
            btnThoiKhoaBieu.FlatStyle = FlatStyle.Flat;
            btnThoiKhoaBieu.Font = new Font("Segoe UI", 9F);
            btnThoiKhoaBieu.ForeColor = Color.FromArgb(33, 37, 41);
            btnThoiKhoaBieu.Kieu = KieuNut.MenuCon;
            btnThoiKhoaBieu.Location = new Point(52, 108);
            btnThoiKhoaBieu.Name = "btnThoiKhoaBieu";
            btnThoiKhoaBieu.Padding = new Padding(48, 0, 0, 0);
            btnThoiKhoaBieu.Size = new Size(150, 29);
            btnThoiKhoaBieu.TabIndex = 3;
            btnThoiKhoaBieu.Text = "Thời khóa biểu";
            btnThoiKhoaBieu.TextAlign = ContentAlignment.MiddleLeft;
            btnThoiKhoaBieu.UseVisualStyleBackColor = false;
            btnThoiKhoaBieu.Visible = false;
            btnThoiKhoaBieu.Click += btnThoiKhoaBieu_Click;
            // 
            // btnTraCuuMenu
            // 
            btnTraCuuMenu.BackColor = Color.FromArgb(210, 225, 246);
            btnTraCuuMenu.FlatAppearance.BorderColor = Color.FromArgb(210, 225, 246);
            btnTraCuuMenu.FlatAppearance.BorderSize = 0;
            btnTraCuuMenu.FlatAppearance.MouseDownBackColor = Color.FromArgb(193, 214, 242);
            btnTraCuuMenu.FlatAppearance.MouseOverBackColor = Color.FromArgb(193, 214, 242);
            btnTraCuuMenu.FlatStyle = FlatStyle.Flat;
            btnTraCuuMenu.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnTraCuuMenu.ForeColor = Color.FromArgb(30, 74, 130);
            btnTraCuuMenu.ImageAlign = ContentAlignment.MiddleLeft;
            btnTraCuuMenu.Kieu = KieuNut.MenuCha;
            btnTraCuuMenu.Location = new Point(9, 3);
            btnTraCuuMenu.Name = "btnTraCuuMenu";
            btnTraCuuMenu.Padding = new Padding(8, 0, 0, 0);
            btnTraCuuMenu.Size = new Size(107, 29);
            btnTraCuuMenu.TabIndex = 1;
            btnTraCuuMenu.Text = "🔍Tra cứu";
            btnTraCuuMenu.TextAlign = ContentAlignment.MiddleLeft;
            btnTraCuuMenu.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnTraCuuMenu.UseVisualStyleBackColor = false;
            btnTraCuuMenu.Click += btnTraCuuMenu_Click;
            // 
            // panelBaoCao
            // 
            panelBaoCao.Controls.Add(btnLopTheoPhong);
            panelBaoCao.Controls.Add(btnTop3);
            panelBaoCao.Controls.Add(btnDoanhThu);
            panelBaoCao.Controls.Add(btnBaoCaoDiem);
            panelBaoCao.Controls.Add(btnBaoCaoMenu);
            panelBaoCao.Location = new Point(3, 136);
            panelBaoCao.Name = "panelBaoCao";
            panelBaoCao.Size = new Size(271, 34);
            panelBaoCao.TabIndex = 3;
            // 
            // btnLopTheoPhong
            // 
            btnLopTheoPhong.BackColor = Color.FromArgb(232, 240, 251);
            btnLopTheoPhong.FlatAppearance.BorderColor = Color.FromArgb(232, 240, 251);
            btnLopTheoPhong.FlatAppearance.BorderSize = 0;
            btnLopTheoPhong.FlatAppearance.MouseDownBackColor = Color.FromArgb(193, 214, 242);
            btnLopTheoPhong.FlatAppearance.MouseOverBackColor = Color.FromArgb(193, 214, 242);
            btnLopTheoPhong.FlatStyle = FlatStyle.Flat;
            btnLopTheoPhong.Font = new Font("Segoe UI", 9F);
            btnLopTheoPhong.ForeColor = Color.FromArgb(33, 37, 41);
            btnLopTheoPhong.Kieu = KieuNut.MenuCon;
            btnLopTheoPhong.Location = new Point(52, 141);
            btnLopTheoPhong.Name = "btnLopTheoPhong";
            btnLopTheoPhong.Padding = new Padding(48, 0, 0, 0);
            btnLopTheoPhong.Size = new Size(156, 29);
            btnLopTheoPhong.TabIndex = 6;
            btnLopTheoPhong.Text = "Lớp học theo phòng";
            btnLopTheoPhong.TextAlign = ContentAlignment.MiddleLeft;
            btnLopTheoPhong.UseVisualStyleBackColor = false;
            // 
            // btnTop3
            // 
            btnTop3.BackColor = Color.FromArgb(232, 240, 251);
            btnTop3.FlatAppearance.BorderColor = Color.FromArgb(232, 240, 251);
            btnTop3.FlatAppearance.BorderSize = 0;
            btnTop3.FlatAppearance.MouseDownBackColor = Color.FromArgb(193, 214, 242);
            btnTop3.FlatAppearance.MouseOverBackColor = Color.FromArgb(193, 214, 242);
            btnTop3.FlatStyle = FlatStyle.Flat;
            btnTop3.Font = new Font("Segoe UI", 9F);
            btnTop3.ForeColor = Color.FromArgb(33, 37, 41);
            btnTop3.Kieu = KieuNut.MenuCon;
            btnTop3.Location = new Point(52, 107);
            btnTop3.Name = "btnTop3";
            btnTop3.Padding = new Padding(48, 0, 0, 0);
            btnTop3.Size = new Size(138, 29);
            btnTop3.TabIndex = 5;
            btnTop3.Text = "Bảng xếp hạng hv";
            btnTop3.TextAlign = ContentAlignment.MiddleLeft;
            btnTop3.UseVisualStyleBackColor = false;
            // 
            // btnDoanhThu
            // 
            btnDoanhThu.BackColor = Color.FromArgb(232, 240, 251);
            btnDoanhThu.FlatAppearance.BorderColor = Color.FromArgb(232, 240, 251);
            btnDoanhThu.FlatAppearance.BorderSize = 0;
            btnDoanhThu.FlatAppearance.MouseDownBackColor = Color.FromArgb(193, 214, 242);
            btnDoanhThu.FlatAppearance.MouseOverBackColor = Color.FromArgb(193, 214, 242);
            btnDoanhThu.FlatStyle = FlatStyle.Flat;
            btnDoanhThu.Font = new Font("Segoe UI", 9F);
            btnDoanhThu.ForeColor = Color.FromArgb(33, 37, 41);
            btnDoanhThu.Kieu = KieuNut.MenuCon;
            btnDoanhThu.Location = new Point(52, 72);
            btnDoanhThu.Name = "btnDoanhThu";
            btnDoanhThu.Padding = new Padding(48, 0, 0, 0);
            btnDoanhThu.Size = new Size(138, 29);
            btnDoanhThu.TabIndex = 3;
            btnDoanhThu.Text = "Doanh thu";
            btnDoanhThu.TextAlign = ContentAlignment.MiddleLeft;
            btnDoanhThu.UseVisualStyleBackColor = false;
            // 
            // btnBaoCaoDiem
            // 
            btnBaoCaoDiem.BackColor = Color.FromArgb(232, 240, 251);
            btnBaoCaoDiem.FlatAppearance.BorderColor = Color.FromArgb(232, 240, 251);
            btnBaoCaoDiem.FlatAppearance.BorderSize = 0;
            btnBaoCaoDiem.FlatAppearance.MouseDownBackColor = Color.FromArgb(193, 214, 242);
            btnBaoCaoDiem.FlatAppearance.MouseOverBackColor = Color.FromArgb(193, 214, 242);
            btnBaoCaoDiem.FlatStyle = FlatStyle.Flat;
            btnBaoCaoDiem.Font = new Font("Segoe UI", 9F);
            btnBaoCaoDiem.ForeColor = Color.FromArgb(33, 37, 41);
            btnBaoCaoDiem.Kieu = KieuNut.MenuCon;
            btnBaoCaoDiem.Location = new Point(52, 38);
            btnBaoCaoDiem.Name = "btnBaoCaoDiem";
            btnBaoCaoDiem.Padding = new Padding(48, 0, 0, 0);
            btnBaoCaoDiem.Size = new Size(120, 29);
            btnBaoCaoDiem.TabIndex = 2;
            btnBaoCaoDiem.Text = "Báo cáo điểm";
            btnBaoCaoDiem.TextAlign = ContentAlignment.MiddleLeft;
            btnBaoCaoDiem.UseVisualStyleBackColor = false;
            // 
            // btnBaoCaoMenu
            // 
            btnBaoCaoMenu.BackColor = Color.FromArgb(210, 225, 246);
            btnBaoCaoMenu.FlatAppearance.BorderColor = Color.FromArgb(210, 225, 246);
            btnBaoCaoMenu.FlatAppearance.BorderSize = 0;
            btnBaoCaoMenu.FlatAppearance.MouseDownBackColor = Color.FromArgb(193, 214, 242);
            btnBaoCaoMenu.FlatAppearance.MouseOverBackColor = Color.FromArgb(193, 214, 242);
            btnBaoCaoMenu.FlatStyle = FlatStyle.Flat;
            btnBaoCaoMenu.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnBaoCaoMenu.ForeColor = Color.FromArgb(30, 74, 130);
            btnBaoCaoMenu.ImageAlign = ContentAlignment.MiddleLeft;
            btnBaoCaoMenu.Kieu = KieuNut.MenuCha;
            btnBaoCaoMenu.Location = new Point(9, 3);
            btnBaoCaoMenu.Name = "btnBaoCaoMenu";
            btnBaoCaoMenu.Padding = new Padding(8, 0, 0, 0);
            btnBaoCaoMenu.Size = new Size(107, 29);
            btnBaoCaoMenu.TabIndex = 1;
            btnBaoCaoMenu.Text = "📊Báo cáo";
            btnBaoCaoMenu.TextAlign = ContentAlignment.MiddleLeft;
            btnBaoCaoMenu.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnBaoCaoMenu.UseVisualStyleBackColor = false;
            btnBaoCaoMenu.Click += btnBaoCaoMenu_Click;
            // 
            // panelHeThong
            // 
            panelHeThong.Controls.Add(btnPhanQuyen);
            panelHeThong.Controls.Add(btnBackup);
            panelHeThong.Controls.Add(btnRestore);
            panelHeThong.Controls.Add(btnHeThong);
            panelHeThong.Location = new Point(3, 176);
            panelHeThong.Name = "panelHeThong";
            panelHeThong.Size = new Size(271, 140);
            panelHeThong.TabIndex = 4;
            panelHeThong.Visible = false;
            // 
            // btnPhanQuyen
            // 
            btnPhanQuyen.BackColor = Color.FromArgb(232, 240, 251);
            btnPhanQuyen.FlatAppearance.BorderColor = Color.FromArgb(232, 240, 251);
            btnPhanQuyen.FlatAppearance.BorderSize = 0;
            btnPhanQuyen.FlatAppearance.MouseDownBackColor = Color.FromArgb(193, 214, 242);
            btnPhanQuyen.FlatAppearance.MouseOverBackColor = Color.FromArgb(193, 214, 242);
            btnPhanQuyen.FlatStyle = FlatStyle.Flat;
            btnPhanQuyen.Font = new Font("Segoe UI", 9F);
            btnPhanQuyen.ForeColor = Color.FromArgb(33, 37, 41);
            btnPhanQuyen.Kieu = KieuNut.MenuCon;
            btnPhanQuyen.Location = new Point(52, 38);
            btnPhanQuyen.Name = "btnPhanQuyen";
            btnPhanQuyen.Padding = new Padding(48, 0, 0, 0);
            btnPhanQuyen.Size = new Size(120, 29);
            btnPhanQuyen.TabIndex = 2;
            btnPhanQuyen.Text = "Phân quyền";
            btnPhanQuyen.TextAlign = ContentAlignment.MiddleLeft;
            btnPhanQuyen.UseVisualStyleBackColor = false;
            btnPhanQuyen.Visible = false;
            btnPhanQuyen.Click += btnPhanQuyen_Click;
            // 
            // btnBackup
            // 
            btnBackup.BackColor = Color.FromArgb(232, 240, 251);
            btnBackup.FlatAppearance.BorderColor = Color.FromArgb(232, 240, 251);
            btnBackup.FlatAppearance.BorderSize = 0;
            btnBackup.FlatAppearance.MouseDownBackColor = Color.FromArgb(193, 214, 242);
            btnBackup.FlatAppearance.MouseOverBackColor = Color.FromArgb(193, 214, 242);
            btnBackup.FlatStyle = FlatStyle.Flat;
            btnBackup.Font = new Font("Segoe UI", 9F);
            btnBackup.ForeColor = Color.FromArgb(33, 37, 41);
            btnBackup.Kieu = KieuNut.MenuCon;
            btnBackup.Location = new Point(52, 73);
            btnBackup.Name = "btnBackup";
            btnBackup.Padding = new Padding(48, 0, 0, 0);
            btnBackup.Size = new Size(120, 29);
            btnBackup.TabIndex = 3;
            btnBackup.Text = "Sao lưu Database";
            btnBackup.TextAlign = ContentAlignment.MiddleLeft;
            btnBackup.UseVisualStyleBackColor = false;
            btnBackup.Visible = false;
            btnBackup.Click += btnBackup_Click;
            // 
            // btnRestore
            // 
            btnRestore.BackColor = Color.FromArgb(232, 240, 251);
            btnRestore.FlatAppearance.BorderColor = Color.FromArgb(232, 240, 251);
            btnRestore.FlatAppearance.BorderSize = 0;
            btnRestore.FlatAppearance.MouseDownBackColor = Color.FromArgb(193, 214, 242);
            btnRestore.FlatAppearance.MouseOverBackColor = Color.FromArgb(193, 214, 242);
            btnRestore.FlatStyle = FlatStyle.Flat;
            btnRestore.Font = new Font("Segoe UI", 9F);
            btnRestore.ForeColor = Color.FromArgb(33, 37, 41);
            btnRestore.Kieu = KieuNut.MenuCon;
            btnRestore.Location = new Point(52, 108);
            btnRestore.Name = "btnRestore";
            btnRestore.Padding = new Padding(48, 0, 0, 0);
            btnRestore.Size = new Size(120, 29);
            btnRestore.TabIndex = 4;
            btnRestore.Text = "Khôi phục Database";
            btnRestore.TextAlign = ContentAlignment.MiddleLeft;
            btnRestore.UseVisualStyleBackColor = false;
            btnRestore.Visible = false;
            btnRestore.Click += btnRestore_Click;
            // 
            // btnHeThong
            // 
            btnHeThong.BackColor = Color.FromArgb(210, 225, 246);
            btnHeThong.FlatAppearance.BorderColor = Color.FromArgb(210, 225, 246);
            btnHeThong.FlatAppearance.BorderSize = 0;
            btnHeThong.FlatAppearance.MouseDownBackColor = Color.FromArgb(193, 214, 242);
            btnHeThong.FlatAppearance.MouseOverBackColor = Color.FromArgb(193, 214, 242);
            btnHeThong.FlatStyle = FlatStyle.Flat;
            btnHeThong.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnHeThong.ForeColor = Color.FromArgb(30, 74, 130);
            btnHeThong.ImageAlign = ContentAlignment.MiddleLeft;
            btnHeThong.Kieu = KieuNut.MenuCha;
            btnHeThong.Location = new Point(9, 3);
            btnHeThong.Name = "btnHeThong";
            btnHeThong.Padding = new Padding(8, 0, 0, 0);
            btnHeThong.Size = new Size(94, 29);
            btnHeThong.TabIndex = 1;
            btnHeThong.Text = "Hệ thống";
            btnHeThong.TextAlign = ContentAlignment.MiddleLeft;
            btnHeThong.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnHeThong.UseVisualStyleBackColor = false;
            btnHeThong.Visible = false;
            btnHeThong.Click += btnHeThong_Click;
            // 
            // panelContent
            // 
            panelContent.Controls.Add(lblChao);
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(284, 0);
            panelContent.Name = "panelContent";
            panelContent.Size = new Size(722, 553);
            panelContent.TabIndex = 2;
            // 
            // lblChao
            // 
            lblChao.AutoSize = true;
            lblChao.Font = new Font("Segoe UI", 14F);
            lblChao.ForeColor = Color.FromArgb(64, 64, 64);
            lblChao.Location = new Point(42, 23);
            lblChao.Name = "lblChao";
            lblChao.Size = new Size(69, 32);
            lblChao.TabIndex = 0;
            lblChao.Text = "Chào";
            lblChao.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelThoat
            // 
            panelThoat.Controls.Add(btnXuatExcel);
            panelThoat.Controls.Add(btnDangXuat);
            panelThoat.Controls.Add(btnDoiMatKhau);
            panelThoat.Controls.Add(btnThoat);
            panelThoat.Dock = DockStyle.Bottom;
            panelThoat.Location = new Point(0, 354);
            panelThoat.Name = "panelThoat";
            panelThoat.Size = new Size(284, 199);
            panelThoat.TabIndex = 5;
            // 
            // btnXuatExcel
            // 
            btnXuatExcel.BackColor = Color.FromArgb(232, 240, 251);
            btnXuatExcel.FlatAppearance.BorderColor = Color.FromArgb(232, 240, 251);
            btnXuatExcel.FlatAppearance.BorderSize = 0;
            btnXuatExcel.FlatAppearance.MouseDownBackColor = Color.FromArgb(193, 214, 242);
            btnXuatExcel.FlatAppearance.MouseOverBackColor = Color.FromArgb(193, 214, 242);
            btnXuatExcel.FlatStyle = FlatStyle.Flat;
            btnXuatExcel.Font = new Font("Segoe UI", 9F);
            btnXuatExcel.ForeColor = Color.FromArgb(41, 98, 168);
            btnXuatExcel.Kieu = KieuNut.MenuTienIch;
            btnXuatExcel.Location = new Point(55, 6);
            btnXuatExcel.Name = "btnXuatExcel";
            btnXuatExcel.Size = new Size(157, 38);
            btnXuatExcel.TabIndex = 1;
            btnXuatExcel.Text = "Xuất File &Excel";
            btnXuatExcel.UseVisualStyleBackColor = false;
            btnXuatExcel.Click += btnXuatExcel_Click;
            // 
            // btnDangXuat
            // 
            btnDangXuat.BackColor = Color.FromArgb(232, 240, 251);
            btnDangXuat.FlatAppearance.BorderColor = Color.FromArgb(232, 240, 251);
            btnDangXuat.FlatAppearance.BorderSize = 0;
            btnDangXuat.FlatAppearance.MouseDownBackColor = Color.FromArgb(193, 214, 242);
            btnDangXuat.FlatAppearance.MouseOverBackColor = Color.FromArgb(193, 214, 242);
            btnDangXuat.FlatStyle = FlatStyle.Flat;
            btnDangXuat.Font = new Font("Segoe UI", 9F);
            btnDangXuat.ForeColor = Color.FromArgb(41, 98, 168);
            btnDangXuat.Kieu = KieuNut.MenuTienIch;
            btnDangXuat.Location = new Point(55, 94);
            btnDangXuat.Name = "btnDangXuat";
            btnDangXuat.Size = new Size(157, 38);
            btnDangXuat.TabIndex = 3;
            btnDangXuat.Text = "Đăng xuất";
            btnDangXuat.UseVisualStyleBackColor = false;
            btnDangXuat.Click += btnDangXuat_Click;
            // 
            // btnDoiMatKhau
            // 
            btnDoiMatKhau.BackColor = Color.FromArgb(232, 240, 251);
            btnDoiMatKhau.FlatAppearance.BorderColor = Color.FromArgb(232, 240, 251);
            btnDoiMatKhau.FlatAppearance.BorderSize = 0;
            btnDoiMatKhau.FlatAppearance.MouseDownBackColor = Color.FromArgb(193, 214, 242);
            btnDoiMatKhau.FlatAppearance.MouseOverBackColor = Color.FromArgb(193, 214, 242);
            btnDoiMatKhau.FlatStyle = FlatStyle.Flat;
            btnDoiMatKhau.Font = new Font("Segoe UI", 9F);
            btnDoiMatKhau.ForeColor = Color.FromArgb(41, 98, 168);
            btnDoiMatKhau.Kieu = KieuNut.MenuTienIch;
            btnDoiMatKhau.Location = new Point(55, 50);
            btnDoiMatKhau.Name = "btnDoiMatKhau";
            btnDoiMatKhau.Size = new Size(157, 38);
            btnDoiMatKhau.TabIndex = 2;
            btnDoiMatKhau.Text = "Đổi mật khẩu";
            btnDoiMatKhau.UseVisualStyleBackColor = false;
            btnDoiMatKhau.Click += btnDoiMatKhau_Click;
            // 
            // btnThoat
            // 
            btnThoat.BackColor = Color.FromArgb(232, 240, 251);
            btnThoat.FlatAppearance.BorderColor = Color.FromArgb(232, 240, 251);
            btnThoat.FlatAppearance.BorderSize = 0;
            btnThoat.FlatAppearance.MouseDownBackColor = Color.FromArgb(250, 225, 222);
            btnThoat.FlatAppearance.MouseOverBackColor = Color.FromArgb(250, 225, 222);
            btnThoat.FlatStyle = FlatStyle.Flat;
            btnThoat.Font = new Font("Segoe UI", 9F);
            btnThoat.ForeColor = Color.FromArgb(192, 57, 43);
            btnThoat.Kieu = KieuNut.MenuThoat;
            btnThoat.Location = new Point(55, 139);
            btnThoat.Name = "btnThoat";
            btnThoat.Size = new Size(156, 32);
            btnThoat.TabIndex = 0;
            btnThoat.Text = "Thoát";
            btnThoat.UseVisualStyleBackColor = false;
            btnThoat.Click += btnThoat_Click;
            // 
            // panelSidebar
            // 
            panelSidebar.Controls.Add(flowLayoutPanel1);
            panelSidebar.Controls.Add(panelThoat);
            panelSidebar.Dock = DockStyle.Left;
            panelSidebar.Location = new Point(0, 0);
            panelSidebar.Name = "panelSidebar";
            panelSidebar.Size = new Size(284, 553);
            panelSidebar.TabIndex = 0;
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1006, 553);
            Controls.Add(panelContent);
            Controls.Add(panelSidebar);
            Name = "frmMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = " Quản lý học viên lớp Tin Học và ngoại ngữ ngắn hạn";
            Load += frmMain_Load;
            flowLayoutPanel1.ResumeLayout(false);
            panelCRUD.ResumeLayout(false);
            panelTraCuu.ResumeLayout(false);
            panelBaoCao.ResumeLayout(false);
            panelHeThong.ResumeLayout(false);
            panelContent.ResumeLayout(false);
            panelContent.PerformLayout();
            panelThoat.ResumeLayout(false);
            panelSidebar.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Panel panelContent;
        private FlowLayoutPanel flowLayoutPanel1;
        private Panel panelTraCuu;
        private Panel panelBaoCao;
        private Panel panelHeThong;
        private AppButton btnTrangChu;
        private AppButton btnTraCuuMenu;
        private AppButton btnBaoCaoMenu;
        private AppButton btnHeThong;
        private AppButton btnPhanQuyen;
        private AppButton btnBackup;
        private AppButton btnRestore;
        private AppButton btnTimGiaoVien;
        private AppButton btnTimLop;
        private AppButton btnThoiKhoaBieu;
        private AppButton btnTop3;
        private AppButton btnDoanhThu;
        private AppButton btnBaoCaoDiem;
        private AppButton btnLopTheoPhong;
        private Panel panelThoat;
        private AppButton btnThoat;
        private Panel panelSidebar;
        private Panel panelCRUD;
        private AppButton btnDanhMucKhac;
        private AppButton btnPhongHoc;
        private AppButton btnMonHoc;
        private AppButton btnGiaoVien;
        private AppButton btnHocVien;
        private AppButton btnLopHoc;
        private AppButton btnDangKyLop;
        private AppButton btnNhapDuLieu;
        private AppButton btnQuanLyMenu;
        private AppButton btnXuatExcel;
        private AppButton btnDoiMatKhau;
        private AppButton btnDangXuat;
        private Label lblChao;
    }
}
