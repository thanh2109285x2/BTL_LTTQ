namespace BTL_LTTQ.Forms.QuanLy
{
    partial class frmLopHoc
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panelNhapLieu = new Panel();
            panelNoiDung = new Panel();
            groupBox1 = new GroupBox();
            panelNut = new Panel();
            btnLamMoi = new Button();
            btnXoa = new Button();
            btnSua = new Button();
            btnThem = new Button();
            cboThoiGian = new ComboBox();
            cboCaHoc = new ComboBox();
            dtpNgayKT = new DateTimePicker();
            dtpNgayBD = new DateTimePicker();
            label10 = new Label();
            label9 = new Label();
            label8 = new Label();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            cboPhongHoc = new ComboBox();
            cboGiaoVien = new ComboBox();
            cboMonHoc = new ComboBox();
            txtTenLop = new TextBox();
            txtMaLop = new TextBox();
            panelTieuDe = new Panel();
            btnThu = new Button();
            label1 = new Label();
            panelDanhSach = new Panel();
            txtTimKiem = new TextBox();
            label11 = new Label();
            groupBox2 = new GroupBox();
            dgvLopHoc = new DataGridView();
            panelNhapLieu.SuspendLayout();
            panelNoiDung.SuspendLayout();
            groupBox1.SuspendLayout();
            panelNut.SuspendLayout();
            panelTieuDe.SuspendLayout();
            panelDanhSach.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLopHoc).BeginInit();
            SuspendLayout();
            // 
            // panelNhapLieu
            // 
            panelNhapLieu.Controls.Add(panelNoiDung);
            panelNhapLieu.Controls.Add(panelTieuDe);
            panelNhapLieu.Dock = DockStyle.Top;
            panelNhapLieu.Location = new Point(0, 0);
            panelNhapLieu.Name = "panelNhapLieu";
            panelNhapLieu.Size = new Size(1332, 333);
            panelNhapLieu.TabIndex = 0;
            // 
            // panelNoiDung
            // 
            panelNoiDung.Controls.Add(groupBox1);
            panelNoiDung.Dock = DockStyle.Bottom;
            panelNoiDung.Location = new Point(0, 40);
            panelNoiDung.Name = "panelNoiDung";
            panelNoiDung.Size = new Size(1332, 293);
            panelNoiDung.TabIndex = 1;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(panelNut);
            groupBox1.Controls.Add(cboThoiGian);
            groupBox1.Controls.Add(cboCaHoc);
            groupBox1.Controls.Add(dtpNgayKT);
            groupBox1.Controls.Add(dtpNgayBD);
            groupBox1.Controls.Add(label10);
            groupBox1.Controls.Add(label9);
            groupBox1.Controls.Add(label8);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(cboPhongHoc);
            groupBox1.Controls.Add(cboGiaoVien);
            groupBox1.Controls.Add(cboMonHoc);
            groupBox1.Controls.Add(txtTenLop);
            groupBox1.Controls.Add(txtMaLop);
            groupBox1.Location = new Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1300, 293);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Thông tin lớp học";
            // 
            // panelNut
            // 
            panelNut.Controls.Add(btnLamMoi);
            panelNut.Controls.Add(btnXoa);
            panelNut.Controls.Add(btnSua);
            panelNut.Controls.Add(btnThem);
            panelNut.Dock = DockStyle.Bottom;
            panelNut.Location = new Point(3, 230);
            panelNut.Name = "panelNut";
            panelNut.Size = new Size(1294, 60);
            panelNut.TabIndex = 1;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Location = new Point(612, 7);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(105, 42);
            btnLamMoi.TabIndex = 3;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = true;
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // btnXoa
            // 
            btnXoa.Location = new Point(460, 7);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(105, 42);
            btnXoa.TabIndex = 2;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = true;
            btnXoa.Click += btnXoa_Click;
            // 
            // btnSua
            // 
            btnSua.Location = new Point(308, 7);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(105, 42);
            btnSua.TabIndex = 1;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = true;
            btnSua.Click += btnSua_Click;
            // 
            // btnThem
            // 
            btnThem.Location = new Point(145, 7);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(105, 42);
            btnThem.TabIndex = 0;
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = true;
            btnThem.Click += btnThem_Click;
            // 
            // cboThoiGian
            // 
            cboThoiGian.FormattingEnabled = true;
            cboThoiGian.Location = new Point(604, 138);
            cboThoiGian.Name = "cboThoiGian";
            cboThoiGian.Size = new Size(151, 28);
            cboThoiGian.TabIndex = 16;
            // 
            // cboCaHoc
            // 
            cboCaHoc.FormattingEnabled = true;
            cboCaHoc.Location = new Point(604, 103);
            cboCaHoc.Name = "cboCaHoc";
            cboCaHoc.Size = new Size(156, 28);
            cboCaHoc.TabIndex = 15;
            // 
            // dtpNgayKT
            // 
            dtpNgayKT.Location = new Point(604, 69);
            dtpNgayKT.Name = "dtpNgayKT";
            dtpNgayKT.Size = new Size(250, 27);
            dtpNgayKT.TabIndex = 14;
            // 
            // dtpNgayBD
            // 
            dtpNgayBD.Location = new Point(604, 35);
            dtpNgayBD.Name = "dtpNgayBD";
            dtpNgayBD.Size = new Size(250, 27);
            dtpNgayBD.TabIndex = 13;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(475, 142);
            label10.Name = "label10";
            label10.Size = new Size(71, 20);
            label10.TabIndex = 12;
            label10.Text = "Thời gian";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(475, 110);
            label9.Name = "label9";
            label9.Size = new Size(54, 20);
            label9.TabIndex = 11;
            label9.Text = "Ca học";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(475, 76);
            label8.Name = "label8";
            label8.Size = new Size(65, 20);
            label8.TabIndex = 10;
            label8.Text = "Ngày KT";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(475, 35);
            label7.Name = "label7";
            label7.Size = new Size(68, 20);
            label7.TabIndex = 9;
            label7.Text = "Ngày BĐ";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(56, 191);
            label6.Name = "label6";
            label6.Size = new Size(79, 20);
            label6.TabIndex = 8;
            label6.Text = "Phòng học";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(56, 151);
            label5.Name = "label5";
            label5.Size = new Size(71, 20);
            label5.TabIndex = 7;
            label5.Text = "Giáo viên";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(56, 111);
            label4.Name = "label4";
            label4.Size = new Size(67, 20);
            label4.TabIndex = 6;
            label4.Text = "Môn học";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(56, 71);
            label3.Name = "label3";
            label3.Size = new Size(58, 20);
            label3.TabIndex = 5;
            label3.Text = "Tên lớp";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(56, 31);
            label2.Name = "label2";
            label2.Size = new Size(56, 20);
            label2.TabIndex = 4;
            label2.Text = "Mã lớp";
            // 
            // cboPhongHoc
            // 
            cboPhongHoc.FormattingEnabled = true;
            cboPhongHoc.Location = new Point(176, 186);
            cboPhongHoc.Name = "cboPhongHoc";
            cboPhongHoc.Size = new Size(151, 28);
            cboPhongHoc.TabIndex = 1;
            // 
            // cboGiaoVien
            // 
            cboGiaoVien.FormattingEnabled = true;
            cboGiaoVien.Location = new Point(176, 146);
            cboGiaoVien.Name = "cboGiaoVien";
            cboGiaoVien.Size = new Size(151, 28);
            cboGiaoVien.TabIndex = 3;
            // 
            // cboMonHoc
            // 
            cboMonHoc.FormattingEnabled = true;
            cboMonHoc.Location = new Point(179, 106);
            cboMonHoc.Name = "cboMonHoc";
            cboMonHoc.Size = new Size(151, 28);
            cboMonHoc.TabIndex = 2;
            // 
            // txtTenLop
            // 
            txtTenLop.Location = new Point(179, 67);
            txtTenLop.Name = "txtTenLop";
            txtTenLop.Size = new Size(125, 27);
            txtTenLop.TabIndex = 1;
            // 
            // txtMaLop
            // 
            txtMaLop.Location = new Point(179, 28);
            txtMaLop.Name = "txtMaLop";
            txtMaLop.Size = new Size(125, 27);
            txtMaLop.TabIndex = 0;
            // 
            // panelTieuDe
            // 
            panelTieuDe.Controls.Add(btnThu);
            panelTieuDe.Controls.Add(label1);
            panelTieuDe.Dock = DockStyle.Top;
            panelTieuDe.Location = new Point(0, 0);
            panelTieuDe.Name = "panelTieuDe";
            panelTieuDe.Size = new Size(1332, 47);
            panelTieuDe.TabIndex = 0;
            panelTieuDe.Paint += panel2_Paint;
            // 
            // btnThu
            // 
            btnThu.Location = new Point(179, 3);
            btnThu.Name = "btnThu";
            btnThu.Size = new Size(29, 29);
            btnThu.TabIndex = 1;
            btnThu.Text = "▲";
            btnThu.UseVisualStyleBackColor = true;
            btnThu.Click += btnThu_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(46, 10);
            label1.Margin = new Padding(3);
            label1.Name = "label1";
            label1.Size = new Size(126, 20);
            label1.TabIndex = 0;
            label1.Text = "Thông tin lớp học";
            // 
            // panelDanhSach
            // 
            panelDanhSach.Controls.Add(txtTimKiem);
            panelDanhSach.Controls.Add(label11);
            panelDanhSach.Controls.Add(groupBox2);
            panelDanhSach.Dock = DockStyle.Fill;
            panelDanhSach.Location = new Point(0, 333);
            panelDanhSach.Name = "panelDanhSach";
            panelDanhSach.Size = new Size(1332, 179);
            panelDanhSach.TabIndex = 1;
            // 
            // txtTimKiem
            // 
            txtTimKiem.Location = new Point(227, 17);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Size = new Size(178, 27);
            txtTimKiem.TabIndex = 3;
            txtTimKiem.KeyDown += txtTimKiem_KeyDown;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(54, 17);
            label11.Name = "label11";
            label11.Size = new Size(167, 20);
            label11.TabIndex = 2;
            label11.Text = "Tìm kiếm(Theo tên lớp):";
            label11.Click += label11_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(dgvLopHoc);
            groupBox2.Location = new Point(3, 47);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(1300, 179);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Danh sách lớp học";
            // 
            // dgvLopHoc
            // 
            dgvLopHoc.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLopHoc.Dock = DockStyle.Fill;
            dgvLopHoc.Location = new Point(3, 23);
            dgvLopHoc.Name = "dgvLopHoc";
            dgvLopHoc.RowHeadersWidth = 51;
            dgvLopHoc.Size = new Size(1294, 153);
            dgvLopHoc.TabIndex = 0;
            dgvLopHoc.CellClick += dgvLopHoc_CellClick;
            // 
            // frmLopHoc
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1332, 512);
            Controls.Add(panelDanhSach);
            Controls.Add(panelNhapLieu);
            Name = "frmLopHoc";
            Text = "frmLopHoc";
            Load += frmLopHoc_Load;
            panelNhapLieu.ResumeLayout(false);
            panelNoiDung.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            panelNut.ResumeLayout(false);
            panelTieuDe.ResumeLayout(false);
            panelTieuDe.PerformLayout();
            panelDanhSach.ResumeLayout(false);
            panelDanhSach.PerformLayout();
            groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvLopHoc).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelNhapLieu;
        private Panel panelTieuDe;
        private Button btnThu;
        private Label label1;
        private Panel panelNoiDung;
        private GroupBox groupBox1;
        private ComboBox cboPhongHoc;
        private ComboBox cboGiaoVien;
        private ComboBox cboMonHoc;
        private TextBox txtTenLop;
        private TextBox txtMaLop;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private DateTimePicker dtpNgayKT;
        private DateTimePicker dtpNgayBD;
        private Label label10;
        private Label label9;
        private Label label8;
        private Label label7;
        private Panel panelNut;
        private Button btnThem;
        private ComboBox cboThoiGian;
        private ComboBox cboCaHoc;
        private Button btnLamMoi;
        private Button btnXoa;
        private Button btnSua;
        private GroupBox groupBox2;
        private DataGridView dgvLopHoc;
        private Panel panelDanhSach;
        private TextBox txtTimKiem;
        private Label label11;
    }
}