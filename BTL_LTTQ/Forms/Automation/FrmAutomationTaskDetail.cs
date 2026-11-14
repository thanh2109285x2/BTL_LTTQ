using BTL_LTTQ.Models.Automation;
using BTL_LTTQ.Repositories.Automation;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.Automation
{
    /// <summary>Xem chi tiết một nhiệm vụ do Automation tạo ra (chỉ đọc).</summary>
    public class FrmAutomationTaskDetail : Form
    {
        private readonly int _taskId;
        private readonly AutomationTaskRepository _repo = new();
        private readonly Label _lblThongTin;

        public FrmAutomationTaskDetail(int taskId)
        {
            _taskId = taskId;

            Text = $"Chi tiết nhiệm vụ #{taskId}";
            ClientSize = new Size(760, 420);
            MinimumSize = new Size(600, 320);
            StartPosition = FormStartPosition.CenterParent;

            _lblThongTin = new Label
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(16),
                TextAlign = ContentAlignment.TopLeft,
                Font = new Font("Segoe UI", 9.5F)
            };

            Controls.Add(_lblThongTin);

            Load += FrmAutomationTaskDetail_Load;
        }

        private void FrmAutomationTaskDetail_Load(object? sender, EventArgs e)
        {
            try
            {
                AutomationTask? task = _repo.LayTheoID(_taskId);
                if (task == null)
                {
                    _lblThongTin.Text = "Không tìm thấy nhiệm vụ này.";
                    return;
                }

                _lblThongTin.Text = string.Join(Environment.NewLine,
                    $"Tiêu đề: {task.Title}",
                    $"Mô tả: {task.Description}",
                    $"Quy tắc: #{task.RuleID}   |   Lần thực thi: {(task.ExecutionID.HasValue ? "#" + task.ExecutionID.Value : "-")}",
                    $"Đối tượng: {task.EntityType} {task.EntityID}",
                    $"Giao cho: {task.AssignedToAccount ?? "-"} ({task.AssignedToRole ?? "-"})",
                    $"Ưu tiên: {task.Priority}   |   Trạng thái: {task.Status}",
                    $"Hạn xử lý: {(task.DueDate.HasValue ? task.DueDate.Value.ToString("dd/MM/yyyy") : "-")}",
                    $"Tạo lúc: {task.CreatedAt:dd/MM/yyyy HH:mm:ss}",
                    $"Hoàn thành: {(task.CompletedAt.HasValue ? task.CompletedAt.Value.ToString("dd/MM/yyyy HH:mm:ss") + " bởi " + (task.CompletedBy ?? "-") : "-")}");
            }
            catch (Exception ex)
            {
                _lblThongTin.Text = $"Lỗi khi tải chi tiết: {ex.Message}";
            }
        }
    }
}
