using BTL_LTTQ.Common;
using BTL_LTTQ.Forms.Automation;
using BTL_LTTQ.Forms.HeThong;
using BTL_LTTQ.Forms.Import;
using BTL_LTTQ.Forms.QuanLy;
using BTL_LTTQ.Forms.TraCuu;

namespace BTL_LTTQ
{
    public partial class frmMain : Form
    {
        private const int PANEL_COLLAPSED_HEIGHT = 35;
        public bool DangXuat { get; private set; }
        private Form? activeForm;

        /// <summary>Số mục đang hiển thị trong nhóm "Hệ thống" (dùng để tính chiều cao panel).</summary>
        private int _soMucHeThong;

        public frmMain()
        {
            InitializeComponent();
            GiaoDien.ApDungMain(this);
        }
        private void CollapseAllPanels()
        {
            panelCRUD.Height = PANEL_COLLAPSED_HEIGHT;
            panelTraCuu.Height = PANEL_COLLAPSED_HEIGHT;
            panelBaoCao.Height = PANEL_COLLAPSED_HEIGHT;
            panelHeThong.Height = PANEL_COLLAPSED_HEIGHT;
        }

        private void HienThiTrangChu()
        {
            panelContent.Controls.Clear();

            lblChao.Text = $"Chào mừng bạn trở lại, {CurrentUser.TenDangNhap}!\nVai trò hệ thống: {CurrentUser.VaiTro}";

            panelContent.Controls.Add(lblChao);
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            ApDungQuyenMenu();
            CollapseAllPanels();
            HienThiTrangChu();
        }

        private void ApDungQuyenMenu()
        {
            PhanQuyenUi.AnNeuKhongCoQuyen(btnLopHoc, ChucNangConst.LOPHOC_XEM);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnHocVien, ChucNangConst.HOCVIEN_XEM);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnGiaoVien, ChucNangConst.GIAOVIEN_XEM);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnMonHoc, ChucNangConst.MONHOC_XEM);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnDanhMucKhac, ChucNangConst.DANHMUC_KHAC);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnDangKyLop, ChucNangConst.DANGKYLOP);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnThoiKhoaBieu, ChucNangConst.THOIKHOA_XEM);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnTimLop, ChucNangConst.TIM_LOP);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnTimGiaoVien, ChucNangConst.TIM_GIAOVIEN);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnBaoCaoDiem, ChucNangConst.BC_DIEM);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnDoanhThu, ChucNangConst.BC_DOANHTHU);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnTop3, ChucNangConst.BC_XEPHANG);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnLopTheoPhong, ChucNangConst.BC_LOP_PHONG);

            // Chức năng đăng ký lớp chỉ dành cho học viên.
            btnDangKyLop.Visible = CurrentUser.IsHocVien && PermissionService.Can(ChucNangConst.DANGKYLOP);

            // Nhập hàng loạt là chức năng quản trị dữ liệu: học viên không được dùng
            // (học viên tự đăng ký lớp bằng chức năng Đăng ký lớp, không nhập hàng loạt).
            bool coQuyenNhapHangLoat = !CurrentUser.IsHocVien &&
                                       (PermissionService.Can(ChucNangConst.HOCVIEN_THEM) ||
                                        PermissionService.Can(ChucNangConst.GIAOVIEN_THEM) ||
                                        PermissionService.Can(ChucNangConst.DANGKYLOP));
            btnNhapDuLieu.Visible = coQuyenNhapHangLoat;

            btnQuanLyMenu.Visible = SapXepMenu(panelCRUD,
                (btnLopHoc, ChucNangConst.LOPHOC_XEM),
                (btnHocVien, ChucNangConst.HOCVIEN_XEM),
                (btnGiaoVien, ChucNangConst.GIAOVIEN_XEM),
                (btnMonHoc, ChucNangConst.MONHOC_XEM),
                (btnDanhMucKhac, ChucNangConst.DANHMUC_KHAC),
                (btnDangKyLop, null),
                (btnNhapDuLieu, null));
            btnTraCuuMenu.Visible = SapXepMenu(panelTraCuu,
                (btnTimLop, ChucNangConst.TIM_LOP),
                (btnTimGiaoVien, ChucNangConst.TIM_GIAOVIEN),
                (btnThoiKhoaBieu, null));
            btnBaoCaoMenu.Visible = SapXepMenu(panelBaoCao,
                (btnBaoCaoDiem, ChucNangConst.BC_DIEM),
                (btnDoanhThu, ChucNangConst.BC_DOANHTHU),
                (btnTop3, ChucNangConst.BC_XEPHANG),
                (btnLopTheoPhong, ChucNangConst.BC_LOP_PHONG));

            bool coQuyenPhanQuyen = PermissionService.Can(ChucNangConst.PHANQUYEN);
            bool coQuyenTuDongHoa = PermissionService.Can(ChucNangConst.AUTOMATION_XEM);

            btnPhanQuyen.Visible = coQuyenPhanQuyen;
            btnTuDongHoa.Visible = coQuyenTuDongHoa;

            btnPhanQuyen.Location = new Point(btnPhanQuyen.Left, 38);
            btnTuDongHoa.Location = new Point(btnTuDongHoa.Left, 73);

            // Đếm theo biến quyền, không đọc Visible của control:
            // thuộc tính Visible trả về false khi panel cha đang ẩn.
            _soMucHeThong = (coQuyenPhanQuyen ? 1 : 0) + (coQuyenTuDongHoa ? 1 : 0);

            btnHeThong.Visible = _soMucHeThong > 0;
            panelHeThong.Visible = _soMucHeThong > 0;
            panelHeThong.Height = PANEL_COLLAPSED_HEIGHT + (_soMucHeThong * 35);
        }

        private void btnHeThong_Click(object? sender, EventArgs e)
        {
            panelHeThong.Height = panelHeThong.Height == PANEL_COLLAPSED_HEIGHT
                ? PANEL_COLLAPSED_HEIGHT + (_soMucHeThong * 35)
                : PANEL_COLLAPSED_HEIGHT;
        }

        private void btnPhanQuyen_Click(object? sender, EventArgs e)
        {
            if (!PhanQuyenUi.CoQuyenMo(ChucNangConst.PHANQUYEN))
            {
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này.", "Không có quyền",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var form = new frmPhanQuyen();
            form.ShowDialog(this);
        }

        private static bool SapXepMenu(Panel panel, params (Button Nut, string? MaChucNang)[] muc)
        {
            // MaChucNang = null nghĩa là mục đã được tính sẵn ở ApDungQuyenMenu.
            Button[] mucCoQuyen = muc
                .Where(m => (m.MaChucNang == null || PermissionService.Can(m.MaChucNang)) && m.Nut.Visible)
                .Select(m => m.Nut)
                .ToArray();
            bool canSapXepLai = mucCoQuyen.Length != muc.Length;
            int y = 38;
            for (int i = 0; i < mucCoQuyen.Length; i++)
            {
                Button button = mucCoQuyen[i];
                if (canSapXepLai)
                    button.Location = new Point(button.Left, y);
                y += 35;
            }

            panel.Visible = mucCoQuyen.Length > 0;
            panel.Height = PANEL_COLLAPSED_HEIGHT + (mucCoQuyen.Length * 35);
            return mucCoQuyen.Length > 0;
        }

        private void TogglePanel(Panel panelToToggle, params (Button Nut, string? MaChucNang)[] muc)
        {
            if (panelToToggle.Height == PANEL_COLLAPSED_HEIGHT)
            {
                panelToToggle.Height = PANEL_COLLAPSED_HEIGHT +
                    (muc.Count(m => (m.MaChucNang == null || PermissionService.Can(m.MaChucNang)) && m.Nut.Visible) * 35);
            }
            else
            {
                panelToToggle.Height = PANEL_COLLAPSED_HEIGHT;
            }
        }

        private void btnDoiMatKhau_Click(object? sender, EventArgs e)
        {
            using var form = new frmDoiMatKhau();
            form.ShowDialog(this);
        }

        private void btnDangXuat_Click(object? sender, EventArgs e)
        {
            DialogResult check = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (check != DialogResult.Yes)
                return;

            if (activeForm != null)
            {
                activeForm.Close();
                activeForm.Dispose();
                activeForm = null;
            }
            panelContent.Controls.Clear();

            PermissionService.Clear();
            CurrentUser.Xoa();
            DangXuat = true;
            Close();
        }

        private void btnQuanLyMenu_Click(object sender, EventArgs e)
        {
            TogglePanel(panelCRUD,
                (btnLopHoc, ChucNangConst.LOPHOC_XEM),
                (btnHocVien, ChucNangConst.HOCVIEN_XEM),
                (btnGiaoVien, ChucNangConst.GIAOVIEN_XEM),
                (btnMonHoc, ChucNangConst.MONHOC_XEM),
                (btnDanhMucKhac, ChucNangConst.DANHMUC_KHAC),
                (btnDangKyLop, null),
                (btnNhapDuLieu, null));
        }

        private void btnTraCuuMenu_Click(object sender, EventArgs e)
        {
            TogglePanel(panelTraCuu,
                (btnTimLop, ChucNangConst.TIM_LOP),
                (btnTimGiaoVien, ChucNangConst.TIM_GIAOVIEN),
                (btnThoiKhoaBieu, null));
        }

        private void btnBaoCaoMenu_Click(object sender, EventArgs e)
        {
            TogglePanel(panelBaoCao,
                (btnBaoCaoDiem, ChucNangConst.BC_DIEM),
                (btnDoanhThu, ChucNangConst.BC_DOANHTHU),
                (btnTop3, ChucNangConst.BC_XEPHANG),
                (btnLopTheoPhong, ChucNangConst.BC_LOP_PHONG));
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult check = MessageBox.Show("Bạn có chắc chắn muốn thoát ứng dụng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (check == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void panelCRUD_Paint(object sender, PaintEventArgs e)
        {

        }

        //Xu li form con
        private void OpenChildForm(Func<Form> taoForm, Button mucMenu, string? maChucNang)
        {
            if (maChucNang != null && !PhanQuyenUi.CoQuyenMo(maChucNang))
            {
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này.", "Không có quyền",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            GiaoDien.DanhDauMuc(mucMenu);
            Form childForm = taoForm();
            if (childForm == null)
            {
                MessageBox.Show("Chua tao form con", "Loi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (activeForm != null)
            {
                activeForm.Close();
            }

            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill; // fill toan bo panelContent

            panelContent.Controls.Add(childForm);
            GanSuKienBang(childForm);
            childForm.BringToFront();
            childForm.Show();
            CapNhatNutXuatExcel();
        }

        private void GanSuKienBang(Control cha)
        {
            foreach (Control control in cha.Controls)
            {
                if (control is DataGridView luoi)
                {
                    luoi.DataBindingComplete += Luoi_DataBindingComplete;
                    luoi.RowsAdded += Luoi_RowsAdded;
                    luoi.RowsRemoved += Luoi_RowsRemoved;
                }

                if (control.HasChildren)
                    GanSuKienBang(control);
            }
        }

        private void Luoi_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
        {
            CapNhatNutXuatExcel();
        }

        private void Luoi_RowsAdded(object? sender, DataGridViewRowsAddedEventArgs e)
        {
            CapNhatNutXuatExcel();
        }

        private void Luoi_RowsRemoved(object? sender, DataGridViewRowsRemovedEventArgs e)
        {
            CapNhatNutXuatExcel();
        }

        private void CapNhatNutXuatExcel()
        {
            DataGridView? luoi = activeForm == null || activeForm.Parent != panelContent? null: ExcelHelper.TimLuoi(activeForm);
            bool coDuLieu = luoi != null && luoi.Rows.Cast<DataGridViewRow>().Any(row => !row.IsNewRow);
            btnXuatExcel.Enabled = PermissionService.Can(ChucNangConst.XUAT_EXCEL) && coDuLieu;
        }

        private void btnLopHoc_Click(object sender, EventArgs e)
        {
            OpenChildForm(() => new frmLopHoc(), btnLopHoc, ChucNangConst.LOPHOC_XEM);
        }

        private void btnMonHoc_Click(object sender, EventArgs e)
        {
            OpenChildForm(() => new frmMonHoc(), btnMonHoc, ChucNangConst.MONHOC_XEM);
        }

        private void btnTimLop_Click(object sender, EventArgs e)
        {
            OpenChildForm(() => new frmTimKiemLopHoc(), btnTimLop, ChucNangConst.TIM_LOP);
        }

        private void btnTimGiaoVien_Click(object sender, EventArgs e)
        {
            OpenChildForm(() => new frmTimKiemGiaoVien(), btnTimGiaoVien, ChucNangConst.TIM_GIAOVIEN);
        }

        private void btnDangKyLop_Click(object sender, EventArgs e)
        {
            OpenChildForm(() => new frmDangKyLop(), btnDangKyLop, ChucNangConst.DANGKYLOP);
        }

        private void btnThoiKhoaBieu_Click(object sender, EventArgs e)
        {
            OpenChildForm(() => new frmThoiKhoaBieu(), btnThoiKhoaBieu, ChucNangConst.THOIKHOA_XEM);
        }

        private void btnNhapDuLieu_Click(object sender, EventArgs e)
        {
            // Quyền đã được kiểm tra khi dựng menu; form nhập hàng loạt tự kiểm tra theo từng loại dữ liệu.
            OpenChildForm(() => new FrmBulkImport(), btnNhapDuLieu, null);
        }

        private void btnTuDongHoa_Click(object sender, EventArgs e)
        {
            OpenChildForm(() => new FrmAutomationDashboard(), btnTuDongHoa, ChucNangConst.AUTOMATION_XEM);
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            DataGridView? luoi = activeForm == null || activeForm.Parent != panelContent ? null : ExcelHelper.TimLuoi(activeForm);
            if (!PermissionService.Can(ChucNangConst.XUAT_EXCEL) || luoi == null || !luoi.Rows.Cast<DataGridViewRow>().Any(row => !row.IsNewRow))
                return;

            ExcelHelper.XuatTrangHienTai(activeForm);
        }
    }
}
