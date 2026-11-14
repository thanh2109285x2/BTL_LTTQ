namespace BTL_LTTQ.Forms.Automation
{
    partial class FrmHanhDongThem
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
            this.lblActionType = new System.Windows.Forms.Label();
            this.CboActionType = new System.Windows.Forms.ComboBox();
            this.panelNoiDung = new System.Windows.Forms.Panel();
            this.panelNotification = new System.Windows.Forms.Panel();
            this.lblTieuDe = new System.Windows.Forms.Label();
            this.txtTieuDe = new System.Windows.Forms.TextBox();
            this.lblNoiDung = new System.Windows.Forms.Label();
            this.txtMoTa = new System.Windows.Forms.TextBox();
            this.lblMucDo = new System.Windows.Forms.Label();
            this.cmbMucDo = new System.Windows.Forms.ComboBox();
            this.panelTask = new System.Windows.Forms.Panel();
            this.lblTieuDeTask = new System.Windows.Forms.Label();
            this.txtTieuDeTask = new System.Windows.Forms.TextBox();
            this.lblUuTien = new System.Windows.Forms.Label();
            this.cmbUuTien = new System.Windows.Forms.ComboBox();
            this.lblHanXuLy = new System.Windows.Forms.Label();
            this.dtHanXuLy = new System.Windows.Forms.DateTimePicker();
            this.panelStatus = new System.Windows.Forms.Panel();
            this.lblTrangThaiMoi = new System.Windows.Forms.Label();
            this.cmbTrangThaiMoi = new System.Windows.Forms.ComboBox();
            this.panelAudit = new System.Windows.Forms.Panel();
            this.lblNoiDungAudit = new System.Windows.Forms.Label();
            this.txtNoiDung = new System.Windows.Forms.TextBox();
            this.lblLoaiAudit = new System.Windows.Forms.Label();
            this.cmbLoaiAudit = new System.Windows.Forms.ComboBox();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.chkTiepTucKhiLoi = new System.Windows.Forms.CheckBox();
            this.BtnOK = new System.Windows.Forms.Button();
            this.BtnHuy = new System.Windows.Forms.Button();
            this.panelNoiDung.SuspendLayout();
            this.panelNotification.SuspendLayout();
            this.panelTask.SuspendLayout();
            this.panelStatus.SuspendLayout();
            this.panelAudit.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblActionType
            // 
            this.lblActionType.AutoSize = true;
            this.lblActionType.Location = new System.Drawing.Point(15, 18);
            this.lblActionType.Name = "lblActionType";
            this.lblActionType.Size = new System.Drawing.Size(72, 15);
            this.lblActionType.TabIndex = 0;
            this.lblActionType.Text = "Loại hành động:";
            // 
            // CboActionType
            // 
            this.CboActionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CboActionType.FormattingEnabled = true;
            this.CboActionType.Location = new System.Drawing.Point(140, 15);
            this.CboActionType.Name = "CboActionType";
            this.CboActionType.Size = new System.Drawing.Size(350, 23);
            this.CboActionType.TabIndex = 1;
            this.CboActionType.SelectedIndexChanged += new System.EventHandler(this.CboActionType_SelectedIndexChanged);
            // 
            // panelNoiDung
            // 
            this.panelNoiDung.Controls.Add(this.panelNotification);
            this.panelNoiDung.Controls.Add(this.panelTask);
            this.panelNoiDung.Controls.Add(this.panelStatus);
            this.panelNoiDung.Controls.Add(this.panelAudit);
            this.panelNoiDung.Location = new System.Drawing.Point(15, 55);
            this.panelNoiDung.Name = "panelNoiDung";
            this.panelNoiDung.Size = new System.Drawing.Size(490, 330);
            this.panelNoiDung.TabIndex = 2;
            // 
            // panelNotification
            // 
            this.panelNotification.Controls.Add(this.lblTieuDe);
            this.panelNotification.Controls.Add(this.txtTieuDe);
            this.panelNotification.Controls.Add(this.lblNoiDung);
            this.panelNotification.Controls.Add(this.txtMoTa);
            this.panelNotification.Controls.Add(this.lblMucDo);
            this.panelNotification.Controls.Add(this.cmbMucDo);
            this.panelNotification.Location = new System.Drawing.Point(0, 0);
            this.panelNotification.Name = "panelNotification";
            this.panelNotification.Size = new System.Drawing.Size(490, 330);
            this.panelNotification.TabIndex = 0;
            // 
            // lblTieuDe
            // 
            this.lblTieuDe.AutoSize = true;
            this.lblTieuDe.Location = new System.Drawing.Point(5, 18);
            this.lblTieuDe.Name = "lblTieuDe";
            this.lblTieuDe.Size = new System.Drawing.Size(47, 15);
            this.lblTieuDe.TabIndex = 0;
            this.lblTieuDe.Text = "Tiêu đề:";
            // 
            // txtTieuDe
            // 
            this.txtTieuDe.Location = new System.Drawing.Point(125, 15);
            this.txtTieuDe.Name = "txtTieuDe";
            this.txtTieuDe.Size = new System.Drawing.Size(350, 23);
            this.txtTieuDe.TabIndex = 1;
            // 
            // lblNoiDung
            // 
            this.lblNoiDung.AutoSize = true;
            this.lblNoiDung.Location = new System.Drawing.Point(5, 55);
            this.lblNoiDung.Name = "lblNoiDung";
            this.lblNoiDung.Size = new System.Drawing.Size(58, 15);
            this.lblNoiDung.TabIndex = 2;
            this.lblNoiDung.Text = "Nội dung:";
            // 
            // txtMoTa
            // 
            this.txtMoTa.Location = new System.Drawing.Point(125, 52);
            this.txtMoTa.Multiline = true;
            this.txtMoTa.Name = "txtMoTa";
            this.txtMoTa.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMoTa.Size = new System.Drawing.Size(350, 120);
            this.txtMoTa.TabIndex = 3;
            // 
            // lblMucDo
            // 
            this.lblMucDo.AutoSize = true;
            this.lblMucDo.Location = new System.Drawing.Point(5, 192);
            this.lblMucDo.Name = "lblMucDo";
            this.lblMucDo.Size = new System.Drawing.Size(50, 15);
            this.lblMucDo.TabIndex = 4;
            this.lblMucDo.Text = "Mức độ:";
            // 
            // cmbMucDo
            // 
            this.cmbMucDo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMucDo.FormattingEnabled = true;
            this.cmbMucDo.Location = new System.Drawing.Point(125, 189);
            this.cmbMucDo.Name = "cmbMucDo";
            this.cmbMucDo.Size = new System.Drawing.Size(180, 23);
            this.cmbMucDo.TabIndex = 5;
            // 
            // panelTask
            // 
            this.panelTask.Controls.Add(this.lblTieuDeTask);
            this.panelTask.Controls.Add(this.txtTieuDeTask);
            this.panelTask.Controls.Add(this.lblUuTien);
            this.panelTask.Controls.Add(this.cmbUuTien);
            this.panelTask.Controls.Add(this.lblHanXuLy);
            this.panelTask.Controls.Add(this.dtHanXuLy);
            this.panelTask.Location = new System.Drawing.Point(0, 0);
            this.panelTask.Name = "panelTask";
            this.panelTask.Size = new System.Drawing.Size(490, 330);
            this.panelTask.TabIndex = 1;
            this.panelTask.Visible = false;
            // 
            // lblTieuDeTask
            // 
            this.lblTieuDeTask.AutoSize = true;
            this.lblTieuDeTask.Location = new System.Drawing.Point(5, 18);
            this.lblTieuDeTask.Name = "lblTieuDeTask";
            this.lblTieuDeTask.Size = new System.Drawing.Size(47, 15);
            this.lblTieuDeTask.TabIndex = 0;
            this.lblTieuDeTask.Text = "Tiêu đề:";
            // 
            // txtTieuDeTask
            // 
            this.txtTieuDeTask.Location = new System.Drawing.Point(125, 15);
            this.txtTieuDeTask.Name = "txtTieuDeTask";
            this.txtTieuDeTask.Size = new System.Drawing.Size(350, 23);
            this.txtTieuDeTask.TabIndex = 1;
            // 
            // lblUuTien
            // 
            this.lblUuTien.AutoSize = true;
            this.lblUuTien.Location = new System.Drawing.Point(5, 62);
            this.lblUuTien.Name = "lblUuTien";
            this.lblUuTien.Size = new System.Drawing.Size(49, 15);
            this.lblUuTien.TabIndex = 2;
            this.lblUuTien.Text = "Ưu tiên:";
            // 
            // cmbUuTien
            // 
            this.cmbUuTien.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUuTien.FormattingEnabled = true;
            this.cmbUuTien.Location = new System.Drawing.Point(125, 59);
            this.cmbUuTien.Name = "cmbUuTien";
            this.cmbUuTien.Size = new System.Drawing.Size(180, 23);
            this.cmbUuTien.TabIndex = 3;
            // 
            // lblHanXuLy
            // 
            this.lblHanXuLy.AutoSize = true;
            this.lblHanXuLy.Location = new System.Drawing.Point(5, 106);
            this.lblHanXuLy.Name = "lblHanXuLy";
            this.lblHanXuLy.Size = new System.Drawing.Size(58, 15);
            this.lblHanXuLy.TabIndex = 4;
            this.lblHanXuLy.Text = "Hạn xử lý:";
            // 
            // dtHanXuLy
            // 
            this.dtHanXuLy.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtHanXuLy.Location = new System.Drawing.Point(125, 103);
            this.dtHanXuLy.Name = "dtHanXuLy";
            this.dtHanXuLy.Size = new System.Drawing.Size(180, 23);
            this.dtHanXuLy.TabIndex = 5;
            // 
            // panelStatus
            // 
            this.panelStatus.Controls.Add(this.lblTrangThaiMoi);
            this.panelStatus.Controls.Add(this.cmbTrangThaiMoi);
            this.panelStatus.Location = new System.Drawing.Point(0, 0);
            this.panelStatus.Name = "panelStatus";
            this.panelStatus.Size = new System.Drawing.Size(490, 330);
            this.panelStatus.TabIndex = 2;
            this.panelStatus.Visible = false;
            // 
            // lblTrangThaiMoi
            // 
            this.lblTrangThaiMoi.AutoSize = true;
            this.lblTrangThaiMoi.Location = new System.Drawing.Point(5, 18);
            this.lblTrangThaiMoi.Name = "lblTrangThaiMoi";
            this.lblTrangThaiMoi.Size = new System.Drawing.Size(84, 15);
            this.lblTrangThaiMoi.TabIndex = 0;
            this.lblTrangThaiMoi.Text = "Trạng thái mới:";
            // 
            // cmbTrangThaiMoi
            // 
            this.cmbTrangThaiMoi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTrangThaiMoi.FormattingEnabled = true;
            this.cmbTrangThaiMoi.Location = new System.Drawing.Point(125, 15);
            this.cmbTrangThaiMoi.Name = "cmbTrangThaiMoi";
            this.cmbTrangThaiMoi.Size = new System.Drawing.Size(220, 23);
            this.cmbTrangThaiMoi.TabIndex = 1;
            // 
            // panelAudit
            // 
            this.panelAudit.Controls.Add(this.lblNoiDungAudit);
            this.panelAudit.Controls.Add(this.txtNoiDung);
            this.panelAudit.Controls.Add(this.lblLoaiAudit);
            this.panelAudit.Controls.Add(this.cmbLoaiAudit);
            this.panelAudit.Location = new System.Drawing.Point(0, 0);
            this.panelAudit.Name = "panelAudit";
            this.panelAudit.Size = new System.Drawing.Size(490, 330);
            this.panelAudit.TabIndex = 3;
            this.panelAudit.Visible = false;
            // 
            // lblNoiDungAudit
            // 
            this.lblNoiDungAudit.AutoSize = true;
            this.lblNoiDungAudit.Location = new System.Drawing.Point(5, 18);
            this.lblNoiDungAudit.Name = "lblNoiDungAudit";
            this.lblNoiDungAudit.Size = new System.Drawing.Size(58, 15);
            this.lblNoiDungAudit.TabIndex = 0;
            this.lblNoiDungAudit.Text = "Nội dung:";
            // 
            // txtNoiDung
            // 
            this.txtNoiDung.Location = new System.Drawing.Point(125, 15);
            this.txtNoiDung.Multiline = true;
            this.txtNoiDung.Name = "txtNoiDung";
            this.txtNoiDung.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNoiDung.Size = new System.Drawing.Size(350, 140);
            this.txtNoiDung.TabIndex = 1;
            // 
            // lblLoaiAudit
            // 
            this.lblLoaiAudit.AutoSize = true;
            this.lblLoaiAudit.Location = new System.Drawing.Point(5, 172);
            this.lblLoaiAudit.Name = "lblLoaiAudit";
            this.lblLoaiAudit.Size = new System.Drawing.Size(32, 15);
            this.lblLoaiAudit.TabIndex = 2;
            this.lblLoaiAudit.Text = "Loại:";
            // 
            // cmbLoaiAudit
            // 
            this.cmbLoaiAudit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLoaiAudit.FormattingEnabled = true;
            this.cmbLoaiAudit.Location = new System.Drawing.Point(125, 169);
            this.cmbLoaiAudit.Name = "cmbLoaiAudit";
            this.cmbLoaiAudit.Size = new System.Drawing.Size(180, 23);
            this.cmbLoaiAudit.TabIndex = 3;
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.chkTiepTucKhiLoi);
            this.panelBottom.Controls.Add(this.BtnHuy);
            this.panelBottom.Controls.Add(this.BtnOK);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 400);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(520, 55);
            this.panelBottom.TabIndex = 3;
            // 
            // chkTiepTucKhiLoi
            // 
            this.chkTiepTucKhiLoi.AutoSize = true;
            this.chkTiepTucKhiLoi.Checked = true;
            this.chkTiepTucKhiLoi.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTiepTucKhiLoi.Location = new System.Drawing.Point(15, 18);
            this.chkTiepTucKhiLoi.Name = "chkTiepTucKhiLoi";
            this.chkTiepTucKhiLoi.Size = new System.Drawing.Size(120, 19);
            this.chkTiepTucKhiLoi.TabIndex = 0;
            this.chkTiepTucKhiLoi.Text = "Tiếp tục khi lỗi";
            this.chkTiepTucKhiLoi.UseVisualStyleBackColor = true;
            // 
            // BtnOK
            // 
            this.BtnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnOK.Location = new System.Drawing.Point(294, 12);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(100, 30);
            this.BtnOK.TabIndex = 1;
            this.BtnOK.Text = "Đồng ý";
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // BtnHuy
            // 
            this.BtnHuy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnHuy.Location = new System.Drawing.Point(402, 12);
            this.BtnHuy.Name = "BtnHuy";
            this.BtnHuy.Size = new System.Drawing.Size(100, 30);
            this.BtnHuy.TabIndex = 2;
            this.BtnHuy.Text = "Hủy";
            this.BtnHuy.UseVisualStyleBackColor = true;
            this.BtnHuy.Click += new System.EventHandler(this.BtnHuy_Click);
            // 
            // FrmHanhDongThem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 455);
            this.Controls.Add(this.lblActionType);
            this.Controls.Add(this.CboActionType);
            this.Controls.Add(this.panelNoiDung);
            this.Controls.Add(this.panelBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmHanhDongThem";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Hành động";
            this.panelNoiDung.ResumeLayout(false);
            this.panelNotification.ResumeLayout(false);
            this.panelNotification.PerformLayout();
            this.panelTask.ResumeLayout(false);
            this.panelTask.PerformLayout();
            this.panelStatus.ResumeLayout(false);
            this.panelStatus.PerformLayout();
            this.panelAudit.ResumeLayout(false);
            this.panelAudit.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblActionType;
        private System.Windows.Forms.ComboBox CboActionType;
        private System.Windows.Forms.Panel panelNoiDung;
        private System.Windows.Forms.Panel panelNotification;
        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.TextBox txtTieuDe;
        private System.Windows.Forms.Label lblNoiDung;
        private System.Windows.Forms.TextBox txtMoTa;
        private System.Windows.Forms.Label lblMucDo;
        private System.Windows.Forms.ComboBox cmbMucDo;
        private System.Windows.Forms.Panel panelTask;
        private System.Windows.Forms.Label lblTieuDeTask;
        private System.Windows.Forms.TextBox txtTieuDeTask;
        private System.Windows.Forms.Label lblUuTien;
        private System.Windows.Forms.ComboBox cmbUuTien;
        private System.Windows.Forms.Label lblHanXuLy;
        private System.Windows.Forms.DateTimePicker dtHanXuLy;
        private System.Windows.Forms.Panel panelStatus;
        private System.Windows.Forms.Label lblTrangThaiMoi;
        private System.Windows.Forms.ComboBox cmbTrangThaiMoi;
        private System.Windows.Forms.Panel panelAudit;
        private System.Windows.Forms.Label lblNoiDungAudit;
        private System.Windows.Forms.TextBox txtNoiDung;
        private System.Windows.Forms.Label lblLoaiAudit;
        private System.Windows.Forms.ComboBox cmbLoaiAudit;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.CheckBox chkTiepTucKhiLoi;
        private System.Windows.Forms.Button BtnOK;
        private System.Windows.Forms.Button BtnHuy;
    }
}
