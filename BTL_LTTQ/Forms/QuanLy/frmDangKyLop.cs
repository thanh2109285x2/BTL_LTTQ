using BTL_LTTQ.Common;
using BTL_LTTQ.Repositories;
using System;
using System.Data;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.QuanLy
{
    //Học viên tự đăng ký / hủy đăng ký nhiều lớp, có giới hạn sĩ số
    public partial class frmDangKyLop : Form
    {
        private readonly DangKyRepository _repo = new DangKyRepository();

        private string maLopDangChon = "";
        private bool _choPhepDangKy;

        public frmDangKyLop()
        {
            InitializeComponent();
            GiaoDien.ApDung(this);
        }

        private void frmDangKyLop_Load(object sender, EventArgs e)
        {
            _choPhepDangKy = PermissionService.Can(ChucNangConst.DANGKYLOP);

            if (!CurrentUser.IsHocVien || string.IsNullOrWhiteSpace(CurrentUser.MaHocVien))
            {
                MessageBox.Show("Chức năng đăng ký lớp chỉ dành cho học viên.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnThem.Enabled = false;
                btnXoa.Enabled = false;
                return;
            }

            NapDanhSachLop();
        }

        private void NapDanhSachLop(string tuKhoa = "")
        {
            DataTable dt = _repo.LayLopChoHocVien(CurrentUser.MaHocVien!, tuKhoa);

            if (!dt.Columns.Contains("SiSoText"))
                dt.Columns.Add("SiSoText", typeof(string));
            if (!dt.Columns.Contains("TrangThai"))
                dt.Columns.Add("TrangThai", typeof(string));

            foreach (DataRow dong in dt.Rows)
            {
                int siSo = Convert.ToInt32(dong["SiSo"]);
                int toiDa = Convert.ToInt32(dong["SiSoToiDa"]);
                bool daDangKy = Convert.ToInt32(dong["DaDangKy"]) == 1;

                dong["SiSoText"] = $"{siSo}/{toiDa}";
                dong["TrangThai"] = daDangKy ? "Đã đăng ký" : (siSo >= toiDa ? "Đã đầy" : "Còn chỗ");
            }

            dgvLopHoc.DataSource = dt;
            DinhDangBang();
        }

        private void DinhDangBang()
        {
            if (dgvLopHoc.Columns.Count == 0) return;

            dgvLopHoc.Columns["SiSo"].Visible = false;
            dgvLopHoc.Columns["SiSoToiDa"].Visible = false;
            dgvLopHoc.Columns["DaDangKy"].Visible = false;

            dgvLopHoc.Columns["MaLop"].HeaderText = "Mã lớp";
            dgvLopHoc.Columns["TenLop"].HeaderText = "Tên lớp";
            dgvLopHoc.Columns["TenMon"].HeaderText = "Môn học";
            dgvLopHoc.Columns["TenGV"].HeaderText = "Giáo viên";
            dgvLopHoc.Columns["NgayBD"].HeaderText = "Ngày bắt đầu";
            dgvLopHoc.Columns["NgayKT"].HeaderText = "Ngày kết thúc";
            dgvLopHoc.Columns["SiSoText"].HeaderText = "Sĩ số";
            dgvLopHoc.Columns["TrangThai"].HeaderText = "Trạng thái";

            dgvLopHoc.Columns["NgayBD"].DefaultCellStyle.Format = "dd/MM/yyyy";
            dgvLopHoc.Columns["NgayKT"].DefaultCellStyle.Format = "dd/MM/yyyy";

            dgvLopHoc.Columns["TrangThai"].DisplayIndex = dgvLopHoc.Columns.Count - 1;
            dgvLopHoc.Columns["SiSoText"].DisplayIndex = dgvLopHoc.Columns.Count - 2;

            dgvLopHoc.ClearSelection();
        }

        private void dgvLopHoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvLopHoc.Rows[e.RowIndex];
            maLopDangChon = row.Cells["MaLop"].Value?.ToString() ?? "";
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!_choPhepDangKy)
            {
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này.", "Không có quyền",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (maLopDangChon == "")
            {
                MessageBox.Show("Vui lòng chọn một lớp trong danh sách để đăng ký.", "Chưa chọn lớp",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string loi = _repo.Them(CurrentUser.MaHocVien!, maLopDangChon);
            if (loi != "")
            {
                MessageBox.Show(loi, "Không đăng ký được", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                NapDanhSachLop(txtTimKiem.Text.Trim());
                return;
            }

            MessageBox.Show("Đăng ký lớp " + maLopDangChon + " thành công.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            NapDanhSachLop(txtTimKiem.Text.Trim());
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (!_choPhepDangKy)
            {
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này.", "Không có quyền",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (maLopDangChon == "")
            {
                MessageBox.Show("Vui lòng chọn một lớp trong danh sách để hủy đăng ký.", "Chưa chọn lớp",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult hoi = MessageBox.Show(
                "Bạn có chắc chắn muốn hủy đăng ký lớp " + maLopDangChon + "?",
                "Xác nhận hủy", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (hoi != DialogResult.Yes) return;

            string loi = _repo.Huy(CurrentUser.MaHocVien!, maLopDangChon);
            if (loi != "")
            {
                MessageBox.Show(loi, "Không hủy được", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Đã hủy đăng ký lớp " + maLopDangChon + ".", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            NapDanhSachLop(txtTimKiem.Text.Trim());
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            maLopDangChon = "";
            NapDanhSachLop();
        }

        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            e.SuppressKeyPress = true;
            maLopDangChon = "";
            NapDanhSachLop(txtTimKiem.Text.Trim());
        }
    }
}
