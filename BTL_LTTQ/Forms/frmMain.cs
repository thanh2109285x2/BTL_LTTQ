using BTL_LTTQ.Common;
using BTL_LTTQ.Forms.HeThong;
using BTL_LTTQ.Forms.QuanLy;
using BTL_LTTQ.Forms.TraCuu;

namespace BTL_LTTQ
{
    public partial class frmMain : Form
    {
        private const int PANEL_COLLAPSED_HEIGHT = 35;
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
            btnTrangChu.Visible = true;
            btnThoat.Visible = true;
            HienThiTrangChu();
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
            PhanQuyenUi.AnNeuKhongCoQuyen(btnBaoCaoDiem, ChucNangConst.BC_DIEM);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnDoanhThu, ChucNangConst.BC_DOANHTHU);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnTop3, ChucNangConst.BC_XEPHANG);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnLopTheoPhong, ChucNangConst.BC_LOP_PHONG);

            btnQuanLyMenu.Visible = SapXepMenu(panelCRUD,
                (btnLopHoc, ChucNangConst.LOPHOC_XEM),
                (btnHocVien, ChucNangConst.HOCVIEN_XEM),
                (btnGiaoVien, ChucNangConst.GIAOVIEN_XEM),
                (btnMonHoc, ChucNangConst.MONHOC_XEM),
                (btnDanhMucKhac, ChucNangConst.DANHMUC_KHAC));
            btnTraCuuMenu.Visible = SapXepMenu(panelTraCuu,
                (btnTimLop, ChucNangConst.TIM_LOP),
                (btnTimGiaoVien, ChucNangConst.TIM_GIAOVIEN));
            btnBaoCaoMenu.Visible = SapXepMenu(panelBaoCao,
                (btnBaoCaoDiem, ChucNangConst.BC_DIEM),
                (btnDoanhThu, ChucNangConst.BC_DOANHTHU),
                (btnTop3, ChucNangConst.BC_XEPHANG),
                (btnLopTheoPhong, ChucNangConst.BC_LOP_PHONG));

            bool coQuyenPhanQuyen = PermissionService.Can(ChucNangConst.PHANQUYEN);
            btnPhanQuyen.Visible = coQuyenPhanQuyen;
            btnHeThong.Visible = coQuyenPhanQuyen;
            panelHeThong.Visible = coQuyenPhanQuyen;
            panelHeThong.Height = coQuyenPhanQuyen ? PANEL_COLLAPSED_HEIGHT + 35 : PANEL_COLLAPSED_HEIGHT;
        }

        private void btnHeThong_Click(object? sender, EventArgs e)
        {
            panelHeThong.Height = panelHeThong.Height == PANEL_COLLAPSED_HEIGHT
                ? PANEL_COLLAPSED_HEIGHT + 35
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

        private static bool SapXepMenu(Panel panel, params (Button Nut, string MaChucNang)[] muc)
        {
            Button[] mucCoQuyen = muc
                .Where(m => PermissionService.Can(m.MaChucNang))
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

        private void TogglePanel(Panel panelToToggle, params (Button Nut, string MaChucNang)[] muc)
        {
            if (panelToToggle.Height == PANEL_COLLAPSED_HEIGHT)
            {
                panelToToggle.Height = PANEL_COLLAPSED_HEIGHT +
                    (muc.Count(m => PermissionService.Can(m.MaChucNang)) * 35);
            }
            else
            {
                panelToToggle.Height = PANEL_COLLAPSED_HEIGHT;
            }
        }

        private void btnQuanLyMenu_Click(object sender, EventArgs e)
        {
            TogglePanel(panelCRUD,
                (btnLopHoc, ChucNangConst.LOPHOC_XEM),
                (btnHocVien, ChucNangConst.HOCVIEN_XEM),
                (btnGiaoVien, ChucNangConst.GIAOVIEN_XEM),
                (btnMonHoc, ChucNangConst.MONHOC_XEM),
                (btnDanhMucKhac, ChucNangConst.DANHMUC_KHAC));
        }

        private void btnTraCuuMenu_Click(object sender, EventArgs e)
        {
            TogglePanel(panelTraCuu,
                (btnTimLop, ChucNangConst.TIM_LOP),
                (btnTimGiaoVien, ChucNangConst.TIM_GIAOVIEN));
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
        private Form activeForm = null;
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
            DataGridView? luoi = activeForm == null || activeForm.Parent != panelContent
                ? null
                : ExcelHelper.TimLuoi(activeForm);
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

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            DataGridView? luoi = activeForm == null || activeForm.Parent != panelContent
                ? null
                : ExcelHelper.TimLuoi(activeForm);
            if (!PermissionService.Can(ChucNangConst.XUAT_EXCEL) || luoi == null ||
                !luoi.Rows.Cast<DataGridViewRow>().Any(row => !row.IsNewRow))
                return;

            ExcelHelper.XuatTrangHienTai(activeForm);
        }
    }
}
