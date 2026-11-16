using BTL_LTTQ.BUS;
using BTL_LTTQ.Common;
using System.Data;

namespace BTL_LTTQ.Forms.BaoCao
{
    public partial class frmBaoCaoLopTheoPhong : Form, IXuatExcel
    {
        private readonly BaoCaoService _service = new();
        public DataGridView LuoiXuat => dgvBaoCao;
        public string TieuDeXuat => Text;

        public frmBaoCaoLopTheoPhong()
        {
            InitializeComponent();
            GiaoDien.ApDung(this);
            Load += (_, _) => NapDuLieu();
        }

        private void NapDuLieu()
        {
            if (!PermissionService.Can(ChucNangConst.BC_LOP_PHONG))
            {
                MessageBox.Show("Bạn không có quyền xem danh sách lớp theo phòng.", "Không có quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }
            try
            {
                if (cboPhong.DataSource == null)
                    NapDanhSachPhong(_service.LayLopTheoPhong());

                string? maPhong = cboPhong.SelectedValue == null || cboPhong.SelectedValue == DBNull.Value
                    ? null : cboPhong.SelectedValue.ToString();
                DataTable data = _service.LayLopTheoPhong(maPhong);
                dgvBaoCao.DataSource = data;
                string[] headers = ["Mã phòng", "Phòng học", "Mã lớp", "Tên lớp", "Môn học", "Giáo viên", "Ca học", "Thời gian"];
                for (int i = 0; i < data.Columns.Count; i++) dgvBaoCao.Columns[i].HeaderText = headers[i];
                lblTong.Text = $"Số lớp: {data.AsEnumerable().Select(row => row["MaLop"]?.ToString()).Where(value => !string.IsNullOrWhiteSpace(value)).Distinct().Count()}     |     Số phòng: {data.AsEnumerable().Select(row => row["MaPhong"]?.ToString()).Where(value => !string.IsNullOrWhiteSpace(value)).Distinct().Count()}";
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Bạn không có quyền xem danh sách lớp theo phòng.", "Không có quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
            }
            catch (Exception ex) { MessageBox.Show("Không tải được báo cáo: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void NapDanhSachPhong(DataTable data)
        {
            var phong = data.DefaultView.ToTable(true, "MaPhong", "TenPhong");
            var tatCa = phong.NewRow(); tatCa["MaPhong"] = DBNull.Value; tatCa["TenPhong"] = "-- Tất cả phòng --"; phong.Rows.InsertAt(tatCa, 0);
            cboPhong.DataSource = phong; cboPhong.DisplayMember = "TenPhong"; cboPhong.ValueMember = "MaPhong";
        }

        private void btnXemBaoCao_Click(object? sender, EventArgs e) => NapDuLieu();
        private void btnLamMoi_Click(object? sender, EventArgs e)
        {
            if (cboPhong.Items.Count > 0) cboPhong.SelectedIndex = 0;
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
