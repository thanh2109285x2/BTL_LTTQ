#nullable enable
namespace BTL_LTTQ.Forms.BaoCao
{
    partial class frmBaoCaoTop3
    {
        private System.ComponentModel.IContainer? components = null;
        private DataGridView dgvBaoCao = null!;
        private Label lblTong = null!;
        private AppButton btnLamMoi = null!;
        private AppButton btnXuatExcel = null!;
        private AppButton btnXemIn = null!;
        private Panel pnlCongCu = null!;
        private Label lblLop = null!;
        private ComboBox cboLop = null!;
        private Label lblTimKiem = null!;
        private TextBox txtTimKiem = null!;
        private Label lblSapXep = null!;
        private ComboBox cboSapXep = null!;
        private AppButton btnSapXep = null!;
        private Panel pnlTieuDe = null!;
        private Label lblTieuDe = null!;
        private Label lblMoTa = null!;

        private void InitializeComponent()
        {
            dgvBaoCao = new DataGridView();
            lblTong = new Label();
            btnLamMoi = new AppButton { Kieu = KieuNut.Phu };
            btnXuatExcel = new AppButton { Kieu = KieuNut.Chinh };
            btnXemIn = new AppButton { Kieu = KieuNut.Phu };
            pnlCongCu = new Panel();
            lblLop = new Label();
            cboLop = new ComboBox();
            lblTimKiem = new Label();
            txtTimKiem = new TextBox();
            lblSapXep = new Label();
            cboSapXep = new ComboBox();
            btnSapXep = new AppButton { Kieu = KieuNut.Phu };
            pnlTieuDe = new Panel();
            lblTieuDe = new Label();
            lblMoTa = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvBaoCao).BeginInit();
            pnlCongCu.SuspendLayout();
            pnlTieuDe.SuspendLayout();
            SuspendLayout();
            //
            // dgvBaoCao
            //
            dgvBaoCao.AllowUserToAddRows = false;
            dgvBaoCao.AllowUserToDeleteRows = false;
            dgvBaoCao.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBaoCao.ColumnHeadersHeight = 29;
            dgvBaoCao.Dock = DockStyle.Fill;
            dgvBaoCao.Location = new Point(0, 98);
            dgvBaoCao.MultiSelect = false;
            dgvBaoCao.Name = "dgvBaoCao";
            dgvBaoCao.ReadOnly = true;
            dgvBaoCao.RowHeadersWidth = 51;
            dgvBaoCao.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBaoCao.Size = new Size(900, 464);
            dgvBaoCao.TabIndex = 0;
            //
            // lblTong
            //
            lblTong.Dock = DockStyle.Bottom;
            lblTong.BackColor = Color.White;
            lblTong.Location = new Point(0, 562);
            lblTong.Name = "lblTong";
            lblTong.Padding = new Padding(10, 0, 0, 0);
            lblTong.Size = new Size(900, 38);
            lblTong.TabIndex = 1;
            lblTong.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnLamMoi
            //
            btnLamMoi.Location = new Point(378, 52);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(90, 34);
            btnLamMoi.TabIndex = 6;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.Click += btnLamMoi_Click;
            //
            // btnXuatExcel
            //
            btnXuatExcel.Location = new Point(476, 52);
            btnXuatExcel.Name = "btnXuatExcel";
            btnXuatExcel.Size = new Size(100, 34);
            btnXuatExcel.TabIndex = 7;
            btnXuatExcel.Text = "Xuất Excel";
            btnXuatExcel.Click += btnXuatExcel_Click;
            //
            // btnXemIn
            //
            btnXemIn.Location = new Point(584, 52);
            btnXemIn.Name = "btnXemIn";
            btnXemIn.Size = new Size(135, 34);
            btnXemIn.TabIndex = 8;
            btnXemIn.Text = "Xem / In / PDF";
            btnXemIn.Click += btnXemIn_Click;
            //
            // btnSapXep
            //
            btnSapXep.Location = new Point(280, 52);
            btnSapXep.Name = "btnSapXep";
            btnSapXep.Size = new Size(90, 34);
            btnSapXep.TabIndex = 9;
            btnSapXep.Text = "Sắp xếp";
            btnSapXep.Click += btnSapXep_Click;
            //
            // pnlCongCu
            //
            pnlCongCu.Controls.Add(lblLop);
            pnlCongCu.Controls.Add(cboLop);
            pnlCongCu.Controls.Add(lblTimKiem);
            pnlCongCu.Controls.Add(txtTimKiem);
            pnlCongCu.Controls.Add(lblSapXep);
            pnlCongCu.Controls.Add(cboSapXep);
            pnlCongCu.Controls.Add(btnSapXep);
            pnlCongCu.Controls.Add(btnLamMoi);
            pnlCongCu.Controls.Add(btnXuatExcel);
            pnlCongCu.Controls.Add(btnXemIn);
            pnlCongCu.Dock = DockStyle.Top;
            pnlCongCu.BackColor = Color.White;
            pnlCongCu.Location = new Point(0, 0);
            pnlCongCu.Name = "pnlCongCu";
            pnlCongCu.Padding = new Padding(8);
            pnlCongCu.Size = new Size(900, 98);
            pnlCongCu.TabIndex = 2;
            //
            // pnlTieuDe
            //
            pnlTieuDe.BackColor = Color.FromArgb(41, 98, 168);
            pnlTieuDe.Controls.Add(lblTieuDe);
            pnlTieuDe.Controls.Add(lblMoTa);
            pnlTieuDe.Dock = DockStyle.Top;
            pnlTieuDe.Name = "pnlTieuDe";
            pnlTieuDe.Size = new Size(900, 74);
            //
            // lblTieuDe
            //
            lblTieuDe.AutoSize = true;
            lblTieuDe.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblTieuDe.ForeColor = Color.White;
            lblTieuDe.Location = new Point(18, 8);
            lblTieuDe.Name = "lblTieuDe";
            lblTieuDe.Text = "BẢNG XẾP HẠNG HỌC VIÊN";
            //
            // lblMoTa
            //
            lblMoTa.AutoSize = true;
            lblMoTa.Font = new Font("Segoe UI", 9F);
            lblMoTa.ForeColor = Color.FromArgb(226, 237, 250);
            lblMoTa.Location = new Point(20, 43);
            lblMoTa.Name = "lblMoTa";
            lblMoTa.Text = "Top 3 học viên có điểm cao nhất trong từng lớp";
            //
            // lblLop
            //
            lblLop.AutoSize = true;
            lblLop.Location = new Point(10, 16);
            lblLop.Name = "lblLop";
            lblLop.Size = new Size(34, 20);
            lblLop.TabIndex = 0;
            lblLop.Text = "Lớp";
            //
            // cboLop
            //
            cboLop.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLop.Location = new Point(55, 9);
            cboLop.Name = "cboLop";
            cboLop.Size = new Size(210, 28);
            cboLop.TabIndex = 1;
            cboLop.SelectedIndexChanged += cboLop_SelectedIndexChanged;
            //
            // lblTimKiem
            //
            lblTimKiem.AutoSize = true;
            lblTimKiem.Location = new Point(280, 16);
            lblTimKiem.Name = "lblTimKiem";
            lblTimKiem.Size = new Size(93, 20);
            lblTimKiem.TabIndex = 2;
            lblTimKiem.Text = "Tìm học viên";
            //
            // txtTimKiem
            //
            txtTimKiem.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtTimKiem.Location = new Point(379, 12);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.PlaceholderText = "Mã hoặc tên học viên";
            txtTimKiem.Size = new Size(510, 27);
            txtTimKiem.TabIndex = 3;
            txtTimKiem.TextChanged += txtTimKiem_TextChanged;
            //
            // lblSapXep
            //
            lblSapXep.AutoSize = true;
            lblSapXep.Location = new Point(10, 62);
            lblSapXep.Name = "lblSapXep";
            lblSapXep.Size = new Size(62, 20);
            lblSapXep.TabIndex = 4;
            lblSapXep.Text = "Sắp xếp";
            //
            // cboSapXep
            //
            cboSapXep.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSapXep.Items.AddRange(new object[] { "Theo lớp và hạng", "Điểm cao xuống thấp", "Điểm thấp lên cao" });
            cboSapXep.Location = new Point(75, 54);
            cboSapXep.Name = "cboSapXep";
            cboSapXep.Size = new Size(190, 28);
            cboSapXep.TabIndex = 5;
            //
            // frmBaoCaoTop3
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 600);
            Controls.Add(dgvBaoCao);
            Controls.Add(lblTong);
            Controls.Add(pnlCongCu);
            Controls.Add(pnlTieuDe);
            MinimumSize = new Size(700, 450);
            Name = "frmBaoCaoTop3";
            Text = "Bảng xếp hạng học viên";
            ((System.ComponentModel.ISupportInitialize)dgvBaoCao).EndInit();
            pnlCongCu.ResumeLayout(false);
            pnlCongCu.PerformLayout();
            pnlTieuDe.ResumeLayout(false);
            pnlTieuDe.PerformLayout();
            ResumeLayout(false);
        }

        protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
    }
}
