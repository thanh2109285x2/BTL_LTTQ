using BTL_LTTQ.Common;
using BTL_LTTQ.Models.Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.Automation
{
    public partial class FrmHanhDongThem : Form
    {
        private readonly AutomationAction _action;
        private string _actionTypeSel = AutomationConst.ActionType.CreateNotification;

        public FrmHanhDongThem(AutomationAction? actionGoc = null)
        {
            InitializeComponent();

            _action = actionGoc != null
                ? new AutomationAction
                {
                    ActionID = actionGoc.ActionID,
                    RuleID = actionGoc.RuleID,
                    ActionType = actionGoc.ActionType,
                    SortOrder = actionGoc.SortOrder,
                    ParametersJson = actionGoc.ParametersJson,
                    ContinueOnError = actionGoc.ContinueOnError
                }
                : new AutomationAction { SortOrder = 1 };

            Load += FrmHanhDongThem_Load;
        }

        /// <summary>Hành động người dùng đã nhập (chỉ dùng khi ShowDialog trả về OK).</summary>
        public AutomationAction LayAction() => _action;

        private void FrmHanhDongThem_Load(object? sender, EventArgs e)
        {
            CboActionType.DisplayMember = "Ten";
            CboActionType.ValueMember = "Code";
            CboActionType.DataSource = AutomationConst.ActionType.TatCa
                .Select(a => new { Code = a, Ten = AutomationConst.ActionType.TenHienThi(a) })
                .ToList();

            cmbMucDo.DisplayMember = "Ten";
            cmbMucDo.ValueMember = "Code";
            cmbMucDo.DataSource = new List<object>
            {
                new { Code = AutomationConst.MucDoThongBao.Info, Ten = "Thông tin" },
                new { Code = AutomationConst.MucDoThongBao.Warning, Ten = "Cảnh báo" },
                new { Code = AutomationConst.MucDoThongBao.Error, Ten = "Lỗi" }
            };

            cmbLoaiAudit.DisplayMember = "Ten";
            cmbLoaiAudit.ValueMember = "Code";
            cmbLoaiAudit.DataSource = new List<object>
            {
                new { Code = AutomationConst.MucDoThongBao.Info, Ten = "Thông tin" },
                new { Code = AutomationConst.MucDoThongBao.Warning, Ten = "Cảnh báo" },
                new { Code = AutomationConst.MucDoThongBao.Error, Ten = "Lỗi" }
            };

            cmbUuTien.DisplayMember = "Ten";
            cmbUuTien.ValueMember = "Code";
            cmbUuTien.DataSource = new List<object>
            {
                new { Code = AutomationConst.TaskPriority.Low, Ten = "Thấp" },
                new { Code = AutomationConst.TaskPriority.Normal, Ten = "Bình thường" },
                new { Code = AutomationConst.TaskPriority.High, Ten = "Cao" }
            };

            cmbTrangThaiMoi.DisplayMember = "Ten";
            cmbTrangThaiMoi.ValueMember = "Code";
            cmbTrangThaiMoi.DataSource = AutomationConst.ClassStatus.TatCa
                .Select(s => new { Code = s, Ten = AutomationConst.ClassStatus.TenHienThi(s) })
                .ToList();

            NapDuLieuCu();

            string actionType = string.IsNullOrWhiteSpace(_action.ActionType)
                ? AutomationConst.ActionType.CreateNotification
                : _action.ActionType;

            if (AutomationConst.ActionType.TatCa.Contains(actionType))
                CboActionType.SelectedValue = actionType;
            else
                CboActionType.SelectedIndex = 0;

            chkTiepTucKhiLoi.Checked = _action.ContinueOnError;

            _actionTypeSel = CboActionType.SelectedValue?.ToString() ?? actionType;
            CapNhatPanel();
        }

        private void NapDuLieuCu()
        {
            if (string.IsNullOrWhiteSpace(_action.ParametersJson)) return;

            txtTieuDe.Text = DocChuoi("TieuDe") ?? "";
            txtMoTa.Text = DocChuoi("NoiDung") ?? "";
            txtTieuDeTask.Text = DocChuoi("TieuDe") ?? "";
            txtNoiDung.Text = DocChuoi("NoiDung") ?? "";

            string? mucDo = DocChuoi("MucDo");
            if (!string.IsNullOrWhiteSpace(mucDo)) cmbMucDo.SelectedValue = mucDo;

            string? uuTien = DocChuoi("UuTien");
            if (!string.IsNullOrWhiteSpace(uuTien)) cmbUuTien.SelectedValue = uuTien;

            string? trangThai = DocChuoi("TrangThai");
            if (!string.IsNullOrWhiteSpace(trangThai)) cmbTrangThaiMoi.SelectedValue = trangThai;

            string? loai = DocChuoi("Loai");
            if (!string.IsNullOrWhiteSpace(loai)) cmbLoaiAudit.SelectedValue = loai;

            string? han = DocChuoi("HanXuLy");
            if (!string.IsNullOrWhiteSpace(han) && DateTime.TryParse(han, out DateTime ngay))
                dtHanXuLy.Value = ngay;
        }

        private string? DocChuoi(string key)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(_action.ParametersJson);
                if (doc.RootElement.TryGetProperty(key, out JsonElement el) &&
                    el.ValueKind == JsonValueKind.String)
                {
                    return el.GetString();
                }
            }
            catch
            {
                // ParametersJson hỏng hoặc không phải JSON: bỏ qua, người dùng nhập lại.
            }
            return null;
        }

        private void CboActionType_SelectedIndexChanged(object? sender, EventArgs e)
        {
            _actionTypeSel = CboActionType.SelectedValue?.ToString()
                             ?? AutomationConst.ActionType.CreateNotification;
            CapNhatPanel();
        }

        private void CapNhatPanel()
        {
            panelNotification.Visible = _actionTypeSel == AutomationConst.ActionType.CreateNotification;
            panelTask.Visible = _actionTypeSel == AutomationConst.ActionType.CreateTask;
            panelStatus.Visible = _actionTypeSel == AutomationConst.ActionType.UpdateClassStatus;
            panelAudit.Visible = _actionTypeSel == AutomationConst.ActionType.WriteAuditLog;
        }

        private void BtnOK_Click(object? sender, EventArgs e)
        {
            string actionType = CboActionType.SelectedValue?.ToString()
                                ?? AutomationConst.ActionType.CreateNotification;

            var doc = new JsonObject();

            switch (actionType)
            {
                case AutomationConst.ActionType.CreateNotification:
                    if (string.IsNullOrWhiteSpace(txtTieuDe.Text) ||
                        string.IsNullOrWhiteSpace(txtMoTa.Text))
                    {
                        MessageBox.Show("Vui lòng nhập tiêu đề và nội dung thông báo.",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    doc["TieuDe"] = txtTieuDe.Text.Trim();
                    doc["NoiDung"] = txtMoTa.Text.Trim();
                    doc["MucDo"] = cmbMucDo.SelectedValue?.ToString() ?? AutomationConst.MucDoThongBao.Info;
                    break;

                case AutomationConst.ActionType.CreateTask:
                    if (string.IsNullOrWhiteSpace(txtTieuDeTask.Text))
                    {
                        MessageBox.Show("Vui lòng nhập tiêu đề nhiệm vụ.",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    doc["TieuDe"] = txtTieuDeTask.Text.Trim();
                    doc["UuTien"] = cmbUuTien.SelectedValue?.ToString() ?? AutomationConst.TaskPriority.Normal;
                    doc["HanXuLy"] = dtHanXuLy.Value.ToString("yyyy-MM-dd");
                    break;

                case AutomationConst.ActionType.UpdateClassStatus:
                    doc["TrangThai"] = cmbTrangThaiMoi.SelectedValue?.ToString()
                                       ?? AutomationConst.ClassStatus.SapKhaiGiang;
                    break;

                case AutomationConst.ActionType.WriteAuditLog:
                    if (string.IsNullOrWhiteSpace(txtNoiDung.Text))
                    {
                        MessageBox.Show("Vui lòng nhập nội dung nhật ký.",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    doc["NoiDung"] = txtNoiDung.Text.Trim();
                    doc["Loai"] = cmbLoaiAudit.SelectedValue?.ToString() ?? AutomationConst.MucDoThongBao.Info;
                    break;
            }

            _action.ActionType = actionType;
            _action.ParametersJson = doc.ToJsonString();
            _action.ContinueOnError = chkTiepTucKhiLoi.Checked;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnHuy_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
