using BTL_LTTQ;

namespace BTL_LTTQ.Forms.HeThong
{
    partial class frmBackup
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTieuDe;
        private Label lblNoiDung;
        private AppButton btnSaoLuu;
        private AppButton btnHuy;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTieuDe = new Label();
            lblNoiDung = new Label();
            btnSaoLuu = new AppButton { Kieu = KieuNut.Phu };
            btnHuy = new AppButton { Kieu = KieuNut.Phu };
            SuspendLayout();
            // 
            // lblTieuDe
            // 
            lblTieuDe.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
            lblTieuDe.Location = new Point(32, 20);
            lblTieuDe.Name = "lblTieuDe";
            lblTieuDe.Size = new Size(396, 36);
            lblTieuDe.TabIndex = 0;
            lblTieuDe.Text = "SAO LƯU DATABASE";
            lblTieuDe.TextAlign = ContentAlignment.MiddleCenter;
            lblTieuDe.Click += lblTieuDe_Click;
            // 
            // lblNoiDung
            // 
            lblNoiDung.AutoSize = true;
            lblNoiDung.Location = new Point(57, 102);
            lblNoiDung.Name = "lblNoiDung";
            lblNoiDung.Size = new Size(352, 20);
            lblNoiDung.TabIndex = 1;
            lblNoiDung.Text = "Chọn đường dẫn file sao lưu (.bak) để lưu database.";
            // 
            // btnSaoLuu
            // 
            btnSaoLuu.Location = new Point(70, 188);
            btnSaoLuu.Name = "btnSaoLuu";
            btnSaoLuu.Size = new Size(132, 36);
            btnSaoLuu.TabIndex = 2;
            btnSaoLuu.Text = "Sao lưu";
            btnSaoLuu.Click += btnSaoLuu_Click;
            // 
            // btnHuy
            // 
            btnHuy.DialogResult = DialogResult.Cancel;
            btnHuy.Location = new Point(270, 188);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new Size(86, 36);
            btnHuy.TabIndex = 3;
            btnHuy.Text = "Hủy";
            btnHuy.Click += btnHuy_Click;
            // 
            // frmBackup
            // 
            AcceptButton = btnSaoLuu;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnHuy;
            ClientSize = new Size(460, 284);
            Controls.Add(btnHuy);
            Controls.Add(btnSaoLuu);
            Controls.Add(lblNoiDung);
            Controls.Add(lblTieuDe);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmBackup";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Sao lưu database";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
