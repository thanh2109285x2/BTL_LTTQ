using BTL_LTTQ.BUS.Reports;
using BTL_LTTQ.Common.Reports;
using System;
using System.Linq;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.Reports
{
    /// <summary>Các bước của luồng tạo báo cáo.</summary>
    public enum BuocBaoCao
    {
        Dashboard = 0,
        NguonDuLieu = 1,
        CauHinh = 2,
        BoCuc = 3,
        XemTruoc = 4
    }

    public sealed class ChuyenBuocEventArgs : EventArgs
    {
        public ChuyenBuocEventArgs(BuocBaoCao buoc) => Buoc = buoc;
        public BuocBaoCao Buoc { get; }
    }

    /// <summary>
    /// Lớp nền cho các màn hình Report Builder. Giữ tham chiếu tới trạng thái dùng chung
    /// và cung cấp sự kiện điều hướng lên form chủ.
    /// </summary>
    public abstract class ReportScreenBase : UserControl
    {
        protected ReportScreenBase(ReportEditorState trangThai, ReportDefinitionService dichVu)
        {
            TrangThai = trangThai ?? throw new ArgumentNullException(nameof(trangThai));
            DichVu = dichVu ?? throw new ArgumentNullException(nameof(dichVu));
            DanhMuc = dichVu.DanhMuc;
            Dock = DockStyle.Fill;
            BackColor = Common.Reports.ReportDesignTokens.NenPanel;
        }

        protected ReportEditorState TrangThai { get; }
        protected ReportDefinitionService DichVu { get; }
        protected ReportCatalogService DanhMuc { get; }

        /// <summary>Màn hình yêu cầu form chủ chuyển bước.</summary>
        public event EventHandler<ChuyenBuocEventArgs>? YeuCauChuyenBuoc;

        /// <summary>Gọi mỗi khi màn hình được hiển thị để nạp lại dữ liệu.</summary>
        public virtual void Nap()
        {
        }

        protected void Chuyen(BuocBaoCao buoc) =>
            YeuCauChuyenBuoc?.Invoke(this, new ChuyenBuocEventArgs(buoc));

        /// <summary>
        /// Lưu báo cáo đang dựng. Kiểm tra cấu hình trước; chỉ lưu khi không còn lỗi.
        /// Dùng chung cho nút Lưu trên thanh công cụ của form chủ và các màn hình.
        /// </summary>
        public bool LuuBaoCao()
        {
            if (TrangThai.Nguon == null)
            {
                HienThongBao("Chưa chọn nguồn dữ liệu nên chưa thể lưu.",
                    "Thiếu thông tin", MessageBoxIcon.Warning);
                return false;
            }

            var thongBao = DichVu.KiemTra(TrangThai.DinhNghia);
            if (DichVu.CoLoi(thongBao))
            {
                var loi = thongBao.First(t => t.MucDo == MucDoThongDiep.Loi);
                HienThongBao("Không thể lưu vì cấu hình còn lỗi:\n\n• " + loi.ThongDiep,
                    "Cấu hình chưa hợp lệ", MessageBoxIcon.Warning);
                return false;
            }

            int ma = DichVu.Luu(TrangThai.DinhNghia, TrangThai.NguoiDung, out string loiLuu);
            if (ma <= 0)
            {
                HienThongBao(string.IsNullOrWhiteSpace(loiLuu) ? "Không lưu được báo cáo." : loiLuu,
                    "Không lưu được", MessageBoxIcon.Error);
                return false;
            }

            TrangThai.DanhDauDaLuu(ma);
            HienThongBao($"Đã lưu báo cáo “{TrangThai.DinhNghia.Ten}”.",
                "Thành công", MessageBoxIcon.Information);
            return true;
        }

        protected static void HienThongBao(string noiDung, string tieuDe = "Thông báo",
            MessageBoxIcon bieuTuong = MessageBoxIcon.Information)
        {
            MessageBox.Show(noiDung, tieuDe, MessageBoxButtons.OK, bieuTuong);
        }
    }
}
