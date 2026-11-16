using BTL_LTTQ;
using BTL_LTTQ.Common;

namespace BTL_LTTQ.Forms.Reports
{
    partial class frmReportBuilder
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
            lblNguon = new System.Windows.Forms.Label();
            cboNguon = new System.Windows.Forms.ComboBox();
            lblMau = new System.Windows.Forms.Label();
            cboMau = new System.Windows.Forms.ComboBox();
            lblTenBaoCao = new System.Windows.Forms.Label();
            txtTenBaoCao = new System.Windows.Forms.TextBox();
            lblCot = new System.Windows.Forms.Label();
            clbCot = new System.Windows.Forms.CheckedListBox();
            btnChonTatCaCot = new AppButton { Kieu = KieuNut.Phu };
            btnBoChonTatCaCot = new AppButton { Kieu = KieuNut.Phu };
            lblCotLoc = new System.Windows.Forms.Label();
            cboCotLoc = new System.Windows.Forms.ComboBox();
            lblPhepToan = new System.Windows.Forms.Label();
            cboPhepToan = new System.Windows.Forms.ComboBox();
            lblGiaTriLoc = new System.Windows.Forms.Label();
            txtGiaTriLoc = new System.Windows.Forms.TextBox();
            lblSapXep = new System.Windows.Forms.Label();
            cboSapXep = new System.Windows.Forms.ComboBox();
            chkGiamDan = new System.Windows.Forms.CheckBox();
            panelNut = new System.Windows.Forms.Panel();
            btnXemBaoCao = new AppButton { Kieu = KieuNut.Chinh };
            btnXuatExcel = new AppButton { Kieu = KieuNut.Phu };
            btnLuuBaoCao = new AppButton { Kieu = KieuNut.Phu };
            btnLamMoi = new AppButton { Kieu = KieuNut.Phu };
            lblThongKe = new System.Windows.Forms.Label();
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
            grpDieuKien.Controls.Add(lblNguon);
            grpDieuKien.Controls.Add(cboNguon);
            grpDieuKien.Controls.Add(lblMau);
            grpDieuKien.Controls.Add(cboMau);
            grpDieuKien.Controls.Add(lblTenBaoCao);
            grpDieuKien.Controls.Add(txtTenBaoCao);
            grpDieuKien.Controls.Add(lblCot);
            grpDieuKien.Controls.Add(clbCot);
            grpDieuKien.Controls.Add(btnChonTatCaCot);
            grpDieuKien.Controls.Add(btnBoChonTatCaCot);
            grpDieuKien.Controls.Add(lblCotLoc);
            grpDieuKien.Controls.Add(cboCotLoc);
            grpDieuKien.Controls.Add(lblPhepToan);
            grpDieuKien.Controls.Add(cboPhepToan);
            grpDieuKien.Controls.Add(lblGiaTriLoc);
            grpDieuKien.Controls.Add(txtGiaTriLoc);
            grpDieuKien.Controls.Add(lblSapXep);
            grpDieuKien.Controls.Add(cboSapXep);
            grpDieuKien.Controls.Add(chkGiamDan);
            grpDieuKien.Dock = System.Windows.Forms.DockStyle.Top;
            grpDieuKien.Location = new System.Drawing.Point(0, 0);
            grpDieuKien.Name = "grpDieuKien";
            grpDieuKien.Padding = new System.Windows.Forms.Padding(12);
            grpDieuKien.Size = new System.Drawing.Size(1040, 240);
            grpDieuKien.TabIndex = 0;
            grpDieuKien.TabStop = false;
            grpDieuKien.Text = "Cấu hình báo cáo động";
            // 
            // lblNguon
            // 
            lblNguon.AutoSize = true;
            lblNguon.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblNguon.Location = new System.Drawing.Point(20, 32);
            lblNguon.Name = "lblNguon";
            lblNguon.Size = new System.Drawing.Size(115, 20);
            lblNguon.TabIndex = 0;
            lblNguon.Text = "Nguồn dữ liệu:";
            // 
            // cboNguon
            // 
            cboNguon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboNguon.FormattingEnabled = true;
            cboNguon.Location = new System.Drawing.Point(145, 28);
            cboNguon.Name = "cboNguon";
            cboNguon.Size = new System.Drawing.Size(240, 28);
            cboNguon.TabIndex = 1;
            cboNguon.SelectedIndexChanged += cboNguon_SelectedIndexChanged;
            // 
            // lblMau
            // 
            lblMau.AutoSize = true;
            lblMau.Location = new System.Drawing.Point(405, 32);
            lblMau.Name = "lblMau";
            lblMau.Size = new System.Drawing.Size(100, 20);
            lblMau.TabIndex = 2;
            lblMau.Text = "Mẫu dựng sẵn:";
            // 
            // cboMau
            // 
            cboMau.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboMau.FormattingEnabled = true;
            cboMau.Location = new System.Drawing.Point(515, 28);
            cboMau.Name = "cboMau";
            cboMau.Size = new System.Drawing.Size(220, 28);
            cboMau.TabIndex = 3;
            cboMau.SelectedIndexChanged += cboMau_SelectedIndexChanged;
            // 
            // lblTenBaoCao
            // 
            lblTenBaoCao.AutoSize = true;
            lblTenBaoCao.Location = new System.Drawing.Point(750, 32);
            lblTenBaoCao.Name = "lblTenBaoCao";
            lblTenBaoCao.Size = new System.Drawing.Size(90, 20);
            lblTenBaoCao.TabIndex = 4;
            lblTenBaoCao.Text = "Tên báo cáo:";
            // 
            // txtTenBaoCao
            // 
            txtTenBaoCao.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtTenBaoCao.Location = new System.Drawing.Point(845, 28);
            txtTenBaoCao.Name = "txtTenBaoCao";
            txtTenBaoCao.Size = new System.Drawing.Size(175, 27);
            txtTenBaoCao.TabIndex = 5;
            // 
            // lblCot
            // 
            lblCot.AutoSize = true;
            lblCot.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblCot.Location = new System.Drawing.Point(20, 74);
            lblCot.Name = "lblCot";
            lblCot.Size = new System.Drawing.Size(103, 20);
            lblCot.TabIndex = 6;
            lblCot.Text = "Chọn các cột:";
            // 
            // clbCot
            // 
            clbCot.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            clbCot.CheckOnClick = true;
            clbCot.FormattingEnabled = true;
            clbCot.Location = new System.Drawing.Point(145, 68);
            clbCot.MultiColumn = true;
            clbCot.Name = "clbCot";
            clbCot.Size = new System.Drawing.Size(730, 92);
            clbCot.TabIndex = 7;
            // 
            // btnChonTatCaCot
            // 
            btnChonTatCaCot.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnChonTatCaCot.Location = new System.Drawing.Point(890, 68);
            btnChonTatCaCot.Name = "btnChonTatCaCot";
            btnChonTatCaCot.Size = new System.Drawing.Size(130, 34);
            btnChonTatCaCot.TabIndex = 8;
            btnChonTatCaCot.Text = "Chọn tất cả";
            btnChonTatCaCot.UseVisualStyleBackColor = true;
            btnChonTatCaCot.Click += btnChonTatCaCot_Click;
            // 
            // btnBoChonTatCaCot
            // 
            btnBoChonTatCaCot.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnBoChonTatCaCot.Location = new System.Drawing.Point(890, 110);
            btnBoChonTatCaCot.Name = "btnBoChonTatCaCot";
            btnBoChonTatCaCot.Size = new System.Drawing.Size(130, 34);
            btnBoChonTatCaCot.TabIndex = 9;
            btnBoChonTatCaCot.Text = "Bỏ chọn hết";
            btnBoChonTatCaCot.UseVisualStyleBackColor = true;
            btnBoChonTatCaCot.Click += btnBoChonTatCaCot_Click;
            // 
            // lblCotLoc
            // 
            lblCotLoc.AutoSize = true;
            lblCotLoc.Location = new System.Drawing.Point(20, 185);
            lblCotLoc.Name = "lblCotLoc";
            lblCotLoc.Size = new System.Drawing.Size(95, 20);
            lblCotLoc.TabIndex = 10;
            lblCotLoc.Text = "Lọc theo cột:";
            // 
            // cboCotLoc
            // 
            cboCotLoc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboCotLoc.FormattingEnabled = true;
            cboCotLoc.Location = new System.Drawing.Point(145, 181);
            cboCotLoc.Name = "cboCotLoc";
            cboCotLoc.Size = new System.Drawing.Size(170, 28);
            cboCotLoc.TabIndex = 11;
            // 
            // lblPhepToan
            // 
            lblPhepToan.AutoSize = true;
            lblPhepToan.Location = new System.Drawing.Point(325, 185);
            lblPhepToan.Name = "lblPhepToan";
            lblPhepToan.Size = new System.Drawing.Size(74, 20);
            lblPhepToan.TabIndex = 12;
            lblPhepToan.Text = "Điều kiện:";
            // 
            // cboPhepToan
            // 
            cboPhepToan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboPhepToan.FormattingEnabled = true;
            cboPhepToan.Location = new System.Drawing.Point(405, 181);
            cboPhepToan.Name = "cboPhepToan";
            cboPhepToan.Size = new System.Drawing.Size(120, 28);
            cboPhepToan.TabIndex = 13;
            // 
            // lblGiaTriLoc
            // 
            lblGiaTriLoc.AutoSize = true;
            lblGiaTriLoc.Location = new System.Drawing.Point(535, 185);
            lblGiaTriLoc.Name = "lblGiaTriLoc";
            lblGiaTriLoc.Size = new System.Drawing.Size(52, 20);
            lblGiaTriLoc.TabIndex = 14;
            lblGiaTriLoc.Text = "Giá trị:";
            // 
            // txtGiaTriLoc
            // 
            txtGiaTriLoc.Location = new System.Drawing.Point(595, 181);
            txtGiaTriLoc.Name = "txtGiaTriLoc";
            txtGiaTriLoc.Size = new System.Drawing.Size(140, 27);
            txtGiaTriLoc.TabIndex = 15;
            // 
            // lblSapXep
            // 
            lblSapXep.AutoSize = true;
            lblSapXep.Location = new System.Drawing.Point(750, 185);
            lblSapXep.Name = "lblSapXep";
            lblSapXep.Size = new System.Drawing.Size(65, 20);
            lblSapXep.TabIndex = 16;
            lblSapXep.Text = "Sắp xếp:";
            // 
            // cboSapXep
            // 
            cboSapXep.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboSapXep.FormattingEnabled = true;
            cboSapXep.Location = new System.Drawing.Point(820, 181);
            cboSapXep.Name = "cboSapXep";
            cboSapXep.Size = new System.Drawing.Size(115, 28);
            cboSapXep.TabIndex = 17;
            // 
            // chkGiamDan
            // 
            chkGiamDan.AutoSize = true;
            chkGiamDan.Location = new System.Drawing.Point(945, 183);
            chkGiamDan.Name = "chkGiamDan";
            chkGiamDan.Size = new System.Drawing.Size(95, 24);
            chkGiamDan.TabIndex = 18;
            chkGiamDan.Text = "Giảm dần";
            chkGiamDan.UseVisualStyleBackColor = true;
            // 
            // panelNut
            // 
            panelNut.Controls.Add(btnXemBaoCao);
            panelNut.Controls.Add(btnXuatExcel);
            panelNut.Controls.Add(btnLuuBaoCao);
            panelNut.Controls.Add(btnLamMoi);
            panelNut.Controls.Add(lblThongKe);
            panelNut.Dock = System.Windows.Forms.DockStyle.Top;
            panelNut.Location = new System.Drawing.Point(0, 240);
            panelNut.Name = "panelNut";
            panelNut.Size = new System.Drawing.Size(1040, 56);
            panelNut.TabIndex = 1;
            // 
            // btnXemBaoCao
            // 
            btnXemBaoCao.Location = new System.Drawing.Point(20, 10);
            btnXemBaoCao.Name = "btnXemBaoCao";
            btnXemBaoCao.Size = new System.Drawing.Size(150, 36);
            btnXemBaoCao.TabIndex = 0;
            btnXemBaoCao.Text = "🔍 Xem báo cáo";
            btnXemBaoCao.UseVisualStyleBackColor = true;
            btnXemBaoCao.Click += btnXemBaoCao_Click;
            // 
            // btnXuatExcel
            // 
            btnXuatExcel.Location = new System.Drawing.Point(180, 10);
            btnXuatExcel.Name = "btnXuatExcel";
            btnXuatExcel.Size = new System.Drawing.Size(140, 36);
            btnXuatExcel.TabIndex = 1;
            btnXuatExcel.Text = "📊 Xuất Excel";
            btnXuatExcel.UseVisualStyleBackColor = true;
            btnXuatExcel.Click += btnXuatExcel_Click;
            // 
            // btnLuuBaoCao
            // 
            btnLuuBaoCao.Location = new System.Drawing.Point(330, 10);
            btnLuuBaoCao.Name = "btnLuuBaoCao";
            btnLuuBaoCao.Size = new System.Drawing.Size(130, 36);
            btnLuuBaoCao.TabIndex = 2;
            btnLuuBaoCao.Text = "💾 Lưu cấu hình";
            btnLuuBaoCao.UseVisualStyleBackColor = true;
            btnLuuBaoCao.Click += btnLuuBaoCao_Click;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Location = new System.Drawing.Point(470, 10);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new System.Drawing.Size(110, 36);
            btnLamMoi.TabIndex = 3;
            btnLamMoi.Text = "↻ Làm mới";
            btnLamMoi.UseVisualStyleBackColor = true;
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // lblThongKe
            // 
            lblThongKe.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            lblThongKe.AutoSize = true;
            lblThongKe.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Italic);
            lblThongKe.ForeColor = System.Drawing.Color.FromArgb(108, 117, 125);
            lblThongKe.Location = new System.Drawing.Point(780, 18);
            lblThongKe.Name = "lblThongKe";
            lblThongKe.Size = new System.Drawing.Size(235, 21);
            lblThongKe.TabIndex = 4;
            lblThongKe.Text = "Sẵn sàng tạo báo cáo...";
            // 
            // grpKetQua
            // 
            grpKetQua.Controls.Add(dgvKetQua);
            grpKetQua.Dock = System.Windows.Forms.DockStyle.Fill;
            grpKetQua.Location = new System.Drawing.Point(0, 296);
            grpKetQua.Name = "grpKetQua";
            grpKetQua.Padding = new System.Windows.Forms.Padding(12);
            grpKetQua.Size = new System.Drawing.Size(1040, 384);
            grpKetQua.TabIndex = 2;
            grpKetQua.TabStop = false;
            grpKetQua.Text = "Kết quả dữ liệu báo cáo";
            // 
            // dgvKetQua
            // 
            dgvKetQua.AllowUserToAddRows = false;
            dgvKetQua.AllowUserToDeleteRows = false;
            dgvKetQua.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvKetQua.BackgroundColor = System.Drawing.Color.White;
            dgvKetQua.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvKetQua.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvKetQua.Location = new System.Drawing.Point(12, 27);
            dgvKetQua.MultiSelect = false;
            dgvKetQua.Name = "dgvKetQua";
            dgvKetQua.ReadOnly = true;
            dgvKetQua.RowHeadersVisible = false;
            dgvKetQua.RowHeadersWidth = 51;
            dgvKetQua.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvKetQua.Size = new System.Drawing.Size(1016, 345);
            dgvKetQua.TabIndex = 0;
            // 
            // frmReportBuilder
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1040, 680);
            Controls.Add(grpKetQua);
            Controls.Add(panelNut);
            Controls.Add(grpDieuKien);
            Name = "frmReportBuilder";
            Text = "Trình tạo báo cáo động";
            Load += frmReportBuilder_Load;
            grpDieuKien.ResumeLayout(false);
            grpDieuKien.PerformLayout();
            panelNut.ResumeLayout(false);
            panelNut.PerformLayout();
            grpKetQua.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvKetQua).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox grpDieuKien;
        private System.Windows.Forms.Label lblNguon;
        private System.Windows.Forms.ComboBox cboNguon;
        private System.Windows.Forms.Label lblMau;
        private System.Windows.Forms.ComboBox cboMau;
        private System.Windows.Forms.Label lblTenBaoCao;
        private System.Windows.Forms.TextBox txtTenBaoCao;
        private System.Windows.Forms.Label lblCot;
        private System.Windows.Forms.CheckedListBox clbCot;
        private AppButton btnChonTatCaCot;
        private AppButton btnBoChonTatCaCot;
        private System.Windows.Forms.Label lblCotLoc;
        private System.Windows.Forms.ComboBox cboCotLoc;
        private System.Windows.Forms.Label lblPhepToan;
        private System.Windows.Forms.ComboBox cboPhepToan;
        private System.Windows.Forms.Label lblGiaTriLoc;
        private System.Windows.Forms.TextBox txtGiaTriLoc;
        private System.Windows.Forms.Label lblSapXep;
        private System.Windows.Forms.ComboBox cboSapXep;
        private System.Windows.Forms.CheckBox chkGiamDan;
        private System.Windows.Forms.Panel panelNut;
        private AppButton btnXemBaoCao;
        private AppButton btnXuatExcel;
        private AppButton btnLuuBaoCao;
        private AppButton btnLamMoi;
        private System.Windows.Forms.Label lblThongKe;
        private System.Windows.Forms.GroupBox grpKetQua;
        private System.Windows.Forms.DataGridView dgvKetQua;
    }
}
