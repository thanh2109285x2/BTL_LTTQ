using BTL_LTTQ;

namespace BTL_LTTQ.Forms.TraCuu
{
    partial class frmTimKiemGiaoVien
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
            lblTenGV = new Label();
            txtTenGV = new TextBox();
            lblTrinhDo = new Label();
            cboTrinhDo = new ComboBox();
            lblChuyenMon = new Label();
            cboChuyenMon = new ComboBox();
            lblMonDay = new Label();
            cboMonDay = new ComboBox();
            panelNut = new Panel();
            btnTimKiem = new AppButton();
            btnLamMoi = new AppButton();
            grpKetQua = new GroupBox();
            dgvKetQua = new DataGridView();
            grpDieuKien.SuspendLayout();
            panelNut.SuspendLayout();
            grpKetQua.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvKetQua).BeginInit();
            SuspendLayout();
            // 
            // grpDieuKien
            // 
            grpDieuKien.Controls.Add(lblTenGV);
            grpDieuKien.Controls.Add(txtTenGV);
            grpDieuKien.Controls.Add(lblTrinhDo);
            grpDieuKien.Controls.Add(cboTrinhDo);
            grpDieuKien.Controls.Add(lblChuyenMon);
            grpDieuKien.Controls.Add(cboChuyenMon);
            grpDieuKien.Controls.Add(lblMonDay);
            grpDieuKien.Controls.Add(cboMonDay);
            grpDieuKien.Dock = DockStyle.Top;
            grpDieuKien.Location = new Point(0, 0);
            grpDieuKien.Name = "grpDieuKien";
            grpDieuKien.Padding = new Padding(10);
            grpDieuKien.Size = new Size(984, 140);
            grpDieuKien.TabIndex = 0;
            grpDieuKien.TabStop = false;
            grpDieuKien.Text = "Điều kiện tìm kiếm";
            // 
            // lblTenGV
            // 
            lblTenGV.AutoSize = true;
            lblTenGV.Location = new Point(60, 40);
            lblTenGV.Name = "lblTenGV";
            lblTenGV.Size = new Size(97, 20);
            lblTenGV.TabIndex = 0;
            lblTenGV.Text = "Tên giáo viên";
            // 
            // txtTenGV
            // 
            txtTenGV.Location = new Point(180, 36);
            txtTenGV.Name = "txtTenGV";
            txtTenGV.Size = new Size(220, 27);
            txtTenGV.TabIndex = 1;
            txtTenGV.KeyDown += txtTenGV_KeyDown;
            // 
            // lblTrinhDo
            // 
            lblTrinhDo.AutoSize = true;
            lblTrinhDo.Location = new Point(60, 90);
            lblTrinhDo.Name = "lblTrinhDo";
            lblTrinhDo.Size = new Size(63, 20);
            lblTrinhDo.TabIndex = 2;
            lblTrinhDo.Text = "Trình độ";
            // 
            // cboTrinhDo
            // 
            cboTrinhDo.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTrinhDo.FormattingEnabled = true;
            cboTrinhDo.Location = new Point(180, 86);
            cboTrinhDo.Name = "cboTrinhDo";
            cboTrinhDo.Size = new Size(220, 28);
            cboTrinhDo.TabIndex = 3;
            // 
            // lblChuyenMon
            // 
            lblChuyenMon.AutoSize = true;
            lblChuyenMon.Location = new Point(520, 40);
            lblChuyenMon.Name = "lblChuyenMon";
            lblChuyenMon.Size = new Size(91, 20);
            lblChuyenMon.TabIndex = 4;
            lblChuyenMon.Text = "Chuyên môn";
            // 
            // cboChuyenMon
            // 
            cboChuyenMon.DropDownStyle = ComboBoxStyle.DropDownList;
            cboChuyenMon.FormattingEnabled = true;
            cboChuyenMon.Location = new Point(640, 36);
            cboChuyenMon.Name = "cboChuyenMon";
            cboChuyenMon.Size = new Size(220, 28);
            cboChuyenMon.TabIndex = 5;
            // 
            // lblMonDay
            // 
            lblMonDay.AutoSize = true;
            lblMonDay.Location = new Point(520, 90);
            lblMonDay.Name = "lblMonDay";
            lblMonDay.Size = new Size(67, 20);
            lblMonDay.TabIndex = 6;
            lblMonDay.Text = "Môn dạy";
            // 
            // cboMonDay
            // 
            cboMonDay.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMonDay.FormattingEnabled = true;
            cboMonDay.Location = new Point(640, 86);
            cboMonDay.Name = "cboMonDay";
            cboMonDay.Size = new Size(220, 28);
            cboMonDay.TabIndex = 7;
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
            // frmTimKiemGiaoVien
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 561);
            Controls.Add(grpKetQua);
            Controls.Add(panelNut);
            Controls.Add(grpDieuKien);
            Name = "frmTimKiemGiaoVien";
            Text = "Tìm kiếm giáo viên";
            Load += frmTimKiemGiaoVien_Load;
            grpDieuKien.ResumeLayout(false);
            grpDieuKien.PerformLayout();
            panelNut.ResumeLayout(false);
            grpKetQua.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvKetQua).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox grpDieuKien;
        private System.Windows.Forms.Label lblTenGV;
        private System.Windows.Forms.TextBox txtTenGV;
        private System.Windows.Forms.Label lblTrinhDo;
        private System.Windows.Forms.ComboBox cboTrinhDo;
        private System.Windows.Forms.Label lblChuyenMon;
        private System.Windows.Forms.ComboBox cboChuyenMon;
        private System.Windows.Forms.Label lblMonDay;
        private System.Windows.Forms.ComboBox cboMonDay;
        private System.Windows.Forms.Panel panelNut;
        private AppButton btnTimKiem;
        private AppButton btnLamMoi;
        private System.Windows.Forms.GroupBox grpKetQua;
        private System.Windows.Forms.DataGridView dgvKetQua;
    }
}