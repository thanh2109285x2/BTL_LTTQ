using BTL_LTTQ.BUS.Reports;
using BTL_LTTQ.Common.Reports;
using BTL_LTTQ.Models.Reports;
using System.Collections;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.Reports.Controls
{
    /// <summary>Trình biên tập cấp nhóm dữ liệu (trường nhóm + hiển thị chân nhóm).</summary>
    public sealed class UcGroupingEditor : UcListEditorBase
    {
        private const string CotTruong = "cotTruong";
        private const string CotChanNhom = "cotChanNhom";
        private const string CotTongNhom = "cotTongNhom";

        public UcGroupingEditor(ReportEditorState trangThai) : base(trangThai, "Thêm nhóm")
        {
        }

        protected override IList DanhSach => TrangThai.DinhNghia.Nhom;

        protected override void DungCot(DataGridView g)
        {
            g.Columns.Add(new DataGridViewComboBoxColumn
            {
                Name = CotTruong,
                HeaderText = "Trường nhóm (theo thứ tự cấp)",
                ValueType = typeof(string),
                DisplayMember = nameof(LuaChon<string>.Nhan),
                ValueMember = nameof(LuaChon<string>.GiaTri),
                FlatStyle = FlatStyle.Flat,
                FillWeight = 280,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton
            });

            g.Columns.Add(CotChonHop(CotChanNhom, "Hiện tiêu đề nhóm", 120));
            g.Columns.Add(CotChonHop(CotTongNhom, "Hiện tổng nhóm", 110));
        }

        public override void Nap()
        {
            Luoi.Rows.Clear();
            if (Luoi.Columns[CotTruong] is DataGridViewComboBoxColumn cot)
                cot.DataSource = LuaChonTruong(t => t.CoTheNhom);
            DungLai();
        }

        protected override object TaoMoi()
        {
            var nguon = TrangThai.Nguon;
            var truong = nguon?.Truong.Find(t => t.CoTheNhom);
            return new ReportGroupDef
            {
                MaTruong = truong?.Ma ?? "",
                HienThiChanNhom = true,
                HienThiTongNhom = true
            };
        }

        protected override void DongVaoBang(DataGridViewRow row, object item)
        {
            var nhom = (ReportGroupDef)item;
            row.Cells[CotTruong].Value = nhom.MaTruong;
            row.Cells[CotChanNhom].Value = nhom.HienThiChanNhom;
            row.Cells[CotTongNhom].Value = nhom.HienThiTongNhom;
        }

        protected override void DocDong(DataGridViewRow row)
        {
            var nhom = (ReportGroupDef)row.Tag!;
            nhom.MaTruong = Convert.ToString(row.Cells[CotTruong].Value) ?? nhom.MaTruong;
            nhom.HienThiChanNhom = Convert.ToBoolean(row.Cells[CotChanNhom].Value ?? false);
            nhom.HienThiTongNhom = Convert.ToBoolean(row.Cells[CotTongNhom].Value ?? false);
        }
    }
}
