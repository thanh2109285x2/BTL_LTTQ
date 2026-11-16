using System;
using System.Drawing;
using System.Windows.Forms;

namespace BTL_LTTQ.Common.Reports
{
    /// <summary>
    /// Nhà máy tạo control đã định dạng sẵn cho module Report Builder.
    /// Giữ phong cách thống nhất với <see cref="GiaoDien"/> nhưng bổ sung
    /// các dạng thẻ, tiêu đề mục, ô tìm kiếm và lưới dữ liệu riêng của module.
    /// </summary>
    public static class ReportUiFactory
    {
        // ---------------------------------------------------------------- Nhãn

        public static Label TieuDeTrang(string text) => new()
        {
            Text = text,
            Font = ReportDesignTokens.ChuTieuDeTrang,
            ForeColor = ReportDesignTokens.ChuChinh,
            AutoSize = true
        };

        public static Label PhuDeTrang(string text) => new()
        {
            Text = text,
            Font = ReportDesignTokens.ChuPhuDeTrang,
            ForeColor = ReportDesignTokens.ChuPhu,
            AutoSize = true
        };

        public static Label TieuDeMuc(string text) => new()
        {
            Text = text,
            Font = ReportDesignTokens.ChuTieuDeMuc,
            ForeColor = ReportDesignTokens.ChuChinh,
            AutoSize = true,
            Margin = new Padding(0, 0, 0, ReportDesignTokens.Khoang4)
        };

        public static Label Nhan(string text, bool dam = false) => new()
        {
            Text = text,
            Font = dam ? ReportDesignTokens.ChuNhan : ReportDesignTokens.ChuThuong,
            ForeColor = dam ? ReportDesignTokens.ChuChinh : ReportDesignTokens.ChuPhu,
            AutoSize = true,
            TextAlign = ContentAlignment.MiddleLeft
        };

        public static Label NhanMo(string text) => new()
        {
            Text = text,
            Font = ReportDesignTokens.ChuNho,
            ForeColor = ReportDesignTokens.ChuMo,
            AutoSize = true
        };

        /// <summary>Nhãn dạng "badge" nền màu nhạt, chữ đậm.</summary>
        public static Label Badge(string text, Color? mauChu = null, Color? mauNen = null)
        {
            Color chu = mauChu ?? ReportDesignTokens.Nhan;
            Color nen = mauNen ?? ReportDesignTokens.NhanNhat;
            var lbl = new Label
            {
                Text = "  " + text + "  ",
                Font = ReportDesignTokens.ChuNho,
                ForeColor = chu,
                BackColor = nen,
                AutoSize = true,
                Padding = new Padding(4, 2, 4, 2),
                TextAlign = ContentAlignment.MiddleCenter
            };
            return lbl;
        }

        /// <summary>Đường kẻ ngang 1px.</summary>
        public static Panel DuongKe(bool doc = false) => new()
        {
            BackColor = ReportDesignTokens.VienNhat,
            Height = doc ? 0 : 1,
            Width = doc ? 1 : 0,
            Dock = doc ? DockStyle.Left : DockStyle.Top
        };

        // ---------------------------------------------------------------- Nút

        public static AppButton Nut(string text, KieuNut kieu = KieuNut.Phu,
            int rong = ReportDesignTokens.ChieuRongNut, int cao = ReportDesignTokens.ChieuCaoNut)
        {
            return new AppButton
            {
                Text = text,
                Kieu = kieu,
                Width = rong,
                Height = cao,
                Font = ReportDesignTokens.ChuThuong,
                Margin = new Padding(0, 0, ReportDesignTokens.Khoang8, 0)
            };
        }

        // ------------------------------------------------------- Ô nhập / chọn

        public static TextBox OTimKiem(string placeholder, int rong = 260)
        {
            return new TextBox
            {
                PlaceholderText = placeholder,
                Font = ReportDesignTokens.ChuThuong,
                ForeColor = ReportDesignTokens.ChuChinh,
                BorderStyle = BorderStyle.FixedSingle,
                Width = rong,
                Height = 30,
                Margin = new Padding(0)
            };
        }

        public static TextBox OChu(string text = "", int rong = 200)
        {
            return new TextBox
            {
                Text = text,
                Font = ReportDesignTokens.ChuThuong,
                ForeColor = ReportDesignTokens.ChuChinh,
                BorderStyle = BorderStyle.FixedSingle,
                Width = rong,
                Height = 28
            };
        }

        public static ComboBox OChon(int rong = 200)
        {
            return new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = ReportDesignTokens.ChuThuong,
                ForeColor = ReportDesignTokens.ChuChinh,
                FlatStyle = FlatStyle.Flat,
                Width = rong,
                Height = 28
            };
        }

        public static CheckBox OChonHop(string text, bool check = false)
        {
            return new CheckBox
            {
                Text = text,
                Checked = check,
                Font = ReportDesignTokens.ChuThuong,
                ForeColor = ReportDesignTokens.ChuChinh,
                AutoSize = true,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
        }

        public static NumericUpDown OSo(decimal min, decimal max, decimal value, int rong = 100)
        {
            return new NumericUpDown
            {
                Minimum = min,
                Maximum = max,
                Value = Math.Min(Math.Max(value, min), max),
                Font = ReportDesignTokens.ChuThuong,
                ForeColor = ReportDesignTokens.ChuChinh,
                BorderStyle = BorderStyle.FixedSingle,
                Width = rong,
                Height = 28,
                TextAlign = HorizontalAlignment.Right
            };
        }

        // ---------------------------------------------------------------- Lưới

        /// <summary>Lưới dữ liệu đã định dạng theo hệ thống (bản công khai của GiaoDien.DinhDangBang).</summary>
        public static DataGridView Luoi()
        {
            var g = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = ReportDesignTokens.Vien,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                EnableHeadersVisualStyles = false,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                ColumnHeadersHeight = 36,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AutoGenerateColumns = true,
                RowTemplate = { Height = 30 }
            };

            g.ColumnHeadersDefaultCellStyle.BackColor = ReportDesignTokens.Nhan;
            g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            g.ColumnHeadersDefaultCellStyle.SelectionBackColor = ReportDesignTokens.Nhan;
            g.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            g.ColumnHeadersDefaultCellStyle.Font = ReportDesignTokens.ChuNhan;
            g.ColumnHeadersDefaultCellStyle.Padding = new Padding(6, 0, 6, 0);

            g.DefaultCellStyle.ForeColor = ReportDesignTokens.ChuChinh;
            g.DefaultCellStyle.SelectionBackColor = ReportDesignTokens.NenChon;
            g.DefaultCellStyle.SelectionForeColor = ReportDesignTokens.ChuChinh;
            g.DefaultCellStyle.Font = ReportDesignTokens.ChuThuong;
            g.DefaultCellStyle.Padding = new Padding(6, 0, 6, 0);

            g.AlternatingRowsDefaultCellStyle.BackColor = ReportDesignTokens.NhanNhat;
            g.AlternatingRowsDefaultCellStyle.SelectionBackColor = ReportDesignTokens.NenChon;
            g.AlternatingRowsDefaultCellStyle.SelectionForeColor = ReportDesignTokens.ChuChinh;

            return g;
        }

        /// <summary>Bảng chỉ đọc, không cho chọn dòng (dùng cho lưới xem trước).</summary>
        public static DataGridView LuoiXemTruoc()
        {
            var g = Luoi();
            g.SelectionMode = DataGridViewSelectionMode.CellSelect;
            g.AllowUserToResizeColumns = false;
            g.MultiSelect = false;
            g.DefaultCellStyle.SelectionBackColor = Color.White;
            g.DefaultCellStyle.SelectionForeColor = ReportDesignTokens.ChuChinh;
            g.AlternatingRowsDefaultCellStyle.SelectionBackColor = ReportDesignTokens.NhanNhat;
            g.AlternatingRowsDefaultCellStyle.SelectionForeColor = ReportDesignTokens.ChuChinh;
            return g;
        }

        // -------------------------------------------------------------- Khung

        /// <summary>Panel cuộn dọc, nền trong suốt theo nền chung.</summary>
        public static Panel KhungCuon()
        {
            return new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = ReportDesignTokens.NenPanel
            };
        }

        public static ReportCard The(string tieuDe = "", string phuDe = "") => new(tieuDe, phuDe);

        /// <summary>Bọc control vào một ô có nhãn phía trên (dùng cho biểu mẫu thuộc tính).</summary>
        public static Panel OCoNhan(string nhan, Control control, int rong = 0)
        {
            var hop = new Panel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Margin = new Padding(0, 0, ReportDesignTokens.Khoang8, ReportDesignTokens.Khoang8)
            };

            var lbl = Nhan(nhan);
            lbl.Location = new Point(0, 0);

            control.Location = new Point(0, 22);
            if (rong > 0)
                control.Width = rong;

            hop.Controls.Add(control);
            hop.Controls.Add(lbl);
            hop.Height = control.Bottom + 2;
            hop.Width = Math.Max(control.Width, lbl.Width) + 2;
            return hop;
        }
    }
}
