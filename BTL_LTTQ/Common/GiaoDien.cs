using System.Drawing;
using System.Windows.Forms;

namespace BTL_LTTQ
{
    // Bảng màu đơn giản dùng chung cho cả ứng dụng.
    // Muốn đổi màu cả app: chỉ cần sửa các hằng màu ngay dưới đây.
    public static class GiaoDien
    {
        public static readonly Color MauChinh = Color.FromArgb(41, 98, 168);    // xanh dương (nút chính, tiêu đề bảng)
        public static readonly Color MauChinhDam = Color.FromArgb(30, 74, 130);  // xanh đậm khi rê chuột
        public static readonly Color MauNguyHiem = Color.FromArgb(192, 57, 43);  // đỏ (nút Xóa)
        public static readonly Color MauNguyHiemDam = Color.FromArgb(150, 40, 30);
        public static readonly Color MauNen = Color.FromArgb(244, 247, 251);  // nền form
        public static readonly Color MauHangLe = Color.FromArgb(234, 241, 250);  // dòng xen kẽ trong bảng
        public static readonly Color MauChon = Color.FromArgb(200, 220, 245);  // dòng đang chọn
        public static readonly Color MauVien = Color.FromArgb(200, 208, 220);  // viền nhạt
        public static readonly Color MauChu = Color.FromArgb(33, 37, 41);     // chữ
        public static readonly Color MauMucCha = Color.FromArgb(221, 227, 236);  // nền mục cha trong menu (xám xanh)
        public static readonly Color MauMucChaHover = Color.FromArgb(205, 214, 228);

        // Dùng cho các form con: lớp học, tìm kiếm...
        // Gọi sau InitializeComponent();
        public static void ApDung(Form f)
        {
            f.BackColor = MauNen;
            Duyet(f);
        }

        // Dùng riêng cho form Main (menu bên trái)
        public static void ApDungMain(Form f)
        {
            f.BackColor = MauNen;
            DuyetMain(f);
        }

        private static void Duyet(Control cha)
        {
            foreach (Control c in cha.Controls)
            {
                if (c is Button b)
                {
                    DinhDangNut(b);
                }
                else if (c is DataGridView g)
                {
                    DinhDangBang(g);
                    continue;   // không cần duyệt control con bên trong bảng
                }
                else if (c is GroupBox gb)
                {
                    gb.ForeColor = MauChinh;
                }
                else if (c is Label || c is TextBoxBase || c is ComboBox ||
                         c is NumericUpDown || c is DateTimePicker)
                {
                    // đặt lại màu chữ vì GroupBox ở trên làm control con bị đổi sang xanh
                    c.ForeColor = MauChu;
                }

                if (c.HasChildren) Duyet(c);
            }
        }

        private static void DinhDangNut(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.UseVisualStyleBackColor = false;
            b.Cursor = Cursors.Hand;

            Color nen, chu, vien, hover;

            switch (b.Name)
            {
                case "btnThem":
                case "btnSua":
                case "btnTimKiem":
                    // nút chính: nền xanh, chữ trắng
                    nen = MauChinh; chu = Color.White; vien = MauChinh; hover = MauChinhDam;
                    break;
                case "btnXoa":
                    // nút xóa: nền đỏ, chữ trắng
                    nen = MauNguyHiem; chu = Color.White; vien = MauNguyHiem; hover = MauNguyHiemDam;
                    break;
                default:
                    // nút phụ (Làm mới, thu gọn...): nền trắng, viền xám
                    nen = Color.White; chu = MauChu; vien = MauVien; hover = MauHangLe;
                    break;
            }

            b.BackColor = nen;
            b.ForeColor = chu;
            b.FlatAppearance.BorderColor = vien;
            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.MouseOverBackColor = hover;
            b.FlatAppearance.MouseDownBackColor = hover;
        }

        private static void DinhDangBang(DataGridView g)
        {
            g.BackgroundColor = Color.White;
            g.BorderStyle = BorderStyle.None;
            g.GridColor = MauVien;
            g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            g.EnableHeadersVisualStyles = false;
            g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            g.ColumnHeadersHeight = 34;
            g.ColumnHeadersDefaultCellStyle.BackColor = MauChinh;
            g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            g.ColumnHeadersDefaultCellStyle.SelectionBackColor = MauChinh;
            g.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            g.ColumnHeadersDefaultCellStyle.Font = new Font(g.Font, FontStyle.Bold);

            g.DefaultCellStyle.ForeColor = MauChu;
            g.DefaultCellStyle.SelectionBackColor = MauChon;
            g.DefaultCellStyle.SelectionForeColor = MauChu;
            g.AlternatingRowsDefaultCellStyle.BackColor = MauHangLe;
            g.AlternatingRowsDefaultCellStyle.SelectionBackColor = MauChon;
            g.AlternatingRowsDefaultCellStyle.SelectionForeColor = MauChu;
            g.RowTemplate.Height = 28;

            g.RowHeadersVisible = false;
            g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            g.MultiSelect = false;
            g.AllowUserToAddRows = false;
            g.AllowUserToDeleteRows = false;
            g.ReadOnly = true;
            g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // ===== Form Main: menu bên trái =====
        // Nút menu giãn hết chiều ngang khung chứa, chữ canh trái, không viền,
        // rê chuột thì đổi màu. Mục cha (có icon) in đậm, mục con thụt vào.

        private static Button _mucDangChon;

        // Gọi trong các hàm click của mục con (Lớp học, Tìm lớp...) để tô màu mục đang mở
        public static void DanhDauMuc(Button b)
        {
            if (_mucDangChon != null && !_mucDangChon.IsDisposed)
            {
                _mucDangChon.BackColor = MauNen;
                _mucDangChon.ForeColor = MauChu;
            }
            _mucDangChon = b;
            b.BackColor = MauChon;
            b.ForeColor = MauChinh;
        }

        private static void DuyetMain(Control cha)
        {
            foreach (Control c in cha.Controls)
            {
                if (c is Button b)
                {
                    DinhDangNutMenu(b);
                }

                if (c.HasChildren) DuyetMain(c);
            }
        }

        // Mục cha = nút có icon (Image) hoặc có icon/emoji ở đầu chữ, ví dụ "🏠 Trang chủ"
        private static bool LaMucCha(Button b)
        {
            if (b.Image != null || b.ImageList != null) return true;
            string t = b.Text.TrimStart();
            return t.Length > 0 && !char.IsLetterOrDigit(t[0]);
        }

        private static void DinhDangNutMenu(Button b)
        {
            bool laMucCha = LaMucCha(b);   // Trang chủ, Quản lý, Tra cứu, Báo cáo
            bool laThoat = b.Text.Trim() == "Thoát";

            b.FlatStyle = FlatStyle.Flat;
            b.UseVisualStyleBackColor = false;
            b.Cursor = Cursors.Hand;
            b.FlatAppearance.BorderSize = 0;   // bỏ viền cho đỡ rối mắt

            if (laThoat)
            {
                b.BackColor = MauNen;
                b.ForeColor = MauNguyHiem;
                b.TextAlign = ContentAlignment.MiddleCenter;
                b.FlatAppearance.MouseOverBackColor = Color.FromArgb(250, 225, 222);
                b.FlatAppearance.MouseDownBackColor = Color.FromArgb(250, 225, 222);
            }
            else if (laMucCha)
            {
                // mục cha: nền xám xanh đậm hơn, chữ in đậm
                b.BackColor = MauMucCha;
                b.ForeColor = MauChu;
                b.Font = new Font(b.Font, FontStyle.Bold);
                b.TextAlign = ContentAlignment.MiddleLeft;
                b.ImageAlign = ContentAlignment.MiddleLeft;
                b.TextImageRelation = TextImageRelation.ImageBeforeText;
                b.Padding = new Padding(8, 0, 0, 0);
                b.FlatAppearance.MouseOverBackColor = MauMucChaHover;
                b.FlatAppearance.MouseDownBackColor = MauMucChaHover;
            }
            else
            {
                // mục con: nền nhạt, không viền, thụt vào sâu hơn mục cha
                b.BackColor = MauNen;
                b.ForeColor = MauChu;
                b.TextAlign = ContentAlignment.MiddleLeft;
                b.Padding = new Padding(48, 0, 0, 0);
                b.FlatAppearance.MouseOverBackColor = MauChon;
                b.FlatAppearance.MouseDownBackColor = MauChon;
            }

            // giãn hết chiều ngang khung chứa, và tự giãn theo khi đổi kích thước form
            if (b.Parent != null)
            {
                b.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                b.Left = 6;
                b.Width = b.Parent.ClientSize.Width - 12;
            }
        }
    }
}