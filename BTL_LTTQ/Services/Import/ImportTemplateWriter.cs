using BTL_LTTQ.Models.Import;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BTL_LTTQ.Services.Import
{
    /// <summary>Tạo tệp Excel mẫu (chỉ có tiêu đề và sheet hướng dẫn, không có dòng dữ liệu mẫu).</summary>
    public static class ImportTemplateWriter
    {
        public static void Luu(ImportLoai loai, string duongDan)
        {
            if (string.IsNullOrWhiteSpace(duongDan))
                throw new ArgumentException("Đường dẫn tệp mẫu không hợp lệ.", nameof(duongDan));

            IReadOnlyList<DinhNghiaCot> cot = BangTinhReader.LayDinhNghiaCot(loai);

            using var workbook = new XLWorkbook();

            IXLWorksheet sheet = workbook.Worksheets.Add("DuLieu");
            for (int i = 0; i < cot.Count; i++)
            {
                IXLCell o = sheet.Cell(1, i + 1);
                o.Value = cot[i].TenHienThi;
                o.Style.Font.Bold = true;
                o.Style.Fill.BackgroundColor = cot[i].BatBuoc
                    ? XLColor.FromHtml("#FDE2E2")
                    : XLColor.FromHtml("#E8F0FE");
                o.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                o.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                o.Style.Alignment.WrapText = true;

                if (cot[i].BatBuoc)
                    o.CreateComment().AddText("Bắt buộc");

                sheet.Column(i + 1).Width = Math.Max(14, cot[i].TenHienThi.Length + 4);
            }

            sheet.Row(1).Height = 24;
            sheet.SheetView.FreezeRows(1);
            sheet.Range(1, 1, 1, Math.Max(1, cot.Count)).SetAutoFilter();
            sheet.Cell(2, 1).SetActive();

            IXLWorksheet huongDan = workbook.Worksheets.Add("HuongDan");
            int dong = 1;

            huongDan.Cell(dong, 1).Value = "Hướng dẫn nhập hàng loạt";
            huongDan.Cell(dong, 1).Style.Font.Bold = true;
            huongDan.Cell(dong, 1).Style.Font.FontSize = 14;
            dong += 2;

            huongDan.Cell(dong, 1).Value = "Loại dữ liệu";
            huongDan.Cell(dong, 2).Value = ImportLoaiMoTa.TenHienThi(loai);
            dong++;

            huongDan.Cell(dong, 1).Value = "Cột bắt buộc";
            huongDan.Cell(dong, 2).Value = string.Join(", ",
                cot.Where(c => c.BatBuoc).Select(c => c.TenHienThi));
            dong++;

            huongDan.Cell(dong, 1).Value = "Cột tùy chọn";
            huongDan.Cell(dong, 2).Value = string.Join(", ",
                cot.Where(c => !c.BatBuoc).Select(c => c.TenHienThi));
            dong += 2;

            foreach (string ghiChu in GhiChu(loai))
            {
                huongDan.Cell(dong, 1).Value = "•";
                huongDan.Cell(dong, 2).Value = ghiChu;
                huongDan.Cell(dong, 2).Style.Alignment.WrapText = true;
                dong++;
            }

            huongDan.Column(1).Width = 26;
            huongDan.Column(2).Width = 90;

            string? thuMuc = Path.GetDirectoryName(duongDan);
            if (!string.IsNullOrWhiteSpace(thuMuc))
                Directory.CreateDirectory(thuMuc);

            workbook.SaveAs(duongDan);
        }

        private static IEnumerable<string> GhiChu(ImportLoai loai)
        {
            yield return "Không sửa tên cột ở sheet DuLieu. Có thể đổi vị trí cột, hệ thống nhận diện theo tiêu đề.";
            yield return "Mỗi lần chỉ nhập một loại dữ liệu. Không để dòng trống ở giữa các dòng dữ liệu.";
            yield return "Định dạng ngày: yyyy-MM-dd hoặc dd/MM/yyyy. Định dạng số thập phân: 8.5 hoặc 8,5.";
            yield return "Các mã tham chiếu (mã nghề, xếp loại, chức danh, chuyên môn, cơ quan, trình độ, mã lớp) phải tồn tại trong danh mục.";
            yield return "Toàn bộ tệp được nhập trong một lần: nếu có bất kỳ dòng lỗi nào, không dòng nào được ghi.";

            switch (loai)
            {
                case ImportLoai.DangKy:
                    yield return "Nếu để trống Ngày đăng ký, hệ thống dùng ngày hiện tại.";
                    yield return "Lớp phải ở trạng thái SapKhaiGiang hoặc DangHoc và còn đủ chỗ cho tổng số dòng trong tệp.";
                    yield return "Không nhập trùng cặp (Mã học viên, Mã lớp) trong tệp hoặc với đăng ký đã có.";
                    break;
                case ImportLoai.HocVien:
                    yield return "Mã học viên tối đa 20 ký tự, chỉ gồm chữ, số và _ . -";
                    yield return "Mã lớp (nếu có) phải tồn tại trong danh mục lớp học.";
                    break;
                case ImportLoai.GiaoVien:
                    yield return "Mã giáo viên tối đa 20 ký tự, chỉ gồm chữ, số và _ . -";
                    break;
            }
        }
    }
}
