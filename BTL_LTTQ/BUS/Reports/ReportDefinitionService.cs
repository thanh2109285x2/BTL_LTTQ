using BTL_LTTQ.Common.Reports;
using BTL_LTTQ.DAL.Repositories.Reports;
using BTL_LTTQ.Models;
using BTL_LTTQ.Models.Reports;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BTL_LTTQ.BUS.Reports
{
    /// <summary>
    /// Điều phối lưu/đọc định nghĩa báo cáo, kiểm tra cấu hình và chạy báo cáo.
    /// Đây là điểm vào duy nhất cho tầng giao diện của module Report Builder.
    /// </summary>
    public sealed class ReportDefinitionService
    {
        private readonly ReportCatalogService _danhMuc;
        private readonly ReportRepository _kho = new();
        private readonly ReportDefinitionValidator _kiemTra;
        private readonly ReportDataService _duLieu;

        public ReportDefinitionService(ReportCatalogService danhMuc)
        {
            _danhMuc = danhMuc;
            _kiemTra = new ReportDefinitionValidator(danhMuc);
            _duLieu = new ReportDataService(danhMuc);
        }

        public ReportCatalogService DanhMuc => _danhMuc;

        public List<ReportValidationMessage> KiemTra(ReportDefinition dinhNghia) => _kiemTra.KiemTra(dinhNghia);

        public bool CoLoi(IEnumerable<ReportValidationMessage> thongBao) => _kiemTra.CoLoi(thongBao);

        /// <summary>Lưu báo cáo; trả về mã báo cáo hoặc 0 nếu thất bại.</summary>
        public int Luu(ReportDefinition dinhNghia, string nguoiDung, out string loi)
        {
            loi = "";

            var thongBao = _kiemTra.KiemTra(dinhNghia);
            if (_kiemTra.CoLoi(thongBao))
            {
                loi = thongBao.First(t => t.MucDo == MucDoThongDiep.Loi).ThongDiep;
                return 0;
            }

            try
            {
                if (dinhNghia.NgayTao == default)
                    dinhNghia.NgayTao = DateTime.Now;
                dinhNghia.NgayCapNhat = DateTime.Now;
                dinhNghia.NguoiTao = string.IsNullOrWhiteSpace(dinhNghia.NguoiTao) ? nguoiDung : dinhNghia.NguoiTao;

                int ma = _kho.Luu(dinhNghia, nguoiDung);
                dinhNghia.MaBaoCao = ma;
                return ma;
            }
            catch (Exception ex)
            {
                loi = "Không lưu được báo cáo: " + ex.Message;
                return 0;
            }
        }

        public ReportDefinition? Lay(int maBaoCao)
        {
            try
            {
                return _kho.LayTheoMa(maBaoCao);
            }
            catch
            {
                return null;
            }
        }

        public List<ReportListItem> DanhSach(string tuKhoa, bool chiCuaMinh, string nguoiDung)
        {
            try
            {
                var ds = _kho.LayDanhSach(tuKhoa, chiCuaMinh, nguoiDung);
                foreach (var item in ds)
                    item.TenNguon = _danhMuc.TimNguon(item.MaNguon)?.Ten ?? item.MaNguon;
                return ds;
            }
            catch
            {
                return new List<ReportListItem>();
            }
        }

        public int Dem(string nguoiDung, bool chiCuaMinh)
        {
            try
            {
                return _kho.Dem(nguoiDung, chiCuaMinh);
            }
            catch
            {
                return 0;
            }
        }

        public bool Xoa(int maBaoCao, out string loi)
        {
            loi = "";
            try
            {
                _kho.Xoa(maBaoCao);
                return true;
            }
            catch (Exception ex)
            {
                loi = "Không xóa được báo cáo: " + ex.Message;
                return false;
            }
        }

        /// <summary>Dựng cấu hình từ mẫu, điền sẵn tiêu đề/độ rộng/căn lề theo danh mục.</summary>
        public ReportDefinition TaoTuMau(ReportTemplate mau)
        {
            var dinhNghia = mau.Tao();
            dinhNghia.Ten = mau.Ten;
            dinhNghia.MoTa = mau.MoTa;
            dinhNghia.MaNguon = mau.MaNguon;
            dinhNghia.NgayTao = DateTime.Now;
            dinhNghia.NgayCapNhat = DateTime.Now;

            var nguon = _danhMuc.TimNguon(mau.MaNguon);
            if (nguon != null)
                ChuanHoaTheoNguon(dinhNghia, nguon);

            return dinhNghia;
        }

        public ReportDefinition TaoMoi(string maNguon)
        {
            var dinhNghia = new ReportDefinition
            {
                MaNguon = maNguon,
                Ten = "Báo cáo mới",
                NgayTao = DateTime.Now,
                NgayCapNhat = DateTime.Now
            };

            var nguon = _danhMuc.TimNguon(maNguon);
            if (nguon == null)
                return dinhNghia;

            int thuTu = 0;
            foreach (var truong in nguon.Truong.Where(t => t.MacDinh))
            {
                dinhNghia.Cot.Add(new ReportColumnDef
                {
                    MaTruong = truong.Ma,
                    TieuDe = truong.TieuDe,
                    DoRong = truong.DoRong,
                    CanLe = truong.CanLeMacDinh,
                    DinhDang = truong.DinhDangMacDinh,
                    HienThi = true,
                    ThuTu = thuTu++
                });
            }

            ChuanHoaTheoNguon(dinhNghia, nguon);
            return dinhNghia;
        }

        /// <summary>Điền tiêu đề/độ rộng/căn lề/định dạng còn thiếu từ danh mục cho mọi cột.</summary>
        public void ChuanHoaTheoNguon(ReportDefinition dinhNghia, DataSourceDefinition nguon)
        {
            foreach (var cot in dinhNghia.Cot)
            {
                var truong = nguon.TimTruong(cot.MaTruong);
                if (truong == null)
                    continue;

                if (string.IsNullOrWhiteSpace(cot.TieuDe))
                    cot.TieuDe = truong.TieuDe;
                if (cot.DoRong <= 0)
                    cot.DoRong = truong.DoRong;
                if (string.IsNullOrWhiteSpace(cot.DinhDang))
                    cot.DinhDang = truong.DinhDangMacDinh;
            }
        }

        public ReportResult Chay(ReportDefinition dinhNghia, IReportDataProvider nhaCungCap, PhamViInfo phamVi)
            => _duLieu.Chay(dinhNghia, nhaCungCap, phamVi);
    }
}
