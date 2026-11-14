#nullable enable
namespace BTL_LTTQ.Forms.TraCuu
{
    partial class frmTimKiemHocVien
    {
        private System.ComponentModel.IContainer? components = null;
        private TextBox txtTuKhoa = null!;
        private ComboBox cboLop = null!;
        private ComboBox cboNghe = null!;
        private ComboBox cboXepLoai = null!;
        private DataGridView dgvKetQua = null!;
        private Button btnTimKiem = null!;
        private Button btnLamMoi = null!;
        private GroupBox grpDieuKien = null!;
        private GroupBox grpKetQua = null!;
        private Panel pnlNut = null!;
        private Label lblTuKhoa = null!;
        private Label lblLop = null!;
        private Label lblNghe = null!;
        private Label lblXepLoai = null!;

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            txtTuKhoa = new TextBox(); cboLop = new ComboBox(); cboNghe = new ComboBox(); cboXepLoai = new ComboBox(); dgvKetQua = new DataGridView(); btnTimKiem = new Button(); btnLamMoi = new Button();
            grpDieuKien = new GroupBox(); grpKetQua = new GroupBox(); pnlNut = new Panel();
            lblTuKhoa = new Label(); lblLop = new Label(); lblNghe = new Label(); lblXepLoai = new Label();
            grpDieuKien.Name = "grpDieuKien"; grpKetQua.Name = "grpKetQua"; pnlNut.Name = "pnlNut";
            lblTuKhoa.Name = "lblTuKhoa"; lblLop.Name = "lblLop"; lblNghe.Name = "lblNghe"; lblXepLoai.Name = "lblXepLoai";
            ((System.ComponentModel.ISupportInitialize)dgvKetQua).BeginInit(); SuspendLayout();
            grpDieuKien.Dock = DockStyle.Top; grpDieuKien.Height = 150; grpDieuKien.Text = "Điều kiện tra cứu"; grpDieuKien.Padding = new Padding(10);
            lblTuKhoa.Text = "Mã, tên hoặc điện thoại"; lblTuKhoa.AutoSize = true; lblTuKhoa.Location = new Point(18, 34);
            txtTuKhoa.Name = "txtTuKhoa"; txtTuKhoa.SetBounds(190, 29, 240, 28); txtTuKhoa.KeyDown += txtTuKhoa_KeyDown;
            lblLop.Text = "Lớp học"; lblLop.AutoSize = true; lblLop.Location = new Point(465, 34);
            cboLop.Name = "cboLop"; cboLop.DropDownStyle = ComboBoxStyle.DropDownList; cboLop.SetBounds(545, 29, 250, 28); cboLop.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblNghe.Text = "Nghề nghiệp"; lblNghe.AutoSize = true; lblNghe.Location = new Point(18, 88);
            cboNghe.Name = "cboNghe"; cboNghe.DropDownStyle = ComboBoxStyle.DropDownList; cboNghe.SetBounds(190, 83, 240, 28);
            lblXepLoai.Text = "Xếp loại"; lblXepLoai.AutoSize = true; lblXepLoai.Location = new Point(465, 88);
            cboXepLoai.Name = "cboXepLoai"; cboXepLoai.DropDownStyle = ComboBoxStyle.DropDownList; cboXepLoai.SetBounds(545, 83, 250, 28); cboXepLoai.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpDieuKien.Controls.AddRange(new Control[] { lblTuKhoa, txtTuKhoa, lblLop, cboLop, lblNghe, cboNghe, lblXepLoai, cboXepLoai });
            pnlNut.Dock = DockStyle.Top; pnlNut.Height = 52; pnlNut.Padding = new Padding(12, 6, 0, 0);
            btnTimKiem.Name = "btnTimKiem"; btnTimKiem.Text = "Tìm kiếm"; btnTimKiem.Size = new Size(120, 36); btnTimKiem.Click += btnTimKiem_Click;
            btnLamMoi.Name = "btnLamMoi"; btnLamMoi.Text = "Làm mới"; btnLamMoi.Size = new Size(120, 36); btnLamMoi.Location = new Point(145, 6); btnLamMoi.Click += btnLamMoi_Click;
            pnlNut.Controls.AddRange(new Control[] { btnTimKiem, btnLamMoi });
            grpKetQua.Dock = DockStyle.Fill; grpKetQua.Text = "Kết quả tra cứu"; grpKetQua.Padding = new Padding(8);
            dgvKetQua.Name = "dgvKetQua"; dgvKetQua.Dock = DockStyle.Fill; dgvKetQua.ReadOnly = true; dgvKetQua.AllowUserToAddRows = false; dgvKetQua.AllowUserToDeleteRows = false; dgvKetQua.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; grpKetQua.Controls.Add(dgvKetQua);
            Controls.Add(grpKetQua); Controls.Add(pnlNut); Controls.Add(grpDieuKien);
            AutoScaleDimensions = new SizeF(8F, 20F); AutoScaleMode = AutoScaleMode.Font; BackColor = Color.FromArgb(244, 247, 251); ClientSize = new Size(900, 620); MinimumSize = new Size(750, 500); Name = "frmTimKiemHocVien"; Text = "Tra cứu học viên";
            ((System.ComponentModel.ISupportInitialize)dgvKetQua).EndInit(); ResumeLayout(false);
        }

        protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
    }
}
