namespace BTL_LTTQ.Forms.QuanLy
{
    partial class frmDangKyLop
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
            panelTieuDe = new Panel();
            txtTimKiem = new TextBox();
            label2 = new Label();
            label1 = new Label();
            panelDanhSach = new Panel();
            groupBox1 = new GroupBox();
            dgvLopHoc = new DataGridView();
            panelNut = new Panel();
            btnLamMoi = new Button();
            btnXoa = new Button();
            btnThem = new Button();
            panelTieuDe.SuspendLayout();
            panelDanhSach.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLopHoc).BeginInit();
            panelNut.SuspendLayout();
            SuspendLayout();
            // 
            // panelTieuDe
            // 
            panelTieuDe.Controls.Add(txtTimKiem);
            panelTieuDe.Controls.Add(label2);
            panelTieuDe.Controls.Add(label1);
            panelTieuDe.Dock = DockStyle.Top;
            panelTieuDe.Location = new Point(0, 0);
            panelTieuDe.Name = "panelTieuDe";
            panelTieuDe.Size = new Size(1200, 64);
            panelTieuDe.TabIndex = 0;
            // 
            // txtTimKiem
            // 
            txtTimKiem.Location = new Point(520, 20);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Size = new Size(260, 27);
            txtTimKiem.TabIndex = 2;
            txtTimKiem.KeyDown += txtTimKiem_KeyDown;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(420, 24);
            label2.Name = "label2";
            label2.Size = new Size(91, 20);
            label2.TabIndex = 1;
            label2.Text = "Tìm lớp học:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label1.Location = new Point(39, 16);
            label1.Name = "label1";
            label1.Size = new Size(174, 28);
            label1.TabIndex = 0;
            label1.Text = "Đăng ký lớp học";
            // 
            // panelDanhSach
            // 
            panelDanhSach.Controls.Add(groupBox1);
            panelDanhSach.Dock = DockStyle.Fill;
            panelDanhSach.Location = new Point(0, 64);
            panelDanhSach.Name = "panelDanhSach";
            panelDanhSach.Padding = new Padding(12);
            panelDanhSach.Size = new Size(1200, 432);
            panelDanhSach.TabIndex = 1;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(dgvLopHoc);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1176, 408);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Danh sách lớp học";
            // 
            // dgvLopHoc
            // 
            dgvLopHoc.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLopHoc.Dock = DockStyle.Fill;
            dgvLopHoc.Location = new Point(3, 23);
            dgvLopHoc.Name = "dgvLopHoc";
            dgvLopHoc.RowHeadersWidth = 51;
            dgvLopHoc.Size = new Size(1170, 382);
            dgvLopHoc.TabIndex = 0;
            dgvLopHoc.CellClick += dgvLopHoc_CellClick;
            // 
            // panelNut
            // 
            panelNut.Controls.Add(btnLamMoi);
            panelNut.Controls.Add(btnXoa);
            panelNut.Controls.Add(btnThem);
            panelNut.Dock = DockStyle.Bottom;
            panelNut.Location = new Point(0, 496);
            panelNut.Name = "panelNut";
            panelNut.Size = new Size(1200, 64);
            panelNut.TabIndex = 2;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Location = new Point(360, 12);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(120, 40);
            btnLamMoi.TabIndex = 2;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = true;
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // btnXoa
            // 
            btnXoa.Location = new Point(200, 12);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(140, 40);
            btnXoa.TabIndex = 1;
            btnXoa.Text = "Hủy đăng ký";
            btnXoa.UseVisualStyleBackColor = true;
            btnXoa.Click += btnXoa_Click;
            // 
            // btnThem
            // 
            btnThem.Location = new Point(40, 12);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(140, 40);
            btnThem.TabIndex = 0;
            btnThem.Text = "Đăng ký lớp";
            btnThem.UseVisualStyleBackColor = true;
            btnThem.Click += btnThem_Click;
            // 
            // frmDangKyLop
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 560);
            Controls.Add(panelDanhSach);
            Controls.Add(panelNut);
            Controls.Add(panelTieuDe);
            Name = "frmDangKyLop";
            Text = "Đăng ký lớp học";
            Load += frmDangKyLop_Load;
            panelTieuDe.ResumeLayout(false);
            panelTieuDe.PerformLayout();
            panelDanhSach.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvLopHoc).EndInit();
            panelNut.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelTieuDe;
        private Label label1;
        private Label label2;
        private TextBox txtTimKiem;
        private Panel panelDanhSach;
        private GroupBox groupBox1;
        private DataGridView dgvLopHoc;
        private Panel panelNut;
        private Button btnThem;
        private Button btnXoa;
        private Button btnLamMoi;
    }
}
