using BTL_LTTQ.Common;
using BTL_LTTQ.Models;
using BTL_LTTQ.Models.Reports;

namespace BTL_LTTQ.BUS.Reports
{
    /// <summary>
    /// Quyết định phạm vi dữ liệu cho báo cáo dựa trên quyền <see cref="ChucNangConst.BAOCAO_TAO"/>.
    /// Tách riêng khỏi <see cref="PhamViDuLieu"/> để không phải sửa logic phân quyền của module khác.
    ///
    ///  - Không có quyền  -> Từ chối.
    ///  - Có quyền, không giới hạn "chỉ của mình" -> Toàn bộ.
    ///  - Có quyền + "chỉ của mình" -> Giáo viên: theo lớp mình dạy; Học viên: theo hồ sơ của mình.
    /// </summary>
    public static class ReportScopeService
    {
        public static PhamViInfo ChoNguon(DataSourceDefinition? nguon)
        {
            if (nguon == null || nguon.PhamVi == PhamViNguon.Khong)
                return new PhamViInfo(LoaiPhamVi.TatCa);

            if (!PermissionService.Can(ChucNangConst.BAOCAO_TAO))
                return new PhamViInfo(LoaiPhamVi.TuChoi);

            if (!PermissionService.ChiCuaMinh(ChucNangConst.BAOCAO_TAO))
                return new PhamViInfo(LoaiPhamVi.TatCa);

            if (CurrentUser.IsGiaoVien && !string.IsNullOrWhiteSpace(CurrentUser.MaGV))
                return new PhamViInfo(LoaiPhamVi.GiaoVien, MaGiaoVien: CurrentUser.MaGV);

            if (CurrentUser.IsHocVien && !string.IsNullOrWhiteSpace(CurrentUser.MaHocVien))
                return new PhamViInfo(LoaiPhamVi.HocVien, MaHocVien: CurrentUser.MaHocVien);

            return new PhamViInfo(LoaiPhamVi.TuChoi);
        }
    }
}
