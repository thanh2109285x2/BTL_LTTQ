using BTL_LTTQ.Services.Ai;
using System.Data;

namespace BTL_LTTQ.Forms
{
    public partial class frmTroLyAi : Form
    {
        private readonly TroLyAiService _troLy = new();

        public frmTroLyAi()
        {
            InitializeComponent();
            GiaoDien.ApDung(this);
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
                ThemTinNhan("Lỗi: " + ex.Message, laNguoiDung: false);
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
                Margin = new Padding(8, 4, 8, 4),
                Tag = laNguoiDung
            };
            var bongBong = new Label
            {
                AutoSize = true,
                MaximumSize = new Size(Math.Max(180, (flpTinNhan.ClientSize.Width - 80) * 2 / 3), 0),
                Padding = new Padding(12, 8, 12, 8),
                Text = noiDung.Trim(),
                BackColor = laNguoiDung ? Color.FromArgb(220, 238, 255) : Color.FromArgb(242, 244, 247),
                ForeColor = Color.FromArgb(35, 40, 48),
                Font = new Font("Segoe UI", 10F),
                BorderStyle = BorderStyle.FixedSingle
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
                        BorderStyle = BorderStyle.FixedSingle,
                        CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                        ColumnHeadersHeight = 36,
                        EnableHeadersVisualStyles = false,
                        Font = new Font("Segoe UI", 9F)
                    };
                    luoi.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(35, 91, 155);
                    luoi.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    luoi.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    luoi.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(244, 247, 251);
                    luoi.DefaultCellStyle.SelectionBackColor = Color.FromArgb(215, 231, 248);
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
