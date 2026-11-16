using BTL_LTTQ.BUS.Reports;
using BTL_LTTQ.Common.Reports;
using BTL_LTTQ.Models.Reports;
using System;
using System.Collections;
using System.Linq;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.Reports.Controls
{
    /// <summary>
    /// Trình biên tập điều kiện lọc. Toán tử được giới hạn theo kiểu dữ liệu của trường,
    /// và ô "Giá trị 2" chỉ bật khi toán tử cần khoảng giá trị.
    /// </summary>
    public sealed class UcFiltersEditor : UcListEditorBase
    {
        private const string CotBat = "cotBat";
        private const string CotTruong = "cotTruong";
        private const string CotToanTu = "cotToanTu";
        private const string CotGiaTri1 = "cotGiaTri1";
        private const string CotGiaTri2 = "cotGiaTri2";
        private const string CotLienKet = "cotLienKet";

        private bool _dangDoi;

        public UcFiltersEditor(ReportEditorState trangThai) : base(trangThai, "Thêm điều kiện")
        {
        }

        protected override IList DanhSach => TrangThai.DinhNghia.Loc;

        protected override void DungCot(DataGridView g)
        {
            g.Columns.Add(CotChonHop(CotBat, "Bật", 50));

            g.Columns.Add(new DataGridViewComboBoxColumn
            {
                Name = CotTruong,
                HeaderText = "Trường",
                ValueType = typeof(string),
                DisplayMember = nameof(LuaChon<string>.Nhan),
                ValueMember = nameof(LuaChon<string>.GiaTri),
                FlatStyle = FlatStyle.Flat,
                FillWeight = 190,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton
            });

            g.Columns.Add(new DataGridViewComboBoxColumn
            {
                Name = CotToanTu,
                HeaderText = "Toán tử",
                ValueType = typeof(ToanTuLoc),
                FlatStyle = FlatStyle.Flat,
                FillWeight = 140,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton
            });

            g.Columns.Add(CotChu(CotGiaTri1, "Giá trị 1", 160));
            g.Columns.Add(CotChu(CotGiaTri2, "Giá trị 2", 160));

            g.Columns.Add(CotChon(CotLienKet, "Liên kết", LuaChonCua(
                new[] { LienKetDieuKien.Va, LienKetDieuKien.Hoac }, ReportEnumText.Ten), 90));
        }

        public override void Nap()
        {
            Luoi.Rows.Clear();
            if (Luoi.Columns[CotTruong] is DataGridViewComboBoxColumn cot)
                cot.DataSource = LuaChonTruong();
            DungLai();
        }

        protected override object TaoMoi()
        {
            var nguon = TrangThai.Nguon;
            return new ReportFilterDef
            {
                MaTruong = nguon?.Truong.Count > 0 ? nguon.Truong[0].Ma : "",
                ToanTu = ToanTuLoc.Bang,
                Bat = true,
                LienKet = LienKetDieuKien.Va
            };
        }

        protected override void DongVaoBang(DataGridViewRow row, object item)
        {
            var loc = (ReportFilterDef)item;
            row.Cells[CotBat].Value = loc.Bat;
            row.Cells[CotTruong].Value = loc.MaTruong;
            DatToanTu(row, loc);
            row.Cells[CotGiaTri1].Value = loc.GiaTri1;
            row.Cells[CotGiaTri2].Value = loc.GiaTri2;
            row.Cells[CotLienKet].Value = loc.LienKet;
            CapNhatGoiY(row, loc);
        }

        protected override void DocDong(DataGridViewRow row)
        {
            if (_dangDoi)
                return;

            var loc = (ReportFilterDef)row.Tag!;
            loc.Bat = Convert.ToBoolean(row.Cells[CotBat].Value ?? false);
            loc.MaTruong = Convert.ToString(row.Cells[CotTruong].Value) ?? loc.MaTruong;
            if (row.Cells[CotToanTu].Value is ToanTuLoc tt)
                loc.ToanTu = tt;
            loc.GiaTri1 = Convert.ToString(row.Cells[CotGiaTri1].Value) ?? "";
            loc.GiaTri2 = Convert.ToString(row.Cells[CotGiaTri2].Value) ?? "";
            loc.LienKet = row.Cells[CotLienKet].Value is LienKetDieuKien lk ? lk : LienKetDieuKien.Va;
        }

        protected override void SauKhiDoc(DataGridViewRow row, int cot)
        {
            if (_dangDoi)
                return;

            _dangDoi = true;
            try
            {
                var loc = (ReportFilterDef)row.Tag!;

                if (cot == Luoi.Columns[CotTruong].Index)
                {
                    DatToanTu(row, loc);
                    CapNhatGoiY(row, loc);
                }
                else if (cot == Luoi.Columns[CotToanTu].Index)
                {
                    CapNhatGoiY(row, loc);
                }
            }
            finally
            {
                _dangDoi = false;
            }
        }

        private void DatToanTu(DataGridViewRow row, ReportFilterDef loc)
        {
            var truong = TrangThai.Nguon?.TimTruong(loc.MaTruong);
            var kieu = truong?.Kieu ?? KieuDuLieu.Chuoi;
            var ds = LuaChonCua(ReportEnumText.ToanTuCho(kieu), ReportEnumText.Ten);

            if (!ds.Any(o => Equals(o.GiaTri, loc.ToanTu)))
                loc.ToanTu = ds[0].GiaTri;

            var cell = new DataGridViewComboBoxCell
            {
                DataSource = ds,
                DisplayMember = nameof(LuaChon<ToanTuLoc>.Nhan),
                ValueMember = nameof(LuaChon<ToanTuLoc>.GiaTri),
                FlatStyle = FlatStyle.Flat,
                DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
                Value = loc.ToanTu
            };

            int idx = Luoi.Columns[CotToanTu].Index;
            row.Cells[idx] = cell;
        }

        private void CapNhatGoiY(DataGridViewRow row, ReportFilterDef loc)
        {
            bool canHai = ReportEnumText.CanHaiGiaTri(loc.ToanTu);
            bool laDanhSach = ReportEnumText.LaDanhSach(loc.ToanTu);
            bool laLike = ReportEnumText.LaLike(loc.ToanTu);

            row.Cells[CotGiaTri2].ReadOnly = !canHai;
            row.Cells[CotGiaTri2].Style.BackColor = canHai
                ? ReportDesignTokens.NenTrang
                : ReportDesignTokens.NenPanel;

            row.Cells[CotGiaTri1].ToolTipText = laDanhSach
                ? "Nhập nhiều giá trị, phân tách bằng dấu phẩy."
                : laLike ? "Có thể nhập một phần nội dung cần tìm." : "";
            row.Cells[CotGiaTri2].ToolTipText = canHai ? "Giá trị cận trên của khoảng." : "";
        }
    }
}
