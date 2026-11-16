using BTL_LTTQ.Services.Ai;
using System.Data;
using System.Diagnostics;

namespace BTL_LTTQ.Forms
{
    public partial class frmTroLyAi : Form
    {
        private readonly TroLyAiService _troLy = new();

        public frmTroLyAi()
        {
            InitializeComponent();
            GiaoDien.ApDung(this);
            // GiaoDien.ApDung đặt ForeColor mặc định cho mọi Label.
            lblTieuDe.ForeColor = Color.White;
            lblMoTa.ForeColor = Color.FromArgb(222, 235, 250);
            ThemTinNhan("Chào bạn! Mình có thể giúp gì?", laNguoiDung: false);
        }

        private async Task Gui()
        {
            string cauHoi = txtCauHoi.Text.Trim();
            if (cauHoi.Length == 0 || !btnGui.Enabled)
                return;

            txtCauHoi.Clear();
            ThemTinNhan(cauHoi, laNguoiDung: true);
            btnGui.Enabled = false;
            txtCauHoi.Enabled = false;

            try
            {
                TroLyAiResponse phanHoi = await _troLy.Hoi(cauHoi);
                ThemTinNhan(phanHoi.Text, laNguoiDung: false, phanHoi.Tables);
            }
            catch (Exception ex)
            {
                // Chi tiết kỹ thuật chỉ dành cho người phát triển; không đưa API response/key
                // hay thông tin cấu hình vào hội thoại của giáo viên và học viên.
                Trace.TraceError("Trợ lý AI gặp lỗi: {0}", ex);
                ThemTinNhan("Trợ lý AI hiện chưa thể trả lời. Bạn vui lòng thử lại sau ít phút. Nếu tình trạng tiếp diễn, hãy báo quản trị viên.", laNguoiDung: false);
            }
            finally
            {
                btnGui.Enabled = true;
                txtCauHoi.Enabled = true;
                txtCauHoi.Focus();
            }
        }

        private void ThemTinNhan(string noiDung, bool laNguoiDung, IReadOnlyList<DataTable>? cacBang = null)
        {
            var dong = new Panel
            {
                Width = Math.Max(100, flpTinNhan.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 12),
                Height = 1,
                Margin = new Padding(8, 7, 8, 7),
                Tag = laNguoiDung,
                BackColor = flpTinNhan.BackColor
            };
            var bongBong = new Label
            {
                AutoSize = true,
                MaximumSize = new Size(Math.Max(180, (flpTinNhan.ClientSize.Width - 80) * 2 / 3), 0),
                Padding = new Padding(14, 10, 14, 10),
                Text = noiDung.Trim(),
                BackColor = laNguoiDung ? Color.FromArgb(35, 91, 155) : Color.White,
                ForeColor = laNguoiDung ? Color.White : Color.FromArgb(35, 40, 48),
                Font = new Font("Segoe UI", 10F),
                BorderStyle = BorderStyle.None
            };
            dong.Controls.Add(bongBong);
            int chieuCao = bongBong.PreferredHeight + 8;
            if (!laNguoiDung && cacBang != null)
            {
                foreach (DataTable bang in cacBang)
                {
                    var luoi = new DataGridView
                    {
                        DataSource = bang,
                        Left = 4,
                        Top = chieuCao,
                        Width = Math.Max(160, dong.Width - 12),
                        Height = Math.Min(260, 42 + bang.Rows.Count * 34),
                        Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                        ReadOnly = true,
                        AllowUserToAddRows = false,
                        AllowUserToDeleteRows = false,
                        AllowUserToResizeRows = false,
                        MultiSelect = false,
                        RowHeadersVisible = false,
                        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                        BackgroundColor = Color.White,
                        BorderStyle = BorderStyle.None,
                        CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                        GridColor = Color.FromArgb(229, 234, 241),
                        ColumnHeadersHeight = 40,
                        ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                        EnableHeadersVisualStyles = false,
                        Font = new Font("Segoe UI", 9F)
                    };
                    luoi.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(35, 91, 155);
                    luoi.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    luoi.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    luoi.ColumnHeadersDefaultCellStyle.Padding = new Padding(6, 0, 0, 0);
                    luoi.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(244, 247, 251);
                    luoi.DefaultCellStyle.Padding = new Padding(6, 2, 6, 2);
                    luoi.DefaultCellStyle.ForeColor = Color.FromArgb(45, 53, 64);
                    luoi.DefaultCellStyle.SelectionBackColor = Color.FromArgb(225, 238, 252);
                    luoi.DefaultCellStyle.SelectionForeColor = Color.FromArgb(35, 40, 48);
                    luoi.RowTemplate.Height = 30;
                    dong.Controls.Add(luoi);
                    chieuCao += luoi.Height + 8;
                }
            }
            dong.Height = chieuCao;
            CanChinhTinNhan(dong);
            flpTinNhan.Controls.Add(dong);
            flpTinNhan.ScrollControlIntoView(dong);
        }

        private void CanChinhTinNhan(Panel dong)
        {
            dong.Width = Math.Max(100, flpTinNhan.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 12);
            var bongBong = (Label)dong.Controls[0];
            bongBong.MaximumSize = new Size(Math.Max(180, (dong.Width - 24) * 2 / 3), 0);
            bongBong.Left = (bool)dong.Tag! ? Math.Max(0, dong.Width - bongBong.Width - 4) : 4;
            bongBong.Top = 0;
            int chieuCao = bongBong.PreferredHeight + 8;
            foreach (DataGridView luoi in dong.Controls.OfType<DataGridView>())
            {
                luoi.Left = 4;
                luoi.Top = chieuCao;
                luoi.Width = Math.Max(160, dong.Width - 12);
                chieuCao += luoi.Height + 8;
            }
            dong.Height = chieuCao;
        }

        private void flpTinNhan_SizeChanged(object? sender, EventArgs e)
        {
            foreach (Panel dong in flpTinNhan.Controls.OfType<Panel>())
                CanChinhTinNhan(dong);
        }

        private void panelNhap_Paint(object? sender, PaintEventArgs e)
        {
            using var but = new Pen(Color.FromArgb(224, 230, 239));
            e.Graphics.DrawLine(but, 0, 0, panelNhap.Width, 0);
        }

        private async void btnGui_Click(object? sender, EventArgs e) => await Gui();

        private async void txtCauHoi_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || e.Shift)
                return;
            e.SuppressKeyPress = true;
            await Gui();
        }
    }
}
