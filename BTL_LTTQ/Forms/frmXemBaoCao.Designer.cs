// Bố cục form báo cáo chỉnh được trong WinForms Designer.
namespace BTL_LTTQ.Forms
{
    partial class frmXemBaoCao
    {
        private System.ComponentModel.IContainer? components = null;
        private System.Windows.Forms.Panel panelTieuDe = null!;
        private System.Windows.Forms.Label lblTieuDe = null!;
        private System.Windows.Forms.Label lblThongTin = null!;
        private System.Windows.Forms.FlowLayoutPanel flowHanhDong = null!;
        private System.Windows.Forms.Button btnDong = null!;
        private System.Windows.Forms.Button btnXuatExcel = null!;
        private System.Windows.Forms.Button btnXuatPdf = null!;
        private System.Windows.Forms.Button btnInBaoCao = null!;
        private System.Windows.Forms.DataGridView dgvBaoCao = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _printDocument.Dispose();
                _previewDialog.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelTieuDe = new Panel();
            lblTieuDe = new Label();
            lblThongTin = new Label();
            flowHanhDong = new FlowLayoutPanel();
            btnInBaoCao = new Button();
            btnXuatPdf = new Button();
            btnXuatExcel = new Button();
            btnDong = new Button();
            dgvBaoCao = new DataGridView();
            panelTieuDe.SuspendLayout();
            flowHanhDong.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBaoCao).BeginInit();
            SuspendLayout();
            // 
            // panelTieuDe
            // 
            panelTieuDe.BackColor = Color.White;
            panelTieuDe.Controls.Add(lblTieuDe);
            panelTieuDe.Controls.Add(lblThongTin);
            panelTieuDe.Controls.Add(flowHanhDong);
            panelTieuDe.Dock = DockStyle.Top;
            panelTieuDe.Location = new Point(0, 0);
            panelTieuDe.Margin = new Padding(3, 4, 3, 4);
            panelTieuDe.Name = "panelTieuDe";
            panelTieuDe.Padding = new Padding(21, 13, 21, 11);
            panelTieuDe.Size = new Size(1353, 165);
            panelTieuDe.TabIndex = 0;
            // 
            // lblTieuDe
            // 
            lblTieuDe.Dock = DockStyle.Top;
            lblTieuDe.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTieuDe.ForeColor = Color.FromArgb(41, 98, 168);
            lblTieuDe.Location = new Point(21, 46);
            lblTieuDe.Name = "lblTieuDe";
            lblTieuDe.Size = new Size(1311, 56);
            lblTieuDe.TabIndex = 0;
            lblTieuDe.Text = "TIÊU ĐỀ BÁO CÁO";
            lblTieuDe.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblThongTin
            // 
            lblThongTin.Dock = DockStyle.Top;
            lblThongTin.ForeColor = Color.DimGray;
            lblThongTin.Location = new Point(21, 13);
            lblThongTin.Name = "lblThongTin";
            lblThongTin.Size = new Size(1311, 33);
            lblThongTin.TabIndex = 1;
            lblThongTin.Text = "Ngày lập báo cáo";
            lblThongTin.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // flowHanhDong
            // 
            flowHanhDong.Controls.Add(btnInBaoCao);
            flowHanhDong.Controls.Add(btnXuatPdf);
            flowHanhDong.Controls.Add(btnXuatExcel);
            flowHanhDong.Controls.Add(btnDong);
            flowHanhDong.Dock = DockStyle.Bottom;
            flowHanhDong.FlowDirection = FlowDirection.RightToLeft;
            flowHanhDong.Location = new Point(21, 109);
            flowHanhDong.Margin = new Padding(3, 4, 3, 4);
            flowHanhDong.Name = "flowHanhDong";
            flowHanhDong.Size = new Size(1311, 45);
            flowHanhDong.TabIndex = 2;
            flowHanhDong.WrapContents = false;
            // 
            // btnInBaoCao
            // 
            btnInBaoCao.Location = new Point(1183, 1);
            btnInBaoCao.Margin = new Padding(7, 1, 0, 1);
            btnInBaoCao.Name = "btnInBaoCao";
            btnInBaoCao.Size = new Size(128, 43);
            btnInBaoCao.TabIndex = 0;
            btnInBaoCao.Text = "In báo cáo";
            btnInBaoCao.UseVisualStyleBackColor = true;
            btnInBaoCao.Click += btnInBaoCao_Click;
            // 
            // btnXuatPdf
            // 
            btnXuatPdf.Location = new Point(1048, 1);
            btnXuatPdf.Margin = new Padding(7, 1, 0, 1);
            btnXuatPdf.Name = "btnXuatPdf";
            btnXuatPdf.Size = new Size(128, 43);
            btnXuatPdf.TabIndex = 1;
            btnXuatPdf.Text = "Xuất PDF";
            btnXuatPdf.UseVisualStyleBackColor = true;
            btnXuatPdf.Click += btnXuatPdf_Click;
            // 
            // btnXuatExcel
            // 
            btnXuatExcel.Location = new Point(913, 1);
            btnXuatExcel.Margin = new Padding(7, 1, 0, 1);
            btnXuatExcel.Name = "btnXuatExcel";
            btnXuatExcel.Size = new Size(128, 43);
            btnXuatExcel.TabIndex = 2;
            btnXuatExcel.Text = "Xuất Excel";
            btnXuatExcel.UseVisualStyleBackColor = true;
            btnXuatExcel.Click += btnXuatExcel_Click;
            // 
            // btnDong
            // 
            btnDong.Location = new Point(778, 1);
            btnDong.Margin = new Padding(7, 1, 0, 1);
            btnDong.Name = "btnDong";
            btnDong.Size = new Size(128, 43);
            btnDong.TabIndex = 3;
            btnDong.Text = "Đóng";
            btnDong.UseVisualStyleBackColor = true;
            btnDong.Click += btnDong_Click;
            // 
            // dgvBaoCao
            // 
            dgvBaoCao.AllowUserToAddRows = false;
            dgvBaoCao.AllowUserToDeleteRows = false;
            dgvBaoCao.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBaoCao.BackgroundColor = Color.White;
            dgvBaoCao.BorderStyle = BorderStyle.None;
            dgvBaoCao.ColumnHeadersHeight = 29;
            dgvBaoCao.Dock = DockStyle.Fill;
            dgvBaoCao.Location = new Point(0, 165);
            dgvBaoCao.Margin = new Padding(3, 4, 3, 4);
            dgvBaoCao.Name = "dgvBaoCao";
            dgvBaoCao.ReadOnly = true;
            dgvBaoCao.RowHeadersVisible = false;
            dgvBaoCao.RowHeadersWidth = 51;
            dgvBaoCao.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBaoCao.Size = new Size(1353, 716);
            dgvBaoCao.TabIndex = 1;
            dgvBaoCao.CellContentClick += dgvBaoCao_CellContentClick;
            // 
            // frmXemBaoCao
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 247, 251);
            ClientSize = new Size(1353, 881);
            Controls.Add(dgvBaoCao);
            Controls.Add(panelTieuDe);
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(912, 718);
            Name = "frmXemBaoCao";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Xem trước báo cáo";
            panelTieuDe.ResumeLayout(false);
            flowHanhDong.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvBaoCao).EndInit();
            ResumeLayout(false);
        }
    }
}
