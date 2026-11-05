using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BTL_LTTQ.Models;
using BTL_LTTQ.Services;

namespace BTL_LTTQ.Forms.TraCuu
{
    public partial class frmTimKiemGiaoVien : Form
    {
        private readonly GiaoVienTraCuuService _giaoVienService = new GiaoVienTraCuuService();
        private readonly LookupService _lookupService = new LookupService();

        public frmTimKiemGiaoVien()
        {
            InitializeComponent();
            GiaoDien.ApDung(this);
        }

        private void NapComboBox(ComboBox cbo, DataTable dt, string tenCotMa, string tenCotHienThi)
        {
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
            NapComboBox(cboTrinhDo, _lookupService.LayTrinhDo(), "MaTrinhDo", "TenTrinhDo");
            NapComboBox(cboChuyenMon, _lookupService.LayChuyenMon(), "MaChuyenMon", "TenChuyenMon");
            NapComboBox(cboMonDay, _lookupService.LayMonHoc(), "MaMon", "TenMon");
            TimKiem();
        }

        private void TimKiem()
        {
            var tieuChi = new TimKiemGiaoVien
            {
                TenGV = txtTenGV.Text,
                MaTrinhDo = LayGiaTriComboBox(cboTrinhDo),
                MaChuyenMon = LayGiaTriComboBox(cboChuyenMon),
                MaMon = LayGiaTriComboBox(cboMonDay)
            };

            dgvKetQua.DataSource = _giaoVienService.TimKiem(tieuChi);

            DinhDangBang();
        }

        private static string? LayGiaTriComboBox(ComboBox cbo)
        {
            return cbo.SelectedValue == null || cbo.SelectedValue == DBNull.Value
                ? null
                : cbo.SelectedValue.ToString();
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