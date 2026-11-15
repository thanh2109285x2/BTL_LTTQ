using BTL_LTTQ.BUS;
using BTL_LTTQ.Common;
using BTL_LTTQ.Models;
using System.Data;
using System.Globalization;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.NghiepVu
{
    public partial class frmNhapDiem : Form, IXuatExcel
    {
        private readonly NhapDiemService _service = new();
        private DataTable _danhSachXepLoai = new();
        private DataTable? _bangGoc;
        private bool _dangNap;
        private string _maHocVienDangChon = "";

        public DataGridView LuoiXuat => dgvHocVien;
        public string TieuDeXuat => "Bảng điểm";

        public frmNhapDiem()
        {
            InitializeComponent();
            GiaoDien.ApDung(this);
            Load += frmNhapDiem_Load;
            cboLop.SelectionChangeCommitted += cboLop_SelectionChangeCommitted;
            dgvHocVien.SelectionChanged += dgvHocVien_SelectionChanged;
            txtDiem.TextChanged += txtDiem_TextChanged;
            txtTimMaHocVien.KeyDown += txtTimKiem_KeyDown;
            txtTimTenHocVien.KeyDown += txtTimKiem_KeyDown;
            btnTimKiem.Click += btnTimKiem_Click;
            btnSuaDiem.Click += btnSuaDiem_Click;
            btnLamMoi.Click += btnLamMoi_Click;
        }

        private void frmNhapDiem_Load(object? sender, EventArgs e)
        {
            if (!PermissionService.Can(ChucNangConst.HOCVIEN_NHAPDIEM))
            {
                panelNoiDung.Visible = false;
                lblMoTa.Text = "Bạn không có quyền sử dụng chức năng nhập điểm.";
                return;
            }

            string loi = _service.LayDanhSachXepLoai(out _danhSachXepLoai);
            if (loi != "")
            {
                BaoLoi(loi);
                return;
            }
            loi = _service.LayDanhSachLop(out DataTable cacLop);
            if (loi != "")
            {
                BaoLoi(loi);
                return;
            }

            cboLop.DataSource = cacLop;
            cboLop.DisplayMember = "TenLop";
            cboLop.ValueMember = "MaLop";
            if (cacLop.Rows.Count > 0) NapDanhSachHocVien();
        }

        private void cboLop_SelectionChangeCommitted(object? sender, EventArgs e)
        {
            XoaThongTinDangChon();
            NapDanhSachHocVien();
        }

        private void dgvHocVien_SelectionChanged(object? sender, EventArgs e)
        {
            if (_dangNap) return;
            if (dgvHocVien.SelectedRows.Count == 0)
            {
                XoaThongTinDangChon();
                return;
            }

            DataGridViewRow row = dgvHocVien.SelectedRows[0];
            _maHocVienDangChon = Convert.ToString(row.Cells["MaHocVien"].Value) ?? "";
            txtMaHocVien.Text = _maHocVienDangChon;
            txtTenHocVien.Text = Convert.ToString(row.Cells["TenHocVien"].Value) ?? "";
            txtDiem.TextChanged -= txtDiem_TextChanged;
            txtDiem.Text = Convert.ToString(row.Cells["Diem"].Value, CultureInfo.CurrentCulture) ?? "";
            txtDiem.TextChanged += txtDiem_TextChanged;
            txtXepLoai.Text = Convert.ToString(row.Cells["XepLoai"].Value) ?? "";
            txtDiem.ReadOnly = false;
            btnSuaDiem.Enabled = true;
        }

        private void txtDiem_TextChanged(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDiem.Text))
            {
                txtXepLoai.Clear();
                return;
            }
            if (!decimal.TryParse(txtDiem.Text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out decimal diem))
            {
                txtXepLoai.Clear();
                return;
            }
            if (diem < 0m || diem > 10m)
            {
                txtXepLoai.Clear();
                return;
            }
            string loi = _service.TinhTenXepLoai(diem, _danhSachXepLoai, out string ten);
            txtXepLoai.Text = loi == "" ? ten : "";
        }

        private void btnSuaDiem_Click(object? sender, EventArgs e)
        {
            if (_maHocVienDangChon == "") return;
            decimal? diem = null;
            if (!string.IsNullOrWhiteSpace(txtDiem.Text))
            {
                if (!decimal.TryParse(txtDiem.Text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out decimal value))
                {
                    BaoLoi("Điểm phải là số từ 0 đến 10 hoặc để trống.");
                    return;
                }
                diem = value;
            }

            string maLop = Convert.ToString(cboLop.SelectedValue) ?? "";
            string ketQua = _service.LuuDiem(maLop, new[] { new DiemNhapDangKy(_maHocVienDangChon, diem) });
            if (ketQua != "")
            {
                BaoLoi(ketQua);
                return;
            }

            MessageBox.Show("Cập nhật điểm thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            NapDanhSachHocVien(_maHocVienDangChon);
        }

        private void btnLamMoi_Click(object? sender, EventArgs e)
        {
            XoaThongTinDangChon();
            NapDanhSachHocVien();
        }

        private void btnTimKiem_Click(object? sender, EventArgs e) => TimKiem();

        private void txtTimKiem_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
            TimKiem();
        }

        private void TimKiem()
        {
            if (_bangGoc == null) return;
            string maCanTim = txtTimMaHocVien.Text.Trim();
            string tenCanTim = txtTimTenHocVien.Text.Trim();
            DataTable loc = _bangGoc.Clone();
            foreach (DataRow row in _bangGoc.Rows)
            {
                if ((maCanTim.Length == 0 || Convert.ToString(row["MaHocVien"])!.Contains(maCanTim, StringComparison.CurrentCultureIgnoreCase)) &&
                    (tenCanTim.Length == 0 || Convert.ToString(row["TenHocVien"])!.Contains(tenCanTim, StringComparison.CurrentCultureIgnoreCase)))
                    loc.ImportRow(row);
            }
            HienThiBang(loc);
        }

        private void NapDanhSachHocVien(string maHocVienCanChon = "")
        {
            string maLop = Convert.ToString(cboLop.SelectedValue) ?? "";
            if (maLop == "") return;
            string loi = _service.LayDanhSachHocVien(maLop, out DataTable danhSach);
            if (loi != "")
            {
                BaoLoi(loi);
                return;
            }

            var bang = new DataTable();
            bang.Columns.Add("MaHocVien", typeof(string));
            bang.Columns.Add("TenHocVien", typeof(string));
            bang.Columns.Add("Diem", typeof(string));
            bang.Columns.Add("XepLoai", typeof(string));
            foreach (DataRow dong in danhSach.Rows)
            {
                decimal? diem = dong.IsNull("Diem") ? null : Convert.ToDecimal(dong["Diem"], CultureInfo.InvariantCulture);
                loi = _service.TinhTenXepLoai(diem, _danhSachXepLoai, out string ten);
                if (loi != "")
                {
                    BaoLoi(loi);
                    return;
                }
                bang.Rows.Add(dong["MaHocVien"], dong["TenHocVien"], diem?.ToString(CultureInfo.CurrentCulture) ?? "", ten);
            }

            _bangGoc = bang.Copy();
            _bangGoc.PrimaryKey = new[] { _bangGoc.Columns["MaHocVien"]! };
            txtTimMaHocVien.Clear();
            txtTimTenHocVien.Clear();
            HienThiBang(bang, maHocVienCanChon);
        }

        private void HienThiBang(DataTable bang, string maHocVienCanChon = "")
        {
            _dangNap = true;
            dgvHocVien.DataSource = bang;
            if (dgvHocVien.Columns.Count > 0)
            {
                dgvHocVien.Columns["MaHocVien"].HeaderText = "Mã học viên";
                dgvHocVien.Columns["TenHocVien"].HeaderText = "Họ và tên";
                dgvHocVien.Columns["Diem"].HeaderText = "Điểm";
                dgvHocVien.Columns["XepLoai"].HeaderText = "Xếp loại";
                foreach (DataGridViewColumn column in dgvHocVien.Columns) column.ReadOnly = true;
            }
            dgvHocVien.ClearSelection();
            if (maHocVienCanChon != "" && dgvHocVien.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvHocVien.Rows)
                    if (string.Equals(Convert.ToString(row.Cells["MaHocVien"].Value), maHocVienCanChon, StringComparison.OrdinalIgnoreCase))
                    {
                        row.Selected = true;
                        dgvHocVien.CurrentCell = row.Cells[0];
                        break;
                    }
            }
            _dangNap = false;
            if (dgvHocVien.SelectedRows.Count == 0) XoaThongTinDangChon();
            else dgvHocVien_SelectionChanged(this, EventArgs.Empty);
        }

        private void XoaThongTinDangChon()
        {
            _maHocVienDangChon = "";
            dgvHocVien.ClearSelection();
            txtMaHocVien.Clear();
            txtTenHocVien.Clear();
            txtDiem.Clear();
            txtXepLoai.Clear();
            txtDiem.ReadOnly = true;
            btnSuaDiem.Enabled = false;
        }

        private void BaoLoi(string loi)
        {
            MessageBox.Show(loi, "Nhập điểm", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
