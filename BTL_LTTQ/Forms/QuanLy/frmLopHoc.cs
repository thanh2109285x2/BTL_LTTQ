using BTL_LTTQ.DataAccessLayer;
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
        private string maLopDangChon = "";
        public frmLopHoc()
        {
            InitializeComponent();
            GiaoDien.ApDung(this);
        }

        private void NapComboBox(ComboBox cbo, string sql, string tenCotMa, string tenCotHienThi)
        {
            DataTable dt = DbHelper.ExecuteQuery(sql);
            cbo.DataSource = dt;
            cbo.DisplayMember = tenCotHienThi;
            cbo.ValueMember = tenCotMa;
            cbo.SelectedIndex = -1;
        }

        private void NaptatcaComboBox()
        {
            NapComboBox(cboMonHoc, "SELECT MaMon, TenMon From MonHoc", "MaMon", "TenMon");
            NapComboBox(cboGiaoVien, "SELECT MaGV, TenGV From GiaoVien", "MaGV", "TenGV");
            NapComboBox(cboCaHoc, "SELECT MaCa, TenCa FROM CaHoc", "MaCa", "TenCa");
            NapComboBox(cboThoiGian, "SELECT MaThoiGian, TenThoiGian FROM ThoiGian", "MaThoiGian", "TenThoiGian");
            NapComboBox(cboPhongHoc, "SELECT MaPhong, TenPhong FROM PhongHoc", "MaPhong", "TenPhong");

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

        private void NapDanhSachLop(string tuKhoaTimKiem = "")
        {
            string sql = @"SELECT l.MaLop, l.TenLop, 
                            l.MaMon, m.TenMon,
                            l.MaGV, g.TenGV,
                            l.NgayBD, l.NgayKT,
                            l.MaCa, c.TenCa,
                            l.MaThoiGian, t.TenThoiGian,
                            l.MaPhong, p.TenPhong
                            FROM LopHoc l
                        LEFT JOIN MonHoc m   ON l.MaMon = m.MaMon
                        LEFT JOIN GiaoVien g ON l.MaGV = g.MaGV
                        LEFT JOIN CaHoc c    ON l.MaCa = c.MaCa
                        LEFT JOIN ThoiGian t ON l.MaThoiGian = t.MaThoiGian
                        LEFT JOIN PhongHoc p ON l.MaPhong = p.MaPhong
                        WHERE @TuKhoa = '' OR l.TenLop COLLATE Vietnamese_CI_AI LIKE '%' + @TuKhoa + '%'
                        ORDER BY l.NgayBD DESC";

            dgvLopHoc.DataSource = DbHelper.ExecuteQuery(sql, new SqlParameter("@TuKhoa", tuKhoaTimKiem));
            DinhDangBang();
        }

        private void frmLopHoc_Load(object sender, EventArgs e)
        {
            NaptatcaComboBox();
            NapDanhSachLop();
            LamMoi();
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
            txtMaLop.Clear();

            maLopDangChon = "";
            txtMaLop.ReadOnly = false;
        }
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoi();
        }

        private bool KiemTraDuLieu()
        {
            string errorM = "";
            Control oLoiDauTien = null;

            if (string.IsNullOrWhiteSpace(txtMaLop.Text))
            {
                errorM += "Vui lòng nhập mã lớp\n";
                oLoiDauTien ??= txtMaLop;
            }
            if (string.IsNullOrWhiteSpace(txtTenLop.Text))
            {
                errorM += "Vui lòng nhập tên lớp\n";
                oLoiDauTien ??= txtTenLop;
            }
            if (cboMonHoc.SelectedIndex == -1 || cboGiaoVien.SelectedIndex == -1 ||
                cboCaHoc.SelectedIndex == -1 || cboThoiGian.SelectedIndex == -1 ||
                cboPhongHoc.SelectedIndex == -1)
            {
                errorM += "Vui lòng chọn đầy đủ môn học, giáo viên, ca học, thời gian và phòng học\n";
                oLoiDauTien ??= cboMonHoc;
            }
            if (dtpNgayKT.Value.Date < dtpNgayBD.Value.Date)
            {
                errorM += "Ngày kết thúc không được trước ngày bắt đầu\n";
                oLoiDauTien ??= dtpNgayKT;
            }

            if (errorM != "")
            {
                MessageBox.Show(errorM, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                oLoiDauTien?.Focus();
                return false;
            }
            return true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!KiemTraDuLieu())
            {
                return;
            }

            string maLop = txtMaLop.Text.Trim();

            DataTable dt = DbHelper.ExecuteQuery("select * from LopHoc where MaLop = @MaLop",
                                                new SqlParameter("@MaLop", maLop));
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("Mã lớp này đã tồn tại", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMaLop.Focus();
                return;
            }
            string sql = @"INSERT INTO LopHoc (MaLop, TenLop, MaMon, MaGV, NgayBD, NgayKT, MaCa, MaThoiGian, MaPhong)
                   VALUES (@MaLop, @TenLop, @MaMon, @MaGV, @NgayBD, @NgayKT, @MaCa, @MaThoiGian, @MaPhong)";
            try
            {
                DbHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MaLop", maLop),
                new SqlParameter("@TenLop", txtTenLop.Text.Trim()),
                new SqlParameter("@MaMon", cboMonHoc.SelectedValue),
                new SqlParameter("@MaGV", cboGiaoVien.SelectedValue),
                new SqlParameter("@NgayBD", dtpNgayBD.Value.Date),
                new SqlParameter("@NgayKT", dtpNgayKT.Value.Date),
                new SqlParameter("@MaCa", cboCaHoc.SelectedValue),
                new SqlParameter("@MaThoiGian", cboThoiGian.SelectedValue),
                new SqlParameter("@MaPhong", cboPhongHoc.SelectedValue));

                MessageBox.Show("Thêm lớp học thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                NapDanhSachLop();
                LamMoi();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Không thêm được: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (maLopDangChon == "")
            {
                MessageBox.Show("Vui lòng chọn một lớp trong danh sách để sửa.", "Chưa chọn lớp",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!KiemTraDuLieu()) return;

            string sql = @"UPDATE LopHoc
                           SET TenLop = @TenLop, MaMon = @MaMon, MaGV = @MaGV,
                               NgayBD = @NgayBD, NgayKT = @NgayKT,
                               MaCa = @MaCa, MaThoiGian = @MaThoiGian, MaPhong = @MaPhong
                           WHERE MaLop = @MaLop";
            try
            {
                DbHelper.ExecuteNonQuery(sql,
                    new SqlParameter("@MaLop", maLopDangChon),
                    new SqlParameter("@TenLop", txtTenLop.Text.Trim()),
                    new SqlParameter("@MaMon", cboMonHoc.SelectedValue),
                    new SqlParameter("@MaGV", cboGiaoVien.SelectedValue),
                    new SqlParameter("@NgayBD", dtpNgayBD.Value.Date),
                    new SqlParameter("@NgayKT", dtpNgayKT.Value.Date),
                    new SqlParameter("@MaCa", cboCaHoc.SelectedValue),
                    new SqlParameter("@MaThoiGian", cboThoiGian.SelectedValue),
                    new SqlParameter("@MaPhong", cboPhongHoc.SelectedValue));

                MessageBox.Show("Cập nhật lớp học thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                NapDanhSachLop();
                LamMoi();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Không sửa được: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (maLopDangChon == "")
            {
                MessageBox.Show("Vui lòng chọn một lớp trong danh sách để xóa.", "Chưa chọn lớp", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult hoi = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa lớp " + maLopDangChon + "?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (hoi != DialogResult.Yes) return;

            try
            {
                DbHelper.ExecuteNonQuery("DELETE FROM LopHoc WHERE MaLop = @MaLop",
                                         new SqlParameter("@MaLop", maLopDangChon));

                MessageBox.Show("Đã xóa lớp học.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                NapDanhSachLop();
                LamMoi();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                    MessageBox.Show("Lớp này đang có học viên (hoặc dữ liệu liên quan), không thể xóa", "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    MessageBox.Show("Không xóa được: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                NapDanhSachLop(txtTimKiem.Text.Trim());
                LamMoi();

                if (dgvLopHoc.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy lớp học nào phù hợp.", "Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
