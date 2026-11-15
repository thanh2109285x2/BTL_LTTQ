using BTL_LTTQ.BUS;
using BTL_LTTQ.Common;
using System.Data;

namespace BTL_LTTQ.Forms.BaoCao
{
    public partial class frmBaoCaoTop3 : Form, IXuatExcel
    {
        private readonly BaoCaoService _service = new();
        private DataTable _duLieuGoc = new();
        private bool _dangNap;
        public DataGridView LuoiXuat => dgvBaoCao;
        public string TieuDeXuat => Text;

        public frmBaoCaoTop3()
        {
            InitializeComponent();
            GiaoDien.ApDung(this);
            lblTieuDe.ForeColor = Color.White;
            lblMoTa.ForeColor = Color.FromArgb(226, 237, 250);
            DinhDangNutChinh(btnSapXep);
            Load += (_, _) => NapDuLieu();
        }

        private static void DinhDangNutChinh(Button nut)
        {
            nut.BackColor = GiaoDien.MauChinh;
            nut.ForeColor = Color.White;
            nut.FlatStyle = FlatStyle.Flat;
            nut.FlatAppearance.BorderSize = 0;
            nut.FlatAppearance.MouseOverBackColor = GiaoDien.MauChinhDam;
            nut.FlatAppearance.MouseDownBackColor = GiaoDien.MauChinhDam;
            nut.Cursor = Cursors.Hand;
            nut.Font = new Font(nut.Font, FontStyle.Bold);
        }

        private void NapDuLieu()
        {
            if (!PermissionService.Can(ChucNangConst.BC_XEPHANG))
            {
                MessageBox.Show("Bạn không có quyền xem báo cáo xếp hạng.", "Không có quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }
            try
            {
                _dangNap = true;
                _duLieuGoc = _service.LayTop3HocVienTheoLop();
                NapDanhSachLop();
                cboSapXep.SelectedIndex = 0;
                _dangNap = false;
                ApDungBoLoc();
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Bạn không có quyền xem báo cáo xếp hạng.", "Không có quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
            }
            catch (Exception ex)
            {
                _dangNap = false;
                MessageBox.Show("Không tải được báo cáo: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void NapDanhSachLop()
        {
            DataTable lop = _duLieuGoc.DefaultView.ToTable(true, "MaLop", "TenLop");
            DataRow tatCa = lop.NewRow();
            tatCa["MaLop"] = DBNull.Value;
            tatCa["TenLop"] = "-- Tất cả lớp --";
            lop.Rows.InsertAt(tatCa, 0);
            cboLop.DataSource = lop;
            cboLop.DisplayMember = "TenLop";
            cboLop.ValueMember = "MaLop";
            cboLop.SelectedIndex = 0;
        }

        private void ApDungBoLoc()
        {
            if (_dangNap || _duLieuGoc.Columns.Count == 0) return;
            IEnumerable<DataRow> rows = _duLieuGoc.AsEnumerable();
            string? maLop = cboLop.SelectedValue == null || cboLop.SelectedValue == DBNull.Value
                ? null : cboLop.SelectedValue.ToString();
            if (!string.IsNullOrWhiteSpace(maLop))
                rows = rows.Where(row => string.Equals(row["MaLop"]?.ToString(), maLop, StringComparison.OrdinalIgnoreCase));

            string tuKhoa = txtTimKiem.Text.Trim();
            if (tuKhoa.Length > 0)
                rows = rows.Where(row => (row["MaHocVien"]?.ToString() ?? "").Contains(tuKhoa, StringComparison.OrdinalIgnoreCase)
                    || (row["TenHocVien"]?.ToString() ?? "").Contains(tuKhoa, StringComparison.OrdinalIgnoreCase));

            rows = cboSapXep.SelectedIndex switch
            {
                1 => rows.OrderByDescending(row => Convert.ToDouble(row["Diem"])).ThenBy(row => row["TenLop"].ToString()).ThenBy(row => row["MaHocVien"].ToString()),
                2 => rows.OrderBy(row => Convert.ToDouble(row["Diem"])).ThenBy(row => row["TenLop"].ToString()).ThenBy(row => row["MaHocVien"].ToString()),
                _ => rows.OrderBy(row => row["TenLop"].ToString()).ThenBy(row => row["MaLop"].ToString()).ThenBy(row => Convert.ToInt32(row["Hang"]))
            };

            DataTable ketQua = _duLieuGoc.Clone();
            foreach (DataRow row in rows) ketQua.ImportRow(row);
            dgvBaoCao.DataSource = ketQua;
            string[] headers = ["Mã lớp", "Tên lớp", "Mã học viên", "Họ tên học viên", "Điểm", "Hạng"];
            for (int i = 0; i < ketQua.Columns.Count; i++) dgvBaoCao.Columns[i].HeaderText = headers[i];
            if (dgvBaoCao.Columns.Contains("Diem")) dgvBaoCao.Columns["Diem"].DefaultCellStyle.Format = "0.##";
            double trungBinh = ketQua.Rows.Count == 0 ? 0 : ketQua.AsEnumerable().Average(row => Convert.ToDouble(row["Diem"]));
            int soLop = ketQua.AsEnumerable().Select(row => row["MaLop"].ToString()).Distinct().Count();
            lblTong.Text = $"Sắp xếp: {cboSapXep.Text}     |     Số học viên: {ketQua.Rows.Count}     |     Số lớp: {soLop}     |     Điểm trung bình: {(ketQua.Rows.Count == 0 ? "—" : trungBinh.ToString("0.##"))}";
        }

        private void cboLop_SelectedIndexChanged(object? sender, EventArgs e) => ApDungBoLoc();
        private void btnSapXep_Click(object? sender, EventArgs e) => ApDungBoLoc();
        private void txtTimKiem_TextChanged(object? sender, EventArgs e) => ApDungBoLoc();
        private void btnLamMoi_Click(object? sender, EventArgs e)
        {
            txtTimKiem.Clear();
            NapDuLieu();
        }
        private void btnXuatExcel_Click(object? sender, EventArgs e) => ExcelHelper.XuatTrangHienTai(this);
        private void btnXemIn_Click(object? sender, EventArgs e)
        {
            if (dgvBaoCao.DataSource is DataTable data)
                using (var form = new BTL_LTTQ.Forms.frmXemBaoCao(Text, data)) form.ShowDialog(this);
        }
    }
}
