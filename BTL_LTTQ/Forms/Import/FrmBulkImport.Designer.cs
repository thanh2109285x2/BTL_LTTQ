namespace BTL_LTTQ.Forms.Import
{
    partial class FrmBulkImport
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
            lblTieuDe = new Label();
            lblMoTa = new Label();
            panelCauHinh = new Panel();
            lblLoai = new Label();
            cboLoai = new ComboBox();
            lblTep = new Label();
            txtTep = new TextBox();
            btnChonTep = new Button();
            btnTaiMau = new Button();
            btnXemTruoc = new Button();
            lblGoiY = new Label();
            panelTomTat = new Panel();
            lblTongDongCaption = new Label();
            lblTongDong = new Label();
            lblHopLeCaption = new Label();
            lblHopLe = new Label();
            lblLoiCaption = new Label();
            lblLoi = new Label();
            lblTrangThai = new Label();
            lblCanhBao = new Label();
            panelDanhSach = new Panel();
            dgvDuLieu = new DataGridView();
            panelNut = new Panel();
            progressNhap = new ProgressBar();
            btnXuatBaoCao = new Button();
            btnXacNhan = new Button();
            btnDong = new Button();
            panelTieuDe.SuspendLayout();
            panelCauHinh.SuspendLayout();
            panelTomTat.SuspendLayout();
            panelDanhSach.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDuLieu).BeginInit();
            panelNut.SuspendLayout();
            SuspendLayout();
            // 
            // panelTieuDe
            // 
            panelTieuDe.Controls.Add(lblTieuDe);
            panelTieuDe.Controls.Add(lblMoTa);
            panelTieuDe.Dock = DockStyle.Top;
            panelTieuDe.Location = new Point(0, 0);
            panelTieuDe.Name = "panelTieuDe";
            panelTieuDe.Size = new Size(1100, 64);
            panelTieuDe.TabIndex = 0;
            // 
            // lblTieuDe
            // 
            lblTieuDe.AutoSize = true;
            lblTieuDe.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblTieuDe.Location = new Point(24, 10);
            lblTieuDe.Name = "lblTieuDe";
            lblTieuDe.Size = new Size(310, 35);
            lblTieuDe.TabIndex = 0;
            lblTieuDe.Text = "Nhập dữ liệu hàng loạt";
            // 
            // lblMoTa
            // 
            lblMoTa.AutoSize = true;
            lblMoTa.ForeColor = Color.FromArgb(90, 98, 110);
            lblMoTa.Location = new Point(27, 44);
            lblMoTa.Name = "lblMoTa";
            lblMoTa.Size = new Size(620, 20);
            lblMoTa.TabIndex = 1;
            lblMoTa.Text = "Chọn một loại dữ liệu, tải tệp Excel hoặc CSV, xem trước rồi xác nhận nhập. Toàn bộ tệp được nhập trong một lần.";
            // 
            // panelCauHinh
            // 
            panelCauHinh.Controls.Add(lblLoai);
            panelCauHinh.Controls.Add(cboLoai);
            panelCauHinh.Controls.Add(lblTep);
            panelCauHinh.Controls.Add(txtTep);
            panelCauHinh.Controls.Add(btnChonTep);
            panelCauHinh.Controls.Add(btnTaiMau);
            panelCauHinh.Controls.Add(btnXemTruoc);
            panelCauHinh.Controls.Add(lblGoiY);
            panelCauHinh.Dock = DockStyle.Top;
            panelCauHinh.Location = new Point(0, 64);
            panelCauHinh.Name = "panelCauHinh";
            panelCauHinh.Size = new Size(1100, 112);
            panelCauHinh.TabIndex = 1;
            // 
            // lblLoai
            // 
            lblLoai.AutoSize = true;
            lblLoai.Location = new Point(24, 18);
            lblLoai.Name = "lblLoai";
            lblLoai.Size = new Size(88, 20);
            lblLoai.TabIndex = 0;
            lblLoai.Text = "Loại dữ liệu:";
            // 
            // cboLoai
            // 
            cboLoai.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLoai.FormattingEnabled = true;
            cboLoai.Location = new Point(130, 14);
            cboLoai.Name = "cboLoai";
            cboLoai.Size = new Size(240, 28);
            cboLoai.TabIndex = 1;
            cboLoai.SelectedIndexChanged += cboLoai_SelectedIndexChanged;
            // 
            // lblTep
            // 
            lblTep.AutoSize = true;
            lblTep.Location = new Point(24, 58);
            lblTep.Name = "lblTep";
            lblTep.Size = new Size(80, 20);
            lblTep.TabIndex = 2;
            lblTep.Text = "Tệp dữ liệu:";
            // 
            // txtTep
            // 
            txtTep.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtTep.Location = new Point(130, 54);
            txtTep.Name = "txtTep";
            txtTep.PlaceholderText = "Chưa chọn tệp .xlsx hoặc .csv";
            txtTep.ReadOnly = true;
            txtTep.Size = new Size(520, 27);
            txtTep.TabIndex = 3;
            // 
            // btnChonTep
            // 
            btnChonTep.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnChonTep.Location = new Point(662, 53);
            btnChonTep.Name = "btnChonTep";
            btnChonTep.Size = new Size(112, 30);
            btnChonTep.TabIndex = 4;
            btnChonTep.Text = "Chọn tệp...";
            btnChonTep.UseVisualStyleBackColor = true;
            btnChonTep.Click += btnChonTep_Click;
            // 
            // btnTaiMau
            // 
            btnTaiMau.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnTaiMau.Location = new Point(784, 53);
            btnTaiMau.Name = "btnTaiMau";
            btnTaiMau.Size = new Size(124, 30);
            btnTaiMau.TabIndex = 5;
            btnTaiMau.Text = "Tải tệp mẫu";
            btnTaiMau.UseVisualStyleBackColor = true;
            btnTaiMau.Click += btnTaiMau_Click;
            // 
            // btnXemTruoc
            // 
            btnXemTruoc.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnXemTruoc.Location = new Point(918, 53);
            btnXemTruoc.Name = "btnXemTruoc";
            btnXemTruoc.Size = new Size(112, 30);
            btnXemTruoc.TabIndex = 6;
            btnXemTruoc.Text = "Xem trước";
            btnXemTruoc.UseVisualStyleBackColor = true;
            btnXemTruoc.Click += btnXemTruoc_Click;
            // 
            // lblGoiY
            // 
            lblGoiY.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblGoiY.AutoEllipsis = true;
            lblGoiY.ForeColor = Color.FromArgb(90, 98, 110);
            lblGoiY.Location = new Point(130, 86);
            lblGoiY.Name = "lblGoiY";
            lblGoiY.Size = new Size(900, 20);
            lblGoiY.TabIndex = 7;
            lblGoiY.Text = "Hỗ trợ .xlsx và .csv. Hệ thống tự nhận diện cột theo tiêu đề tiếng Việt hoặc tên cột trong database. Có thể kéo thả tệp vào cửa sổ này.";
            // 
            // panelTomTat
            // 
            panelTomTat.Controls.Add(lblTongDongCaption);
            panelTomTat.Controls.Add(lblTongDong);
            panelTomTat.Controls.Add(lblHopLeCaption);
            panelTomTat.Controls.Add(lblHopLe);
            panelTomTat.Controls.Add(lblLoiCaption);
            panelTomTat.Controls.Add(lblLoi);
            panelTomTat.Controls.Add(lblTrangThai);
            panelTomTat.Controls.Add(lblCanhBao);
            panelTomTat.Dock = DockStyle.Top;
            panelTomTat.Location = new Point(0, 176);
            panelTomTat.Name = "panelTomTat";
            panelTomTat.Size = new Size(1100, 74);
            panelTomTat.TabIndex = 2;
            // 
            // lblTongDongCaption
            // 
            lblTongDongCaption.AutoSize = true;
            lblTongDongCaption.Location = new Point(24, 12);
            lblTongDongCaption.Name = "lblTongDongCaption";
            lblTongDongCaption.Size = new Size(48, 20);
            lblTongDongCaption.TabIndex = 0;
            lblTongDongCaption.Text = "Tổng dòng:";
            // 
            // lblTongDong
            // 
            lblTongDong.AutoSize = true;
            lblTongDong.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTongDong.Location = new Point(112, 10);
            lblTongDong.Name = "lblTongDong";
            lblTongDong.Size = new Size(20, 23);
            lblTongDong.TabIndex = 1;
            lblTongDong.Text = "0";
            // 
            // lblHopLeCaption
            // 
            lblHopLeCaption.AutoSize = true;
            lblHopLeCaption.Location = new Point(210, 12);
            lblHopLeCaption.Name = "lblHopLeCaption";
            lblHopLeCaption.Size = new Size(58, 20);
            lblHopLeCaption.TabIndex = 2;
            lblHopLeCaption.Text = "Hợp lệ:";
            // 
            // lblHopLe
            // 
            lblHopLe.AutoSize = true;
            lblHopLe.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblHopLe.ForeColor = Color.FromArgb(24, 128, 56);
            lblHopLe.Location = new Point(272, 10);
            lblHopLe.Name = "lblHopLe";
            lblHopLe.Size = new Size(20, 23);
            lblHopLe.TabIndex = 3;
            lblHopLe.Text = "0";
            // 
            // lblLoiCaption
            // 
            lblLoiCaption.AutoSize = true;
            lblLoiCaption.Location = new Point(368, 12);
            lblLoiCaption.Name = "lblLoiCaption";
            lblLoiCaption.Size = new Size(35, 20);
            lblLoiCaption.TabIndex = 4;
            lblLoiCaption.Text = "Lỗi:";
            // 
            // lblLoi
            // 
            lblLoi.AutoSize = true;
            lblLoi.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblLoi.ForeColor = Color.FromArgb(192, 57, 43);
            lblLoi.Location = new Point(406, 10);
            lblLoi.Name = "lblLoi";
            lblLoi.Size = new Size(20, 23);
            lblLoi.TabIndex = 5;
            lblLoi.Text = "0";
            // 
            // lblTrangThai
            // 
            lblTrangThai.AutoSize = true;
            lblTrangThai.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTrangThai.Location = new Point(506, 11);
            lblTrangThai.Name = "lblTrangThai";
            lblTrangThai.Size = new Size(190, 23);
            lblTrangThai.TabIndex = 6;
            lblTrangThai.Text = "Chưa chọn tệp dữ liệu";
            // 
            // lblCanhBao
            // 
            lblCanhBao.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblCanhBao.AutoEllipsis = true;
            lblCanhBao.ForeColor = Color.FromArgb(176, 103, 0);
            lblCanhBao.Location = new Point(27, 40);
            lblCanhBao.Name = "lblCanhBao";
            lblCanhBao.Size = new Size(1046, 24);
            lblCanhBao.TabIndex = 7;
            // 
            // panelDanhSach
            // 
            panelDanhSach.Controls.Add(dgvDuLieu);
            panelDanhSach.Dock = DockStyle.Fill;
            panelDanhSach.Location = new Point(0, 250);
            panelDanhSach.Name = "panelDanhSach";
            panelDanhSach.Padding = new Padding(24, 6, 24, 6);
            panelDanhSach.Size = new Size(1100, 394);
            panelDanhSach.TabIndex = 3;
            // 
            // dgvDuLieu
            // 
            dgvDuLieu.AllowUserToAddRows = false;
            dgvDuLieu.AllowUserToDeleteRows = false;
            dgvDuLieu.AutoGenerateColumns = false;
            dgvDuLieu.BackgroundColor = Color.White;
            dgvDuLieu.BorderStyle = BorderStyle.None;
            dgvDuLieu.ColumnHeadersHeight = 34;
            dgvDuLieu.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvDuLieu.Dock = DockStyle.Fill;
            dgvDuLieu.Location = new Point(24, 6);
            dgvDuLieu.Name = "dgvDuLieu";
            dgvDuLieu.ReadOnly = true;
            dgvDuLieu.RowHeadersVisible = false;
            dgvDuLieu.RowTemplate.Height = 28;
            dgvDuLieu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDuLieu.Size = new Size(1052, 382);
            dgvDuLieu.TabIndex = 0;
            // 
            // panelNut
            // 
            panelNut.Controls.Add(progressNhap);
            panelNut.Controls.Add(btnXuatBaoCao);
            panelNut.Controls.Add(btnXacNhan);
            panelNut.Controls.Add(btnDong);
            panelNut.Dock = DockStyle.Bottom;
            panelNut.Location = new Point(0, 644);
            panelNut.Name = "panelNut";
            panelNut.Size = new Size(1100, 56);
            panelNut.TabIndex = 4;
            // 
            // progressNhap
            // 
            progressNhap.Location = new Point(24, 17);
            progressNhap.MarqueeAnimationSpeed = 30;
            progressNhap.Name = "progressNhap";
            progressNhap.Size = new Size(220, 20);
            progressNhap.Style = ProgressBarStyle.Marquee;
            progressNhap.TabIndex = 0;
            progressNhap.Visible = false;
            // 
            // btnXuatBaoCao
            // 
            btnXuatBaoCao.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnXuatBaoCao.Location = new Point(642, 12);
            btnXuatBaoCao.Name = "btnXuatBaoCao";
            btnXuatBaoCao.Size = new Size(150, 32);
            btnXuatBaoCao.TabIndex = 1;
            btnXuatBaoCao.Text = "Xuất báo cáo lỗi";
            btnXuatBaoCao.UseVisualStyleBackColor = true;
            btnXuatBaoCao.Click += btnXuatBaoCao_Click;
            // 
            // btnXacNhan
            // 
            btnXacNhan.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnXacNhan.Enabled = false;
            btnXacNhan.Location = new Point(802, 12);
            btnXacNhan.Name = "btnXacNhan";
            btnXacNhan.Size = new Size(160, 32);
            btnXacNhan.TabIndex = 2;
            btnXacNhan.Text = "Xác nhận nhập";
            btnXacNhan.UseVisualStyleBackColor = true;
            btnXacNhan.Click += btnXacNhan_Click;
            // 
            // btnDong
            // 
            btnDong.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDong.Location = new Point(972, 12);
            btnDong.Name = "btnDong";
            btnDong.Size = new Size(104, 32);
            btnDong.TabIndex = 3;
            btnDong.Text = "Đóng";
            btnDong.UseVisualStyleBackColor = true;
            btnDong.Click += btnDong_Click;
            // 
            // FrmBulkImport
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 700);
            Controls.Add(panelDanhSach);
            Controls.Add(panelNut);
            Controls.Add(panelTomTat);
            Controls.Add(panelCauHinh);
            Controls.Add(panelTieuDe);
            Name = "FrmBulkImport";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Nhập dữ liệu hàng loạt";
            Load += FrmBulkImport_Load;
            DragDrop += FrmBulkImport_DragDrop;
            DragEnter += FrmBulkImport_DragEnter;
            DragLeave += FrmBulkImport_DragLeave;
            panelTieuDe.ResumeLayout(false);
            panelTieuDe.PerformLayout();
            panelCauHinh.ResumeLayout(false);
            panelCauHinh.PerformLayout();
            panelTomTat.ResumeLayout(false);
            panelTomTat.PerformLayout();
            panelDanhSach.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvDuLieu).EndInit();
            panelNut.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelTieuDe;
        private Label lblTieuDe;
        private Label lblMoTa;
        private Panel panelCauHinh;
        private Label lblLoai;
        private ComboBox cboLoai;
        private Label lblTep;
        private TextBox txtTep;
        private Button btnChonTep;
        private Button btnTaiMau;
        private Button btnXemTruoc;
        private Label lblGoiY;
        private Panel panelTomTat;
        private Label lblTongDongCaption;
        private Label lblTongDong;
        private Label lblHopLeCaption;
        private Label lblHopLe;
        private Label lblLoiCaption;
        private Label lblLoi;
        private Label lblTrangThai;
        private Label lblCanhBao;
        private Panel panelDanhSach;
        private DataGridView dgvDuLieu;
        private Panel panelNut;
        private ProgressBar progressNhap;
        private Button btnXuatBaoCao;
        private Button btnXacNhan;
        private Button btnDong;
    }
}
