using BTL_LTTQ.BUS;
using BTL_LTTQ.Common;
using BTL_LTTQ.Models;
using System.Data;
using System.Globalization;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.QuanLy
{
    public partial class frmDanhMucKhac : Form, IXuatExcel
    {
        private readonly DanhMucService _service = new();
        private readonly Dictionary<string, Control> _inputs = new(StringComparer.Ordinal);
        private readonly List<CheckBox> _dayChecks = new();
        private DanhMucMoTa? _current;
        private Label? _dayLabel;
        private string _selectedKey = "";
        private bool _changing;
        private bool _thoiGianDangDuocDung;

        public DataGridView LuoiXuat => dgvDanhMuc;
        public string TieuDeXuat => _current?.TenHienThi ?? "Danh mục khác";

        public frmDanhMucKhac()
        {
            InitializeComponent();
            GiaoDien.ApDung(this);
        }

        private void frmDanhMucKhac_Load(object? sender, EventArgs e)
        {
            if (!_service.CoQuyen)
            {
                MessageBox.Show("Bạn không có quyền sử dụng chức năng Danh mục khác.", "Không có quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }
            cboDanhMuc.DataSource = _service.LayMoTa().ToList();
            cboDanhMuc.DisplayMember = nameof(DanhMucMoTa.TenHienThi);
            cboDanhMuc.ValueMember = nameof(DanhMucMoTa.Loai);
            if (cboDanhMuc.Items.Count > 0) ChonDanhMuc();
        }

        private void cboDanhMuc_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_changing) return;
            ChonDanhMuc();
        }

        private void ChonDanhMuc()
        {
            if (cboDanhMuc.SelectedItem is not DanhMucMoTa moTa) return;
            _current = moTa;
            _selectedKey = "";
            DungTruongNhap(moTa);
            NapDanhSach();
            LamMoiForm();
        }

        private void DungTruongNhap(DanhMucMoTa moTa)
        {
            grpThongTin.SuspendLayout();
            grpThongTin.Controls.Clear();
            _inputs.Clear();
            _dayChecks.Clear();
            _dayLabel = null;
            int y = 28;
            foreach (DanhMucTruong field in moTa.Truong)
            {
                var label = new Label { Text = field.Nhan, AutoSize = true, Location = new Point(18, y + 5) };
                Control input = field.Kieu switch
                {
                    DanhMucKieuTruong.So => new TextBox { Location = new Point(190, y), Width = 260 },
                    DanhMucKieuTruong.Gio => new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "HH:mm", ShowUpDown = true, ShowCheckBox = false, Location = new Point(190, y), Width = 110 },
                    _ => new TextBox { Location = new Point(190, y), Width = 420, MaxLength = field.MaxLength ?? 4000 }
                };
                input.Name = "input_" + field.Ten;
                if (moTa.Loai == DanhMucLoai.ThoiGian && field.Ten == "TenThoiGian" && input is TextBox name)
                    name.ReadOnly = true;
                if (field.LaKhoa) input.Enabled = true;
                grpThongTin.Controls.Add(label);
                grpThongTin.Controls.Add(input);
                _inputs[field.Ten] = input;
                y += 38;
            }
            if (moTa.CoThuTrongTuan)
            {
                _dayLabel = new Label { Text = "Các thứ", AutoSize = true, Location = new Point(18, y + 5) };
                grpThongTin.Controls.Add(_dayLabel);
                for (int i = 0; i < ThuTrongTuanConst.CacThu.Count; i++)
                {
                    byte thu = ThuTrongTuanConst.CacThu[i];
                    var check = new CheckBox
                    {
                        Text = ThuTrongTuanConst.TenHienThi(thu),
                        Tag = thu,
                        AutoSize = true,
                        Location = new Point(190 + (i % 4) * 112, y + (i / 4) * 30)
                    };
                    check.CheckedChanged += (_, _) => CapNhatTenTuThu();
                    grpThongTin.Controls.Add(check);
                    _dayChecks.Add(check);
                }
                y += 66;
            }
            grpThongTin.Height = Math.Max(150, y + 16);
            grpThongTin.ResumeLayout();
        }

        private void CapNhatTenTuThu()
        {
            if (_changing || _current?.Loai != DanhMucLoai.ThoiGian || _inputs["TenThoiGian"] is not TextBox name) return;
            name.Text = ThuTrongTuanConst.TaoTenThoiGian(LayCacThu());
        }

        private void NapDanhSach()
        {
            if (_current == null) return;
            string error = _service.LayDanhSach(_current.Loai, out DataTable data);
            if (error != "") { BaoLoi(error); return; }
            dgvDanhMuc.DataSource = data;
            foreach (DataGridViewColumn column in dgvDanhMuc.Columns)
            {
                column.ReadOnly = true;
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.HeaderText = _current.Truong.FirstOrDefault(field => field.Ten == column.Name)?.Nhan ?? (column.Name == DanhMucKhacConst.CotCacThu ? "Các thứ" : column.Name);
                if (column.Name is DanhMucKhacConst.CotCacThuMa or DanhMucKhacConst.CotDaDuocSuDung) column.Visible = false;
                if (column.Name is "GioBatDau" or "GioKetThuc") column.DefaultCellStyle.Format = @"hh\:mm";
            }
            dgvDanhMuc.ClearSelection();
        }

        private void dgvDanhMuc_SelectionChanged(object? sender, EventArgs e)
        {
            if (_changing || _current == null || dgvDanhMuc.SelectedRows.Count == 0) return;
            DataGridViewRow row = dgvDanhMuc.SelectedRows[0];
            _selectedKey = Convert.ToString(row.Cells[_current.Truong[0].Ten].Value) ?? "";
            _thoiGianDangDuocDung = _current.Loai == DanhMucLoai.ThoiGian &&
                Convert.ToBoolean(row.Cells[DanhMucKhacConst.CotDaDuocSuDung].Value);
            _changing = true;
            foreach (DanhMucTruong field in _current.Truong)
            {
                object? value = row.Cells[field.Ten].Value;
                Control input = _inputs[field.Ten];
                if (field.LaKhoa) input.Enabled = false;
                if (input is TextBox text) text.Text = value == null || value == DBNull.Value ? "" : Convert.ToString(value, CultureInfo.CurrentCulture) ?? "";
                else if (input is TextBox number) number.Text = value == null || value == DBNull.Value ? "" : Convert.ToString(value, CultureInfo.InvariantCulture) ?? "";
                else if (input is DateTimePicker time)
                {
                    time.Value = DateTime.Today.Add(value is TimeSpan storedTime ? storedTime : GioMacDinh(field));
                }
            }
            if (_current.Loai == DanhMucLoai.ThoiGian)
            {
                var thuDaLuu = (Convert.ToString(row.Cells[DanhMucKhacConst.CotCacThuMa].Value) ?? "")
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(thu => byte.TryParse(thu, out byte parsed) ? parsed : (byte)0)
                    .ToHashSet();
                foreach (CheckBox check in _dayChecks)
                {
                    check.Checked = thuDaLuu.Contains(Convert.ToByte(check.Tag));
                    check.Enabled = !_thoiGianDangDuocDung;
                }
                if (_dayLabel != null)
                    _dayLabel.Text = _thoiGianDangDuocDung
                        ? "Các thứ (không thể đổi vì đang được lớp sử dụng)"
                        : "Các thứ";
            }
            _changing = false;
            CapNhatTenTuThu();
        }

        private IReadOnlyDictionary<string, object?> LayGiaTri(out string error)
        {
            error = "";
            var values = new Dictionary<string, object?>(StringComparer.Ordinal);
            if (_current == null) return values;
            foreach (DanhMucTruong field in _current.Truong)
            {
                Control input = _inputs[field.Ten];
                object? value = input switch
                {
                    TextBox text when field.Kieu == DanhMucKieuTruong.Text => string.IsNullOrWhiteSpace(text.Text) ? null : text.Text.Trim(),
                    TextBox number when field.Kieu == DanhMucKieuTruong.So => string.IsNullOrWhiteSpace(number.Text) ? null : number.Text.Trim(),
                    DateTimePicker time => time.Value.TimeOfDay,
                    _ => null
                };
                values[field.Ten] = value;
            }
            return values;
        }

        private byte[] LayCacThu() => _dayChecks.Where(check => check.Checked)
            .Select(check => Convert.ToByte(check.Tag)).ToArray();

        private static TimeSpan GioMacDinh(DanhMucTruong field) =>
            field.Ten == "GioBatDau" ? TimeSpan.FromHours(8) : TimeSpan.FromHours(10);

        private void btnThem_Click(object? sender, EventArgs e)
        {
            if (_current == null) return;
            IReadOnlyDictionary<string, object?> values = LayGiaTri(out string error);
            if (error != "") { BaoLoi(error); return; }
            error = _service.Them(_current.Loai, values, LayCacThu());
            if (error != "") { BaoLoi(error); return; }
            DaLuu("Đã thêm danh mục.");
        }

        private void btnSua_Click(object? sender, EventArgs e)
        {
            if (_current == null || _selectedKey == "") { BaoLoi("Vui lòng chọn dòng cần sửa."); return; }
            IReadOnlyDictionary<string, object?> values = LayGiaTri(out string error);
            if (error != "") { BaoLoi(error); return; }
            error = _service.Sua(_current.Loai, _selectedKey, values, LayCacThu());
            if (error != "") { BaoLoi(error); return; }
            DaLuu("Đã cập nhật danh mục.");
        }

        private void btnXoa_Click(object? sender, EventArgs e)
        {
            if (_current == null || _selectedKey == "") { BaoLoi("Vui lòng chọn dòng cần xóa."); return; }
            if (MessageBox.Show($"Xóa danh mục có mã {_selectedKey}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            string error = _service.Xoa(_current.Loai, _selectedKey);
            if (error != "") { BaoLoi(error); return; }
            DaLuu("Đã xóa danh mục.");
        }

        private void btnLamMoi_Click(object? sender, EventArgs e) { LamMoiForm(); dgvDanhMuc.ClearSelection(); }

        private void LamMoiForm()
        {
            _selectedKey = "";
            foreach (DanhMucTruong field in _current?.Truong ?? Array.Empty<DanhMucTruong>())
            {
                Control input = _inputs[field.Ten];
                input.Enabled = true;
                if (input is TextBox text) text.Clear();
                else if (input is TextBox numericText && field.Kieu == DanhMucKieuTruong.So) numericText.Clear();
                else if (input is NumericUpDown numericInput) numericInput.Value = 0;
                else if (input is DateTimePicker time) time.Value = DateTime.Today.Add(GioMacDinh(field));
            }
            _thoiGianDangDuocDung = false;
            if (_dayLabel != null) _dayLabel.Text = "Các thứ";
            foreach (CheckBox check in _dayChecks)
            {
                check.Enabled = true;
                check.Checked = false;
            }
            if (_current?.Loai == DanhMucLoai.ThoiGian && _inputs["MaThoiGian"] is TextBox code)
            {
                string error = _service.LayMaThoiGianKeTiep(out string nextCode);
                if (error == "") code.Text = nextCode;
                else BaoLoi(error);
            }
            CapNhatTenTuThu();
        }

        private void DaLuu(string message)
        {
            MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            NapDanhSach();
            LamMoiForm();
        }

        private void BaoLoi(string message) => MessageBox.Show(message, "Danh mục khác", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}
