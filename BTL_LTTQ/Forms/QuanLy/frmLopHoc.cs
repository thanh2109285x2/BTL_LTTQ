using BTL_LTTQ.Common;
using BTL_LTTQ.DataAccessLayer;
using BTL_LTTQ.Models;
using BTL_LTTQ.Repositories;
using BTL_LTTQ.Services;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.QuanLy
{
    public partial class frmLopHoc : Form
    {
        private readonly LopHocService _lopHocService = new LopHocService();
        private readonly LookupRepository _lookupRepo = new LookupRepository();

        private string maLopDangChon = "";

        public frmLopHoc()
        {
            InitializeComponent();
            GiaoDien.ApDung(this);
        }

        private void NapComboBox(ComboBox cbo, DataTable dt, string tenCotMa, string tenCotHienThi)
        {
            cbo.DataSource = dt;
            cbo.DisplayMember = tenCotHienThi;
            cbo.ValueMember = tenCotMa;
            cbo.SelectedIndex = -1;
        }

        private void NaptatcaComboBox()
        {
            NapComboBox(cboMonHoc, _lookupRepo.LayMonHoc(), "MaMon", "TenMon");
            NapComboBox(cboGiaoVien, _lookupRepo.LayGiaoVien(), "MaGV", "TenGV");
            NapComboBox(cboCaHoc, _lookupRepo.LayCaHoc(), "MaCa", "TenCa");
            NapComboBox(cboThoiGian, _lookupRepo.LayThoiGian(), "MaThoiGian", "TenThoiGian");
            NapComboBox(cboPhongHoc, _lookupRepo.LayPhongHoc(), "MaPhong", "TenPhong");
        }

        private void NapDanhSachLop(string tuKhoa = "")
        {
            dgvLopHoc.DataSource = _lopHocService.LayDanhSach(tuKhoa);
            DinhDangBang();
        }

        private void DinhDangBang()
        {
            dgvLopHoc.Columns["MaMon"].Visible = false;
            dgvLopHoc.Columns["MaGV"].Visible = false;
            dgvLopHoc.Columns["MaCa"].Visible = false;
            dgvLopHoc.Columns["MaThoiGian"].Visible = false;
            dgvLopHoc.Columns["MaPhong"].Visible = false;

            dgvLopHoc.Columns["MaLop"].HeaderText = "Mã lớp";
            dgvLopHoc.Columns["TenLop"].HeaderText = "Tên lớp";
            dgvLopHoc.Columns["TenMon"].HeaderText = "Môn học";
            dgvLopHoc.Columns["TenGV"].HeaderText = "Giáo viên";
            dgvLopHoc.Columns["NgayBD"].HeaderText = "Ngày bắt đầu";
            dgvLopHoc.Columns["NgayKT"].HeaderText = "Ngày kết thúc";
            dgvLopHoc.Columns["TenCa"].HeaderText = "Ca học";
            dgvLopHoc.Columns["TenThoiGian"].HeaderText = "Thời gian";
            dgvLopHoc.Columns["TenPhong"].HeaderText = "Phòng học";

            dgvLopHoc.Columns["NgayBD"].DefaultCellStyle.Format = "dd/MM/yyyy";
            dgvLopHoc.Columns["NgayKT"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        private LopHoc LayDuLieuTuForm()
        {
            return new LopHoc
            {
                MaLop = txtMaLop.Text.Trim(),
                TenLop = txtTenLop.Text.Trim(),
                MaMon = cboMonHoc.SelectedValue?.ToString(),
                MaGV = cboGiaoVien.SelectedValue?.ToString(),
                NgayBD = dtpNgayBD.Value.Date,
                NgayKT = dtpNgayKT.Value.Date,
                MaCa = cboCaHoc.SelectedValue?.ToString(),
                MaThoiGian = cboThoiGian.SelectedValue?.ToString(),
                MaPhong = cboPhongHoc.SelectedValue?.ToString()
            };
        }

        private void LamMoi()
        {
            txtMaLop.Clear();
            txtTenLop.Clear();

            cboMonHoc.SelectedIndex = -1;
            cboGiaoVien.SelectedIndex = -1;
            cboCaHoc.SelectedIndex = -1;
            cboThoiGian.SelectedIndex = -1;
            cboPhongHoc.SelectedIndex = -1;

            dtpNgayBD.Value = DateTime.Today;
            dtpNgayKT.Value = DateTime.Today;

            dgvLopHoc.ClearSelection();

            maLopDangChon = "";
            txtMaLop.ReadOnly = false;
        }

        private void frmLopHoc_Load(object sender, EventArgs e)
        {
            NaptatcaComboBox();
            NapDanhSachLop();
            LamMoi();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoi();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string loi = _lopHocService.Them(LayDuLieuTuForm());
            if (loi != "")
            {
                MessageBox.Show(loi, "Không thêm được", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Thêm lớp học thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            NapDanhSachLop();
            LamMoi();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (maLopDangChon == "")
            {
                MessageBox.Show("Vui lòng chọn một lớp trong danh sách để sửa.", "Chưa chọn lớp",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LopHoc lop = LayDuLieuTuForm();
            lop.MaLop = maLopDangChon;

            string loi = _lopHocService.Sua(lop);
            if (loi != "")
            {
                MessageBox.Show(loi, "Không sửa được", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Cập nhật lớp học thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            NapDanhSachLop();
            LamMoi();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (maLopDangChon == "")
            {
                MessageBox.Show("Vui lòng chọn một lớp trong danh sách để xóa.", "Chưa chọn lớp",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult hoi = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa lớp " + maLopDangChon + "?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (hoi != DialogResult.Yes) return;

            string loi = _lopHocService.Xoa(maLopDangChon);
            if (loi != "")
            {
                MessageBox.Show(loi, "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Đã xóa lớp học.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            NapDanhSachLop();
            LamMoi();
        }

        private void dgvLopHoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvLopHoc.Rows[e.RowIndex];

            maLopDangChon = row.Cells["MaLop"].Value.ToString();

            txtMaLop.Text = maLopDangChon;
            txtMaLop.ReadOnly = true;
            txtTenLop.Text = row.Cells["TenLop"].Value.ToString();

            cboMonHoc.SelectedValue = row.Cells["MaMon"].Value;
            cboGiaoVien.SelectedValue = row.Cells["MaGV"].Value;
            cboCaHoc.SelectedValue = row.Cells["MaCa"].Value;
            cboThoiGian.SelectedValue = row.Cells["MaThoiGian"].Value;
            cboPhongHoc.SelectedValue = row.Cells["MaPhong"].Value;

            dtpNgayBD.Value = Convert.ToDateTime(row.Cells["NgayBD"].Value);
            dtpNgayKT.Value = Convert.ToDateTime(row.Cells["NgayKT"].Value);
        }

        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            e.SuppressKeyPress = true;
            NapDanhSachLop(txtTimKiem.Text.Trim());
            LamMoi();

            if (dgvLopHoc.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy lớp học nào phù hợp.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private int chieuCaopanelNhapLieuBanDau = 340;
        private int chieuCaoNutThuPhong = 40;

        private void btnThu_Click(object sender, EventArgs e)
        {
            panelNhapLieu.SuspendLayout();

            bool dangMo = panelNoiDung.Visible;
            panelNoiDung.Visible = !dangMo;
            panelNhapLieu.Height = dangMo ? chieuCaoNutThuPhong : chieuCaopanelNhapLieuBanDau;
            btnThu.Text = panelNoiDung.Visible ? "▲" : "▼";

            panelTieuDe.BringToFront();

            panelNhapLieu.ResumeLayout(true);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
        private void button5_Click(object sender, EventArgs e)
        {

        }
        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        private void label11_Click(object sender, EventArgs e)
        {

        }
    }
}