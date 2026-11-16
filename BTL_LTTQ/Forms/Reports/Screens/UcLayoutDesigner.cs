using BTL_LTTQ.BUS.Reports;
using BTL_LTTQ.Common.Reports;
using BTL_LTTQ.Forms.Reports.Controls;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.Reports.Screens
{
    /// <summary>
    /// Bước 3: thiết kế bố cục. Bên trái là thiết lập trang in và thông tin báo cáo,
    /// bên phải là thứ tự/độ rộng cột kèm khung xem trước bố cục bằng dữ liệu minh họa.
    /// </summary>
    public sealed class UcLayoutDesigner : ReportScreenBase
    {
        private readonly UcPropertiesPanel _thuocTinh;
        private readonly UcColumnsEditor _editorCot;
        private readonly UcReportPreviewCanvas _xemTruoc;
        private readonly Label _lblMoTaTrang;

        private bool _dangNap;
        private bool _dangDongBo;

        public UcLayoutDesigner(ReportEditorState trangThai, ReportDefinitionService dichVu)
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
                Height = 56,
                BackColor = ReportDesignTokens.NenPanel,
                Margin = new Padding(0, 0, 0, ReportDesignTokens.Khoang12)
            };

            var tieuDe = ReportUiFactory.TieuDeTrang("Thiết kế bố cục");
            tieuDe.Location = new Point(0, 4);
            _lblMoTaTrang = ReportUiFactory.PhuDeTrang("");
            _lblMoTaTrang.Location = new Point(2, 34);

            var nutQuayLai = ReportUiFactory.Nut("← Cấu hình", KieuNut.Phu, 118);
            nutQuayLai.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            nutQuayLai.Click += (_, _) => Chuyen(BuocBaoCao.CauHinh);

            var nutTiep = ReportUiFactory.Nut("Xem trước →", KieuNut.Chinh, 132);
            nutTiep.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            nutTiep.Click += (_, _) => TiepTuc();

            top.Controls.Add(_lblMoTaTrang);
            top.Controls.Add(tieuDe);
            top.Controls.Add(nutTiep);
            top.Controls.Add(nutQuayLai);

            top.Resize += (_, _) =>
            {
                nutTiep.Location = new Point(top.ClientSize.Width - nutTiep.Width, 12);
                nutQuayLai.Location = new Point(nutTiep.Left - nutQuayLai.Width - ReportDesignTokens.Khoang8, 12);
            };

            // ---------------------------------------------------------- Thân 2 cột
            var than = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = ReportDesignTokens.NenPanel
            };
            than.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 348));
            than.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            // Trái: thiết lập trang in
            var theTrangIn = ReportUiFactory.The("Trang in & thuộc tính", "Khổ giấy, lề, phông chữ và tùy chọn");
            theTrangIn.Dock = DockStyle.Fill;
            theTrangIn.Margin = new Padding(0, 0, ReportDesignTokens.Khoang12, 0);
            _thuocTinh = new UcPropertiesPanel(trangThai);
            theTrangIn.Than.Padding = new Padding(0);
            theTrangIn.Than.Controls.Add(_thuocTinh);

            // Phải: cột + xem trước
            var phai = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = ReportDesignTokens.NenPanel,
                Margin = new Padding(0)
            };
            phai.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            phai.RowStyles.Add(new RowStyle(SizeType.Percent, 46F));
            phai.RowStyles.Add(new RowStyle(SizeType.Percent, 54F));

            var theCot = ReportUiFactory.The("Cột hiển thị", "Thứ tự, tiêu đề, độ rộng và căn lề");
            theCot.Dock = DockStyle.Fill;
            theCot.Margin = new Padding(0);
            _editorCot = new UcColumnsEditor(trangThai);
            theCot.Than.Padding = new Padding(0);
            theCot.Than.Controls.Add(_editorCot);

            var theXemTruoc = ReportUiFactory.The("Xem trước bố cục", "Dữ liệu minh họa để kiểm tra căn lề và khổ giấy");
            theXemTruoc.Dock = DockStyle.Fill;
            theXemTruoc.Margin = new Padding(0, ReportDesignTokens.Khoang12, 0, 0);
            _xemTruoc = new UcReportPreviewCanvas();
            theXemTruoc.Than.Padding = new Padding(0);
            theXemTruoc.Than.Controls.Add(_xemTruoc);

            phai.Controls.Add(theCot, 0, 0);
            phai.Controls.Add(theXemTruoc, 0, 1);

            than.Controls.Add(theTrangIn, 0, 0);
            than.Controls.Add(phai, 1, 0);

            goc.Controls.Add(top, 0, 0);
            goc.Controls.Add(than, 0, 1);
            Controls.Add(goc);

            _thuocTinh.ThayDoi += (_, _) => { if (!_dangNap) { TrangThai.DanhDauThayDoi(); CapNhatXemTruoc(); } };
            _editorCot.ThayDoi += (_, _) => { if (!_dangNap && !_dangDongBo) { TrangThai.DanhDauThayDoi(); CapNhatXemTruoc(); } };
        }

        public override void Nap()
        {
            _dangNap = true;
            try
            {
                _thuocTinh.Nap();
                _editorCot.Nap();
            }
            finally
            {
                _dangNap = false;
            }

            _lblMoTaTrang.Text = ReportLayoutService.MoTaKho(TrangThai.DinhNghia.BoCuc)
                + $" • {TrangThai.DinhNghia.CotHienThi.Count()} cột hiển thị";
            CapNhatXemTruoc();
        }

        private void CapNhatXemTruoc()
        {
            if (TrangThai.Nguon == null)
            {
                _xemTruoc.HienTrong("Chưa có nguồn dữ liệu", "Chọn nguồn dữ liệu ở bước trước để xem trước bố cục.");
                return;
            }

            try
            {
                var ketQua = DichVu.Chay(TrangThai.DinhNghia, new MockReportDataProvider(18), TrangThai.PhamVi);
                _xemTruoc.HienKetQua(ketQua, TrangThai.DinhNghia.BoCuc);
            }
            catch (Exception ex)
            {
                _xemTruoc.HienLoi("Không dựng được bố cục", ex.Message);
            }

            _lblMoTaTrang.Text = ReportLayoutService.MoTaKho(TrangThai.DinhNghia.BoCuc)
                + $" • {TrangThai.DinhNghia.CotHienThi.Count()} cột hiển thị";
        }

        private void TiepTuc()
        {
            if (TrangThai.Nguon == null)
            {
                HienThongBao("Chưa chọn nguồn dữ liệu.", "Thiếu thông tin", MessageBoxIcon.Warning);
                return;
            }

            if (!TrangThai.DinhNghia.CotHienThi.Any())
            {
                HienThongBao("Báo cáo cần ít nhất một cột hiển thị.", "Thiếu cột",
                    MessageBoxIcon.Warning);
                return;
            }

            Chuyen(BuocBaoCao.XemTruoc);
        }
    }
}
