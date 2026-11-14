using BTL_LTTQ.DataAccessLayer;
using BTL_LTTQ.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace BTL_LTTQ.Repositories
{
    public class ThoiKhoaBieuRepository
    {

        //Ghi đè toàn bộ lịch trong một transaction

        public void XoaTatCa()
        {
            DbHelper.ExecuteNonQuery("DELETE FROM BuoiHoc");
        }

        public bool CoLich()
        {
            object? ketQua = DbHelper.ExecuteScalar("SELECT COUNT(*) FROM BuoiHoc");
            return Convert.ToInt32(ketQua) > 0;
        }

        //Lịch theo vai trò: giáo viên truyền MaGV, học viên truyền MaHocVien, admin truyền null
        public List<BuoiHoc> LayLich(string? maGV, string? maHocVien)
        {
            const string sql = @"
                SELECT b.MaBuoi, b.MaLop, l.TenLop, m.TenMon,
                       b.MaGV, g.TenGV, b.Ngay, b.MaCa, c.TenCa,
                       c.GioBatDau, c.GioKetThuc, b.MaPhong, p.TenPhong
                FROM BuoiHoc b
                JOIN LopHoc l ON l.MaLop = b.MaLop
                LEFT JOIN MonHoc m ON m.MaMon = l.MaMon
                JOIN GiaoVien g ON g.MaGV = b.MaGV
                JOIN CaHoc c ON c.MaCa = b.MaCa
                JOIN PhongHoc p ON p.MaPhong = b.MaPhong
                WHERE (@MaGV IS NULL OR b.MaGV = @MaGV)
                  AND (@MaHocVien IS NULL OR EXISTS (
                        SELECT 1 FROM DangKy d
                        WHERE d.MaLop = b.MaLop AND d.MaHocVien = @MaHocVien))
                ORDER BY b.Ngay, c.GioBatDau, b.MaLop";

            DataTable dt = DbHelper.ExecuteQuery(sql,
                new SqlParameter("@MaGV", SqlDbType.VarChar, 20) { Value = (object?)maGV ?? DBNull.Value },
                new SqlParameter("@MaHocVien", SqlDbType.VarChar, 20) { Value = (object?)maHocVien ?? DBNull.Value });

            var ketQua = new List<BuoiHoc>();
            foreach (DataRow dong in dt.Rows)
            {
                ketQua.Add(new BuoiHoc
                {
                    MaBuoi = Convert.ToInt32(dong["MaBuoi"]),
                    MaLop = dong["MaLop"].ToString() ?? "",
                    TenLop = dong["TenLop"].ToString() ?? "",
                    TenMon = dong["TenMon"].ToString() ?? "",
                    MaGV = dong["MaGV"].ToString() ?? "",
                    TenGV = dong["TenGV"].ToString() ?? "",
                    Ngay = Convert.ToDateTime(dong["Ngay"]),
                    MaCa = dong["MaCa"].ToString() ?? "",
                    TenCa = dong["TenCa"].ToString() ?? "",
                    GioBatDau = dong["GioBatDau"] == DBNull.Value ? null : (TimeSpan)dong["GioBatDau"],
                    GioKetThuc = dong["GioKetThuc"] == DBNull.Value ? null : (TimeSpan)dong["GioKetThuc"],
                    MaPhong = dong["MaPhong"].ToString() ?? "",
                    TenPhong = dong["TenPhong"].ToString() ?? ""
                });
            }

            return ketQua;
        }
    }
}
