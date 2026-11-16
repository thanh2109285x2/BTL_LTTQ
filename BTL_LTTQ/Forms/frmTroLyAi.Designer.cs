#nullable enable
namespace BTL_LTTQ.Forms
{
    partial class frmTroLyAi
    {
        private System.ComponentModel.IContainer? components = null;
        private System.Windows.Forms.FlowLayoutPanel flpTinNhan = null!;
        private System.Windows.Forms.Panel panelNhap = null!;
        private System.Windows.Forms.TextBox txtCauHoi = null!;
        private AppButton btnGui = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                components?.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            flpTinNhan = new FlowLayoutPanel();
            panelNhap = new Panel();
            txtCauHoi = new TextBox();
            btnGui = new AppButton { Kieu = KieuNut.Phu };
            panelNhap.SuspendLayout();
            SuspendLayout();
            // 
            // flpTinNhan
            // 
            flpTinNhan.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            flpTinNhan.AutoScroll = true;
            flpTinNhan.BackColor = Color.White;
            flpTinNhan.FlowDirection = FlowDirection.TopDown;
            flpTinNhan.Location = new Point(0, 0);
            flpTinNhan.Margin = new Padding(3, 4, 3, 4);
            flpTinNhan.Name = "flpTinNhan";
            flpTinNhan.Padding = new Padding(9, 11, 9, 11);
            flpTinNhan.Size = new Size(914, 587);
            flpTinNhan.TabIndex = 0;
            flpTinNhan.WrapContents = false;
            flpTinNhan.SizeChanged += flpTinNhan_SizeChanged;
            // 
            // panelNhap
            // 
            panelNhap.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelNhap.Controls.Add(txtCauHoi);
            panelNhap.Controls.Add(btnGui);
            panelNhap.Location = new Point(0, 597);
            panelNhap.Margin = new Padding(3, 4, 3, 4);
            panelNhap.Name = "panelNhap";
            panelNhap.Padding = new Padding(11, 11, 11, 11);
            panelNhap.Size = new Size(914, 69);
            panelNhap.TabIndex = 1;
            // 
            // txtCauHoi
            // 
            txtCauHoi.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtCauHoi.Font = new Font("Segoe UI", 10F);
            txtCauHoi.Location = new Point(14, 15);
            txtCauHoi.Margin = new Padding(3, 4, 3, 4);
            txtCauHoi.Name = "txtCauHoi";
            txtCauHoi.PlaceholderText = "Nhập tin nhắn...";
            txtCauHoi.Size = new Size(765, 30);
            txtCauHoi.TabIndex = 0;
            txtCauHoi.KeyDown += txtCauHoi_KeyDown;
            // 
            // btnGui
            // 
            btnGui.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnGui.Location = new Point(797, 9);
            btnGui.Margin = new Padding(3, 4, 3, 4);
            btnGui.Name = "btnGui";
            btnGui.Size = new Size(103, 48);
            btnGui.TabIndex = 1;
            btnGui.Text = "Gửi";
            btnGui.Click += btnGui_Click;
            // 
            // frmTroLyAi
            // 
            AcceptButton = btnGui;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(914, 667);
            Controls.Add(flpTinNhan);
            Controls.Add(panelNhap);
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(546, 464);
            Name = "frmTroLyAi";
            Text = "Trợ lý AI";
            panelNhap.ResumeLayout(false);
            panelNhap.PerformLayout();
            ResumeLayout(false);
        }
    }
}
