using BTL_LTTQ.Forms.QuanLy;

namespace BTL_LTTQ
{
    public partial class frmMain : Form
    {
        private const int PANEL_COLLAPSED_HEIGHT = 35;
        private const int PANEL_CRUD_EXPANDED = PANEL_COLLAPSED_HEIGHT + (5 * 35);
        private const int PANEL_TRACUU_EXPANDED = PANEL_COLLAPSED_HEIGHT + (2 * 35);
        private const int PANEL_BAOCAO_EXPANDED = PANEL_COLLAPSED_HEIGHT + (4 * 35);
        public frmMain()
        {
            InitializeComponent();
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
        }
        private void TogglePanel(Panel panelToToggle, int expandedHeight)
        {
            if (panelToToggle.Height == PANEL_COLLAPSED_HEIGHT)
            {
                panelToToggle.Height = expandedHeight;
            }
            else
            {
                panelToToggle.Height = PANEL_COLLAPSED_HEIGHT;
            }
        }

        private void btnQuanLyMenu_Click(object sender, EventArgs e)
        {
            TogglePanel(panelCRUD, PANEL_CRUD_EXPANDED);
        }

        private void btnTraCuuMenu_Click(object sender, EventArgs e)
        {
            TogglePanel(panelTraCuu, PANEL_TRACUU_EXPANDED);
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
        private void OpenChildForm(Form childForm)
        {
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
        }
        private void btnLopHoc_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmLopHoc());
        }

    }
}
