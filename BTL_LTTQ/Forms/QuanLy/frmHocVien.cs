using BTL_LTTQ.BUS;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace BTL_LTTQ.Forms.QuanLy
{
    public partial class frmHocVien : Form
    {
        private readonly HocVienService _hocVienService = new();
        private string maDangChon = "";

        public frmHocVien()
        {
            InitializeComponent();
            GiaoDien.ApDung(this);
            Load += frmHocVien_Load;
            nudDiem.ValueChanged += (_, _) => CapNhatXepLoaiTheoDiem();
            txtTenHocVien.KeyPress += ChiChoNhapChuVaKhoangTrang;
            txtDienThoai.KeyPress += ChiChoNhapSo;
            txtDienThoai.MaxLength = 10;
            txtMaHocVien.KeyPress += ChiChoNhapMa;
            txtMaHocVien.MaxLength = 20;
            txtTenHocVien.MaxLength = 100;
        }

        private void frmHocVien_Load(object? sender, EventArgs e)
        {
            try
            {
                NapCombo(cboLop, _repo.LayDanhSachLop(), "MaLop", "TenLop");
                NapCombo(cboNghe, _repo.LayDanhSachNghe(), "MaNghe", "TenNghe");
                NapCombo(cboXepLoai, _repo.LayDanhSachXepLoai(), "MaXepLoai", "TenLoai");
                NapDanhSachTinhThanh();
                cboXepLoai.Enabled = false;
                NapDanhSach();
            }
            catch (SqlException ex)
            {
                ShowError($"Không kết nối được SQL Server. Kiểm tra SQL Server service/instance đang chạy, sửa Data Source trong App.config cho đúng instance, và xác nhận database QuanLyLopTinHocVaNgoaiNguNganHan đã được tạo.\n\nChi tiết: {ex.Message}");
            }
            catch (Exception ex) { ShowError("Không tải được dữ liệu: " + ex.Message); }
        }

        private static void NapCombo(ComboBox combo, DataTable data, string value, string display)
        {
            combo.DataSource = data; combo.ValueMember = value; combo.DisplayMember = display; combo.SelectedIndex = -1;
        }

        private void NapDanhSachTinhThanh()
        {
            cboTinhThanh.Items.AddRange(new object[]
            {
                "An Giang", "Bắc Ninh", "Cà Mau", "Cao Bằng", "Cần Thơ", "Đà Nẵng",
                "Đắk Lắk", "Điện Biên", "Đồng Nai", "Đồng Tháp", "Gia Lai", "Hà Nội",
                "Hà Tĩnh", "Hải Phòng", "Huế", "Hưng Yên", "Khánh Hòa",
                "Lai Châu", "Lâm Đồng", "Lạng Sơn", "Lào Cai", "Nghệ An", "Ninh Bình",
                "Phú Thọ", "Quảng Ngãi", "Quảng Ninh", "Quảng Trị", "Sơn La", "Tây Ninh",
                "Thái Nguyên", "Thành phố Hồ Chí Minh", "Thanh Hóa", "Tuyên Quang", "Vĩnh Long"
            });
            cboTinhThanh.SelectedIndex = -1;
        }

        private void CapNhatXepLoaiTheoDiem()
        {
            if (cboXepLoai.DataSource is not DataTable danhSach) return;
            double diem = (double)nudDiem.Value;
            string tenLoai = diem >= 9 ? "Xuất sắc"
                : diem >= 8 ? "Giỏi"
                : diem >= 6.5 ? "Khá"
                : "Trung bình";
            var dong = danhSach.AsEnumerable()
                .FirstOrDefault(r => string.Equals(Convert.ToString(r["TenLoai"]), tenLoai, StringComparison.OrdinalIgnoreCase));
            if (dong == null) cboXepLoai.SelectedIndex = -1;
            else cboXepLoai.SelectedValue = dong["MaXepLoai"];
        }

        private void NapDanhSach(string keyword = "")
        {
            dgvHocVien.DataSource = _hocVienService.LayDanhSach(keyword);
            if (dgvHocVien.Columns.Count == 0) return;
            dgvHocVien.Columns["MaLop"].Visible = dgvHocVien.Columns["MaNghe"].Visible = dgvHocVien.Columns["MaXepLoai"].Visible = false;
            SetHeader("MaHocVien", "Mã học viên"); SetHeader("TenHocVien", "Tên học viên");
            SetHeader("NgaySinh", "Ngày sinh"); SetHeader("GioiTinh", "Giới tính"); SetHeader("DienThoai", "Điện thoại");
            SetHeader("DiaChi", "Địa chỉ"); SetHeader("TenLop", "Lớp học"); SetHeader("TenNghe", "Nghề nghiệp");
            SetHeader("NgayNopHocPhi", "Ngày nộp học phí"); SetHeader("Diem", "Điểm"); SetHeader("TenLoai", "Xếp loại");
            dgvHocVien.Columns["NgaySinh"].DefaultCellStyle.Format = "dd/MM/yyyy";
            dgvHocVien.Columns["NgayNopHocPhi"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        private void SetHeader(string column, string header) => dgvHocVien.Columns[column].HeaderText = header;

        private (string ma, string ten, DateTime? ngaySinh, string? gioiTinh, string diaChi,
            string? dienThoai, string? maLop, string? maNghe, DateTime? ngayNop, double? diem, string? xepLoai) DuLieu()
        {
            DateTime? ngaySinh = dtpNgaySinh.ShowCheckBox && !dtpNgaySinh.Checked ? null : dtpNgaySinh.Value.Date;
            DateTime? ngayNop = dtpNgayNopHocPhi.ShowCheckBox && !dtpNgayNopHocPhi.Checked ? null : dtpNgayNopHocPhi.Value.Date;
            return (txtMaHocVien.Text.Trim(), txtTenHocVien.Text.Trim(), ngaySinh,
                cboGioiTinh.SelectedItem?.ToString(), cboTinhThanh.SelectedItem?.ToString() ?? "", txtDienThoai.Text.Trim(),
                cboLop.SelectedValue?.ToString(), cboNghe.SelectedValue?.ToString(), ngayNop,
                (double)nudDiem.Value, cboXepLoai.SelectedValue?.ToString());
        }

        private bool ValidateInput(out string error, out Control control)
        {
            var d = DuLieu();
            if (d.ma.Length == 0) return LoiNhap("Vui lòng nhập mã học viên.", txtMaHocVien, out error, out control);
            if (d.ten.Length == 0) return LoiNhap("Vui lòng nhập tên học viên.", txtTenHocVien, out error, out control);
            if (!Regex.IsMatch(d.ma, @"^[A-Za-z0-9_-]+$")) return LoiNhap("Mã học viên chỉ được gồm chữ không dấu, chữ số, dấu gạch dưới hoặc gạch ngang.", txtMaHocVien, out error, out control);
            if (!Regex.IsMatch(d.ten, @"^\p{L}+(?:[ '\-]\p{L}+)*$")) return LoiNhap("Tên học viên chỉ được chứa chữ cái và khoảng trắng, dấu gạch ngang hoặc dấu nháy.", txtTenHocVien, out error, out control);
            if (d.dienThoai is { Length: > 0 } && !Regex.IsMatch(d.dienThoai, @"^[0-9]{10}$")) return LoiNhap("Số điện thoại phải gồm đúng 10 chữ số.", txtDienThoai, out error, out control);
            if (d.diaChi.Length == 0) return LoiNhap("Vui lòng chọn tỉnh/thành phố.", cboTinhThanh, out error, out control);
            if (!d.ngaySinh.HasValue) return LoiNhap("Vui lòng chọn ngày sinh học viên.", dtpNgaySinh, out error, out control);
            if (d.ngaySinh.Value.Date > DateTime.Today) return LoiNhap("Ngày sinh không được ở tương lai.", dtpNgaySinh, out error, out control);
            if (d.ngaySinh.Value.Date > DateTime.Today.AddYears(-18))
                return LoiNhap("Học viên phải từ đủ 18 tuổi trở lên.", dtpNgaySinh, out error, out control);
            if (d.ngayNop > DateTime.Today) return LoiNhap("Ngày nộp học phí không được ở tương lai.", dtpNgayNopHocPhi, out error, out control);
            error = "";
            control = this;
            return true;
        }

        private static bool LoiNhap(string message, Control control, out string error, out Control controlCanSua)
        {
            error = message;
            controlCanSua = control;
            return false;
        }

        private void BaoLoiNhap(string message, Control control)
        {
            MessageBox.Show(message + "\nVui lòng sửa thông tin rồi thử lại.", "Thông tin chưa hợp lệ",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            control.Focus();
            if (control is TextBox textBox) textBox.SelectAll();
        }

        private static void ChiChoNhapChuVaKhoangTrang(object? sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar) || char.IsLetter(e.KeyChar) || e.KeyChar is ' ' or '-' or '\'') return;
            e.Handled = true;
        }

        private static void ChiChoNhapSo(object? sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && (e.KeyChar < '0' || e.KeyChar > '9')) e.Handled = true;
        }

        private static void ChiChoNhapMa(object? sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar) || (e.KeyChar <= 127 && char.IsLetterOrDigit(e.KeyChar)) || e.KeyChar is '_' or '-') return;
            e.Handled = true;
        }

        private void btnThem_Click(object? sender, EventArgs e)
        {
            if (!ValidateInput(out string error, out Control controlCanSua)) { BaoLoiNhap(error, controlCanSua); return; }
            var d = DuLieu();
            try
            {
                if (_repo.TonTai(d.ma)) { BaoLoiNhap("Mã học viên đã tồn tại.", txtMaHocVien); return; }
                _repo.Them(d.ma, d.ten, d.ngaySinh, d.gioiTinh, d.diaChi, nullIfEmpty(cboNghe), d.dienThoai,
                    d.ngayNop, d.diem, nullIfEmpty(cboXepLoai), nullIfEmpty(cboLop));
                Saved("Thêm học viên thành công.");
            }
            catch (SqlException ex) { ShowError("Không thêm được: " + ex.Message); }
        }

        private void btnSua_Click(object? sender, EventArgs e)
        {
            if (maDangChon == "") { ShowError("Hãy chọn học viên trong danh sách để sửa."); return; }
            if (!ValidateInput(out string error, out Control controlCanSua)) { BaoLoiNhap(error, controlCanSua); return; }
            var d = DuLieu();
            try
            {
                _hocVienService.Sua(maDangChon, d.ten, d.ngaySinh, d.gioiTinh, d.diaChi, nullIfEmpty(cboNghe), d.dienThoai,
                    d.ngayNop, d.diem, nullIfEmpty(cboXepLoai), nullIfEmpty(cboLop));
                Saved("Cập nhật học viên thành công.");
            }
            catch (SqlException ex) { ShowError("Không sửa được: " + ex.Message); }
        }

        private void btnXoa_Click(object? sender, EventArgs e)
        {
            if (maDangChon == "") { ShowError("Hãy chọn học viên trong danh sách để xóa."); return; }
            if (MessageBox.Show($"Xóa học viên {maDangChon}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            try { _hocVienService.Xoa(maDangChon); Saved("Đã xóa học viên."); }
            catch (SqlException ex) { ShowError("Không xóa được: " + ex.Message); }
        }

        private string? nullIfEmpty(ComboBox combo) => combo.SelectedValue?.ToString();
        private void Saved(string message)
        {
            MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            NapDanhSach(); LamMoi();
        }
        private void btnLamMoi_Click(object? sender, EventArgs e) => LamMoi();
        private void btnTimKiem_Click(object? sender, EventArgs e) => Search();
        private void txtTimKiem_KeyDown(object? sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; Search(); } }
        private void Search() { try { NapDanhSach(txtTimKiem.Text.Trim()); LamMoi(false); } catch (SqlException ex) { ShowError(ex.Message); } }

        private void dgvHocVien_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var r = dgvHocVien.Rows[e.RowIndex];
            maDangChon = Convert.ToString(r.Cells["MaHocVien"].Value) ?? "";
            txtMaHocVien.Text = maDangChon; txtMaHocVien.ReadOnly = true;
            txtTenHocVien.Text = Convert.ToString(r.Cells["TenHocVien"].Value) ?? "";
            SetSelectedText(cboTinhThanh, r.Cells["DiaChi"].Value);
            txtDienThoai.Text = Convert.ToString(r.Cells["DienThoai"].Value) ?? "";
            SetDate(dtpNgaySinh, r.Cells["NgaySinh"].Value); SetDate(dtpNgayNopHocPhi, r.Cells["NgayNopHocPhi"].Value);
            cboGioiTinh.SelectedItem = Convert.ToString(r.Cells["GioiTinh"].Value);
            SetSelected(cboLop, r.Cells["MaLop"].Value); SetSelected(cboNghe, r.Cells["MaNghe"].Value);
            if (r.Cells["Diem"].Value != DBNull.Value) nudDiem.Value = Math.Clamp(Convert.ToDecimal(r.Cells["Diem"].Value), nudDiem.Minimum, nudDiem.Maximum);
            CapNhatXepLoaiTheoDiem();
        }

        private static void SetDate(DateTimePicker picker, object? value)
        {
            picker.ShowCheckBox = true; picker.Checked = value != null && value != DBNull.Value;
            if (picker.Checked) picker.Value = Convert.ToDateTime(value);
        }
        private static void SetSelected(ComboBox combo, object? value)
        {
            if (value == null || value == DBNull.Value) combo.SelectedIndex = -1;
            else combo.SelectedValue = value;
        }
        private static void SetSelectedText(ComboBox combo, object? value)
        {
            string text = value == null || value == DBNull.Value ? "" : Convert.ToString(value) ?? "";
            combo.SelectedIndex = combo.FindStringExact(text);
        }
        private void LamMoi(bool clearSearch = true)
        {
            txtMaHocVien.Clear(); txtMaHocVien.ReadOnly = false; txtTenHocVien.Clear(); cboTinhThanh.SelectedIndex = -1; txtDienThoai.Clear();
            dtpNgaySinh.Value = dtpNgayNopHocPhi.Value = DateTime.Today; dtpNgaySinh.Checked = dtpNgayNopHocPhi.Checked = false;
            cboGioiTinh.SelectedIndex = cboLop.SelectedIndex = cboNghe.SelectedIndex = cboXepLoai.SelectedIndex = -1;
            nudDiem.Value = 0; if (clearSearch) txtTimKiem.Clear(); dgvHocVien.ClearSelection(); maDangChon = "";
        }
        private static void ShowError(string message) => MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}
