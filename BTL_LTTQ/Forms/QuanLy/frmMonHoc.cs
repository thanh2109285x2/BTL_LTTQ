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
    public partial class frmMonHoc : Form
    {
        private readonly MonHocService _monHocService = new MonHocService();

        private string maMonDangChon = "";
        private bool _chiXem;

        public frmMonHoc()
        {
            InitializeComponent();
            GiaoDien.ApDung(this);
        }

        private void NapDanhSach(string tuKhoa = "")
        {
            dgvMonHoc.DataSource = _monHocService.LayDanhSach(tuKhoa);
            DinhDangBang();
        }

        private void DinhDangBang()
        {
            if (dgvMonHoc.Columns["MaMon"] is DataGridViewColumn cotMaMon)
                cotMaMon.HeaderText = "Mã môn";

            if (dgvMonHoc.Columns["TenMon"] is DataGridViewColumn cotTenMon)
                cotTenMon.HeaderText = "Tên môn";

            if (dgvMonHoc.Columns["HocPhi"] is DataGridViewColumn cotHocPhi)
            {
                cotHocPhi.HeaderText = "Học phí";
                cotHocPhi.DefaultCellStyle.Format = "N0";
                cotHocPhi.DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleLeft;
            }
        }

        private bool LayDuLieuTuForm(out MonHoc monHoc, out string loi)
        {
            monHoc = new MonHoc
            {
                MaMon = txtMaMon.Text.Trim(),
                TenMon = txtTenMon.Text.Trim(),
                HocPhi = null
            };
            loi = "";

            string hocPhi = textHocPhi.Text.Trim();
            if (hocPhi != "")
            {
                string chiSo = new string(hocPhi.Where(char.IsDigit).ToArray());
                if (chiSo == "" ||
                    !decimal.TryParse(chiSo, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal giaTri))
                {
                    loi = "Học phí không hợp lệ. Vui lòng chỉ nhập số tiền, ví dụ: 1500000.";
                    return false;
                }

                monHoc.HocPhi = giaTri;
            }

            return true;
        }

        private void LamMoi()
        {
            txtMaMon.Clear();
            txtTenMon.Clear();
            textHocPhi.Clear();

            dgvMonHoc.ClearSelection();

            maMonDangChon = "";
            txtMaMon.ReadOnly = _chiXem;

            txtMaMon.Focus();
        }

        private void frmMonHoc_Load(object sender, EventArgs e)
        {
            bool coQuyenThem = PermissionService.Can(ChucNangConst.MONHOC_THEM);
            bool coQuyenSua = PermissionService.Can(ChucNangConst.MONHOC_SUA);
            _chiXem = !coQuyenThem && !coQuyenSua;
            PhanQuyenUi.AnNutThemSuaXoa(
                btnThem, btnSua, btnXoa,
                ChucNangConst.MONHOC_THEM, ChucNangConst.MONHOC_SUA, ChucNangConst.MONHOC_XOA);

            NapDanhSach();
            LamMoi();
            ApDungCheDoChiXem();

        }

        private void ApDungCheDoChiXem()
        {
            txtMaMon.ReadOnly = _chiXem;
            txtTenMon.ReadOnly = _chiXem;
            textHocPhi.ReadOnly = _chiXem;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!PermissionService.Can(ChucNangConst.MONHOC_THEM))
            {
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này.", "Không có quyền",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!LayDuLieuTuForm(out MonHoc monHoc, out string loiNhap))
            {
                MessageBox.Show(loiNhap, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textHocPhi.Focus();
                return;
            }

            string loi = _monHocService.Them(monHoc);
            if (loi != "")
            {
                MessageBox.Show(loi, "Không thêm được", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Thêm môn học thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            NapDanhSach();
            LamMoi();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!PermissionService.Can(ChucNangConst.MONHOC_SUA))
            {
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này.", "Không có quyền",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (maMonDangChon == "")
            {
                MessageBox.Show("Vui lòng chọn một môn học trong danh sách để sửa.", "Chưa chọn môn học",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!LayDuLieuTuForm(out MonHoc monHoc, out string loiNhap))
            {
                MessageBox.Show(loiNhap, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textHocPhi.Focus();
                return;
            }

            monHoc.MaMon = maMonDangChon;

            string loi = _monHocService.Sua(monHoc);
            if (loi != "")
            {
                MessageBox.Show(loi, "Không sửa được", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Cập nhật môn học thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            NapDanhSach();
            LamMoi();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (!PermissionService.Can(ChucNangConst.MONHOC_XOA))
            {
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này.", "Không có quyền",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (maMonDangChon == "")
            {
                MessageBox.Show("Vui lòng chọn một môn học trong danh sách để xóa.", "Chưa chọn môn học",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult hoi = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa môn " + maMonDangChon + "?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (hoi != DialogResult.Yes) return;

            string loi = _monHocService.Xoa(maMonDangChon);
            if (loi != "")
            {
                MessageBox.Show(loi, "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Đã xóa môn học.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            NapDanhSach();
            LamMoi();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoi();
        }

        private void dgvMonHoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvMonHoc.Rows[e.RowIndex];

            maMonDangChon = Convert.ToString(row.Cells["MaMon"].Value) ?? "";

            txtMaMon.Text = maMonDangChon;
            txtMaMon.ReadOnly = true;
            txtTenMon.Text = Convert.ToString(row.Cells["TenMon"].Value) ?? "";

            object? hocPhi = row.Cells["HocPhi"].Value;
            textHocPhi.Text = hocPhi == null || hocPhi == DBNull.Value
                ? ""
                : Convert.ToDecimal(hocPhi).ToString("0.##", CultureInfo.InvariantCulture);
        }

        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            e.SuppressKeyPress = true;
            NapDanhSach(txtTimKiem.Text.Trim());
            LamMoi();

            if (dgvMonHoc.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy môn học nào phù hợp.", "Thông báo",
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
