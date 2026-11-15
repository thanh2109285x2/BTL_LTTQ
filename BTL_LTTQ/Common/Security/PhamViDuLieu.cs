using BTL_LTTQ.DAL;
using BTL_LTTQ.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BTL_LTTQ.Common
{
    public static class PhamViDuLieu
    {
        public static PhamViInfo ChoLopHoc(string maChucNang)
        {
            if (!PermissionService.ChiCuaMinh(maChucNang))
                return new PhamViInfo(LoaiPhamVi.TatCa);

            if (!string.Equals(maChucNang, ChucNangConst.LOPHOC_XEM, StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(maChucNang, ChucNangConst.TIM_LOP, StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(maChucNang, ChucNangConst.LOPHOC_SUA, StringComparison.OrdinalIgnoreCase))
                return new PhamViInfo(LoaiPhamVi.TuChoi);

            if (CurrentUser.IsGiaoVien)
            {
                if (string.IsNullOrWhiteSpace(CurrentUser.MaGV))
                    return new PhamViInfo(LoaiPhamVi.TuChoi);

                return new PhamViInfo(LoaiPhamVi.GiaoVien, MaGiaoVien: CurrentUser.MaGV);
            }

            if (CurrentUser.IsHocVien)
            {
                if (string.IsNullOrWhiteSpace(CurrentUser.MaHocVien))
                    return new PhamViInfo(LoaiPhamVi.TuChoi);

                return new PhamViInfo(LoaiPhamVi.HocVien, MaHocVien: CurrentUser.MaHocVien);
            }

            return new PhamViInfo(LoaiPhamVi.TuChoi);
        }

        public static (string DieuKienSql, SqlParameter[] ThamSo) ChoHocVien(string maChucNang, string aliasBang)
        {
            KiemTraAlias(aliasBang);

            if (!PermissionService.ChiCuaMinh(maChucNang))
                return DieuKienRong();

            if (!string.Equals(maChucNang, ChucNangConst.HOCVIEN_XEM, StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(maChucNang, ChucNangConst.BC_DIEM, StringComparison.OrdinalIgnoreCase))
                return TuChoiTatCa();

            if (CurrentUser.IsGiaoVien)
            {
                if (string.IsNullOrWhiteSpace(CurrentUser.MaGV))
                    return TuChoiTatCa();

                return ($"{aliasBang}.MaLop IN (SELECT pv_l.MaLop FROM LopHoc pv_l WHERE pv_l.MaGV = @PV_MaGV)", new[]
                {
                    new SqlParameter("@PV_MaGV", SqlDbType.VarChar, 20) { Value = CurrentUser.MaGV }
                });
            }

            if (CurrentUser.IsHocVien)
            {
                if (string.IsNullOrWhiteSpace(CurrentUser.MaHocVien))
                    return TuChoiTatCa();

                return ($"{aliasBang}.MaHocVien = @PV_MaHocVien", new[]
                {
                    new SqlParameter("@PV_MaHocVien", SqlDbType.VarChar, 20) { Value = CurrentUser.MaHocVien }
                });
            }

            return TuChoiTatCa();
        }

        private static (string DieuKienSql, SqlParameter[] ThamSo) DieuKienRong()
        {
            return ("", Array.Empty<SqlParameter>());
        }

        private static (string DieuKienSql, SqlParameter[] ThamSo) TuChoiTatCa()
        {
            return ("1 = 0", Array.Empty<SqlParameter>());
        }

        private static void KiemTraAlias(string aliasBang)
        {
            if (string.IsNullOrEmpty(aliasBang) || !LaChuCaiHoacGachDuoi(aliasBang[0]))
                throw new ArgumentException("Alias bảng không hợp lệ.", nameof(aliasBang));

            for (int i = 1; i < aliasBang.Length; i++)
            {
                char kyTu = aliasBang[i];
                if (!LaChuCaiHoacGachDuoi(kyTu) && (kyTu < '0' || kyTu > '9'))
                    throw new ArgumentException("Alias bảng không hợp lệ.", nameof(aliasBang));
            }
        }

        private static bool LaChuCaiHoacGachDuoi(char kyTu)
        {
            return (kyTu >= 'A' && kyTu <= 'Z') ||
                   (kyTu >= 'a' && kyTu <= 'z') ||
                   kyTu == '_';
        }
    }
}
