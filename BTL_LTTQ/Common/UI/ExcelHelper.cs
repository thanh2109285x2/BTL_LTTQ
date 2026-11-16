using ClosedXML.Excel;
using System.Diagnostics;
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
            DataGridView? luoi = TimLuoi(form);
            if (luoi == null)
            {
                MessageBox.Show("Trang hiện tại không có dữ liệu để xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string tieuDe = form is IXuatExcel x && !string.IsNullOrWhiteSpace(x.TieuDeXuat)
                ? x.TieuDeXuat
                : form.Text;
            tieuDe = tieuDe.Trim().ToUpperInvariant();
            XuatDataGridView(luoi, tieuDe, TenFile(tieuDe));
        }

        public static DataGridView? TimLuoi(Form? form)
        {
            if (form == null) return null;
            if (form is IXuatExcel x && x.LuoiXuat != null) return x.LuoiXuat;

            DataGridView? tot = null;
            void Duyet(Control cha)
            {
                foreach (Control control in cha.Controls)
                {
                    if (control is DataGridView grid && grid.Visible && (tot == null || grid.Rows.Count > tot.Rows.Count))
                        tot = grid;
                    if (control.HasChildren) Duyet(control);
                }
            }

            Duyet(form);
            return tot;
        }

        private static string TenFile(string s)
        {
            var sb = new StringBuilder();
            foreach (char ch in s.Normalize(NormalizationForm.FormD))
            {
                if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                    sb.Append(ch);
            }

            string bo = sb.ToString().Replace('Đ', 'D').Replace('đ', 'd');
            var tu = bo.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(word => new string(word.Where(char.IsLetterOrDigit).ToArray()))
                .Where(word => word.Length > 0)
                .Select(word => char.ToUpper(word[0]) + word.Substring(1).ToLower());
            string kq = string.Concat(tu);
            return kq.Length > 0 ? kq : "BaoCao";
        }

        public static void XuatDataGridView(DataGridView dgv, string tieuDe, string tenFileMacDinh = "BaoCao")
        {
            if (dgv == null || dgv.Rows.Cast<DataGridViewRow>().All(row => row.IsNewRow))
            {
                MessageBox.Show("Không có dữ liệu để xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Title = "Chọn nơi lưu file Excel",
                Filter = "Excel (*.xlsx)|*.xlsx",
                FileName = $"{tenFileMacDinh}_{DateTime.Now:yyyyMMdd_HHmm}.xlsx"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                GhiFile(dgv, tieuDe, sfd.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xuất Excel thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Xuất file thành công! Bạn có muốn mở file không?", "Xuất Excel", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Process.Start(new ProcessStartInfo(sfd.FileName) { UseShellExecute = true });
        }

        private enum Kieu { Chu, So, Ngay }

        private static void GhiFile(DataGridView dgv, string tieuDe, string duongDan)
        {
            List<DataGridViewColumn> cols = dgv.Columns.Cast<DataGridViewColumn>()
                .Where(column => column.Visible)
                .OrderBy(column => column.DisplayIndex)
                .ToList();
            List<DataGridViewRow> rows = dgv.Rows.Cast<DataGridViewRow>()
                .Where(row => !row.IsNewRow)
                .ToList();

            int soCot = cols.Count;
            if (soCot == 0)
                throw new InvalidOperationException("Bảng không có cột nào đang hiển thị.");

            int soDong = rows.Count;
            const int dongTieuDe = 1;
            const int dongNgay = 2;
            const int dongHeader = 4;
            int dongDau = dongHeader + 1;
            int dongCuoi = dongHeader + soDong;

            using var wb = new XLWorkbook();
            IXLWorksheet ws = wb.Worksheets.Add("Dữ liệu");

            IXLRange rTitle = ws.Range(dongTieuDe, 1, dongTieuDe, soCot);
            rTitle.Merge();
            ws.Cell(dongTieuDe, 1).Value = tieuDe;
            rTitle.Style.Font.Bold = true;
            rTitle.Style.Font.FontSize = 16;
            rTitle.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            IXLRange rNgay = ws.Range(dongNgay, 1, dongNgay, soCot);
            rNgay.Merge();
            ws.Cell(dongNgay, 1).Value = "Ngày xuất: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            rNgay.Style.Font.Italic = true;
            rNgay.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            for (int i = 0; i < soCot; i++)
                ws.Cell(dongHeader, i + 1).Value = cols[i].HeaderText;

            IXLRange rHeader = ws.Range(dongHeader, 1, dongHeader, soCot);
            rHeader.Style.Fill.BackgroundColor = XLColor.FromColor(GiaoDien.MauChinh);
            rHeader.Style.Font.FontColor = XLColor.White;
            rHeader.Style.Font.Bold = true;
            rHeader.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            var kieu = new Kieu[soCot];
            for (int i = 0; i < soCot; i++)
            {
                kieu[i] = Kieu.Chu;
                foreach (DataGridViewRow row in rows)
                {
                    object? value = row.Cells[cols[i].Index].Value;
                    if (value == null || value is DBNull) continue;
                    if (value is DateTime) kieu[i] = Kieu.Ngay;
                    else if (LaSo(value)) kieu[i] = Kieu.So;
                    break;
                }

                if (soDong == 0) continue;
                IXLRange cotDuLieu = ws.Range(dongDau, i + 1, dongCuoi, i + 1);
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
                    DataGridViewCell cell = rows[r].Cells[cols[c].Index];
                    object? value = cell.Value;
                    if (value == null || value is DBNull) continue;

                    IXLCell target = ws.Cell(dongDau + r, c + 1);
                    if (value is DateTime date)
                        target.Value = date;
                    else if (kieu[c] == Kieu.So && LaSo(value))
                        target.Value = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                    else
                        target.Value = cell.FormattedValue?.ToString() ?? string.Empty;
                }
            }

            IXLRange bang = ws.Range(dongHeader, 1, Math.Max(dongHeader, dongCuoi), soCot);
            bang.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            bang.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            ws.Columns(1, soCot).AdjustToContents();
            wb.SaveAs(duongDan);
        }

        private static bool LaSo(object? value) => value is byte || value is short || value is int || value is long ||
            value is float || value is double || value is decimal;

        private static string DinhDangSo(string? fmt)
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
