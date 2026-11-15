using BTL_LTTQ.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BTL_LTTQ.DAL
{
    public static class PhamViSqlBuilder
    {
        public static (string DieuKienSql, SqlParameter[] ThamSo) TaoDieuKienLopHoc(PhamViInfo phamVi, string aliasBang)
        {
            KiemTraAlias(aliasBang);

            return phamVi.Loai switch
            {
                LoaiPhamVi.TatCa => ("", Array.Empty<SqlParameter>()),
                LoaiPhamVi.GiaoVien when !string.IsNullOrWhiteSpace(phamVi.MaGiaoVien) =>
                    ($"{aliasBang}.MaGV = @PV_MaGV", new[]
                    {
                        new SqlParameter("@PV_MaGV", SqlDbType.VarChar, 20) { Value = phamVi.MaGiaoVien }
                    }),
                LoaiPhamVi.HocVien when !string.IsNullOrWhiteSpace(phamVi.MaHocVien) =>
                    ($"EXISTS (SELECT 1 FROM HocVien pv_h WHERE pv_h.MaHocVien = @PV_MaHocVien AND pv_h.MaLop = {aliasBang}.MaLop)", new[]
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
