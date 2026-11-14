using BTL_LTTQ.Common;
using BTL_LTTQ.Models.Automation;
using BTL_LTTQ.Repositories.Automation;
using BTL_LTTQ.Services.Automation;
using System;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.Automation
{
    public partial class FrmAutomationDashboard : Form
    {
        private readonly AutomationRuleRepository _ruleRepo = new AutomationRuleRepository();
        private readonly AutomationExecutionRepository _execRepo = new AutomationExecutionRepository();
        private readonly AutomationTaskRepository _taskRepo = new AutomationTaskRepository();
        private readonly ThongBaoRepository _tbRepo = new ThongBaoRepository();

        public FrmAutomationDashboard()
        {
            InitializeComponent();
            Text = "Dashboard Tự động hóa";
            Load += FrmAutomationDashboard_Load;
        }

        private void FrmAutomationDashboard_Load(object sender, EventArgs e)
        {
            // Lớp bảo vệ cuối: chỉ người có quyền xem tự động hóa mới mở được dashboard.
            if (!PermissionService.Can(ChucNangConst.AUTOMATION_XEM))
            {
                MessageBox.Show("Bạn không có quyền xem Trung tâm tự động hóa.",
                    "Không có quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            // Quản lý quy tắc là quyền riêng (chỉ Admin theo cấu hình mặc định).
            btnQuanLyRule.Visible = PermissionService.Can(ChucNangConst.AUTOMATION_QUANLY);

            LoadThongKe();
            LoadLanChayGanDay();
            LoadTaskDangCho();
            LoadThongBaoMoi();
        }

        private void btnQuanLyRule_Click(object sender, EventArgs e)
        {
            if (!PermissionService.Can(ChucNangConst.AUTOMATION_QUANLY))
            {
                MessageBox.Show("Bạn không có quyền quản lý quy tắc tự động hóa.",
                    "Không có quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var frmQuanLy = new FrmRuleManagement();
            frmQuanLy.ShowDialog(this);
        }

        private void LoadThongKe()
        {
            var thongKe = _execRepo.LayThongKe();

            lblTongRule.Text = thongKe.TongSoRule.ToString();
            lblRuleHoatDong.Text = thongKe.RuleDangHoatDong.ToString();
            lblLanChayThanhCong.Text = thongKe.LanChayThanhCong.ToString();
            lblLanChayThatBai.Text = thongKe.LanChayThatBai.ToString();
            lblLanChayMotPhan.Text = thongKe.LanChayMotPhan.ToString();
        }

        private void LoadLanChayGanDay()
        {
            var dt = _execRepo.LayDanhSach(null, null, "", "", "Lần chạy gần đây", 10);
            dgvLanChay.DataSource = dt;
            if (dgvLanChay.Columns["RuleNameSnapshot"] != null)
                dgvLanChay.Columns["RuleNameSnapshot"].HeaderText = "Rule";
            if (dgvLanChay.Columns["EntityID"] != null)
                dgvLanChay.Columns["EntityID"].HeaderText = "Lớp/Mã";
            if (dgvLanChay.Columns["Status"] != null)
                dgvLanChay.Columns["Status"].HeaderText = "Trạng thái";
            if (dgvLanChay.Columns["StartedAt"] != null)
                dgvLanChay.Columns["StartedAt"].HeaderText = "Thời gian";
        }

        private void LoadTaskDangCho()
        {
            int dangCho = _taskRepo.DemDangCho();
            lblTaskDangCho.Text = dangCho.ToString();

            if (dangCho > 0)
            {
                var dt = _taskRepo.LayDanhSach("", "", "Admin", "admin");
                dgvTaskDangCho.DataSource = dt;
                if (dgvTaskDangCho.Columns["Title"] != null)
                    dgvTaskDangCho.Columns["Title"].HeaderText = "Tiêu đề";
                if (dgvTaskDangCho.Columns["AssignedToRole"] != null)
                    dgvTaskDangCho.Columns["AssignedToRole"].HeaderText = "Giao cho";
            }
        }

        private void LoadThongBaoMoi()
        {
            int chuaDoc = _tbRepo.DemChuaDoc(null, null);
            lblThongBaoMoi.Text = chuaDoc.ToString();

            if (chuaDoc > 0)
            {
                var dt = _tbRepo.LayGanDay(null, null, 20);
                dgvThongBaoMoi.DataSource = dt;
                if (dgvThongBaoMoi.Columns["TieuDe"] != null)
                    dgvThongBaoMoi.Columns["TieuDe"].HeaderText = "Tiêu đề";
                if (dgvThongBaoMoi.Columns["MucDo"] != null)
                    dgvThongBaoMoi.Columns["MucDo"].HeaderText = "Mức độ";
                if (dgvThongBaoMoi.Columns["CreatedAt"] != null)
                    dgvThongBaoMoi.Columns["CreatedAt"].HeaderText = "Thời gian";
            }
        }

        private void dgvLanChay_DoubleClick(object sender, EventArgs e)
        {
            if (dgvLanChay.CurrentRow == null) return;
            long executionId = Convert.ToInt64(dgvLanChay.CurrentRow.Cells["ExecutionID"].Value);
            var execForm = new FrmAutomationExecutionDetail(executionId);
            execForm.Show();
        }

        private void dgvTaskDangCho_DoubleClick(object sender, EventArgs e)
        {
            if (dgvTaskDangCho.CurrentRow == null) return;
            int taskId = Convert.ToInt32(dgvTaskDangCho.CurrentRow.Cells["TaskID"].Value);
            var taskForm = new FrmAutomationTaskDetail(taskId);
            taskForm.Show();
        }

        private void dgvThongBaoMoi_DoubleClick(object sender, EventArgs e)
        {
            if (dgvThongBaoMoi.CurrentRow == null) return;
            long tbId = Convert.ToInt64(dgvThongBaoMoi.CurrentRow.Cells["ThongBaoID"].Value);
            _tbRepo.DanhDauDaDoc(tbId);
            LoadThongBaoMoi();
        }
    }
}