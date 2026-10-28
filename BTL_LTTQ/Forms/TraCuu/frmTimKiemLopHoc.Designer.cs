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
            grpDieuKien = new System.Windows.Forms.GroupBox();
            lblMonHoc = new System.Windows.Forms.Label();
            cboMonHoc = new System.Windows.Forms.ComboBox();
            lblGiaoVien = new System.Windows.Forms.Label();
            cboGiaoVien = new System.Windows.Forms.ComboBox();
            lblSiSoTu = new System.Windows.Forms.Label();
            nudSiSoTu = new System.Windows.Forms.NumericUpDown();
            lblSiSoDen = new System.Windows.Forms.Label();
            nudSiSoDen = new System.Windows.Forms.NumericUpDown();
            panelNut = new System.Windows.Forms.Panel();
            btnTimKiem = new System.Windows.Forms.Button();
            btnLamMoi = new System.Windows.Forms.Button();
            grpKetQua = new System.Windows.Forms.GroupBox();
            dgvKetQua = new System.Windows.Forms.DataGridView();
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
            grpDieuKien.Dock = System.Windows.Forms.DockStyle.Top;
            grpDieuKien.Location = new System.Drawing.Point(0, 0);
            grpDieuKien.Name = "grpDieuKien";
            grpDieuKien.Padding = new System.Windows.Forms.Padding(10);
            grpDieuKien.Size = new System.Drawing.Size(984, 140);
            grpDieuKien.TabIndex = 0;
            grpDieuKien.TabStop = false;
            grpDieuKien.Text = "Điều kiện tìm kiếm";
            // 
            // lblMonHoc
            // 
            lblMonHoc.AutoSize = true;
            lblMonHoc.Location = new System.Drawing.Point(60, 40);
            lblMonHoc.Name = "lblMonHoc";
            lblMonHoc.Size = new System.Drawing.Size(55, 20);
            lblMonHoc.TabIndex = 0;
            lblMonHoc.Text = "Môn học";
            // 
            // cboMonHoc
            // 
            cboMonHoc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboMonHoc.FormattingEnabled = true;
            cboMonHoc.Location = new System.Drawing.Point(180, 36);
            cboMonHoc.Name = "cboMonHoc";
            cboMonHoc.Size = new System.Drawing.Size(220, 28);
            cboMonHoc.TabIndex = 1;
            // 
            // lblGiaoVien
            // 
            lblGiaoVien.AutoSize = true;
            lblGiaoVien.Location = new System.Drawing.Point(60, 90);
            lblGiaoVien.Name = "lblGiaoVien";
            lblGiaoVien.Size = new System.Drawing.Size(98, 20);
            lblGiaoVien.TabIndex = 2;
            lblGiaoVien.Text = "Giáo viên dạy";
            // 
            // cboGiaoVien
            // 
            cboGiaoVien.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboGiaoVien.FormattingEnabled = true;
            cboGiaoVien.Location = new System.Drawing.Point(180, 86);
            cboGiaoVien.Name = "cboGiaoVien";
            cboGiaoVien.Size = new System.Drawing.Size(220, 28);
            cboGiaoVien.TabIndex = 3;
            // 
            // lblSiSoTu
            // 
            lblSiSoTu.AutoSize = true;
            lblSiSoTu.Location = new System.Drawing.Point(520, 40);
            lblSiSoTu.Name = "lblSiSoTu";
            lblSiSoTu.Size = new System.Drawing.Size(61, 20);
            lblSiSoTu.TabIndex = 4;
            lblSiSoTu.Text = "Sĩ số từ";
            // 
            // nudSiSoTu
            // 
            nudSiSoTu.Location = new System.Drawing.Point(640, 36);
            nudSiSoTu.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            nudSiSoTu.Name = "nudSiSoTu";
            nudSiSoTu.Size = new System.Drawing.Size(120, 27);
            nudSiSoTu.TabIndex = 5;
            // 
            // lblSiSoDen
            // 
            lblSiSoDen.AutoSize = true;
            lblSiSoDen.Location = new System.Drawing.Point(520, 90);
            lblSiSoDen.Name = "lblSiSoDen";
            lblSiSoDen.Size = new System.Drawing.Size(67, 20);
            lblSiSoDen.TabIndex = 6;
            lblSiSoDen.Text = "Sĩ số đến";
            // 
            // nudSiSoDen
            // 
            nudSiSoDen.Location = new System.Drawing.Point(640, 86);
            nudSiSoDen.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            nudSiSoDen.Name = "nudSiSoDen";
            nudSiSoDen.Size = new System.Drawing.Size(120, 27);
            nudSiSoDen.TabIndex = 7;
            nudSiSoDen.Value = new decimal(new int[] { 1000, 0, 0, 0 });
            // 
            // panelNut
            // 
            panelNut.Controls.Add(btnTimKiem);
            panelNut.Controls.Add(btnLamMoi);
            panelNut.Dock = System.Windows.Forms.DockStyle.Top;
            panelNut.Location = new System.Drawing.Point(0, 140);
            panelNut.Name = "panelNut";
            panelNut.Size = new System.Drawing.Size(984, 60);
            panelNut.TabIndex = 1;
            // 
            // btnTimKiem
            // 
            btnTimKiem.Location = new System.Drawing.Point(300, 10);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new System.Drawing.Size(120, 40);
            btnTimKiem.TabIndex = 0;
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.UseVisualStyleBackColor = true;
            btnTimKiem.Click += btnTimKiem_Click;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Location = new System.Drawing.Point(480, 10);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new System.Drawing.Size(120, 40);
            btnLamMoi.TabIndex = 1;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = true;
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // grpKetQua
            // 
            grpKetQua.Controls.Add(dgvKetQua);
            grpKetQua.Dock = System.Windows.Forms.DockStyle.Fill;
            grpKetQua.Location = new System.Drawing.Point(0, 200);
            grpKetQua.Name = "grpKetQua";
            grpKetQua.Padding = new System.Windows.Forms.Padding(10);
            grpKetQua.Size = new System.Drawing.Size(984, 361);
            grpKetQua.TabIndex = 2;
            grpKetQua.TabStop = false;
            grpKetQua.Text = "Kết quả tìm kiếm";
            // 
            // dgvKetQua
            // 
            dgvKetQua.AllowUserToAddRows = false;
            dgvKetQua.AllowUserToDeleteRows = false;
            dgvKetQua.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvKetQua.BackgroundColor = System.Drawing.Color.White;
            dgvKetQua.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvKetQua.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvKetQua.Location = new System.Drawing.Point(10, 30);
            dgvKetQua.MultiSelect = false;
            dgvKetQua.Name = "dgvKetQua";
            dgvKetQua.ReadOnly = true;
            dgvKetQua.RowHeadersVisible = false;
            dgvKetQua.RowHeadersWidth = 51;
            dgvKetQua.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvKetQua.Size = new System.Drawing.Size(964, 321);
            dgvKetQua.TabIndex = 0;
            // 
            // frmTimKiemLopHoc
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(984, 561);
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
        private System.Windows.Forms.Button btnTimKiem;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.GroupBox grpKetQua;
        private System.Windows.Forms.DataGridView dgvKetQua;
    }
}
