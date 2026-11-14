using BTL_LTTQ.Common;
using BTL_LTTQ.Repositories;
using BTL_LTTQ.Services.Automation;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.Automation
{
    /// <summary>
    /// Phát sự kiện thử cho một lớp để kiểm tra Rule ngay trên giao diện.
    /// Dùng đúng đường code của nghiệp vụ (AutomationEventPublisher.PhatSuKienAsync),
    /// nên Rule được đánh giá và hành động được thực thi y hệt khi chạy thật.
    /// </summary>
    public partial class FrmPhatSuKienThu : Form
    {
        private readonly LopHocRepository _lopHocRepo = new();

        public FrmPhatSuKienThu()
        {
            InitializeComponent();
            GiaoDien.ApDung(this);
        }

        private void FrmPhatSuKienThu_Load(object? sender, EventArgs e)
        {
            LoadLop();
            LoadTrigger();
        }

        private void LoadLop()
        {
            try
            {
                DataTable dt = _lopHocRepo.LayDanhSach();

                var ds = dt.Rows.Cast<DataRow>()
                    .Select(r => new
                    {
                        MaLop = r["MaLop"].ToString() ?? "",
                        Ten = $"{r["MaLop"].ToString()} - {r["TenLop"].ToString()}"
                    })
                    .Where(x => x.MaLop != "")
                    .ToList();

                cboLop.DisplayMember = "Ten";
                cboLop.ValueMember = "MaLop";
                cboLop.DataSource = ds;

                if (cboLop.Items.Count == 0)
                {
                    lblKetQua.ForeColor = Color.FromArgb(192, 57, 43);
                    lblKetQua.Text = "Không có lớp học nào để thử.";
                }
            }
            catch (Exception ex)
            {
                lblKetQua.ForeColor = Color.FromArgb(192, 57, 43);
                lblKetQua.Text = "Không tải được danh sách lớp: " + ex.Message;
            }
        }

        private void LoadTrigger()
        {
            var ds = AutomationConst.Trigger.TatCa
                .Select(t => new { Code = t, Ten = AutomationConst.Trigger.TenHienThi(t) })
                .ToList();

            cboTrigger.DisplayMember = "Ten";
            cboTrigger.ValueMember = "Code";
            cboTrigger.DataSource = ds;

            // Ưu tiên trigger đang thực sự được phát từ nghiệp vụ.
            int viTri = ds.FindIndex(x => x.Code == AutomationConst.Trigger.ClassEnrollmentChanged);
            if (viTri >= 0)
                cboTrigger.SelectedIndex = viTri;
        }

        private async void btnPhat_Click(object sender, EventArgs e)
        {
            string maLop = cboLop.SelectedValue?.ToString() ?? "";
            string trigger = cboTrigger.SelectedValue?.ToString() ?? "";

            if (maLop == "" || trigger == "")
            {
                lblKetQua.ForeColor = Color.FromArgb(192, 57, 43);
                lblKetQua.Text = "Hãy chọn lớp và loại sự kiện.";
                return;
            }

            DialogResult hoi = MessageBox.Show(
                $"Phát sự kiện '{AutomationConst.Trigger.TenHienThi(trigger)}' cho lớp '{maLop}'?\n\n" +
                "Các Rule đang hoạt động có trigger tương ứng sẽ chạy hành động thật.",
                "Xác nhận phát sự kiện thử", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (hoi != DialogResult.Yes)
                return;

            try
            {
                UseWaitCursor = true;
                btnPhat.Enabled = false;

                var publisher = new AutomationEventPublisher();

                // Token duy nhất cho mỗi lần phát: cùng token sẽ bị dedup chặn (tránh chạy trùng).
                string token = $"TEST:{trigger}:{maLop}:{Guid.NewGuid():N}";

                await publisher.PhatSuKienAsync(
                    trigger,
                    AutomationConst.EntityType.Class,
                    maLop,
                    token,
                    context: null,
                    triggeredBy: CurrentUser.TenDangNhap);

                lblKetQua.ForeColor = Color.FromArgb(24, 128, 56);
                lblKetQua.Text = "Đã phát sự kiện. Xem tab \"Lần chạy gần đây\" để biết kết quả.";
            }
            catch (Exception ex)
            {
                lblKetQua.ForeColor = Color.FromArgb(192, 57, 43);
                lblKetQua.Text = "Không phát được sự kiện: " + ex.Message;
            }
            finally
            {
                btnPhat.Enabled = true;
                UseWaitCursor = false;
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
