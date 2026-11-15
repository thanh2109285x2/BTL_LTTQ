using BTL_LTTQ.DAL;
using BTL_LTTQ.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace BTL_LTTQ.DAL
{
    public sealed class NhapDiemRepository
    {
        public DataTable LayDanhSachLop(PhamViInfo phamVi)
        {
            var dieuKien = PhamViSqlBuilder.TaoDieuKienLopHoc(phamVi, "l");
            string dieuKienPhamVi = dieuKien.DieuKienSql.Length == 0
                ? ""
                : " AND " + dieuKien.DieuKienSql;
            string sql = $@"
                SELECT l.MaLop, l.TenLop
                FROM dbo.LopHoc l
                WHERE 1 = 1{dieuKienPhamVi}
                ORDER BY l.TenLop, l.MaLop";

            return DbHelper.ExecuteQuery(sql, dieuKien.ThamSo);
        }

        public DataTable LayDanhSachHocVien(string maLop, PhamViInfo phamVi)
        {
            var dieuKien = PhamViSqlBuilder.TaoDieuKienLopHoc(phamVi, "l");
            string dieuKienPhamVi = dieuKien.DieuKienSql.Length == 0
                ? ""
                : " AND " + dieuKien.DieuKienSql;
            string sql = $@"
                SELECT d.MaHocVien, h.TenHocVien, d.Diem, d.MaXepLoai
                FROM dbo.DangKy d
                INNER JOIN dbo.HocVien h ON h.MaHocVien = d.MaHocVien
                INNER JOIN dbo.LopHoc l ON l.MaLop = d.MaLop
                WHERE d.MaLop = @MaLop{dieuKienPhamVi}
                ORDER BY h.TenHocVien, d.MaHocVien";

            var thamSo = new List<SqlParameter>
            {
                new SqlParameter("@MaLop", SqlDbType.VarChar, 20) { Value = maLop }
            };
            thamSo.AddRange(dieuKien.ThamSo);

            return DbHelper.ExecuteQuery(sql, thamSo.ToArray());
        }

        public DataTable LayDanhSachXepLoai() => DbHelper.ExecuteQuery(
            "SELECT MaXepLoai, TenLoai FROM dbo.XepLoai ORDER BY TenLoai, MaXepLoai");

        public void CapNhatDiem(PhamViInfo phamVi, IReadOnlyCollection<CapNhatDiemDangKy> danhSach)
        {
            if (danhSach.Count == 0)
                return;

            DbHelper.ExecuteTransaction((conn, tran) =>
            {
                foreach (CapNhatDiemDangKy dong in danhSach)
                {
                    var dieuKien = PhamViSqlBuilder.TaoDieuKienLopHoc(phamVi, "l");
                    string dieuKienPhamVi = dieuKien.DieuKienSql.Length == 0
                        ? ""
                        : $" AND EXISTS (SELECT 1 FROM dbo.LopHoc l WHERE l.MaLop = d.MaLop AND {dieuKien.DieuKienSql})";
                    string sql = $@"
                        UPDATE d
                        SET Diem = @Diem, MaXepLoai = @MaXepLoai
                        FROM dbo.DangKy d
                        WHERE d.MaHocVien = @MaHocVien
                          AND d.MaLop = @MaLop{dieuKienPhamVi}";

                    using var cmd = new SqlCommand(sql, conn, tran);
                    cmd.Parameters.Add(new SqlParameter("@MaHocVien", SqlDbType.VarChar, 20) { Value = dong.MaHocVien });
                    cmd.Parameters.Add(new SqlParameter("@MaLop", SqlDbType.VarChar, 20) { Value = dong.MaLop });
                    cmd.Parameters.Add(new SqlParameter("@Diem", SqlDbType.Float) { Value = (object?)dong.Diem ?? DBNull.Value });
                    cmd.Parameters.Add(new SqlParameter("@MaXepLoai", SqlDbType.VarChar, 20) { Value = (object?)dong.MaXepLoai ?? DBNull.Value });
                    cmd.Parameters.AddRange(dieuKien.ThamSo);

                    if (cmd.ExecuteNonQuery() != 1)
                        throw new InvalidOperationException($"Không cập nhật được điểm cho học viên {dong.MaHocVien} trong lớp {dong.MaLop}; thao tác đã được hoàn tác.");
                }
            });
        }
    }
}
