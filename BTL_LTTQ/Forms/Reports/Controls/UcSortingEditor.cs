using BTL_LTTQ.BUS.Reports;
using BTL_LTTQ.Common.Reports;
using BTL_LTTQ.Models.Reports;
using System.Collections;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.Reports.Controls
{
    /// <summary>Trình biên tập tiêu chí sắp xếp (trường + hướng tăng/giảm).</summary>
    public sealed class UcSortingEditor : UcListEditorBase
    {
        private const string CotTruong = "cotTruong";
        private const string CotHuong = "cotHuong";

        public UcSortingEditor(ReportEditorState trangThai) : base(trangThai, "Thêm tiêu chí")
        {
        }

        protected override IList DanhSach => TrangThai.DinhNghia.SapXep;

        protected override void DungCot(DataGridView g)
        {
            g.Columns.Add(new DataGridViewComboBoxColumn
            {
                Name = CotTruong,
                HeaderText = "Trường sắp xếp",
                ValueType = typeof(string),
                DisplayMember = nameof(LuaChon<string>.Nhan),
                ValueMember = nameof(LuaChon<string>.GiaTri),
                FlatStyle = FlatStyle.Flat,
                FillWeight = 300,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton
            });

            g.Columns.Add(CotChon(CotHuong, "Hướng", LuaChonCua(
                new[] { HuongSapXep.TangDan, HuongSapXep.GiamDan }, ReportEnumText.Ten), 120));
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
            return new ReportSortDef
            {
                MaTruong = nguon?.Truong.Count > 0 ? nguon.Truong[0].Ma : "",
                Huong = HuongSapXep.TangDan
            };
        }

        protected override void DongVaoBang(DataGridViewRow row, object item)
        {
            var sx = (ReportSortDef)item;
            row.Cells[CotTruong].Value = sx.MaTruong;
            row.Cells[CotHuong].Value = sx.Huong;
        }

        protected override void DocDong(DataGridViewRow row)
        {
            var sx = (ReportSortDef)row.Tag!;
            sx.MaTruong = Convert.ToString(row.Cells[CotTruong].Value) ?? sx.MaTruong;
            sx.Huong = row.Cells[CotHuong].Value is HuongSapXep h ? h : HuongSapXep.TangDan;
        }
    }
}
