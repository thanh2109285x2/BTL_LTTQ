using BTL_LTTQ.Common;
using BTL_LTTQ.DataAccessLayer;
using BTL_LTTQ.Models;
using System.Drawing;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.HeThong
{
    public partial class frmPhanQuyen : Form
    {
        private readonly PhanQuyenDAL _phanQuyenDAL = new PhanQuyenDAL();
        private List<PhanQuyenChucNang> _duLieuGoc = new List<PhanQuyenChucNang>();
        private bool _dangCapNhat;
        private bool _dangNap;

        public frmPhanQuyen()
        {
            InitializeComponent();
            GiaoDien.ApDung(this);
            dgvPhanQuyen.ReadOnly = false;
            dgvPhanQuyen.SelectionMode = DataGridViewSelectionMode.CellSelect;
            colChucNang.ReadOnly = true;
        }

        private void frmPhanQuyen_Load(object? sender, EventArgs e)
        {
            if (!PermissionService.Can(ChucNangConst.PHANQUYEN))
            {
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này.", "Không có quyền",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            NapDuLieu();
        }

        private void NapDuLieu()
        {
            _dangNap = true;
            try
            {
                List<PhanQuyenChucNang> danhSach = _phanQuyenDAL.LayDanhSachChucNang();
                dgvPhanQuyen.Rows.Clear();
                foreach (string nhom in new[] { "Quản lý", "Tra cứu", "Báo cáo", "Khác" })
                    ThemNhom(nhom, danhSach.Where(x => string.Equals(x.Nhom, nhom, StringComparison.OrdinalIgnoreCase)));

                foreach (var nhomKhac in danhSach
                    .Where(x => !new[] { "Quản lý", "Tra cứu", "Báo cáo", "Khác" }.Contains(x.Nhom, StringComparer.OrdinalIgnoreCase))
                    .GroupBy(x => string.IsNullOrWhiteSpace(x.Nhom) ? "Khác" : x.Nhom))
                    ThemNhom(nhomKhac.Key, nhomKhac);

                _duLieuGoc = Clone(danhSach);
                CapNhatTrangThaiNut();
            }
            finally
            {
                _dangNap = false;
            }
        }

        private void ThemNhom(string nhom, IEnumerable<PhanQuyenChucNang> danhSach)
        {
            List<PhanQuyenChucNang> cacChucNang = danhSach.OrderBy(x => x.ThuTu).ToList();
            if (cacChucNang.Count == 0)
                return;

            int dongNhom = dgvPhanQuyen.Rows.Add($"▾  {nhom}", null, null, null, null);
            DataGridViewRow rowNhom = dgvPhanQuyen.Rows[dongNhom];
            rowNhom.Tag = "GROUP:" + nhom;
            rowNhom.ReadOnly = true;
            rowNhom.DefaultCellStyle.BackColor = Color.FromArgb(224, 232, 240);
            rowNhom.DefaultCellStyle.Font = new Font(dgvPhanQuyen.Font, FontStyle.Bold);

            foreach (PhanQuyenChucNang item in cacChucNang)
            {
                int index = dgvPhanQuyen.Rows.Add(item.TenChucNang, item.GiaoVienCoQuyen,
                    item.GiaoVienChiCuaMinh, item.HocVienCoQuyen, item.HocVienChiCuaMinh);
                DataGridViewRow row = dgvPhanQuyen.Rows[index];
                row.Tag = item;
                if (!item.HoTroLocTheoChu)
                {
                    row.Cells[nameof(colGiaoVienChiCuaMinh)].ReadOnly = true;
                    row.Cells[nameof(colGiaoVienChiCuaMinh)].Style.BackColor = SystemColors.ControlLight;
                    row.Cells[nameof(colGiaoVienChiCuaMinh)].Value = false;
                    row.Cells[nameof(colHocVienChiCuaMinh)].ReadOnly = true;
                    row.Cells[nameof(colHocVienChiCuaMinh)].Style.BackColor = SystemColors.ControlLight;
                    row.Cells[nameof(colHocVienChiCuaMinh)].Value = false;
                }
            }
        }

        private void Grid_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
        {
            if (dgvPhanQuyen.IsCurrentCellDirty)
                dgvPhanQuyen.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void Grid_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (_dangNap || _dangCapNhat || e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            if (dgvPhanQuyen.Rows[e.RowIndex].Tag is not PhanQuyenChucNang)
                return;

            DataGridViewRow row = dgvPhanQuyen.Rows[e.RowIndex];
            string column = dgvPhanQuyen.Columns[e.ColumnIndex].Name;
            _dangCapNhat = true;
            try
            {
                if (column == nameof(colGiaoVienChiCuaMinh) && IsChecked(row.Cells[column].Value))
                    row.Cells[nameof(colGiaoVien)].Value = true;
                else if (column == nameof(colGiaoVien) && !IsChecked(row.Cells[column].Value))
                    row.Cells[nameof(colGiaoVienChiCuaMinh)].Value = false;
                else if (column == nameof(colHocVienChiCuaMinh) && IsChecked(row.Cells[column].Value))
                    row.Cells[nameof(colHocVien)].Value = true;
                else if (column == nameof(colHocVien) && !IsChecked(row.Cells[column].Value))
                    row.Cells[nameof(colHocVienChiCuaMinh)].Value = false;
            }
            finally
            {
                _dangCapNhat = false;
            }
            CapNhatTrangThaiNut();
        }

        private void Grid_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvPhanQuyen.Rows[e.RowIndex].Tag is not string tag || !tag.StartsWith("GROUP:", StringComparison.Ordinal))
                return;

            DataGridViewRow header = dgvPhanQuyen.Rows[e.RowIndex];
            bool thuGon = header.Cells[0].Value?.ToString()?.StartsWith("▾", StringComparison.Ordinal) == true;
            string nhom = tag.Substring("GROUP:".Length);
            header.Cells[0].Value = $"{(thuGon ? "▸" : "▾")}  {nhom}";
            for (int i = e.RowIndex + 1; i < dgvPhanQuyen.Rows.Count; i++)
            {
                DataGridViewRow row = dgvPhanQuyen.Rows[i];
                if (row.Tag is string groupTag && groupTag.StartsWith("GROUP:", StringComparison.Ordinal))
                    break;
                row.Visible = !thuGon;
            }
        }

        private void btnLuu_Click(object? sender, EventArgs e)
        {
            if (!PermissionService.Can(ChucNangConst.PHANQUYEN))
            {
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này.", "Không có quyền",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Validate())
                return;
            dgvPhanQuyen.EndEdit();
            List<PhanQuyenChucNang> danhSach = LayDuLieuTrenLuoi();
            try
            {
                _phanQuyenDAL.Luu(danhSach);
                _duLieuGoc = Clone(danhSach);
                CapNhatTrangThaiNut();
                MessageBox.Show("Đã lưu. Quyền mới có hiệu lực khi người dùng đăng nhập lại.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không lưu được phân quyền. " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHoanTac_Click(object? sender, EventArgs e)
        {
            NapDuLieu();
        }

        private void btnDong_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void frmPhanQuyen_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (!CoThayDoi())
                return;

            DialogResult result = MessageBox.Show("Bạn có thay đổi chưa lưu. Bạn có muốn đóng form không?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
                e.Cancel = true;
        }

        private List<PhanQuyenChucNang> LayDuLieuTrenLuoi()
        {
            var ketQua = new List<PhanQuyenChucNang>();
            foreach (DataGridViewRow row in dgvPhanQuyen.Rows)
            {
                if (row.Tag is not PhanQuyenChucNang item)
                    continue;
                ketQua.Add(new PhanQuyenChucNang
                {
                    MaChucNang = item.MaChucNang,
                    TenChucNang = item.TenChucNang,
                    Nhom = item.Nhom,
                    HoTroLocTheoChu = item.HoTroLocTheoChu,
                    ThuTu = item.ThuTu,
                    GiaoVienCoQuyen = IsChecked(row.Cells[nameof(colGiaoVien)].Value),
                    GiaoVienChiCuaMinh = item.HoTroLocTheoChu && IsChecked(row.Cells[nameof(colGiaoVienChiCuaMinh)].Value),
                    HocVienCoQuyen = IsChecked(row.Cells[nameof(colHocVien)].Value),
                    HocVienChiCuaMinh = item.HoTroLocTheoChu && IsChecked(row.Cells[nameof(colHocVienChiCuaMinh)].Value)
                });
            }
            return ketQua;
        }

        private bool CoThayDoi()
        {
            if (_dangNap)
                return false;
            List<PhanQuyenChucNang> hienTai = LayDuLieuTrenLuoi();
            if (hienTai.Count != _duLieuGoc.Count)
                return true;

            for (int i = 0; i < hienTai.Count; i++)
            {
                PhanQuyenChucNang a = hienTai[i];
                PhanQuyenChucNang b = _duLieuGoc[i];
                if (a.MaChucNang != b.MaChucNang || a.GiaoVienCoQuyen != b.GiaoVienCoQuyen ||
                    a.GiaoVienChiCuaMinh != b.GiaoVienChiCuaMinh || a.HocVienCoQuyen != b.HocVienCoQuyen ||
                    a.HocVienChiCuaMinh != b.HocVienChiCuaMinh)
                    return true;
            }
            return false;
        }

        private void CapNhatTrangThaiNut()
        {
            bool thayDoi = CoThayDoi();
            btnLuu.Enabled = thayDoi;
            btnHoanTac.Enabled = thayDoi;
        }

        private static bool IsChecked(object? value)
        {
            return value != null && value != DBNull.Value && Convert.ToBoolean(value);
        }

        private static List<PhanQuyenChucNang> Clone(IEnumerable<PhanQuyenChucNang> danhSach)
        {
            return danhSach.Select(x => new PhanQuyenChucNang
            {
                MaChucNang = x.MaChucNang,
                TenChucNang = x.TenChucNang,
                Nhom = x.Nhom,
                HoTroLocTheoChu = x.HoTroLocTheoChu,
                ThuTu = x.ThuTu,
                GiaoVienCoQuyen = x.GiaoVienCoQuyen,
                GiaoVienChiCuaMinh = x.GiaoVienChiCuaMinh,
                HocVienCoQuyen = x.HocVienCoQuyen,
                HocVienChiCuaMinh = x.HocVienChiCuaMinh
            }).ToList();
        }
    }
}
