using BTL_LTTQ.Common;
using BTL_LTTQ.Forms.HeThong;
using BTL_LTTQ.Forms;
using BTL_LTTQ.Forms.QuanLy;
using BTL_LTTQ.Forms.Import;
using BTL_LTTQ.Forms.BaoCao;
using BTL_LTTQ.Forms.NghiepVu;
using BTL_LTTQ.Forms.TraCuu;

namespace BTL_LTTQ
{
    public partial class frmMain : Form
    {
        private const int PANEL_COLLAPSED_HEIGHT = 35;
        private const int SIDEBAR_COLLAPSED_WIDTH = 55;
        private const int SIDEBAR_EXPANDED_WIDTH = 284;
        private readonly System.Windows.Forms.Timer sidebarTimer;
        private readonly System.Windows.Forms.Timer menuTimer;
        private bool sidebarExpand = true;
        private Panel? panelMenuDangHoatDong;
        private int menuTargetHeight = PANEL_COLLAPSED_HEIGHT;

        public bool DangXuat { get; private set; }
        private Form? activeForm;
        private frmTroLyAi? formTroLyAi;
        private Panel? panelTroLyAiMenu;
        private AppButton? btnTroLy;
        public frmMain()
        {
            InitializeComponent();
            components ??= new System.ComponentModel.Container();
            panelSidebar.MinimumSize = new Size(SIDEBAR_COLLAPSED_WIDTH, 0);
            panelSidebar.MaximumSize = new Size(SIDEBAR_EXPANDED_WIDTH, 0);

            sidebarTimer = new System.Windows.Forms.Timer(components) { Interval = 10 };
            sidebarTimer.Tick += sidebarTimer_Tick;
            menuTimer = new System.Windows.Forms.Timer(components) { Interval = 10 };
            menuTimer.Tick += menuTimer_Tick;

            GiaoDien.ApDungMain(this);
        }

        private void btnTrangChu_Click(object? sender, EventArgs e)
        {
            sidebarTimer.Start();
        }

        private void sidebarTimer_Tick(object? sender, EventArgs e)
        {
            if (sidebarExpand)
            {
                panelSidebar.Width -= 10;
                panelThoat.Visible = false;
                if (panelSidebar.Width <= SIDEBAR_COLLAPSED_WIDTH)
                {
                    panelSidebar.Width = SIDEBAR_COLLAPSED_WIDTH;
                    sidebarExpand = false;
                    sidebarTimer.Stop();
                }
            }
            else
            {
                panelSidebar.Width += 10;
                if (panelSidebar.Width >= SIDEBAR_EXPANDED_WIDTH)
                {
                    panelSidebar.Width = SIDEBAR_EXPANDED_WIDTH;
                    panelThoat.Visible = true;
                    sidebarExpand = true;
                    sidebarTimer.Stop();
                }
            }
        }

        private void menuTimer_Tick(object? sender, EventArgs e)
        {
            if (panelMenuDangHoatDong == null)
            {
                menuTimer.Stop();
                return;
            }

            int height = panelMenuDangHoatDong.Height;
            if (height < menuTargetHeight)
                panelMenuDangHoatDong.Height = Math.Min(height + 10, menuTargetHeight);
            else if (height > menuTargetHeight)
                panelMenuDangHoatDong.Height = Math.Max(height - 10, menuTargetHeight);

            if (panelMenuDangHoatDong.Height == menuTargetHeight)
                menuTimer.Stop();
        }
        private void CollapseAllPanels()
        {
            menuTimer.Stop();
            panelMenuDangHoatDong = null;
            menuTargetHeight = PANEL_COLLAPSED_HEIGHT;
            panelCRUD.Height = PANEL_COLLAPSED_HEIGHT;
            panelNghiepVu.Height = PANEL_COLLAPSED_HEIGHT;
            panelTraCuu.Height = PANEL_COLLAPSED_HEIGHT;
            panelBaoCao.Height = PANEL_COLLAPSED_HEIGHT;
            panelKhac.Height = PANEL_COLLAPSED_HEIGHT;
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

            if (btnTroLy == null || btnTroLy.IsDisposed)
            {
                panelTroLyAiMenu = new Panel
                {
                    Name = "panelTroLyAiMenu",
                    Size = new Size(271, 35),
                    Margin = new Padding(3)
                };
                btnTroLy = new AppButton
                {
                    Name = "btnTroLy",
                    Text = "🤖      Trợ lý AI",
                    Kieu = KieuNut.MenuCha,
                    Size = new Size(panelTroLyAiMenu.ClientSize.Width - 12, 29),
                    Margin = new Padding(3),
                    Location = new Point(6, 3),
                    TextAlign = ContentAlignment.MiddleLeft,
                    TextImageRelation = TextImageRelation.ImageBeforeText,
                    Padding = new Padding(8, 0, 0, 0)
                };
                btnTroLy.Click += btnTroLy_Click;
                btnTroLy.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                panelTroLyAiMenu.Controls.Add(btnTroLy);
            }

            if (panelTroLyAiMenu != null && !flowLayoutPanel1.Controls.Contains(panelTroLyAiMenu))
                flowLayoutPanel1.Controls.Add(panelTroLyAiMenu);
        }

        private void ApDungQuyenMenu()
        {
            PhanQuyenUi.AnNeuKhongCoQuyen(btnLopHoc, ChucNangConst.LOPHOC_XEM);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnHocVien, ChucNangConst.HOCVIEN_XEM);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnGiaoVien, ChucNangConst.GIAOVIEN_XEM);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnMonHoc, ChucNangConst.MONHOC_XEM);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnDanhMucKhac, ChucNangConst.DANHMUC_KHAC);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnTimLop, ChucNangConst.TIM_LOP);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnTimGiaoVien, ChucNangConst.TIM_GIAOVIEN);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnTimHocVien, ChucNangConst.TIM_HOCVIEN);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnThoiKhoaBieu, ChucNangConst.THOIKHOA_XEM);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnBaoCaoDiem, ChucNangConst.BC_DIEM);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnDoanhThu, ChucNangConst.BC_DOANHTHU);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnTop3, ChucNangConst.BC_XEPHANG);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnLopTheoPhong, ChucNangConst.BC_LOP_PHONG);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnDongHocPhi, ChucNangConst.DONGHOCPHI);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnNhapDiem, ChucNangConst.HOCVIEN_NHAPDIEM);

            btnQuanLyMenu.Visible = SapXepMenu(panelCRUD,
                (btnLopHoc, ChucNangConst.LOPHOC_XEM),
                (btnHocVien, ChucNangConst.HOCVIEN_XEM),
                (btnGiaoVien, ChucNangConst.GIAOVIEN_XEM),
                (btnMonHoc, ChucNangConst.MONHOC_XEM),
                (btnDanhMucKhac, ChucNangConst.DANHMUC_KHAC));
            btnTraCuuMenu.Visible = SapXepMenu(panelTraCuu,
                (btnTimLop, ChucNangConst.TIM_LOP),
                (btnTimGiaoVien, ChucNangConst.TIM_GIAOVIEN),
                (btnTimHocVien, ChucNangConst.TIM_HOCVIEN),
                (btnThoiKhoaBieu, ChucNangConst.THOIKHOA_XEM));
            btnNghiepVuMenu.Visible = SapXepMenuTheoQuyen(panelNghiepVu,
                (btnDangKyLop, CurrentUser.IsHocVien && PermissionService.Can(ChucNangConst.DANGKYLOP)),
                (btnDongHocPhi, PermissionService.Can(ChucNangConst.DONGHOCPHI)),
                (btnNhapDiem, PermissionService.Can(ChucNangConst.HOCVIEN_NHAPDIEM)));
            btnBaoCaoMenu.Visible = SapXepMenu(panelBaoCao,
                (btnBaoCaoDiem, ChucNangConst.BC_DIEM),
                (btnDoanhThu, ChucNangConst.BC_DOANHTHU),
                (btnTop3, ChucNangConst.BC_XEPHANG),
                (btnLopTheoPhong, ChucNangConst.BC_LOP_PHONG));

            bool coQuyenPhanQuyen = PermissionService.Can(ChucNangConst.PHANQUYEN);
            bool coQuyenNhap = CoQuyenNhapHangLoat();
            bool coQuyenXuat = PermissionService.Can(ChucNangConst.XUAT_EXCEL);
            btnPhanQuyen.Visible = coQuyenPhanQuyen;
            btnThemDuLieu.Visible = coQuyenNhap;
            btnXuatExcel.Visible = coQuyenXuat;
            btnKhac.Visible = SapXepMenuTheoQuyen(panelKhac,
                (btnThemDuLieu, coQuyenNhap),
                (btnXuatExcel, coQuyenXuat));
            panelKhac.Visible = btnKhac.Visible;
            btnHeThong.Visible = SapXepMenu(panelHeThong,
                (btnPhanQuyen, ChucNangConst.PHANQUYEN));
            panelHeThong.Visible = btnHeThong.Visible;
        }

        private void btnHeThong_Click(object? sender, EventArgs e)
        {
            TogglePanel(panelHeThong, btnPhanQuyen);
        }

        private void btnPhanQuyen_Click(object? sender, EventArgs e)
        {
            OpenChildForm(() => new frmPhanQuyen(), btnPhanQuyen, ChucNangConst.PHANQUYEN);
        }

        private static bool SapXepMenu(Panel panel, params (Button Nut, string MaChucNang)[] muc)
        {
            return SapXepMenuTheoQuyen(panel,
                muc.Select(m => (m.Nut, PermissionService.Can(m.MaChucNang))).ToArray());
        }

        private static bool SapXepMenuTheoQuyen(Panel panel, params (Button Nut, bool CoQuyen)[] muc)
        {
            Button[] mucCoQuyen = muc
                .Where(m => m.CoQuyen)
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

        private void TogglePanel(Panel panelToToggle, params Button[] muc)
        {
            int expandedHeight = Math.Max(PANEL_COLLAPSED_HEIGHT,
                muc.Where(button => button.Visible)
                    .Select(button => button.Bottom + 3)
                    .DefaultIfEmpty(PANEL_COLLAPSED_HEIGHT)
                    .Max());
            panelToToggle.MinimumSize = new Size(0, PANEL_COLLAPSED_HEIGHT);

            if (panelMenuDangHoatDong == panelToToggle && menuTimer.Enabled)
            {
                menuTargetHeight = menuTargetHeight > PANEL_COLLAPSED_HEIGHT
                    ? PANEL_COLLAPSED_HEIGHT
                    : expandedHeight;
            }
            else
            {
                menuTimer.Stop();
                CollapseOtherPanels(panelToToggle);
                menuTargetHeight = panelToToggle.Height <= PANEL_COLLAPSED_HEIGHT
                    ? expandedHeight
                    : PANEL_COLLAPSED_HEIGHT;
            }

            panelMenuDangHoatDong = panelToToggle;
            menuTimer.Start();
        }

        private void CollapseOtherPanels(Panel panelToKeep)
        {
            foreach (Panel panel in new[] { panelCRUD, panelNghiepVu, panelTraCuu, panelBaoCao, panelKhac, panelHeThong })
            {
                if (panel != panelToKeep)
                    panel.Height = PANEL_COLLAPSED_HEIGHT;
            }
        }

        private void btnNghiepVuMenu_Click(object sender, EventArgs e)
        {
            TogglePanel(panelNghiepVu, btnDangKyLop, btnDongHocPhi, btnNhapDiem);
        }

        private void btnDangKyLop_Click(object sender, EventArgs e)
        {
            OpenChildForm(() => new frmDangKyLop(), btnDangKyLop, ChucNangConst.DANGKYLOP);
        }

        private void btnDongHocPhi_Click(object sender, EventArgs e)
        {
            OpenChildForm(() => new frmDongHocPhi(), btnDongHocPhi, ChucNangConst.DONGHOCPHI);
        }

        private void btnNhapDiem_Click(object sender, EventArgs e)
        {
            OpenChildForm(() => new frmNhapDiem(), btnNhapDiem, ChucNangConst.HOCVIEN_NHAPDIEM);
        }

        private static bool CoQuyenNhapHangLoat()
        {
            return !CurrentUser.IsHocVien &&
                   (PermissionService.Can(ChucNangConst.HOCVIEN_THEM) ||
                    PermissionService.Can(ChucNangConst.GIAOVIEN_THEM) ||
                    PermissionService.Can(ChucNangConst.DANGKYLOP));
        }

        private void btnKhac_Click(object? sender, EventArgs e)
        {
            TogglePanel(panelKhac, btnThemDuLieu, btnXuatExcel);
        }

        private void btnThemDuLieu_Click(object? sender, EventArgs e)
        {
            if (!CoQuyenNhapHangLoat())
            {
                MessageBox.Show("Bạn không có quyền nhập dữ liệu hàng loạt.", "Không có quyền",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var form = new FrmBulkImport();
            form.ShowDialog(this);
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
            if (formTroLyAi != null && !formTroLyAi.IsDisposed)
            {
                formTroLyAi.Dispose();
                formTroLyAi = null;
            }
            panelContent.Controls.Clear();

            PermissionService.Clear();
            CurrentUser.Xoa();
            DangXuat = true;
            Close();
        }

        private void btnQuanLyMenu_Click(object sender, EventArgs e)
        {
            TogglePanel(panelCRUD, btnLopHoc, btnHocVien, btnGiaoVien, btnMonHoc, btnDanhMucKhac);
        }

        private void btnTraCuuMenu_Click(object sender, EventArgs e)
        {
            TogglePanel(panelTraCuu, btnTimLop, btnTimGiaoVien, btnTimHocVien, btnThoiKhoaBieu);
        }

        private void btnBaoCaoMenu_Click(object sender, EventArgs e)
        {
            TogglePanel(panelBaoCao, btnBaoCaoDiem, btnDoanhThu, btnTop3, btnLopTheoPhong);
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
        private void OpenChildForm(Func<Form> taoForm, Button mucMenu, string maChucNang)
        {
            if (!PhanQuyenUi.CoQuyenMo(maChucNang))
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
                if (activeForm == formTroLyAi)
                    activeForm.Hide();
                else
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
            DataGridView? luoi = activeForm == null || activeForm.Parent != panelContent ? null : ExcelHelper.TimLuoi(activeForm);
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

        private void btnHocVien_Click(object sender, EventArgs e)
        {
            OpenChildForm(() => new frmHocVien(), btnHocVien, ChucNangConst.HOCVIEN_XEM);
        }

        private void btnGiaoVien_Click(object sender, EventArgs e)
        {
            OpenChildForm(() => new frmGiaoVien(), btnGiaoVien, ChucNangConst.GIAOVIEN_XEM);
        }

        private void btnDanhMucKhac_Click(object? sender, EventArgs e)
        {
            OpenChildForm(() => new frmDanhMucKhac(), btnDanhMucKhac, ChucNangConst.DANHMUC_KHAC);
        }

        private void btnTimLop_Click(object sender, EventArgs e)
        {
            OpenChildForm(() => new frmTimKiemLopHoc(), btnTimLop, ChucNangConst.TIM_LOP);
        }

        private void btnTimGiaoVien_Click(object sender, EventArgs e)
        {
            OpenChildForm(() => new frmTimKiemGiaoVien(), btnTimGiaoVien, ChucNangConst.TIM_GIAOVIEN);
        }

        private void btnTimHocVien_Click(object sender, EventArgs e)
        {
            OpenChildForm(() => new frmTimKiemHocVien(), btnTimHocVien, ChucNangConst.TIM_HOCVIEN);
        }

        private void btnTroLy_Click(object? sender, EventArgs e)
        {
            if (btnTroLy == null) return;
            GiaoDien.DanhDauMuc(btnTroLy);
            if (activeForm != null && activeForm != formTroLyAi)
            {
                activeForm.Close();
                activeForm.Dispose();
                activeForm = null;
            }

            if (formTroLyAi == null || formTroLyAi.IsDisposed)
            {
                formTroLyAi = new frmTroLyAi
                {
                    TopLevel = false,
                    FormBorderStyle = FormBorderStyle.None,
                    Dock = DockStyle.Fill
                };
            }
            if (formTroLyAi.Parent != panelContent)
            {
                panelContent.Controls.Add(formTroLyAi);
            }

            activeForm = formTroLyAi;
            formTroLyAi.Show();
            formTroLyAi.BringToFront();
            CapNhatNutXuatExcel();
        }

        private void btnThoiKhoaBieu_Click(object sender, EventArgs e)
        {
            OpenChildForm(() => new frmThoiKhoaBieu(), btnThoiKhoaBieu, ChucNangConst.THOIKHOA_XEM);
        }

        private void btnBaoCaoDiem_Click(object sender, EventArgs e)
        {
            OpenChildForm(() => new frmBaoCaoDiem(), btnBaoCaoDiem, ChucNangConst.BC_DIEM);
        }

        private void btnDoanhThu_Click(object sender, EventArgs e)
        {
            OpenChildForm(() => new frmBaoCaoDoanhThu(), btnDoanhThu, ChucNangConst.BC_DOANHTHU);
        }

        private void btnTop3_Click(object? sender, EventArgs e)
        {
            if (!PhanQuyenUi.CoQuyenMo(ChucNangConst.BC_XEPHANG))
            {
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này.", "Không có quyền",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            OpenChildForm(() => new frmBaoCaoTop3(), btnTop3, ChucNangConst.BC_XEPHANG);
        }

        private void btnLopTheoPhong_Click(object? sender, EventArgs e)
        {
            if (!PhanQuyenUi.CoQuyenMo(ChucNangConst.BC_LOP_PHONG))
            {
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này.", "Không có quyền",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            OpenChildForm(() => new frmBaoCaoLopTheoPhong(), btnLopTheoPhong, ChucNangConst.BC_LOP_PHONG);
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
