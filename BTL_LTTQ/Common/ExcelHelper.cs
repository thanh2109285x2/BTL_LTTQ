using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace BTL_LTTQ.Common
{
    public static class ExcelHelper
    {
        public static void XuatDataGridView(DataGridView dgv, string tieuDe, string tenFileMacDinh = "BaoCao")
        {
            if (dgv == null || dgv.Rows.Cast<DataGridViewRow>().All(r => r.IsNewRow))
            {
                MessageBox.Show("Không có dữ liệu để xuất.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var sfd = new SaveFileDialog())
            {
                sfd.Title = "Chọn nơi lưu file Excel";
                sfd.Filter = "Excel (*.xlsx)|*.xlsx";
                sfd.FileName = $"{tenFileMacDinh}_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";
                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    GhiFile(dgv, tieuDe, sfd.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Xuất Excel thất bại: " + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (MessageBox.Show("Xuất file thành công! Bạn có muốn mở file không?", "Xuất Excel",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Process.Start(new ProcessStartInfo(sfd.FileName) { UseShellExecute = true });
                }
            }
        }

        private enum Kieu 
        { 
            Chu, So, Ngay 
        }

        private static Excel.Range O(Excel.Worksheet ws, int dong, int cot)
        {
            return (Excel.Range)ws.Cells[dong, cot];
        }

        private static void GhiFile(DataGridView dgv, string tieuDe, string duongDan)
        {
            var cols = dgv.Columns.Cast<DataGridViewColumn>()
                          .Where(c => c.Visible)
                          .OrderBy(c => c.DisplayIndex)
                          .ToList();
            var rows = dgv.Rows.Cast<DataGridViewRow>().Where(r => !r.IsNewRow).ToList();
            int soCot = cols.Count;
            int soDong = rows.Count;

            const int dongTieuDe = 1, dongNgay = 2, dongHeader = 4;
            int dongDau = dongHeader + 1;
            int dongCuoi = dongHeader + soDong;

            Excel.Application app = null;
            Excel.Workbook wb = null;
            Excel.Worksheet ws = null;

            try
            {
                app = new Excel.Application { Visible = false, DisplayAlerts = false };
                wb = app.Workbooks.Add(Type.Missing);
                ws = (Excel.Worksheet)wb.Worksheets[1];
                ws.Name = "Dữ liệu";

                // Tiêu đề + ngày xuất
                Excel.Range rTitle = ws.Range[O(ws, dongTieuDe, 1), O(ws, dongTieuDe, soCot)];
                rTitle.Merge();
                O(ws, dongTieuDe, 1).Value2 = tieuDe;
                rTitle.Font.Bold = true;
                rTitle.Font.Size = 16;
                rTitle.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                Excel.Range rNgay = ws.Range[O(ws, dongNgay, 1), O(ws, dongNgay, soCot)];
                rNgay.Merge();
                O(ws, dongNgay, 1).Value2 = "Ngày xuất: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                rNgay.Font.Italic = true;
                rNgay.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // Dòng tiêu đề cột
                for (int i = 0; i < soCot; i++)
                    O(ws, dongHeader, i + 1).Value2 = cols[i].HeaderText;

                Excel.Range rHeader = ws.Range[O(ws, dongHeader, 1), O(ws, dongHeader, soCot)];
                rHeader.Interior.Color = ColorTranslator.ToOle(GiaoDien.MauChinh);
                rHeader.Font.Color = ColorTranslator.ToOle(Color.White);
                rHeader.Font.Bold = true;
                rHeader.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                var kieu = new Kieu[soCot];
                for (int i = 0; i < soCot; i++)
                {
                    kieu[i] = Kieu.Chu;
                    foreach (var r in rows)
                    {
                        object v = r.Cells[cols[i].Index].Value;
                        if (v == null || v is DBNull) continue;
                        if (v is DateTime) kieu[i] = Kieu.Ngay;
                        else if (v is byte || v is short || v is int || v is long ||
                                 v is float || v is double || v is decimal) kieu[i] = Kieu.So;
                        break;
                    }

                    Excel.Range cotDuLieu = ws.Range[O(ws, dongDau, i + 1), O(ws, dongCuoi, i + 1)];
                    switch (kieu[i])
                    {
                        case Kieu.Ngay:
                            cotDuLieu.NumberFormat = "dd/mm/yyyy";
                            cotDuLieu.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            break;
                        case Kieu.So:
                            cotDuLieu.NumberFormat = DinhDangSo(cols[i].DefaultCellStyle.Format);
                            break;
                        default:
                            cotDuLieu.NumberFormat = "@";
                            break;
                    }
                    Marshal.ReleaseComObject(cotDuLieu);
                }

                object[,] data = new object[soDong, soCot];
                for (int r = 0; r < soDong; r++)
                {
                    for (int c = 0; c < soCot; c++)
                    {
                        var cell = rows[r].Cells[cols[c].Index];
                        object v = cell.Value;

                        if (v == null || v is DBNull) data[r, c] = null;
                        else if (v is DateTime d) data[r, c] = d.ToOADate();
                        else if (kieu[c] == Kieu.So && (v is byte || v is short || v is int || v is long ||
                                 v is float || v is double || v is decimal)) data[r, c] = Convert.ToDouble(v);
                        else data[r, c] = cell.FormattedValue?.ToString() ?? "";
                    }
                }
                Excel.Range vung = ws.Range[O(ws, dongDau, 1), O(ws, dongCuoi, soCot)];
                vung.Value2 = data;

                // Viền + tự giãn cột
                Excel.Range bang = ws.Range[O(ws, dongHeader, 1), O(ws, dongCuoi, soCot)];
                bang.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                bang.Columns.AutoFit();

                wb.SaveAs(duongDan, Excel.XlFileFormat.xlOpenXMLWorkbook);

                Marshal.ReleaseComObject(vung);
                Marshal.ReleaseComObject(bang);
                Marshal.ReleaseComObject(rHeader);
                Marshal.ReleaseComObject(rTitle);
                Marshal.ReleaseComObject(rNgay);
            }
            finally
            {
                if (wb != null) { wb.Close(false); Marshal.ReleaseComObject(wb); }
                if (ws != null) Marshal.ReleaseComObject(ws);
                if (app != null) { app.Quit(); Marshal.ReleaseComObject(app); }
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private static string DinhDangSo(string fmt)
        {
            if (string.IsNullOrEmpty(fmt)) return "General";
            if (fmt.StartsWith("N", StringComparison.OrdinalIgnoreCase))
            {
                int.TryParse(fmt.Substring(1), out int soLe);
                return soLe > 0 ? "#,##0." + new string('0', soLe) : "#,##0";
            }
            return "General";
        }
    }
}
