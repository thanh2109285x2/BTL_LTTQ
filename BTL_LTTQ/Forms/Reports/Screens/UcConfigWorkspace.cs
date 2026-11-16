using BTL_LTTQ.BUS.Reports;
using BTL_LTTQ.Common.Reports;
using BTL_LTTQ.Forms.Reports.Controls;
using BTL_LTTQ.Models.Reports;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.Reports.Screens
{
    /// <summary>
    /// Bước 2: không gian cấu hình báo cáo.
    /// Bên trái là danh mục trường dữ liệu, giữa là các trình biên tập (cột/lọc/sắp xếp/nhóm/tổng hợp),
    /// bên phải là thuộc tính báo cáo và bố cục trang in; phía dưới là kết quả kiểm tra cấu hình.
    /// </summary>
    public sealed class UcConfigWorkspace : ReportScreenBase
    {
        private readonly TextBox _txtTen;
        private readonly Label _lblTrangThai;
        private readonly ReportCard _theTruong;
        private readonly UcFieldList _danhSachTruong;
        private readonly UcColumnsEditor _editorCot;
        private readonly UcFiltersEditor _editorLoc;
        private readonly UcSortingEditor _editorSapXep;
        private readonly UcGroupingEditor _editorNhom;
        private readonly UcSummaryEditor _editorTongHop;
        private readonly UcPropertiesPanel _thuocTinh;
        private readonly FlowLayoutPanel _danhSachKiemTra;
        private readonly UcSegmentedTabs _tabs;

        private bool _dangNap;
        private bool _dangDongBo;
        private string _chuKyCot = "";

        public UcConfigWorkspace(ReportEditorState trangThai, ReportDefinitionService dichVu)
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
            var top = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                AutoSize = true,
                BackColor = ReportDesignTokens.NenPanel,
                Margin = new Padding(0, 0, 0, ReportDesignTokens.Khoang12)
            };
            top.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            top.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            top.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            var oTen = new Panel { AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, BackColor = ReportDesignTokens.NenPanel };
            var lblTen = ReportUiFactory.Nhan("Tên báo cáo", true);
            lblTen.Location = new Point(0, 0);
            _txtTen = ReportUiFactory.OChu("", 320);
            _txtTen.Location = new Point(0, 20);
            oTen.Controls.Add(_txtTen);
            oTen.Controls.Add(lblTen);
            oTen.Height = _txtTen.Bottom + 2;

            var oTrangThai = new Panel { Dock = DockStyle.Fill, BackColor = ReportDesignTokens.NenPanel };
            _lblTrangThai = new Label
            {
                AutoSize = true,
                Font = ReportDesignTokens.ChuNhan,
                ForeColor = ReportDesignTokens.ChuMo,
                Location = new Point(ReportDesignTokens.Khoang12, 26)
            };
            oTrangThai.Controls.Add(_lblTrangThai);

            var oNut = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = ReportDesignTokens.NenPanel,
                Margin = new Padding(0)
            };
            var nutQuayLai = ReportUiFactory.Nut("← Nguồn dữ liệu", KieuNut.Phu, 148);
            nutQuayLai.Margin = new Padding(0, 20, ReportDesignTokens.Khoang8, 0);
            nutQuayLai.Click += (_, _) => Chuyen(BuocBaoCao.NguonDuLieu);

            var nutTiep = ReportUiFactory.Nut("Tiếp tục →", KieuNut.Chinh, 120);
            nutTiep.Margin = new Padding(0, 20, 0, 0);
            nutTiep.Click += (_, _) => TiepTuc();
            oNut.Controls.Add(nutQuayLai);
            oNut.Controls.Add(nutTiep);

            top.Controls.Add(oTen, 0, 0);
            top.Controls.Add(oTrangThai, 1, 0);
            top.Controls.Add(oNut, 2, 0);

            // ---------------------------------------------------------- Thân 3 cột
            var than = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = ReportDesignTokens.NenPanel
            };
            than.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, ReportDesignTokens.ChieuRongPanelTrai));
            than.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            than.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, ReportDesignTokens.ChieuRongPanelPhai));

            // Cột trái: danh mục trường
            _theTruong = ReportUiFactory.The("Trường dữ liệu", "Tích chọn để thêm cột");
            _theTruong.Dock = DockStyle.Fill;
            _theTruong.Margin = new Padding(0, 0, ReportDesignTokens.Khoang12, 0);
            _danhSachTruong = new UcFieldList(trangThai);
            _theTruong.Than.Padding = new Padding(0);
            _theTruong.Than.Controls.Add(_danhSachTruong);

            // Cột giữa: tab trình biên tập + kết quả kiểm tra
            var giua = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = ReportDesignTokens.NenPanel,
                Margin = new Padding(0)
            };
            giua.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            giua.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            giua.RowStyles.Add(new RowStyle(SizeType.Absolute, 168));

            _editorCot = new UcColumnsEditor(trangThai);
            _editorLoc = new UcFiltersEditor(trangThai);
            _editorSapXep = new UcSortingEditor(trangThai);
            _editorNhom = new UcGroupingEditor(trangThai);
            _editorTongHop = new UcSummaryEditor(trangThai);

            _tabs = new UcSegmentedTabs { Dock = DockStyle.Fill };
            _tabs.ThemTrang("Cột hiển thị", _editorCot);
            _tabs.ThemTrang("Bộ lọc", _editorLoc);
            _tabs.ThemTrang("Sắp xếp", _editorSapXep);
            _tabs.ThemTrang("Nhóm dữ liệu", _editorNhom);
            _tabs.ThemTrang("Tổng hợp", _editorTongHop);

            var theKiemTra = ReportUiFactory.The("Kiểm tra cấu hình", "Lỗi chặn lưu và xem trước");
            theKiemTra.Dock = DockStyle.Fill;
            theKiemTra.Margin = new Padding(0, ReportDesignTokens.Khoang12, 0, 0);

            _danhSachKiemTra = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = ReportDesignTokens.NenTrang
            };
            theKiemTra.Than.Controls.Add(_danhSachKiemTra);

            giua.Controls.Add(_tabs, 0, 0);
            giua.Controls.Add(theKiemTra, 0, 1);

            // Cột phải: thuộc tính
            var theThuocTinh = ReportUiFactory.The("Thuộc tính & bố cục", "Thông tin chung và trang in");
            theThuocTinh.Dock = DockStyle.Fill;
            theThuocTinh.Margin = new Padding(ReportDesignTokens.Khoang12, 0, 0, 0);
            _thuocTinh = new UcPropertiesPanel(trangThai);
            theThuocTinh.Than.Padding = new Padding(0);
            theThuocTinh.Than.Controls.Add(_thuocTinh);

            than.Controls.Add(_theTruong, 0, 0);
            than.Controls.Add(giua, 1, 0);
            than.Controls.Add(theThuocTinh, 2, 0);

            goc.Controls.Add(top, 0, 0);
            goc.Controls.Add(than, 0, 1);
            Controls.Add(goc);

            // ---------------------------------------------------------- Sự kiện
            _txtTen.TextChanged += (_, _) =>
            {
                if (_dangNap)
                    return;
                TrangThai.DinhNghia.Ten = _txtTen.Text.Trim();
                DanhDauVaKiemTra();
            };

            _danhSachTruong.ThayDoi += (_, _) =>
            {
                if (_dangDongBo)
                    return;
                _chuKyCot = ChuKyCot();
                _dangDongBo = true;
                try { _editorCot.Nap(); }
                finally { _dangDongBo = false; }
                DanhDauVaKiemTra();
            };

            _editorCot.ThayDoi += (_, _) => { if (!_dangDongBo) DongBoTuCot(); };
            _editorLoc.ThayDoi += (_, _) => DanhDauVaKiemTra();
            _editorSapXep.ThayDoi += (_, _) => DanhDauVaKiemTra();
            _editorNhom.ThayDoi += (_, _) => DanhDauVaKiemTra();
            _editorTongHop.ThayDoi += (_, _) => DanhDauVaKiemTra();

            _thuocTinh.ThayDoi += (_, _) =>
            {
                if (_dangNap)
                    return;

                if (_txtTen.Text != TrangThai.DinhNghia.Ten)
                {
                    _dangNap = true;
                    try { _txtTen.Text = TrangThai.DinhNghia.Ten; }
                    finally { _dangNap = false; }
                }

                DanhDauVaKiemTra();
            };
        }

        public override void Nap()
        {
            _dangNap = true;
            try
            {
                _txtTen.Text = TrangThai.DinhNghia.Ten;
                _danhSachTruong.Nap();
                _editorCot.Nap();
                _editorLoc.Nap();
                _editorSapXep.Nap();
                _editorNhom.Nap();
                _editorTongHop.Nap();
                _thuocTinh.Nap();
                _chuKyCot = ChuKyCot();
            }
            finally
            {
                _dangNap = false;
            }

            CapNhatTieuDeThe();
            CapNhatKiemTra();
        }

        private void CapNhatTieuDeThe()
        {
            var nguon = TrangThai.Nguon;
            _theTruong.PhuDe = nguon == null
                ? "Chưa chọn nguồn dữ liệu"
                : $"{nguon.Ten} • {nguon.Truong.Count} trường";
        }

        private string ChuKyCot() => string.Join("|", TrangThai.DinhNghia.Cot
            .Where(c => c.HienThi)
            .OrderBy(c => c.ThuTu)
            .Select(c => c.MaTruong));

        private void DongBoTuCot()
        {
            string moi = ChuKyCot();
            if (moi != _chuKyCot)
            {
                _chuKyCot = moi;
                _dangDongBo = true;
                try { _danhSachTruong.Nap(); }
                finally { _dangDongBo = false; }
            }

            DanhDauVaKiemTra();
        }

        private void DanhDauVaKiemTra()
        {
            if (_dangNap)
                return;

            TrangThai.DanhDauThayDoi();
            CapNhatKiemTra();
        }

        private void CapNhatKiemTra()
        {
            var ds = DichVu.KiemTra(TrangThai.DinhNghia);

            _danhSachKiemTra.SuspendLayout();
            _danhSachKiemTra.Controls.Clear();
            foreach (var tb in ds)
                _danhSachKiemTra.Controls.Add(TaoDongKiemTra(tb));

            if (ds.Count == 0)
                _danhSachKiemTra.Controls.Add(TaoDongKiemTra(
                    ReportValidationMessage.ThongTin("Cấu hình hợp lệ, sẵn sàng xem trước.")));
            _danhSachKiemTra.ResumeLayout();

            int soLoi = ds.Count(t => t.MucDo == MucDoThongDiep.Loi);
            int soCanhBao = ds.Count(t => t.MucDo == MucDoThongDiep.CanhBao);

            if (soLoi > 0)
            {
                _lblTrangThai.Text = $"⛔  {soLoi} lỗi cần sửa";
                _lblTrangThai.ForeColor = ReportDesignTokens.NguyHiem;
            }
            else if (soCanhBao > 0)
            {
                _lblTrangThai.Text = $"⚠  {soCanhBao} cảnh báo";
                _lblTrangThai.ForeColor = ReportDesignTokens.CanhBao;
            }
            else
            {
                _lblTrangThai.Text = "✓  Cấu hình hợp lệ";
                _lblTrangThai.ForeColor = ReportDesignTokens.ThanhCong;
            }
        }

        private static Control TaoDongKiemTra(ReportValidationMessage tb)
        {
            Color mau = tb.MucDo switch
            {
                MucDoThongDiep.Loi => ReportDesignTokens.NguyHiem,
                MucDoThongDiep.CanhBao => ReportDesignTokens.CanhBao,
                _ => ReportDesignTokens.ThanhCong
            };

            string bieuTuong = tb.MucDo switch
            {
                MucDoThongDiep.Loi => "⛔",
                MucDoThongDiep.CanhBao => "⚠",
                _ => "✓"
            };

            return new Label
            {
                AutoSize = true,
                MaximumSize = new Size(640, 0),
                Font = ReportDesignTokens.ChuNho,
                ForeColor = mau,
                Text = $"{bieuTuong}  {tb.ThongDiep}",
                Margin = new Padding(0, 0, 0, ReportDesignTokens.Khoang4)
            };
        }

        private void TiepTuc()
        {
            if (TrangThai.Nguon == null)
            {
                HienThongBao("Chưa chọn nguồn dữ liệu.", "Thiếu thông tin", MessageBoxIcon.Warning);
                return;
            }

            var thongBao = DichVu.KiemTra(TrangThai.DinhNghia);
            if (DichVu.CoLoi(thongBao))
            {
                var loi = thongBao.First(t => t.MucDo == MucDoThongDiep.Loi);
                HienThongBao("Cần sửa lỗi cấu hình trước khi tiếp tục:\n\n• " + loi.ThongDiep,
                    "Cấu hình chưa hợp lệ", MessageBoxIcon.Warning);
                return;
            }

            Chuyen(BuocBaoCao.BoCuc);
        }
    }
}
