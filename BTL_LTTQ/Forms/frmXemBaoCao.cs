// Form xem trước báo cáo; bố cục nằm trong Designer để chỉnh bằng kéo thả.
using System.Data;
using System.Drawing.Printing;
using BTL_LTTQ.Common;

namespace BTL_LTTQ.Forms
{
    internal partial class frmXemBaoCao : Form
    {
        private readonly string _tieuDe;
        private readonly DataTable _data;
        private readonly PrintDocument _printDocument = new();
        private readonly PrintPreviewDialog _previewDialog = new();
        private int _dongDangIn;

        public frmXemBaoCao(string tieuDe, DataTable data)
        {
            _tieuDe = tieuDe;
            _data = data;
            InitializeComponent();
            GiaoDien.ApDung(this);
            Text = "Xem trước báo cáo";
            lblTieuDe.Text = tieuDe;
            lblThongTin.Text = $"Ngày lập: {DateTime.Now:dd/MM/yyyy HH:mm}     |     Số dòng: {data.Rows.Count}";
            dgvBaoCao.DataSource = data;
            DinhDangBang();

            _printDocument.DocumentName = tieuDe;
            _printDocument.DefaultPageSettings.Landscape = true;
            _printDocument.PrintPage += InTrang;
            _previewDialog.Document = _printDocument;
            _previewDialog.WindowState = FormWindowState.Maximized;
            _previewDialog.UseAntiAlias = true;
        }

        private void DinhDangBang()
        {
            foreach (DataGridViewColumn column in dgvBaoCao.Columns)
            {
                if (column.ValueType == typeof(DateTime)) column.DefaultCellStyle.Format = "dd/MM/yyyy";
                if (column.Name.Contains("Điểm", StringComparison.OrdinalIgnoreCase)) column.DefaultCellStyle.Format = "0.##";
            }
        }

        private void btnInBaoCao_Click(object? sender, EventArgs e)
        {
            _dongDangIn = 0;
            _previewDialog.ShowDialog(this);
        }

        private void btnXuatPdf_Click(object? sender, EventArgs e)
        {
            using var dialog = new SaveFileDialog
            {
                Title = "Lưu báo cáo dạng PDF",
                Filter = "PDF (*.pdf)|*.pdf",
                FileName = $"BaoCao_{DateTime.Now:yyyyMMdd_HHmm}.pdf",
                AddExtension = true,
                DefaultExt = "pdf"
            };
            if (dialog.ShowDialog(this) != DialogResult.OK) return;
            try
            {
                _dongDangIn = 0;
                _printDocument.PrinterSettings = new PrinterSettings
                {
                    PrinterName = "Microsoft Print to PDF",
                    PrintToFile = true,
                    PrintFileName = dialog.FileName
                };
                _printDocument.Print();
                MessageBox.Show("Đã tạo file PDF.", "Báo cáo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không xuất được PDF. Kiểm tra máy có Microsoft Print to PDF. Chi tiết: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { _printDocument.PrinterSettings = new PrinterSettings(); }
        }

        private void btnXuatExcel_Click(object? sender, EventArgs e)
        {
            ExcelHelper.XuatDataGridView(dgvBaoCao, _tieuDe, "BaoCao");
        }

        private void btnDong_Click(object? sender, EventArgs e) => Close();

        private void InTrang(object? sender, PrintPageEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Rectangle bounds = e.MarginBounds;
            using var titleFont = new Font("Arial", 16, FontStyle.Bold);
            using var headerFont = new Font("Arial", 9, FontStyle.Bold);
            using var bodyFont = new Font("Arial", 9);
            using var linePen = new Pen(Color.Gray, 0.7f);
            using var headerBrush = new SolidBrush(GiaoDien.MauChinh);
            using var whiteBrush = new SolidBrush(Color.White);
            using var textBrush = new SolidBrush(Color.Black);
            float y = bounds.Top;
            var titleSize = graphics.MeasureString(_tieuDe, titleFont, bounds.Width);
            graphics.DrawString(_tieuDe, titleFont, textBrush, bounds.Left + (bounds.Width - titleSize.Width) / 2, y);
            y += titleSize.Height + 5;
            string sub = $"Ngày lập: {DateTime.Now:dd/MM/yyyy HH:mm}     |     Số dòng: {_data.Rows.Count}";
            var subSize = graphics.MeasureString(sub, bodyFont);
            graphics.DrawString(sub, bodyFont, textBrush, bounds.Left + (bounds.Width - subSize.Width) / 2, y);
            y += subSize.Height + 14;

            int colCount = _data.Columns.Count;
            if (colCount == 0) { e.HasMorePages = false; return; }
            float colWidth = bounds.Width / colCount;
            float rowHeight = Math.Max(26, graphics.MeasureString("Ag", bodyFont).Height + 8);
            float x = bounds.Left;
            for (int c = 0; c < colCount; c++)
            {
                var rect = new RectangleF(x, y, colWidth, rowHeight);
                graphics.FillRectangle(headerBrush, rect);
                graphics.DrawRectangle(linePen, rect.X, rect.Y, rect.Width, rect.Height);
                DrawText(graphics, _data.Columns[c].ColumnName, headerFont, whiteBrush, rect);
                x += colWidth;
            }
            y += rowHeight;
            while (_dongDangIn < _data.Rows.Count && y + rowHeight <= bounds.Bottom)
            {
                x = bounds.Left;
                DataRow row = _data.Rows[_dongDangIn++];
                for (int c = 0; c < colCount; c++)
                {
                    var rect = new RectangleF(x, y, colWidth, rowHeight);
                    if (_dongDangIn % 2 == 0)
                    {
                        using var alternateBrush = new SolidBrush(Color.FromArgb(234, 241, 250));
                        graphics.FillRectangle(alternateBrush, rect);
                    }
                    graphics.DrawRectangle(linePen, rect.X, rect.Y, rect.Width, rect.Height);
                    string value = row.IsNull(c) ? "" : row[c] is DateTime date ? date.ToString("dd/MM/yyyy") : Convert.ToString(row[c]) ?? "";
                    DrawText(graphics, value, bodyFont, textBrush, rect);
                    x += colWidth;
                }
                y += rowHeight;
            }
            e.HasMorePages = _dongDangIn < _data.Rows.Count;
        }

        private static void DrawText(Graphics graphics, string text, Font font, Brush brush, RectangleF bounds)
        {
            using var format = new StringFormat { Trimming = StringTrimming.EllipsisCharacter, LineAlignment = StringAlignment.Center };
            graphics.DrawString(text, font, brush, RectangleF.Inflate(bounds, -4, -2), format);
        }

        private void dgvBaoCao_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
