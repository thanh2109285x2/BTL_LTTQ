using BTL_LTTQ.BUS.Reports;
using BTL_LTTQ.Common;
using BTL_LTTQ.Common.Reports;
using BTL_LTTQ.Forms.Reports.Controls;
using BTL_LTTQ.Models.Reports;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.Reports.Screens
{
    /// <summary>
    /// Bước 4: xem trước báo cáo. Chạy báo cáo bằng dữ liệu thật (hoặc dữ liệu mẫu khi bật
    /// chế độ thiết kế), hiển thị trang in, thống kê lần chạy và xuất Excel nếu được phép.
    /// </summary>
    public sealed class UcReportPreview : ReportScreenBase
    {
        private readonly UcReportPreviewCanvas _xemTruoc;
        private readonly AppButton _btnXuat;
        private readonly Panel _khungInfo;
        private readonly ReportCard _cardChay;
        private readonly ReportCard _cardCauHinh;

        private readonly Label _lblNguon = TaoGiaTri();
        private readonly Label _lblNguonDuLieu = TaoGiaTri();
        private readonly Label _lblSoCot = TaoGiaTri();
        private readonly Label _lblSoDong = TaoGiaTri();
        private readonly Label _lblSoTrang = TaoGiaTri();
        private readonly Label _lblThoiGian = TaoGiaTri();

        private readonly Label _lblBoLoc = TaoGiaTri();
        private readonly Label _lblSapXep = TaoGiaTri();
        private readonly Label _lblNhom = TaoGiaTri();
        private readonly Label _lblTongHop = TaoGiaTri();
        private readonly Label _lblKhoGiay = TaoGiaTri();

        private ReportResult? _ketQua;
        private bool _dangChay;

        public UcReportPreview(ReportEditorState trangThai, ReportDefinitionService dichVu)
            : base(trangThai, dichVu)
        {
            var goc = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = ReportDesignTokens.NenPanel,
                Padding = new Padding(ReportDesignTokens.Khoang20)
            };
            goc.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            goc.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            goc.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // ---------------------------------------------------------- Thanh trên
            var top = new Panel
            {
                Dock = DockStyle.Fill,
                Height = 60,
                BackColor = ReportDesignTokens.NenPanel,
                Margin = new Padding(0, 0, 0, ReportDesignTokens.Khoang12)
            };

            var tieuDe = ReportUiFactory.TieuDeTrang("Xem trước báo cáo");
            tieuDe.Location = new Point(0, 4);
            var phuDe = ReportUiFactory.PhuDeTrang("Kiểm tra kết quả trước khi lưu hoặc xuất tệp.");
            phuDe.Location = new Point(2, 36);

            var nutQuayLai = ReportUiFactory.Nut("← Bố cục", KieuNut.Phu, 106);
            nutQuayLai.Click += (_, _) => Chuyen(BuocBaoCao.BoCuc);

            var nutChay = ReportUiFactory.Nut("↻ Chạy lại", KieuNut.Phu, 108);
            nutChay.Click += (_, _) => Chay();

            var nutLuu = ReportUiFactory.Nut("Lưu", KieuNut.Chinh, 84);
            nutLuu.Click += (_, _) => LuuBaoCao();

            _btnXuat = ReportUiFactory.Nut("Xuất Excel", KieuNut.Chinh, 116);
            _btnXuat.Click += (_, _) => XuatExcel();

            top.Controls.Add(phuDe);
            top.Controls.Add(tieuDe);
            top.Controls.Add(_btnXuat);
            top.Controls.Add(nutLuu);
            top.Controls.Add(nutChay);
            top.Controls.Add(nutQuayLai);

            top.Resize += (_, _) =>
            {
                int x = top.ClientSize.Width;
                _btnXuat.Location = new Point(x - _btnXuat.Width, 14);
                x -= _btnXuat.Width + ReportDesignTokens.Khoang8;
                nutLuu.Location = new Point(x - nutLuu.Width, 14);
                x -= nutLuu.Width + ReportDesignTokens.Khoang8;
                nutChay.Location = new Point(x - nutChay.Width, 14);
                x -= nutChay.Width + ReportDesignTokens.Khoang8;
                nutQuayLai.Location = new Point(x - nutQuayLai.Width, 14);
            };

            // ---------------------------------------------------------- Thân
            var than = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = ReportDesignTokens.NenPanel
            };
            than.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            than.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 320));

            var theXemTruoc = ReportUiFactory.The("Trang báo cáo", "Hiển thị theo bố cục trang in đã thiết kế");
            theXemTruoc.Dock = DockStyle.Fill;
            theXemTruoc.Margin = new Padding(0, 0, ReportDesignTokens.Khoang12, 0);
            _xemTruoc = new UcReportPreviewCanvas();
            theXemTruoc.Than.Padding = new Padding(0);
            theXemTruoc.Than.Controls.Add(_xemTruoc);

            _khungInfo = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = ReportDesignTokens.NenPanel,
                Margin = new Padding(0)
            };
            _khungInfo.Resize += (_, _) => BoTriThongTin();

            _cardChay = ReportUiFactory.The("Thông tin lần chạy", "Kết quả truy vấn gần nhất");
            _cardCauHinh = ReportUiFactory.The("Cấu hình báo cáo", "Tóm tắt các thành phần đang dùng");

            DungTheChay();
            DungTheCauHinh();
            _khungInfo.Controls.Add(_cardChay);
            _khungInfo.Controls.Add(_cardCauHinh);

            than.Controls.Add(theXemTruoc, 0, 0);
            than.Controls.Add(_khungInfo, 1, 0);

            goc.Controls.Add(top, 0, 0);
            goc.Controls.Add(than, 0, 1);
            Controls.Add(goc);

            CapNhatNutXuat();
        }

        public override void Nap()
        {
            CapNhatTheCauHinh();
            Chay();
        }

        /// <summary>Chạy lại báo cáo theo trạng thái hiện tại (dùng khi form chủ đổi nguồn dữ liệu mẫu).</summary>
        public void ChayLai() => Chay();

        // ------------------------------------------------------------- Chạy báo cáo

        private void Chay()
        {
            if (TrangThai.Nguon == null)
            {
                _xemTruoc.HienTrong("Chưa chọn nguồn dữ liệu",
                    "Quay lại bước chọn nguồn dữ liệu để bắt đầu dựng báo cáo.");
                _ketQua = null;
                CapNhatTheChay(null);
                CapNhatNutXuat();
                return;
            }

            if (!TrangThai.DinhNghia.CotHienThi.Any())
            {
                _xemTruoc.HienTrong("Chưa có cột hiển thị",
                    "Cần chọn ít nhất một cột dữ liệu ở bước cấu hình.");
                _ketQua = null;
                CapNhatTheChay(null);
                CapNhatNutXuat();
                return;
            }

            if (_dangChay)
                return;

            _dangChay = true;
            _ketQua = null;
            _xemTruoc.HienDangTai(TrangThai.DungDuLieuMau
                ? "Đang dựng báo cáo bằng dữ liệu mẫu…"
                : "Đang truy vấn dữ liệu, vui lòng chờ…");
            CapNhatNutXuat();

            if (!IsHandleCreated)
            {
                ChayDongBo();
                return;
            }

            BeginInvoke(new Action(ChayDongBo));
        }

        private void ChayDongBo()
        {
            if (IsDisposed)
                return;

            try
            {
                var ketQua = DichVu.Chay(TrangThai.DinhNghia, TrangThai.TaoNhaCungCap(), TrangThai.PhamVi);
                _ketQua = ketQua;
                _xemTruoc.HienKetQua(ketQua, TrangThai.DinhNghia.BoCuc);
                CapNhatTheChay(ketQua);
            }
            catch (Exception ex)
            {
                _ketQua = null;
                _xemTruoc.HienLoi("Không chạy được báo cáo",
                    "Chi tiết: " + ex.Message, "Thử lại", Chay);
                CapNhatTheChay(null);
            }
            finally
            {
                _dangChay = false;
                CapNhatNutXuat();
            }
        }

        // ------------------------------------------------------------- Xuất Excel

        private void XuatExcel()
        {
            if (!PermissionService.Can(ChucNangConst.XUAT_EXCEL))
            {
                HienThongBao("Bạn không có quyền xuất tệp Excel.", "Không có quyền",
                    MessageBoxIcon.Warning);
                return;
            }

            if (_ketQua == null)
            {
                HienThongBao("Chưa có kết quả để xuất. Hãy chạy báo cáo trước.",
                    "Chưa có dữ liệu", MessageBoxIcon.Information);
                return;
            }

            string ten = string.IsNullOrWhiteSpace(TrangThai.DinhNghia.Ten)
                ? "BaoCao"
                : BoKyTuKhongHopLe(TrangThai.DinhNghia.Ten);

            using var dlg = new SaveFileDialog
            {
                Filter = "Excel Workbook (*.xlsx)|*.xlsx",
                Title = "Xuất báo cáo ra Excel",
                FileName = $"{ten}_{DateTime.Now:yyyyMMdd_HHmm}.xlsx"
            };

            if (dlg.ShowDialog(this) != DialogResult.OK)
                return;

            if (!ReportExcelExporter.Xuat(_ketQua, dlg.FileName, out string loi))
            {
                HienThongBao(loi, "Không xuất được", MessageBoxIcon.Error);
                return;
            }

            var traLoi = MessageBox.Show("Đã xuất tệp thành công.\n\n" + dlg.FileName + "\n\nMở tệp ngay?",
                "Xuất thành công", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (traLoi == DialogResult.Yes)
            {
                try
                {
                    Process.Start(new ProcessStartInfo(dlg.FileName) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    HienThongBao("Không mở được tệp: " + ex.Message, "Lỗi", MessageBoxIcon.Warning);
                }
            }
        }

        private static string BoKyTuKhongHopLe(string ten)
        {
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                ten = ten.Replace(c, '_');
            return ten.Trim();
        }

        // ------------------------------------------------------------- Thẻ thông tin

        private void DungTheChay()
        {
            var bang = TaoBang(2);
            DongThongTin(bang, "Nguồn dữ liệu", _lblNguon);
            DongThongTin(bang, "Loại dữ liệu", _lblNguonDuLieu);
            DongThongTin(bang, "Số cột", _lblSoCot);
            DongThongTin(bang, "Số dòng dữ liệu", _lblSoDong);
            DongThongTin(bang, "Số trang (ước tính)", _lblSoTrang);
            DongThongTin(bang, "Thời gian chạy", _lblThoiGian);
            _cardChay.Than.Controls.Add(bang);
            _cardChay.Height = 56 + (6 * 26) + 16;
        }

        private void DungTheCauHinh()
        {
            var bang = TaoBang(2);
            DongThongTin(bang, "Bộ lọc", _lblBoLoc);
            DongThongTin(bang, "Tiêu chí sắp xếp", _lblSapXep);
            DongThongTin(bang, "Cấp nhóm", _lblNhom);
            DongThongTin(bang, "Dòng tổng hợp", _lblTongHop);
            DongThongTin(bang, "Khổ giấy", _lblKhoGiay);
            _cardCauHinh.Than.Controls.Add(bang);
            _cardCauHinh.Height = 56 + (5 * 26) + 16;
        }

        private static TableLayoutPanel TaoBang(int soDong)
        {
            var bang = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 0,
                BackColor = ReportDesignTokens.NenTrang
            };
            bang.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58F));
            bang.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));
            return bang;
        }

        private static void DongThongTin(TableLayoutPanel bang, string nhan, Label giaTri)
        {
            var lbl = ReportUiFactory.Nhan(nhan);
            lbl.Dock = DockStyle.Fill;
            lbl.Margin = new Padding(0, 2, 0, 2);

            giaTri.Dock = DockStyle.Fill;
            giaTri.Margin = new Padding(0, 2, 0, 2);

            bang.RowCount++;
            bang.RowStyles.Add(new RowStyle(SizeType.Absolute, 26));
            bang.Controls.Add(lbl, 0, bang.RowCount - 1);
            bang.Controls.Add(giaTri, 1, bang.RowCount - 1);
        }

        private static Label TaoGiaTri() => new()
        {
            Text = "—",
            AutoSize = false,
            Font = ReportDesignTokens.ChuNhan,
            ForeColor = ReportDesignTokens.ChuChinh,
            TextAlign = ContentAlignment.MiddleRight
        };

        private void BoTriThongTin()
        {
            int rong = Math.Max(240, _khungInfo.ClientSize.Width - (ReportDesignTokens.Khoang8 * 2) - 2);
            _cardChay.Location = new Point(ReportDesignTokens.Khoang8, ReportDesignTokens.Khoang8);
            _cardChay.Width = rong;
            _cardCauHinh.Location = new Point(ReportDesignTokens.Khoang8,
                _cardChay.Bottom + ReportDesignTokens.Khoang12);
            _cardCauHinh.Width = rong;
            _khungInfo.AutoScrollMinSize = new Size(0, _cardCauHinh.Bottom + ReportDesignTokens.Khoang8);
        }

        private void CapNhatTheChay(ReportResult? kq)
        {
            var nguon = TrangThai.Nguon;

            _lblNguon.Text = nguon?.Ten ?? "—";

            if (kq == null)
            {
                _lblNguonDuLieu.Text = "—";
                _lblSoCot.Text = "—";
                _lblSoDong.Text = "—";
                _lblSoTrang.Text = "—";
                _lblThoiGian.Text = "—";
                return;
            }

            _lblNguonDuLieu.Text = kq.DungDuLieuMau ? "Dữ liệu mẫu" : "Cơ sở dữ liệu";
            _lblNguonDuLieu.ForeColor = kq.DungDuLieuMau
                ? ReportDesignTokens.CanhBao
                : ReportDesignTokens.ChuChinh;

            _lblSoCot.Text = kq.Cot.Count.ToString("N0");
            _lblSoDong.Text = kq.SoDongDuLieu.ToString("N0")
                + (kq.ChamGioiHan ? " (chạm giới hạn)" : "");
            _lblSoTrang.Text = kq.SoTrangUocTinh.ToString("N0");
            _lblThoiGian.Text = $"{kq.ThoiGianChayMs} ms";
        }

        private void CapNhatTheCauHinh()
        {
            var d = TrangThai.DinhNghia;
            _lblBoLoc.Text = d.Loc.Count(l => l.Bat).ToString("N0");
            _lblSapXep.Text = d.SapXep.Count.ToString("N0");
            _lblNhom.Text = d.Nhom.Count.ToString("N0");
            _lblTongHop.Text = d.TongHop.Count(t => t.HienThi && t.Ham != HamTongHop.Khong).ToString("N0");
            _lblKhoGiay.Text = $"{d.BoCuc.KhoGiay} • {(d.BoCuc.HuongGiay == HuongGiay.Doc ? "Dọc" : "Ngang")}";
        }

        private void CapNhatNutXuat()
        {
            _btnXuat.Enabled = PermissionService.Can(ChucNangConst.XUAT_EXCEL) && _ketQua != null;
        }
    }
}
