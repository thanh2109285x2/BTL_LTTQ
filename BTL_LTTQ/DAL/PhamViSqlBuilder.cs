using BTL_LTTQ.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace BTL_LTTQ.DAL
{
    /// <summary>
    /// Biến PhamViInfo (BUS quyết định) thành điều kiện SQL và tham số cho DAL.
    /// </summary>
    public static class PhamViSqlBuilder
    {
        public static (string DieuKienSql, SqlParameter[] ThamSo) ChoHocVien(PhamViInfo? phamVi, string aliasHocVien)
        {
            KiemTraAlias(aliasHocVien);
            if (phamVi == null || phamVi.Loai == LoaiPhamVi.TatCa)
                return ("", Array.Empty<SqlParameter>());
            if (phamVi.Loai == LoaiPhamVi.TuChoi)
                return ("1 = 0", Array.Empty<SqlParameter>());

            return phamVi.Loai switch
            {
                LoaiPhamVi.GiaoVien when !string.IsNullOrWhiteSpace(phamVi.MaGiaoVien) =>
                    ($"EXISTS (SELECT 1 FROM dbo.LopHoc pv_l WHERE pv_l.MaLop = {aliasHocVien}.MaLop AND pv_l.MaGV = @PV_MaGV)", new[]
                    {
                        new SqlParameter("@PV_MaGV", SqlDbType.VarChar, 20) { Value = phamVi.MaGiaoVien }
                    }),
                LoaiPhamVi.HocVien when !string.IsNullOrWhiteSpace(phamVi.MaHocVien) =>
                    ($"{aliasHocVien}.MaHocVien = @PV_MaHocVien", new[]
                    {
                        new SqlParameter("@PV_MaHocVien", SqlDbType.VarChar, 20) { Value = phamVi.MaHocVien }
                    }),
                _ => ("1 = 0", Array.Empty<SqlParameter>())
            };
        }

        public static (string DieuKienSql, SqlParameter[] ThamSo) TaoDieuKienLopHoc(PhamViInfo? phamVi, string aliasBang)
        {
            KiemTraAlias(aliasBang);
            if (phamVi == null || phamVi.Loai == LoaiPhamVi.TatCa)
                return ("", Array.Empty<SqlParameter>());
            if (phamVi.Loai == LoaiPhamVi.TuChoi)
                return ("1 = 0", Array.Empty<SqlParameter>());

            return phamVi.Loai switch
            {
                LoaiPhamVi.TatCa => ("", Array.Empty<SqlParameter>()),
                LoaiPhamVi.GiaoVien when !string.IsNullOrWhiteSpace(phamVi.MaGiaoVien) =>
                    ($"{aliasBang}.MaGV = @PV_MaGV", new[]
                {
                        new SqlParameter("@PV_MaGV", SqlDbType.VarChar, 20) { Value = phamVi.MaGiaoVien }
                    }),
                LoaiPhamVi.HocVien when !string.IsNullOrWhiteSpace(phamVi.MaHocVien) =>
                    ($"EXISTS (SELECT 1 FROM DangKy pv_dk WHERE pv_dk.MaHocVien = @PV_MaHocVien AND pv_dk.MaLop = {aliasBang}.MaLop)", new[]
            {
                        new SqlParameter("@PV_MaHocVien", SqlDbType.VarChar, 20) { Value = phamVi.MaHocVien }
                    }),
                _ => ("1 = 0", Array.Empty<SqlParameter>())
            };
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
