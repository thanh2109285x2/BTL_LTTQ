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
            btnTrangChu = new Button();
            panelCRUD = new Panel();
            btnDanhMucKhac = new Button();
            btnMonHoc = new Button();
            btnGiaoVien = new Button();
            btnHocVien = new Button();
            btnLopHoc = new Button();
            btnQuanLyMenu = new Button();
            panelTraCuu = new Panel();
            btnTimGiaoVien = new Button();
            btnTimLop = new Button();
            btnTraCuuMenu = new Button();
            panelBaoCao = new Panel();
            btnLopTheoPhong = new Button();
            btnTop3 = new Button();
            btnDoanhThu = new Button();
            btnBaoCaoDiem = new Button();
            btnBaoCaoMenu = new Button();
            panelContent = new Panel();
            panelThoat = new Panel();
            btnThoat = new Button();
            panelSidebar = new Panel();
            flowLayoutPanel1.SuspendLayout();
            panelCRUD.SuspendLayout();
            panelTraCuu.SuspendLayout();
            panelBaoCao.SuspendLayout();
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
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(284, 496);
            flowLayoutPanel1.TabIndex = 0;
            flowLayoutPanel1.WrapContents = false;
            // 
            // btnTrangChu
            // 
            btnTrangChu.ImageAlign = ContentAlignment.BottomCenter;
            btnTrangChu.Location = new Point(3, 3);
            btnTrangChu.Name = "btnTrangChu";
            btnTrangChu.Size = new Size(152, 38);
            btnTrangChu.TabIndex = 4;
            btnTrangChu.Text = "🏠Trang chủ";
            btnTrangChu.TextAlign = ContentAlignment.MiddleLeft;
            btnTrangChu.UseVisualStyleBackColor = true;
            btnTrangChu.Click += frmMain_Load;
            // 
            // panelCRUD
            // 
            panelCRUD.Controls.Add(btnDanhMucKhac);
            panelCRUD.Controls.Add(btnMonHoc);
            panelCRUD.Controls.Add(btnGiaoVien);
            panelCRUD.Controls.Add(btnHocVien);
            panelCRUD.Controls.Add(btnLopHoc);
            panelCRUD.Controls.Add(btnQuanLyMenu);
            panelCRUD.Location = new Point(3, 47);
            panelCRUD.Name = "panelCRUD";
            panelCRUD.Size = new Size(271, 231);
            panelCRUD.TabIndex = 1;
            panelCRUD.Paint += panelCRUD_Paint;
            // 
            // btnDanhMucKhac
            // 
            btnDanhMucKhac.Location = new Point(52, 178);
            btnDanhMucKhac.Name = "btnDanhMucKhac";
            btnDanhMucKhac.Size = new Size(120, 29);
            btnDanhMucKhac.TabIndex = 5;
            btnDanhMucKhac.Text = "Danh mục khác";
            btnDanhMucKhac.TextAlign = ContentAlignment.MiddleLeft;
            btnDanhMucKhac.UseVisualStyleBackColor = true;
            // 
            // btnMonHoc
            // 
            btnMonHoc.Location = new Point(52, 143);
            btnMonHoc.Name = "btnMonHoc";
            btnMonHoc.Size = new Size(86, 29);
            btnMonHoc.TabIndex = 4;
            btnMonHoc.Text = "Môn học";
            btnMonHoc.TextAlign = ContentAlignment.MiddleLeft;
            btnMonHoc.UseVisualStyleBackColor = true;
            // 
            // btnGiaoVien
            // 
            btnGiaoVien.Location = new Point(52, 108);
            btnGiaoVien.Name = "btnGiaoVien";
            btnGiaoVien.Size = new Size(86, 29);
            btnGiaoVien.TabIndex = 3;
            btnGiaoVien.Text = "Giáo viên";
            btnGiaoVien.TextAlign = ContentAlignment.MiddleLeft;
            btnGiaoVien.UseVisualStyleBackColor = true;
            // 
            // btnHocVien
            // 
            btnHocVien.Location = new Point(52, 73);
            btnHocVien.Name = "btnHocVien";
            btnHocVien.Size = new Size(86, 29);
            btnHocVien.TabIndex = 2;
            btnHocVien.Text = "Học viên";
            btnHocVien.TextAlign = ContentAlignment.MiddleLeft;
            btnHocVien.UseVisualStyleBackColor = true;
            // 
            // btnLopHoc
            // 
            btnLopHoc.Location = new Point(52, 38);
            btnLopHoc.Name = "btnLopHoc";
            btnLopHoc.Size = new Size(86, 29);
            btnLopHoc.TabIndex = 1;
            btnLopHoc.Text = "Lớp học";
            btnLopHoc.TextAlign = ContentAlignment.MiddleLeft;
            btnLopHoc.UseVisualStyleBackColor = true;
            btnLopHoc.Click += btnLopHoc_Click;
            // 
            // btnQuanLyMenu
            // 
            btnQuanLyMenu.Location = new Point(9, 3);
            btnQuanLyMenu.Name = "btnQuanLyMenu";
            btnQuanLyMenu.Size = new Size(95, 29);
            btnQuanLyMenu.TabIndex = 0;
            btnQuanLyMenu.Text = "📁Quản lý";
            btnQuanLyMenu.TextAlign = ContentAlignment.MiddleLeft;
            btnQuanLyMenu.UseVisualStyleBackColor = true;
            btnQuanLyMenu.Click += btnQuanLyMenu_Click;
            // 
            // panelTraCuu
            // 
            panelTraCuu.Controls.Add(btnTimGiaoVien);
            panelTraCuu.Controls.Add(btnTimLop);
            panelTraCuu.Controls.Add(btnTraCuuMenu);
            panelTraCuu.Location = new Point(3, 284);
            panelTraCuu.Name = "panelTraCuu";
            panelTraCuu.Size = new Size(271, 111);
            panelTraCuu.TabIndex = 2;
            // 
            // btnTimGiaoVien
            // 
            btnTimGiaoVien.Location = new Point(52, 73);
            btnTimGiaoVien.Name = "btnTimGiaoVien";
            btnTimGiaoVien.Size = new Size(120, 29);
            btnTimGiaoVien.TabIndex = 3;
            btnTimGiaoVien.Text = "Tìm giáo viên";
            btnTimGiaoVien.TextAlign = ContentAlignment.MiddleLeft;
            btnTimGiaoVien.UseVisualStyleBackColor = true;
            btnTimGiaoVien.Click += btnTimGiaoVien_Click;
            // 
            // btnTimLop
            // 
            btnTimLop.Location = new Point(52, 38);
            btnTimLop.Name = "btnTimLop";
            btnTimLop.Size = new Size(86, 29);
            btnTimLop.TabIndex = 2;
            btnTimLop.Text = "Tìm lớp";
            btnTimLop.TextAlign = ContentAlignment.MiddleLeft;
            btnTimLop.UseVisualStyleBackColor = true;
            btnTimLop.Click += btnTimLop_Click;
            // 
            // btnTraCuuMenu
            // 
            btnTraCuuMenu.Location = new Point(9, 3);
            btnTraCuuMenu.Name = "btnTraCuuMenu";
            btnTraCuuMenu.Size = new Size(95, 29);
            btnTraCuuMenu.TabIndex = 1;
            btnTraCuuMenu.Text = "🔍Tra cứu";
            btnTraCuuMenu.TextAlign = ContentAlignment.MiddleLeft;
            btnTraCuuMenu.UseVisualStyleBackColor = true;
            btnTraCuuMenu.Click += btnTraCuuMenu_Click;
            // 
            // panelBaoCao
            // 
            panelBaoCao.Controls.Add(btnLopTheoPhong);
            panelBaoCao.Controls.Add(btnTop3);
            panelBaoCao.Controls.Add(btnDoanhThu);
            panelBaoCao.Controls.Add(btnBaoCaoDiem);
            panelBaoCao.Controls.Add(btnBaoCaoMenu);
            panelBaoCao.Location = new Point(3, 401);
            panelBaoCao.Name = "panelBaoCao";
            panelBaoCao.Size = new Size(271, 154);
            panelBaoCao.TabIndex = 3;
            // 
            // btnLopTheoPhong
            // 
            btnLopTheoPhong.Location = new Point(52, 141);
            btnLopTheoPhong.Name = "btnLopTheoPhong";
            btnLopTheoPhong.Size = new Size(156, 29);
            btnLopTheoPhong.TabIndex = 6;
            btnLopTheoPhong.Text = "Lớp học theo phòng";
            btnLopTheoPhong.TextAlign = ContentAlignment.MiddleLeft;
            btnLopTheoPhong.UseVisualStyleBackColor = true;
            // 
            // btnTop3
            // 
            btnTop3.Location = new Point(52, 107);
            btnTop3.Name = "btnTop3";
            btnTop3.Size = new Size(138, 29);
            btnTop3.TabIndex = 5;
            btnTop3.Text = "Bảng xếp hạng hv";
            btnTop3.TextAlign = ContentAlignment.MiddleLeft;
            btnTop3.UseVisualStyleBackColor = true;
            // 
            // btnDoanhThu
            // 
            btnDoanhThu.Location = new Point(52, 72);
            btnDoanhThu.Name = "btnDoanhThu";
            btnDoanhThu.Size = new Size(138, 29);
            btnDoanhThu.TabIndex = 3;
            btnDoanhThu.Text = "Doanh thu";
            btnDoanhThu.TextAlign = ContentAlignment.MiddleLeft;
            btnDoanhThu.UseVisualStyleBackColor = true;
            // 
            // btnBaoCaoDiem
            // 
            btnBaoCaoDiem.Location = new Point(52, 38);
            btnBaoCaoDiem.Name = "btnBaoCaoDiem";
            btnBaoCaoDiem.Size = new Size(120, 29);
            btnBaoCaoDiem.TabIndex = 2;
            btnBaoCaoDiem.Text = "Báo cáo điểm";
            btnBaoCaoDiem.TextAlign = ContentAlignment.MiddleLeft;
            btnBaoCaoDiem.UseVisualStyleBackColor = true;
            // 
            // btnBaoCaoMenu
            // 
            btnBaoCaoMenu.Location = new Point(9, 3);
            btnBaoCaoMenu.Name = "btnBaoCaoMenu";
            btnBaoCaoMenu.Size = new Size(95, 29);
            btnBaoCaoMenu.TabIndex = 1;
            btnBaoCaoMenu.Text = "📊Báo cáo";
            btnBaoCaoMenu.TextAlign = ContentAlignment.MiddleLeft;
            btnBaoCaoMenu.UseVisualStyleBackColor = true;
            btnBaoCaoMenu.Click += btnBaoCaoMenu_Click;
            // 
            // panelContent
            // 
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(284, 0);
            panelContent.Name = "panelContent";
            panelContent.Size = new Size(722, 553);
            panelContent.TabIndex = 2;
            // 
            // panelThoat
            // 
            panelThoat.Controls.Add(btnThoat);
            panelThoat.Dock = DockStyle.Bottom;
            panelThoat.Location = new Point(0, 496);
            panelThoat.Name = "panelThoat";
            panelThoat.Size = new Size(284, 57);
            panelThoat.TabIndex = 5;
            // 
            // btnThoat
            // 
            btnThoat.Location = new Point(183, 13);
            btnThoat.Name = "btnThoat";
            btnThoat.Size = new Size(95, 32);
            btnThoat.TabIndex = 0;
            btnThoat.Text = "Thoát";
            btnThoat.UseVisualStyleBackColor = true;
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
            panelThoat.ResumeLayout(false);
            panelSidebar.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Panel panelContent;
        private FlowLayoutPanel flowLayoutPanel1;
        private Panel panelTraCuu;
        private Panel panelBaoCao;
        private Button btnTrangChu;
        private Button btnTraCuuMenu;
        private Button btnBaoCaoMenu;
        private Button btnTimGiaoVien;
        private Button btnTimLop;
        private Button btnTop3;
        private Button btnDoanhThu;
        private Button btnBaoCaoDiem;
        private Button btnLopTheoPhong;
        private Panel panelThoat;
        private Button btnThoat;
        private Panel panelSidebar;
        private Panel panelCRUD;
        private Button btnDanhMucKhac;
        private Button btnMonHoc;
        private Button btnGiaoVien;
        private Button btnHocVien;
        private Button btnLopHoc;
        private Button btnQuanLyMenu;
    }
}
