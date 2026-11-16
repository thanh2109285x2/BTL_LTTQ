using BTL_LTTQ.BUS.Reports;
using BTL_LTTQ.Common.Reports;
using BTL_LTTQ.Models.Reports;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.Reports.Controls
{
    /// <summary>
    /// Khung xem trước báo cáo dạng trang in: tiêu đề, bảng dữ liệu, dòng nhóm và dòng tổng.
    /// Vẽ bằng GDI+ để hiển thị mượt với nhiều dòng và bám sát bố cục trang in.
    /// </summary>
    public sealed class UcReportPreviewCanvas : UserControl
    {
        private readonly Panel _khung;
        private readonly BangVe _bang;
        private readonly UcStateOverlay _overlay;

        public UcReportPreviewCanvas()
        {
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(120, 130, 145);

            _khung = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Color.FromArgb(120, 130, 145) };

            _bang = new BangVe();
            _khung.Controls.Add(_bang);
            _khung.Resize += (_, _) => CanGiuaTrang();

            _overlay = new UcStateOverlay();

            Controls.Add(_khung);
            Controls.Add(_overlay);
        }

        public void HienKetQua(ReportResult ketQua, ReportLayoutDef boCuc)
        {
            _overlay.An();
            _bang.DatDuLieu(ketQua, boCuc);
            _bang.Location = new Point(ReportDesignTokens.Khoang20, ReportDesignTokens.Khoang20);
            CanGiuaTrang();
            _khung.AutoScrollPosition = new Point(0, 0);
        }

        public void HienTrong(string tieuDe, string moTa)
        {
            _bang.DatDuLieu(null, null);
            _overlay.Hien(TrangThaiXemTruoc.Trong, tieuDe, moTa);
        }

        public void HienDangTai(string moTa = "Đang chạy báo cáo, vui lòng chờ…")
        {
            _overlay.Hien(TrangThaiXemTruoc.DangTai, "Đang tải dữ liệu", moTa);
        }

        public void HienLoi(string tieuDe, string moTa, string? nhanNut = null, Action? hanhDong = null)
        {
            _overlay.Hien(TrangThaiXemTruoc.Loi, tieuDe, moTa, nhanNut, hanhDong);
        }

        private void CanGiuaTrang()
        {
            if (_bang.Width <= 0)
                return;

            int x = Math.Max(ReportDesignTokens.Khoang20,
                (_khung.ClientSize.Width - _bang.Width) / 2);
            _bang.Location = new Point(x, ReportDesignTokens.Khoang20);
        }

        /// <summary>Bề mặt vẽ một trang báo cáo.</summary>
        private sealed class BangVe : Panel
        {
            private const float MmSangPx = 96f / 25.4f;

            private ReportResult? _ketQua;
            private ReportLayoutDef? _boCuc;

            public BangVe()
            {
                DoubleBuffered = true;
                BackColor = Color.White;
                ResizeRedraw = true;
            }

            public void DatDuLieu(ReportResult? ketQua, ReportLayoutDef? boCuc)
            {
                _ketQua = ketQua;
                _boCuc = boCuc;
                CapNhatKichThuoc();
                Invalidate();
            }

            private void CapNhatKichThuoc()
            {
                if (_ketQua == null || _boCuc == null)
                {
                    Width = 794;
                    Height = 300;
                    return;
                }

                var (rong, _) = ReportLayoutService.KichThuocTrang(_boCuc.KhoGiay, _boCuc.HuongGiay);
                Width = (int)(rong * MmSangPx);

                float coChu = _boCuc.CoChu <= 0 ? 9.5f : _boCuc.CoChu;
                int caoDong = Math.Max(20, (int)Math.Round(coChu * 2.1f));
                int dau = (int)(_boCuc.LeTren * MmSangPx);
                int chan = (int)(_boCuc.LeDuoi * MmSangPx);
                int than = _ketQua.Dong.Count * caoDong;
                Height = dau + 96 + than + 60 + chan;
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);

                var g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                using var vien = new Pen(ReportDesignTokens.VienNhat);
                g.DrawRectangle(vien, 0, 0, Width - 1, Height - 1);

                if (_ketQua == null || _boCuc == null)
                    return;

                Ve(g, _ketQua, _boCuc);
            }

            private void Ve(Graphics g, ReportResult kq, ReportLayoutDef bo)
            {
                int leTrai = (int)(bo.LeTrai * MmSangPx);
                int lePhai = (int)(bo.LePhai * MmSangPx);
                int leTren = (int)(bo.LeTren * MmSangPx);
                int vungRong = Width - leTrai - lePhai;

                float coChu = bo.CoChu <= 0 ? 9.5f : bo.CoChu;
                Font chuThuong = TaoFont(bo.KieuChu, coChu, FontStyle.Regular);
                Font chuDam = TaoFont(bo.KieuChu, coChu, FontStyle.Bold);
                Font chuTieuDe = TaoFont(bo.KieuChu, coChu + 7f, FontStyle.Bold);
                Font chuPhu = TaoFont(bo.KieuChu, coChu + 1.5f, FontStyle.Regular);
                Font chuNho = TaoFont(bo.KieuChu, Math.Max(6.5f, coChu - 1.2f), FontStyle.Regular);

                int y = leTren;

                // ----- Tiêu đề -----
                string tieuDe = string.IsNullOrWhiteSpace(kq.TieuDe) ? "BÁO CÁO" : kq.TieuDe.ToUpper();
                VeChuGiua(g, tieuDe, chuTieuDe, ReportDesignTokens.ChuTieuDeBaoCao, leTrai, y, vungRong);
                y += (int)(chuTieuDe.GetHeight(g) + 4);

                if (!string.IsNullOrWhiteSpace(kq.PhuDe))
                {
                    VeChuGiua(g, kq.PhuDe, chuPhu, ReportDesignTokens.ChuPhu, leTrai, y, vungRong);
                    y += (int)(chuPhu.GetHeight(g) + 2);
                }

                string meta = $"Ngày chạy: {kq.ThoiDiemChay:dd/MM/yyyy HH:mm}   •   {kq.SoDongDuLieu} dòng"
                    + (kq.DungDuLieuMau ? "   •   DỮ LIỆU MẪU (thiết kế)" : "");
                VeChuGiua(g, meta, chuNho, ReportDesignTokens.ChuMo, leTrai, y, vungRong);
                y += (int)(chuNho.GetHeight(g) + 8);

                // ----- Cột -----
                int soCot = kq.Cot.Count;
                if (soCot == 0)
                    return;

                var rong = new int[soCot];
                long tongTrongSo = 0;
                for (int i = 0; i < soCot; i++)
                {
                    rong[i] = Math.Max(40, kq.Cot[i].DoRong);
                    tongTrongSo += rong[i];
                }

                var xCot = new int[soCot + 1];
                xCot[0] = leTrai;
                for (int i = 0; i < soCot; i++)
                    xCot[i + 1] = xCot[i] + (int)(vungRong * (double)rong[i] / tongTrongSo);

                int caoDong = Math.Max(20, (int)Math.Round(coChu * 2.1f));
                int caoHeader = caoDong + 4;
                int yDauBang = y;

                // ----- Dòng tiêu đề cột -----
                var vungHeader = new Rectangle(leTrai, y, vungRong, caoHeader);
                using (var co = new SolidBrush(ReportDesignTokens.Nhan))
                    g.FillRectangle(co, vungHeader);

                for (int i = 0; i < soCot; i++)
                    VeChuOCo(g, kq.Cot[i].TieuDe, chuDam, Color.White,
                        xCot[i], y, xCot[i + 1] - xCot[i], caoHeader, ContentAlignment.MiddleCenter);

                y += caoHeader;

                // ----- Các dòng -----
                bool xenKe = false;
                for (int r = 0; r < kq.Dong.Count; r++)
                {
                    var dong = kq.Dong[r];
                    Color nen = Color.White;
                    bool dam = dong.InDam;

                    switch (dong.Loai)
                    {
                        case LoaiDongBaoCao.TieuDeNhom:
                            nen = ReportDesignTokens.NhanNhat;
                            dam = true;
                            break;
                        case LoaiDongBaoCao.TongNhom:
                            nen = ReportDesignTokens.NenPanel;
                            dam = true;
                            break;
                        case LoaiDongBaoCao.TongBaoCao:
                            nen = ReportDesignTokens.NhanNhat;
                            dam = true;
                            break;
                        default:
                            if (bo.NenXenKe && xenKe)
                                nen = ReportDesignTokens.NenPanel;
                            xenKe = !xenKe;
                            break;
                    }

                    var vungDong = new Rectangle(leTrai, y, vungRong, caoDong);
                    if (nen != Color.White)
                    {
                        using var co = new SolidBrush(nen);
                        g.FillRectangle(co, vungDong);
                    }

                    if (dong.LaTong)
                    {
                        using var but = new Pen(ReportDesignTokens.Vien);
                        g.DrawLine(but, leTrai, y, leTrai + vungRong, y);
                    }

                    if (bo.HienDuongKe && dong.Loai == LoaiDongBaoCao.DuLieu)
                    {
                        using var but = new Pen(ReportDesignTokens.VienNhat);
                        g.DrawLine(but, leTrai, y + caoDong, leTrai + vungRong, y + caoDong);
                    }

                    Font font = dam ? chuDam : chuThuong;
                    for (int c = 0; c < soCot; c++)
                    {
                        string chu = ChuoiOCo(kq, dong, c);
                        var canLe = dong.Loai == LoaiDongBaoCao.TieuDeNhom
                            ? ContentAlignment.MiddleLeft
                            : CanLe(kq.Cot[c].CanLe);

                        VeChuOCo(g, chu, font, ReportDesignTokens.ChuBaoCao,
                            xCot[c], y, xCot[c + 1] - xCot[c], caoDong, canLe);
                    }

                    y += caoDong;
                }

                // ----- Viền bảng -----
                using (var but = new Pen(ReportDesignTokens.Vien))
                    g.DrawRectangle(but, leTrai, yDauBang, vungRong, y - yDauBang);

                // ----- Chân trang -----
                y += 12;
                if (bo.HienSoTrang)
                    VeChuGiua(g, "Trang 1 / 1", chuNho, ReportDesignTokens.ChuMo, leTrai, y, vungRong);

                if (kq.ChamGioiHan)
                    VeChuGiua(g, "Kết quả đã chạm giới hạn số dòng an toàn.", chuNho,
                        ReportDesignTokens.CanhBao, leTrai, y + 16, vungRong);

                chuThuong.Dispose();
                chuDam.Dispose();
                chuTieuDe.Dispose();
                chuPhu.Dispose();
                chuNho.Dispose();
            }

            private static string ChuoiOCo(ReportResult kq, ReportResultRow dong, int chiSo)
            {
                if (chiSo >= dong.GiaTri.Length)
                    return "";

                if (dong.Loai == LoaiDongBaoCao.TieuDeNhom)
                    return Convert.ToString(dong.GiaTri[chiSo]) ?? "";

                var cot = kq.Cot[chiSo];
                return ReportFormatters.Hien(dong.GiaTri[chiSo], cot.Kieu, cot.DinhDang, cot.TienTo, cot.HauTo);
            }

            private static ContentAlignment CanLe(CanLeCot canLe) => canLe switch
            {
                CanLeCot.Giua => ContentAlignment.MiddleCenter,
                CanLeCot.Phai => ContentAlignment.MiddleRight,
                _ => ContentAlignment.MiddleLeft
            };

            private static Font TaoFont(string ten, float co, FontStyle kieu)
            {
                try
                {
                    return new Font(ten, co, kieu);
                }
                catch
                {
                    return new Font("Segoe UI", co, kieu);
                }
            }

            private static void VeChuGiua(Graphics g, string chu, Font font, Color mau,
                int x, int y, int rong)
            {
                var vung = new Rectangle(x, y, rong, (int)Math.Ceiling(font.GetHeight(g)));
                TextRenderer.DrawText(g, chu, font, vung, mau,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            }

            private static void VeChuOCo(Graphics g, string chu, Font font, Color mau,
                int x, int y, int rong, int cao, ContentAlignment canLe)
            {
                var vung = new Rectangle(x + 5, y, rong - 8, cao);
                TextFormatFlags flags = canLe switch
                {
                    ContentAlignment.MiddleCenter => TextFormatFlags.HorizontalCenter,
                    ContentAlignment.MiddleRight => TextFormatFlags.Right,
                    _ => TextFormatFlags.Left
                };

                flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix;
                TextRenderer.DrawText(g, chu ?? "", font, vung, mau, flags);
            }
        }
    }
}
