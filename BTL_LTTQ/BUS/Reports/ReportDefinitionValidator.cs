using BTL_LTTQ.Common.Reports;
using BTL_LTTQ.Models.Reports;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BTL_LTTQ.BUS.Reports
{
    /// <summary>
    /// Kiểm tra cấu hình báo cáo trước khi lưu hoặc chạy.
    /// Trả về danh sách thông báo để giao diện hiển thị (lỗi/cảnh báo/gợi ý).
    /// </summary>
    public sealed class ReportDefinitionValidator
    {
        private readonly ReportCatalogService _danhMuc;

        public ReportDefinitionValidator(ReportCatalogService danhMuc)
        {
            _danhMuc = danhMuc;
        }

        public List<ReportValidationMessage> KiemTra(ReportDefinition dinhNghia)
        {
            var thongBao = new List<ReportValidationMessage>();
            if (dinhNghia == null)
            {
                thongBao.Add(ReportValidationMessage.Loi("Chưa có cấu hình báo cáo."));
                return thongBao;
            }

            if (string.IsNullOrWhiteSpace(dinhNghia.Ten))
                thongBao.Add(ReportValidationMessage.Loi("Tên báo cáo không được để trống."));

            var nguon = _danhMuc.TimNguon(dinhNghia.MaNguon);
            if (nguon == null)
            {
                thongBao.Add(ReportValidationMessage.Loi("Chưa chọn nguồn dữ liệu hợp lệ."));
                return thongBao;
            }

            var cotHienThi = dinhNghia.CotHienThi.ToList();
            if (cotHienThi.Count == 0)
                thongBao.Add(ReportValidationMessage.Loi("Báo cáo cần có ít nhất một cột hiển thị."));

            var daGap = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var cot in cotHienThi)
            {
                var truong = nguon.TimTruong(cot.MaTruong);
                if (truong == null)
                {
                    thongBao.Add(ReportValidationMessage.Loi(
                        $"Cột '{cot.MaTruong}' không thuộc nguồn '{nguon.Ten}'.", cot.MaTruong));
                    continue;
                }

                if (!daGap.Add(truong.Ma))
                    thongBao.Add(ReportValidationMessage.CanhBao(
                        $"Cột '{truong.TieuDe}' đang được chọn nhiều lần.", truong.Ma));

                if (cot.DoRong < 40 || cot.DoRong > 800)
                    thongBao.Add(ReportValidationMessage.CanhBao(
                        $"Độ rộng cột '{truong.TieuDe}' nên trong khoảng 40–800 px.", truong.Ma));
            }

            KiemTraLoc(dinhNghia, nguon, thongBao);
            KiemTraSapXep(dinhNghia, nguon, thongBao);
            KiemTraNhom(dinhNghia, nguon, cotHienThi, thongBao);
            KiemTraTongHop(dinhNghia, nguon, cotHienThi, thongBao);
            KiemTraBoCuc(dinhNghia, thongBao);

            if (dinhNghia.GioiHanDong <= 0 || dinhNghia.GioiHanDong > 20000)
                thongBao.Add(ReportValidationMessage.CanhBao("Giới hạn dòng nên trong khoảng 1–20.000."));

            return thongBao;
        }

        public bool CoLoi(IEnumerable<ReportValidationMessage> thongBao) =>
            thongBao.Any(t => t.MucDo == MucDoThongDiep.Loi);

        private static void KiemTraLoc(ReportDefinition d, DataSourceDefinition nguon, List<ReportValidationMessage> thongBao)
        {
            int chiSo = 0;
            foreach (var loc in d.Loc)
            {
                chiSo++;
                var truong = nguon.TimTruong(loc.MaTruong);
                if (truong == null)
                {
                    thongBao.Add(ReportValidationMessage.Loi(
                        $"Bộ lọc #{chiSo} tham chiếu trường không hợp lệ '{loc.MaTruong}'.", loc.MaTruong));
                    continue;
                }

                if (!loc.Bat)
                    continue;

                var hopLe = ReportEnumText.ToanTuCho(truong.Kieu);
                if (!hopLe.Contains(loc.ToanTu))
                    thongBao.Add(ReportValidationMessage.CanhBao(
                        $"Toán tử '{ReportEnumText.Ten(loc.ToanTu)}' không phù hợp với kiểu {ReportEnumText.Ten(truong.Kieu)} của '{truong.TieuDe}'.",
                        truong.Ma));

                if (ReportEnumText.CanHaiGiaTri(loc.ToanTu))
                {
                    if (!ReportFormatters.ThuPhanTich(loc.GiaTri1, truong.Kieu, out _) ||
                        !ReportFormatters.ThuPhanTich(loc.GiaTri2, truong.Kieu, out _))
                        thongBao.Add(ReportValidationMessage.Loi(
                            $"Bộ lọc '{truong.TieuDe}': khoảng giá trị chưa hợp lệ.", truong.Ma));
                }
                else if (ReportEnumText.LaDanhSach(loc.ToanTu))
                {
                    if (ReportFormatters.TachDanhSach(loc.GiaTri1).Count == 0)
                        thongBao.Add(ReportValidationMessage.Loi(
                            $"Bộ lọc '{truong.TieuDe}': danh sách giá trị đang trống.", truong.Ma));
                }
                else if (truong.Kieu != KieuDuLieu.Chuoi &&
                         !ReportFormatters.ThuPhanTich(loc.GiaTri1, truong.Kieu, out _))
                {
                    thongBao.Add(ReportValidationMessage.Loi(
                        $"Bộ lọc '{truong.TieuDe}': giá trị không đúng kiểu {ReportEnumText.Ten(truong.Kieu)}.", truong.Ma));
                }
            }
        }

        private static void KiemTraSapXep(ReportDefinition d, DataSourceDefinition nguon, List<ReportValidationMessage> thongBao)
        {
            foreach (var sx in d.SapXep)
            {
                if (nguon.TimTruong(sx.MaTruong) == null)
                    thongBao.Add(ReportValidationMessage.Loi(
                        $"Tiêu chí sắp xếp tham chiếu trường không hợp lệ '{sx.MaTruong}'.", sx.MaTruong));
            }
        }

        private static void KiemTraNhom(ReportDefinition d, DataSourceDefinition nguon,
            List<ReportColumnDef> cotHienThi, List<ReportValidationMessage> thongBao)
        {
            var daGap = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var nhom in d.Nhom)
            {
                var truong = nguon.TimTruong(nhom.MaTruong);
                if (truong == null)
                {
                    thongBao.Add(ReportValidationMessage.Loi(
                        $"Nhóm tham chiếu trường không hợp lệ '{nhom.MaTruong}'.", nhom.MaTruong));
                    continue;
                }

                if (!truong.CoTheNhom)
                    thongBao.Add(ReportValidationMessage.CanhBao(
                        $"Trường '{truong.TieuDe}' không được khuyến nghị dùng để nhóm.", truong.Ma));

                if (!daGap.Add(truong.Ma))
                    thongBao.Add(ReportValidationMessage.CanhBao(
                        $"Trường nhóm '{truong.TieuDe}' bị lặp.", truong.Ma));

                if (!cotHienThi.Any(c => c.MaTruong.Equals(truong.Ma, StringComparison.OrdinalIgnoreCase)))
                    thongBao.Add(ReportValidationMessage.ThongTin(
                        $"Cột nhóm '{truong.TieuDe}' sẽ được thêm vào kết quả để hiển thị tiêu đề nhóm.", truong.Ma));
            }
        }

        private static void KiemTraTongHop(ReportDefinition d, DataSourceDefinition nguon,
            List<ReportColumnDef> cotHienThi, List<ReportValidationMessage> thongBao)
        {
            foreach (var th in d.TongHop.Where(t => t.HienThi))
            {
                var truong = nguon.TimTruong(th.MaTruong);
                if (truong == null)
                {
                    thongBao.Add(ReportValidationMessage.Loi(
                        $"Dòng tổng hợp tham chiếu trường không hợp lệ '{th.MaTruong}'.", th.MaTruong));
                    continue;
                }

                if (th.Ham == HamTongHop.Khong)
                {
                    thongBao.Add(ReportValidationMessage.CanhBao(
                        $"Dòng tổng hợp cho '{truong.TieuDe}' chưa chọn hàm.", truong.Ma));
                    continue;
                }

                if (!truong.CoTheTongHop)
                    thongBao.Add(ReportValidationMessage.CanhBao(
                        $"Trường '{truong.TieuDe}' không được khuyến nghị dùng để tổng hợp.", truong.Ma));

                bool canSo = th.Ham is HamTongHop.Tong or HamTongHop.TrungBinh;
                if (canSo && !LaKieuSo(truong.Kieu))
                    thongBao.Add(ReportValidationMessage.Loi(
                        $"Hàm '{ReportEnumText.Ten(th.Ham)}' chỉ áp dụng cho trường số ('{truong.TieuDe}').", truong.Ma));

                if (!cotHienThi.Any(c => c.MaTruong.Equals(truong.Ma, StringComparison.OrdinalIgnoreCase)))
                    thongBao.Add(ReportValidationMessage.ThongTin(
                        $"Cột tổng hợp '{truong.TieuDe}' sẽ được thêm vào kết quả.", truong.Ma));
            }
        }

        private static void KiemTraBoCuc(ReportDefinition d, List<ReportValidationMessage> thongBao)
        {
            var b = d.BoCuc;
            if (b.LeTren < 0 || b.LeDuoi < 0 || b.LeTrai < 0 || b.LePhai < 0)
                thongBao.Add(ReportValidationMessage.CanhBao("Lề trang không được âm."));

            if (b.LeTren + b.LeDuoi > 120)
                thongBao.Add(ReportValidationMessage.CanhBao("Tổng lề trên + dưới quá lớn, có thể mất chỗ in dữ liệu."));

            if (b.CoChu < 6 || b.CoChu > 24)
                thongBao.Add(ReportValidationMessage.CanhBao("Cỡ chữ nên trong khoảng 6–24 pt."));
        }

        private static bool LaKieuSo(KieuDuLieu kieu) =>
            kieu is KieuDuLieu.SoNguyen or KieuDuLieu.SoThuc or KieuDuLieu.TienTe or KieuDuLieu.PhanTram;
    }
}
