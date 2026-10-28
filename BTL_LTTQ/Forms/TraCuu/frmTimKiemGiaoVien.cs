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
    public partial class frmTimKiemGiaoVien : Form
    {
        public frmTimKiemGiaoVien()
        {
            InitializeComponent();
            GiaoDien.ApDung(this);
        }

        private void NapComboBox(ComboBox cbo, string sql, string tenCotMa, string tenCotHienThi)
        {
            DataTable dt = DbHelper.ExecuteQuery(sql);

            DataRow dong = dt.NewRow();
            dong[tenCotMa] = DBNull.Value;
            dong[tenCotHienThi] = "-- Bỏ chọn --";
            dt.Rows.InsertAt(dong, 0);

            cbo.DataSource = dt;
            cbo.DisplayMember = tenCotHienThi;
            cbo.ValueMember = tenCotMa;
            cbo.SelectedIndex = 0;
        }

        private void frmTimKiemGiaoVien_Load(object sender, EventArgs e)
        {
            NapComboBox(cboTrinhDo, "SELECT MaTrinhDo, TenTrinhDo FROM TrinhDo", "MaTrinhDo", "TenTrinhDo");
            NapComboBox(cboChuyenMon, "SELECT MaChuyenMon, TenChuyenMon FROM ChuyenMon", "MaChuyenMon", "TenChuyenMon");
            NapComboBox(cboMonDay, "SELECT MaMon, TenMon FROM MonHoc", "MaMon", "TenMon");
            TimKiem();
        }

        private void TimKiem()
        {
            string sql = @"SELECT g.MaGV, g.TenGV, g.DienThoai,t.TenTrinhDo, c.TenChuyenMon,
                            (SELECT STRING_AGG(m.TenMon, ', ')
                            FROM GiaoVien_MonHoc gm
                            JOIN MonHoc m ON gm.MaMon = m.MaMon
                            WHERE gm.MaGV = g.MaGV) AS MonDay
                            FROM GiaoVien g
                            LEFT JOIN TrinhDo t   ON g.MaTrinhDo = t.MaTrinhDo
                            LEFT JOIN ChuyenMon c ON g.MaChuyenMon = c.MaChuyenMon
                            WHERE (@TenGV = '' OR g.TenGV COLLATE Vietnamese_CI_AI LIKE '%' + @TenGV + '%')
                                AND (@MaTrinhDo   IS NULL OR g.MaTrinhDo   = @MaTrinhDo)
                                AND (@MaChuyenMon IS NULL OR g.MaChuyenMon = @MaChuyenMon)
                                AND (@MaMon IS NULL OR EXISTS(
                                    SELECT 1 FROM GiaoVien_MonHoc x
                                    WHERE x.MaGV = g.MaGV AND x.MaMon = @MaMon))
                            ORDER BY g.TenGV";

            dgvKetQua.DataSource = DbHelper.ExecuteQuery(sql,
                new SqlParameter("@TenGV", txtTenGV.Text.Trim()),
                new SqlParameter("@MaTrinhDo", cboTrinhDo.SelectedValue ?? DBNull.Value),
                new SqlParameter("@MaChuyenMon", cboChuyenMon.SelectedValue ?? DBNull.Value),
                new SqlParameter("@MaMon", cboMonDay.SelectedValue ?? DBNull.Value));

            DinhDangBang();
        }

        private void DinhDangBang()
        {
            dgvKetQua.Columns["MaGV"].HeaderText = "Mã GV";
            dgvKetQua.Columns["TenGV"].HeaderText = "Tên giáo viên";
            dgvKetQua.Columns["DienThoai"].HeaderText = "Điện thoại";
            dgvKetQua.Columns["TenTrinhDo"].HeaderText = "Trình độ";
            dgvKetQua.Columns["TenChuyenMon"].HeaderText = "Chuyên môn";
            dgvKetQua.Columns["MonDay"].HeaderText = "Môn dạy";
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            TimKiem();

            if (dgvKetQua.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy giáo viên nào phù hợp.", "Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTenGV.Clear();
            cboTrinhDo.SelectedIndex = 0;
            cboChuyenMon.SelectedIndex = 0;
            cboMonDay.SelectedIndex = 0;
            TimKiem();
            txtTenGV.Focus();
        }

        private void txtTenGV_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnTimKiem_Click(sender, e);
            }
        }
    }
}