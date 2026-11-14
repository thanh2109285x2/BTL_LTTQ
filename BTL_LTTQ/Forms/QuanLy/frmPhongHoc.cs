using BTL_LTTQ.Common;
using BTL_LTTQ.Models;
using BTL_LTTQ.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.QuanLy
{
    //Quan ly phong hoc: them / sua / xoa / tim kiem
    public partial class frmPhongHoc : Form
    {
        private readonly PhongHocService _service = new PhongHocService();

        private string maPhongDangChon = "";
        private bool _chiXem;

        public frmPhongHoc()
        {
            InitializeComponent();
            GiaoDien.ApDung(this);
        }

        private void NapDanhSach(string tuKhoa = "")
        {
            dgvPhongHoc.DataSource = _service.LayDanhSach(tuKhoa);
            DinhDangBang();
        }

        private void DinhDangBang()
        {
            if (dgvPhongHoc.Columns["MaPhong"] is DataGridViewColumn cotMaPhong)
                cotMaPhong.HeaderText = "Mã phòng";

            if (dgvPhongHoc.Columns["TenPhong"] is DataGridViewColumn cotTenPhong)
                cotTenPhong.HeaderText = "Tên phòng";

            if (dgvPhongHoc.Columns["SucChua"] is DataGridViewColumn cotSucChua)
            {
                cotSucChua.HeaderText = "Sức chứa";
                cotSucChua.DefaultCellStyle.Format = "0";
                cotSucChua.DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleLeft;
            }
        }

        private bool LayDuLieuTuForm(out PhongHoc phongHoc, out string loi)
        {
            phongHoc = new PhongHoc
            {
                MaPhong = txtMaPhong.Text.Trim(),
                TenPhong = txtTenPhong.Text.Trim()
            };

            loi = "";

            string sucChua = txtSucChua.Text.Trim();
            if (sucChua != "")
            {
                if (!int.TryParse(sucChua, NumberStyles.Integer, CultureInfo.InvariantCulture, out int soLuong))
                {
                    loi = "Sức chứa không hợp lệ. Vui lòng nhập số nguyên.";
                    return false;
                }

                phongHoc.SucChua = soLuong;
            }

            if (phongHoc.MaPhong == "") { loi = "Vui lòng nhập mã phòng"; return false; }
            if (phongHoc.TenPhong == "") { loi = "Vui lòng nhập tên phòng"; return false; }

            return true;
        }

        private void LamMoi()
        {
            txtMaPhong.Clear();
            txtTenPhong.Clear();
            txtSucChua.Clear();

            dgvPhongHoc.ClearSelection();

            maPhongDangChon = "";
            txtMaPhong.ReadOnly = _chiXem;

            txtMaPhong.Focus();
        }

        private void frmPhongHoc_Load(object sender, EventArgs e)
        {
            bool coQuyenThem = PermissionService.Can(ChucNangConst.DANHMUC_KHAC);
            bool coQuyenSua = PermissionService.Can(ChucNangConst.DANHMUC_KHAC);
            _chiXem = !coQuyenThem && !coQuyenSua;
            PhanQuyenUi.AnNutThemSuaXoa(
                btnThem, btnSua, btnXoa,
                ChucNangConst.DANHMUC_KHAC, ChucNangConst.DANHMUC_KHAC, ChucNangConst.DANHMUC_KHAC);

            NapDanhSach();
            LamMoi();
        }

        private void ApDungCheDoChiXem()
        {
            txtMaPhong.ReadOnly = _chiXem;
            txtTenPhong.ReadOnly = _chiXem;
            txtSucChua.ReadOnly = _chiXem;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!PermissionService.Can(ChucNangConst.DANHMUC_KHAC))
            {
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này.", "Không có quyền",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!LayDuLieuTuForm(out PhongHoc phongHoc, out string loiNhap))
            {
                MessageBox.Show(loiNhap, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string loi = _service.Them(phongHoc);
            if (loi != "")
            {
                MessageBox.Show(loi, "Không thêm được", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Thêm phòng học thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            NapDanhSach();
            LamMoi();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!PermissionService.Can(ChucNangConst.DANHMUC_KHAC))
            {
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này.", "Không có quyền",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (maPhongDangChon == "")
            {
                MessageBox.Show("Vui lòng chọn một phòng học trong danh sách để sửa.", "Chưa chọn phòng học",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!LayDuLieuTuForm(out PhongHoc phongHoc, out string loiNhap))
            {
                MessageBox.Show(loiNhap, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            phongHoc.MaPhong = maPhongDangChon;

            string loi = _service.Sua(phongHoc);
            if (loi != "")
            {
                MessageBox.Show(loi, "Không sửa được", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Cập nhật phòng học thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            NapDanhSach();
            LamMoi();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (!PermissionService.Can(ChucNangConst.DANHMUC_KHAC))
            {
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này.", "Không có quyền",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (maPhongDangChon == "")
            {
                MessageBox.Show("Vui lòng chọn một phòng học trong danh sách để xóa.", "Chưa chọn phòng học",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult hoi = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa phòng " + maPhongDangChon + "?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (hoi != DialogResult.Yes) return;

            string loi = _service.Xoa(maPhongDangChon);
            if (loi != "")
            {
                MessageBox.Show(loi, "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Đã xóa phòng học.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            NapDanhSach();
            LamMoi();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoi();
        }

        private void dgvPhongHoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvPhongHoc.Rows[e.RowIndex];

            maPhongDangChon = Convert.ToString(row.Cells["MaPhong"].Value) ?? "";

            txtMaPhong.Text = maPhongDangChon;
            txtMaPhong.ReadOnly = true;
            txtTenPhong.Text = Convert.ToString(row.Cells["TenPhong"].Value) ?? "";

            object? sucChua = row.Cells["SucChua"].Value;
            txtSucChua.Text = sucChua == null || sucChua == DBNull.Value
                ? ""
                : Convert.ToInt32(sucChua).ToString(CultureInfo.InvariantCulture);
        }

        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            e.SuppressKeyPress = true;
            NapDanhSach(txtTimKiem.Text.Trim());
            LamMoi();

            if (dgvPhongHoc.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy phòng học nào phù hợp.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private const int chieuCaopanelNhapLieuBanDau = 333;
        private const int chieuCaoNutThuPhong = 47;

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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }
    }
}
