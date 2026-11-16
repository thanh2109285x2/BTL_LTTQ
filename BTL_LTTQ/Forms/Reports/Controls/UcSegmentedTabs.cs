using BTL_LTTQ.Common.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.Reports.Controls
{
    /// <summary>
    /// Dải tab dạng phân đoạn (segmented) cho vùng làm việc nhiều chế độ.
    /// Mỗi tab gắn một control nội dung; chỉ tab đang chọn được hiển thị.
    /// </summary>
    public sealed class UcSegmentedTabs : UserControl
    {
        private readonly FlowLayoutPanel _thanh;
        private readonly Panel _noiDung;
        private readonly List<Button> _nut = new();
        private readonly List<Control> _trang = new();
        private int _chon = -1;

        public event EventHandler? DoiTrang;

        public UcSegmentedTabs()
        {
            Dock = DockStyle.Fill;
            BackColor = ReportDesignTokens.NenTrang;

            _thanh = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 44,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = ReportDesignTokens.NenHeader,
                Padding = new Padding(ReportDesignTokens.Khoang8, ReportDesignTokens.Khoang8,
                                       ReportDesignTokens.Khoang8, ReportDesignTokens.Khoang4)
            };

            _noiDung = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ReportDesignTokens.NenTrang
            };

            Controls.Add(_noiDung);
            Controls.Add(_thanh);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int TrangDangChon => _chon;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Control? TrangHienTai => _chon >= 0 && _chon < _trang.Count ? _trang[_chon] : null;

        public void ThemTrang(string tieuDe, Control noiDung)
        {
            ArgumentNullException.ThrowIfNull(noiDung);

            int chiSo = _trang.Count;
            var nut = new Button
            {
                Text = tieuDe,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Height = 30,
                FlatStyle = FlatStyle.Flat,
                UseVisualStyleBackColor = false,
                Font = ReportDesignTokens.ChuThuong,
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, ReportDesignTokens.Khoang4, 0),
                Padding = new Padding(ReportDesignTokens.Khoang12, 0, ReportDesignTokens.Khoang12, 0),
                Tag = chiSo
            };
            nut.FlatAppearance.BorderSize = 0;
            nut.Click += (_, _) => Chon(chiSo);

            noiDung.Dock = DockStyle.Fill;
            noiDung.Visible = false;

            _thanh.Controls.Add(nut);
            _noiDung.Controls.Add(noiDung);
            _nut.Add(nut);
            _trang.Add(noiDung);

            if (_chon < 0)
                Chon(0);
            else
                CapNhatDinhDang();
        }

        public void Chon(int chiSo)
        {
            if (chiSo < 0 || chiSo >= _trang.Count)
                return;

            bool doi = _chon != chiSo;
            _chon = chiSo;

            for (int i = 0; i < _trang.Count; i++)
                _trang[i].Visible = i == chiSo;

            CapNhatDinhDang();

            if (doi)
                DoiTrang?.Invoke(this, EventArgs.Empty);
        }

        private void CapNhatDinhDang()
        {
            for (int i = 0; i < _nut.Count; i++)
            {
                bool chon = i == _chon;
                var nut = _nut[i];
                nut.BackColor = chon ? ReportDesignTokens.Nhan : ReportDesignTokens.NenHeader;
                nut.ForeColor = chon ? Color.White : ReportDesignTokens.ChuPhu;
                nut.Font = chon ? ReportDesignTokens.ChuNhan : ReportDesignTokens.ChuThuong;
            }
        }
    }
}
