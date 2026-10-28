
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BTL_LTTQ.DataAccessLayer;
using Microsoft.Data.SqlClient;
using System;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.TraCuu
{
    public partial class frmTimKiemLopHoc : Form
    {
        public frmTimKiemLopHoc()
        {
            InitializeComponent();
            GiaoDien.ApDung(this);
        }

        private void NapComboBox(ComboBox cbo, string sql, string tenCotMa, string tenCotHienThi)
        {
            DataTable dt = DbHelper.ExecuteQuery(sql);

            DataRow dong = dt.NewRow();
            dong[tenCotMa] = DBNull.Value;
            dong[tenCotHienThi] = "--Bỏ chọn--";
            dt.Rows.InsertAt(dong, 0);

            cbo.DataSource = dt;
            cbo.DisplayMember = tenCotHienThi;
            cbo.ValueMember = tenCotMa;
            cbo.SelectedIndex = 0; // neu la tru 1 hoac 0 thi khong loc theo tieu chi do
        }

        private void frmTimKiemLopHoc_Load(object sender, EventArgs e)
        {
            NapComboBox(cboMonHoc, "SELECT MaMon, TenMon FROM MonHoc", "MaMon", "TenMon");
            NapComboBox(cboGiaoVien, "SELECT MaGV, TenGV FROM GiaoVien", "MaGV", "TenGV");
            TimKiem();
        }

        private void TimKiem()
        {
            string sql = @"SELECT * FROM(
                            SELECT l.MaLop, l.TenLop, m.TenMon, g.TenGV, l.NgayBD, l.NgayKT,
                                   (SELECT COUNT(*) FROM HocVien d WHERE d.MaLop = l.MaLop) AS SiSo
                            FROM LopHoc l
                            LEFT JOIN MonHoc m ON l.MaMon = m.MaMon
                            LEFT JOIN GiaoVien g ON l.MaGV = g.MaGV
                            WHERE (@MaMon IS NULL OR l.MaMon = @MaMon) AND (@MaGV IS NULL OR l.MaGV = @MaGV)) x
                            WHERE x.SiSo BETWEEN @SiSoTu AND @SiSoDen
                            ORDER BY x.NgayBD DESC";

            dgvKetQua.DataSource = DbHelper.ExecuteQuery(sql,
                new SqlParameter("@MaMon", cboMonHoc.SelectedValue ?? DBNull.Value),
                new SqlParameter("@MaGV", cboGiaoVien.SelectedValue ?? DBNull.Value),
                new SqlParameter("@SiSoTu", (int)nudSiSoTu.Value),
                new SqlParameter("@SiSoDen", (int)nudSiSoDen.Value));
            DinhDangBang();
        }

        private void DinhDangBang()
        {
            dgvKetQua.Columns["MaLop"].HeaderText = "Mã lớp";
            dgvKetQua.Columns["TenLop"].HeaderText = "Tên lớp";
            dgvKetQua.Columns["TenMon"].HeaderText = "Môn học";
            dgvKetQua.Columns["TenGV"].HeaderText = "Giáo viên";
            dgvKetQua.Columns["NgayBD"].HeaderText = "Ngày bắt đầu";
            dgvKetQua.Columns["NgayKT"].HeaderText = "Ngày kết thúc";
            dgvKetQua.Columns["SiSo"].HeaderText = "Sĩ số";
            dgvKetQua.Columns["NgayBD"].DefaultCellStyle.Format = "dd/MM/yyyy";
            dgvKetQua.Columns["NgayKT"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (nudSiSoTu.Value > nudSiSoDen.Value)
            {
                MessageBox.Show("Sĩ số 'từ' không được lớn hơn sĩ số 'đến'.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nudSiSoTu.Focus();
                return;
            }

            TimKiem();

            if (dgvKetQua.Rows.Count == 0)
                MessageBox.Show("Không tìm thấy lớp học nào phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            cboMonHoc.SelectedIndex = 0;
            cboGiaoVien.SelectedIndex = 0;
            nudSiSoTu.Value = 0;
            nudSiSoDen.Value = 1000;
            TimKiem();
        }
    }
}