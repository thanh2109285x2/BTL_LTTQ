using BTL_LTTQ.BUS.Reports;
using BTL_LTTQ.Common.Reports;
using BTL_LTTQ.Models.Reports;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.Reports.Controls
{
    /// <summary>
    /// Danh sách trường dữ liệu của nguồn đang chọn, nhóm theo mục, kèm ô tìm kiếm.
    /// Tích chọn một trường để thêm cột vào báo cáo; bỏ chọn để loại cột.
    /// </summary>
    public sealed class UcFieldList : UserControl
    {
        private readonly ReportEditorState _trangThai;
        private readonly TextBox _txtTim;
        private readonly TableLayoutPanel _bang;
        private bool _dangDung;

        public event EventHandler? ThayDoi;

        public UcFieldList(ReportEditorState trangThai)
        {
            _trangThai = trangThai ?? throw new ArgumentNullException(nameof(trangThai));
            Dock = DockStyle.Fill;
            BackColor = ReportDesignTokens.NenTrang;

            var dau = new Panel
            {
                Dock = DockStyle.Top,
                Height = 52,
                BackColor = ReportDesignTokens.NenTrang,
                Padding = new Padding(ReportDesignTokens.Khoang8, ReportDesignTokens.Khoang8,
                                       ReportDesignTokens.Khoang8, ReportDesignTokens.Khoang8)
            };

            _txtTim = ReportUiFactory.OTimKiem("Tìm trường dữ liệu…");
            _txtTim.Dock = DockStyle.Top;
            _txtTim.TextChanged += (_, _) => DungDanhSach();
            dau.Controls.Add(_txtTim);

            _bang = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                ColumnCount = 1,
                BackColor = ReportDesignTokens.NenTrang,
                Padding = new Padding(ReportDesignTokens.Khoang8, 0, ReportDesignTokens.Khoang8, ReportDesignTokens.Khoang8)
            };
            _bang.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            Controls.Add(_bang);
            Controls.Add(dau);
        }

        public void Nap() => DungDanhSach();

        private void DungDanhSach()
        {
            _dangDung = true;
            try
            {
                _bang.SuspendLayout();
                _bang.Controls.Clear();
                _bang.RowStyles.Clear();
                _bang.RowCount = 0;

                var nguon = _trangThai.Nguon;
                if (nguon == null)
                {
                    ThemThongBao("Chưa chọn nguồn dữ liệu.");
                    return;
                }

                string tuKhoa = _txtTim.Text.Trim();
                var daChon = _trangThai.DinhNghia.Cot
                    .Where(c => c.HienThi)
                    .Select(c => c.MaTruong)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                int soHien = 0;
                foreach (string nhom in nguon.CacNhomTruong())
                {
                    var truongNhom = nguon.Truong
                        .Where(t => string.Equals(t.Nhom, nhom, StringComparison.OrdinalIgnoreCase))
                        .Where(t => Khop(t, tuKhoa))
                        .ToList();

                    if (truongNhom.Count == 0)
                        continue;

                    ThemTieuDeMuc(nhom, truongNhom.Count);
                    foreach (var truong in truongNhom)
                    {
                        ThemDongTruong(truong, daChon.Contains(truong.Ma));
                        soHien++;
                    }
                }

                if (soHien == 0)
                    ThemThongBao(string.IsNullOrWhiteSpace(tuKhoa)
                        ? "Nguồn này chưa có trường dữ liệu."
                        : $"Không tìm thấy trường nào khớp \"{tuKhoa}\".");
            }
            finally
            {
                _bang.ResumeLayout();
                _dangDung = false;
            }
        }

        private static bool Khop(FieldDefinition truong, string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa))
                return true;

            return truong.TieuDe.Contains(tuKhoa, StringComparison.CurrentCultureIgnoreCase) ||
                   truong.Ma.Contains(tuKhoa, StringComparison.OrdinalIgnoreCase) ||
                   truong.Nhom.Contains(tuKhoa, StringComparison.CurrentCultureIgnoreCase);
        }

        private void ThemTieuDeMuc(string nhom, int soLuong)
        {
            var lbl = new Label
            {
                Text = $"{nhom}  ({soLuong})",
                Dock = DockStyle.Fill,
                Height = 28,
                Font = ReportDesignTokens.ChuTieuDeThe,
                ForeColor = ReportDesignTokens.ChuChinh,
                BackColor = ReportDesignTokens.NenHeader,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(ReportDesignTokens.Khoang8, 0, 0, 0),
                Margin = new Padding(0, ReportDesignTokens.Khoang8, 0, 2)
            };
            _bang.RowCount++;
            _bang.RowStyles.Add(new RowStyle(SizeType.Absolute, 32));
            _bang.Controls.Add(lbl, 0, _bang.RowCount - 1);
        }

        private void ThemDongTruong(FieldDefinition truong, bool daChon)
        {
            var dong = new Panel
            {
                Dock = DockStyle.Fill,
                Height = 28,
                BackColor = ReportDesignTokens.NenTrang,
                Margin = new Padding(0, 1, 0, 1)
            };

            var chon = new CheckBox
            {
                Text = truong.TieuDe,
                Checked = daChon,
                AutoSize = true,
                Dock = DockStyle.Left,
                Font = ReportDesignTokens.ChuThuong,
                ForeColor = ReportDesignTokens.ChuChinh,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Padding = new Padding(2, 0, 0, 0),
                Tag = truong.Ma
            };
            chon.CheckedChanged += Chon_CheckedChanged;

            string ghiChu = ReportEnumText.Ten(truong.Kieu);
            if (truong.CoTheNhom) ghiChu += " • nhóm";
            if (truong.CoTheTongHop) ghiChu += " • tổng";

            var loai = new Label
            {
                Text = ghiChu,
                AutoSize = true,
                Dock = DockStyle.Right,
                Font = ReportDesignTokens.ChuNho,
                ForeColor = ReportDesignTokens.ChuMo,
                TextAlign = ContentAlignment.MiddleRight,
                Padding = new Padding(0, 0, ReportDesignTokens.Khoang8, 0)
            };

            dong.Controls.Add(loai);
            dong.Controls.Add(chon);

            _bang.RowCount++;
            _bang.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            _bang.Controls.Add(dong, 0, _bang.RowCount - 1);
        }

        private void Chon_CheckedChanged(object? sender, EventArgs e)
        {
            if (_dangDung || sender is not CheckBox chon || chon.Tag is not string ma)
                return;

            var cot = _trangThai.DinhNghia.Cot
                .FirstOrDefault(c => string.Equals(c.MaTruong, ma, StringComparison.OrdinalIgnoreCase));

            if (chon.Checked)
            {
                if (cot != null)
                {
                    if (!cot.HienThi)
                        cot.HienThi = true;
                }
                else
                {
                    var truong = _trangThai.Nguon?.TimTruong(ma);
                    _trangThai.DinhNghia.Cot.Add(new ReportColumnDef
                    {
                        MaTruong = ma,
                        TieuDe = truong?.TieuDe ?? ma,
                        DoRong = truong?.DoRong ?? 120,
                        CanLe = truong?.CanLeMacDinh ?? CanLeCot.Trai,
                        DinhDang = truong?.DinhDangMacDinh ?? "",
                        HienThi = true,
                        ThuTu = _trangThai.DinhNghia.Cot.Count
                    });
                }
            }
            else if (cot != null)
            {
                _trangThai.DinhNghia.Cot.Remove(cot);
            }

            _trangThai.DinhNghia.SapXepLaiThuTuCot();
            ThayDoi?.Invoke(this, EventArgs.Empty);
        }

        private void ThemThongBao(string noiDung)
        {
            var lbl = new Label
            {
                Text = noiDung,
                Dock = DockStyle.Fill,
                Height = 60,
                Font = ReportDesignTokens.ChuThuong,
                ForeColor = ReportDesignTokens.ChuMo,
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(0, ReportDesignTokens.Khoang16, 0, 0)
            };
            _bang.RowCount++;
            _bang.RowStyles.Add(new RowStyle(SizeType.Absolute, 64));
            _bang.Controls.Add(lbl, 0, _bang.RowCount - 1);
        }
    }
}
