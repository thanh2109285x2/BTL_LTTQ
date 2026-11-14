namespace BTL_LTTQ.Forms.Automation
{
    partial class FrmRuleEditor
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
            this.lblTrangThai = new System.Windows.Forms.Label();
            this.CboTrangThai = new System.Windows.Forms.ComboBox();
            this.lblTrigger = new System.Windows.Forms.Label();
            this.CboTrigger = new System.Windows.Forms.ComboBox();
            this.lblMoTa = new System.Windows.Forms.Label();
            this.txtMoTa = new System.Windows.Forms.TextBox();
            this.lblTenRule = new System.Windows.Forms.Label();
            this.txtTenRule = new System.Windows.Forms.TextBox();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.BtnHuy = new System.Windows.Forms.Button();
            this.BtnLuu = new System.Windows.Forms.Button();
            this.BtnTestRule = new System.Windows.Forms.Button();
            this.BtnValidate = new System.Windows.Forms.Button();
            this.panelBody = new System.Windows.Forms.Panel();
            this.groupHanhDong = new System.Windows.Forms.GroupBox();
            this.dgvHanhDong = new System.Windows.Forms.DataGridView();
            this.panelActionButtons = new System.Windows.Forms.Panel();
            this.BtnXoaHanhDong = new System.Windows.Forms.Button();
            this.BtnSuaHanhDong = new System.Windows.Forms.Button();
            this.BtnThemHanhDong = new System.Windows.Forms.Button();
            this.groupDieuKien = new System.Windows.Forms.GroupBox();
            this.dgvDieuKien = new System.Windows.Forms.DataGridView();
            this.panelConditionButtons = new System.Windows.Forms.Panel();
            this.BtnXoaDieuKien = new System.Windows.Forms.Button();
            this.BtnSuaDieuKien = new System.Windows.Forms.Button();
            this.BtnThemDieuKien = new System.Windows.Forms.Button();
            this.lblChonTruong = new System.Windows.Forms.Label();
            this.CboField = new System.Windows.Forms.ComboBox();
            this.panelTop.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.panelBody.SuspendLayout();
            this.groupHanhDong.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHanhDong)).BeginInit();
            this.panelActionButtons.SuspendLayout();
            this.groupDieuKien.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDieuKien)).BeginInit();
            this.panelConditionButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.lblTrangThai);
            this.panelTop.Controls.Add(this.CboTrangThai);
            this.panelTop.Controls.Add(this.lblTrigger);
            this.panelTop.Controls.Add(this.CboTrigger);
            this.panelTop.Controls.Add(this.lblMoTa);
            this.panelTop.Controls.Add(this.txtMoTa);
            this.panelTop.Controls.Add(this.lblTenRule);
            this.panelTop.Controls.Add(this.txtTenRule);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(900, 100);
            this.panelTop.TabIndex = 0;
            // 
            // lblTenRule
            // 
            this.lblTenRule.AutoSize = true;
            this.lblTenRule.Location = new System.Drawing.Point(12, 12);
            this.lblTenRule.Name = "lblTenRule";
            this.lblTenRule.Size = new System.Drawing.Size(56, 15);
            this.lblTenRule.TabIndex = 0;
            this.lblTenRule.Text = "Tên Rule:";
            // 
            // txtTenRule
            // 
            this.txtTenRule.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTenRule.Location = new System.Drawing.Point(90, 9);
            this.txtTenRule.Name = "txtTenRule";
            this.txtTenRule.Size = new System.Drawing.Size(340, 23);
            this.txtTenRule.TabIndex = 1;
            // 
            // lblMoTa
            // 
            this.lblMoTa.AutoSize = true;
            this.lblMoTa.Location = new System.Drawing.Point(450, 12);
            this.lblMoTa.Name = "lblMoTa";
            this.lblMoTa.Size = new System.Drawing.Size(42, 15);
            this.lblMoTa.TabIndex = 2;
            this.lblMoTa.Text = "Mô tả:";
            // 
            // txtMoTa
            // 
            this.txtMoTa.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMoTa.Location = new System.Drawing.Point(500, 9);
            this.txtMoTa.Name = "txtMoTa";
            this.txtMoTa.Size = new System.Drawing.Size(388, 23);
            this.txtMoTa.TabIndex = 3;
            // 
            // lblTrigger
            // 
            this.lblTrigger.AutoSize = true;
            this.lblTrigger.Location = new System.Drawing.Point(12, 55);
            this.lblTrigger.Name = "lblTrigger";
            this.lblTrigger.Size = new System.Drawing.Size(48, 15);
            this.lblTrigger.TabIndex = 4;
            this.lblTrigger.Text = "Trigger:";
            // 
            // CboTrigger
            // 
            this.CboTrigger.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CboTrigger.FormattingEnabled = true;
            this.CboTrigger.Location = new System.Drawing.Point(90, 52);
            this.CboTrigger.Name = "CboTrigger";
            this.CboTrigger.Size = new System.Drawing.Size(240, 23);
            this.CboTrigger.TabIndex = 5;
            // 
            // lblTrangThai
            // 
            this.lblTrangThai.AutoSize = true;
            this.lblTrangThai.Location = new System.Drawing.Point(360, 55);
            this.lblTrangThai.Name = "lblTrangThai";
            this.lblTrangThai.Size = new System.Drawing.Size(64, 15);
            this.lblTrangThai.TabIndex = 6;
            this.lblTrangThai.Text = "Trạng thái:";
            // 
            // CboTrangThai
            // 
            this.CboTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CboTrangThai.FormattingEnabled = true;
            this.CboTrangThai.Items.AddRange(new object[] {
            "Hoạt động",
            "Vô hiệu"});
            this.CboTrangThai.Location = new System.Drawing.Point(430, 52);
            this.CboTrangThai.Name = "CboTrangThai";
            this.CboTrangThai.Size = new System.Drawing.Size(140, 23);
            this.CboTrangThai.TabIndex = 7;
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.BtnHuy);
            this.panelBottom.Controls.Add(this.BtnLuu);
            this.panelBottom.Controls.Add(this.BtnTestRule);
            this.panelBottom.Controls.Add(this.BtnValidate);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 500);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(900, 50);
            this.panelBottom.TabIndex = 2;
            // 
            // BtnValidate
            // 
            this.BtnValidate.Location = new System.Drawing.Point(12, 10);
            this.BtnValidate.Name = "BtnValidate";
            this.BtnValidate.Size = new System.Drawing.Size(130, 30);
            this.BtnValidate.TabIndex = 0;
            this.BtnValidate.Text = "Kiểm tra cấu hình";
            this.BtnValidate.UseVisualStyleBackColor = true;
            this.BtnValidate.Click += new System.EventHandler(this.BtnValidate_Click);
            // 
            // BtnTestRule
            // 
            this.BtnTestRule.Location = new System.Drawing.Point(150, 10);
            this.BtnTestRule.Name = "BtnTestRule";
            this.BtnTestRule.Size = new System.Drawing.Size(100, 30);
            this.BtnTestRule.TabIndex = 1;
            this.BtnTestRule.Text = "Test Rule";
            this.BtnTestRule.UseVisualStyleBackColor = true;
            this.BtnTestRule.Click += new System.EventHandler(this.BtnTestRule_Click);
            // 
            // BtnLuu
            // 
            this.BtnLuu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnLuu.Location = new System.Drawing.Point(672, 10);
            this.BtnLuu.Name = "BtnLuu";
            this.BtnLuu.Size = new System.Drawing.Size(100, 30);
            this.BtnLuu.TabIndex = 2;
            this.BtnLuu.Text = "Lưu";
            this.BtnLuu.UseVisualStyleBackColor = true;
            this.BtnLuu.Click += new System.EventHandler(this.BtnLuu_Click);
            // 
            // BtnHuy
            // 
            this.BtnHuy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnHuy.Location = new System.Drawing.Point(782, 10);
            this.BtnHuy.Name = "BtnHuy";
            this.BtnHuy.Size = new System.Drawing.Size(100, 30);
            this.BtnHuy.TabIndex = 3;
            this.BtnHuy.Text = "Hủy";
            this.BtnHuy.UseVisualStyleBackColor = true;
            this.BtnHuy.Click += new System.EventHandler(this.BtnHuy_Click);
            // 
            // panelBody
            // 
            this.panelBody.Controls.Add(this.groupHanhDong);
            this.panelBody.Controls.Add(this.groupDieuKien);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(0, 100);
            this.panelBody.Name = "panelBody";
            this.panelBody.Size = new System.Drawing.Size(900, 400);
            this.panelBody.TabIndex = 1;
            // 
            // groupDieuKien
            // 
            this.groupDieuKien.Controls.Add(this.dgvDieuKien);
            this.groupDieuKien.Controls.Add(this.panelConditionButtons);
            this.groupDieuKien.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupDieuKien.Location = new System.Drawing.Point(0, 0);
            this.groupDieuKien.Name = "groupDieuKien";
            this.groupDieuKien.Size = new System.Drawing.Size(450, 400);
            this.groupDieuKien.TabIndex = 0;
            this.groupDieuKien.TabStop = false;
            this.groupDieuKien.Text = "Điều kiện (AND)";
            // 
            // dgvDieuKien
            // 
            this.dgvDieuKien.AllowUserToAddRows = false;
            this.dgvDieuKien.AllowUserToDeleteRows = false;
            this.dgvDieuKien.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvDieuKien.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDieuKien.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDieuKien.Location = new System.Drawing.Point(3, 19);
            this.dgvDieuKien.MultiSelect = false;
            this.dgvDieuKien.Name = "dgvDieuKien";
            this.dgvDieuKien.ReadOnly = true;
            this.dgvDieuKien.RowHeadersVisible = false;
            this.dgvDieuKien.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDieuKien.Size = new System.Drawing.Size(444, 328);
            this.dgvDieuKien.TabIndex = 0;
            // 
            // panelConditionButtons
            // 
            this.panelConditionButtons.Controls.Add(this.CboField);
            this.panelConditionButtons.Controls.Add(this.lblChonTruong);
            this.panelConditionButtons.Controls.Add(this.BtnXoaDieuKien);
            this.panelConditionButtons.Controls.Add(this.BtnSuaDieuKien);
            this.panelConditionButtons.Controls.Add(this.BtnThemDieuKien);
            this.panelConditionButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelConditionButtons.Location = new System.Drawing.Point(3, 317);
            this.panelConditionButtons.Name = "panelConditionButtons";
            this.panelConditionButtons.Size = new System.Drawing.Size(444, 80);
            this.panelConditionButtons.TabIndex = 1;
            // 
            // lblChonTruong
            // 
            this.lblChonTruong.AutoSize = true;
            this.lblChonTruong.Location = new System.Drawing.Point(6, 15);
            this.lblChonTruong.Name = "lblChonTruong";
            this.lblChonTruong.Size = new System.Drawing.Size(48, 15);
            this.lblChonTruong.TabIndex = 3;
            this.lblChonTruong.Text = "Trường:";
            // 
            // CboField
            // 
            this.CboField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CboField.FormattingEnabled = true;
            this.CboField.Location = new System.Drawing.Point(60, 12);
            this.CboField.Name = "CboField";
            this.CboField.Size = new System.Drawing.Size(378, 23);
            this.CboField.TabIndex = 4;
            // 
            // BtnThemDieuKien
            // 
            this.BtnThemDieuKien.Location = new System.Drawing.Point(6, 44);
            this.BtnThemDieuKien.Name = "BtnThemDieuKien";
            this.BtnThemDieuKien.Size = new System.Drawing.Size(120, 30);
            this.BtnThemDieuKien.TabIndex = 0;
            this.BtnThemDieuKien.Text = "Thêm điều kiện";
            this.BtnThemDieuKien.UseVisualStyleBackColor = true;
            this.BtnThemDieuKien.Click += new System.EventHandler(this.BtnThemDieuKien_Click);
            // 
            // BtnSuaDieuKien
            // 
            this.BtnSuaDieuKien.Location = new System.Drawing.Point(132, 44);
            this.BtnSuaDieuKien.Name = "BtnSuaDieuKien";
            this.BtnSuaDieuKien.Size = new System.Drawing.Size(100, 30);
            this.BtnSuaDieuKien.TabIndex = 1;
            this.BtnSuaDieuKien.Text = "Sửa";
            this.BtnSuaDieuKien.UseVisualStyleBackColor = true;
            this.BtnSuaDieuKien.Click += new System.EventHandler(this.BtnSuaDieuKien_Click);
            // 
            // BtnXoaDieuKien
            // 
            this.BtnXoaDieuKien.Location = new System.Drawing.Point(238, 44);
            this.BtnXoaDieuKien.Name = "BtnXoaDieuKien";
            this.BtnXoaDieuKien.Size = new System.Drawing.Size(100, 30);
            this.BtnXoaDieuKien.TabIndex = 2;
            this.BtnXoaDieuKien.Text = "Xóa";
            this.BtnXoaDieuKien.UseVisualStyleBackColor = true;
            this.BtnXoaDieuKien.Click += new System.EventHandler(this.BtnXoaDieuKien_Click);
            // 
            // groupHanhDong
            // 
            this.groupHanhDong.Controls.Add(this.dgvHanhDong);
            this.groupHanhDong.Controls.Add(this.panelActionButtons);
            this.groupHanhDong.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupHanhDong.Location = new System.Drawing.Point(450, 0);
            this.groupHanhDong.Name = "groupHanhDong";
            this.groupHanhDong.Size = new System.Drawing.Size(450, 400);
            this.groupHanhDong.TabIndex = 1;
            this.groupHanhDong.TabStop = false;
            this.groupHanhDong.Text = "Hành động";
            // 
            // dgvHanhDong
            // 
            this.dgvHanhDong.AllowUserToAddRows = false;
            this.dgvHanhDong.AllowUserToDeleteRows = false;
            this.dgvHanhDong.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvHanhDong.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHanhDong.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHanhDong.Location = new System.Drawing.Point(3, 19);
            this.dgvHanhDong.MultiSelect = false;
            this.dgvHanhDong.Name = "dgvHanhDong";
            this.dgvHanhDong.ReadOnly = true;
            this.dgvHanhDong.RowHeadersVisible = false;
            this.dgvHanhDong.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHanhDong.Size = new System.Drawing.Size(444, 328);
            this.dgvHanhDong.TabIndex = 0;
            // 
            // panelActionButtons
            // 
            this.panelActionButtons.Controls.Add(this.BtnXoaHanhDong);
            this.panelActionButtons.Controls.Add(this.BtnSuaHanhDong);
            this.panelActionButtons.Controls.Add(this.BtnThemHanhDong);
            this.panelActionButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelActionButtons.Location = new System.Drawing.Point(3, 347);
            this.panelActionButtons.Name = "panelActionButtons";
            this.panelActionButtons.Size = new System.Drawing.Size(444, 50);
            this.panelActionButtons.TabIndex = 1;
            // 
            // BtnThemHanhDong
            // 
            this.BtnThemHanhDong.Location = new System.Drawing.Point(6, 10);
            this.BtnThemHanhDong.Name = "BtnThemHanhDong";
            this.BtnThemHanhDong.Size = new System.Drawing.Size(130, 30);
            this.BtnThemHanhDong.TabIndex = 0;
            this.BtnThemHanhDong.Text = "Thêm hành động";
            this.BtnThemHanhDong.UseVisualStyleBackColor = true;
            this.BtnThemHanhDong.Click += new System.EventHandler(this.BtnThemHanhDong_Click);
            // 
            // BtnSuaHanhDong
            // 
            this.BtnSuaHanhDong.Location = new System.Drawing.Point(142, 10);
            this.BtnSuaHanhDong.Name = "BtnSuaHanhDong";
            this.BtnSuaHanhDong.Size = new System.Drawing.Size(100, 30);
            this.BtnSuaHanhDong.TabIndex = 1;
            this.BtnSuaHanhDong.Text = "Sửa";
            this.BtnSuaHanhDong.UseVisualStyleBackColor = true;
            this.BtnSuaHanhDong.Click += new System.EventHandler(this.BtnSuaHanhDong_Click);
            // 
            // BtnXoaHanhDong
            // 
            this.BtnXoaHanhDong.Location = new System.Drawing.Point(248, 10);
            this.BtnXoaHanhDong.Name = "BtnXoaHanhDong";
            this.BtnXoaHanhDong.Size = new System.Drawing.Size(100, 30);
            this.BtnXoaHanhDong.TabIndex = 2;
            this.BtnXoaHanhDong.Text = "Xóa";
            this.BtnXoaHanhDong.UseVisualStyleBackColor = true;
            this.BtnXoaHanhDong.Click += new System.EventHandler(this.BtnXoaHanhDong_Click);
            // 
            // FrmRuleEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 550);
            this.Controls.Add(this.panelBody);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelTop);
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "FrmRuleEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Editor Rule";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.panelBody.ResumeLayout(false);
            this.groupHanhDong.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHanhDong)).EndInit();
            this.panelActionButtons.ResumeLayout(false);
            this.groupDieuKien.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDieuKien)).EndInit();
            this.panelConditionButtons.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTenRule;
        private System.Windows.Forms.TextBox txtTenRule;
        private System.Windows.Forms.Label lblMoTa;
        private System.Windows.Forms.TextBox txtMoTa;
        private System.Windows.Forms.Label lblTrigger;
        private System.Windows.Forms.ComboBox CboTrigger;
        private System.Windows.Forms.Label lblTrangThai;
        private System.Windows.Forms.ComboBox CboTrangThai;
        private System.Windows.Forms.Panel panelBody;
        private System.Windows.Forms.GroupBox groupDieuKien;
        private System.Windows.Forms.DataGridView dgvDieuKien;
        private System.Windows.Forms.Panel panelConditionButtons;
        private System.Windows.Forms.Button BtnThemDieuKien;
        private System.Windows.Forms.Button BtnSuaDieuKien;
        private System.Windows.Forms.Button BtnXoaDieuKien;
        private System.Windows.Forms.Label lblChonTruong;
        private System.Windows.Forms.ComboBox CboField;
        private System.Windows.Forms.GroupBox groupHanhDong;
        private System.Windows.Forms.DataGridView dgvHanhDong;
        private System.Windows.Forms.Panel panelActionButtons;
        private System.Windows.Forms.Button BtnThemHanhDong;
        private System.Windows.Forms.Button BtnSuaHanhDong;
        private System.Windows.Forms.Button BtnXoaHanhDong;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Button BtnValidate;
        private System.Windows.Forms.Button BtnTestRule;
        private System.Windows.Forms.Button BtnLuu;
        private System.Windows.Forms.Button BtnHuy;
    }
}
