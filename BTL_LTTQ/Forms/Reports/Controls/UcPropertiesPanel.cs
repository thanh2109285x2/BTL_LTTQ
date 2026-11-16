using BTL_LTTQ.BUS.Reports;
using BTL_LTTQ.Common.Reports;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.Reports.Controls
{
    /// <summary>
    /// Bảng thuộc tính báo cáo: thông tin chung (tên, mô tả, tiêu đề in) và bố cục trang in.
    /// Mọi thay đổi được ghi thẳng vào <see cref="ReportEditorState.DinhNghia"/> và phát sự kiện.
    /// </summary>
    public sealed class UcPropertiesPanel : UserControl
    {
        private readonly ReportEditorState _trangThai;

        private readonly TextBox _txtTen;
        private readonly TextBox _txtMoTa;
        private readonly TextBox _txtTieuDe;
        private readonly TextBox _txtPhuDe;
        private readonly NumericUpDown _numGioiHan;
        private readonly CheckBox _chkCongKhai;

        private readonly ComboBox _cboKho;
        private readonly ComboBox _cboHuong;
        private readonly NumericUpDown _numLeTren;
        private readonly NumericUpDown _numLeDuoi;
        private readonly NumericUpDown _numLeTrai;
        private readonly NumericUpDown _numLePhai;
        private readonly NumericUpDown _numCoChu;
        private readonly ComboBox _cboKieuChu;
        private readonly CheckBox _chkDuongKe;
        private readonly CheckBox _chkHienTong;
        private readonly CheckBox _chkSoTrang;
        private readonly CheckBox _chkLapTieuDe;
        private readonly CheckBox _chkNenXenKe;

        private bool _dangNap;

        public event EventHandler? ThayDoi;

        public UcPropertiesPanel(ReportEditorState trangThai)
        {
            _trangThai = trangThai ?? throw new ArgumentNullException(nameof(trangThai));
            Dock = DockStyle.Fill;
            BackColor = ReportDesignTokens.NenTrang;

            _txtTen = ReportUiFactory.OChu();
            _txtMoTa = ReportUiFactory.OChu();
            _txtTieuDe = ReportUiFactory.OChu();
            _txtPhuDe = ReportUiFactory.OChu();
            _numGioiHan = ReportUiFactory.OSo(1, 20000, 1000);
            _chkCongKhai = ReportUiFactory.OChonHop("Cho phép người khác xem báo cáo");

            _cboKho = ReportUiFactory.OChon();
            _cboKho.Items.AddRange(new object[]
            {
                new LuaChon<KhoGiay>(KhoGiay.A4, ReportEnumText.Ten(KhoGiay.A4)),
                new LuaChon<KhoGiay>(KhoGiay.A3, ReportEnumText.Ten(KhoGiay.A3))
            });

            _cboHuong = ReportUiFactory.OChon();
            _cboHuong.Items.AddRange(new object[]
            {
                new LuaChon<HuongGiay>(HuongGiay.Doc, ReportEnumText.Ten(HuongGiay.Doc)),
                new LuaChon<HuongGiay>(HuongGiay.Ngang, ReportEnumText.Ten(HuongGiay.Ngang))
            });

            _numLeTren = ReportUiFactory.OSo(0, 120, 15, 70);
            _numLeDuoi = ReportUiFactory.OSo(0, 120, 15, 70);
            _numLeTrai = ReportUiFactory.OSo(0, 120, 15, 70);
            _numLePhai = ReportUiFactory.OSo(0, 120, 15, 70);
            _numCoChu = ReportUiFactory.OSo(6, 24, 9.5m, 70);
            _numCoChu.DecimalPlaces = 1;
            _numCoChu.Increment = 0.5m;

            _cboKieuChu = ReportUiFactory.OChon();
            _cboKieuChu.Items.AddRange(new object[] { "Segoe UI", "Arial", "Times New Roman", "Calibri", "Tahoma", "Verdana" });

            _chkDuongKe = ReportUiFactory.OChonHop("Hiện đường kẻ");
            _chkHienTong = ReportUiFactory.OChonHop("Hiện dòng tổng cộng");
            _chkSoTrang = ReportUiFactory.OChonHop("Hiện số trang");
            _chkLapTieuDe = ReportUiFactory.OChonHop("Lặp tiêu đề ở mỗi trang");
            _chkNenXenKe = ReportUiFactory.OChonHop("Nền xen kẽ dòng");

            var khung = ReportUiFactory.KhungCuon();
            var bang = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 1,
                BackColor = ReportDesignTokens.NenTrang,
                Padding = new Padding(ReportDesignTokens.Khoang12)
            };
            bang.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            void Muc(string tieuDe)
            {
                bang.RowCount++;
                bang.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                var lbl = ReportUiFactory.TieuDeMuc(tieuDe);
                lbl.Margin = new Padding(0, ReportDesignTokens.Khoang12, 0, ReportDesignTokens.Khoang4);
                bang.Controls.Add(lbl, 0, bang.RowCount - 1);
            }

            void O(string nhan, Control control)
            {
                control.Width = 240;
                var hop = ReportUiFactory.OCoNhan(nhan, control, 240);
                hop.Width = 260;
                hop.Margin = new Padding(0, 0, 0, ReportDesignTokens.Khoang4);
                bang.RowCount++;
                bang.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                bang.Controls.Add(hop, 0, bang.RowCount - 1);
            }

            void Hai(string nhan, params Control[] controls)
            {
                var panel = new FlowLayoutPanel
                {
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    FlowDirection = FlowDirection.LeftToRight,
                    WrapContents = false,
                    Margin = new Padding(0, 0, 0, ReportDesignTokens.Khoang4)
                };
                foreach (var c in controls)
                    panel.Controls.Add(c);

                var hop = new Panel
                {
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    Margin = new Padding(0, 0, 0, ReportDesignTokens.Khoang4)
                };
                var lbl = ReportUiFactory.Nhan(nhan);
                lbl.Location = new Point(0, 0);
                panel.Location = new Point(0, 20);
                hop.Controls.Add(panel);
                hop.Controls.Add(lbl);
                hop.Height = panel.Bottom + 2;
                hop.Width = 260;
                bang.RowCount++;
                bang.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                bang.Controls.Add(hop, 0, bang.RowCount - 1);
            }

            Muc("Thông tin báo cáo");
            O("Tên báo cáo", _txtTen);
            O("Mô tả ngắn", _txtMoTa);
            O("Tiêu đề in", _txtTieuDe);
            O("Phụ đề in", _txtPhuDe);
            O("Giới hạn số dòng", _numGioiHan);
            bang.RowCount++;
            bang.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            bang.Controls.Add(_chkCongKhai, 0, bang.RowCount - 1);

            Muc("Bố cục trang in");
            O("Khổ giấy", _cboKho);
            O("Hướng giấy", _cboHuong);
            Hai("Lề (trên / dưới / trái / phải mm)", _numLeTren, _numLeDuoi, _numLeTrai, _numLePhai);
            Hai("Cỡ chữ (pt)", _numCoChu);
            O("Kiểu chữ", _cboKieuChu);

            Muc("Tùy chọn hiển thị");
            bang.RowCount++;
            bang.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            var tuyChon = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Margin = new Padding(0)
            };
            tuyChon.Controls.AddRange(new Control[] { _chkDuongKe, _chkHienTong, _chkSoTrang, _chkLapTieuDe, _chkNenXenKe });
            bang.Controls.Add(tuyChon, 0, bang.RowCount - 1);

            khung.Controls.Add(bang);
            Controls.Add(khung);

            _txtTen.TextChanged += (_, _) => Ghi();
            _txtMoTa.TextChanged += (_, _) => Ghi();
            _txtTieuDe.TextChanged += (_, _) => Ghi();
            _txtPhuDe.TextChanged += (_, _) => Ghi();
            _numGioiHan.ValueChanged += (_, _) => Ghi();
            _chkCongKhai.CheckedChanged += (_, _) => Ghi();
            _cboKho.SelectedIndexChanged += (_, _) => Ghi();
            _cboHuong.SelectedIndexChanged += (_, _) => Ghi();
            _numLeTren.ValueChanged += (_, _) => Ghi();
            _numLeDuoi.ValueChanged += (_, _) => Ghi();
            _numLeTrai.ValueChanged += (_, _) => Ghi();
            _numLePhai.ValueChanged += (_, _) => Ghi();
            _numCoChu.ValueChanged += (_, _) => Ghi();
            _cboKieuChu.SelectedIndexChanged += (_, _) => Ghi();
            _chkDuongKe.CheckedChanged += (_, _) => Ghi();
            _chkHienTong.CheckedChanged += (_, _) => Ghi();
            _chkSoTrang.CheckedChanged += (_, _) => Ghi();
            _chkLapTieuDe.CheckedChanged += (_, _) => Ghi();
            _chkNenXenKe.CheckedChanged += (_, _) => Ghi();
        }

        public void Nap()
        {
            _dangNap = true;
            try
            {
                var d = _trangThai.DinhNghia;
                var b = d.BoCuc;

                _txtTen.Text = d.Ten;
                _txtMoTa.Text = d.MoTa;
                _txtTieuDe.Text = d.TieuDe;
                _txtPhuDe.Text = d.PhuDe;
                _numGioiHan.Value = Math.Clamp(d.GioiHanDong, 1, 20000);
                _chkCongKhai.Checked = d.CongKhai;

                ChonTheoGiaTri(_cboKho, b.KhoGiay);
                ChonTheoGiaTri(_cboHuong, b.HuongGiay);
                _numLeTren.Value = Math.Clamp(b.LeTren, 0, 120);
                _numLeDuoi.Value = Math.Clamp(b.LeDuoi, 0, 120);
                _numLeTrai.Value = Math.Clamp(b.LeTrai, 0, 120);
                _numLePhai.Value = Math.Clamp(b.LePhai, 0, 120);
                _numCoChu.Value = Math.Clamp((decimal)b.CoChu, 6, 24);
                _cboKieuChu.SelectedItem = _cboKieuChu.Items.Contains(b.KieuChu) ? b.KieuChu : "Segoe UI";

                _chkDuongKe.Checked = b.HienDuongKe;
                _chkHienTong.Checked = b.HienTongCong;
                _chkSoTrang.Checked = b.HienSoTrang;
                _chkLapTieuDe.Checked = b.LapLaiTieuDe;
                _chkNenXenKe.Checked = b.NenXenKe;
            }
            finally
            {
                _dangNap = false;
            }
        }

        private void Ghi()
        {
            if (_dangNap)
                return;

            var d = _trangThai.DinhNghia;
            var b = d.BoCuc;

            d.Ten = _txtTen.Text.Trim();
            d.MoTa = _txtMoTa.Text.Trim();
            d.TieuDe = _txtTieuDe.Text.Trim();
            d.PhuDe = _txtPhuDe.Text.Trim();
            d.GioiHanDong = (int)_numGioiHan.Value;
            d.CongKhai = _chkCongKhai.Checked;

            b.KhoGiay = LayGiaTri(_cboKho, KhoGiay.A4);
            b.HuongGiay = LayGiaTri(_cboHuong, HuongGiay.Doc);
            b.LeTren = (int)_numLeTren.Value;
            b.LeDuoi = (int)_numLeDuoi.Value;
            b.LeTrai = (int)_numLeTrai.Value;
            b.LePhai = (int)_numLePhai.Value;
            b.CoChu = (float)_numCoChu.Value;
            b.KieuChu = Convert.ToString(_cboKieuChu.SelectedItem) ?? "Segoe UI";

            b.HienDuongKe = _chkDuongKe.Checked;
            b.HienTongCong = _chkHienTong.Checked;
            b.HienSoTrang = _chkSoTrang.Checked;
            b.LapLaiTieuDe = _chkLapTieuDe.Checked;
            b.NenXenKe = _chkNenXenKe.Checked;

            ThayDoi?.Invoke(this, EventArgs.Empty);
        }

        private static void ChonTheoGiaTri<T>(ComboBox cbo, T giaTri)
        {
            for (int i = 0; i < cbo.Items.Count; i++)
            {
                if (cbo.Items[i] is LuaChon<T> lc && Equals(lc.GiaTri, giaTri))
                {
                    cbo.SelectedIndex = i;
                    return;
                }
            }

            if (cbo.Items.Count > 0)
                cbo.SelectedIndex = 0;
        }

        private static T LayGiaTri<T>(ComboBox cbo, T macDinh) =>
            cbo.SelectedItem is LuaChon<T> lc ? lc.GiaTri : macDinh;
    }
}
