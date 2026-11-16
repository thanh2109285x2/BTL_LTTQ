using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BTL_LTTQ.Common
{
    public interface IXuatExcel
    {
        DataGridView LuoiXuat { get; }
        string TieuDeXuat { get; }
    }

    public static class ExcelHelper
    {
        public static void XuatTrangHienTai(Form form)
        {
            var luoi = TimLuoi(form);
            if (luoi == null)
            {
                MessageBox.Show("Trang hiện tại không có dữ liệu để xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string tieuDe = (form is IXuatExcel x && !string.IsNullOrWhiteSpace(x.TieuDeXuat)) ? x.TieuDeXuat : form.Text;
            tieuDe = tieuDe.Trim().ToUpper();
            XuatDataGridView(luoi, tieuDe, TenFile(tieuDe));
        }

        public static DataGridView TimLuoi(Form form)
        {
            if (form == null) return null;
            if (form is IXuatExcel x && x.LuoiXuat != null) return x.LuoiXuat;
            DataGridView tot = null;
            void Duyet(Control cha)
            {
                foreach (Control c in cha.Controls)
                {
                    if (c is DataGridView g && g.Visible && (tot == null || g.Rows.Count > tot.Rows.Count)) tot = g;
                    if (c.HasChildren) Duyet(c);
                }
            }
            Duyet(form);
            return tot;
        }

        private static string TenFile(string s)
        {
            var sb = new StringBuilder();
            foreach (char ch in s.Normalize(NormalizationForm.FormD))
                if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark) sb.Append(ch);
            string bo = sb.ToString().Replace('Đ', 'D').Replace('đ', 'd');
            var tu = bo.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(w => new string(w.Where(char.IsLetterOrDigit).ToArray())).Where(w => w.Length > 0)
                .Select(w => char.ToUpper(w[0]) + w.Substring(1).ToLower());
            string kq = string.Concat(tu);
            return kq.Length > 0 ? kq : "BaoCao";
        }

        public static void XuatDataGridView(DataGridView dgv, string tieuDe, string tenFileMacDinh = "BaoCao")
        {
            if (dgv == null || dgv.Rows.Cast<DataGridViewRow>().All(r => r.IsNewRow))
            {
                MessageBox.Show("Không có dữ liệu để xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var sfd = new SaveFileDialog())
            {
                sfd.Title = "Chọn nơi lưu file Excel";
                sfd.Filter = "Excel (*.xlsx)|*.xlsx";
                sfd.FileName = $"{tenFileMacDinh}_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";
                if (sfd.ShowDialog() != DialogResult.OK) return;
                try { GhiFile(dgv, tieuDe, sfd.FileName); }
                catch (Exception ex)
                {
                    MessageBox.Show("Xuất Excel thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (MessageBox.Show("Xuất file thành công! Bạn có muốn mở file không?", "Xuất Excel", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    Process.Start(new ProcessStartInfo(sfd.FileName) { UseShellExecute = true });
            }
        }

        private enum Kieu { Chu, So, Ngay }

        private static object O(dynamic ws, int dong, int cot) => ws.Cells[dong, cot];

            return (Excel.Range)ws.Cells[dong, cot];
        }

            return (Excel.Range)ws.Cells[dong, cot];
        }

            return (Excel.Range)ws.Cells[dong, cot];

            return (Excel.Range)ws.Cells[dong, cot];


            // Late binding qua ProgID tránh cast ApplicationClass sang _Application khi nạp Type Library lỗi.
            object appObject = null, booksObject = null, bookObject = null, sheetsObject = null, sheetObject = null;
            object wbObject = null, wsObject = null;
            try
            Excel.Workbook wb = null;
                Type excelType = Type.GetTypeFromProgID("Excel.Application", throwOnError: true);
                appObject = Activator.CreateInstance(excelType);
                dynamic app = appObject;
                app.Visible = false;
                app.DisplayAlerts = false;
                booksObject = app.Workbooks;
                dynamic books = booksObject;
                bookObject = books.Add(Type.Missing);
                dynamic wb = bookObject;
                sheetsObject = wb.Worksheets;
                dynamic sheets = sheetsObject;
                sheetObject = sheets[1];
                wsObject = sheetObject;
                dynamic ws = wsObject;
                ws.Name = "Dữ liệu";
                app = new Excel.Application { Visible = false, DisplayAlerts = false };
                dynamic rTitle = ws.Range[O(ws, dongTieuDe, 1), O(ws, dongTieuDe, soCot)];
                ws.Name = "Dữ liệu";
                ((dynamic)O(ws, dongTieuDe, 1)).Value2 = tieuDe;
                rTitle.Font.Bold = true;
                rTitle.Font.Size = 16;
                rTitle.HorizontalAlignment = -4108;
                O(ws, dongTieuDe, 1).Value2 = tieuDe;
                dynamic rNgay = ws.Range[O(ws, dongNgay, 1), O(ws, dongNgay, soCot)];
                rTitle.Font.Size = 16;
                ((dynamic)O(ws, dongNgay, 1)).Value2 = "Ngày xuất: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                rNgay.Font.Italic = true;
                rNgay.HorizontalAlignment = -4108;

                for (int i = 0; i < soCot; i++) ((dynamic)O(ws, dongHeader, i + 1)).Value2 = cols[i].HeaderText;
                dynamic rHeader = ws.Range[O(ws, dongHeader, 1), O(ws, dongHeader, soCot)];
                rHeader.Interior.Color = ColorTranslator.ToOle(GiaoDien.MauChinh);
                rHeader.Font.Color = ColorTranslator.ToOle(Color.White);
                rHeader.Font.Bold = true;
                rHeader.HorizontalAlignment = -4108; // xlHAlignCenter
                rHeader.Interior.Color = ColorTranslator.ToOle(GiaoDien.MauChinh);
                rHeader.Font.Color = ColorTranslator.ToOle(Color.White);
                rHeader.Font.Bold = true;
                rHeader.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                rHeader.Style.Font.FontColor = XLColor.White;
                rHeader.Style.Font.Bold = true;
                rHeader.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                rHeader.Style.Font.FontColor = XLColor.White;
                rHeader.Style.Font.Bold = true;
                rHeader.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                rHeader.Style.Font.FontColor = XLColor.White;
                rHeader.Style.Font.Bold = true;
                rHeader.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                rHeader.Style.Font.FontColor = XLColor.White;
                    if (soDong == 0) continue;
                    dynamic cotDuLieu = ws.Range[O(ws, dongDau, i + 1), O(ws, dongCuoi, i + 1)];
                rHeader.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                var kieu = new Kieu[soCot];
                            cotDuLieu.NumberFormat = "dd/mm/yyyy";
                            cotDuLieu.HorizontalAlignment = -4108; // xlHAlignCenter
                            cotDuLieu.NumberFormat = DinhDangSo(cols[i].DefaultCellStyle.Format);
                            break;
                        default:
                            cotDuLieu.NumberFormat = "@";
                            break;
                        default:
                if (soDong > 0)
                {
                    object[,] data = new object[soDong, soCot];
                    for (int r = 0; r < soDong; r++)
                            cotDuLieu.Style.NumberFormat.Format = "@";
                object[,] data = new object[soDong, soCot];
                for (int r = 0; r < soDong; r++)
                {
                        if (v == null || v is DBNull) data[r, c] = null;
                        else if (v is DateTime d) data[r, c] = d.ToOADate();
                        else if (kieu[c] == Kieu.So && LaSo(v)) data[r, c] = Convert.ToDouble(v);
                        else data[r, c] = cell.FormattedValue?.ToString() ?? "";
                        else if (v is DateTime d) data[r, c] = d.ToOADate();
                    dynamic vung = ws.Range[O(ws, dongDau, 1), O(ws, dongCuoi, soCot)];
                    vung.Value2 = data;
                    Marshal.ReleaseComObject(vung);
                }
                        else if (kieu[c] == Kieu.So && (v is byte || v is short || v is int || v is long ||
                dynamic bang = ws.Range[O(ws, dongHeader, 1), O(ws, Math.Max(dongHeader, dongCuoi), soCot)];
                bang.Borders.LineStyle = 1; // xlContinuous
                bang.Columns.AutoFit();
                wb.SaveAs(duongDan, 51); // xlOpenXMLWorkbook
                Marshal.ReleaseComObject(bang);
                Marshal.ReleaseComObject(rHeader);
                Marshal.ReleaseComObject(rTitle);
                Marshal.ReleaseComObject(rNgay);
            }
            finally
            {
                if (wbObject != null) { try { ((dynamic)wbObject).Close(false); } catch { } Marshal.ReleaseComObject(wbObject); }
                else if (bookObject != null && Marshal.IsComObject(bookObject)) Marshal.ReleaseComObject(bookObject);
                if (wsObject != null && Marshal.IsComObject(wsObject)) Marshal.ReleaseComObject(wsObject);
                else if (sheetObject != null && Marshal.IsComObject(sheetObject)) Marshal.ReleaseComObject(sheetObject);
                if (sheetsObject != null && Marshal.IsComObject(sheetsObject)) Marshal.ReleaseComObject(sheetsObject);
                if (booksObject != null && Marshal.IsComObject(booksObject)) Marshal.ReleaseComObject(booksObject);
                if (appObject != null)
                {
                    try { ((dynamic)appObject).Quit(); } catch { }
                    if (Marshal.IsComObject(appObject)) Marshal.ReleaseComObject(appObject);
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                if (ws != null) Marshal.ReleaseComObject(ws);
                if (app != null) { app.Quit(); Marshal.ReleaseComObject(app); }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                bang.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                ws.Columns(1, soCot).AdjustToContents();
                wb.SaveAs(duongDan);
                bang.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                ws.Columns(1, soCot).AdjustToContents();
                wb.SaveAs(duongDan);
                bang.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                ws.Columns(1, soCot).AdjustToContents();
                wb.SaveAs(duongDan);
                bang.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                ws.Columns(1, soCot).AdjustToContents();
                wb.SaveAs(duongDan);
}

        }

        private static bool LaSo(object v) => v is byte || v is short || v is int || v is long || v is float || v is double || v is decimal;
}        private static string DinhDangSo(string fmt)
        {
}
            if (fmt.StartsWith("N", StringComparison.OrdinalIgnoreCase))
            {
}
                return soLe > 0 ? "#,##0." + new string('0', soLe) : "#,##0";
            }
}
        }
    }
}
