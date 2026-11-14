namespace BTL_LTTQ.Forms.Automation
{
    partial class FrmPhatSuKienThu
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblTieuDe = new Label();
            lblMoTa = new Label();
            lblLop = new Label();
            cboLop = new ComboBox();
            lblTrigger = new Label();
            cboTrigger = new ComboBox();
            lblKetQua = new Label();
            btnPhat = new Button();
            btnDong = new Button();
            SuspendLayout();
            // 
            // lblTieuDe
            // 
            lblTieuDe.AutoSize = true;
            lblTieuDe.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTieuDe.Location = new Point(16, 14);
            lblTieuDe.Name = "lblTieuDe";
            lblTieuDe.Size = new Size(220, 20);
            lblTieuDe.Text = "Phát sự kiện thử cho Rule";
            // 
            // lblMoTa
            // 
            lblMoTa.AutoSize = true;
            lblMoTa.ForeColor = Color.FromArgb(90, 98, 110);
            lblMoTa.Location = new Point(19, 40);
            lblMoTa.Name = "lblMoTa";
            lblMoTa.Size = new Size(590, 40);
            lblMoTa.Text = "Chọn lớp và loại sự kiện, hệ thống sẽ phát sự kiện tới các Rule đang hoạt động có trigger tương ứng.\r\nSự kiện được xử lý y hệt khi phát từ nghiệp vụ (cùng đường code, cùng cơ chế chống chạy trùng).";
            // 
            // lblLop
            // 
            lblLop.AutoSize = true;
            lblLop.Location = new Point(16, 96);
            lblLop.Name = "lblLop";
            lblLop.Size = new Size(50, 15);
            lblLop.Text = "Lớp:";
            // 
            // cboLop
            // 
            cboLop.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLop.FormattingEnabled = true;
            cboLop.Location = new Point(70, 92);
            cboLop.Name = "cboLop";
            cboLop.Size = new Size(300, 23);
            cboLop.TabIndex = 0;
            // 
            // lblTrigger
            // 
            lblTrigger.AutoSize = true;
            lblTrigger.Location = new Point(16, 130);
            lblTrigger.Name = "lblTrigger";
            lblTrigger.Size = new Size(60, 15);
            lblTrigger.Text = "Sự kiện:";
            // 
            // cboTrigger
            // 
            cboTrigger.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTrigger.FormattingEnabled = true;
            cboTrigger.Location = new Point(70, 126);
            cboTrigger.Name = "cboTrigger";
            cboTrigger.Size = new Size(300, 23);
            cboTrigger.TabIndex = 1;
            // 
            // lblKetQua
            // 
            lblKetQua.AutoSize = true;
            lblKetQua.ForeColor = Color.FromArgb(24, 128, 56);
            lblKetQua.Location = new Point(19, 166);
            lblKetQua.Name = "lblKetQua";
            lblKetQua.Size = new Size(560, 15);
            lblKetQua.Text = "";
            // 
            // btnPhat
            // 
            btnPhat.Location = new Point(390, 92);
            btnPhat.Name = "btnPhat";
            btnPhat.Size = new Size(130, 28);
            btnPhat.TabIndex = 2;
            btnPhat.Text = "Phát sự kiện";
            btnPhat.UseVisualStyleBackColor = true;
            btnPhat.Click += btnPhat_Click;
            // 
            // btnDong
            // 
            btnDong.Location = new Point(530, 92);
            btnDong.Name = "btnDong";
            btnDong.Size = new Size(90, 28);
            btnDong.TabIndex = 3;
            btnDong.Text = "Đóng";
            btnDong.UseVisualStyleBackColor = true;
            btnDong.Click += btnDong_Click;
            // 
            // FrmPhatSuKienThu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(640, 200);
            Controls.Add(lblTieuDe);
            Controls.Add(lblMoTa);
            Controls.Add(lblLop);
            Controls.Add(cboLop);
            Controls.Add(lblTrigger);
            Controls.Add(cboTrigger);
            Controls.Add(lblKetQua);
            Controls.Add(btnPhat);
            Controls.Add(btnDong);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmPhatSuKienThu";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Phát sự kiện thử";
            Load += FrmPhatSuKienThu_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTieuDe;
        private Label lblMoTa;
        private Label lblLop;
        private ComboBox cboLop;
        private Label lblTrigger;
        private ComboBox cboTrigger;
        private Label lblKetQua;
        private Button btnPhat;
        private Button btnDong;
    }
}
