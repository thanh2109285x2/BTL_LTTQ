using BTL_LTTQ.DAL;
using BTL_LTTQ.Common;
using BTL_LTTQ.BUS;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Drawing.Printing;

namespace BTL_LTTQ.Forms.BaoCao
{
    public class frmBaoCaoBase : Form
    {
        private readonly BaoCaoService _baoCaoService = new();
        private readonly bool _laBaoCaoDiem;
        private DataTable? _duLieuGoc;
        private readonly PrintDocument _printDocument = new();
        private int _dongIn;

        public frmBaoCaoBase() : this(false) { }

        protected frmBaoCaoBase(bool laBaoCaoDiem)
        {
            _laBaoCaoDiem = laBaoCaoDiem;
            GiaoDien.ApDung(this);
            Load += FrmBaoCao_Load;
            _printDocument.PrintPage += InBaoCao_PrintPage;
            _printDocument.DefaultPageSettings.Landscape = true;
            FormClosed += (_, _) => _printDocument.Dispose();
        }

        protected virtual ComboBox cboLop => null!;
        protected virtual DataGridView dgvBaoCao => null!;
        protected virtual Label lblTong => null!;

        private void FrmBaoCao_Load(object? sender, EventArgs e)
        {
            string maChucNang = _laBaoCaoDiem ? ChucNangConst.BC_DIEM : ChucNangConst.BC_DOANHTHU;
            if (!PermissionService.Can(maChucNang))
            {
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này.", "Không có quyền",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            try
            {
                var lop = _baoCaoService.LayDanhSachLop();
                var tatCa = lop.NewRow();
                tatCa["MaLop"] = DBNull.Value;
                tatCa["TenLop"] = "-- Tất cả lớp --";
                lop.Rows.InsertAt(tatCa, 0);
                cboLop.DataSource = lop;
                cboLop.DisplayMember = "TenLop";
                cboLop.ValueMember = "MaLop";
                GanSuKienCongCu();
                NapDuLieu();
            }
            catch (Exception ex) { HienLoi(ex); }
        }

        protected void NapDuLieu()
        {
            try
            {
                string? maLop = cboLop.SelectedValue == null || cboLop.SelectedValue == DBNull.Value
                    ? null : cboLop.SelectedValue.ToString();
                _duLieuGoc = _laBaoCaoDiem ? _baoCaoService.LayBangDiem(maLop) : _baoCaoService.LayDoanhThu(maLop);
                dgvBaoCao.DataSource = _duLieuGoc;
                DinhDangCot(_duLieuGoc);
                ApDungBoLocVaSapXep();
                CapNhatTong(_duLieuGoc.DefaultView.ToTable());
            }
            catch (Exception ex) { HienLoi(ex); }
        }

        private T? TimDieuKhien<T>(string name) where T : Control =>
            Controls.Find(name, true).OfType<T>().FirstOrDefault();

        private void GanSuKienCongCu()
        {
            var txtTimKiem = TimDieuKhien<TextBox>("txtTimKiemBaoCao");
            var cboSapXep = TimDieuKhien<ComboBox>("cboSapXepBaoCao");
            var btnSapXep = TimDieuKhien<Button>("btnSapXepBaoCao");
            var btnLamMoi = TimDieuKhien<Button>("btnLamMoiBaoCao");
            var btnExcel = TimDieuKhien<Button>("btnXuatExcelBaoCao");
            var btnIn = TimDieuKhien<Button>("btnXemInPdfBaoCao");

            txtTimKiem!.PlaceholderText = _laBaoCaoDiem ? "Mã hoặc tên học viên" : "Tên lớp hoặc môn học";
            cboSapXep!.Items.AddRange(_laBaoCaoDiem
                ? new object[] { "Điểm giảm dần", "Điểm tăng dần", "Tên học viên A-Z" }
                : new object[] { "Doanh thu giảm dần", "Doanh thu tăng dần", "Tên lớp A-Z" });
            cboSapXep.SelectedIndex = 0;
            txtTimKiem.TextChanged += (_, _) => ApDungBoLocVaSapXep();
            btnSapXep!.Click += (_, _) => ApDungBoLocVaSapXep();
            btnLamMoi!.Click += (_, _) =>
            {
                cboLop.SelectedIndex = 0;
                txtTimKiem.Clear();
                cboSapXep.SelectedIndex = 0;
                NapDuLieu();
            };
            btnExcel!.Click += (_, _) => ExcelHelper.XuatDataGridView(
                dgvBaoCao,
                _laBaoCaoDiem ? "BÁO CÁO ĐIỂM THEO LỚP" : "BÁO CÁO DOANH THU THEO LỚP",
                _laBaoCaoDiem ? "BaoCaoDiem" : "BaoCaoDoanhThu");
            btnIn!.Click += (_, _) =>
            {
                if (dgvBaoCao.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để in.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                _dongIn = 0;
                using var preview = new PrintPreviewDialog { Document = _printDocument, Width = 1000, Height = 700 };
                preview.ShowDialog(this);
            };
        }

        private void ApDungBoLocVaSapXep()
        {
            if (_duLieuGoc == null) return;
            var txt = TimDieuKhien<TextBox>("txtTimKiemBaoCao");
            var cbo = TimDieuKhien<ComboBox>("cboSapXepBaoCao");
            if (txt == null || cbo == null) return;

            string tuKhoa = txt.Text.Trim().Replace("'", "''");
            var cotTim = _laBaoCaoDiem
                ? new[] { "MaHocVien", "TenHocVien", "MaLop", "TenLop" }
                : new[] { "MaLop", "TenLop", "TenMon" };
            string loc = string.IsNullOrEmpty(tuKhoa)
                ? string.Empty
                : string.Join(" OR ", cotTim.Select(c => $"Convert([{c}], 'System.String') LIKE '%{tuKhoa}%'"));
            string sapXep = cbo.SelectedIndex switch
            {
                1 => _laBaoCaoDiem ? "Diem ASC, TenHocVien ASC" : "DoanhThu ASC, TenLop ASC",
                2 => _laBaoCaoDiem ? "TenHocVien ASC" : "TenLop ASC",
                _ => _laBaoCaoDiem ? "Diem DESC, TenHocVien ASC" : "DoanhThu DESC, TenLop ASC"
            };
            _duLieuGoc.DefaultView.RowFilter = loc;
            _duLieuGoc.DefaultView.Sort = sapXep;
            dgvBaoCao.DataSource = _duLieuGoc.DefaultView;
            DinhDangCot(_duLieuGoc);
            CapNhatTong(_duLieuGoc.DefaultView.ToTable());
        }

        private void CapNhatTong(DataTable data)
        {
            if (_laBaoCaoDiem)
            {
                var diem = data.AsEnumerable().Where(r => !r.IsNull("Diem")).Select(r => Convert.ToDouble(r["Diem"])).ToList();
                lblTong.Text = $"Số học viên có điểm: {diem.Count}     Điểm trung bình: {(diem.Count == 0 ? "—" : diem.Average().ToString("0.##"))}";
            }
            else
            {
                decimal tong = data.AsEnumerable().Sum(r => r.Field<decimal>("DoanhThu"));
                lblTong.Text = $"Tổng doanh thu: {tong:N0} VNĐ";
            }
        }

        private void InBaoCao_PrintPage(object? sender, PrintPageEventArgs e)
        {
            if (e.Graphics == null) return;
            var bounds = e.MarginBounds;
            using var titleFont = new Font("Arial", 14, FontStyle.Bold);
            using var headerFont = new Font("Arial", 9, FontStyle.Bold);
            using var bodyFont = new Font("Arial", 9);
            using var pen = new Pen(Color.Gray);
            using var format = new StringFormat { Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap };
            string title = _laBaoCaoDiem ? "BÁO CÁO ĐIỂM THEO LỚP" : "BÁO CÁO DOANH THU THEO LỚP";
            e.Graphics.DrawString(title, titleFont, Brushes.Black, bounds.Left, bounds.Top - 35);

            var cols = dgvBaoCao.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).OrderBy(c => c.DisplayIndex).ToList();
            if (cols.Count == 0) { e.HasMorePages = false; return; }
            int rowHeight = 28;
            float colWidth = bounds.Width / (float)cols.Count;
            int y = bounds.Top;
            for (int c = 0; c < cols.Count; c++)
            {
                var rect = new RectangleF(bounds.Left + c * colWidth, y, colWidth, rowHeight);
                e.Graphics.FillRectangle(Brushes.LightSteelBlue, rect);
                e.Graphics.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
                e.Graphics.DrawString(cols[c].HeaderText, headerFont, Brushes.Black, rect, format);
            }
            y += rowHeight;
            while (_dongIn < dgvBaoCao.Rows.Count && y + rowHeight <= bounds.Bottom)
            {
                var row = dgvBaoCao.Rows[_dongIn++];
                if (row.IsNewRow) continue;
                for (int c = 0; c < cols.Count; c++)
                {
                    var rect = new RectangleF(bounds.Left + c * colWidth, y, colWidth, rowHeight);
                    e.Graphics.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
                    e.Graphics.DrawString(Convert.ToString(row.Cells[cols[c].Index].FormattedValue) ?? "", bodyFont, Brushes.Black, rect, format);
                }
                y += rowHeight;
            }
            e.HasMorePages = _dongIn < dgvBaoCao.Rows.Count;
            if (!e.HasMorePages) _dongIn = 0;
        }

        private void DinhDangCot(DataTable data)
        {
            string[] headers = _laBaoCaoDiem
                ? ["Mã lớp", "Tên lớp", "Mã học viên", "Họ tên", "Điểm", "Xếp loại"]
                : ["Mã lớp", "Tên lớp", "Môn học", "Số học viên đã nộp", "Học phí / học viên", "Doanh thu"];
            for (int i = 0; i < data.Columns.Count; i++)
            {
                var col = dgvBaoCao.Columns[i];
                col.HeaderText = headers[i];
                if (_laBaoCaoDiem && col.Name == "Diem") col.DefaultCellStyle.Format = "0.##";
                if (!_laBaoCaoDiem && (col.Name == "DoanhThu" || col.Name == "HocPhiMoiHocVien")) col.DefaultCellStyle.Format = "N0";
            }
        }

        private static void HienLoi(Exception ex) => MessageBox.Show(
            ex is SqlException ? "Không tải được báo cáo: " + ex.Message : "Có lỗi khi tải báo cáo: " + ex.Message,
            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}
