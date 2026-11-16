using BTL_LTTQ.BUS.Reports;
using BTL_LTTQ.Common.Reports;
using BTL_LTTQ.Models.Reports;
using System.Collections;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.Reports.Controls
{
    /// <summary>Trình biên tập dòng tổng hợp (trường + hàm + phạm vi + nhãn hiển thị).</summary>
    public sealed class UcSummaryEditor : UcListEditorBase
    {
        private const string CotTruong = "cotTruong";
        private const string CotHam = "cotHam";
        private const string CotPhamVi = "cotPhamVi";
        private const string CotTieuDe = "cotTieuDe";
        private const string CotHienThi = "cotHienThi";

        public UcSummaryEditor(ReportEditorState trangThai) : base(trangThai, "Thêm tổng hợp")
        {
        }

        protected override IList DanhSach => TrangThai.DinhNghia.TongHop;

        protected override void DungCot(DataGridView g)
        {
            g.Columns.Add(new DataGridViewComboBoxColumn
            {
                Name = CotTruong,
                HeaderText = "Trường",
                ValueType = typeof(string),
                DisplayMember = nameof(LuaChon<string>.Nhan),
                ValueMember = nameof(LuaChon<string>.GiaTri),
                FlatStyle = FlatStyle.Flat,
                FillWeight = 200,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton
            });

            g.Columns.Add(CotChon(CotHam, "Hàm tổng hợp", LuaChonCua(
                ReportEnumText.CacHamTongHop(), ReportEnumText.Ten), 170));

            g.Columns.Add(CotChon(CotPhamVi, "Phạm vi", LuaChonCua(
                new[] { PhamViTongHop.ToanBaoCao, PhamViTongHop.TheoNhom }, ReportEnumText.Ten), 130));

            g.Columns.Add(CotChu(CotTieuDe, "Nhãn hiển thị", 170));
            g.Columns.Add(CotChonHop(CotHienThi, "Hiện", 60));
        }

        public override void Nap()
        {
            Luoi.Rows.Clear();
            if (Luoi.Columns[CotTruong] is DataGridViewComboBoxColumn cot)
                cot.DataSource = LuaChonTruong(t => t.CoTheTongHop);
            DungLai();
        }

        protected override object TaoMoi()
        {
            var nguon = TrangThai.Nguon;
            var truong = nguon?.Truong.Find(t => t.CoTheTongHop);
            return new ReportSummaryDef
            {
                MaTruong = truong?.Ma ?? "",
                Ham = HamTongHop.Dem,
                PhamVi = PhamViTongHop.ToanBaoCao,
                HienThi = true
            };
        }

        protected override void DongVaoBang(DataGridViewRow row, object item)
        {
            var th = (ReportSummaryDef)item;
            row.Cells[CotTruong].Value = th.MaTruong;
            row.Cells[CotHam].Value = th.Ham;
            row.Cells[CotPhamVi].Value = th.PhamVi;
            row.Cells[CotTieuDe].Value = th.TieuDe;
            row.Cells[CotHienThi].Value = th.HienThi;
        }

        protected override void DocDong(DataGridViewRow row)
        {
            var th = (ReportSummaryDef)row.Tag!;
            th.MaTruong = Convert.ToString(row.Cells[CotTruong].Value) ?? th.MaTruong;
            th.Ham = row.Cells[CotHam].Value is HamTongHop h ? h : HamTongHop.Khong;
            th.PhamVi = row.Cells[CotPhamVi].Value is PhamViTongHop p ? p : PhamViTongHop.ToanBaoCao;
            th.TieuDe = Convert.ToString(row.Cells[CotTieuDe].Value) ?? "";
            th.HienThi = Convert.ToBoolean(row.Cells[CotHienThi].Value ?? false);
        }
    }
}
