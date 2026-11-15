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
        private Panel pnlDieuKien = null!;
        private Label lblLop = null!;
        private void InitializeComponent()
        {
            _cboLop = new ComboBox();
            btnXemBaoCao = new Button();
            _dgvBaoCao = new DataGridView();
            _lblTong = new Label();
            grpDieuKien = new GroupBox();
            pnlDieuKien = new Panel();
            lblLop = new Label();
            grpKetQua = new GroupBox();
            ((System.ComponentModel.ISupportInitialize)_dgvBaoCao).BeginInit();
            grpDieuKien.SuspendLayout();
            pnlDieuKien.SuspendLayout();
            grpKetQua.SuspendLayout();
            SuspendLayout();
            // 
            // _cboLop
            // 
            _cboLop.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            _cboLop.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboLop.Location = new Point(90, 20);
            _cboLop.Name = "_cboLop";
            _cboLop.Size = new Size(360, 28);
            _cboLop.TabIndex = 1;
            // 
            // btnXemBaoCao
            // 
            btnXemBaoCao.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnXemBaoCao.Location = new Point(465, 18);
            btnXemBaoCao.Name = "btnXemBaoCao";
            btnXemBaoCao.Size = new Size(130, 34);
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
            _dgvBaoCao.Size = new Size(884, 430);
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
            grpDieuKien.Size = new Size(900, 96);
            grpDieuKien.TabIndex = 2;
            grpDieuKien.TabStop = false;
            grpDieuKien.Text = "Điều kiện báo cáo";
            // 
            // pnlDieuKien
            // 
            pnlDieuKien.Controls.Add(lblLop);
            pnlDieuKien.Controls.Add(_cboLop);
            pnlDieuKien.Controls.Add(btnXemBaoCao);
            pnlDieuKien.Dock = DockStyle.Fill;
            pnlDieuKien.Location = new Point(10, 30);
            pnlDieuKien.Name = "pnlDieuKien";
            pnlDieuKien.Size = new Size(880, 56);
            pnlDieuKien.TabIndex = 0;
            // 
            // lblLop
            // 
            lblLop.AutoSize = true;
            lblLop.Location = new Point(12, 25);
            lblLop.Name = "lblLop";
            lblLop.Size = new Size(62, 20);
            lblLop.TabIndex = 0;
            lblLop.Text = "Lớp học";
            // 
            // grpKetQua
            // 
            grpKetQua.Controls.Add(_dgvBaoCao);
            grpKetQua.Dock = DockStyle.Fill;
            grpKetQua.Location = new Point(0, 96);
            grpKetQua.Name = "grpKetQua";
            grpKetQua.Padding = new Padding(8);
            grpKetQua.Size = new Size(900, 466);
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
            MinimumSize = new Size(700, 450);
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
