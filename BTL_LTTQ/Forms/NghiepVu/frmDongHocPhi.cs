using BTL_LTTQ.BUS;
using BTL_LTTQ.Common;
using BTL_LTTQ.Models;
using QRCoder;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.NghiepVu
{
    public partial class frmDongHocPhi : Form, IXuatExcel
    {
        private readonly HocPhiService _service = new();
        private DataTable _lopChuaLapPhieu = new();
        private DataTable _phieuDangHien = new();
        private string? _maHocVienAdmin;
        private int? _maPhieuDangChon;

        public DataGridView LuoiXuat => dgvPhieuThu;
        public string TieuDeXuat => "Danh sách phiếu thu học phí";
        private bool LaAdmin => CurrentUser.IsAdmin;

        public frmDongHocPhi()
        {
            InitializeComponent();
            GiaoDien.ApDung(this);
            lblTieuDe.ForeColor = Color.White;
            lblMoTa.ForeColor = Color.FromArgb(225, 235, 248);
            dgvLopChuaLap.ReadOnly = false;
            foreach (DataGridViewColumn c in dgvLopChuaLap.Columns)
            {
                c.ReadOnly = c.Name != "Chon";
            }
        }

        private void frmDongHocPhi_Load(object? sender, EventArgs e)
        {
            if (!PermissionService.Can(ChucNangConst.DONGHOCPHI))
            {
                panelNoiDung.Visible = false;
                lblMoTa.Text = "Bạn không có quyền sử dụng chức năng đóng học phí.";
                return;
            }
            panelTimHocVien.Visible = LaAdmin;
            dgvHocVien.Visible = LaAdmin;
            btnXacNhanThu.Visible = LaAdmin && PermissionService.Can(ChucNangConst.HOCPHI_XACNHAN);
            btnHuyPhieu.Visible = !LaAdmin;
            btnTaoQR.Visible = true;
            btnTaoQR.Text = LaAdmin ? "Lập phiếu + QR" : "Tạo mã QR";
            lblNhacThanhToan.Text = "Sau khi chuyển khoản, phiếu ở trạng thái chờ cho tới khi Admin xác nhận. Hệ thống không tự biết tiền đã về.";
            NapDanhSachPhieu();
            if (LaAdmin) TimHocVien();
            else NapLopChuaLapPhieu();
        }

        private void btnTimHocVien_Click(object? sender, EventArgs e) => TimHocVien();

        private void txtTimHocVien_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
            TimHocVien();
        }

        private void TimHocVien()
        {
            string loi = _service.LayHocVien(txtTimHocVien.Text.Trim(), out DataTable danhSach);
            if (loi != "") { BaoLoi(loi); return; }
            dgvHocVien.DataSource = danhSach;
            if (dgvHocVien.Columns.Count > 0)
            {
                dgvHocVien.Columns["MaHocVien"].HeaderText = "Mã học viên";
                dgvHocVien.Columns["TenHocVien"].HeaderText = "Họ và tên";
                foreach (DataGridViewColumn column in dgvHocVien.Columns) column.ReadOnly = true;
            }
            dgvHocVien.ClearSelection();
            if (danhSach.Rows.Count > 0)
            {
                dgvHocVien.Rows[0].Selected = true;
                HienThiHocVien(Convert.ToString(danhSach.Rows[0]["MaHocVien"]));
            }
            else
            {
                _maHocVienAdmin = null;
                NapLopChuaLapPhieu();
                NapDanhSachPhieu(null);
            }
        }

        private void dgvHocVien_SelectionChanged(object? sender, EventArgs e)
        {
            if (!LaAdmin || dgvHocVien.SelectedRows.Count == 0) return;
            string? ma = Convert.ToString(dgvHocVien.SelectedRows[0].Cells["MaHocVien"].Value);
            if (!string.Equals(_maHocVienAdmin, ma, StringComparison.OrdinalIgnoreCase)) HienThiHocVien(ma);
        }

        private void HienThiHocVien(string? maHocVien)
        {
            if (string.IsNullOrWhiteSpace(maHocVien)) return;
            _maHocVienAdmin = maHocVien;
            NapLopChuaLapPhieu();
            NapDanhSachPhieu(null);
        }

        private void NapLopChuaLapPhieu()
        {
            string loi = _service.LayLopChuaLapPhieu(_maHocVienAdmin, out _lopChuaLapPhieu);
            if (loi != "") { BaoLoi(loi); return; }
            DataTable view = _lopChuaLapPhieu.Copy();
            if (!view.Columns.Contains("Chon")) view.Columns.Add("Chon", typeof(bool));
            foreach (DataRow row in view.Rows) row["Chon"] = false;
            dgvLopChuaLap.DataSource = view;
            if (dgvLopChuaLap.Columns.Count == 0) return;
            dgvLopChuaLap.Columns["MaHocVien"].Visible = false;
            dgvLopChuaLap.Columns["MaLop"].HeaderText = "Mã lớp";
            dgvLopChuaLap.Columns["TenLop"].HeaderText = "Lớp";
            dgvLopChuaLap.Columns["TenMon"].HeaderText = "Môn học";
            dgvLopChuaLap.Columns["HocPhi"].HeaderText = "Học phí";
            dgvLopChuaLap.Columns["HocPhi"].DefaultCellStyle.Format = "N0";
            dgvLopChuaLap.Columns["MaLop"].ReadOnly = true;
            dgvLopChuaLap.Columns["TenLop"].ReadOnly = true;
            dgvLopChuaLap.Columns["TenMon"].ReadOnly = true;
            dgvLopChuaLap.Columns["HocPhi"].ReadOnly = true;
            dgvLopChuaLap.Columns["Chon"].ReadOnly = false;
            dgvLopChuaLap.Columns["Chon"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvLopChuaLap.Columns["Chon"].DisplayIndex = 0;
            dgvLopChuaLap.Columns["Chon"].HeaderText = "Chọn";
            dgvLopChuaLap.Columns["Chon"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            lblTongTien.Text = "Tổng tiền: " + TongTienChon().ToString("N0", CultureInfo.CurrentCulture) + " đ";
        }

        private void dgvLopChuaLap_SelectionChanged(object? sender, EventArgs e)
        {
            lblTongTien.Text = "Tổng tiền: " + TongTienChon().ToString("N0", CultureInfo.CurrentCulture) + " đ";
        }

        private decimal TongTienChon()
        {
            decimal total = 0m;
            foreach (DataGridViewRow row in dgvLopChuaLap.Rows)
                if (row.Cells["Chon"].Value is bool selected && selected && row.Cells["HocPhi"].Value != DBNull.Value)
                    total += Convert.ToDecimal(row.Cells["HocPhi"].Value, CultureInfo.InvariantCulture);
            return total;
        }

        private void btnTaoQR_Click(object? sender, EventArgs e)
        {
            dgvLopChuaLap.EndEdit();
            string[] maLop = dgvLopChuaLap.Rows.Cast<DataGridViewRow>()
                .Where(r => r.Cells["Chon"].Value is bool selected && selected)
                .Select(r => Convert.ToString(r.Cells["MaLop"].Value) ?? "")
                .ToArray();
            string loi = _service.TaoPhieu(_maHocVienAdmin, maLop, out PhieuThuMoi? phieu);
            if (loi != "") { BaoLoi(loi); return; }
            if (phieu == null) return;
            NapLopChuaLapPhieu();
            NapDanhSachPhieu(null);
            HienThiQr(phieu.NoiDungCK, phieu.TongTien);
        }

        private void NapDanhSachPhieu(string? maHocVien = null)
        {
            string loi = _service.LayDanhSachPhieu(maHocVien, LaAdmin, out _phieuDangHien);
            if (loi != "") { BaoLoi(loi); return; }
            if (!_phieuDangHien.Columns.Contains("TenTrangThai"))
                _phieuDangHien.Columns.Add("TenTrangThai", typeof(string));
            foreach (DataRow row in _phieuDangHien.Rows)
            {
                row["TenTrangThai"] = Convert.ToByte(row["TrangThai"]) switch
                {
                    0 => "Chờ thanh toán",
                    1 => "Đã thu",
                    2 => "Đã hủy",
                    _ => "Không xác định"
                };
            }
            dgvPhieuThu.DataSource = _phieuDangHien;
            if (dgvPhieuThu.Columns.Count == 0) return;
            dgvPhieuThu.Columns["MaPhieu"].HeaderText = "Mã phiếu";
            dgvPhieuThu.Columns["MaHocVien"].HeaderText = "Mã học viên";
            dgvPhieuThu.Columns["TenHocVien"].HeaderText = "Học viên";
            dgvPhieuThu.Columns["NgayLap"].HeaderText = "Ngày lập";
            dgvPhieuThu.Columns["TongTien"].HeaderText = "Tổng tiền";
            dgvPhieuThu.Columns["NoiDungCK"].HeaderText = "Nội dung CK";
            dgvPhieuThu.Columns["TenTrangThai"].HeaderText = "Trạng thái";
            dgvPhieuThu.Columns["NgayThu"].HeaderText = "Ngày thu";
            dgvPhieuThu.Columns["NguoiThu"].HeaderText = "Người thu";
            dgvPhieuThu.Columns["TongTien"].DefaultCellStyle.Format = "N0";
            dgvPhieuThu.Columns["TrangThai"].Visible = false;
            dgvPhieuThu.ClearSelection();
            dgvPhieuThu_SelectionChanged(this, EventArgs.Empty);
        }

        private void dgvPhieuThu_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvPhieuThu.SelectedRows.Count == 0)
            {
                _maPhieuDangChon = null;
                btnXemQR.Enabled = false;
                btnHuyPhieu.Enabled = false;
                btnXacNhanThu.Enabled = false;
                return;
            }
            DataGridViewRow row = dgvPhieuThu.SelectedRows[0];
            _maPhieuDangChon = Convert.ToInt32(row.Cells["MaPhieu"].Value);
            bool cho = Convert.ToByte(row.Cells["TrangThai"].Value) == 0;
            btnXemQR.Enabled = cho && !LaAdmin;
            btnHuyPhieu.Enabled = cho && !LaAdmin;
            btnXacNhanThu.Enabled = cho && LaAdmin && PermissionService.Can(ChucNangConst.HOCPHI_XACNHAN);
        }

        private void btnXemQR_Click(object? sender, EventArgs e)
        {
            if (_maPhieuDangChon == null) return;
            DataRow? row = _phieuDangHien.AsEnumerable().FirstOrDefault(r => r.Field<int>("MaPhieu") == _maPhieuDangChon.Value);
            if (row != null) HienThiQr(row.Field<string>("NoiDungCK") ?? "", row.Field<decimal>("TongTien"));
        }

        private void HienThiQr(string noiDung, decimal tongTien)
        {
            try
            {
                string bin = ConfigurationManager.AppSettings["NganHang_BIN"] ?? "";
                string soTaiKhoan = ConfigurationManager.AppSettings["NganHang_SoTK"] ?? "";
                string tenTaiKhoan = ConfigurationManager.AppSettings["NganHang_TenTK"] ?? "";
                string payload = HocPhiService.TaoNoiDungVietQr(bin, soTaiKhoan, tongTien, noiDung);
                using var generator = new QRCodeGenerator();
                using QRCodeData data = generator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
                using var qr = new QRCode(data);
                using Bitmap image = qr.GetGraphic(8);
                picQr.Image?.Dispose();
                picQr.Image = new Bitmap(image);
                lblNganHang.Text = $"Chủ tài khoản: {tenTaiKhoan}\nSố tài khoản: {soTaiKhoan}\nSố tiền: {tongTien:N0} đ\nNội dung: {noiDung}";
            }
            catch (Exception ex) { BaoLoi("Không tạo được mã QR: " + ex.Message); }
        }

        private void btnHuyPhieu_Click(object? sender, EventArgs e)
        {
            if (_maPhieuDangChon == null) return;
            if (MessageBox.Show("Hủy phiếu đang chờ thanh toán này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            string loi = _service.HuyPhieu(_maPhieuDangChon.Value);
            if (loi != "") { BaoLoi(loi); return; }
            MessageBox.Show("Đã hủy phiếu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            NapDanhSachPhieu();
            NapLopChuaLapPhieu();
        }

        private void btnXacNhanThu_Click(object? sender, EventArgs e)
        {
            if (_maPhieuDangChon == null) return;
            string loi = _service.XacNhanDaThu(_maPhieuDangChon.Value);
            if (loi != "") { BaoLoi(loi); return; }
            NapDanhSachPhieu();
        }

        private void btnLamMoi_Click(object? sender, EventArgs e)
        {
            if (LaAdmin) TimHocVien();
            else
            {
                NapLopChuaLapPhieu();
                NapDanhSachPhieu();
            }
        }

        private void BaoLoi(string loi) => MessageBox.Show(loi, "Đóng học phí", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        private void dgvLopChuaLap_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
        {
            if (dgvLopChuaLap.IsCurrentCellDirty)
                dgvLopChuaLap.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }
    }
}
