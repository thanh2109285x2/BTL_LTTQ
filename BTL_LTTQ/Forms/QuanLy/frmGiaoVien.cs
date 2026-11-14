using BTL_LTTQ.Common;
using BTL_LTTQ.Models;
using BTL_LTTQ.Services;
using System.ComponentModel;
using System.Data;
using System.Drawing;

namespace BTL_LTTQ.Forms.QuanLy
{
    public partial class frmGiaoVien : Form
    {
        private readonly GiaoVienService _service = new();
        private string? _anhDaChon;
        private string _maDangChon = "";
        private bool _chiXem;

        public frmGiaoVien()
        {
            InitializeComponent();
            GiaoDien.ApDung(this);
            Load += Form_Load;
            _btnThem.Click += (_, _) => Them();
            _btnSua.Click += (_, _) => Sua();
            _btnXoa.Click += (_, _) => Xoa();
            _btnMoi.Click += (_, _) => LamMoi();
            _btnChonAnh.Click += (_, _) => ChonAnh();
            _btnTim.Click += (_, _) => NapDanhSach(_txtTim.Text);
            _btnTatCa.Click += (_, _) =>
            {
                _txtTim.Clear();
                NapDanhSach();
            };
            _grid.CellClick += Grid_CellClick;
            _txtTim.KeyDown += Search_KeyDown;
        }

        private void Form_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            if (!PermissionService.Can(ChucNangConst.GIAOVIEN_XEM))
            {
                MessageBox.Show("Bạn không có quyền xem giáo viên.", "Không có quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }
            _ngaySinh.MaxDate = DateTime.Today.AddYears(-18);
            _ngaySinh.Value = _ngaySinh.MaxDate;
            _chiXem = !PermissionService.Can(ChucNangConst.GIAOVIEN_THEM) && !PermissionService.Can(ChucNangConst.GIAOVIEN_SUA);
            PhanQuyenUi.AnNutThemSuaXoa(_btnThem, _btnSua, _btnXoa,
                ChucNangConst.GIAOVIEN_THEM, ChucNangConst.GIAOVIEN_SUA, ChucNangConst.GIAOVIEN_XOA);
            NapCombo(_chucDanh, "ChucDanh", "MaChucDanh", "TenChucDanh");
            NapCombo(_chuyenMon, "ChuyenMon", "MaChuyenMon", "TenChuyenMon");
            NapCombo(_coQuan, "CoQuan", "MaCQ", "TenCQ");
            NapTrinhDo();
            NapCombo(_monDay, "MonHoc", "MaMon", "TenMon");
            NapDanhSach();
            LamMoi();
            ApDungQuyenNhap();
        }

        private void NapCombo(ComboBox combo, string bang, string cotMa, string cotTen)
        {
            DataTable dt = _service.LayDanhMuc(bang, cotMa, cotTen);
            var blank = dt.NewRow();
            blank[cotMa] = "";
            blank[cotTen] = "— Không chọn —";
            dt.Rows.InsertAt(blank, 0);
            combo.DataSource = dt;
            combo.DisplayMember = cotTen;
            combo.ValueMember = cotMa;
            combo.SelectedIndex = 0;
        }

        private void NapCombo(CheckedListBox list, string bang, string cotMa, string cotTen)
        {
            DataTable dt = _service.LayDanhMuc(bang, cotMa, cotTen);
            list.DataSource = dt;
            list.DisplayMember = cotTen;
            list.ValueMember = cotMa;
        }

        private void NapTrinhDo()
        {
            List<RadioButton> radios = _trinhDo.Controls.OfType<RadioButton>().ToList();
            int index = 0;
            foreach (DataRow row in _service.LayDanhMuc("TrinhDo", "MaTrinhDo", "TenTrinhDo").Rows)
            {
                RadioButton radio;
                if (index < radios.Count)
                {
                    radio = radios[index];
                }
                else
                {
                    radio = new RadioButton { AutoSize = true, Margin = new Padding(3, 7, 8, 3) };
                    _trinhDo.Controls.Add(radio);
                    radios.Add(radio);
                }
                radio.Text = Convert.ToString(row["TenTrinhDo"]) ?? "";
                radio.Tag = Convert.ToString(row["MaTrinhDo"]);
                radio.Visible = true;
                radio.Checked = false;
                index++;
            }
            for (int i = index; i < radios.Count; i++) radios[i].Visible = false;
        }

        private string? GiaTriTrinhDo() => _trinhDo.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked)?.Tag?.ToString();

        private void ChonTrinhDo(object? value)
        {
            string ma = value == DBNull.Value ? "" : Convert.ToString(value) ?? "";
            foreach (RadioButton radio in _trinhDo.Controls.OfType<RadioButton>()) radio.Checked = Convert.ToString(radio.Tag) == ma;
        }

        private void ChonAnh()
        {
            using var dialog = new OpenFileDialog { Title = "Chọn ảnh giáo viên", Filter = "Tệp ảnh|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Tất cả tệp|*.*", CheckFileExists = true };
            if (dialog.ShowDialog(this) != DialogResult.OK) return;
            try
            {
                string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "GiaoVien");
                Directory.CreateDirectory(folder);
                string extension = Path.GetExtension(dialog.FileName);
                string fileName = $"{Guid.NewGuid():N}{extension}";
                string destination = Path.Combine(folder, fileName);
                File.Copy(dialog.FileName, destination, true);
                _anhDaChon = Path.Combine("Images", "GiaoVien", fileName);
                using var source = Image.FromFile(destination);
                _previewAnh.Image?.Dispose();
                _previewAnh.Image = new Bitmap(source);
            }
            catch (Exception ex) { BaoLoi("Không thể lưu ảnh: " + ex.Message); }
        }

        private void HienThiAnh(string? relativePath)
        {
            _anhDaChon = string.IsNullOrWhiteSpace(relativePath) ? null : relativePath;
            _previewAnh.Image?.Dispose();
            _previewAnh.Image = null;
            if (_anhDaChon == null) return;
            try
            {
                var candidates = new List<string>();
                if (Path.IsPathRooted(_anhDaChon)) candidates.Add(_anhDaChon);
                else
                {
                    for (DirectoryInfo? directory = new(AppDomain.CurrentDomain.BaseDirectory);
                         directory != null;
                         directory = directory.Parent)
                        candidates.Add(Path.Combine(directory.FullName, _anhDaChon));
                }

                string? fullPath = candidates.Select(Path.GetFullPath).FirstOrDefault(File.Exists);
                if (fullPath == null) return;
                using var source = Image.FromFile(fullPath);
                _previewAnh.Image = new Bitmap(source);
            }
            catch { }
        }

        private void NapDanhSach(string tuKhoa = "")
        {
            _grid.DataSource = _service.LayDanhSach(tuKhoa);
            string[,] headers = { { "MaGV", "Mã giáo viên" }, { "TenGV", "Họ tên" }, { "NgaySinh", "Ngày sinh" },
                { "GioiTinh", "Giới tính" }, { "DiaChi", "Địa chỉ" }, { "DienThoai", "Điện thoại" },
                { "TenChucDanh", "Chức danh" }, { "TenChuyenMon", "Chuyên môn" }, { "TenCQ", "Cơ quan" },
                { "TenTrinhDo", "Trình độ" }, { "MonDay", "Môn giảng dạy" } };
            for (int i = 0; i < headers.GetLength(0); i++)
                if (_grid.Columns[headers[i, 0]] is DataGridViewColumn col) col.HeaderText = headers[i, 1];
            if (_grid.Columns["NgaySinh"] is DataGridViewColumn ngay) ngay.DefaultCellStyle.Format = "dd/MM/yyyy";
            foreach (string hidden in new[] { "MaChucDanh", "MaChuyenMon", "MaCQ", "MaTrinhDo", "Anh" })
                if (_grid.Columns[hidden] is DataGridViewColumn col) col.Visible = false;
        }

        private void Them()
        {
            if (!PermissionService.Can(ChucNangConst.GIAOVIEN_THEM)) return;
            GiaoVien g = LayDuLieu();
            string loi = _service.Them(g);
            if (loi != "") { BaoLoi(loi); return; }
            MessageBox.Show("Đã thêm giáo viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            NapDanhSach(_txtTim.Text);
            LamMoi();
        }

        private void Sua()
        {
            if (!PermissionService.Can(ChucNangConst.GIAOVIEN_SUA)) return;
            if (_maDangChon == "") { BaoLoi("Chọn giáo viên trong danh sách để sửa."); return; }
            GiaoVien g = LayDuLieu();
            g.MaGV = _maDangChon;
            string loi = _service.Sua(g);
            if (loi != "") { BaoLoi(loi); return; }
            MessageBox.Show("Đã cập nhật giáo viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            NapDanhSach(_txtTim.Text);
            LamMoi();
        }

        private void Xoa()
        {
            if (!PermissionService.Can(ChucNangConst.GIAOVIEN_XOA)) return;
            if (_maDangChon == "") { BaoLoi("Chọn giáo viên trong danh sách để xóa."); return; }
            if (MessageBox.Show($"Xóa giáo viên {_maDangChon}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            string loi = _service.Xoa(_maDangChon);
            if (loi != "") { BaoLoi(loi); return; }
            NapDanhSach(_txtTim.Text);
            LamMoi();
        }

        private GiaoVien LayDuLieu() => new()
        {
            MaGV = _txtMa.Text,
            TenGV = _txtTen.Text,
            NgaySinh = _ngaySinh.Checked ? _ngaySinh.Value.Date : null,
            GioiTinh = _nam.Checked ? "Nam" : _nu.Checked ? "Nữ" : null,
            DienThoai = _txtDienThoai.Text,
            DiaChi = _txtDiaChi.Text,
            MaChucDanh = GiaTriCombo(_chucDanh),
            MaChuyenMon = GiaTriCombo(_chuyenMon),
            MaCQ = GiaTriCombo(_coQuan),
            MaTrinhDo = GiaTriTrinhDo(),
            Anh = _anhDaChon,
            MaMonDay = Enumerable.Range(0, _monDay.Items.Count)
                .Where(_monDay.GetItemChecked)
                .Select(i => (_monDay.Items[i] as DataRowView)?["MaMon"]?.ToString() ?? "")
                .Where(x => x != "").ToList()
        };

        private static string? GiaTriCombo(ComboBox combo)
        {
            string? value = combo.SelectedValue?.ToString();
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }

        private void Grid_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = _grid.Rows[e.RowIndex];
            _maDangChon = Convert.ToString(row.Cells["MaGV"].Value) ?? "";
            _txtMa.Text = _maDangChon;
            _txtMa.ReadOnly = true;
            _txtTen.Text = Convert.ToString(row.Cells["TenGV"].Value) ?? "";
            object ngay = row.Cells["NgaySinh"].Value;
            _ngaySinh.Checked = ngay != DBNull.Value && ngay != null;
            if (_ngaySinh.Checked) _ngaySinh.Value = Convert.ToDateTime(ngay);
            string gioiTinh = Convert.ToString(row.Cells["GioiTinh"].Value) ?? "";
            _nam.Checked = gioiTinh == "Nam";
            _nu.Checked = gioiTinh == "Nữ";
            _txtDienThoai.Text = Convert.ToString(row.Cells["DienThoai"].Value) ?? "";
            _txtDiaChi.Text = Convert.ToString(row.Cells["DiaChi"].Value) ?? "";
            HienThiAnh(Convert.ToString(row.Cells["Anh"].Value));
            ChonCombo(_chucDanh, row.Cells["MaChucDanh"].Value);
            ChonCombo(_chuyenMon, row.Cells["MaChuyenMon"].Value);
            ChonCombo(_coQuan, row.Cells["MaCQ"].Value);
            ChonTrinhDo(row.Cells["MaTrinhDo"].Value);
            var subjects = new HashSet<string>(_service.LayMonDay(_maDangChon), StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < _monDay.Items.Count; i++)
            {
                string code = (_monDay.Items[i] as DataRowView)?["MaMon"]?.ToString() ?? "";
                _monDay.SetItemChecked(i, subjects.Contains(code));
            }
            ApDungQuyenNhap();
        }

        private static void ChonCombo(ComboBox combo, object value)
        {
            combo.SelectedValue = value == DBNull.Value ? "" : value;
        }

        private void LamMoi()
        {
            _maDangChon = "";
            _txtMa.Clear();
            _txtTen.Clear();
            _txtDienThoai.Clear();
            _txtDiaChi.Clear();
            HienThiAnh(null);
            _nam.Checked = false;
            _nu.Checked = false;
            _ngaySinh.Value = _ngaySinh.MaxDate;
            _ngaySinh.Checked = false;
            _chucDanh.SelectedIndex = 0;
            _chuyenMon.SelectedIndex = 0;
            _coQuan.SelectedIndex = 0;
            foreach (RadioButton radio in _trinhDo.Controls.OfType<RadioButton>()) radio.Checked = false;
            for (int i = 0; i < _monDay.Items.Count; i++) _monDay.SetItemChecked(i, false);
            _grid.ClearSelection();
            ApDungQuyenNhap();
        }

        private void ApDungQuyenNhap()
        {
            _txtMa.ReadOnly = _chiXem || _maDangChon != "";
            bool sua = _maDangChon != "";
            foreach (Control c in new Control[] { _txtTen, _ngaySinh, _gioiTinh, _txtDienThoai, _txtDiaChi,
                         _chucDanh, _chuyenMon, _coQuan, _trinhDo, _anhPanel, _gioiTinh, _monDay }) c.Enabled = !_chiXem;
            _btnThem.Enabled = !_chiXem && !sua && PermissionService.Can(ChucNangConst.GIAOVIEN_THEM);
            _btnSua.Enabled = !_chiXem && sua && PermissionService.Can(ChucNangConst.GIAOVIEN_SUA);
            _btnXoa.Enabled = sua && PermissionService.Can(ChucNangConst.GIAOVIEN_XOA);
        }

        private void Search_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
            NapDanhSach(_txtTim.Text);
            LamMoi();
        }

        private void BaoLoi(string text) => MessageBox.Show(text, "Giáo viên", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        private void frmGiaoVien_Load(object sender, EventArgs e)
        {

        }

        private void _grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
