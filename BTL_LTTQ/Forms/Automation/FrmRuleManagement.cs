using BTL_LTTQ.Common;
using BTL_LTTQ.Models.Automation;
using BTL_LTTQ.Repositories.Automation;
using BTL_LTTQ.Services.Automation;
using System;
using System.Windows.Forms;
using System.Linq;

namespace BTL_LTTQ.Forms.Automation
{
    public partial class FrmRuleManagement : Form
    {
        private readonly AutomationRuleRepository _ruleRepo = new AutomationRuleRepository();
        private AutomationRule? _ruleDangSua = null;

        public FrmRuleManagement()
        {
            InitializeComponent();
            Text = "Quản lý Rule tự động hóa";
            Load += FrmRuleManagement_Load;
        }

        private void FrmRuleManagement_Load(object sender, EventArgs e)
        {
            // Lớp bảo vệ cuối: quản lý quy tắc là quyền riêng của Admin.
            if (!PermissionService.Can(ChucNangConst.AUTOMATION_QUANLY))
            {
                MessageBox.Show("Bạn không có quyền quản lý quy tắc tự động hóa.",
                    "Không có quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            LoadBoLoc();

            CboLocTrangThai.SelectedIndex = 0;
            CboLocTrigger.SelectedIndex = 0;

            LoadDanhSachRule();
        }

        private void LoadBoLoc()
        {
            var dsTrigger = new System.Collections.Generic.List<object>
            {
                new { Code = "", Ten = "Tất cả trigger" }
            };
            foreach (string trigger in AutomationConst.Trigger.TatCa)
                dsTrigger.Add(new { Code = trigger, Ten = AutomationConst.Trigger.TenHienThi(trigger) });

            CboLocTrigger.DisplayMember = "Ten";
            CboLocTrigger.ValueMember = "Code";
            CboLocTrigger.DataSource = dsTrigger;

            CboLocTrangThai.DisplayMember = "Ten";
            CboLocTrangThai.ValueMember = "Code";
            CboLocTrangThai.DataSource = new System.Collections.Generic.List<object>
            {
                new { Code = "", Ten = "Tất cả" },
                new { Code = "ACTIVE", Ten = "Hoạt động" },
                new { Code = "INACTIVE", Ten = "Vô hiệu" }
            };
        }

        private void LoadDanhSachRule()
        {
            string tuKhoa = TxtTimKieu.Text;
            string trigger = CboLocTrigger.SelectedValue?.ToString() ?? "";
            string trangThai = CboLocTrangThai.SelectedValue?.ToString() ?? "";

            var ds = _ruleRepo.LayDanhSach(tuKhoa, trigger, trangThai);
            dgvDanhSach.DataSource = ds;

            // Đặt tiêu đề cột
            if (dgvDanhSach.Columns["RuleName"] != null)
                dgvDanhSach.Columns["RuleName"].HeaderText = "Tên Rule";
            if (dgvDanhSach.Columns["TriggerType"] != null)
                dgvDanhSach.Columns["TriggerType"].HeaderText = "Trigger";
            if (dgvDanhSach.Columns["Priority"] != null)
                dgvDanhSach.Columns["Priority"].HeaderText = "Priority";
            if (dgvDanhSach.Columns["SoDieuKien"] != null)
                dgvDanhSach.Columns["SoDieuKien"].HeaderText = "Số điều kiện";
            if (dgvDanhSach.Columns["SoHanhDong"] != null)
                dgvDanhSach.Columns["SoHanhDong"].HeaderText = "Số hành động";
            if (dgvDanhSach.Columns["SoLanChay"] != null)
                dgvDanhSach.Columns["SoLanChay"].HeaderText = "Lần chạy";
        }

        private void BtnThemMoi_Click(object sender, EventArgs e)
        {
            _ruleDangSua = null;
            using (var frmEditor = new FrmRuleEditor())
            {
                if (frmEditor.ShowDialog() == DialogResult.OK)
                {
                    var ruleMoi = frmEditor.LayRule();
                    if (_ruleRepo.TonTaiTen(ruleMoi.RuleName))
                    {
                        MessageBox.Show("Tên Rule đã tồn tại. Vui lòng chọn tên khác.", "Thông báo");
                        return;
                    }
                    int id = _ruleRepo.Them(ruleMoi);
                    MessageBox.Show($"Đã thêm Rule ID={id}", "Thông báo");
                    LoadDanhSachRule();
                }
            }
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (dgvDanhSach.CurrentRow == null)
            {
                MessageBox.Show("Chọn Rule cần sửa.", "Thông báo");
                return;
            }
            int ruleId = Convert.ToInt32(dgvDanhSach.CurrentRow.Cells["RuleID"].Value);
            _ruleDangSua = _ruleRepo.LayTheoID(ruleId);
            if (_ruleDangSua == null) return;

            using (var frmEditor = new FrmRuleEditor(_ruleDangSua))
            {
                if (frmEditor.ShowDialog() == DialogResult.OK)
                {
                    var ruleCapNhat = frmEditor.LayRule();
                    try
                    {
                        if (_ruleRepo.TonTaiTen(ruleCapNhat.RuleName, ruleCapNhat.RuleID))
                            throw new InvalidOperationException("Tên Rule đã tồn tại. Vui lòng chọn tên khác.");

                        _ruleRepo.Sua(ruleCapNhat); // Sử dụng RowVersion check
                        MessageBox.Show("Cập nhật Rule thành công.", "Thông báo");
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message, "Lỗi");
                    }
                    LoadDanhSachRule();
                }
            }
        }

        private void BtnXoaMem_Click(object sender, EventArgs e)
        {
            if (dgvDanhSach.CurrentRow == null) return;
            int ruleId = Convert.ToInt32(dgvDanhSach.CurrentRow.Cells["RuleID"].Value);
            var ruleHienTai = _ruleRepo.LayTheoID(ruleId);
            if (ruleHienTai == null) return;

            var result = MessageBox.Show(
                $"Xóa mem Rule '{ruleHienTai.RuleName}'?\n- Lịch sử thực thi vẫn được giữ.\n- Rule sẽ bị vô hiệu hóa.",
                "Xác nhận xóa mem", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                _ruleRepo.XoaMem(ruleId, CurrentUser.TenDangNhap);
                MessageBox.Show("Đã xóa mem Rule.", "Thông báo");
                LoadDanhSachRule();
            }
        }

        private void BtnDoiTrangThai_Click(object sender, EventArgs e)
        {
            if (dgvDanhSach.CurrentRow == null) return;
            int ruleId = Convert.ToInt32(dgvDanhSach.CurrentRow.Cells["RuleID"].Value);
            bool moi = !Convert.ToBoolean(dgvDanhSach.CurrentRow.Cells["IsActive"].Value);
            _ruleRepo.DoiTrangThai(ruleId, moi, CurrentUser.TenDangNhap);
            MessageBox.Show($"Rule {(moi ? "bật" : "tắt")} thành công.", "Thông báo");
            LoadDanhSachRule();
        }

        private void BtnXemChiTiet_Click(object sender, EventArgs e)
        {
            if (dgvDanhSach.CurrentRow == null) return;
            int ruleId = Convert.ToInt32(dgvDanhSach.CurrentRow.Cells["RuleID"].Value);
            var rule = _ruleRepo.LayTheoID(ruleId);
            if (rule == null) return;

            using (var frmEditor = new FrmRuleEditor(rule))
            {
                frmEditor.ReadOnly = true;
                frmEditor.ShowDialog();
            }
        }

        private void TxtTimKieu_TextChanged(object sender, EventArgs e)
        {
            LoadDanhSachRule();
        }

        private void CboLocTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDanhSachRule();
        }

        private void CboLocTrigger_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDanhSachRule();
        }
    }
}