using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace BTL_LTTQ.Common
{
    public interface IXuatExcel
    {
        DataGridView LuoiXuat { get; }
        string TieuDeXuat { get; }
    }

    public static class ExcelHelper
    {
        //Xuất bảng của đúng trang (form con) đang mở
        public static void XuatTrangHienTai(Form form)
        {
            var luoi = TimLuoi(form);
            if (luoi == null)
            {
                MessageBox.Show("Trang hiện tại không có dữ liệu để xuất.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string tieuDe = (form is IXuatExcel x && !string.IsNullOrWhiteSpace(x.TieuDeXuat))
                            ? x.TieuDeXuat : form.Text;
            tieuDe = tieuDe.Trim().ToUpper();
            XuatDataGridView(luoi, tieuDe, TenFile(tieuDe));
        }

        //Tìm bảng cần xuất trong form
        public static DataGridView TimLuoi(Form form)
        {
            if (form == null) return null;
            if (form is IXuatExcel x && x.LuoiXuat != null) return x.LuoiXuat;

            DataGridView tot = null;
            void Duyet(Control cha)
            {
                foreach (Control c in cha.Controls)
                {
                    if (c is DataGridView g && g.Visible && (tot == null || g.Rows.Count > tot.Rows.Count))
                        tot = g;
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
                if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                    sb.Append(ch);
            string bo = sb.ToString().Replace('Đ', 'D').Replace('đ', 'd');
            var tu = bo.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                       .Select(w => new string(w.Where(char.IsLetterOrDigit).ToArray()))
                       .Where(w => w.Length > 0)
                       .Select(w => char.ToUpper(w[0]) + w.Substring(1).ToLower());
            string kq = string.Concat(tu);
            return kq.Length > 0 ? kq : "BaoCao";
        }

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

        private enum Kieu { Chu, So, Ngay }

        private static void GhiFile(DataGridView dgv, string tieuDe, string duongDan)
        {
            var cols = dgv.Columns.Cast<DataGridViewColumn>()
                          .Where(c => c.Visible)
                          .OrderBy(c => c.DisplayIndex)
                          .ToList();
            var rows = dgv.Rows.Cast<DataGridViewRow>().Where(r => !r.IsNewRow).ToList();
            int soCot = cols.Count;
            int soDong = rows.Count;

            if (soCot == 0)
                throw new InvalidOperationException("Không có cột hiển thị để xuất.");

            const int dongTieuDe = 1, dongNgay = 2, dongHeader = 4;
            int dongDau = dongHeader + 1;
            int dongCuoi = dongHeader + soDong;

            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Dữ liệu");

                // Tiêu đề + ngày xuất
                var rTitle = ws.Range(dongTieuDe, 1, dongTieuDe, soCot);
                rTitle.Merge();
                ws.Cell(dongTieuDe, 1).Value = tieuDe;
                rTitle.Style.Font.Bold = true;
                rTitle.Style.Font.FontSize = 16;
                rTitle.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                var rNgay = ws.Range(dongNgay, 1, dongNgay, soCot);
                rNgay.Merge();
                ws.Cell(dongNgay, 1).Value = "Ngày xuất: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                rNgay.Style.Font.Italic = true;
                rNgay.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Dòng tiêu đề cột
                for (int i = 0; i < soCot; i++)
                    ws.Cell(dongHeader, i + 1).Value = cols[i].HeaderText;

                var rHeader = ws.Range(dongHeader, 1, dongHeader, soCot);
                rHeader.Style.Fill.BackgroundColor = XLColor.FromColor(GiaoDien.MauChinh);
                rHeader.Style.Font.FontColor = XLColor.White;
                rHeader.Style.Font.Bold = true;
                rHeader.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Xác định kiểu từng cột, đặt định dạng ô để Excel hiển thị đúng (ngày, số, chữ)
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

                    var cotDuLieu = ws.Range(dongDau, i + 1, dongCuoi, i + 1);
                    switch (kieu[i])
                    {
                        case Kieu.Ngay:
                            cotDuLieu.Style.DateFormat.Format = "dd/mm/yyyy";
                            cotDuLieu.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            break;
                        case Kieu.So:
                            cotDuLieu.Style.NumberFormat.Format = DinhDangSo(cols[i].DefaultCellStyle.Format);
                            break;
                        default:
                            cotDuLieu.Style.NumberFormat.Format = "@";
                            break;
                    }
                }

                for (int r = 0; r < soDong; r++)
                {
                    for (int c = 0; c < soCot; c++)
                    {
                        var cell = rows[r].Cells[cols[c].Index];
                        object v = cell.Value;

                        var target = ws.Cell(dongDau + r, c + 1);
                        if (v == null || v is DBNull) continue;
                        if (v is DateTime date) target.Value = date;
                        else if (kieu[c] == Kieu.So && (v is byte || v is short || v is int || v is long ||
                                 v is float || v is double || v is decimal)) target.Value = Convert.ToDouble(v);
                        else target.Value = cell.FormattedValue?.ToString() ?? "";
                    }
                }

                // Viền + giãn cột
                var bang = ws.Range(dongHeader, 1, dongCuoi, soCot);
                bang.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                bang.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                ws.Columns(1, soCot).AdjustToContents();
                wb.SaveAs(duongDan);
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
