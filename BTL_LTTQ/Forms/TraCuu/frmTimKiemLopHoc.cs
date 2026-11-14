
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BTL_LTTQ.Common;
using BTL_LTTQ.Models;
using BTL_LTTQ.Services;

namespace BTL_LTTQ.Forms.TraCuu
{
    public partial class frmTimKiemLopHoc : Form
    {
        private readonly LopHocTraCuuService _lopHocService = new LopHocTraCuuService();
        private readonly LookupService _lookupService = new LookupService();

        public frmTimKiemLopHoc()
        {
            InitializeComponent();
            GiaoDien.ApDung(this);
        }

        private void NapComboBox(ComboBox cbo, DataTable dt, string tenCotMa, string tenCotHienThi)
        {
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
            NapComboBox(cboMonHoc, _lookupService.LayMonHoc(), "MaMon", "TenMon");
            NapComboBox(cboGiaoVien, _lookupService.LayGiaoVien(), "MaGV", "TenGV");

            if (PermissionService.ChiCuaMinh(ChucNangConst.TIM_LOP) && CurrentUser.IsGiaoVien)
            {
                if (!string.IsNullOrWhiteSpace(CurrentUser.MaGV))
                    cboGiaoVien.SelectedValue = CurrentUser.MaGV;
                cboGiaoVien.Enabled = false;
            }

            TimKiem();
        }

        private string TimKiem()
        {
            var tieuChi = new TimKiemLopHoc
            {
                MaMon = LayGiaTriComboBox(cboMonHoc),
                MaGV = LayGiaTriComboBox(cboGiaoVien),
                SiSoTu = (int)nudSiSoTu.Value,
                SiSoDen = (int)nudSiSoDen.Value
            };

            string loi = _lopHocService.TimKiem(tieuChi, out DataTable ketQua);
            if (loi != "") return loi;

            dgvKetQua.DataSource = ketQua;
            DinhDangBang();
            return "";
        }

        private static string? LayGiaTriComboBox(ComboBox cbo)
        {
            return cbo.SelectedValue == null || cbo.SelectedValue == DBNull.Value
                ? null
                : cbo.SelectedValue.ToString();
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
            string loi = TimKiem();
            if (loi != "")
            {
                MessageBox.Show(loi, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nudSiSoTu.Focus();
                return;
            }

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