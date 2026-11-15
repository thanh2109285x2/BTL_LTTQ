#nullable enable
namespace BTL_LTTQ.Forms.BaoCao
{
    partial class frmBaoCaoDiem
    {
        private System.ComponentModel.IContainer? components = null;
        private ComboBox _cboLop = null!;
        private Button btnXemBaoCao = null!;
        private DataGridView _dgvBaoCao = null!;
        private Label _lblTong = null!;
        private GroupBox grpDieuKien = null!;
        private GroupBox grpKetQua = null!;
        private FlowLayoutPanel pnlDieuKien = null!;
        private Label lblLop = null!;
        private TextBox txtTimKiemBaoCao = null!;
        private ComboBox cboSapXepBaoCao = null!;
        private Button btnSapXepBaoCao = null!;
        private Button btnLamMoiBaoCao = null!;
        private Button btnXuatExcelBaoCao = null!;
        private Button btnXemInPdfBaoCao = null!;
        private Label lblTimKiemBaoCao = null!;
        private Label lblSapXepBaoCao = null!;
        private void InitializeComponent()
        {
            _cboLop = new ComboBox();
            btnXemBaoCao = new Button();
            _dgvBaoCao = new DataGridView();
            _lblTong = new Label();
            grpDieuKien = new GroupBox();
            pnlDieuKien = new FlowLayoutPanel();
            lblLop = new Label();
            lblTimKiemBaoCao = new Label();
            txtTimKiemBaoCao = new TextBox();
            lblSapXepBaoCao = new Label();
            cboSapXepBaoCao = new ComboBox();
            btnSapXepBaoCao = new Button();
            btnLamMoiBaoCao = new Button();
            btnXuatExcelBaoCao = new Button();
            btnXemInPdfBaoCao = new Button();
            grpKetQua = new GroupBox();
            ((System.ComponentModel.ISupportInitialize)_dgvBaoCao).BeginInit();
            grpDieuKien.SuspendLayout();
            pnlDieuKien.SuspendLayout();
            grpKetQua.SuspendLayout();
            SuspendLayout();
            // 
            // _cboLop
            // 
            _cboLop.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboLop.Margin = new Padding(4, 7, 8, 4);
            _cboLop.Location = new Point(90, 20);
            _cboLop.Name = "_cboLop";
            _cboLop.Size = new Size(240, 28);
            _cboLop.TabIndex = 1;
            // 
            // btnXemBaoCao
            // 
            btnXemBaoCao.Margin = new Padding(4, 4, 8, 4);
            btnXemBaoCao.Location = new Point(465, 18);
            btnXemBaoCao.Name = "btnXemBaoCao";
            btnXemBaoCao.Size = new Size(118, 34);
            btnXemBaoCao.TabIndex = 2;
            btnXemBaoCao.Text = "Xem báo cáo";
            btnXemBaoCao.Click += btnXemBaoCao_Click;
            // 
            // _dgvBaoCao
            // 
            _dgvBaoCao.AllowUserToAddRows = false;
            _dgvBaoCao.AllowUserToDeleteRows = false;
            _dgvBaoCao.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _dgvBaoCao.ColumnHeadersHeight = 29;
            _dgvBaoCao.Dock = DockStyle.Fill;
            _dgvBaoCao.Location = new Point(8, 28);
            _dgvBaoCao.Name = "_dgvBaoCao";
            _dgvBaoCao.ReadOnly = true;
            _dgvBaoCao.RowHeadersWidth = 51;
            _dgvBaoCao.Size = new Size(884, 376);
            _dgvBaoCao.TabIndex = 0;
            // 
            // _lblTong
            // 
            _lblTong.Dock = DockStyle.Bottom;
            _lblTong.Location = new Point(0, 562);
            _lblTong.Name = "_lblTong";
            _lblTong.Size = new Size(900, 38);
            _lblTong.TabIndex = 1;
            _lblTong.TextAlign = ContentAlignment.MiddleRight;
            // 
            // grpDieuKien
            // 
            grpDieuKien.Controls.Add(pnlDieuKien);
            grpDieuKien.Dock = DockStyle.Top;
            grpDieuKien.Location = new Point(0, 0);
            grpDieuKien.Name = "grpDieuKien";
            grpDieuKien.Padding = new Padding(10);
            grpDieuKien.Size = new Size(900, 185);
            grpDieuKien.TabIndex = 2;
            grpDieuKien.TabStop = false;
            grpDieuKien.Text = "Điều kiện báo cáo";
            // 
            // pnlDieuKien
            // 
            pnlDieuKien.AutoScroll = true;
            pnlDieuKien.FlowDirection = FlowDirection.LeftToRight;
            pnlDieuKien.WrapContents = true;
            pnlDieuKien.Padding = new Padding(2, 3, 2, 3);
            pnlDieuKien.Controls.Add(lblLop);
            pnlDieuKien.Controls.Add(_cboLop);
            pnlDieuKien.Controls.Add(btnXemBaoCao);
            pnlDieuKien.Controls.Add(lblTimKiemBaoCao);
            pnlDieuKien.Controls.Add(txtTimKiemBaoCao);
            pnlDieuKien.Controls.Add(lblSapXepBaoCao);
            pnlDieuKien.Controls.Add(cboSapXepBaoCao);
            pnlDieuKien.Controls.Add(btnSapXepBaoCao);
            pnlDieuKien.Controls.Add(btnLamMoiBaoCao);
            pnlDieuKien.Controls.Add(btnXuatExcelBaoCao);
            pnlDieuKien.Controls.Add(btnXemInPdfBaoCao);
            pnlDieuKien.Dock = DockStyle.Fill;
            pnlDieuKien.Location = new Point(10, 30);
            pnlDieuKien.Name = "pnlDieuKien";
            pnlDieuKien.Size = new Size(880, 110);
            pnlDieuKien.TabIndex = 0;
            // 
            // lblLop
            // 
            lblLop.AutoSize = true;
            lblLop.Margin = new Padding(4, 12, 3, 0);
            lblLop.Location = new Point(12, 25);
            lblLop.Name = "lblLop";
            lblLop.Size = new Size(62, 20);
            lblLop.TabIndex = 0;
            lblLop.Text = "Lớp học";
            // 
            // lblTimKiemBaoCao
            // 
            lblTimKiemBaoCao.AutoSize = true;
            lblTimKiemBaoCao.Margin = new Padding(4, 12, 3, 0);
            lblTimKiemBaoCao.Location = new Point(12, 78);
            lblTimKiemBaoCao.Name = "lblTimKiemBaoCao";
            lblTimKiemBaoCao.Size = new Size(93, 20);
            lblTimKiemBaoCao.TabIndex = 3;
            lblTimKiemBaoCao.Text = "Tìm học viên";
            // 
            // txtTimKiemBaoCao
            // 
            txtTimKiemBaoCao.Margin = new Padding(2, 7, 8, 4);
            txtTimKiemBaoCao.Location = new Point(105, 72);
            txtTimKiemBaoCao.Name = "txtTimKiemBaoCao";
            txtTimKiemBaoCao.Size = new Size(185, 27);
            txtTimKiemBaoCao.TabIndex = 4;
            // 
            // lblSapXepBaoCao
            // 
            lblSapXepBaoCao.AutoSize = true;
            lblSapXepBaoCao.Margin = new Padding(2, 12, 3, 0);
            lblSapXepBaoCao.Location = new Point(296, 78);
            lblSapXepBaoCao.Name = "lblSapXepBaoCao";
            lblSapXepBaoCao.Size = new Size(62, 20);
            lblSapXepBaoCao.TabIndex = 5;
            lblSapXepBaoCao.Text = "Sắp xếp";
            // 
            // cboSapXepBaoCao
            // 
            cboSapXepBaoCao.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSapXepBaoCao.Margin = new Padding(2, 7, 6, 4);
            cboSapXepBaoCao.Location = new Point(365, 72);
            cboSapXepBaoCao.Name = "cboSapXepBaoCao";
            cboSapXepBaoCao.Size = new Size(145, 28);
            cboSapXepBaoCao.TabIndex = 6;
            // 
            // btnSapXepBaoCao
            // 
            btnSapXepBaoCao.Margin = new Padding(2, 4, 4, 4);
            btnSapXepBaoCao.Location = new Point(520, 70);
            btnSapXepBaoCao.Name = "btnSapXepBaoCao";
            btnSapXepBaoCao.Size = new Size(78, 32);
            btnSapXepBaoCao.TabIndex = 7;
            btnSapXepBaoCao.Text = "Sắp xếp";
            // 
            // btnLamMoiBaoCao
            // 
            btnLamMoiBaoCao.Margin = new Padding(2, 4, 4, 4);
            btnLamMoiBaoCao.Location = new Point(604, 70);
            btnLamMoiBaoCao.Name = "btnLamMoiBaoCao";
            btnLamMoiBaoCao.Size = new Size(72, 32);
            btnLamMoiBaoCao.TabIndex = 8;
            btnLamMoiBaoCao.Text = "Làm mới";
            // 
            // btnXuatExcelBaoCao
            // 
            btnXuatExcelBaoCao.Margin = new Padding(2, 4, 4, 4);
            btnXuatExcelBaoCao.Location = new Point(682, 70);
            btnXuatExcelBaoCao.Name = "btnXuatExcelBaoCao";
            btnXuatExcelBaoCao.Size = new Size(80, 32);
            btnXuatExcelBaoCao.TabIndex = 9;
            btnXuatExcelBaoCao.Text = "Xuất Excel";
            // 
            // btnXemInPdfBaoCao
            // 
            btnXemInPdfBaoCao.Margin = new Padding(2, 4, 4, 4);
            btnXemInPdfBaoCao.Location = new Point(768, 70);
            btnXemInPdfBaoCao.Name = "btnXemInPdfBaoCao";
            btnXemInPdfBaoCao.Size = new Size(122, 32);
            btnXemInPdfBaoCao.TabIndex = 10;
            btnXemInPdfBaoCao.Text = "Xem / In / PDF";
            // 
            // grpKetQua
            // 
            grpKetQua.Controls.Add(_dgvBaoCao);
            grpKetQua.Dock = DockStyle.Fill;
            grpKetQua.Location = new Point(0, 185);
            grpKetQua.Name = "grpKetQua";
            grpKetQua.Padding = new Padding(8);
            grpKetQua.Size = new Size(900, 377);
            grpKetQua.TabIndex = 0;
            grpKetQua.TabStop = false;
            grpKetQua.Text = "Báo cáo danh sách điểm theo lớp học";
            // 
            // frmBaoCaoDiem
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 600);
            Controls.Add(grpKetQua);
            Controls.Add(_lblTong);
            Controls.Add(grpDieuKien);
            MinimumSize = new Size(700, 500);
            Name = "frmBaoCaoDiem";
            Text = "Báo cáo danh sách điểm theo lớp học";
            ((System.ComponentModel.ISupportInitialize)_dgvBaoCao).EndInit();
            grpDieuKien.ResumeLayout(false);
            pnlDieuKien.ResumeLayout(false);
            pnlDieuKien.PerformLayout();
            grpKetQua.ResumeLayout(false);
            ResumeLayout(false);
        }
        protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
    }
}
