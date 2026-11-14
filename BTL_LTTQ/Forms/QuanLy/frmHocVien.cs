using BTL_LTTQ.Repositories;
using BTL_LTTQ.DataAccessLayer;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BTL_LTTQ.Forms.QuanLy
{
    public partial class frmHocVien : Form
    {
        private readonly HocVienRepository _repo = new();
        private string maDangChon = "";

        public frmHocVien()
        {
            InitializeComponent();
            GiaoDien.ApDung(this);
            Load += frmHocVien_Load;
        }

        private void frmHocVien_Load(object? sender, EventArgs e)
        {
            try
            {
                NapCombo(cboLop, _repo.LayDanhSachLop(), "MaLop", "TenLop");
                NapCombo(cboNghe, _repo.LayDanhSachNghe(), "MaNghe", "TenNghe");
                NapCombo(cboXepLoai, _repo.LayDanhSachXepLoai(), "MaXepLoai", "TenLoai");
                NapDanhSach();
            }
            catch (SqlException ex)
            {
                ShowError($"Không kết nối được SQL Server '{DbHelper.DataSource}'. Kiểm tra SQL Server service/instance đang chạy, sửa Data Source trong App.config cho đúng instance, và xác nhận database QuanLyLopTinHocVaNgoaiNguNganHan đã được tạo.\n\nChi tiết: {ex.Message}");
            }
            catch (Exception ex) { ShowError("Không tải được dữ liệu: " + ex.Message); }
        }

        private static void NapCombo(ComboBox combo, DataTable data, string value, string display)
        {
            combo.DataSource = data; combo.ValueMember = value; combo.DisplayMember = display; combo.SelectedIndex = -1;
        }

        private void NapDanhSach(string keyword = "")
        {
            dgvHocVien.DataSource = _repo.LayDanhSach(keyword);
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
                cboGioiTinh.SelectedItem?.ToString(), txtDiaChi.Text.Trim(), txtDienThoai.Text.Trim(),
                cboLop.SelectedValue?.ToString(), cboNghe.SelectedValue?.ToString(), ngayNop,
                (double)nudDiem.Value, cboXepLoai.SelectedValue?.ToString());
        }

        private string ValidateInput()
        {
            var d = DuLieu();
            if (d.ma.Length == 0 || d.ten.Length == 0) return "Vui lòng nhập mã và tên học viên.";
            if (d.ngaySinh > DateTime.Today || d.ngayNop > DateTime.Today) return "Ngày không được ở tương lai.";
            return "";
        }

        private void btnThem_Click(object? sender, EventArgs e)
        {
            string error = ValidateInput(); if (error != "") { ShowError(error); return; }
            var d = DuLieu();
            try
            {
                if (_repo.TonTai(d.ma)) { ShowError("Mã học viên đã tồn tại."); return; }
                _repo.Them(d.ma, d.ten, d.ngaySinh, d.gioiTinh, d.diaChi, nullIfEmpty(cboNghe), d.dienThoai,
                    d.ngayNop, d.diem, nullIfEmpty(cboXepLoai), nullIfEmpty(cboLop));
                Saved("Thêm học viên thành công.");
            }
            catch (SqlException ex) { ShowError("Không thêm được: " + ex.Message); }
        }

        private void btnSua_Click(object? sender, EventArgs e)
        {
            if (maDangChon == "") { ShowError("Hãy chọn học viên trong danh sách để sửa."); return; }
            string error = ValidateInput(); if (error != "") { ShowError(error); return; }
            var d = DuLieu();
            try
            {
                _repo.Sua(maDangChon, d.ten, d.ngaySinh, d.gioiTinh, d.diaChi, nullIfEmpty(cboNghe), d.dienThoai,
                    d.ngayNop, d.diem, nullIfEmpty(cboXepLoai), nullIfEmpty(cboLop));
                Saved("Cập nhật học viên thành công.");
            }
            catch (SqlException ex) { ShowError("Không sửa được: " + ex.Message); }
        }

        private void btnXoa_Click(object? sender, EventArgs e)
        {
            if (maDangChon == "") { ShowError("Hãy chọn học viên trong danh sách để xóa."); return; }
            if (MessageBox.Show($"Xóa học viên {maDangChon}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            try { _repo.Xoa(maDangChon); Saved("Đã xóa học viên."); }
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
            txtDiaChi.Text = Convert.ToString(r.Cells["DiaChi"].Value) ?? "";
            txtDienThoai.Text = Convert.ToString(r.Cells["DienThoai"].Value) ?? "";
            SetDate(dtpNgaySinh, r.Cells["NgaySinh"].Value); SetDate(dtpNgayNopHocPhi, r.Cells["NgayNopHocPhi"].Value);
            cboGioiTinh.SelectedItem = Convert.ToString(r.Cells["GioiTinh"].Value);
            SetSelected(cboLop, r.Cells["MaLop"].Value); SetSelected(cboNghe, r.Cells["MaNghe"].Value); SetSelected(cboXepLoai, r.Cells["MaXepLoai"].Value);
            if (r.Cells["Diem"].Value != DBNull.Value) nudDiem.Value = Math.Clamp(Convert.ToDecimal(r.Cells["Diem"].Value), nudDiem.Minimum, nudDiem.Maximum);
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
        private void LamMoi(bool clearSearch = true)
        {
            txtMaHocVien.Clear(); txtMaHocVien.ReadOnly = false; txtTenHocVien.Clear(); txtDiaChi.Clear(); txtDienThoai.Clear();
            dtpNgaySinh.Value = dtpNgayNopHocPhi.Value = DateTime.Today; dtpNgaySinh.Checked = dtpNgayNopHocPhi.Checked = false;
            cboGioiTinh.SelectedIndex = cboLop.SelectedIndex = cboNghe.SelectedIndex = cboXepLoai.SelectedIndex = -1;
            nudDiem.Value = 0; if (clearSearch) txtTimKiem.Clear(); dgvHocVien.ClearSelection(); maDangChon = "";
        }
        private static void ShowError(string message) => MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}
