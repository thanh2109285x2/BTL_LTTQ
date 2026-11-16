using BTL_LTTQ.Common.Reports;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.Reports.Controls
{
    /// <summary>
    /// Lớp phủ hiển thị trạng thái trống / đang tải / có dữ liệu / lỗi
    /// cho các vùng nội dung của Report Builder.
    /// </summary>
    public sealed class UcStateOverlay : Panel
    {
        private readonly Label _lblBieuTuong;
        private readonly Label _lblTieuDe;
        private readonly Label _lblMoTa;
        private readonly AppButton _btnHanhDong;
        private Action? _hanhDong;

        public UcStateOverlay()
        {
            Dock = DockStyle.Fill;
            BackColor = ReportDesignTokens.NenTrang;
            Visible = false;
            DoubleBuffered = true;

            var hop = new TableLayoutPanel
            {
                ColumnCount = 1,
                RowCount = 4,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Anchor = AnchorStyles.None,
                BackColor = Color.Transparent
            };
            hop.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            hop.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            hop.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            hop.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            hop.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            _lblBieuTuong = new Label
            {
                Text = "📄",
                Font = new Font("Segoe UI Emoji", 30F),
                AutoSize = true,
                Anchor = AnchorStyles.None,
                ForeColor = ReportDesignTokens.ChuMo
            };

            _lblTieuDe = new Label
            {
                Text = "Chưa có dữ liệu",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = ReportDesignTokens.ChuChinh,
                AutoSize = true,
                Anchor = AnchorStyles.None,
                Margin = new Padding(0, ReportDesignTokens.Khoang8, 0, 0)
            };

            _lblMoTa = new Label
            {
                Text = "",
                Font = ReportDesignTokens.ChuThuong,
                ForeColor = ReportDesignTokens.ChuPhu,
                AutoSize = false,
                MaximumSize = new Size(420, 0),
                TextAlign = ContentAlignment.MiddleCenter,
                Anchor = AnchorStyles.None,
                Margin = new Padding(0, ReportDesignTokens.Khoang8, 0, ReportDesignTokens.Khoang12)
            };

            _btnHanhDong = ReportUiFactory.Nut("Thực hiện", KieuNut.Chinh, 130);
            _btnHanhDong.Anchor = AnchorStyles.None;
            _btnHanhDong.Visible = false;
            _btnHanhDong.Click += (_, _) => _hanhDong?.Invoke();

            hop.Controls.Add(_lblBieuTuong, 0, 0);
            hop.Controls.Add(_lblTieuDe, 0, 1);
            hop.Controls.Add(_lblMoTa, 0, 2);
            hop.Controls.Add(_btnHanhDong, 0, 3);

            Controls.Add(hop);

            Resize += (_, _) =>
            {
                hop.Left = Math.Max(0, (ClientSize.Width - hop.Width) / 2);
                hop.Top = Math.Max(0, (ClientSize.Height - hop.Height) / 2);
            };
            hop.SizeChanged += (_, _) =>
            {
                hop.Left = Math.Max(0, (ClientSize.Width - hop.Width) / 2);
                hop.Top = Math.Max(0, (ClientSize.Height - hop.Height) / 2);
            };
        }

        public void Hien(TrangThaiXemTruoc trangThai, string tieuDe, string moTa,
            string? nhanNut = null, Action? hanhDong = null)
        {
            _lblBieuTuong.Text = trangThai switch
            {
                TrangThaiXemTruoc.DangTai => "⏳",
                TrangThaiXemTruoc.Loi => "⚠️",
                TrangThaiXemTruoc.Trong => "📄",
                _ => "✅"
            };

            _lblBieuTuong.ForeColor = trangThai switch
            {
                TrangThaiXemTruoc.Loi => ReportDesignTokens.NguyHiem,
                TrangThaiXemTruoc.Trong => ReportDesignTokens.ChuMo,
                _ => ReportDesignTokens.Nhan
            };

            _lblTieuDe.Text = tieuDe;
            _lblMoTa.Text = moTa;
            _hanhDong = hanhDong;

            if (!string.IsNullOrWhiteSpace(nhanNut) && hanhDong != null)
            {
                _btnHanhDong.Text = nhanNut;
                _btnHanhDong.Visible = true;
            }
            else
            {
                _btnHanhDong.Visible = false;
            }

            Visible = true;
            BringToFront();
        }

        public void An()
        {
            Visible = false;
        }
    }
}
