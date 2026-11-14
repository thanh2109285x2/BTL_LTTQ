using BTL_LTTQ.Common;
using BTL_LTTQ.Models.Automation;
using BTL_LTTQ.Repositories.Automation;
using BTL_LTTQ.Services.Automation;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.Automation
{
    public partial class FrmRuleEditor : Form
    {
        private AutomationRule _rule;
        private bool _readOnly;

        public FrmRuleEditor()
        {
            InitializeComponent();
            _rule = new AutomationRule();
            _readOnly = false;
            Text = "Thêm Rule mới";
            Load += FrmRuleEditor_Load;
        }

        public FrmRuleEditor(AutomationRule rule)
        {
            InitializeComponent();
            _rule = rule ?? new AutomationRule();
            _readOnly = false;
            Text = $"Sửa Rule: {_rule.RuleName}";
            Load += FrmRuleEditor_Load;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ReadOnly
        {
            get => _readOnly;
            set
            {
                _readOnly = value;
                ApDungCheDoChiDoc();
            }
        }

        /// <summary>Trả về Rule đang được cấu hình (đã gắn điều kiện và hành động trong bộ nhớ).</summary>
        public AutomationRule LayRule() => _rule;

        private void FrmRuleEditor_Load(object sender, EventArgs e)
        {
            // Lớp bảo vệ cuối: chỉ người có quyền quản lý quy tắc mới mở được form này.
            if (!PermissionService.Can(ChucNangConst.AUTOMATION_QUANLY))
            {
                MessageBox.Show("Bạn không có quyền quản lý quy tắc tự động hóa.",
                    "Không có quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            LoadTruong();
            LoadTrigger();

            txtTenRule.Text = _rule.RuleName;
            txtMoTa.Text = _rule.Description ?? "";
            CboTrangThai.SelectedItem = _rule.IsActive ? "Hoạt động" : "Vô hiệu";
            CboTrigger.SelectedValue = _rule.TriggerType;

            LoadListDieuKien();
            LoadListHanhDong();

            ApDungCheDoChiDoc();
        }

        private void LoadTruong()
        {
            var ds = AutomationFieldCatalog.TatCa
                .Select(t => new { Code = t.Code, Ten = t.Ten })
                .ToList();

            CboField.DataSource = ds;
            CboField.DisplayMember = "Ten";
            CboField.ValueMember = "Code";
            CboField.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void LoadTrigger()
        {
            var ds = AutomationConst.Trigger.TatCa
                .Select(t => new { Code = t, Ten = AutomationConst.Trigger.TenHienThi(t) })
                .ToList();

            CboTrigger.DataSource = ds;
            CboTrigger.DisplayMember = "Ten";
            CboTrigger.ValueMember = "Code";
            CboTrigger.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void LoadListDieuKien()
        {
            var hienThi = _rule.Conditions
                .OrderBy(c => c.SortOrder)
                .Select(c => new
                {
                    c.ConditionID,
                    Field = AutomationFieldCatalog.Tim(c.FieldCode)?.Ten ?? c.FieldCode,
                    ToanTu = AutomationConst.Operator.TenHienThi(c.Operator),
                    GiaTri = ConditionEvaluator.GiaTriMongDoi(c),
                    MoTa = ConditionEvaluator.MoTa(c)
                })
                .ToList();

            dgvDieuKien.DataSource = hienThi;

            if (dgvDieuKien.Columns["ConditionID"] != null)
                dgvDieuKien.Columns["ConditionID"].Visible = false;
            if (dgvDieuKien.Columns["Field"] != null)
                dgvDieuKien.Columns["Field"].HeaderText = "Trường";
            if (dgvDieuKien.Columns["ToanTu"] != null)
                dgvDieuKien.Columns["ToanTu"].HeaderText = "Toán tử";
            if (dgvDieuKien.Columns["GiaTri"] != null)
                dgvDieuKien.Columns["GiaTri"].HeaderText = "Giá trị";
            if (dgvDieuKien.Columns["MoTa"] != null)
            {
                dgvDieuKien.Columns["MoTa"].HeaderText = "Mô tả";
                dgvDieuKien.Columns["MoTa"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void LoadListHanhDong()
        {
            var hienThi = _rule.Actions
                .OrderBy(a => a.SortOrder)
                .Select(a => new
                {
                    a.ActionID,
                    HanhDong = AutomationConst.ActionType.TenHienThi(a.ActionType),
                    a.ParametersJson,
                    TiepTucKhiLoi = a.ContinueOnError ? "Có" : "Không"
                })
                .ToList();

            dgvHanhDong.DataSource = hienThi;

            if (dgvHanhDong.Columns["ActionID"] != null)
                dgvHanhDong.Columns["ActionID"].Visible = false;
            if (dgvHanhDong.Columns["HanhDong"] != null)
                dgvHanhDong.Columns["HanhDong"].HeaderText = "Hành động";
            if (dgvHanhDong.Columns["ParametersJson"] != null)
            {
                dgvHanhDong.Columns["ParametersJson"].HeaderText = "Tham số JSON";
                dgvHanhDong.Columns["ParametersJson"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            if (dgvHanhDong.Columns["TiepTucKhiLoi"] != null)
                dgvHanhDong.Columns["TiepTucKhiLoi"].HeaderText = "Tiếp tục khi lỗi";
        }

        private void BtnThemDieuKien_Click(object sender, EventArgs e)
        {
            string? fieldCode = CboField.SelectedValue?.ToString();
            if (string.IsNullOrWhiteSpace(fieldCode))
            {
                MessageBox.Show("Vui lòng chọn trường dữ liệu.", "Thông báo");
                return;
            }

            using var dlg = new FrmDieuKienThem(fieldCode, AutomationFieldCatalog.ToanTuCho(fieldCode));
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            AutomationCondition dk = dlg.LayDieuKien();
            dk.SortOrder = _rule.Conditions.Count == 0
                ? 1
                : _rule.Conditions.Max(c => c.SortOrder) + 1;

            _rule.Conditions.Add(dk);
            LoadListDieuKien();
        }

        private void BtnSuaDieuKien_Click(object sender, EventArgs e)
        {
            int idx = LayChiSoDieuKienDangChon();
            if (idx < 0) return;

            AutomationCondition cond = _rule.Conditions[idx];
            using var dlg = new FrmDieuKienThem(
                cond.FieldCode,
                AutomationFieldCatalog.ToanTuCho(cond.FieldCode),
                cond);

            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            AutomationCondition moi = dlg.LayDieuKien();
            moi.ConditionID = cond.ConditionID;
            moi.RuleID = cond.RuleID;
            moi.SortOrder = cond.SortOrder;

            _rule.Conditions[idx] = moi;
            LoadListDieuKien();
        }

        private void BtnXoaDieuKien_Click(object sender, EventArgs e)
        {
            int idx = LayChiSoDieuKienDangChon();
            if (idx < 0) return;

            _rule.Conditions.RemoveAt(idx);
            DanhLaiSortOrderDieuKien();
            LoadListDieuKien();
        }

        private int LayChiSoDieuKienDangChon()
        {
            if (dgvDieuKien.CurrentRow == null) return -1;

            var ds = _rule.Conditions.OrderBy(c => c.SortOrder).ToList();
            int idx = dgvDieuKien.CurrentRow.Index;
            if (idx < 0 || idx >= ds.Count) return -1;

            return _rule.Conditions.IndexOf(ds[idx]);
        }

        private void DanhLaiSortOrderDieuKien()
        {
            int sort = 1;
            foreach (AutomationCondition c in _rule.Conditions.OrderBy(c => c.SortOrder))
                c.SortOrder = sort++;
        }

        private void BtnThemHanhDong_Click(object sender, EventArgs e)
        {
            using var dlg = new FrmHanhDongThem();
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            AutomationAction action = dlg.LayAction();
            action.SortOrder = _rule.Actions.Count == 0
                ? 1
                : _rule.Actions.Max(a => a.SortOrder) + 1;

            _rule.Actions.Add(action);
            LoadListHanhDong();
        }

        private void BtnSuaHanhDong_Click(object sender, EventArgs e)
        {
            int idx = LayChiSoHanhDongDangChon();
            if (idx < 0) return;

            AutomationAction action = _rule.Actions[idx];
            using var dlg = new FrmHanhDongThem(action);
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            AutomationAction moi = dlg.LayAction();
            moi.ActionID = action.ActionID;
            moi.RuleID = action.RuleID;
            moi.SortOrder = action.SortOrder;

            _rule.Actions[idx] = moi;
            LoadListHanhDong();
        }

        private void BtnXoaHanhDong_Click(object sender, EventArgs e)
        {
            int idx = LayChiSoHanhDongDangChon();
            if (idx < 0) return;

            _rule.Actions.RemoveAt(idx);
            DanhLaiSortOrderHanhDong();
            LoadListHanhDong();
        }

        private int LayChiSoHanhDongDangChon()
        {
            if (dgvHanhDong.CurrentRow == null) return -1;

            var ds = _rule.Actions.OrderBy(a => a.SortOrder).ToList();
            int idx = dgvHanhDong.CurrentRow.Index;
            if (idx < 0 || idx >= ds.Count) return -1;

            return _rule.Actions.IndexOf(ds[idx]);
        }

        private void DanhLaiSortOrderHanhDong()
        {
            int sort = 1;
            foreach (AutomationAction a in _rule.Actions.OrderBy(a => a.SortOrder))
                a.SortOrder = sort++;
        }

        private void BtnValidate_Click(object sender, EventArgs e)
        {
            var loi = ConditionEvaluator.KiemTraDanhSach(_rule.Conditions);
            if (loi.Count == 0 && _rule.Actions.Count == 0)
                loi.Add("Rule phải có ít nhất một hành động.");

            if (loi.Count == 0)
                MessageBox.Show("Cấu hình Rule hợp lệ.", "Kiểm tra", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show(string.Join(Environment.NewLine, loi), "Lỗi cấu hình", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void BtnTestRule_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                var context = new AutomationContext
                {
                    MaLop = "L01",
                    TenLop = "Lớp dữ liệu mẫu",
                    CurrentEnrollment = 5,
                    ClassCapacity = 30,
                    MinimumEnrollment = 4,
                    HasTeacher = true,
                    HasSchedule = false,
                    DaysUntilStart = 7,
                    DaysUntilEnd = 60,
                    EntityID = "L01",
                    EntityType = AutomationConst.EntityType.Class
                };

                var eval = new RuleEvaluator();
                KetQuaDanhGiaRule kq = eval.DanhGiaAsync(_rule, context).GetAwaiter().GetResult();

                if (!kq.HopLe)
                {
                    MessageBox.Show($"Lỗi cấu hình Rule:{Environment.NewLine}{kq.Loi}",
                        "Test Rule", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string thongBao = kq.TatCaDieuKienDung
                    ? "Tất cả điều kiện đúng."
                    : "Một số điều kiện sai.";

                foreach (KetQuaDieuKien dk in kq.DieuKien)
                {
                    thongBao += $"{Environment.NewLine}- {dk.MoTa}: {(dk.ThanhCong ? "Đúng" : "Sai")}";
                    if (!string.IsNullOrWhiteSpace(dk.Loi))
                        thongBao += $" (Lỗi: {dk.Loi})";
                }

                MessageBox.Show(thongBao, "Kết quả Test Rule", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi Test Rule: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenRule.Text))
            {
                MessageBox.Show("Vui lòng nhập tên Rule.", "Thông báo");
                txtTenRule.Focus();
                return;
            }

            var loi = ConditionEvaluator.KiemTraDanhSach(_rule.Conditions);
            if (loi.Count == 0 && _rule.Actions.Count == 0)
                loi.Add("Rule phải có ít nhất một hành động.");

            if (loi.Count > 0)
            {
                MessageBox.Show(string.Join(Environment.NewLine, loi), "Lỗi cấu hình",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _rule.RuleName = txtTenRule.Text.Trim();
            _rule.Description = string.IsNullOrWhiteSpace(txtMoTa.Text) ? null : txtMoTa.Text.Trim();
            _rule.TriggerType = CboTrigger.SelectedValue?.ToString() ?? "";
            _rule.IsActive = (CboTrangThai.SelectedItem?.ToString() == "Hoạt động");
            _rule.UpdatedBy = CurrentUser.TenDangNhap;
            if (_rule.RuleID <= 0)
                _rule.CreatedBy = CurrentUser.TenDangNhap;

            try
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ApDungCheDoChiDoc()
        {
            if (!_readOnly) return;

            txtTenRule.ReadOnly = true;
            txtMoTa.ReadOnly = true;
            CboTrigger.Enabled = false;
            CboTrangThai.Enabled = false;
            CboField.Enabled = false;
            BtnThemDieuKien.Enabled = false;
            BtnSuaDieuKien.Enabled = false;
            BtnXoaDieuKien.Enabled = false;
            BtnThemHanhDong.Enabled = false;
            BtnSuaHanhDong.Enabled = false;
            BtnXoaHanhDong.Enabled = false;
            BtnLuu.Enabled = false;
        }
    }
}
