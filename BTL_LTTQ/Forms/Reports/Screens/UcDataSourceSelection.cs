using BTL_LTTQ.BUS.Reports;
using BTL_LTTQ.Common.Reports;
using BTL_LTTQ.Models.Reports;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.Reports.Screens
{
    /// <summary>
    /// Bước 1: chọn nguồn dữ liệu (danh sách trắng do hệ thống cung cấp).
    /// Người dùng chỉ chọn nguồn, không bao giờ nhập câu truy vấn.
    /// </summary>
    public sealed class UcDataSourceSelection : ReportScreenBase
    {
        private readonly TextBox _txtTim;
        private readonly Label _lblDem;
        private readonly Panel _khung;
        private readonly AppButton _btnTiep;
        private readonly List<Control> _hang = new();
        private string _maChon;

        public UcDataSourceSelection(ReportEditorState trangThai, ReportDefinitionService dichVu)
            : base(trangThai, dichVu)
        {
            _maChon = trangThai.DinhNghia.MaNguon;

            var goc = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                BackColor = ReportDesignTokens.NenPanel,
                Padding = new Padding(ReportDesignTokens.Khoang20)
            };
            goc.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            goc.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            goc.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            goc.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            goc.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            // ----- Tiêu đề -----
            var dau = new Panel { AutoSize = true, Dock = DockStyle.Fill, Margin = new Padding(0, 0, 0, ReportDesignTokens.Khoang12) };
            var tieuDe = ReportUiFactory.TieuDeTrang("Chọn nguồn dữ liệu");
            tieuDe.Location = new Point(0, 0);
            var phuDe = ReportUiFactory.PhuDeTrang("Báo cáo được dựng từ các nguồn đã kiểm duyệt. Chọn một nguồn để bắt đầu.");
            phuDe.Location = new Point(2, 34);
            dau.Controls.Add(phuDe);
            dau.Controls.Add(tieuDe);
            dau.Height = 62;

            // ----- Thanh tìm kiếm -----
            var thanh = new Panel
            {
                Dock = DockStyle.Fill,
                Height = 40,
                BackColor = ReportDesignTokens.NenPanel,
                Margin = new Padding(0, 0, 0, ReportDesignTokens.Khoang8)
            };

            _txtTim = ReportUiFactory.OTimKiem("Tìm nguồn dữ liệu theo tên, mô tả…", 320);
            _txtTim.Location = new Point(0, 4);
            _txtTim.TextChanged += (_, _) => DungDanhSach();

            _lblDem = new Label
            {
                AutoSize = true,
                Font = ReportDesignTokens.ChuNho,
                ForeColor = ReportDesignTokens.ChuMo,
                Location = new Point(332, 10)
            };

            thanh.Controls.Add(_lblDem);
            thanh.Controls.Add(_txtTim);

            // ----- Danh sách nguồn -----
            _khung = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = ReportDesignTokens.NenPanel
            };
            _khung.Resize += (_, _) => BoTri();

            // ----- Chân: điều hướng -----
            var chan = new Panel
            {
                Dock = DockStyle.Fill,
                Height = 48,
                BackColor = ReportDesignTokens.NenPanel,
                Margin = new Padding(0, ReportDesignTokens.Khoang12, 0, 0)
            };

            var nutQuayLai = ReportUiFactory.Nut("← Quay lại", KieuNut.Phu, 118);
            nutQuayLai.Location = new Point(0, 6);
            nutQuayLai.Click += (_, _) => Chuyen(BuocBaoCao.Dashboard);

            _btnTiep = ReportUiFactory.Nut("Tiếp tục →", KieuNut.Chinh, 130);
            _btnTiep.Location = new Point(126, 6);
            _btnTiep.Click += (_, _) => TiepTuc();

            chan.Controls.Add(_btnTiep);
            chan.Controls.Add(nutQuayLai);

            goc.Controls.Add(dau, 0, 0);
            goc.Controls.Add(thanh, 0, 1);
            goc.Controls.Add(_khung, 0, 2);
            goc.Controls.Add(chan, 0, 3);
            Controls.Add(goc);
        }

        public override void Nap()
        {
            _maChon = TrangThai.DinhNghia.MaNguon;
            DungDanhSach();
        }

        private void DungDanhSach()
        {
            _khung.SuspendLayout();
            _khung.Controls.Clear();
            _hang.Clear();

            string tuKhoa = _txtTim.Text.Trim();
            var tatCa = DanhMuc.NguonDuLieu
                .Where(n => Khop(n, tuKhoa))
                .ToList();

            _lblDem.Text = $"{tatCa.Count}/{DanhMuc.NguonDuLieu.Count} nguồn dữ liệu";

            if (tatCa.Count == 0)
            {
                var trong = new Label
                {
                    Text = "Không tìm thấy nguồn dữ liệu nào khớp từ khóa.",
                    Font = ReportDesignTokens.ChuThuong,
                    ForeColor = ReportDesignTokens.ChuMo,
                    AutoSize = false,
                    Height = 60,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                _hang.Add(trong);
                _khung.Controls.Add(trong);
                _khung.ResumeLayout();
                BoTri();
                return;
            }

            foreach (var nhom in tatCa.GroupBy(n => n.Nhom))
            {
                var tieuDe = new Label
                {
                    Text = nhom.Key.ToUpperInvariant(),
                    Font = ReportDesignTokens.ChuNhan,
                    ForeColor = ReportDesignTokens.ChuPhu,
                    AutoSize = false,
                    Height = 26,
                    TextAlign = ContentAlignment.BottomLeft
                };
                _hang.Add(tieuDe);
                _khung.Controls.Add(tieuDe);

                foreach (var nguon in nhom)
                {
                    var the = new TheNguon(nguon, nguon.Ma == _maChon);
                    the.YeuCauChon += The_YeuCauChon;
                    the.YeuCauMo += The_YeuCauMo;
                    _hang.Add(the);
                    _khung.Controls.Add(the);
                }
            }

            _khung.ResumeLayout();
            BoTri();
        }

        private static bool Khop(DataSourceDefinition nguon, string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa))
                return true;

            return nguon.Ten.Contains(tuKhoa, StringComparison.CurrentCultureIgnoreCase) ||
                   nguon.MoTa.Contains(tuKhoa, StringComparison.CurrentCultureIgnoreCase) ||
                   nguon.Ma.Contains(tuKhoa, StringComparison.OrdinalIgnoreCase) ||
                   nguon.Nhom.Contains(tuKhoa, StringComparison.CurrentCultureIgnoreCase);
        }

        private void BoTri()
        {
            int rong = Math.Max(360, _khung.ClientSize.Width - (ReportDesignTokens.Khoang20 * 2) - 2);
            int y = 0;

            foreach (var control in _hang)
            {
                control.Location = new Point(ReportDesignTokens.Khoang20, y);
                control.Width = rong;
                y += control.Height + (control is TheNguon ? ReportDesignTokens.Khoang8 : 0);
            }

            _khung.AutoScrollMinSize = new Size(0, y + ReportDesignTokens.Khoang20);
        }

        private void The_YeuCauChon(object? sender, EventArgs e)
        {
            if (sender is not TheNguon the)
                return;

            _maChon = the.Nguon.Ma;
            foreach (var control in _hang)
                if (control is TheNguon khac)
                    khac.DatChon(khac.Nguon.Ma == _maChon);

            _btnTiep.Enabled = true;
        }

        private void The_YeuCauMo(object? sender, EventArgs e)
        {
            The_YeuCauChon(sender, e);
            TiepTuc();
        }

        private void TiepTuc()
        {
            if (string.IsNullOrWhiteSpace(_maChon))
            {
                HienThongBao("Vui lòng chọn một nguồn dữ liệu trước khi tiếp tục.",
                    "Chưa chọn nguồn", MessageBoxIcon.Information);
                return;
            }

            var hienTai = TrangThai.DinhNghia;
            if (!string.Equals(hienTai.MaNguon, _maChon, StringComparison.OrdinalIgnoreCase))
            {
                if (CoCauHinh(hienTai))
                {
                    var traLoi = MessageBox.Show(
                        "Đổi nguồn dữ liệu sẽ xóa toàn bộ cột, bộ lọc và bố cục đã cấu hình.\nBạn có muốn tiếp tục?",
                        "Xác nhận đổi nguồn", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (traLoi != DialogResult.Yes)
                        return;
                }

                var moi = DichVu.TaoMoi(_maChon);
                TrangThai.DatDinhNghia(moi, true);
            }
            else
            {
                TrangThai.DatNguon(_maChon);
            }

            Chuyen(BuocBaoCao.CauHinh);
        }

        private static bool CoCauHinh(ReportDefinition dinhNghia) =>
            dinhNghia.Cot.Count > 0 || dinhNghia.Loc.Count > 0 || dinhNghia.Nhom.Count > 0 ||
            dinhNghia.SapXep.Count > 0 || dinhNghia.TongHop.Count > 0;

        /// <summary>Thẻ nguồn dữ liệu dạng dòng ngang, tự vẽ để kiểm soát trạng thái chọn/hover.</summary>
        private sealed class TheNguon : Panel
        {
            public readonly DataSourceDefinition Nguon;

            private bool _chon;
            private bool _hover;

            public event EventHandler? YeuCauChon;
            public event EventHandler? YeuCauMo;

            public TheNguon(DataSourceDefinition nguon, bool chon)
            {
                Nguon = nguon;
                _chon = chon;
                Height = 80;
                DoubleBuffered = true;
                BackColor = ReportDesignTokens.NenTrang;
                Cursor = Cursors.Hand;
                Margin = new Padding(0);
            }

            public void DatChon(bool chon)
            {
                if (_chon == chon)
                    return;

                _chon = chon;
                Invalidate();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                var g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                Color nen = _chon
                    ? ReportDesignTokens.NhanNhat
                    : _hover ? ReportDesignTokens.NenHeader : ReportDesignTokens.NenTrang;

                using (var co = new SolidBrush(nen))
                    g.FillRectangle(co, ClientRectangle);

                if (_chon)
                {
                    using var nhan = new SolidBrush(ReportDesignTokens.Nhan);
                    g.FillRectangle(nhan, 0, 0, 4, Height);
                }

                using (var but = new Pen(_chon ? ReportDesignTokens.Nhan : ReportDesignTokens.VienNhat,
                    _chon ? 1.6f : 1f))
                {
                    g.DrawRectangle(but, 0, 0, Width - 1, Height - 1);
                }

                // Biểu tượng
                var vungIcon = new Rectangle(16, 16, 48, 48);
                TextRenderer.DrawText(g, Nguon.BieuTuong, new Font("Segoe UI Emoji", 20F), vungIcon,
                    ReportDesignTokens.ChuChinh,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

                int phai = Math.Min(300, Math.Max(150, Width / 3));
                int rongChu = Math.Max(80, Width - 76 - phai - 16);

                var vungTen = new Rectangle(74, 14, rongChu, 24);
                TextRenderer.DrawText(g, Nguon.Ten, ReportDesignTokens.ChuTieuDeThe, vungTen,
                    _chon ? ReportDesignTokens.NhanDam : ReportDesignTokens.ChuChinh,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis |
                    TextFormatFlags.NoPrefix);

                var vungMoTa = new Rectangle(74, 40, rongChu, 22);
                TextRenderer.DrawText(g, Nguon.MoTa, ReportDesignTokens.ChuNho, vungMoTa,
                    ReportDesignTokens.ChuPhu,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis |
                    TextFormatFlags.NoPrefix);

                // Khối bên phải: số trường + phạm vi
                var vungPhai = new Rectangle(Width - phai, 0, phai - 16, Height);
                TextRenderer.DrawText(g, $"{Nguon.Truong.Count} trường dữ liệu",
                    ReportDesignTokens.ChuNho, new Rectangle(vungPhai.X, 16, vungPhai.Width, 20),
                    ReportDesignTokens.ChuChinh,
                    TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

                TextRenderer.DrawText(g, MoTaPhamVi(Nguon.PhamVi), ReportDesignTokens.ChuNho,
                    new Rectangle(vungPhai.X, 38, vungPhai.Width, 20),
                    ReportDesignTokens.ChuMo,
                    TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

                if (_chon)
                {
                    var vungChon = new Rectangle(Width - 34, 8, 22, 22);
                    using (var co = new SolidBrush(ReportDesignTokens.Nhan))
                        g.FillEllipse(co, vungChon);
                    TextRenderer.DrawText(g, "✓", ReportDesignTokens.ChuNhan, vungChon, Color.White,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
                }
            }

            private static string MoTaPhamVi(PhamViNguon phamVi) => phamVi switch
            {
                PhamViNguon.ChoHocVien => "Phạm vi: theo học viên",
                PhamViNguon.ChoLopHoc => "Phạm vi: theo lớp học",
                _ => "Phạm vi: toàn hệ thống"
            };

            protected override void OnMouseEnter(EventArgs e)
            {
                base.OnMouseEnter(e);
                _hover = true;
                Invalidate();
            }

            protected override void OnMouseLeave(EventArgs e)
            {
                base.OnMouseLeave(e);
                _hover = false;
                Invalidate();
            }

            protected override void OnClick(EventArgs e)
            {
                base.OnClick(e);
                YeuCauChon?.Invoke(this, EventArgs.Empty);
            }

            protected override void OnDoubleClick(EventArgs e)
            {
                base.OnDoubleClick(e);
                YeuCauMo?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
