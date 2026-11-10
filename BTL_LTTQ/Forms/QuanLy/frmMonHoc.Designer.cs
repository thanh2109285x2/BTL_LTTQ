namespace BTL_LTTQ.Forms.QuanLy
{
    partial class frmMonHoc
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            txtTimKiem = new TextBox();
            groupBox2 = new GroupBox();
            dgvMonHoc = new DataGridView();
            panelDanhSach = new Panel();
            label11 = new Label();
            label1 = new Label();
            panelNut = new Panel();
            btnLamMoi = new Button();
            btnXoa = new Button();
            btnSua = new Button();
            btnThem = new Button();
            btnThu = new Button();
            panelTieuDe = new Panel();
            label3 = new Label();
            label2 = new Label();
            panelNhapLieu = new Panel();
            panelNoiDung = new Panel();
            groupBox1 = new GroupBox();
            textHocPhi = new TextBox();
            label4 = new Label();
            txtTenMon = new TextBox();
            txtMaMon = new TextBox();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMonHoc).BeginInit();
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
            groupBox2.Controls.Add(dgvMonHoc);
            groupBox2.Location = new Point(3, 47);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(1300, 179);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Danh sách môn học";
            // 
            // dgvMonHoc
            // 
            dgvMonHoc.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dgvMonHoc.DefaultCellStyle = dataGridViewCellStyle1;
            dgvMonHoc.Dock = DockStyle.Fill;
            dgvMonHoc.Location = new Point(3, 23);
            dgvMonHoc.Name = "dgvMonHoc";
            dgvMonHoc.RowHeadersWidth = 51;
            dgvMonHoc.Size = new Size(1294, 153);
            dgvMonHoc.TabIndex = 0;
            dgvMonHoc.CellClick += dgvMonHoc_CellClick;
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
            label11.Size = new Size(237, 20);
            label11.TabIndex = 2;
            label11.Text = "Tìm kiếm(Theo tên môn, mã môn):";
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
            label1.Text = "Thông tin môn học";
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
            label3.Text = "Tên môn";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(56, 31);
            label2.Name = "label2";
            label2.Size = new Size(68, 20);
            label2.TabIndex = 4;
            label2.Text = "Mã môn ";
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
            groupBox1.Controls.Add(textHocPhi);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(panelNut);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(txtTenMon);
            groupBox1.Controls.Add(txtMaMon);
            groupBox1.Location = new Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1300, 293);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Thông tin môn học";
            // 
            // textHocPhi
            // 
            textHocPhi.Location = new Point(533, 28);
            textHocPhi.Name = "textHocPhi";
            textHocPhi.Size = new Size(125, 27);
            textHocPhi.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(441, 35);
            label4.Name = "label4";
            label4.Size = new Size(61, 20);
            label4.TabIndex = 6;
            label4.Text = "Học phí";
            // 
            // txtTenMon
            // 
            txtTenMon.Location = new Point(179, 67);
            txtTenMon.Name = "txtTenMon";
            txtTenMon.Size = new Size(196, 27);
            txtTenMon.TabIndex = 1;
            // 
            // txtMaMon
            // 
            txtMaMon.Location = new Point(179, 28);
            txtMaMon.Name = "txtMaMon";
            txtMaMon.Size = new Size(196, 27);
            txtMaMon.TabIndex = 0;
            // 
            // frmMonHoc
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1317, 550);
            Controls.Add(panelDanhSach);
            Controls.Add(panelNhapLieu);
            Name = "frmMonHoc";
            Text = "Quản lý môn học";
            Load += frmMonHoc_Load;
            groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvMonHoc).EndInit();
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

        #endregion

        private TextBox txtTimKiem;
        private GroupBox groupBox2;
        private DataGridView dgvMonHoc;
        private Panel panelDanhSach;
        private Label label11;
        private Label label1;
        private Panel panelNut;
        private Button btnLamMoi;
        private Button btnXoa;
        private Button btnSua;
        private Button btnThem;
        private Button btnThu;
        private Panel panelTieuDe;
        private Label label3;
        private Label label2;
        private Panel panelNhapLieu;
        private Panel panelNoiDung;
        private GroupBox groupBox1;
        private TextBox txtTenMon;
        private TextBox txtMaMon;
        private TextBox textHocPhi;
        private Label label4;
    }
}