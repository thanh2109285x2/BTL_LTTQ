using BTL_LTTQ;

namespace BTL_LTTQ.Forms.HeThong
{
    partial class frmPhanQuyen
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            layoutRoot = new TableLayoutPanel();
            panelButtons = new FlowLayoutPanel();
            lblGiaiThich = new Label();
            dgvPhanQuyen = new DataGridView();
            colChucNang = new DataGridViewTextBoxColumn();
            colGiaoVien = new DataGridViewCheckBoxColumn();
            colGiaoVienChiCuaMinh = new DataGridViewCheckBoxColumn();
            colHocVien = new DataGridViewCheckBoxColumn();
            colHocVienChiCuaMinh = new DataGridViewCheckBoxColumn();
            btnLuu = new AppButton { Kieu = KieuNut.Chinh };
            btnHoanTac = new AppButton { Kieu = KieuNut.Phu };
            btnDong = new AppButton { Kieu = KieuNut.Phu };
            ((System.ComponentModel.ISupportInitialize)dgvPhanQuyen).BeginInit();
            layoutRoot.SuspendLayout();
            panelButtons.SuspendLayout();
            SuspendLayout();
            // 
            // layoutRoot
            // 
            layoutRoot.ColumnCount = 1;
            layoutRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layoutRoot.Controls.Add(lblGiaiThich, 0, 0);
            layoutRoot.Controls.Add(dgvPhanQuyen, 0, 1);
            layoutRoot.Controls.Add(panelButtons, 0, 2);
            layoutRoot.Dock = DockStyle.Fill;
            layoutRoot.Location = new Point(0, 0);
            layoutRoot.Name = "layoutRoot";
            layoutRoot.Padding = new Padding(12);
            layoutRoot.RowCount = 3;
            layoutRoot.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layoutRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layoutRoot.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layoutRoot.Size = new Size(1100, 680);
            layoutRoot.TabIndex = 0;
            // 
            // panelButtons
            // 
            panelButtons.AutoSize = true;
            panelButtons.Controls.Add(btnDong);
            panelButtons.Controls.Add(btnHoanTac);
            panelButtons.Controls.Add(btnLuu);
            panelButtons.Dock = DockStyle.Fill;
            panelButtons.FlowDirection = FlowDirection.RightToLeft;
            panelButtons.Location = new Point(15, 618);
            panelButtons.Name = "panelButtons";
            panelButtons.Padding = new Padding(0, 10, 0, 0);
            panelButtons.Size = new Size(1070, 51);
            panelButtons.TabIndex = 2;
            panelButtons.WrapContents = false;
            // 
            // lblGiaiThich
            // 
            lblGiaiThich.AutoSize = true;
            lblGiaiThich.Dock = DockStyle.Fill;
            lblGiaiThich.Margin = new Padding(3, 3, 3, 10);
            lblGiaiThich.Location = new Point(15, 15);
            lblGiaiThich.Name = "lblGiaiThich";
            lblGiaiThich.Size = new Size(800, 20);
            lblGiaiThich.TabIndex = 0;
            lblGiaiThich.Text = "Không tích = chỉ Admin; chỉ ô chính = dữ liệu của tất cả; cả hai = chỉ dữ liệu của chính mình.";
            // 
            // dgvPhanQuyen
            // 
            dgvPhanQuyen.AllowUserToAddRows = false;
            dgvPhanQuyen.AllowUserToDeleteRows = false;
            dgvPhanQuyen.AllowUserToResizeRows = false;
            dgvPhanQuyen.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPhanQuyen.BackgroundColor = Color.White;
            dgvPhanQuyen.BorderStyle = BorderStyle.None;
            dgvPhanQuyen.ColumnHeadersHeight = 34;
            dgvPhanQuyen.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvPhanQuyen.Columns.AddRange(new DataGridViewColumn[] { colChucNang, colGiaoVien, colGiaoVienChiCuaMinh, colHocVien, colHocVienChiCuaMinh });
            dgvPhanQuyen.Dock = DockStyle.Fill;
            dgvPhanQuyen.EnableHeadersVisualStyles = false;
            dgvPhanQuyen.GridColor = Color.FromArgb(200, 208, 220);
            dgvPhanQuyen.Location = new Point(15, 45);
            dgvPhanQuyen.Margin = new Padding(3);
            dgvPhanQuyen.MultiSelect = false;
            dgvPhanQuyen.Name = "dgvPhanQuyen";
            dgvPhanQuyen.RowHeadersVisible = false;
            dgvPhanQuyen.RowTemplate.Height = 28;
            dgvPhanQuyen.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvPhanQuyen.Size = new Size(1068, 553);
            dgvPhanQuyen.TabIndex = 1;
            dgvPhanQuyen.CurrentCellDirtyStateChanged += Grid_CurrentCellDirtyStateChanged;
            dgvPhanQuyen.CellValueChanged += Grid_CellValueChanged;
            dgvPhanQuyen.CellClick += Grid_CellClick;
            // 
            // colChucNang
            // 
            colChucNang.FillWeight = 180F;
            colChucNang.HeaderText = "Chức năng";
            colChucNang.Name = "colChucNang";
            colChucNang.ReadOnly = true;
            // 
            // colGiaoVien
            // 
            colGiaoVien.FillWeight = 90F;
            colGiaoVien.HeaderText = "Giáo viên";
            colGiaoVien.Name = "colGiaoVien";
            colGiaoVien.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // colGiaoVienChiCuaMinh
            // 
            colGiaoVienChiCuaMinh.FillWeight = 125F;
            colGiaoVienChiCuaMinh.HeaderText = "Giáo viên - chỉ của mình";
            colGiaoVienChiCuaMinh.Name = "colGiaoVienChiCuaMinh";
            colGiaoVienChiCuaMinh.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // colHocVien
            // 
            colHocVien.FillWeight = 90F;
            colHocVien.HeaderText = "Học viên";
            colHocVien.Name = "colHocVien";
            colHocVien.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // colHocVienChiCuaMinh
            // 
            colHocVienChiCuaMinh.FillWeight = 125F;
            colHocVienChiCuaMinh.HeaderText = "Học viên - chỉ của mình";
            colHocVienChiCuaMinh.Name = "colHocVienChiCuaMinh";
            colHocVienChiCuaMinh.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // btnLuu
            // 
            btnLuu.BackColor = Color.FromArgb(41, 98, 168);
            btnLuu.FlatStyle = FlatStyle.Flat;
            btnLuu.ForeColor = Color.White;
            btnLuu.Location = new Point(955, 13);
            btnLuu.Name = "btnLuu";
            btnLuu.Size = new Size(112, 38);
            btnLuu.TabIndex = 2;
            btnLuu.Text = "Lưu";
            btnLuu.UseVisualStyleBackColor = false;
            btnLuu.Click += btnLuu_Click;
            // 
            // btnHoanTac
            // 
            btnHoanTac.Location = new Point(825, 13);
            btnHoanTac.Name = "btnHoanTac";
            btnHoanTac.Size = new Size(124, 38);
            btnHoanTac.TabIndex = 3;
            btnHoanTac.Text = "Hoàn tác";
            btnHoanTac.UseVisualStyleBackColor = true;
            btnHoanTac.Click += btnHoanTac_Click;
            // 
            // btnDong
            // 
            btnDong.Location = new Point(707, 13);
            btnDong.Name = "btnDong";
            btnDong.Size = new Size(112, 38);
            btnDong.TabIndex = 4;
            btnDong.Text = "Đóng";
            btnDong.UseVisualStyleBackColor = true;
            btnDong.Click += btnDong_Click;
            // 
            // frmPhanQuyen
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 247, 251);
            ClientSize = new Size(1100, 680);
            Controls.Add(layoutRoot);
            MinimumSize = new Size(850, 500);
            Name = "frmPhanQuyen";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Quản lý phân quyền";
            FormClosing += frmPhanQuyen_FormClosing;
            Load += frmPhanQuyen_Load;
            ((System.ComponentModel.ISupportInitialize)dgvPhanQuyen).EndInit();
            panelButtons.ResumeLayout(false);
            panelButtons.PerformLayout();
            layoutRoot.ResumeLayout(false);
            layoutRoot.PerformLayout();
            ResumeLayout(false);
        }

        private TableLayoutPanel layoutRoot;
        private FlowLayoutPanel panelButtons;
        private Label lblGiaiThich;
        private DataGridView dgvPhanQuyen;
        private DataGridViewTextBoxColumn colChucNang;
        private DataGridViewCheckBoxColumn colGiaoVien;
        private DataGridViewCheckBoxColumn colGiaoVienChiCuaMinh;
        private DataGridViewCheckBoxColumn colHocVien;
        private DataGridViewCheckBoxColumn colHocVienChiCuaMinh;
        private AppButton btnLuu;
        private AppButton btnHoanTac;
        private AppButton btnDong;
    }
}
