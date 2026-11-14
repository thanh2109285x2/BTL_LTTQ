namespace BTL_LTTQ.Forms.Automation
{
    partial class FrmRuleManagement
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblTimKieu = new System.Windows.Forms.Label();
            this.TxtTimKieu = new System.Windows.Forms.TextBox();
            this.lblLocTrigger = new System.Windows.Forms.Label();
            this.CboLocTrigger = new System.Windows.Forms.ComboBox();
            this.lblLocTrangThai = new System.Windows.Forms.Label();
            this.CboLocTrangThai = new System.Windows.Forms.ComboBox();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.BtnThemMoi = new System.Windows.Forms.Button();
            this.BtnSua = new System.Windows.Forms.Button();
            this.BtnXoaMem = new System.Windows.Forms.Button();
            this.BtnDoiTrangThai = new System.Windows.Forms.Button();
            this.BtnXemChiTiet = new System.Windows.Forms.Button();
            this.dgvDanhSach = new System.Windows.Forms.DataGridView();
            this.panelTop.SuspendLayout();
            this.panelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDanhSach)).BeginInit();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.lblTimKieu);
            this.panelTop.Controls.Add(this.TxtTimKieu);
            this.panelTop.Controls.Add(this.lblLocTrigger);
            this.panelTop.Controls.Add(this.CboLocTrigger);
            this.panelTop.Controls.Add(this.lblLocTrangThai);
            this.panelTop.Controls.Add(this.CboLocTrangThai);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(940, 55);
            this.panelTop.TabIndex = 0;
            // 
            // lblTimKieu
            // 
            this.lblTimKieu.AutoSize = true;
            this.lblTimKieu.Location = new System.Drawing.Point(12, 18);
            this.lblTimKieu.Name = "lblTimKieu";
            this.lblTimKieu.Size = new System.Drawing.Size(55, 15);
            this.lblTimKieu.TabIndex = 0;
            this.lblTimKieu.Text = "Tìm kiếm:";
            // 
            // TxtTimKieu
            // 
            this.TxtTimKieu.Location = new System.Drawing.Point(73, 15);
            this.TxtTimKieu.Name = "TxtTimKieu";
            this.TxtTimKieu.Size = new System.Drawing.Size(240, 23);
            this.TxtTimKieu.TabIndex = 1;
            this.TxtTimKieu.TextChanged += new System.EventHandler(this.TxtTimKieu_TextChanged);
            // 
            // lblLocTrigger
            // 
            this.lblLocTrigger.AutoSize = true;
            this.lblLocTrigger.Location = new System.Drawing.Point(335, 18);
            this.lblLocTrigger.Name = "lblLocTrigger";
            this.lblLocTrigger.Size = new System.Drawing.Size(48, 15);
            this.lblLocTrigger.TabIndex = 2;
            this.lblLocTrigger.Text = "Trigger:";
            // 
            // CboLocTrigger
            // 
            this.CboLocTrigger.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CboLocTrigger.FormattingEnabled = true;
            this.CboLocTrigger.Location = new System.Drawing.Point(389, 15);
            this.CboLocTrigger.Name = "CboLocTrigger";
            this.CboLocTrigger.Size = new System.Drawing.Size(210, 23);
            this.CboLocTrigger.TabIndex = 3;
            this.CboLocTrigger.SelectedIndexChanged += new System.EventHandler(this.CboLocTrigger_SelectedIndexChanged);
            // 
            // lblLocTrangThai
            // 
            this.lblLocTrangThai.AutoSize = true;
            this.lblLocTrangThai.Location = new System.Drawing.Point(620, 18);
            this.lblLocTrangThai.Name = "lblLocTrangThai";
            this.lblLocTrangThai.Size = new System.Drawing.Size(64, 15);
            this.lblLocTrangThai.TabIndex = 4;
            this.lblLocTrangThai.Text = "Trạng thái:";
            // 
            // CboLocTrangThai
            // 
            this.CboLocTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CboLocTrangThai.FormattingEnabled = true;
            this.CboLocTrangThai.Location = new System.Drawing.Point(690, 15);
            this.CboLocTrangThai.Name = "CboLocTrangThai";
            this.CboLocTrangThai.Size = new System.Drawing.Size(160, 23);
            this.CboLocTrangThai.TabIndex = 5;
            this.CboLocTrangThai.SelectedIndexChanged += new System.EventHandler(this.CboLocTrangThai_SelectedIndexChanged);
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.BtnThemMoi);
            this.panelButtons.Controls.Add(this.BtnSua);
            this.panelButtons.Controls.Add(this.BtnXoaMem);
            this.panelButtons.Controls.Add(this.BtnDoiTrangThai);
            this.panelButtons.Controls.Add(this.BtnXemChiTiet);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(0, 500);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(940, 50);
            this.panelButtons.TabIndex = 2;
            // 
            // BtnThemMoi
            // 
            this.BtnThemMoi.Location = new System.Drawing.Point(12, 10);
            this.BtnThemMoi.Name = "BtnThemMoi";
            this.BtnThemMoi.Size = new System.Drawing.Size(110, 30);
            this.BtnThemMoi.TabIndex = 0;
            this.BtnThemMoi.Text = "Thêm Rule";
            this.BtnThemMoi.UseVisualStyleBackColor = true;
            this.BtnThemMoi.Click += new System.EventHandler(this.BtnThemMoi_Click);
            // 
            // BtnSua
            // 
            this.BtnSua.Location = new System.Drawing.Point(128, 10);
            this.BtnSua.Name = "BtnSua";
            this.BtnSua.Size = new System.Drawing.Size(90, 30);
            this.BtnSua.TabIndex = 1;
            this.BtnSua.Text = "Sửa";
            this.BtnSua.UseVisualStyleBackColor = true;
            this.BtnSua.Click += new System.EventHandler(this.BtnSua_Click);
            // 
            // BtnXoaMem
            // 
            this.BtnXoaMem.Location = new System.Drawing.Point(224, 10);
            this.BtnXoaMem.Name = "BtnXoaMem";
            this.BtnXoaMem.Size = new System.Drawing.Size(100, 30);
            this.BtnXoaMem.TabIndex = 2;
            this.BtnXoaMem.Text = "Xóa mềm";
            this.BtnXoaMem.UseVisualStyleBackColor = true;
            this.BtnXoaMem.Click += new System.EventHandler(this.BtnXoaMem_Click);
            // 
            // BtnDoiTrangThai
            // 
            this.BtnDoiTrangThai.Location = new System.Drawing.Point(330, 10);
            this.BtnDoiTrangThai.Name = "BtnDoiTrangThai";
            this.BtnDoiTrangThai.Size = new System.Drawing.Size(110, 30);
            this.BtnDoiTrangThai.TabIndex = 3;
            this.BtnDoiTrangThai.Text = "Bật / Tắt";
            this.BtnDoiTrangThai.UseVisualStyleBackColor = true;
            this.BtnDoiTrangThai.Click += new System.EventHandler(this.BtnDoiTrangThai_Click);
            // 
            // BtnXemChiTiet
            // 
            this.BtnXemChiTiet.Location = new System.Drawing.Point(446, 10);
            this.BtnXemChiTiet.Name = "BtnXemChiTiet";
            this.BtnXemChiTiet.Size = new System.Drawing.Size(110, 30);
            this.BtnXemChiTiet.TabIndex = 4;
            this.BtnXemChiTiet.Text = "Xem chi tiết";
            this.BtnXemChiTiet.UseVisualStyleBackColor = true;
            this.BtnXemChiTiet.Click += new System.EventHandler(this.BtnXemChiTiet_Click);
            // 
            // dgvDanhSach
            // 
            this.dgvDanhSach.AllowUserToAddRows = false;
            this.dgvDanhSach.AllowUserToDeleteRows = false;
            this.dgvDanhSach.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvDanhSach.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvDanhSach.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDanhSach.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDanhSach.Location = new System.Drawing.Point(0, 55);
            this.dgvDanhSach.MultiSelect = false;
            this.dgvDanhSach.Name = "dgvDanhSach";
            this.dgvDanhSach.ReadOnly = true;
            this.dgvDanhSach.RowHeadersVisible = false;
            this.dgvDanhSach.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDanhSach.Size = new System.Drawing.Size(940, 445);
            this.dgvDanhSach.TabIndex = 1;
            // 
            // FrmRuleManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(940, 550);
            this.Controls.Add(this.dgvDanhSach);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.panelTop);
            this.MinimumSize = new System.Drawing.Size(800, 450);
            this.Name = "FrmRuleManagement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Quản lý Rule tự động hóa";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDanhSach)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTimKieu;
        private System.Windows.Forms.TextBox TxtTimKieu;
        private System.Windows.Forms.Label lblLocTrigger;
        private System.Windows.Forms.ComboBox CboLocTrigger;
        private System.Windows.Forms.Label lblLocTrangThai;
        private System.Windows.Forms.ComboBox CboLocTrangThai;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button BtnThemMoi;
        private System.Windows.Forms.Button BtnSua;
        private System.Windows.Forms.Button BtnXoaMem;
        private System.Windows.Forms.Button BtnDoiTrangThai;
        private System.Windows.Forms.Button BtnXemChiTiet;
        private System.Windows.Forms.DataGridView dgvDanhSach;
    }
}
