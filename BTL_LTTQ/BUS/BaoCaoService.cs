using BTL_LTTQ.DAL;
using BTL_LTTQ.Common;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BTL_LTTQ.BUS
{
    /// <summary>
    /// BUS cho báo cáo (mở rộng theo yêu cầu 3 tầng).
    /// </summary>
    public class BaoCaoService
    {
        private readonly BaoCaoRepository _repo = new();

        public DataTable LayDanhSachLop(string maChucNang)
        {
            KiemTraQuyenBaoCao(maChucNang);
            if (!string.Equals(maChucNang, ChucNangConst.BC_DIEM, StringComparison.OrdinalIgnoreCase))
                return _repo.LayDanhSachLop("", Array.Empty<SqlParameter>());

            var phamVi = PhamViDiemTheoLop("l", "d");
            return _repo.LayDanhSachLop(phamVi.DieuKienSql, phamVi.ThamSo);
        }

        public DataTable LayBangDiem(string? maLop)
        {
            KiemTraQuyenBaoCao(ChucNangConst.BC_DIEM);
            var phamVi = PhamViDiemTheoLop("l", "d");
            return _repo.LayBangDiem(maLop, phamVi.DieuKienSql, phamVi.ThamSo);
        }

        public DataTable LayDoanhThu(string? maLop) => _repo.LayDoanhThu(maLop);

        public DataTable LayTop3HocVienTheoLop()
        {
            KiemTraQuyenBaoCao(ChucNangConst.BC_XEPHANG);
            var phamVi = PhamViTheoLop(ChucNangConst.BC_XEPHANG, "l", "hv");
            return _repo.LayTop3HocVienTheoLop(phamVi.DieuKienSql, phamVi.ThamSo);
        }

        public DataTable LayLopTheoPhong(string? maPhong = null)
        {
            KiemTraQuyenBaoCao(ChucNangConst.BC_LOP_PHONG);
            string dieuKien = "";
            SqlParameter[] thamSo = Array.Empty<SqlParameter>();
            if (PermissionService.ChiCuaMinh(ChucNangConst.BC_LOP_PHONG))
            {
                if (CurrentUser.IsGiaoVien && !string.IsNullOrWhiteSpace(CurrentUser.MaGV))
                {
                    dieuKien = "l.MaGV = @PV_MaGV";
                    thamSo = [new SqlParameter("@PV_MaGV", SqlDbType.VarChar, 20) { Value = CurrentUser.MaGV }];
                }
                else if (CurrentUser.IsHocVien && !string.IsNullOrWhiteSpace(CurrentUser.MaHocVien))
                {
                    dieuKien = "EXISTS (SELECT 1 FROM HocVien pv_h WHERE pv_h.MaHocVien = @PV_MaHocVien AND pv_h.MaLop = l.MaLop)";
                    thamSo = [new SqlParameter("@PV_MaHocVien", SqlDbType.VarChar, 20) { Value = CurrentUser.MaHocVien }];
                }
                else
                {
                    dieuKien = "1 = 0";
                }
            }
            return _repo.LayLopTheoPhong(maPhong, dieuKien, thamSo);
        }

        private static (string DieuKienSql, SqlParameter[] ThamSo) PhamViTheoLop(string maChucNang, string aliasLop, string aliasHocVien)
        {
            if (!PermissionService.ChiCuaMinh(maChucNang))
                return ("", Array.Empty<SqlParameter>());
            if (CurrentUser.IsGiaoVien && !string.IsNullOrWhiteSpace(CurrentUser.MaGV))
                return ($"{aliasLop}.MaGV = @PV_MaGV", [new SqlParameter("@PV_MaGV", SqlDbType.VarChar, 20) { Value = CurrentUser.MaGV }]);
            if (CurrentUser.IsHocVien && !string.IsNullOrWhiteSpace(CurrentUser.MaHocVien))
                return ($"{aliasHocVien}.MaHocVien = @PV_MaHocVien", [new SqlParameter("@PV_MaHocVien", SqlDbType.VarChar, 20) { Value = CurrentUser.MaHocVien }]);
            return ("1 = 0", Array.Empty<SqlParameter>());
        }

        private static void KiemTraQuyenBaoCao(string maChucNang)
        {
            if (!PermissionService.Can(maChucNang))
                throw new UnauthorizedAccessException("Bạn không có quyền sử dụng chức năng này.");
        }

        private static (string DieuKienSql, SqlParameter[] ThamSo) PhamViDiemTheoLop(string aliasLop, string aliasDangKy)
        {
            if (!PermissionService.ChiCuaMinh(ChucNangConst.BC_DIEM))
                return ("", Array.Empty<SqlParameter>());

            if (CurrentUser.IsGiaoVien && !string.IsNullOrWhiteSpace(CurrentUser.MaGV))
                return ($"{aliasLop}.MaGV = @PV_MaGV", [new SqlParameter("@PV_MaGV", SqlDbType.VarChar, 20) { Value = CurrentUser.MaGV }]);

            if (CurrentUser.IsHocVien && !string.IsNullOrWhiteSpace(CurrentUser.MaHocVien))
                return ($"{aliasDangKy}.MaHocVien = @PV_MaHocVien", [new SqlParameter("@PV_MaHocVien", SqlDbType.VarChar, 20) { Value = CurrentUser.MaHocVien }]);

            return ("1 = 0", Array.Empty<SqlParameter>());
        }
    }
}
