using BTL_LTTQ.Common;
<<<<<<< Updated upstream
=======
using BTL_LTTQ.Forms.Automation;
using BTL_LTTQ.Forms.HeThong;
using BTL_LTTQ.Forms.Import;
>>>>>>> Stashed changes
using BTL_LTTQ.Forms.QuanLy;
using BTL_LTTQ.Forms.TraCuu;

namespace BTL_LTTQ
{
    public partial class frmMain : Form
    {
        private const int PANEL_COLLAPSED_HEIGHT = 35;
<<<<<<< Updated upstream
        private const int PANEL_CRUD_EXPANDED = PANEL_COLLAPSED_HEIGHT + (5 * 35);
        private const int PANEL_TRACUU_EXPANDED = PANEL_COLLAPSED_HEIGHT + (2 * 35);
        private const int PANEL_BAOCAO_EXPANDED = PANEL_COLLAPSED_HEIGHT + (4 * 35);
=======

        /// <summary>Số mục đang hiển thị trong nhóm "Hệ thống" (dùng để tính chiều cao panel).</summary>
        private int _soMucHeThong;
>>>>>>> Stashed changes
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
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            CollapseAllPanels();
            btnXuatExcel.Enabled = false;
        }
<<<<<<< Updated upstream
        private void TogglePanel(Panel panelToToggle, int expandedHeight)
        {
            if (panelToToggle.Height == PANEL_COLLAPSED_HEIGHT)
            {
                panelToToggle.Height = expandedHeight;
=======

        private void ApDungQuyenMenu()
        {
            PhanQuyenUi.AnNeuKhongCoQuyen(btnLopHoc, ChucNangConst.LOPHOC_XEM);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnHocVien, ChucNangConst.HOCVIEN_XEM);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnGiaoVien, ChucNangConst.GIAOVIEN_XEM);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnMonHoc, ChucNangConst.MONHOC_XEM);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnDanhMucKhac, ChucNangConst.DANHMUC_KHAC);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnDangKyLop, ChucNangConst.DANGKYLOP);

            // Nhập hàng loạt là chức năng quản trị dữ liệu: học viên không được dùng
            // (học viên tự đăng ký lớp bằng chức năng Đăng ký lớp, không nhập hàng loạt).
            bool coQuyenNhapHangLoat = !CurrentUser.IsHocVien &&
                                       (PermissionService.Can(ChucNangConst.HOCVIEN_THEM) ||
                                        PermissionService.Can(ChucNangConst.GIAOVIEN_THEM) ||
                                        PermissionService.Can(ChucNangConst.DANGKYLOP));
            btnNhapDuLieu.Visible = coQuyenNhapHangLoat;
            PhanQuyenUi.AnNeuKhongCoQuyen(btnTimLop, ChucNangConst.TIM_LOP);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnTimGiaoVien, ChucNangConst.TIM_GIAOVIEN);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnThoiKhoaBieu, ChucNangConst.THOIKHOA_XEM);

            //Chức năng đăng ký lớp chỉ dành cho học viên
            if (!CurrentUser.IsHocVien)
                btnDangKyLop.Visible = false;
            PhanQuyenUi.AnNeuKhongCoQuyen(btnBaoCaoDiem, ChucNangConst.BC_DIEM);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnDoanhThu, ChucNangConst.BC_DOANHTHU);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnTop3, ChucNangConst.BC_XEPHANG);
            PhanQuyenUi.AnNeuKhongCoQuyen(btnLopTheoPhong, ChucNangConst.BC_LOP_PHONG);

            btnQuanLyMenu.Visible = SapXepMenu(panelCRUD,
                (btnLopHoc, ChucNangConst.LOPHOC_XEM),
                (btnHocVien, ChucNangConst.HOCVIEN_XEM),
                (btnGiaoVien, ChucNangConst.GIAOVIEN_XEM),
                (btnMonHoc, ChucNangConst.MONHOC_XEM),
                (btnDanhMucKhac, ChucNangConst.DANHMUC_KHAC),
                (btnDangKyLop, ChucNangConst.DANGKYLOP),
                (btnNhapDuLieu, null));
            btnTraCuuMenu.Visible = SapXepMenu(panelTraCuu,
                (btnTimLop, ChucNangConst.TIM_LOP),
                (btnTimGiaoVien, ChucNangConst.TIM_GIAOVIEN),
                (btnThoiKhoaBieu, ChucNangConst.THOIKHOA_XEM));
            btnBaoCaoMenu.Visible = SapXepMenu(panelBaoCao,
                (btnBaoCaoDiem, ChucNangConst.BC_DIEM),
                (btnDoanhThu, ChucNangConst.BC_DOANHTHU),
                (btnTop3, ChucNangConst.BC_XEPHANG),
                (btnLopTheoPhong, ChucNangConst.BC_LOP_PHONG));

            bool coQuyenPhanQuyen = PermissionService.Can(ChucNangConst.PHANQUYEN);
            btnPhanQuyen.Visible = coQuyenPhanQuyen;

            bool coQuyenTuDongHoa = PermissionService.Can(ChucNangConst.AUTOMATION_XEM);
            btnTuDongHoa.Visible = coQuyenTuDongHoa;

            // Sắp xếp các mục trong nhóm "Hệ thống" theo quyền.
            btnPhanQuyen.Location = new Point(btnPhanQuyen.Left, 38);
            btnTuDongHoa.Location = new Point(btnTuDongHoa.Left, 73);

            // Đếm theo biến quyền, không đọc Visible của control:
            // thuộc tính Visible trả về false khi panel cha đang ẩn.
            _soMucHeThong = (coQuyenPhanQuyen ? 1 : 0) + (coQuyenTuDongHoa ? 1 : 0);

            panelHeThong.Visible = _soMucHeThong > 0;
            btnHeThong.Visible = _soMucHeThong > 0;
            panelHeThong.Height = PANEL_COLLAPSED_HEIGHT + (_soMucHeThong * 35);
        }

        private void btnTuDongHoa_Click(object sender, EventArgs e)
        {
            OpenChildForm(() => new FrmAutomationDashboard(), btnTuDongHoa, ChucNangConst.AUTOMATION_XEM);
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
            Button[] mucCoQuyen = muc
                .Where(m => (m.MaChucNang == null || PermissionService.Can(m.MaChucNang)) && m.Nut.Visible)
                .Select(m => m.Nut)
                .ToArray();

            int y = 38;
            for (int i = 0; i < mucCoQuyen.Length; i++)
            {
                Button button = mucCoQuyen[i];
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
>>>>>>> Stashed changes
            }
            else
            {
                panelToToggle.Height = PANEL_COLLAPSED_HEIGHT;
            }
        }

        private void btnQuanLyMenu_Click(object sender, EventArgs e)
        {
<<<<<<< Updated upstream
            TogglePanel(panelCRUD, PANEL_CRUD_EXPANDED);
=======
            TogglePanel(panelCRUD,
                (btnLopHoc, ChucNangConst.LOPHOC_XEM),
                (btnHocVien, ChucNangConst.HOCVIEN_XEM),
                (btnGiaoVien, ChucNangConst.GIAOVIEN_XEM),
                (btnMonHoc, ChucNangConst.MONHOC_XEM),
                (btnDanhMucKhac, ChucNangConst.DANHMUC_KHAC),
                (btnDangKyLop, ChucNangConst.DANGKYLOP),
                (btnNhapDuLieu, null));
>>>>>>> Stashed changes
        }

        private void btnTraCuuMenu_Click(object sender, EventArgs e)
        {
<<<<<<< Updated upstream
            TogglePanel(panelTraCuu, PANEL_TRACUU_EXPANDED);
=======
            TogglePanel(panelTraCuu,
                (btnTimLop, ChucNangConst.TIM_LOP),
                (btnTimGiaoVien, ChucNangConst.TIM_GIAOVIEN),
                (btnThoiKhoaBieu, ChucNangConst.THOIKHOA_XEM));
>>>>>>> Stashed changes
        }

        private void btnBaoCaoMenu_Click(object sender, EventArgs e)
        {
            TogglePanel(panelBaoCao, PANEL_BAOCAO_EXPANDED);
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
<<<<<<< Updated upstream
        private void OpenChildForm(Form childForm)
        {
=======
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
>>>>>>> Stashed changes
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
            childForm.BringToFront();
            childForm.Show();

            btnXuatExcel.Enabled = ExcelHelper.TimLuoi(childForm) != null;
        }
        private void btnLopHoc_Click(object sender, EventArgs e)
        {
            GiaoDien.DanhDauMuc(btnLopHoc);
            OpenChildForm(new frmLopHoc());
        }

        private void btnMonHoc_Click(object sender, EventArgs e)
        {
            GiaoDien.DanhDauMuc(btnMonHoc);
            OpenChildForm(new frmMonHoc());
        }

        private void btnDangKyLop_Click(object sender, EventArgs e)
        {
            OpenChildForm(() => new frmDangKyLop(), btnDangKyLop, ChucNangConst.DANGKYLOP);
        }

        private void btnNhapDuLieu_Click(object sender, EventArgs e)
        {
            OpenChildForm(() => new FrmBulkImport(), btnNhapDuLieu, null);
        }

        private void btnThoiKhoaBieu_Click(object sender, EventArgs e)
        {
            OpenChildForm(() => new frmThoiKhoaBieu(), btnThoiKhoaBieu, ChucNangConst.THOIKHOA_XEM);
        }

        private void btnTimLop_Click(object sender, EventArgs e)
        {
            GiaoDien.DanhDauMuc(btnTimLop);
            OpenChildForm(new frmTimKiemLopHoc());
        }

        private void btnTimGiaoVien_Click(object sender, EventArgs e)
        {
            GiaoDien.DanhDauMuc(btnTimGiaoVien);
            OpenChildForm(new frmTimKiemGiaoVien());
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            ExcelHelper.XuatTrangHienTai(activeForm);
        }
    }
}
