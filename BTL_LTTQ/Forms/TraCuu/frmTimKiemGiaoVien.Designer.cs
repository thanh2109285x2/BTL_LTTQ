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
            grpDieuKien = new System.Windows.Forms.GroupBox();
            lblTenGV = new System.Windows.Forms.Label();
            txtTenGV = new System.Windows.Forms.TextBox();
            lblTrinhDo = new System.Windows.Forms.Label();
            cboTrinhDo = new System.Windows.Forms.ComboBox();
            lblChuyenMon = new System.Windows.Forms.Label();
            cboChuyenMon = new System.Windows.Forms.ComboBox();
            lblMonDay = new System.Windows.Forms.Label();
            cboMonDay = new System.Windows.Forms.ComboBox();
            panelNut = new System.Windows.Forms.Panel();
            btnTimKiem = new System.Windows.Forms.Button();
            btnLamMoi = new System.Windows.Forms.Button();
            grpKetQua = new System.Windows.Forms.GroupBox();
            dgvKetQua = new System.Windows.Forms.DataGridView();
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
            grpDieuKien.Dock = System.Windows.Forms.DockStyle.Top;
            grpDieuKien.Location = new System.Drawing.Point(0, 0);
            grpDieuKien.Name = "grpDieuKien";
            grpDieuKien.Padding = new System.Windows.Forms.Padding(10);
            grpDieuKien.Size = new System.Drawing.Size(984, 140);
            grpDieuKien.TabIndex = 0;
            grpDieuKien.TabStop = false;
            grpDieuKien.Text = "Điều kiện tìm kiếm";
            // 
            // lblTenGV
            // 
            lblTenGV.AutoSize = true;
            lblTenGV.Location = new System.Drawing.Point(60, 40);
            lblTenGV.Name = "lblTenGV";
            lblTenGV.Size = new System.Drawing.Size(95, 20);
            lblTenGV.TabIndex = 0;
            lblTenGV.Text = "Tên giáo viên";
            // 
            // txtTenGV
            // 
            txtTenGV.Location = new System.Drawing.Point(180, 36);
            txtTenGV.Name = "txtTenGV";
            txtTenGV.Size = new System.Drawing.Size(220, 27);
            txtTenGV.TabIndex = 1;
            txtTenGV.KeyDown += txtTenGV_KeyDown;
            // 
            // lblTrinhDo
            // 
            lblTrinhDo.AutoSize = true;
            lblTrinhDo.Location = new System.Drawing.Point(60, 90);
            lblTrinhDo.Name = "lblTrinhDo";
            lblTrinhDo.Size = new System.Drawing.Size(60, 20);
            lblTrinhDo.TabIndex = 2;
            lblTrinhDo.Text = "Trình độ";
            // 
            // cboTrinhDo
            // 
            cboTrinhDo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboTrinhDo.FormattingEnabled = true;
            cboTrinhDo.Location = new System.Drawing.Point(180, 86);
            cboTrinhDo.Name = "cboTrinhDo";
            cboTrinhDo.Size = new System.Drawing.Size(220, 28);
            cboTrinhDo.TabIndex = 3;
            // 
            // lblChuyenMon
            // 
            lblChuyenMon.AutoSize = true;
            lblChuyenMon.Location = new System.Drawing.Point(520, 40);
            lblChuyenMon.Name = "lblChuyenMon";
            lblChuyenMon.Size = new System.Drawing.Size(83, 20);
            lblChuyenMon.TabIndex = 4;
            lblChuyenMon.Text = "Chuyên môn";
            // 
            // cboChuyenMon
            // 
            cboChuyenMon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboChuyenMon.FormattingEnabled = true;
            cboChuyenMon.Location = new System.Drawing.Point(640, 36);
            cboChuyenMon.Name = "cboChuyenMon";
            cboChuyenMon.Size = new System.Drawing.Size(220, 28);
            cboChuyenMon.TabIndex = 5;
            // 
            // lblMonDay
            // 
            lblMonDay.AutoSize = true;
            lblMonDay.Location = new System.Drawing.Point(520, 90);
            lblMonDay.Name = "lblMonDay";
            lblMonDay.Size = new System.Drawing.Size(61, 20);
            lblMonDay.TabIndex = 6;
            lblMonDay.Text = "Môn dạy";
            // 
            // cboMonDay
            // 
            cboMonDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboMonDay.FormattingEnabled = true;
            cboMonDay.Location = new System.Drawing.Point(640, 86);
            cboMonDay.Name = "cboMonDay";
            cboMonDay.Size = new System.Drawing.Size(220, 28);
            cboMonDay.TabIndex = 7;
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
            // frmTimKiemGiaoVien
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(984, 561);
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
        private System.Windows.Forms.Button btnTimKiem;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.GroupBox grpKetQua;
        private System.Windows.Forms.DataGridView dgvKetQua;
    }
}