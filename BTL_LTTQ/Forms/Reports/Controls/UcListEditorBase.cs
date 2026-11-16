using BTL_LTTQ.BUS.Reports;
using BTL_LTTQ.Common.Reports;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.Reports.Controls
{
    /// <summary>
    /// Lớp nền cho các trình biên tập dạng danh sách (cột / lọc / sắp xếp / nhóm / tổng hợp).
    /// Cung cấp thanh công cụ Thêm – Xóa – Lên – Xuống, lưới chỉnh sửa trực tiếp,
    /// trạng thái trống và sự kiện <see cref="ThayDoi"/> khi cấu hình bị sửa.
    /// </summary>
    public abstract class UcListEditorBase : UserControl
    {
        protected readonly ReportEditorState TrangThai;

        private readonly Panel _thanhCongCu;
        private readonly Label _lblTrong;
        private readonly AppButton _btnThem;
        private readonly AppButton _btnXoa;
        private readonly AppButton _btnLen;
        private readonly AppButton _btnXuong;
        private bool _dangNap;

        /// <summary>Phát sinh khi danh sách cấu hình thay đổi (thêm/xóa/sửa/đổi thứ tự).</summary>
        public event EventHandler? ThayDoi;

        protected UcListEditorBase(ReportEditorState trangThai, string nhanThem = "Thêm dòng")
        {
            TrangThai = trangThai ?? throw new ArgumentNullException(nameof(trangThai));
            Dock = DockStyle.Fill;
            BackColor = ReportDesignTokens.NenTrang;
            Font = ReportDesignTokens.ChuThuong;

            _thanhCongCu = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = ReportDesignTokens.NenPanel
            };

            _btnThem = ReportUiFactory.Nut(nhanThem, KieuNut.Chinh, 124);
            _btnXoa = ReportUiFactory.Nut("Xóa dòng", KieuNut.NguyHiem, 96);
            _btnLen = ReportUiFactory.Nut("↑ Lên", KieuNut.Phu, 72);
            _btnXuong = ReportUiFactory.Nut("↓ Xuống", KieuNut.Phu, 84);

            _btnThem.Click += (_, _) => ThemDong();
            _btnXoa.Click += (_, _) => XoaDong();
            _btnLen.Click += (_, _) => DiChuyenDong(-1);
            _btnXuong.Click += (_, _) => DiChuyenDong(1);

            _thanhCongCu.Controls.AddRange(new Control[] { _btnThem, _btnXoa, _btnLen, _btnXuong });
            _thanhCongCu.Resize += (_, _) => BoTriThanhCongCu();
            BoTriThanhCongCu();

            Luoi = ReportUiFactory.Luoi();
            Luoi.ReadOnly = false;
            Luoi.AutoGenerateColumns = false;
            Luoi.EditMode = DataGridViewEditMode.EditOnEnter;
            Luoi.AllowUserToOrderColumns = false;
            Luoi.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Luoi.CellValueChanged += Luoi_CellValueChanged;
            Luoi.CurrentCellDirtyStateChanged += Luoi_CurrentCellDirtyStateChanged;
            Luoi.DataError += (_, e) => e.ThrowException = false;
            Luoi.SelectionChanged += (_, _) => CapNhatNut();
            Luoi.RowsAdded += (_, _) => CapNhatTrong();
            Luoi.RowsRemoved += (_, _) => CapNhatTrong();

            _lblTrong = new Label
            {
                Dock = DockStyle.Fill,
                Text = "Chưa có dòng nào.\nNhấn \"" + nhanThem + "\" để bắt đầu.",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = ReportDesignTokens.ChuThuong,
                ForeColor = ReportDesignTokens.ChuMo,
                BackColor = ReportDesignTokens.NenTrang,
                Visible = false
            };

            var khung = new Panel { Dock = DockStyle.Fill, BackColor = ReportDesignTokens.NenTrang };
            khung.Controls.Add(_lblTrong);
            khung.Controls.Add(Luoi);

            Controls.Add(khung);
            Controls.Add(_thanhCongCu);

            DungCot(Luoi);
        }

        protected DataGridView Luoi { get; }

        // ---------------------------------------------------------- Thành phần trừu tượng

        /// <summary>Cấu hình các cột cho lưới (chỉ gọi một lần trong hàm dựng).</summary>
        protected abstract void DungCot(DataGridView g);

        /// <summary>Danh sách cấu hình thật (List&lt;T&gt; luôn hiện thực IList).</summary>
        protected abstract IList DanhSach { get; }

        /// <summary>Tạo một dòng cấu hình mới (chưa thêm vào danh sách).</summary>
        protected abstract object TaoMoi();

        /// <summary>Đổ dữ liệu của một mục cấu hình lên các ô của dòng.</summary>
        protected abstract void DongVaoBang(DataGridViewRow row, object item);

        /// <summary>Đọc giá trị các ô của dòng vào mục cấu hình (row.Tag).</summary>
        protected abstract void DocDong(DataGridViewRow row);

        /// <summary>Hook sau khi đọc dòng, dùng khi cần cấu hình lại ô phụ thuộc.</summary>
        protected virtual void SauKhiDoc(DataGridViewRow row, int cot)
        {
        }

        // ---------------------------------------------------------- Nạp / thao tác

        /// <summary>Nạp lại toàn bộ lưới từ danh sách cấu hình.</summary>
        public virtual void Nap() => DungLai();

        /// <summary>Dựng lại các dòng của lưới (giữ nguyên cấu hình cột).</summary>
        protected void DungLai()
        {
            _dangNap = true;
            try
            {
                int chon = Luoi.CurrentRow?.Index ?? -1;
                Luoi.Rows.Clear();
                foreach (object item in DanhSach)
                {
                    int idx = Luoi.Rows.Add();
                    var row = Luoi.Rows[idx];
                    row.Tag = item;
                    DongVaoBang(row, item);
                }

                if (chon >= 0 && chon < Luoi.Rows.Count)
                    Luoi.CurrentCell = Luoi.Rows[chon].Cells[0];
            }
            finally
            {
                _dangNap = false;
            }

            CapNhatTrong();
            CapNhatNut();
        }

        private void ThemDong()
        {
            object item = TaoMoi();
            DanhSach.Add(item);
            Nap();
            if (Luoi.Rows.Count > 0)
            {
                Luoi.CurrentCell = Luoi.Rows[Luoi.Rows.Count - 1].Cells[0];
                Luoi.Focus();
            }
            BaoThayDoi();
        }

        private void XoaDong()
        {
            int idx = Luoi.CurrentRow?.Index ?? -1;
            if (idx < 0 || idx >= DanhSach.Count)
                return;

            DanhSach.RemoveAt(idx);
            Nap();
            if (Luoi.Rows.Count > 0)
                Luoi.CurrentCell = Luoi.Rows[Math.Min(idx, Luoi.Rows.Count - 1)].Cells[0];
            BaoThayDoi();
        }

        private void DiChuyenDong(int huong)
        {
            int idx = Luoi.CurrentRow?.Index ?? -1;
            int moi = idx + huong;
            if (idx < 0 || moi < 0 || moi >= DanhSach.Count)
                return;

            object item = DanhSach[idx]!;
            DanhSach.RemoveAt(idx);
            DanhSach.Insert(moi, item);
            Nap();
            Luoi.CurrentCell = Luoi.Rows[moi].Cells[0];
            BaoThayDoi();
        }

        private void Luoi_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (_dangNap || e.RowIndex < 0 || e.RowIndex >= Luoi.Rows.Count)
                return;

            var row = Luoi.Rows[e.RowIndex];
            DocDong(row);
            SauKhiDoc(row, e.ColumnIndex);
            BaoThayDoi();
        }

        private void Luoi_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
        {
            if (Luoi.IsCurrentCellDirty)
                Luoi.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        protected void BaoThayDoi() => ThayDoi?.Invoke(this, EventArgs.Empty);

        private void CapNhatTrong()
        {
            bool trong = Luoi.Rows.Count == 0;
            _lblTrong.Visible = trong;
            if (trong)
                _lblTrong.BringToFront();
        }

        private void CapNhatNut()
        {
            int idx = Luoi.CurrentRow?.Index ?? -1;
            int tong = DanhSach.Count;
            _btnXoa.Enabled = idx >= 0 && idx < tong;
            _btnLen.Enabled = idx > 0 && idx < tong;
            _btnXuong.Enabled = idx >= 0 && idx < tong - 1;
        }

        private void BoTriThanhCongCu()
        {
            const int y = 8;
            int x = ReportDesignTokens.Khoang8;

            _btnThem.Location = new Point(x, y);
            x += _btnThem.Width + ReportDesignTokens.Khoang8;
            _btnXoa.Location = new Point(x, y);

            int xPhai = _thanhCongCu.ClientSize.Width - ReportDesignTokens.Khoang8 - _btnXuong.Width;
            _btnXuong.Location = new Point(Math.Max(x + ReportDesignTokens.Khoang16, xPhai), y);
            _btnLen.Location = new Point(_btnXuong.Left - ReportDesignTokens.Khoang8 - _btnLen.Width, y);
        }

        // ---------------------------------------------------------- Tiện ích dựng cột

        protected static DataGridViewTextBoxColumn CotChu(string ten, string tieuDe, int fillWeight,
            bool chiDoc = false)
        {
            return new DataGridViewTextBoxColumn
            {
                Name = ten,
                HeaderText = tieuDe,
                FillWeight = fillWeight,
                ReadOnly = chiDoc,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft }
            };
        }

        protected static DataGridViewCheckBoxColumn CotChonHop(string ten, string tieuDe, int fillWeight)
        {
            return new DataGridViewCheckBoxColumn
            {
                Name = ten,
                HeaderText = tieuDe,
                FillWeight = fillWeight,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                FlatStyle = FlatStyle.Flat
            };
        }

        protected static DataGridViewComboBoxColumn CotChon<T>(string ten, string tieuDe,
            IEnumerable<LuaChon<T>> luaChon, int fillWeight)
        {
            return new DataGridViewComboBoxColumn
            {
                Name = ten,
                HeaderText = tieuDe,
                DataSource = luaChon.ToList(),
                DisplayMember = nameof(LuaChon<T>.Nhan),
                ValueMember = nameof(LuaChon<T>.GiaTri),
                ValueType = typeof(T),
                FlatStyle = FlatStyle.Flat,
                FillWeight = fillWeight,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton
            };
        }

        /// <summary>Danh sách lựa chọn cho một enum kèm hàm chuyển nhãn.</summary>
        protected static List<LuaChon<T>> LuaChonCua<T>(IEnumerable<T> giaTri, Func<T, string> nhan) =>
            giaTri.Select(v => new LuaChon<T>(v, nhan(v))).ToList();

        /// <summary>Danh sách trường của nguồn hiện tại (có thể lọc theo điều kiện).</summary>
        protected List<LuaChon<string>> LuaChonTruong(Func<Models.Reports.FieldDefinition, bool>? loc = null)
        {
            var nguon = TrangThai.Nguon;
            if (nguon == null)
                return new List<LuaChon<string>>();

            return nguon.Truong
                .Where(t => loc == null || loc(t))
                .Select(t => new LuaChon<string>(t.Ma, t.TieuDe))
                .ToList();
        }
    }
}
