#nullable enable
namespace BTL_LTTQ.Forms.BaoCao
{
    partial class frmBaoCaoDoanhThu
    {
        private System.ComponentModel.IContainer? components = null;
        private ComboBox _cboLop = null!;
        private Button btnXemBaoCao = null!;
        private DataGridView _dgvBaoCao = null!;
        private Label _lblTong = null!;
        private GroupBox grpDieuKien = null!;
        private GroupBox grpKetQua = null!;
        private Panel pnlDieuKien = null!;
        private Label lblLop = null!;
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            _cboLop = new ComboBox(); btnXemBaoCao = new Button(); _dgvBaoCao = new DataGridView(); _lblTong = new Label();
            grpDieuKien = new GroupBox(); grpKetQua = new GroupBox(); pnlDieuKien = new Panel(); lblLop = new Label();
            grpDieuKien.Name = "grpDieuKien"; grpKetQua.Name = "grpKetQua"; pnlDieuKien.Name = "pnlDieuKien"; lblLop.Name = "lblLop";
            _cboLop.Name = "cboLop"; _dgvBaoCao.Name = "dgvBaoCao"; _lblTong.Name = "lblTong";
            ((System.ComponentModel.ISupportInitialize)_dgvBaoCao).BeginInit(); SuspendLayout();
            grpDieuKien.Dock = DockStyle.Top; grpDieuKien.Height = 96; grpDieuKien.Text = "Điều kiện báo cáo"; grpDieuKien.Padding = new Padding(10);
            pnlDieuKien.Dock = DockStyle.Fill;
            lblLop.Text = "Lớp học"; lblLop.AutoSize = true; lblLop.Location = new Point(12, 25);
            _cboLop.Name = "cboLop"; _cboLop.DropDownStyle = ComboBoxStyle.DropDownList; _cboLop.SetBounds(90, 20, 360, 30); _cboLop.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnXemBaoCao.Name = "btnXemBaoCao"; btnXemBaoCao.Text = "Xem báo cáo"; btnXemBaoCao.SetBounds(465, 18, 130, 34); btnXemBaoCao.Anchor = AnchorStyles.Top | AnchorStyles.Right; btnXemBaoCao.Click += btnXemBaoCao_Click;
            pnlDieuKien.Controls.AddRange(new Control[] { lblLop, _cboLop, btnXemBaoCao }); grpDieuKien.Controls.Add(pnlDieuKien);
            grpKetQua.Dock = DockStyle.Fill; grpKetQua.Text = "Báo cáo doanh thu trung tâm theo lớp"; grpKetQua.Padding = new Padding(8);
            _dgvBaoCao.Name = "dgvBaoCao"; _dgvBaoCao.Dock = DockStyle.Fill; _dgvBaoCao.ReadOnly = true; _dgvBaoCao.AllowUserToAddRows = false; _dgvBaoCao.AllowUserToDeleteRows = false; _dgvBaoCao.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; grpKetQua.Controls.Add(_dgvBaoCao);
            _lblTong.Name = "lblTong"; _lblTong.Dock = DockStyle.Bottom; _lblTong.Height = 38; _lblTong.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(grpKetQua); Controls.Add(_lblTong); Controls.Add(grpDieuKien);
            AutoScaleDimensions = new SizeF(8F, 20F); AutoScaleMode = AutoScaleMode.Font; ClientSize = new Size(900, 600); MinimumSize = new Size(700, 450); Name = "frmBaoCaoDoanhThu"; Text = "Báo cáo doanh thu trung tâm theo lớp";
            ((System.ComponentModel.ISupportInitialize)_dgvBaoCao).EndInit(); ResumeLayout(false);
        }
        protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
    }
}
