using BTL_LTTQ;

namespace BTL_LTTQ.Forms.QuanLy
{
    partial class frmDanhMucKhac
    {
        private System.ComponentModel.IContainer? components = null;
        private ComboBox cboDanhMuc;
        private GroupBox grpThongTin;
        private DataGridView dgvDanhMuc;
        private FlowLayoutPanel pnlButtons;
        private AppButton btnThem;
        private AppButton btnSua;
        private AppButton btnXoa;
        private AppButton btnLamMoi;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            cboDanhMuc = new ComboBox();
            grpThongTin = new GroupBox();
            dgvDanhMuc = new DataGridView();
            pnlButtons = new FlowLayoutPanel();
            btnThem = new AppButton();
            btnSua = new AppButton();
            btnXoa = new AppButton();
            btnLamMoi = new AppButton();
            ((System.ComponentModel.ISupportInitialize)dgvDanhMuc).BeginInit();
            pnlButtons.SuspendLayout();
            SuspendLayout();
            // 
            // cboDanhMuc
            // 
            cboDanhMuc.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDanhMuc.Location = new Point(24, 20);
            cboDanhMuc.Name = "cboDanhMuc";
            cboDanhMuc.Size = new Size(320, 28);
            cboDanhMuc.TabIndex = 0;
            cboDanhMuc.SelectedIndexChanged += cboDanhMuc_SelectedIndexChanged;
            // 
            // grpThongTin
            // 
            grpThongTin.Location = new Point(24, 60);
            grpThongTin.Name = "grpThongTin";
            grpThongTin.Size = new Size(1152, 220);
            grpThongTin.TabIndex = 1;
            grpThongTin.TabStop = false;
            grpThongTin.Text = "Thông tin danh mục";
            grpThongTin.Enter += grpThongTin_Enter;
            // 
            // dgvDanhMuc
            // 
            dgvDanhMuc.AllowUserToAddRows = false;
            dgvDanhMuc.AllowUserToDeleteRows = false;
            dgvDanhMuc.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDanhMuc.ColumnHeadersHeight = 29;
            dgvDanhMuc.Location = new Point(24, 346);
            dgvDanhMuc.MultiSelect = false;
            dgvDanhMuc.Name = "dgvDanhMuc";
            dgvDanhMuc.ReadOnly = true;
            dgvDanhMuc.RowHeadersVisible = false;
            dgvDanhMuc.RowHeadersWidth = 51;
            dgvDanhMuc.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDanhMuc.Size = new Size(1152, 300);
            dgvDanhMuc.TabIndex = 3;
            dgvDanhMuc.SelectionChanged += dgvDanhMuc_SelectionChanged;
            // 
            // pnlButtons
            // 
            pnlButtons.Controls.Add(btnThem);
            pnlButtons.Controls.Add(btnSua);
            pnlButtons.Controls.Add(btnXoa);
            pnlButtons.Controls.Add(btnLamMoi);
            pnlButtons.Location = new Point(24, 290);
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Size = new Size(1152, 44);
            pnlButtons.TabIndex = 2;
            // 
            // btnThem
            // 
            btnThem.BackColor = Color.White;
            btnThem.FlatAppearance.BorderColor = Color.FromArgb(200, 208, 220);
            btnThem.FlatAppearance.MouseDownBackColor = Color.FromArgb(234, 241, 250);
            btnThem.FlatAppearance.MouseOverBackColor = Color.FromArgb(234, 241, 250);
            btnThem.FlatStyle = FlatStyle.Flat;
            btnThem.ForeColor = Color.FromArgb(33, 37, 41);
            btnThem.Location = new Point(3, 3);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(100, 36);
            btnThem.TabIndex = 0;
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = false;
            btnThem.Click += btnThem_Click;
            // 
            // btnSua
            // 
            btnSua.BackColor = Color.White;
            btnSua.FlatAppearance.BorderColor = Color.FromArgb(200, 208, 220);
            btnSua.FlatAppearance.MouseDownBackColor = Color.FromArgb(234, 241, 250);
            btnSua.FlatAppearance.MouseOverBackColor = Color.FromArgb(234, 241, 250);
            btnSua.FlatStyle = FlatStyle.Flat;
            btnSua.ForeColor = Color.FromArgb(33, 37, 41);
            btnSua.Location = new Point(109, 3);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(100, 36);
            btnSua.TabIndex = 1;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = false;
            btnSua.Click += btnSua_Click;
            // 
            // btnXoa
            // 
            btnXoa.BackColor = Color.White;
            btnXoa.FlatAppearance.BorderColor = Color.FromArgb(200, 208, 220);
            btnXoa.FlatAppearance.MouseDownBackColor = Color.FromArgb(234, 241, 250);
            btnXoa.FlatAppearance.MouseOverBackColor = Color.FromArgb(234, 241, 250);
            btnXoa.FlatStyle = FlatStyle.Flat;
            btnXoa.ForeColor = Color.FromArgb(33, 37, 41);
            btnXoa.Location = new Point(215, 3);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(100, 36);
            btnXoa.TabIndex = 2;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = false;
            btnXoa.Click += btnXoa_Click;
            // 
            // btnLamMoi
            // 
            btnLamMoi.BackColor = Color.White;
            btnLamMoi.FlatAppearance.BorderColor = Color.FromArgb(200, 208, 220);
            btnLamMoi.FlatAppearance.MouseDownBackColor = Color.FromArgb(234, 241, 250);
            btnLamMoi.FlatAppearance.MouseOverBackColor = Color.FromArgb(234, 241, 250);
            btnLamMoi.FlatStyle = FlatStyle.Flat;
            btnLamMoi.ForeColor = Color.FromArgb(33, 37, 41);
            btnLamMoi.Location = new Point(321, 3);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(110, 36);
            btnLamMoi.TabIndex = 3;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = false;
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // frmDanhMucKhac
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 247, 251);
            ClientSize = new Size(1200, 680);
            Controls.Add(cboDanhMuc);
            Controls.Add(grpThongTin);
            Controls.Add(pnlButtons);
            Controls.Add(dgvDanhMuc);
            Name = "frmDanhMucKhac";
            Text = "Danh mục khác";
            Load += frmDanhMucKhac_Load;
            ((System.ComponentModel.ISupportInitialize)dgvDanhMuc).EndInit();
            pnlButtons.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
