using System;
using System.Drawing;
using System.Windows.Forms;

namespace BTL_LTTQ.Common.Reports
{
    /// <summary>
    /// Thẻ (card) có viền mảnh và vùng tiêu đề, dùng thống nhất cho mọi màn hình
    /// của Report Builder. Vùng nội dung nằm ở <see cref="Than"/>.
    /// </summary>
    public class ReportCard : Panel
    {
        private readonly Panel _header;
        private readonly Label _lblTieuDe;
        private readonly Label _lblPhuDe;
        private readonly Label _lblPhai;

        public Panel Than { get; }

        public ReportCard(string tieuDe = "", string phuDe = "")
        {
            BackColor = ReportDesignTokens.NenTrang;
            Padding = new Padding(1);
            DoubleBuffered = true;

            _header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 0,
                BackColor = ReportDesignTokens.NenHeader,
                Padding = new Padding(ReportDesignTokens.Khoang12, ReportDesignTokens.Khoang8,
                                       ReportDesignTokens.Khoang12, ReportDesignTokens.Khoang8)
            };

            _lblTieuDe = new Label
            {
                Text = tieuDe,
                Font = ReportDesignTokens.ChuTieuDeThe,
                ForeColor = ReportDesignTokens.ChuChinh,
                AutoSize = true,
                Location = new Point(ReportDesignTokens.Khoang12, 9)
            };

            _lblPhuDe = new Label
            {
                Text = phuDe,
                Font = ReportDesignTokens.ChuNho,
                ForeColor = ReportDesignTokens.ChuPhu,
                AutoSize = true,
                Location = new Point(ReportDesignTokens.Khoang12, 27)
            };

            _lblPhai = new Label
            {
                Font = ReportDesignTokens.ChuNho,
                ForeColor = ReportDesignTokens.ChuPhu,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleRight,
                Dock = DockStyle.Right,
                Width = 140,
                Padding = new Padding(0, 0, ReportDesignTokens.Khoang12, 0)
            };

            _header.Controls.Add(_lblPhai);
            _header.Controls.Add(_lblTieuDe);
            _header.Controls.Add(_lblPhuDe);
            _header.Height = phuDe.Length > 0 ? 50 : 36;

            Than = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ReportDesignTokens.NenTrang,
                Padding = new Padding(ReportDesignTokens.Khoang12)
            };

            Controls.Add(Than);
            Controls.Add(_header);
        }

        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public string TieuDe
        {
            get => _lblTieuDe.Text;
            set
            {
                _lblTieuDe.Text = value;
                CapNhatChieuCaoHeader();
            }
        }

        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public string PhuDe
        {
            get => _lblPhuDe.Text;
            set
            {
                _lblPhuDe.Text = value;
                _lblPhuDe.Visible = value.Length > 0;
                CapNhatChieuCaoHeader();
            }
        }

        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public string GhiChuPhai
        {
            get => _lblPhai.Text;
            set => _lblPhai.Text = value;
        }

        public void AnHeader()
        {
            _header.Visible = false;
        }

        private void CapNhatChieuCaoHeader()
        {
            if (!_header.Visible)
                return;

            _header.Height = _lblPhuDe.Text.Length > 0 ? 50 : 36;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using var but = new Pen(ReportDesignTokens.VienNhat);
            e.Graphics.DrawRectangle(but, 0, 0, Width - 1, Height - 1);
        }
    }
}
