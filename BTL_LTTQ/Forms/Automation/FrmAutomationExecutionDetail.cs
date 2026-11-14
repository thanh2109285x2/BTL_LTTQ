using BTL_LTTQ.Models.Automation;
using BTL_LTTQ.Repositories.Automation;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.Automation
{
    /// <summary>Xem chi tiết một lần thực thi Rule (chỉ đọc).</summary>
    public class FrmAutomationExecutionDetail : Form
    {
        private readonly long _executionId;
        private readonly AutomationExecutionRepository _repo = new();
        private readonly Label _lblThongTin;
        private readonly DataGridView _dgvLog;

        public FrmAutomationExecutionDetail(long executionId)
        {
            _executionId = executionId;

            Text = $"Chi tiết lần chạy #{executionId}";
            ClientSize = new Size(900, 560);
            MinimumSize = new Size(700, 450);
            StartPosition = FormStartPosition.CenterParent;

            _lblThongTin = new Label
            {
                Dock = DockStyle.Top,
                Height = 130,
                Padding = new Padding(12),
                TextAlign = ContentAlignment.TopLeft,
                Font = new Font("Segoe UI", 9.5F)
            };

            _dgvLog = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells,
                BackgroundColor = SystemColors.Window
            };

            Controls.Add(_dgvLog);
            Controls.Add(_lblThongTin);

            Load += FrmAutomationExecutionDetail_Load;
        }

        private void FrmAutomationExecutionDetail_Load(object? sender, EventArgs e)
        {
            try
            {
                AutomationExecution? exec = _repo.LayTheoID(_executionId);
                if (exec == null)
                {
                    _lblThongTin.Text = "Không tìm thấy lần thực thi này.";
                    return;
                }

                _lblThongTin.Text = string.Join(Environment.NewLine,
                    $"Rule: {exec.RuleNameSnapshot} (#{exec.RuleID})",
                    $"Trigger: {exec.TriggerType}   |   Đối tượng: {exec.EntityType} {exec.EntityID}",
                    $"Trạng thái: {exec.Status}   |   Điều kiện khớp: {(exec.ConditionsMatched ? "Có" : "Không")}",
                    $"Hành động: {exec.ActionSuccessCount} thành công / {exec.ActionFailCount} thất bại   |   Thời gian: {exec.DurationMs ?? 0} ms",
                    $"Bắt đầu: {exec.StartedAt:dd/MM/yyyy HH:mm:ss}   |   Kết thúc: {(exec.FinishedAt.HasValue ? exec.FinishedAt.Value.ToString("dd/MM/yyyy HH:mm:ss") : "-")}",
                    string.IsNullOrWhiteSpace(exec.ErrorMessage) ? "" : $"Lỗi: {exec.ErrorMessage}");

                DataTable log = _repo.LayLog(_executionId);
                _dgvLog.DataSource = log;

                if (_dgvLog.Columns["LogID"] != null) _dgvLog.Columns["LogID"].Visible = false;
                if (_dgvLog.Columns["ExecutionID"] != null) _dgvLog.Columns["ExecutionID"].Visible = false;
                if (_dgvLog.Columns["ConditionID"] != null) _dgvLog.Columns["ConditionID"].Visible = false;
                if (_dgvLog.Columns["ActionID"] != null) _dgvLog.Columns["ActionID"].Visible = false;
                if (_dgvLog.Columns["StepType"] != null) _dgvLog.Columns["StepType"].HeaderText = "Loại bước";
                if (_dgvLog.Columns["StepOrder"] != null) _dgvLog.Columns["StepOrder"].HeaderText = "Thứ tự";
                if (_dgvLog.Columns["StepName"] != null) _dgvLog.Columns["StepName"].HeaderText = "Tên bước";
                if (_dgvLog.Columns["IsSuccess"] != null) _dgvLog.Columns["IsSuccess"].HeaderText = "Thành công";
                if (_dgvLog.Columns["ExpectedValue"] != null) _dgvLog.Columns["ExpectedValue"].HeaderText = "Mong đợi";
                if (_dgvLog.Columns["ActualValue"] != null) _dgvLog.Columns["ActualValue"].HeaderText = "Thực tế";
                if (_dgvLog.Columns["Message"] != null) _dgvLog.Columns["Message"].HeaderText = "Thông điệp";
                if (_dgvLog.Columns["DurationMs"] != null) _dgvLog.Columns["DurationMs"].HeaderText = "Thời gian (ms)";
                if (_dgvLog.Columns["CreatedAt"] != null) _dgvLog.Columns["CreatedAt"].HeaderText = "Thời điểm";
            }
            catch (Exception ex)
            {
                _lblThongTin.Text = $"Lỗi khi tải chi tiết: {ex.Message}";
            }
        }
    }
}
