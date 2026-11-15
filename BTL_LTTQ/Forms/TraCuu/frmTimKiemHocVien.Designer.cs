#nullable enable
using BTL_LTTQ;

namespace BTL_LTTQ.Forms.TraCuu
{
    partial class frmTimKiemHocVien
    {
        private System.ComponentModel.IContainer? components = null;
        private TextBox txtTuKhoa = null!;
        private ComboBox cboLop = null!;
        private ComboBox cboNghe = null!;
        private ComboBox cboXepLoai = null!;
        private DataGridView dgvKetQua = null!;
        private AppButton btnTimKiem = null!;
        private AppButton btnLamMoi = null!;
        private GroupBox grpDieuKien = null!;
        private GroupBox grpKetQua = null!;
        private Panel pnlNut = null!;
        private Label lblTuKhoa = null!;
        private Label lblLop = null!;
        private Label lblNghe = null!;
        private Label lblXepLoai = null!;

        private void InitializeComponent()
        {
            txtTuKhoa = new TextBox();
            cboLop = new ComboBox();
            cboNghe = new ComboBox();
            cboXepLoai = new ComboBox();
            dgvKetQua = new DataGridView();
            btnTimKiem = new AppButton();
            btnLamMoi = new AppButton();
            grpDieuKien = new GroupBox();
            lblTuKhoa = new Label();
            lblLop = new Label();
            lblNghe = new Label();
            lblXepLoai = new Label();
            grpKetQua = new GroupBox();
            pnlNut = new Panel();
            ((System.ComponentModel.ISupportInitialize)dgvKetQua).BeginInit();
            grpDieuKien.SuspendLayout();
            grpKetQua.SuspendLayout();
            pnlNut.SuspendLayout();
            SuspendLayout();
            // 
            // txtTuKhoa
            // 
            txtTuKhoa.Location = new Point(190, 29);
            txtTuKhoa.Name = "txtTuKhoa";
            txtTuKhoa.Size = new Size(240, 27);
            txtTuKhoa.TabIndex = 1;
            txtTuKhoa.KeyDown += txtTuKhoa_KeyDown;
            // 
            // cboLop
            // 
            cboLop.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cboLop.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLop.Location = new Point(545, 29);
            cboLop.Name = "cboLop";
            cboLop.Size = new Size(327, 28);
            cboLop.TabIndex = 3;
            // 
            // cboNghe
            // 
            cboNghe.DropDownStyle = ComboBoxStyle.DropDownList;
            cboNghe.Location = new Point(190, 83);
            cboNghe.Name = "cboNghe";
            cboNghe.Size = new Size(240, 28);
            cboNghe.TabIndex = 5;
            // 
            // cboXepLoai
            // 
            cboXepLoai.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cboXepLoai.DropDownStyle = ComboBoxStyle.DropDownList;
            cboXepLoai.Location = new Point(545, 83);
            cboXepLoai.Name = "cboXepLoai";
            cboXepLoai.Size = new Size(327, 28);
            cboXepLoai.TabIndex = 7;
            // 
            // dgvKetQua
            // 
            dgvKetQua.AllowUserToAddRows = false;
            dgvKetQua.AllowUserToDeleteRows = false;
            dgvKetQua.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvKetQua.ColumnHeadersHeight = 29;
            dgvKetQua.Dock = DockStyle.Fill;
            dgvKetQua.Location = new Point(8, 28);
            dgvKetQua.Name = "dgvKetQua";
            dgvKetQua.ReadOnly = true;
            dgvKetQua.RowHeadersWidth = 51;
            dgvKetQua.Size = new Size(884, 382);
            dgvKetQua.TabIndex = 0;
            // 
            // btnTimKiem
            // 
            btnTimKiem.BackColor = Color.White;
            btnTimKiem.FlatAppearance.BorderColor = Color.FromArgb(200, 208, 220);
            btnTimKiem.FlatAppearance.MouseDownBackColor = Color.FromArgb(234, 241, 250);
            btnTimKiem.FlatAppearance.MouseOverBackColor = Color.FromArgb(234, 241, 250);
            btnTimKiem.FlatStyle = FlatStyle.Flat;
            btnTimKiem.ForeColor = Color.FromArgb(33, 37, 41);
            btnTimKiem.Location = new Point(0, 0);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(120, 36);
            btnTimKiem.TabIndex = 0;
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.UseVisualStyleBackColor = false;
            btnTimKiem.Click += btnTimKiem_Click;
            // 
            // btnLamMoi
            // 
            btnLamMoi.BackColor = Color.White;
            btnLamMoi.FlatAppearance.BorderColor = Color.FromArgb(200, 208, 220);
            btnLamMoi.FlatAppearance.MouseDownBackColor = Color.FromArgb(234, 241, 250);
            btnLamMoi.FlatAppearance.MouseOverBackColor = Color.FromArgb(234, 241, 250);
            btnLamMoi.FlatStyle = FlatStyle.Flat;
            btnLamMoi.ForeColor = Color.FromArgb(33, 37, 41);
            btnLamMoi.Location = new Point(145, 6);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(120, 36);
            btnLamMoi.TabIndex = 1;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = false;
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // grpDieuKien
            // 
            grpDieuKien.Controls.Add(lblTuKhoa);
            grpDieuKien.Controls.Add(txtTuKhoa);
            grpDieuKien.Controls.Add(lblLop);
            grpDieuKien.Controls.Add(cboLop);
            grpDieuKien.Controls.Add(lblNghe);
            grpDieuKien.Controls.Add(cboNghe);
            grpDieuKien.Controls.Add(lblXepLoai);
            grpDieuKien.Controls.Add(cboXepLoai);
            grpDieuKien.Dock = DockStyle.Top;
            grpDieuKien.Location = new Point(0, 0);
            grpDieuKien.Name = "grpDieuKien";
            grpDieuKien.Padding = new Padding(10);
            grpDieuKien.Size = new Size(900, 150);
            grpDieuKien.TabIndex = 2;
            grpDieuKien.TabStop = false;
            grpDieuKien.Text = "Điều kiện tra cứu";
            // 
            // lblTuKhoa
            // 
            lblTuKhoa.AutoSize = true;
            lblTuKhoa.Location = new Point(18, 34);
            lblTuKhoa.Name = "lblTuKhoa";
            lblTuKhoa.Size = new Size(165, 20);
            lblTuKhoa.TabIndex = 0;
            lblTuKhoa.Text = "Mã, tên hoặc điện thoại";
            // 
            // lblLop
            // 
            lblLop.AutoSize = true;
            lblLop.Location = new Point(465, 34);
            lblLop.Name = "lblLop";
            lblLop.Size = new Size(62, 20);
            lblLop.TabIndex = 2;
            lblLop.Text = "Lớp học";
            // 
            // lblNghe
            // 
            lblNghe.AutoSize = true;
            lblNghe.Location = new Point(18, 88);
            lblNghe.Name = "lblNghe";
            lblNghe.Size = new Size(95, 20);
            lblNghe.TabIndex = 4;
            lblNghe.Text = "Nghề nghiệp";
            // 
            // lblXepLoai
            // 
            lblXepLoai.AutoSize = true;
            lblXepLoai.Location = new Point(465, 88);
            lblXepLoai.Name = "lblXepLoai";
            lblXepLoai.Size = new Size(64, 20);
            lblXepLoai.TabIndex = 6;
            lblXepLoai.Text = "Xếp loại";
            // 
            // grpKetQua
            // 
            grpKetQua.Controls.Add(dgvKetQua);
            grpKetQua.Dock = DockStyle.Fill;
            grpKetQua.Location = new Point(0, 202);
            grpKetQua.Name = "grpKetQua";
            grpKetQua.Padding = new Padding(8);
            grpKetQua.Size = new Size(900, 418);
            grpKetQua.TabIndex = 0;
            grpKetQua.TabStop = false;
            grpKetQua.Text = "Kết quả tra cứu";
            // 
            // pnlNut
            // 
            pnlNut.Controls.Add(btnTimKiem);
            pnlNut.Controls.Add(btnLamMoi);
            pnlNut.Dock = DockStyle.Top;
            pnlNut.Location = new Point(0, 150);
            pnlNut.Name = "pnlNut";
            pnlNut.Padding = new Padding(12, 6, 0, 0);
            pnlNut.Size = new Size(900, 52);
            pnlNut.TabIndex = 1;
            // 
            // frmTimKiemHocVien
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 247, 251);
            ClientSize = new Size(900, 620);
            Controls.Add(grpKetQua);
            Controls.Add(pnlNut);
            Controls.Add(grpDieuKien);
            MinimumSize = new Size(750, 500);
            Name = "frmTimKiemHocVien";
            Text = "Tra cứu học viên";
            ((System.ComponentModel.ISupportInitialize)dgvKetQua).EndInit();
            grpDieuKien.ResumeLayout(false);
            grpDieuKien.PerformLayout();
            grpKetQua.ResumeLayout(false);
            pnlNut.ResumeLayout(false);
            ResumeLayout(false);
        }

        protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
    }
}
