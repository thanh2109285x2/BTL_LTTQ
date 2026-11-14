namespace BTL_LTTQ.Forms.Automation
{
    partial class FrmAutomationDashboard
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
            this.panelThongKe = new System.Windows.Forms.Panel();
            this.lblTieuDeThongKe = new System.Windows.Forms.Label();
            this.lblTongRuleCaption = new System.Windows.Forms.Label();
            this.lblTongRule = new System.Windows.Forms.Label();
            this.lblRuleHoatDongCaption = new System.Windows.Forms.Label();
            this.lblRuleHoatDong = new System.Windows.Forms.Label();
            this.lblLanChayThanhCongCaption = new System.Windows.Forms.Label();
            this.lblLanChayThanhCong = new System.Windows.Forms.Label();
            this.lblLanChayThatBaiCaption = new System.Windows.Forms.Label();
            this.lblLanChayThatBai = new System.Windows.Forms.Label();
            this.lblLanChayMotPhanCaption = new System.Windows.Forms.Label();
            this.lblLanChayMotPhan = new System.Windows.Forms.Label();
            this.lblTaskDangChoCaption = new System.Windows.Forms.Label();
            this.lblTaskDangCho = new System.Windows.Forms.Label();
            this.lblThongBaoMoiCaption = new System.Windows.Forms.Label();
            this.lblThongBaoMoi = new System.Windows.Forms.Label();
            this.btnQuanLyRule = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabLanChay = new System.Windows.Forms.TabPage();
            this.dgvLanChay = new System.Windows.Forms.DataGridView();
            this.tabTask = new System.Windows.Forms.TabPage();
            this.dgvTaskDangCho = new System.Windows.Forms.DataGridView();
            this.tabThongBao = new System.Windows.Forms.TabPage();
            this.dgvThongBaoMoi = new System.Windows.Forms.DataGridView();
            this.panelThongKe.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabLanChay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLanChay)).BeginInit();
            this.tabTask.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTaskDangCho)).BeginInit();
            this.tabThongBao.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvThongBaoMoi)).BeginInit();
            this.SuspendLayout();
            // 
            // panelThongKe
            // 
            this.panelThongKe.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelThongKe.Controls.Add(this.lblTieuDeThongKe);
            this.panelThongKe.Controls.Add(this.lblTongRuleCaption);
            this.panelThongKe.Controls.Add(this.lblTongRule);
            this.panelThongKe.Controls.Add(this.lblRuleHoatDongCaption);
            this.panelThongKe.Controls.Add(this.lblRuleHoatDong);
            this.panelThongKe.Controls.Add(this.lblLanChayThanhCongCaption);
            this.panelThongKe.Controls.Add(this.lblLanChayThanhCong);
            this.panelThongKe.Controls.Add(this.lblLanChayThatBaiCaption);
            this.panelThongKe.Controls.Add(this.lblLanChayThatBai);
            this.panelThongKe.Controls.Add(this.lblLanChayMotPhanCaption);
            this.panelThongKe.Controls.Add(this.lblLanChayMotPhan);
            this.panelThongKe.Controls.Add(this.lblTaskDangChoCaption);
            this.panelThongKe.Controls.Add(this.lblTaskDangCho);
            this.panelThongKe.Controls.Add(this.lblThongBaoMoiCaption);
            this.panelThongKe.Controls.Add(this.lblThongBaoMoi);
            this.panelThongKe.Controls.Add(this.btnQuanLyRule);
            this.panelThongKe.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelThongKe.Location = new System.Drawing.Point(0, 0);
            this.panelThongKe.Name = "panelThongKe";
            this.panelThongKe.Size = new System.Drawing.Size(1000, 130);
            this.panelThongKe.TabIndex = 0;
            // 
            // lblTieuDeThongKe
            // 
            this.lblTieuDeThongKe.AutoSize = true;
            this.lblTieuDeThongKe.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTieuDeThongKe.Location = new System.Drawing.Point(12, 9);
            this.lblTieuDeThongKe.Name = "lblTieuDeThongKe";
            this.lblTieuDeThongKe.Size = new System.Drawing.Size(150, 20);
            this.lblTieuDeThongKe.TabIndex = 0;
            this.lblTieuDeThongKe.Text = "Tổng quan tự động hóa";
            // 
            // lblTongRuleCaption
            // 
            this.lblTongRuleCaption.AutoSize = true;
            this.lblTongRuleCaption.Location = new System.Drawing.Point(12, 45);
            this.lblTongRuleCaption.Name = "lblTongRuleCaption";
            this.lblTongRuleCaption.Size = new System.Drawing.Size(74, 15);
            this.lblTongRuleCaption.TabIndex = 1;
            this.lblTongRuleCaption.Text = "Tổng số Rule:";
            // 
            // lblTongRule
            // 
            this.lblTongRule.AutoSize = true;
            this.lblTongRule.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTongRule.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblTongRule.Location = new System.Drawing.Point(12, 62);
            this.lblTongRule.Name = "lblTongRule";
            this.lblTongRule.Size = new System.Drawing.Size(19, 21);
            this.lblTongRule.TabIndex = 2;
            this.lblTongRule.Text = "0";
            // 
            // lblRuleHoatDongCaption
            // 
            this.lblRuleHoatDongCaption.AutoSize = true;
            this.lblRuleHoatDongCaption.Location = new System.Drawing.Point(170, 45);
            this.lblRuleHoatDongCaption.Name = "lblRuleHoatDongCaption";
            this.lblRuleHoatDongCaption.Size = new System.Drawing.Size(86, 15);
            this.lblRuleHoatDongCaption.TabIndex = 3;
            this.lblRuleHoatDongCaption.Text = "Rule hoạt động:";
            // 
            // lblRuleHoatDong
            // 
            this.lblRuleHoatDong.AutoSize = true;
            this.lblRuleHoatDong.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblRuleHoatDong.ForeColor = System.Drawing.Color.SeaGreen;
            this.lblRuleHoatDong.Location = new System.Drawing.Point(170, 62);
            this.lblRuleHoatDong.Name = "lblRuleHoatDong";
            this.lblRuleHoatDong.Size = new System.Drawing.Size(19, 21);
            this.lblRuleHoatDong.TabIndex = 4;
            this.lblRuleHoatDong.Text = "0";
            // 
            // lblLanChayThanhCongCaption
            // 
            this.lblLanChayThanhCongCaption.AutoSize = true;
            this.lblLanChayThanhCongCaption.Location = new System.Drawing.Point(340, 45);
            this.lblLanChayThanhCongCaption.Name = "lblLanChayThanhCongCaption";
            this.lblLanChayThanhCongCaption.Size = new System.Drawing.Size(92, 15);
            this.lblLanChayThanhCongCaption.TabIndex = 5;
            this.lblLanChayThanhCongCaption.Text = "Chạy thành công:";
            // 
            // lblLanChayThanhCong
            // 
            this.lblLanChayThanhCong.AutoSize = true;
            this.lblLanChayThanhCong.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblLanChayThanhCong.ForeColor = System.Drawing.Color.SeaGreen;
            this.lblLanChayThanhCong.Location = new System.Drawing.Point(340, 62);
            this.lblLanChayThanhCong.Name = "lblLanChayThanhCong";
            this.lblLanChayThanhCong.Size = new System.Drawing.Size(19, 21);
            this.lblLanChayThanhCong.TabIndex = 6;
            this.lblLanChayThanhCong.Text = "0";
            // 
            // lblLanChayThatBaiCaption
            // 
            this.lblLanChayThatBaiCaption.AutoSize = true;
            this.lblLanChayThatBaiCaption.Location = new System.Drawing.Point(500, 45);
            this.lblLanChayThatBaiCaption.Name = "lblLanChayThatBaiCaption";
            this.lblLanChayThatBaiCaption.Size = new System.Drawing.Size(69, 15);
            this.lblLanChayThatBaiCaption.TabIndex = 7;
            this.lblLanChayThatBaiCaption.Text = "Chạy thất bại:";
            // 
            // lblLanChayThatBai
            // 
            this.lblLanChayThatBai.AutoSize = true;
            this.lblLanChayThatBai.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblLanChayThatBai.ForeColor = System.Drawing.Color.Firebrick;
            this.lblLanChayThatBai.Location = new System.Drawing.Point(500, 62);
            this.lblLanChayThatBai.Name = "lblLanChayThatBai";
            this.lblLanChayThatBai.Size = new System.Drawing.Size(19, 21);
            this.lblLanChayThatBai.TabIndex = 8;
            this.lblLanChayThatBai.Text = "0";
            // 
            // lblLanChayMotPhanCaption
            // 
            this.lblLanChayMotPhanCaption.AutoSize = true;
            this.lblLanChayMotPhanCaption.Location = new System.Drawing.Point(650, 45);
            this.lblLanChayMotPhanCaption.Name = "lblLanChayMotPhanCaption";
            this.lblLanChayMotPhanCaption.Size = new System.Drawing.Size(87, 15);
            this.lblLanChayMotPhanCaption.TabIndex = 9;
            this.lblLanChayMotPhanCaption.Text = "Chạy một phần:";
            // 
            // lblLanChayMotPhan
            // 
            this.lblLanChayMotPhan.AutoSize = true;
            this.lblLanChayMotPhan.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblLanChayMotPhan.ForeColor = System.Drawing.Color.DarkOrange;
            this.lblLanChayMotPhan.Location = new System.Drawing.Point(650, 62);
            this.lblLanChayMotPhan.Name = "lblLanChayMotPhan";
            this.lblLanChayMotPhan.Size = new System.Drawing.Size(19, 21);
            this.lblLanChayMotPhan.TabIndex = 10;
            this.lblLanChayMotPhan.Text = "0";
            // 
            // lblTaskDangChoCaption
            // 
            this.lblTaskDangChoCaption.AutoSize = true;
            this.lblTaskDangChoCaption.Location = new System.Drawing.Point(810, 45);
            this.lblTaskDangChoCaption.Name = "lblTaskDangChoCaption";
            this.lblTaskDangChoCaption.Size = new System.Drawing.Size(84, 15);
            this.lblTaskDangChoCaption.TabIndex = 11;
            this.lblTaskDangChoCaption.Text = "Task đang chờ:";
            // 
            // lblTaskDangCho
            // 
            this.lblTaskDangCho.AutoSize = true;
            this.lblTaskDangCho.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTaskDangCho.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblTaskDangCho.Location = new System.Drawing.Point(810, 62);
            this.lblTaskDangCho.Name = "lblTaskDangCho";
            this.lblTaskDangCho.Size = new System.Drawing.Size(19, 21);
            this.lblTaskDangCho.TabIndex = 12;
            this.lblTaskDangCho.Text = "0";
            // 
            // lblThongBaoMoiCaption
            // 
            this.lblThongBaoMoiCaption.AutoSize = true;
            this.lblThongBaoMoiCaption.Location = new System.Drawing.Point(12, 94);
            this.lblThongBaoMoiCaption.Name = "lblThongBaoMoiCaption";
            this.lblThongBaoMoiCaption.Size = new System.Drawing.Size(108, 15);
            this.lblThongBaoMoiCaption.TabIndex = 13;
            this.lblThongBaoMoiCaption.Text = "Thông báo chưa đọc:";
            // 
            // lblThongBaoMoi
            // 
            this.lblThongBaoMoi.AutoSize = true;
            this.lblThongBaoMoi.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblThongBaoMoi.ForeColor = System.Drawing.Color.Firebrick;
            this.lblThongBaoMoi.Location = new System.Drawing.Point(130, 90);
            this.lblThongBaoMoi.Name = "lblThongBaoMoi";
            this.lblThongBaoMoi.Size = new System.Drawing.Size(19, 21);
            this.lblThongBaoMoi.TabIndex = 14;
            this.lblThongBaoMoi.Text = "0";
            // 
            // btnQuanLyRule
            // 
            this.btnQuanLyRule.Location = new System.Drawing.Point(818, 9);
            this.btnQuanLyRule.Name = "btnQuanLyRule";
            this.btnQuanLyRule.Size = new System.Drawing.Size(165, 30);
            this.btnQuanLyRule.TabIndex = 15;
            this.btnQuanLyRule.Text = "Quản lý quy tắc";
            this.btnQuanLyRule.UseVisualStyleBackColor = true;
            this.btnQuanLyRule.Click += new System.EventHandler(this.btnQuanLyRule_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabLanChay);
            this.tabControl.Controls.Add(this.tabTask);
            this.tabControl.Controls.Add(this.tabThongBao);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 130);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1000, 520);
            this.tabControl.TabIndex = 1;
            // 
            // tabLanChay
            // 
            this.tabLanChay.Controls.Add(this.dgvLanChay);
            this.tabLanChay.Location = new System.Drawing.Point(4, 24);
            this.tabLanChay.Name = "tabLanChay";
            this.tabLanChay.Padding = new System.Windows.Forms.Padding(3);
            this.tabLanChay.Size = new System.Drawing.Size(992, 492);
            this.tabLanChay.TabIndex = 0;
            this.tabLanChay.Text = "Lần chạy gần đây";
            this.tabLanChay.UseVisualStyleBackColor = true;
            // 
            // dgvLanChay
            // 
            this.dgvLanChay.AllowUserToAddRows = false;
            this.dgvLanChay.AllowUserToDeleteRows = false;
            this.dgvLanChay.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvLanChay.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvLanChay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLanChay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLanChay.Location = new System.Drawing.Point(3, 3);
            this.dgvLanChay.MultiSelect = false;
            this.dgvLanChay.Name = "dgvLanChay";
            this.dgvLanChay.ReadOnly = true;
            this.dgvLanChay.RowHeadersVisible = false;
            this.dgvLanChay.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLanChay.Size = new System.Drawing.Size(986, 486);
            this.dgvLanChay.TabIndex = 0;
            this.dgvLanChay.DoubleClick += new System.EventHandler(this.dgvLanChay_DoubleClick);
            // 
            // tabTask
            // 
            this.tabTask.Controls.Add(this.dgvTaskDangCho);
            this.tabTask.Location = new System.Drawing.Point(4, 24);
            this.tabTask.Name = "tabTask";
            this.tabTask.Padding = new System.Windows.Forms.Padding(3);
            this.tabTask.Size = new System.Drawing.Size(992, 492);
            this.tabTask.TabIndex = 1;
            this.tabTask.Text = "Nhiệm vụ đang chờ";
            this.tabTask.UseVisualStyleBackColor = true;
            // 
            // dgvTaskDangCho
            // 
            this.dgvTaskDangCho.AllowUserToAddRows = false;
            this.dgvTaskDangCho.AllowUserToDeleteRows = false;
            this.dgvTaskDangCho.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvTaskDangCho.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvTaskDangCho.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTaskDangCho.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTaskDangCho.Location = new System.Drawing.Point(3, 3);
            this.dgvTaskDangCho.MultiSelect = false;
            this.dgvTaskDangCho.Name = "dgvTaskDangCho";
            this.dgvTaskDangCho.ReadOnly = true;
            this.dgvTaskDangCho.RowHeadersVisible = false;
            this.dgvTaskDangCho.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTaskDangCho.Size = new System.Drawing.Size(986, 486);
            this.dgvTaskDangCho.TabIndex = 0;
            this.dgvTaskDangCho.DoubleClick += new System.EventHandler(this.dgvTaskDangCho_DoubleClick);
            // 
            // tabThongBao
            // 
            this.tabThongBao.Controls.Add(this.dgvThongBaoMoi);
            this.tabThongBao.Location = new System.Drawing.Point(4, 24);
            this.tabThongBao.Name = "tabThongBao";
            this.tabThongBao.Padding = new System.Windows.Forms.Padding(3);
            this.tabThongBao.Size = new System.Drawing.Size(992, 492);
            this.tabThongBao.TabIndex = 2;
            this.tabThongBao.Text = "Thông báo mới";
            this.tabThongBao.UseVisualStyleBackColor = true;
            // 
            // dgvThongBaoMoi
            // 
            this.dgvThongBaoMoi.AllowUserToAddRows = false;
            this.dgvThongBaoMoi.AllowUserToDeleteRows = false;
            this.dgvThongBaoMoi.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvThongBaoMoi.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvThongBaoMoi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvThongBaoMoi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvThongBaoMoi.Location = new System.Drawing.Point(3, 3);
            this.dgvThongBaoMoi.MultiSelect = false;
            this.dgvThongBaoMoi.Name = "dgvThongBaoMoi";
            this.dgvThongBaoMoi.ReadOnly = true;
            this.dgvThongBaoMoi.RowHeadersVisible = false;
            this.dgvThongBaoMoi.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvThongBaoMoi.Size = new System.Drawing.Size(986, 486);
            this.dgvThongBaoMoi.TabIndex = 0;
            this.dgvThongBaoMoi.DoubleClick += new System.EventHandler(this.dgvThongBaoMoi_DoubleClick);
            // 
            // FrmAutomationDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 650);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.panelThongKe);
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "FrmAutomationDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Dashboard Tự động hóa";
            this.panelThongKe.ResumeLayout(false);
            this.panelThongKe.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabLanChay.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLanChay)).EndInit();
            this.tabTask.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTaskDangCho)).EndInit();
            this.tabThongBao.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvThongBaoMoi)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panelThongKe;
        private System.Windows.Forms.Label lblTieuDeThongKe;
        private System.Windows.Forms.Label lblTongRuleCaption;
        private System.Windows.Forms.Label lblTongRule;
        private System.Windows.Forms.Label lblRuleHoatDongCaption;
        private System.Windows.Forms.Label lblRuleHoatDong;
        private System.Windows.Forms.Label lblLanChayThanhCongCaption;
        private System.Windows.Forms.Label lblLanChayThanhCong;
        private System.Windows.Forms.Label lblLanChayThatBaiCaption;
        private System.Windows.Forms.Label lblLanChayThatBai;
        private System.Windows.Forms.Label lblLanChayMotPhanCaption;
        private System.Windows.Forms.Label lblLanChayMotPhan;
        private System.Windows.Forms.Label lblTaskDangChoCaption;
        private System.Windows.Forms.Label lblTaskDangCho;
        private System.Windows.Forms.Label lblThongBaoMoiCaption;
        private System.Windows.Forms.Label lblThongBaoMoi;
        private System.Windows.Forms.Button btnQuanLyRule;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabLanChay;
        private System.Windows.Forms.DataGridView dgvLanChay;
        private System.Windows.Forms.TabPage tabTask;
        private System.Windows.Forms.DataGridView dgvTaskDangCho;
        private System.Windows.Forms.TabPage tabThongBao;
        private System.Windows.Forms.DataGridView dgvThongBaoMoi;
    }
}
