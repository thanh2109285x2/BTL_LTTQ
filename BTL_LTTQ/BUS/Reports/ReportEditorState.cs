using BTL_LTTQ.Common;
using BTL_LTTQ.Models;
using BTL_LTTQ.Models.Reports;
using System;
using System.Collections.Generic;

namespace BTL_LTTQ.BUS.Reports
{
    /// <summary>
    /// Trạng thái dùng chung giữa các màn hình của Report Builder:
    /// báo cáo đang dựng, nguồn dữ liệu đang chọn, phạm vi bảo mật và cờ thay đổi.
    /// </summary>
    public sealed class ReportEditorState
    {
        public ReportEditorState(ReportCatalogService danhMuc)
        {
            DanhMuc = danhMuc ?? throw new ArgumentNullException(nameof(danhMuc));
            NguoiDung = string.IsNullOrWhiteSpace(CurrentUser.TenDangNhap) ? "admin" : CurrentUser.TenDangNhap;
        }

        public ReportCatalogService DanhMuc { get; }

        public ReportDefinition DinhNghia { get; private set; } = new();

        public DataSourceDefinition? Nguon { get; private set; }

        public PhamViInfo PhamVi { get; private set; } = new(LoaiPhamVi.TuChoi);

        public string NguoiDung { get; }

        /// <summary>Bật để xem báo cáo bằng dữ liệu mẫu thay vì truy vấn thật (chế độ thiết kế).</summary>
        public bool DungDuLieuMau { get; set; }

        public bool CoThayDoi { get; private set; }

        public bool DaLuu => DinhNghia.MaBaoCao > 0;

        public event EventHandler? ThayDoi;

        public void DatDinhNghia(ReportDefinition dinhNghia, bool danhDauThayDoi = false)
        {
            DinhNghia = dinhNghia ?? new ReportDefinition();
            Nguon = DanhMuc.TimNguon(DinhNghia.MaNguon);
            PhamVi = ReportScopeService.ChoNguon(Nguon);
            CoThayDoi = danhDauThayDoi;
            ThayDoi?.Invoke(this, EventArgs.Empty);
        }

        public void DatNguon(string maNguon)
        {
            DinhNghia.MaNguon = maNguon;
            Nguon = DanhMuc.TimNguon(maNguon);
            PhamVi = ReportScopeService.ChoNguon(Nguon);
            DanhDauThayDoi();
        }

        public void DanhDauThayDoi()
        {
            CoThayDoi = true;
            ThayDoi?.Invoke(this, EventArgs.Empty);
        }

        public void DanhDauDaLuu(int maBaoCao)
        {
            DinhNghia.MaBaoCao = maBaoCao;
            CoThayDoi = false;
            ThayDoi?.Invoke(this, EventArgs.Empty);
        }

        public void BaoThayDoi() => ThayDoi?.Invoke(this, EventArgs.Empty);

        public IReportDataProvider TaoNhaCungCap() =>
            DungDuLieuMau ? new MockReportDataProvider() : new ReportSqlDataProvider();
    }
}
