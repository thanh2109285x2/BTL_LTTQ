using BTL_LTTQ.Models.Import;
using ClosedXML.Excel;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace BTL_LTTQ.Services.Import
{
    /// <summary>Xuất báo cáo lỗi ra Excel (.xlsx) hoặc CSV (.csv).</summary>
    public static class ImportErrorReportWriter
    {
        private static readonly string[] _tieuDe = { "Dòng", "Cột", "Giá trị", "Thông điệp" };

        public static void Luu(ImportLoai loai, string tenFileNguon,
            IEnumerable<string> loiFile, IEnumerable<ImportLoi> danhSachLoi, string duongDan)
        {
            if (string.IsNullOrWhiteSpace(duongDan))
                throw new ArgumentException("Đường dẫn báo cáo không hợp lệ.", nameof(duongDan));

            List<ImportLoi> loi = danhSachLoi.ToList();
            List<string> loiCapTep = loiFile.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

            string? thuMuc = Path.GetDirectoryName(duongDan);
            if (!string.IsNullOrWhiteSpace(thuMuc))
                Directory.CreateDirectory(thuMuc);

            string duoi = Path.GetExtension(duongDan).ToLowerInvariant();
            if (duoi == ".csv")
                LuuCsv(loai, tenFileNguon, loiCapTep, loi, duongDan);
            else
                LuuExcel(loai, tenFileNguon, loiCapTep, loi, duongDan);
        }

        private static void LuuExcel(ImportLoai loai, string tenFileNguon,
            List<string> loiCapTep, List<ImportLoi> loi, string duongDan)
        {
            using var workbook = new XLWorkbook();
            IXLWorksheet sheet = workbook.Worksheets.Add("Loi");

            sheet.Cell(1, 1).Value = "Báo cáo lỗi nhập dữ liệu";
            sheet.Range(1, 1, 1, 4).Merge();
            sheet.Cell(1, 1).Style.Font.Bold = true;
            sheet.Cell(1, 1).Style.Font.FontSize = 14;

            sheet.Cell(2, 1).Value = "Loại dữ liệu";
            sheet.Cell(2, 2).Value = ImportLoaiMoTa.TenHienThi(loai);
            sheet.Cell(3, 1).Value = "Tệp nguồn";
            sheet.Cell(3, 2).Value = tenFileNguon;
            sheet.Cell(4, 1).Value = "Thời điểm xuất";
            sheet.Cell(4, 2).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

            int dongBatDau = 6;

            if (loiCapTep.Count > 0)
            {
                sheet.Cell(dongBatDau, 1).Value = "Lỗi cấp tệp";
                sheet.Range(dongBatDau, 1, dongBatDau, 4).Merge();
                sheet.Cell(dongBatDau, 1).Style.Font.Bold = true;
                sheet.Cell(dongBatDau, 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#FDE2E2");
                dongBatDau++;

                foreach (string thongDiep in loiCapTep)
                {
                    sheet.Cell(dongBatDau, 1).Value = 0;
                    sheet.Cell(dongBatDau, 4).Value = thongDiep;
                    dongBatDau++;
                }

                dongBatDau++;
            }

            for (int c = 0; c < _tieuDe.Length; c++)
            {
                IXLCell o = sheet.Cell(dongBatDau, c + 1);
                o.Value = _tieuDe[c];
                o.Style.Font.Bold = true;
                o.Style.Fill.BackgroundColor = XLColor.FromHtml("#E8F0FE");
                o.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }

            int dongDuLieu = dongBatDau + 1;

            foreach (ImportLoi item in loi)
            {
                sheet.Cell(dongDuLieu, 1).Value = item.SoDong;
                sheet.Cell(dongDuLieu, 2).Value = item.Cot;
                sheet.Cell(dongDuLieu, 3).Value = item.GiaTri ?? "";
                sheet.Cell(dongDuLieu, 4).Value = item.ThongDiep;
                sheet.Cell(dongDuLieu, 4).Style.Alignment.WrapText = true;
                dongDuLieu++;
            }

            if (loi.Count == 0 && loiCapTep.Count == 0)
            {
                sheet.Cell(dongDuLieu, 1).Value = "Không có lỗi.";
            }

            sheet.Column(1).Width = 10;
            sheet.Column(2).Width = 20;
            sheet.Column(3).Width = 24;
            sheet.Column(4).Width = 80;
            sheet.SheetView.FreezeRows(dongBatDau);

            workbook.SaveAs(duongDan);
        }

        private static void LuuCsv(ImportLoai loai, string tenFileNguon,
            List<string> loiCapTep, List<ImportLoi> loi, string duongDan)
        {
            using var writer = new StreamWriter(new FileStream(duongDan, FileMode.Create, FileAccess.Write),
                new System.Text.UTF8Encoding(true));

            var cauHinh = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false
            };

            using var csv = new CsvWriter(writer, cauHinh);

            csv.WriteField("Báo cáo lỗi nhập dữ liệu");
            csv.NextRecord();
            csv.WriteField("Loại dữ liệu");
            csv.WriteField(ImportLoaiMoTa.TenHienThi(loai));
            csv.NextRecord();
            csv.WriteField("Tệp nguồn");
            csv.WriteField(tenFileNguon);
            csv.NextRecord();
            csv.WriteField("Thời điểm xuất");
            csv.WriteField(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
            csv.NextRecord();
            csv.NextRecord();

            foreach (string thongDiep in loiCapTep)
            {
                csv.WriteField(0);
                csv.WriteField("");
                csv.WriteField("");
                csv.WriteField(thongDiep);
                csv.NextRecord();
            }

            foreach (string tieuDe in _tieuDe)
                csv.WriteField(tieuDe);
            csv.NextRecord();

            foreach (ImportLoi item in loi)
            {
                csv.WriteField(item.SoDong);
                csv.WriteField(item.Cot);
                csv.WriteField(item.GiaTri ?? "");
                csv.WriteField(item.ThongDiep);
                csv.NextRecord();
            }

            if (loi.Count == 0 && loiCapTep.Count == 0)
            {
                csv.WriteField("Không có lỗi.");
                csv.NextRecord();
            }
        }
    }
}
