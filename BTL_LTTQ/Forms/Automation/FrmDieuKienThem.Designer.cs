namespace BTL_LTTQ.Forms.Automation
{
    partial class FrmDieuKienThem
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
            this.lblTieuDe = new System.Windows.Forms.Label();
            this.lblTenTruong = new System.Windows.Forms.Label();
            this.lblMoTaTruong = new System.Windows.Forms.Label();
            this.lblToanTu = new System.Windows.Forms.Label();
            this.CboToanTu = new System.Windows.Forms.ComboBox();
            this.lblGiaTri = new System.Windows.Forms.Label();
            this.numGiaTriSo = new System.Windows.Forms.NumericUpDown();
            this.cboGiaTriLuanLy = new System.Windows.Forms.ComboBox();
            this.BtnOK = new System.Windows.Forms.Button();
            this.BtnHuy = new System.Windows.Forms.Button();
            this.panelBottom = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.numGiaTriSo)).BeginInit();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTieuDe
            // 
            this.lblTieuDe.AutoSize = true;
            this.lblTieuDe.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTieuDe.Location = new System.Drawing.Point(15, 15);
            this.lblTieuDe.Name = "lblTieuDe";
            this.lblTieuDe.Size = new System.Drawing.Size(120, 20);
            this.lblTieuDe.TabIndex = 0;
            this.lblTieuDe.Text = "Thêm điều kiện";
            // 
            // lblTenTruong
            // 
            this.lblTenTruong.AutoSize = true;
            this.lblTenTruong.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTenTruong.Location = new System.Drawing.Point(15, 50);
            this.lblTenTruong.Name = "lblTenTruong";
            this.lblTenTruong.Size = new System.Drawing.Size(61, 15);
            this.lblTenTruong.TabIndex = 1;
            this.lblTenTruong.Text = "Tên trường";
            // 
            // lblMoTaTruong
            // 
            this.lblMoTaTruong.AutoSize = true;
            this.lblMoTaTruong.ForeColor = System.Drawing.Color.Gray;
            this.lblMoTaTruong.Location = new System.Drawing.Point(15, 70);
            this.lblMoTaTruong.Name = "lblMoTaTruong";
            this.lblMoTaTruong.Size = new System.Drawing.Size(0, 15);
            this.lblMoTaTruong.TabIndex = 2;
            // 
            // lblToanTu
            // 
            this.lblToanTu.AutoSize = true;
            this.lblToanTu.Location = new System.Drawing.Point(15, 105);
            this.lblToanTu.Name = "lblToanTu";
            this.lblToanTu.Size = new System.Drawing.Size(53, 15);
            this.lblToanTu.TabIndex = 3;
            this.lblToanTu.Text = "Toán tử:";
            // 
            // CboToanTu
            // 
            this.CboToanTu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CboToanTu.FormattingEnabled = true;
            this.CboToanTu.Location = new System.Drawing.Point(110, 102);
            this.CboToanTu.Name = "CboToanTu";
            this.CboToanTu.Size = new System.Drawing.Size(300, 23);
            this.CboToanTu.TabIndex = 4;
            this.CboToanTu.SelectedIndexChanged += new System.EventHandler(this.CboToanTu_SelectedIndexChanged);
            // 
            // lblGiaTri
            // 
            this.lblGiaTri.AutoSize = true;
            this.lblGiaTri.Location = new System.Drawing.Point(15, 150);
            this.lblGiaTri.Name = "lblGiaTri";
            this.lblGiaTri.Size = new System.Drawing.Size(42, 15);
            this.lblGiaTri.TabIndex = 5;
            this.lblGiaTri.Text = "Giá trị:";
            // 
            // numGiaTriSo
            // 
            this.numGiaTriSo.Location = new System.Drawing.Point(110, 148);
            this.numGiaTriSo.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.numGiaTriSo.Minimum = new decimal(new int[] { 1000000, 0, 0, -2147483648 });
            this.numGiaTriSo.Name = "numGiaTriSo";
            this.numGiaTriSo.Size = new System.Drawing.Size(140, 23);
            this.numGiaTriSo.TabIndex = 6;
            // 
            // cboGiaTriLuanLy
            // 
            this.cboGiaTriLuanLy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGiaTriLuanLy.FormattingEnabled = true;
            this.cboGiaTriLuanLy.Location = new System.Drawing.Point(110, 148);
            this.cboGiaTriLuanLy.Name = "cboGiaTriLuanLy";
            this.cboGiaTriLuanLy.Size = new System.Drawing.Size(140, 23);
            this.cboGiaTriLuanLy.TabIndex = 7;
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.BtnHuy);
            this.panelBottom.Controls.Add(this.BtnOK);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 200);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(440, 55);
            this.panelBottom.TabIndex = 8;
            // 
            // BtnOK
            // 
            this.BtnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnOK.Location = new System.Drawing.Point(220, 12);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(100, 30);
            this.BtnOK.TabIndex = 0;
            this.BtnOK.Text = "Đồng ý";
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // BtnHuy
            // 
            this.BtnHuy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnHuy.Location = new System.Drawing.Point(328, 12);
            this.BtnHuy.Name = "BtnHuy";
            this.BtnHuy.Size = new System.Drawing.Size(100, 30);
            this.BtnHuy.TabIndex = 1;
            this.BtnHuy.Text = "Hủy";
            this.BtnHuy.UseVisualStyleBackColor = true;
            this.BtnHuy.Click += new System.EventHandler(this.BtnHuy_Click);
            // 
            // FrmDieuKienThem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 255);
            this.Controls.Add(this.lblTieuDe);
            this.Controls.Add(this.lblTenTruong);
            this.Controls.Add(this.lblMoTaTruong);
            this.Controls.Add(this.lblToanTu);
            this.Controls.Add(this.CboToanTu);
            this.Controls.Add(this.lblGiaTri);
            this.Controls.Add(this.numGiaTriSo);
            this.Controls.Add(this.cboGiaTriLuanLy);
            this.Controls.Add(this.panelBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDieuKienThem";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Điều kiện";
            ((System.ComponentModel.ISupportInitialize)(this.numGiaTriSo)).EndInit();
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.Label lblTenTruong;
        private System.Windows.Forms.Label lblMoTaTruong;
        private System.Windows.Forms.Label lblToanTu;
        private System.Windows.Forms.ComboBox CboToanTu;
        private System.Windows.Forms.Label lblGiaTri;
        private System.Windows.Forms.NumericUpDown numGiaTriSo;
        private System.Windows.Forms.ComboBox cboGiaTriLuanLy;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Button BtnOK;
        private System.Windows.Forms.Button BtnHuy;
    }
}
