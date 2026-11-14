using BTL_LTTQ;

namespace BTL_LTTQ.Forms.QuanLy
{
    partial class frmPhongHoc
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox txtTimKiem;
        private GroupBox groupBox2;
        private DataGridView dgvPhongHoc;
        private Panel panelDanhSach;
        private Label label11;
        private Label label1;
        private Panel panelNut;
        private AppButton btnLamMoi;
        private AppButton btnXoa;
        private AppButton btnSua;
        private AppButton btnThem;
        private AppButton btnThu;
        private Panel panelTieuDe;
        private Label label3;
        private Label label2;
        private Panel panelNhapLieu;
        private Panel panelNoiDung;
        private GroupBox groupBox1;
        private TextBox txtSucChua;
        private Label label4;
        private TextBox txtTenPhong;
        private TextBox txtMaPhong;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            txtTimKiem = new TextBox();
            groupBox2 = new GroupBox();
            dgvPhongHoc = new DataGridView();
            panelDanhSach = new Panel();
            label11 = new Label();
            label1 = new Label();
            panelNut = new Panel();
            btnLamMoi = new AppButton { Kieu = KieuNut.Phu };
            btnXoa = new AppButton { Kieu = KieuNut.NguyHiem };
            btnSua = new AppButton { Kieu = KieuNut.Chinh };
            btnThem = new AppButton { Kieu = KieuNut.Chinh };
            btnThu = new AppButton { Kieu = KieuNut.Phu };
            panelTieuDe = new Panel();
            label3 = new Label();
            label2 = new Label();
            panelNhapLieu = new Panel();
            panelNoiDung = new Panel();
            groupBox1 = new GroupBox();
            txtSucChua = new TextBox();
            label4 = new Label();
            txtTenPhong = new TextBox();
            txtMaPhong = new TextBox();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPhongHoc).BeginInit();
            panelDanhSach.SuspendLayout();
            panelNut.SuspendLayout();
            panelTieuDe.SuspendLayout();
            panelNhapLieu.SuspendLayout();
            panelNoiDung.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // txtTimKiem
            // 
            txtTimKiem.Location = new Point(321, 17);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Size = new Size(178, 27);
            txtTimKiem.TabIndex = 3;
            txtTimKiem.KeyDown += txtTimKiem_KeyDown;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(dgvPhongHoc);
            groupBox2.Location = new Point(3, 47);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(1300, 179);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Danh sách phòng học";
            // 
            // dgvPhongHoc
            // 
            dgvPhongHoc.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dgvPhongHoc.DefaultCellStyle = dataGridViewCellStyle1;
            dgvPhongHoc.Dock = DockStyle.Fill;
            dgvPhongHoc.Location = new Point(3, 23);
            dgvPhongHoc.Name = "dgvPhongHoc";
            dgvPhongHoc.RowHeadersWidth = 51;
            dgvPhongHoc.Size = new Size(1294, 153);
            dgvPhongHoc.TabIndex = 0;
            dgvPhongHoc.CellClick += dgvPhongHoc_CellClick;
            // 
            // panelDanhSach
            // 
            panelDanhSach.Controls.Add(txtTimKiem);
            panelDanhSach.Controls.Add(label11);
            panelDanhSach.Controls.Add(groupBox2);
            panelDanhSach.Dock = DockStyle.Fill;
            panelDanhSach.Location = new Point(0, 333);
            panelDanhSach.Name = "panelDanhSach";
            panelDanhSach.Size = new Size(1317, 217);
            panelDanhSach.TabIndex = 3;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(54, 17);
            label11.Name = "label11";
            label11.Size = new Size(300, 20);
            label11.TabIndex = 2;
            label11.Text = "Tìm kiếm (Theo tên phòng, mã phòng):";
            label11.Click += label11_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(46, 10);
            label1.Margin = new Padding(3);
            label1.Name = "label1";
            label1.Size = new Size(134, 20);
            label1.TabIndex = 0;
            label1.Text = "Thông tin phòng học";
            label1.Click += label1_Click;
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
            // panelTieuDe
            // 
            panelTieuDe.Controls.Add(btnThu);
            panelTieuDe.Controls.Add(label1);
            panelTieuDe.Dock = DockStyle.Top;
            panelTieuDe.Location = new Point(0, 0);
            panelTieuDe.Name = "panelTieuDe";
            panelTieuDe.Size = new Size(1317, 47);
            panelTieuDe.TabIndex = 0;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(56, 71);
            label3.Name = "label3";
            label3.Size = new Size(66, 20);
            label3.TabIndex = 5;
            label3.Text = "Tên phòng";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(56, 31);
            label2.Name = "label2";
            label2.Size = new Size(68, 20);
            label2.TabIndex = 4;
            label2.Text = "Mã phòng ";
            // 
            // panelNhapLieu
            // 
            panelNhapLieu.Controls.Add(panelNoiDung);
            panelNhapLieu.Controls.Add(panelTieuDe);
            panelNhapLieu.Dock = DockStyle.Top;
            panelNhapLieu.Location = new Point(0, 0);
            panelNhapLieu.Name = "panelNhapLieu";
            panelNhapLieu.Size = new Size(1317, 333);
            panelNhapLieu.TabIndex = 2;
            // 
            // panelNoiDung
            // 
            panelNoiDung.Controls.Add(groupBox1);
            panelNoiDung.Dock = DockStyle.Bottom;
            panelNoiDung.Location = new Point(0, 40);
            panelNoiDung.Name = "panelNoiDung";
            panelNoiDung.Size = new Size(1317, 293);
            panelNoiDung.TabIndex = 1;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(txtSucChua);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(panelNut);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(txtTenPhong);
            groupBox1.Controls.Add(txtMaPhong);
            groupBox1.Location = new Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1300, 293);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Thông tin phòng học";
            // 
            // txtSucChua
            // 
            txtSucChua.Location = new Point(533, 28);
            txtSucChua.Name = "txtSucChua";
            txtSucChua.Size = new Size(125, 27);
            txtSucChua.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(441, 35);
            label4.Name = "label4";
            label4.Size = new Size(61, 20);
            label4.TabIndex = 6;
            label4.Text = "Sức chứa";
            // 
            // txtTenPhong
            // 
            txtTenPhong.Location = new Point(179, 67);
            txtTenPhong.Name = "txtTenPhong";
            txtTenPhong.Size = new Size(196, 27);
            txtTenPhong.TabIndex = 1;
            // 
            // txtMaPhong
            // 
            txtMaPhong.Location = new Point(179, 28);
            txtMaPhong.Name = "txtMaPhong";
            txtMaPhong.Size = new Size(196, 27);
            txtMaPhong.TabIndex = 0;
            // 
            // frmPhongHoc
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1317, 550);
            Controls.Add(panelDanhSach);
            Controls.Add(panelNhapLieu);
            Name = "frmPhongHoc";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Quản lý phòng học";
            Load += frmPhongHoc_Load;
            groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvPhongHoc).EndInit();
            panelDanhSach.ResumeLayout(false);
            panelDanhSach.PerformLayout();
            panelNut.ResumeLayout(false);
            panelTieuDe.ResumeLayout(false);
            panelTieuDe.PerformLayout();
            panelNhapLieu.ResumeLayout(false);
            panelNoiDung.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }
    }
}
