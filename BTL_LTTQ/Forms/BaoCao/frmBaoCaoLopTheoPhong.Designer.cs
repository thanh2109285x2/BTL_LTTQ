#nullable enable
namespace BTL_LTTQ.Forms.BaoCao
{
    partial class frmBaoCaoLopTheoPhong
    {
        private System.ComponentModel.IContainer? components = null;
        private DataGridView dgvBaoCao = null!;
        private Label lblTong = null!;
        private Label lblPhong = null!;
        private ComboBox cboPhong = null!;
        private Button btnXemBaoCao = null!;
        private Button btnLamMoi = null!;
        private Button btnXuatExcel = null!;
        private Button btnXemIn = null!;
        private Panel pnlCongCu = null!;
        private Panel pnlTieuDe = null!;
        private Label lblTieuDe = null!;
        private Label lblMoTa = null!;

        private void InitializeComponent()
        {
            dgvBaoCao = new DataGridView();
            lblTong = new Label();
            lblPhong = new Label();
            cboPhong = new ComboBox();
            btnXemBaoCao = new Button();
            btnLamMoi = new Button();
            btnXuatExcel = new Button();
            btnXemIn = new Button();
            pnlCongCu = new Panel();
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
            dgvBaoCao.Location = new Point(0, 92);
            dgvBaoCao.MultiSelect = false;
            dgvBaoCao.Name = "dgvBaoCao";
            dgvBaoCao.ReadOnly = true;
            dgvBaoCao.RowHeadersWidth = 51;
            dgvBaoCao.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBaoCao.Size = new Size(900, 470);
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
            // lblPhong
            //
            lblPhong.AutoSize = true;
            lblPhong.Location = new Point(10, 17);
            lblPhong.Name = "lblPhong";
            lblPhong.Size = new Size(113, 20);
            lblPhong.TabIndex = 0;
            lblPhong.Text = "Lọc theo phòng";
            //
            // cboPhong
            //
            cboPhong.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cboPhong.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPhong.Location = new Point(129, 10);
            cboPhong.Name = "cboPhong";
            cboPhong.Size = new Size(724, 28);
            cboPhong.TabIndex = 1;
            //
            // btnXemBaoCao
            //
            btnXemBaoCao.Location = new Point(12, 47);
            btnXemBaoCao.Name = "btnXemBaoCao";
            btnXemBaoCao.Size = new Size(120, 34);
            btnXemBaoCao.TabIndex = 2;
            btnXemBaoCao.Text = "Xem báo cáo";
            btnXemBaoCao.Click += btnXemBaoCao_Click;
            //
            // btnLamMoi
            //
            btnLamMoi.Location = new Point(138, 47);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(100, 34);
            btnLamMoi.TabIndex = 3;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.Click += btnLamMoi_Click;
            //
            // btnXuatExcel
            //
            btnXuatExcel.Location = new Point(743, 47);
            btnXuatExcel.Name = "btnXuatExcel";
            btnXuatExcel.Size = new Size(110, 34);
            btnXuatExcel.TabIndex = 4;
            btnXuatExcel.Text = "Xuất Excel";
            btnXuatExcel.Click += btnXuatExcel_Click;
            //
            // btnXemIn
            //
            btnXemIn.Location = new Point(597, 47);
            btnXemIn.Name = "btnXemIn";
            btnXemIn.Size = new Size(140, 34);
            btnXemIn.TabIndex = 5;
            btnXemIn.Text = "Xem / In / PDF";
            btnXemIn.Click += btnXemIn_Click;
            //
            // pnlCongCu
            //
            pnlCongCu.Controls.Add(lblPhong);
            pnlCongCu.Controls.Add(cboPhong);
            pnlCongCu.Controls.Add(btnXemBaoCao);
            pnlCongCu.Controls.Add(btnLamMoi);
            pnlCongCu.Controls.Add(btnXuatExcel);
            pnlCongCu.Controls.Add(btnXemIn);
            pnlCongCu.Dock = DockStyle.Top;
            pnlCongCu.BackColor = Color.White;
            pnlCongCu.Location = new Point(0, 0);
            pnlCongCu.Name = "pnlCongCu";
            pnlCongCu.Padding = new Padding(8);
            pnlCongCu.Size = new Size(900, 92);
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
            lblTieuDe.Text = "LỚP HỌC THEO PHÒNG";
            //
            // lblMoTa
            //
            lblMoTa.AutoSize = true;
            lblMoTa.Font = new Font("Segoe UI", 9F);
            lblMoTa.ForeColor = Color.FromArgb(226, 237, 250);
            lblMoTa.Location = new Point(20, 43);
            lblMoTa.Name = "lblMoTa";
            lblMoTa.Text = "Tra cứu các lớp đang được bố trí tại từng phòng học";
            //
            // frmBaoCaoLopTheoPhong
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 600);
            Controls.Add(dgvBaoCao);
            Controls.Add(lblTong);
            Controls.Add(pnlCongCu);
            Controls.Add(pnlTieuDe);
            MinimumSize = new Size(700, 450);
            Name = "frmBaoCaoLopTheoPhong";
            Text = "Danh sách lớp học theo phòng";
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
