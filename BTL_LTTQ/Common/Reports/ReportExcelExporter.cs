using BTL_LTTQ.Models.Reports;
using ClosedXML.Excel;
using System;
using System.Linq;

namespace BTL_LTTQ.Common.Reports
{
    /// <summary>
    /// Xuất kết quả báo cáo ra tệp Excel (.xlsx) bằng ClosedXML.
    /// Không dùng COM nên không phụ thuộc vào việc cài đặt Microsoft Excel trên máy.
    /// </summary>
    public static class ReportExcelExporter
    {
        private static readonly XLColor MauNhan = XLColor.FromArgb(41, 98, 168);
        private static readonly XLColor MauNhanNhat = XLColor.FromArgb(234, 241, 250);
        private static readonly XLColor MauTong = XLColor.FromArgb(240, 244, 250);
        private static readonly XLColor MauChuPhu = XLColor.FromArgb(108, 117, 125);
        private static readonly XLColor MauChuBaoCao = XLColor.FromArgb(39, 54, 77);
        private static readonly XLColor MauTieuDeBaoCao = XLColor.FromArgb(30, 74, 130);

        public static bool Xuat(ReportResult ketQua, string duongDan, out string loi)
        {
            loi = "";

            try
            {
                using var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("Bao cao");
                ws.Style.Font.FontColor = MauChuBaoCao;

                int soCot = Math.Max(1, ketQua.Cot.Count);
                int dong = 1;

                // ----- Tiêu đề -----
                string tieuDe = string.IsNullOrWhiteSpace(ketQua.TieuDe) ? "BÁO CÁO" : ketQua.TieuDe.ToUpperInvariant();
                ws.Range(dong, 1, dong, soCot).Merge();
                var oTieuDe = ws.Cell(dong, 1);
                oTieuDe.Value = tieuDe;
                oTieuDe.Style.Font.Bold = true;
                oTieuDe.Style.Font.FontSize = 14;
                oTieuDe.Style.Font.FontColor = MauTieuDeBaoCao;
                oTieuDe.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                dong++;

                // ----- Dòng thông tin -----
                string meta = (string.IsNullOrWhiteSpace(ketQua.PhuDe) ? "" : ketQua.PhuDe + "  —  ")
                    + $"Ngày chạy: {ketQua.ThoiDiemChay:dd/MM/yyyy HH:mm}  •  {ketQua.SoDongDuLieu} dòng"
                    + (ketQua.DungDuLieuMau ? "  •  DỮ LIỆU MẪU (thiết kế)" : "");
                ws.Range(dong, 1, dong, soCot).Merge();
                var oMeta = ws.Cell(dong, 1);
                oMeta.Value = meta;
                oMeta.Style.Font.FontSize = 9;
                oMeta.Style.Font.FontColor = MauChuPhu;
                oMeta.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                dong += 2;

                // ----- Dòng tiêu đề cột -----
                int dongTieuDe = dong;
                for (int c = 0; c < soCot; c++)
                {
                    var o = ws.Cell(dongTieuDe, c + 1);
                    o.Value = ketQua.Cot.Count > c ? ketQua.Cot[c].TieuDe : "";
                    o.Style.Font.Bold = true;
                    o.Style.Font.FontColor = XLColor.White;
                    o.Style.Fill.BackgroundColor = MauNhan;
                    o.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    o.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    o.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                dong++;

                // ----- Các dòng dữ liệu / nhóm / tổng -----
                foreach (var row in ketQua.Dong)
                {
                    if (row.Loai == LoaiDongBaoCao.TieuDeNhom)
                    {
                        ws.Range(dong, 1, dong, soCot).Merge();
                        var o = ws.Cell(dong, 1);
                        o.Value = row.NhanNhom ?? Convert.ToString(row.GiaTri.FirstOrDefault()) ?? "";
                        o.Style.Font.Bold = true;
                        o.Style.Font.FontColor = MauTieuDeBaoCao;
                        o.Style.Fill.BackgroundColor = MauNhanNhat;
                        dong++;
                        continue;
                    }

                    for (int c = 0; c < soCot; c++)
                    {
                        var o = ws.Cell(dong, c + 1);
                        var cot = ketQua.Cot.Count > c ? ketQua.Cot[c] : null;
                        object? giaTri = c < row.GiaTri.Length ? row.GiaTri[c] : null;
                        DatGiaTri(o, giaTri, cot);

                        if (row.LaTong)
                        {
                            o.Style.Font.Bold = true;
                            o.Style.Fill.BackgroundColor = MauTong;
                        }
                    }

                    dong++;
                }

                // ----- Độ rộng cột & cố định tiêu đề -----
                for (int c = 0; c < soCot; c++)
                {
                    double rong = ketQua.Cot.Count > c ? ketQua.Cot[c].DoRong : 120;
                    ws.Column(c + 1).Width = Math.Clamp(rong / 7.0, 8, 52);
                }

                ws.SheetView.FreezeRows(dongTieuDe);
                ws.PageSetup.PageOrientation = ketQua.SoDongMoiTrang > 0
                    ? XLPageOrientation.Default
                    : XLPageOrientation.Default;

                wb.SaveAs(duongDan);
                return true;
            }
            catch (Exception ex)
            {
                loi = "Không xuất được tệp Excel: " + ex.Message;
                return false;
            }
        }

        private static void DatGiaTri(IXLCell o, object? giaTri, ReportResultColumn? cot)
        {
            if (giaTri == null || giaTri == DBNull.Value)
            {
                o.Value = "";
                return;
            }

            var kieu = cot?.Kieu ?? KieuDuLieu.Chuoi;

            if (kieu is KieuDuLieu.SoNguyen or KieuDuLieu.SoThuc or KieuDuLieu.TienTe &&
                ReportFormatters.LaSo(giaTri, out double so))
            {
                o.Value = so;
                string dinhDang = string.IsNullOrWhiteSpace(cot?.DinhDang)
                    ? ReportFormatters.DinhDangMacDinh(kieu)
                    : cot!.DinhDang;
                if (!string.IsNullOrWhiteSpace(dinhDang))
                    o.Style.NumberFormat.Format = dinhDang;
                return;
            }

            if (kieu is KieuDuLieu.Ngay or KieuDuLieu.Gio &&
                ReportFormatters.LaNgay(giaTri, out DateTime ngay))
            {
                o.Value = ngay;
                string dinhDang = string.IsNullOrWhiteSpace(cot?.DinhDang)
                    ? (kieu == KieuDuLieu.Ngay ? "dd/MM/yyyy" : "HH:mm")
                    : cot!.DinhDang;
                o.Style.NumberFormat.Format = dinhDang;
                return;
            }

            if (kieu == KieuDuLieu.Logic && ReportFormatters.LaBool(giaTri, out bool b))
            {
                o.Value = b ? "Đúng" : "Sai";
                return;
            }

            o.Value = ReportFormatters.Hien(giaTri, kieu, cot?.DinhDang ?? "",
                cot?.TienTo ?? "", cot?.HauTo ?? "");
        }
    }
}
