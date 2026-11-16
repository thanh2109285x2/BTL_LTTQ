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
        private System.Windows.Forms.Panel panelTieuDe = null!;
        private System.Windows.Forms.Label lblTieuDe = null!;
        private System.Windows.Forms.Label lblMoTa = null!;

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
            btnGui = new AppButton { Kieu = KieuNut.Chinh };
            panelTieuDe = new Panel();
            lblTieuDe = new Label();
            lblMoTa = new Label();
            panelNhap.SuspendLayout();
            panelTieuDe.SuspendLayout();
            SuspendLayout();
            //
            // flpTinNhan
            //
            flpTinNhan.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            flpTinNhan.AutoScroll = true;
            flpTinNhan.BackColor = Color.FromArgb(246, 248, 252);
            flpTinNhan.FlowDirection = FlowDirection.TopDown;
            flpTinNhan.Location = new Point(0, 82);
            flpTinNhan.Margin = new Padding(3, 4, 3, 4);
            flpTinNhan.Name = "flpTinNhan";
            flpTinNhan.Padding = new Padding(9, 11, 9, 11);
            flpTinNhan.Size = new Size(914, 505);
            flpTinNhan.TabIndex = 0;
            flpTinNhan.WrapContents = false;
            flpTinNhan.SizeChanged += flpTinNhan_SizeChanged;
            //
            // panelNhap
            //
            panelNhap.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelNhap.Controls.Add(txtCauHoi);
            panelNhap.Controls.Add(btnGui);
            panelNhap.BackColor = Color.White;
            panelNhap.Paint += panelNhap_Paint;
            panelNhap.Location = new Point(0, 587);
            panelNhap.Margin = new Padding(3, 4, 3, 4);
            panelNhap.Name = "panelNhap";
            panelNhap.Padding = new Padding(11);
            panelNhap.Size = new Size(914, 80);
            panelNhap.TabIndex = 1;
            //
            // txtCauHoi
            //
            txtCauHoi.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtCauHoi.Font = new Font("Segoe UI", 10.5F);
            txtCauHoi.BackColor = Color.White;
            txtCauHoi.ForeColor = Color.FromArgb(39, 48, 61);
            txtCauHoi.BorderStyle = BorderStyle.FixedSingle;
            txtCauHoi.Location = new Point(20, 24);
            txtCauHoi.Margin = new Padding(3, 4, 3, 4);
            txtCauHoi.Name = "txtCauHoi";
            txtCauHoi.PlaceholderText = "Nhập câu hỏi của bạn...";
            txtCauHoi.Size = new Size(750, 30);
            txtCauHoi.TabIndex = 0;
            txtCauHoi.KeyDown += txtCauHoi_KeyDown;
            //
            // btnGui
            //
            btnGui.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnGui.Location = new Point(790, 15);
            btnGui.Margin = new Padding(3, 4, 3, 4);
            btnGui.Name = "btnGui";
            btnGui.Size = new Size(104, 46);
            btnGui.TabIndex = 1;
            btnGui.Text = "Gửi  ➤";
            btnGui.Click += btnGui_Click;
            //
            // panelTieuDe
            //
            panelTieuDe.BackColor = Color.FromArgb(30, 67, 120);
            panelTieuDe.Controls.Add(lblMoTa);
            panelTieuDe.Controls.Add(lblTieuDe);
            panelTieuDe.Dock = DockStyle.Top;
            panelTieuDe.Location = new Point(0, 0);
            panelTieuDe.Name = "panelTieuDe";
            panelTieuDe.Padding = new Padding(24, 10, 24, 8);
            panelTieuDe.Size = new Size(914, 82);
            panelTieuDe.TabIndex = 2;
            //
            // lblTieuDe
            //
            lblTieuDe.AutoSize = true;
            lblTieuDe.Font = new Font("Segoe UI Semibold", 17F, FontStyle.Bold);
            lblTieuDe.ForeColor = Color.White;
            lblTieuDe.Location = new Point(22, 8);
            lblTieuDe.Name = "lblTieuDe";
            lblTieuDe.Size = new Size(137, 40);
            lblTieuDe.TabIndex = 0;
            lblTieuDe.Text = "Trợ lý AI";
            //
            // lblMoTa
            //
            lblMoTa.AutoSize = true;
            lblMoTa.Font = new Font("Segoe UI", 9F);
            lblMoTa.ForeColor = Color.FromArgb(220, 233, 250);
            lblMoTa.Location = new Point(25, 49);
            lblMoTa.Name = "lblMoTa";
            lblMoTa.Size = new Size(250, 20);
            lblMoTa.TabIndex = 1;
            lblMoTa.Text = "Hỏi đáp và tra cứu thông tin lớp học";
            //
            // frmTroLyAi
            //
            AcceptButton = btnGui;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(246, 248, 252);
            ClientSize = new Size(914, 667);
            Controls.Add(panelTieuDe);
            Controls.Add(flpTinNhan);
            Controls.Add(panelNhap);
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(546, 464);
            Name = "frmTroLyAi";
            Text = "Trợ lý AI";
            panelNhap.ResumeLayout(false);
            panelNhap.PerformLayout();
            panelTieuDe.ResumeLayout(false);
            panelTieuDe.PerformLayout();
            ResumeLayout(false);
        }
    }
}
