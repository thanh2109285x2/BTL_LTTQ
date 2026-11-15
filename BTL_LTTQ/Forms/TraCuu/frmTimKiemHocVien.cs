using BTL_LTTQ.Common;
using BTL_LTTQ.BUS;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BTL_LTTQ.Forms.TraCuu
{
    public partial class frmTimKiemHocVien : Form
    {
        private readonly HocVienService _hocVienService = new();

        public frmTimKiemHocVien()
        {
            InitializeComponent();
            GiaoDien.ApDung(this);
            Load += frmTimKiemHocVien_Load;
        }

        private void frmTimKiemHocVien_Load(object? sender, EventArgs e)
        {
            if (!PermissionService.Can(ChucNangConst.TIM_HOCVIEN))
            {
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này.", "Không có quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            try
            {
                NapCombo(cboLop, _hocVienService.LayDanhSachLop(), "MaLop", "TenLop");
                NapCombo(cboNghe, _hocVienService.LayDanhSachNghe(), "MaNghe", "TenNghe");
                TimKiem();
            }
            catch (SqlException ex) { ShowError("Không tra cứu được học viên: " + ex.Message); }
        }

        private static void NapCombo(ComboBox combo, DataTable data, string value, string display)
        {
            var all = data.NewRow(); all[value] = DBNull.Value; all[display] = "-- Tất cả --"; data.Rows.InsertAt(all, 0);
            combo.DataSource = data; combo.ValueMember = value; combo.DisplayMember = display; combo.SelectedIndex = 0;
        }

        private static string? GiaTri(ComboBox combo) =>
            combo.SelectedValue == null || combo.SelectedValue == DBNull.Value ? null : combo.SelectedValue.ToString();

        private void TimKiem()
        {
            dgvKetQua.DataSource = _hocVienService.TimKiem(txtTuKhoa.Text, GiaTri(cboLop), GiaTri(cboNghe), null);
            DinhDangBang();
        }

        private void DinhDangBang()
        {
            if (dgvKetQua.Columns.Count == 0) return;
            dgvKetQua.Columns["MaHocVien"].HeaderText = "Mã học viên";
            dgvKetQua.Columns["TenHocVien"].HeaderText = "Tên học viên";
            dgvKetQua.Columns["NgaySinh"].HeaderText = "Ngày sinh";
            dgvKetQua.Columns["GioiTinh"].HeaderText = "Giới tính";
            dgvKetQua.Columns["DienThoai"].HeaderText = "Điện thoại";
            dgvKetQua.Columns["DiaChi"].HeaderText = "Địa chỉ";
            dgvKetQua.Columns["TenNghe"].HeaderText = "Nghề nghiệp";
            dgvKetQua.Columns["NgaySinh"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        private void btnTimKiem_Click(object? sender, EventArgs e)
        {
            try
            {
                TimKiem();
                if (dgvKetQua.Rows.Count == 0) MessageBox.Show("Không tìm thấy học viên phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException ex) { ShowError("Tra cứu thất bại: " + ex.Message); }
        }

        private void btnLamMoi_Click(object? sender, EventArgs e)
        {
            txtTuKhoa.Clear(); cboLop.SelectedIndex = cboNghe.SelectedIndex = 0;
            btnTimKiem_Click(sender, e);
        }

        private void txtTuKhoa_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true; btnTimKiem_Click(sender, e);
        }

        private static void ShowError(string text) => MessageBox.Show(text, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}
