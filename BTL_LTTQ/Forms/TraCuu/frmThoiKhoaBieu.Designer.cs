namespace BTL_LTTQ.Forms.TraCuu
{
    partial class frmThoiKhoaBieu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            panelTieuDe = new Panel();
            lblMoTa = new Label();
            label1 = new Label();
            tabControl1 = new TabControl();
            tabPageTuan = new TabPage();
            dgvTuan = new DataGridView();
            panelDieuHuong = new Panel();
            panelChonTuan = new Panel();
            lblKhoang = new Label();
            btnTuanSau = new Button();
            btnTuanNay = new Button();
            btnTuanTruoc = new Button();
            calThang = new MonthCalendar();
            tabPageChiTiet = new TabPage();
            dgvChiTiet = new DataGridView();
            panelTieuDe.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPageTuan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTuan).BeginInit();
            panelDieuHuong.SuspendLayout();
            panelChonTuan.SuspendLayout();
            tabPageChiTiet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvChiTiet).BeginInit();
            SuspendLayout();
            // 
            // panelTieuDe
            // 
            panelTieuDe.Controls.Add(lblMoTa);
            panelTieuDe.Controls.Add(label1);
            panelTieuDe.Dock = DockStyle.Top;
            panelTieuDe.Location = new Point(0, 0);
            panelTieuDe.Name = "panelTieuDe";
            panelTieuDe.Size = new Size(1200, 84);
            panelTieuDe.TabIndex = 0;
            // 
            // lblMoTa
            // 
            lblMoTa.AutoSize = true;
            lblMoTa.Location = new Point(42, 50);
            lblMoTa.Name = "lblMoTa";
            lblMoTa.Size = new Size(0, 20);
            lblMoTa.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label1.Location = new Point(39, 14);
            label1.Name = "label1";
            label1.Size = new Size(154, 28);
            label1.TabIndex = 0;
            label1.Text = "Thời khóa biểu";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPageTuan);
            tabControl1.Controls.Add(tabPageChiTiet);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 84);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1200, 536);
            tabControl1.TabIndex = 1;
            // 
            // tabPageTuan
            // 
            tabPageTuan.Controls.Add(dgvTuan);
            tabPageTuan.Controls.Add(panelDieuHuong);
            tabPageTuan.Location = new Point(4, 29);
            tabPageTuan.Name = "tabPageTuan";
            tabPageTuan.Padding = new Padding(10);
            tabPageTuan.Size = new Size(1192, 503);
            tabPageTuan.TabIndex = 0;
            tabPageTuan.Text = "Lịch tuần";
            tabPageTuan.UseVisualStyleBackColor = true;
            // 
            // dgvTuan
            // 
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvTuan.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvTuan.ColumnHeadersHeight = 55;
            dgvTuan.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvTuan.DefaultCellStyle = dataGridViewCellStyle2;
            dgvTuan.Dock = DockStyle.Fill;
            dgvTuan.Location = new Point(10, 230);
            dgvTuan.Name = "dgvTuan";
            dgvTuan.RowHeadersWidth = 51;
            dgvTuan.RowTemplate.Height = 55;
            dgvTuan.RowTemplate.ReadOnly = true;
            dgvTuan.Size = new Size(1172, 263);
            dgvTuan.TabIndex = 1;
            // 
            // panelDieuHuong
            // 
            panelDieuHuong.Controls.Add(panelChonTuan);
            panelDieuHuong.Controls.Add(calThang);
            panelDieuHuong.Dock = DockStyle.Top;
            panelDieuHuong.Location = new Point(10, 10);
            panelDieuHuong.Name = "panelDieuHuong";
            panelDieuHuong.Padding = new Padding(6);
            panelDieuHuong.Size = new Size(1172, 220);
            panelDieuHuong.TabIndex = 0;
            // 
            // panelChonTuan
            // 
            panelChonTuan.Controls.Add(lblKhoang);
            panelChonTuan.Controls.Add(btnTuanSau);
            panelChonTuan.Controls.Add(btnTuanNay);
            panelChonTuan.Controls.Add(btnTuanTruoc);
            panelChonTuan.Location = new Point(433, 6);
            panelChonTuan.Name = "panelChonTuan";
            panelChonTuan.Size = new Size(640, 205);
            panelChonTuan.TabIndex = 1;
            // 
            // lblKhoang
            // 
            lblKhoang.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblKhoang.ForeColor = Color.FromArgb(41, 98, 168);
            lblKhoang.Location = new Point(4, 72);
            lblKhoang.Name = "lblKhoang";
            lblKhoang.Size = new Size(620, 133);
            lblKhoang.TabIndex = 5;
            lblKhoang.Text = "Tuần: --";
            // 
            // btnTuanSau
            // 
            btnTuanSau.Location = new Point(264, 12);
            btnTuanSau.Name = "btnTuanSau";
            btnTuanSau.Size = new Size(130, 40);
            btnTuanSau.TabIndex = 2;
            btnTuanSau.Text = "Tuần sau ▶";
            btnTuanSau.UseVisualStyleBackColor = true;
            btnTuanSau.Click += btnTuanSau_Click;
            // 
            // btnTuanNay
            // 
            btnTuanNay.Location = new Point(142, 12);
            btnTuanNay.Name = "btnTuanNay";
            btnTuanNay.Size = new Size(110, 40);
            btnTuanNay.TabIndex = 1;
            btnTuanNay.Text = "Tuần này";
            btnTuanNay.UseVisualStyleBackColor = true;
            btnTuanNay.Click += btnTuanNay_Click;
            // 
            // btnTuanTruoc
            // 
            btnTuanTruoc.Location = new Point(0, 12);
            btnTuanTruoc.Name = "btnTuanTruoc";
            btnTuanTruoc.Size = new Size(130, 40);
            btnTuanTruoc.TabIndex = 0;
            btnTuanTruoc.Text = "◀ Tuần trước";
            btnTuanTruoc.UseVisualStyleBackColor = true;
            btnTuanTruoc.Click += btnTuanTruoc_Click;
            // 
            // calThang
            // 
            calThang.Location = new Point(6, 6);
            calThang.MaxSelectionCount = 1;
            calThang.Name = "calThang";
            calThang.TabIndex = 0;
            calThang.DateChanged += calThang_DateChanged;
            // 
            // tabPageChiTiet
            // 
            tabPageChiTiet.Controls.Add(dgvChiTiet);
            tabPageChiTiet.Location = new Point(4, 29);
            tabPageChiTiet.Name = "tabPageChiTiet";
            tabPageChiTiet.Padding = new Padding(10);
            tabPageChiTiet.Size = new Size(1192, 503);
            tabPageChiTiet.TabIndex = 1;
            tabPageChiTiet.Text = "Chi tiết buổi học";
            tabPageChiTiet.UseVisualStyleBackColor = true;
            // 
            // dgvChiTiet
            // 
            dgvChiTiet.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvChiTiet.Dock = DockStyle.Fill;
            dgvChiTiet.Location = new Point(10, 10);
            dgvChiTiet.Name = "dgvChiTiet";
            dgvChiTiet.RowHeadersWidth = 51;
            dgvChiTiet.Size = new Size(1172, 483);
            dgvChiTiet.TabIndex = 0;
            // 
            // frmThoiKhoaBieu
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 620);
            Controls.Add(tabControl1);
            Controls.Add(panelTieuDe);
            Name = "frmThoiKhoaBieu";
            Text = "Thời khóa biểu";
            Load += frmThoiKhoaBieu_Load;
            panelTieuDe.ResumeLayout(false);
            panelTieuDe.PerformLayout();
            tabControl1.ResumeLayout(false);
            tabPageTuan.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvTuan).EndInit();
            panelDieuHuong.ResumeLayout(false);
            panelChonTuan.ResumeLayout(false);
            tabPageChiTiet.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvChiTiet).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelTieuDe;
        private Label label1;
        private Label lblMoTa;
        private TabControl tabControl1;
        private TabPage tabPageTuan;
        private DataGridView dgvTuan;
        private Panel panelDieuHuong;
        private Panel panelChonTuan;
        private MonthCalendar calThang;
        private Button btnTuanTruoc;
        private Button btnTuanNay;
        private Button btnTuanSau;
        private Label lblKhoang;
        private TabPage tabPageChiTiet;
        private DataGridView dgvChiTiet;
    }
}
