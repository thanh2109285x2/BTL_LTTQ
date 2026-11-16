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
            btnThem = new AppButton { Kieu = KieuNut.Chinh };
            btnSua = new AppButton { Kieu = KieuNut.Chinh };
            btnXoa = new AppButton { Kieu = KieuNut.NguyHiem };
            btnLamMoi = new AppButton { Kieu = KieuNut.Phu };
            ((System.ComponentModel.ISupportInitialize)dgvDanhMuc).BeginInit();
            SuspendLayout();
            cboDanhMuc.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDanhMuc.Location = new Point(24, 20);
            cboDanhMuc.Name = "cboDanhMuc";
            cboDanhMuc.Size = new Size(320, 28);
            cboDanhMuc.SelectedIndexChanged += cboDanhMuc_SelectedIndexChanged;
            grpThongTin.Location = new Point(24, 60);
            grpThongTin.Name = "grpThongTin";
            grpThongTin.Size = new Size(1152, 220);
            grpThongTin.TabStop = false;
            grpThongTin.Text = "Thông tin danh mục";
            pnlButtons.FlowDirection = FlowDirection.LeftToRight;
            pnlButtons.Location = new Point(24, 290);
            pnlButtons.Size = new Size(1152, 44);
            pnlButtons.Controls.Add(btnThem);
            pnlButtons.Controls.Add(btnSua);
            pnlButtons.Controls.Add(btnXoa);
            pnlButtons.Controls.Add(btnLamMoi);
            btnThem.Size = new Size(100, 36);
            btnThem.Text = "Thêm";
            btnThem.Click += btnThem_Click;
            btnSua.Size = new Size(100, 36);
            btnSua.Text = "Sửa";
            btnSua.Click += btnSua_Click;
            btnXoa.Size = new Size(100, 36);
            btnXoa.Text = "Xóa";
            btnXoa.Click += btnXoa_Click;
            btnLamMoi.Size = new Size(110, 36);
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.Click += btnLamMoi_Click;
            dgvDanhMuc.AllowUserToAddRows = false;
            dgvDanhMuc.AllowUserToDeleteRows = false;
            dgvDanhMuc.AutoGenerateColumns = true;
            dgvDanhMuc.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDanhMuc.Location = new Point(24, 346);
            dgvDanhMuc.MultiSelect = false;
            dgvDanhMuc.Name = "dgvDanhMuc";
            dgvDanhMuc.ReadOnly = true;
            dgvDanhMuc.RowHeadersVisible = false;
            dgvDanhMuc.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDanhMuc.Size = new Size(1152, 300);
            dgvDanhMuc.SelectionChanged += dgvDanhMuc_SelectionChanged;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = GiaoDien.MauNen;
            ClientSize = new Size(1200, 680);
            Controls.Add(cboDanhMuc);
            Controls.Add(grpThongTin);
            Controls.Add(pnlButtons);
            Controls.Add(dgvDanhMuc);
            Name = "frmDanhMucKhac";
            Text = "Danh mục khác";
            Load += frmDanhMucKhac_Load;
            ((System.ComponentModel.ISupportInitialize)dgvDanhMuc).EndInit();
            ResumeLayout(false);
        }
    }
}
