using BTL_LTTQ;

namespace BTL_LTTQ.Forms.HeThong
{
    partial class frmRestore
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTieuDe;
        private Label lblNoiDung;
        private AppButton btnKhoiPhuc;
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
            btnKhoiPhuc = new AppButton();
            btnHuy = new AppButton();
            SuspendLayout();
            // 
            // lblTieuDe
            // 
            lblTieuDe.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
            lblTieuDe.Location = new Point(32, 20);
            lblTieuDe.Name = "lblTieuDe";
            lblTieuDe.Size = new Size(396, 36);
            lblTieuDe.TabIndex = 0;
            lblTieuDe.Text = "KHÔI PHỤC DATABASE";
            lblTieuDe.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblNoiDung
            // 
            lblNoiDung.AutoSize = true;
            lblNoiDung.Location = new Point(96, 118);
            lblNoiDung.Name = "lblNoiDung";
            lblNoiDung.Size = new Size(256, 20);
            lblNoiDung.TabIndex = 1;
            lblNoiDung.Text = "Chọn file .bak để khôi phục database.";
            // 
            // btnKhoiPhuc
            // 
            btnKhoiPhuc.BackColor = Color.White;
            btnKhoiPhuc.FlatAppearance.BorderColor = Color.FromArgb(200, 208, 220);
            btnKhoiPhuc.FlatAppearance.MouseDownBackColor = Color.FromArgb(234, 241, 250);
            btnKhoiPhuc.FlatAppearance.MouseOverBackColor = Color.FromArgb(234, 241, 250);
            btnKhoiPhuc.FlatStyle = FlatStyle.Flat;
            btnKhoiPhuc.ForeColor = Color.FromArgb(33, 37, 41);
            btnKhoiPhuc.Location = new Point(54, 196);
            btnKhoiPhuc.Name = "btnKhoiPhuc";
            btnKhoiPhuc.Size = new Size(132, 36);
            btnKhoiPhuc.TabIndex = 2;
            btnKhoiPhuc.Text = "Khôi phục";
            btnKhoiPhuc.UseVisualStyleBackColor = false;
            btnKhoiPhuc.Click += btnKhoiPhuc_Click;
            // 
            // btnHuy
            // 
            btnHuy.BackColor = Color.White;
            btnHuy.DialogResult = DialogResult.Cancel;
            btnHuy.FlatAppearance.BorderColor = Color.FromArgb(200, 208, 220);
            btnHuy.FlatAppearance.MouseDownBackColor = Color.FromArgb(234, 241, 250);
            btnHuy.FlatAppearance.MouseOverBackColor = Color.FromArgb(234, 241, 250);
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.ForeColor = Color.FromArgb(33, 37, 41);
            btnHuy.Location = new Point(266, 196);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new Size(86, 36);
            btnHuy.TabIndex = 3;
            btnHuy.Text = "Hủy";
            btnHuy.UseVisualStyleBackColor = false;
            btnHuy.Click += btnHuy_Click;
            // 
            // frmRestore
            // 
            AcceptButton = btnKhoiPhuc;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnHuy;
            ClientSize = new Size(460, 284);
            Controls.Add(btnHuy);
            Controls.Add(btnKhoiPhuc);
            Controls.Add(lblNoiDung);
            Controls.Add(lblTieuDe);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmRestore";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Khôi phục database";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
