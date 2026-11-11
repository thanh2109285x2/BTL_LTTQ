using BTL_LTTQ.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Microsoft.Data.SqlClient;

namespace BTL_LTTQ.DataAccessLayer
{
    public class PhanQuyenDAL
    {
        public List<PhanQuyenChucNang> LayDanhSachChucNang()
        {
            const string sql = @"SELECT c.MaChucNang, c.TenChucNang, c.Nhom,
                                        c.HoTroLocTheoChu, c.ThuTu,
                                        ISNULL(gv.CoQuyen, 0) AS GV_CoQuyen,
                                        ISNULL(gv.ChiCuaMinh, 0) AS GV_ChiCuaMinh,
                                        ISNULL(hv.CoQuyen, 0) AS HV_CoQuyen,
                                        ISNULL(hv.ChiCuaMinh, 0) AS HV_ChiCuaMinh
                                 FROM ChucNang c
                                 LEFT JOIN PhanQuyen gv
                                     ON gv.MaChucNang = c.MaChucNang AND gv.VaiTro = @VaiTroGV
                                 LEFT JOIN PhanQuyen hv
                                     ON hv.MaChucNang = c.MaChucNang AND hv.VaiTro = @VaiTroHV
                                 WHERE c.ChiAdmin = 0
                                 ORDER BY c.ThuTu";

            DataTable dt = DbHelper.ExecuteQuery(sql,
                new SqlParameter("@VaiTroGV", "GiaoVien"),
                new SqlParameter("@VaiTroHV", "HocVien"));

            var danhSach = new List<PhanQuyenChucNang>();
            foreach (DataRow dong in dt.Rows)
            {
                danhSach.Add(new PhanQuyenChucNang
                {
                    MaChucNang = dong["MaChucNang"].ToString() ?? "",
                    TenChucNang = dong["TenChucNang"].ToString() ?? "",
                    Nhom = dong["Nhom"].ToString() ?? "",
                    HoTroLocTheoChu = Convert.ToBoolean(dong["HoTroLocTheoChu"]),
                    ThuTu = Convert.ToInt32(dong["ThuTu"]),
                    GiaoVienCoQuyen = Convert.ToBoolean(dong["GV_CoQuyen"]),
                    GiaoVienChiCuaMinh = Convert.ToBoolean(dong["GV_ChiCuaMinh"]),
                    HocVienCoQuyen = Convert.ToBoolean(dong["HV_CoQuyen"]),
                    HocVienChiCuaMinh = Convert.ToBoolean(dong["HV_ChiCuaMinh"])
                });
            }

            return danhSach;
        }

        public void Luu(IEnumerable<PhanQuyenChucNang> danhSach)
        {
            ArgumentNullException.ThrowIfNull(danhSach);
            List<PhanQuyenChucNang> cacChucNang = danhSach.ToList();

            foreach (PhanQuyenChucNang chucNang in cacChucNang)
            {
                if ((chucNang.GiaoVienChiCuaMinh && !chucNang.GiaoVienCoQuyen) ||
                    (chucNang.HocVienChiCuaMinh && !chucNang.HocVienCoQuyen))
                    throw new InvalidOperationException("Quyền chỉ của mình yêu cầu quyền chính phải được bật.");

                if (!chucNang.HoTroLocTheoChu &&
                    (chucNang.GiaoVienChiCuaMinh || chucNang.HocVienChiCuaMinh))
                    throw new InvalidOperationException("Chức năng này không hỗ trợ quyền chỉ của mình.");
            }

            DbHelper.ExecuteTransaction((connection, transaction) =>
            {
                foreach (PhanQuyenChucNang chucNang in cacChucNang)
                {
                    Upsert(connection, transaction, chucNang.MaChucNang, "GiaoVien",
                        chucNang.GiaoVienCoQuyen, chucNang.GiaoVienChiCuaMinh);
                    Upsert(connection, transaction, chucNang.MaChucNang, "HocVien",
                        chucNang.HocVienCoQuyen, chucNang.HocVienChiCuaMinh);
                }
            });
        }

        public DataTable LayTheoVaiTro(string vaiTro)
        {
            const string sql = @"SELECT MaChucNang, CoQuyen, ChiCuaMinh
                                 FROM PhanQuyen
                                 WHERE VaiTro = @VaiTro";

            return DbHelper.ExecuteQuery(sql,
                new SqlParameter("@VaiTro", SqlDbType.VarChar, 20) { Value = vaiTro });
        }

        private static void Upsert(SqlConnection connection, SqlTransaction transaction,
            string maChucNang, string vaiTro, bool coQuyen, bool chiCuaMinh)
        {
            const string sql = @"UPDATE PhanQuyen WITH (UPDLOCK, HOLDLOCK)
                                 SET CoQuyen = @CoQuyen, ChiCuaMinh = @ChiCuaMinh
                                 WHERE MaChucNang = @MaChucNang AND VaiTro = @VaiTro;
                                 IF @@ROWCOUNT = 0
                                     INSERT INTO PhanQuyen (MaChucNang, VaiTro, CoQuyen, ChiCuaMinh)
                                     VALUES (@MaChucNang, @VaiTro, @CoQuyen, @ChiCuaMinh);";

            using var command = new SqlCommand(sql, connection, transaction);
            command.Parameters.AddWithValue("@MaChucNang", maChucNang);
            command.Parameters.AddWithValue("@VaiTro", vaiTro);
            command.Parameters.AddWithValue("@CoQuyen", coQuyen);
            command.Parameters.AddWithValue("@ChiCuaMinh", chiCuaMinh);
            command.ExecuteNonQuery();
        }
    }
}
