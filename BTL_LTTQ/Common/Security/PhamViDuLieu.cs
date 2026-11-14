using BTL_LTTQ.Models;
using System;

namespace BTL_LTTQ.Common
{
    /// <summary>
    /// BUS quyết định phạm vi dữ liệu được xem; DAL nhận PhamViInfo và tạo SQL.
    /// </summary>
    public static class PhamViDuLieu
    {
        public static PhamViInfo ChoLopHoc(string maChucNang)
        {
            if (!PermissionService.Can(maChucNang))
                return new PhamViInfo { Loai = LoaiPhamVi.TuChoi };

            if (!PermissionService.ChiCuaMinh(maChucNang))
                return new PhamViInfo { Loai = LoaiPhamVi.TatCa };

            if (!string.Equals(maChucNang, ChucNangConst.LOPHOC_XEM, StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(maChucNang, ChucNangConst.TIM_LOP, StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(maChucNang, ChucNangConst.LOPHOC_SUA, StringComparison.OrdinalIgnoreCase))
                return new PhamViInfo { Loai = LoaiPhamVi.TuChoi };

            if (CurrentUser.IsGiaoVien)
            {
                if (string.IsNullOrWhiteSpace(CurrentUser.MaGV))
                    return new PhamViInfo { Loai = LoaiPhamVi.TuChoi };
                return new PhamViInfo { Loai = LoaiPhamVi.GiaoVien, Ma = CurrentUser.MaGV };
            }

            if (CurrentUser.IsHocVien)
            {
                if (string.IsNullOrWhiteSpace(CurrentUser.MaHocVien))
                    return new PhamViInfo { Loai = LoaiPhamVi.TuChoi };
                return new PhamViInfo { Loai = LoaiPhamVi.HocVien, Ma = CurrentUser.MaHocVien };
            }

            return new PhamViInfo { Loai = LoaiPhamVi.TuChoi };
        }

        public static PhamViInfo ChoHocVien(string maChucNang)
        {
            if (!PermissionService.Can(maChucNang))
                return new PhamViInfo { Loai = LoaiPhamVi.TuChoi };

            if (!PermissionService.ChiCuaMinh(maChucNang))
                return new PhamViInfo { Loai = LoaiPhamVi.TatCa };

            if (!string.Equals(maChucNang, ChucNangConst.HOCVIEN_XEM, StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(maChucNang, ChucNangConst.BC_DIEM, StringComparison.OrdinalIgnoreCase))
                return new PhamViInfo { Loai = LoaiPhamVi.TuChoi };

            if (CurrentUser.IsGiaoVien)
            {
                if (string.IsNullOrWhiteSpace(CurrentUser.MaGV))
                    return new PhamViInfo { Loai = LoaiPhamVi.TuChoi };
                return new PhamViInfo { Loai = LoaiPhamVi.GiaoVien, Ma = CurrentUser.MaGV };
            }

            if (CurrentUser.IsHocVien)
            {
                if (string.IsNullOrWhiteSpace(CurrentUser.MaHocVien))
                    return new PhamViInfo { Loai = LoaiPhamVi.TuChoi };
                return new PhamViInfo { Loai = LoaiPhamVi.HocVien, Ma = CurrentUser.MaHocVien };
            }

            return new PhamViInfo { Loai = LoaiPhamVi.TuChoi };
        }
    }
}
