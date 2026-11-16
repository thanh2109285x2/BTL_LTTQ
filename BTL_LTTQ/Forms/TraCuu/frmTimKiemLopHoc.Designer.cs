using BTL_LTTQ;

namespace BTL_LTTQ.Forms.TraCuu
{
    partial class frmTimKiemLopHoc
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
            grpDieuKien = new GroupBox();
            lblMonHoc = new Label();
            cboMonHoc = new ComboBox();
            lblGiaoVien = new Label();
            cboGiaoVien = new ComboBox();
            lblSiSoTu = new Label();
            nudSiSoTu = new NumericUpDown();
            lblSiSoDen = new Label();
            nudSiSoDen = new NumericUpDown();
            panelNut = new Panel();
            btnTimKiem = new AppButton();
            btnLamMoi = new AppButton();
            grpKetQua = new GroupBox();
            dgvKetQua = new DataGridView();
            grpDieuKien.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudSiSoTu).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudSiSoDen).BeginInit();
            panelNut.SuspendLayout();
            grpKetQua.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvKetQua).BeginInit();
            SuspendLayout();
            // 
            // grpDieuKien
            // 
            grpDieuKien.Controls.Add(lblMonHoc);
            grpDieuKien.Controls.Add(cboMonHoc);
            grpDieuKien.Controls.Add(lblGiaoVien);
            grpDieuKien.Controls.Add(cboGiaoVien);
            grpDieuKien.Controls.Add(lblSiSoTu);
            grpDieuKien.Controls.Add(nudSiSoTu);
            grpDieuKien.Controls.Add(lblSiSoDen);
            grpDieuKien.Controls.Add(nudSiSoDen);
            grpDieuKien.Dock = DockStyle.Top;
            grpDieuKien.Location = new Point(0, 0);
            grpDieuKien.Name = "grpDieuKien";
            grpDieuKien.Padding = new Padding(10);
            grpDieuKien.Size = new Size(984, 140);
            grpDieuKien.TabIndex = 0;
            grpDieuKien.TabStop = false;
            grpDieuKien.Text = "Điều kiện tìm kiếm";
            // 
            // lblMonHoc
            // 
            lblMonHoc.AutoSize = true;
            lblMonHoc.Location = new Point(60, 40);
            lblMonHoc.Name = "lblMonHoc";
            lblMonHoc.Size = new Size(67, 20);
            lblMonHoc.TabIndex = 0;
            lblMonHoc.Text = "Môn học";
            // 
            // cboMonHoc
            // 
            cboMonHoc.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMonHoc.FormattingEnabled = true;
            cboMonHoc.Location = new Point(180, 36);
            cboMonHoc.Name = "cboMonHoc";
            cboMonHoc.Size = new Size(220, 28);
            cboMonHoc.TabIndex = 1;
            // 
            // lblGiaoVien
            // 
            lblGiaoVien.AutoSize = true;
            lblGiaoVien.Location = new Point(60, 90);
            lblGiaoVien.Name = "lblGiaoVien";
            lblGiaoVien.Size = new Size(99, 20);
            lblGiaoVien.TabIndex = 2;
            lblGiaoVien.Text = "Giáo viên dạy";
            // 
            // cboGiaoVien
            // 
            cboGiaoVien.DropDownStyle = ComboBoxStyle.DropDownList;
            cboGiaoVien.FormattingEnabled = true;
            cboGiaoVien.Location = new Point(180, 86);
            cboGiaoVien.Name = "cboGiaoVien";
            cboGiaoVien.Size = new Size(220, 28);
            cboGiaoVien.TabIndex = 3;
            // 
            // lblSiSoTu
            // 
            lblSiSoTu.AutoSize = true;
            lblSiSoTu.Location = new Point(520, 40);
            lblSiSoTu.Name = "lblSiSoTu";
            lblSiSoTu.Size = new Size(58, 20);
            lblSiSoTu.TabIndex = 4;
            lblSiSoTu.Text = "Sĩ số từ";
            // 
            // nudSiSoTu
            // 
            nudSiSoTu.Location = new Point(640, 36);
            nudSiSoTu.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            nudSiSoTu.Name = "nudSiSoTu";
            nudSiSoTu.Size = new Size(120, 27);
            nudSiSoTu.TabIndex = 5;
            // 
            // lblSiSoDen
            // 
            lblSiSoDen.AutoSize = true;
            lblSiSoDen.Location = new Point(520, 90);
            lblSiSoDen.Name = "lblSiSoDen";
            lblSiSoDen.Size = new Size(69, 20);
            lblSiSoDen.TabIndex = 6;
            lblSiSoDen.Text = "Sĩ số đến";
            // 
            // nudSiSoDen
            // 
            nudSiSoDen.Location = new Point(640, 86);
            nudSiSoDen.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            nudSiSoDen.Name = "nudSiSoDen";
            nudSiSoDen.Size = new Size(120, 27);
            nudSiSoDen.TabIndex = 7;
            nudSiSoDen.Value = new decimal(new int[] { 1000, 0, 0, 0 });
            // 
            // panelNut
            // 
            panelNut.Controls.Add(btnTimKiem);
            panelNut.Controls.Add(btnLamMoi);
            panelNut.Dock = DockStyle.Top;
            panelNut.Location = new Point(0, 140);
            panelNut.Name = "panelNut";
            panelNut.Size = new Size(984, 60);
            panelNut.TabIndex = 1;
            // 
            // btnTimKiem
            // 
            btnTimKiem.BackColor = Color.FromArgb(232, 240, 251);
            btnTimKiem.FlatAppearance.BorderColor = Color.FromArgb(232, 240, 251);
            btnTimKiem.FlatAppearance.BorderSize = 0;
            btnTimKiem.FlatAppearance.MouseDownBackColor = Color.FromArgb(193, 214, 242);
            btnTimKiem.FlatAppearance.MouseOverBackColor = Color.FromArgb(193, 214, 242);
            btnTimKiem.FlatStyle = FlatStyle.Flat;
            btnTimKiem.Font = new Font("Segoe UI", 9F);
            btnTimKiem.ForeColor = Color.FromArgb(41, 98, 168);
            btnTimKiem.Kieu = KieuNut.MenuTienIch;
            btnTimKiem.Location = new Point(300, 10);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(120, 40);
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
            btnLamMoi.Location = new Point(480, 10);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(120, 40);
            btnLamMoi.TabIndex = 1;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = false;
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // grpKetQua
            // 
            grpKetQua.Controls.Add(dgvKetQua);
            grpKetQua.Dock = DockStyle.Fill;
            grpKetQua.Location = new Point(0, 200);
            grpKetQua.Name = "grpKetQua";
            grpKetQua.Padding = new Padding(10);
            grpKetQua.Size = new Size(984, 361);
            grpKetQua.TabIndex = 2;
            grpKetQua.TabStop = false;
            grpKetQua.Text = "Kết quả tìm kiếm";
            // 
            // dgvKetQua
            // 
            dgvKetQua.AllowUserToAddRows = false;
            dgvKetQua.AllowUserToDeleteRows = false;
            dgvKetQua.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvKetQua.BackgroundColor = Color.White;
            dgvKetQua.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvKetQua.Dock = DockStyle.Fill;
            dgvKetQua.Location = new Point(10, 30);
            dgvKetQua.MultiSelect = false;
            dgvKetQua.Name = "dgvKetQua";
            dgvKetQua.ReadOnly = true;
            dgvKetQua.RowHeadersVisible = false;
            dgvKetQua.RowHeadersWidth = 51;
            dgvKetQua.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvKetQua.Size = new Size(964, 321);
            dgvKetQua.TabIndex = 0;
            // 
            // frmTimKiemLopHoc
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 561);
            Controls.Add(grpKetQua);
            Controls.Add(panelNut);
            Controls.Add(grpDieuKien);
            Name = "frmTimKiemLopHoc";
            Text = "Tìm kiếm lớp học";
            Load += frmTimKiemLopHoc_Load;
            grpDieuKien.ResumeLayout(false);
            grpDieuKien.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudSiSoTu).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudSiSoDen).EndInit();
            panelNut.ResumeLayout(false);
            grpKetQua.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvKetQua).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox grpDieuKien;
        private System.Windows.Forms.Label lblMonHoc;
        private System.Windows.Forms.ComboBox cboMonHoc;
        private System.Windows.Forms.Label lblGiaoVien;
        private System.Windows.Forms.ComboBox cboGiaoVien;
        private System.Windows.Forms.Label lblSiSoTu;
        private System.Windows.Forms.NumericUpDown nudSiSoTu;
        private System.Windows.Forms.Label lblSiSoDen;
        private System.Windows.Forms.NumericUpDown nudSiSoDen;
        private System.Windows.Forms.Panel panelNut;
        private AppButton btnTimKiem;
        private AppButton btnLamMoi;
        private System.Windows.Forms.GroupBox grpKetQua;
        private System.Windows.Forms.DataGridView dgvKetQua;
    }
}
