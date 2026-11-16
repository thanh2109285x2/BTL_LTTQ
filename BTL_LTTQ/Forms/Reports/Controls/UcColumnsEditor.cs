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
    /// Trình biên tập cột hiển thị: bật/tắt, tiêu đề, độ rộng, căn lề, định dạng, in đậm.
    /// Thứ tự dòng chính là thứ tự cột trong báo cáo.
    /// </summary>
    public sealed class UcColumnsEditor : UcListEditorBase
    {
        private const string CotHienThi = "cotHienThi";
        private const string CotTruong = "cotTruong";
        private const string CotTieuDe = "cotTieuDe";
        private const string CotDoRong = "cotDoRong";
        private const string CotCanLe = "cotCanLe";
        private const string CotDinhDang = "cotDinhDang";
        private const string CotInDam = "cotInDam";

        private bool _dangDoi;

        public UcColumnsEditor(ReportEditorState trangThai) : base(trangThai, "Thêm cột")
        {
        }

        protected override IList DanhSach => TrangThai.DinhNghia.Cot;

        protected override void DungCot(DataGridView g)
        {
            g.Columns.Add(CotChonHop(CotHienThi, "Hiện", 50));

            g.Columns.Add(new DataGridViewComboBoxColumn
            {
                Name = CotTruong,
                HeaderText = "Trường dữ liệu",
                ValueType = typeof(string),
                DisplayMember = nameof(LuaChon<string>.Nhan),
                ValueMember = nameof(LuaChon<string>.GiaTri),
                FlatStyle = FlatStyle.Flat,
                FillWeight = 200,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton
            });

            g.Columns.Add(CotChu(CotTieuDe, "Tiêu đề hiển thị", 200));
            g.Columns.Add(CotChu(CotDoRong, "Độ rộng (px)", 90));
            g.Columns.Add(CotChon(CotCanLe, "Căn lề", LuaChonCua(
                new[] { CanLeCot.Trai, CanLeCot.Giua, CanLeCot.Phai }, ReportEnumText.Ten), 100));
            g.Columns.Add(CotChu(CotDinhDang, "Định dạng", 110));
            g.Columns.Add(CotChonHop(CotInDam, "Đậm", 55));

            g.Columns[CotDoRong]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
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
            var daDung = TrangThai.DinhNghia.Cot.Select(c => c.MaTruong).ToHashSet(StringComparer.OrdinalIgnoreCase);
            var truong = nguon?.Truong.Find(t => !daDung.Contains(t.Ma)) ?? nguon?.Truong.FirstOrDefault();

            var cot = new ReportColumnDef { MaTruong = truong?.Ma ?? "" };
            if (truong != null)
                ApDungMacDinh(cot, truong);
            return cot;
        }

        protected override void DongVaoBang(DataGridViewRow row, object item)
        {
            var cot = (ReportColumnDef)item;
            row.Cells[CotHienThi].Value = cot.HienThi;
            row.Cells[CotTruong].Value = cot.MaTruong;
            row.Cells[CotTieuDe].Value = cot.TieuDe;
            row.Cells[CotDoRong].Value = cot.DoRong;
            row.Cells[CotCanLe].Value = cot.CanLe;
            row.Cells[CotDinhDang].Value = cot.DinhDang;
            row.Cells[CotInDam].Value = cot.InDam;
            cot.ThuTu = row.Index;
        }

        protected override void DocDong(DataGridViewRow row)
        {
            if (_dangDoi)
                return;

            var cot = (ReportColumnDef)row.Tag!;
            cot.HienThi = Convert.ToBoolean(row.Cells[CotHienThi].Value ?? false);
            cot.MaTruong = Convert.ToString(row.Cells[CotTruong].Value) ?? cot.MaTruong;
            cot.TieuDe = Convert.ToString(row.Cells[CotTieuDe].Value) ?? "";
            if (int.TryParse(Convert.ToString(row.Cells[CotDoRong].Value), out int rong))
                cot.DoRong = Math.Clamp(rong, 40, 800);
            cot.CanLe = row.Cells[CotCanLe].Value is CanLeCot cl ? cl : CanLeCot.Trai;
            cot.DinhDang = Convert.ToString(row.Cells[CotDinhDang].Value) ?? "";
            cot.InDam = Convert.ToBoolean(row.Cells[CotInDam].Value ?? false);
        }

        protected override void SauKhiDoc(DataGridViewRow row, int cot)
        {
            if (_dangDoi || cot != Luoi.Columns[CotTruong].Index)
                return;

            _dangDoi = true;
            try
            {
                var cotDef = (ReportColumnDef)row.Tag!;
                var truong = TrangThai.Nguon?.TimTruong(cotDef.MaTruong);
                if (truong != null)
                    ApDungMacDinh(cotDef, truong);
                DongVaoBang(row, cotDef);
            }
            finally
            {
                _dangDoi = false;
            }
        }

        private static void ApDungMacDinh(ReportColumnDef cot, FieldDefinition truong)
        {
            cot.TieuDe = truong.TieuDe;
            cot.DoRong = truong.DoRong;
            cot.CanLe = truong.CanLeMacDinh;
            cot.DinhDang = truong.DinhDangMacDinh;
        }
    }
}
