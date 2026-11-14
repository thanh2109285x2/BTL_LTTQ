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
        //Gom dữ liệu đầu vào cho bộ xếp lịch
        public DuLieuXepLich LayDuLieuXepLich()
        {
            var duLieu = new DuLieuXepLich();

            DataTable phong = DbHelper.ExecuteQuery(
                "SELECT MaPhong, TenPhong, ISNULL(SucChua, 0) AS SucChua FROM PhongHoc ORDER BY MaPhong");
            foreach (DataRow dong in phong.Rows)
            {
                duLieu.Phong.Add(new PhongXepLich
                {
                    MaPhong = dong["MaPhong"].ToString() ?? "",
                    TenPhong = dong["TenPhong"].ToString() ?? "",
                    SucChua = Convert.ToInt32(dong["SucChua"])
                });
            }

            DataTable ca = DbHelper.ExecuteQuery(
                "SELECT MaCa FROM CaHoc ORDER BY ISNULL(GioBatDau, '23:59'), MaCa");
            foreach (DataRow dong in ca.Rows)
                duLieu.Ca.Add(dong["MaCa"].ToString() ?? "");

            var thuTheoMau = new Dictionary<string, List<int>>(StringComparer.OrdinalIgnoreCase);
            DataTable thu = DbHelper.ExecuteQuery("SELECT MaThoiGian, Thu FROM ThoiGian_Thu");
            foreach (DataRow dong in thu.Rows)
            {
                string maThoiGian = dong["MaThoiGian"].ToString() ?? "";
                int soThu = Convert.ToInt32(dong["Thu"]); //2..7, 8 = CN

                if (!thuTheoMau.TryGetValue(maThoiGian, out var ds))
                {
                    ds = new List<int>();
                    thuTheoMau[maThoiGian] = ds;
                }

                if (soThu >= 2 && soThu <= 8)
                    ds.Add(soThu - 2); //0 = Thứ 2 ... 6 = CN
            }

            var hocVienTheoLop = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
            DataTable dangKy = DbHelper.ExecuteQuery(
                "SELECT MaLop, MaHocVien FROM DangKy ORDER BY MaLop, MaHocVien");
            foreach (DataRow dong in dangKy.Rows)
            {
                string maLop = dong["MaLop"].ToString() ?? "";
                string maHocVien = dong["MaHocVien"].ToString() ?? "";

                if (!hocVienTheoLop.TryGetValue(maLop, out var ds))
                {
                    ds = new List<string>();
                    hocVienTheoLop[maLop] = ds;
                }

                ds.Add(maHocVien);
            }

            const string sqlLop = @"
                SELECT l.MaLop, l.TenLop, l.MaGV, l.MaMon, m.TenMon,
                       l.MaCa, l.MaPhong, l.SoBuoiTuan, l.NgayBD, l.NgayKT, l.MaThoiGian
                FROM LopHoc l
                LEFT JOIN MonHoc m ON m.MaMon = l.MaMon
                ORDER BY l.MaLop";

            DataTable lop = DbHelper.ExecuteQuery(sqlLop);
            foreach (DataRow dong in lop.Rows)
            {
                string maLop = dong["MaLop"].ToString() ?? "";

                var item = new LopXepLich
                {
                    MaLop = maLop,
                    TenLop = dong["TenLop"].ToString() ?? "",
                    MaGV = dong["MaGV"].ToString() ?? "",
                    MaMon = dong["MaMon"].ToString() ?? "",
                    TenMon = dong["TenMon"].ToString() ?? "",
                    MaCaUuTien = dong["MaCa"].ToString() ?? "",
                    MaPhongUuTien = dong["MaPhong"].ToString() ?? "",
                    SoBuoiTuan = dong["SoBuoiTuan"] == DBNull.Value ? 3 : Convert.ToInt32(dong["SoBuoiTuan"]),
                    NgayBD = dong["NgayBD"] == DBNull.Value ? DateTime.Today : Convert.ToDateTime(dong["NgayBD"]),
                    NgayKT = dong["NgayKT"] == DBNull.Value ? DateTime.Today : Convert.ToDateTime(dong["NgayKT"])
                };

                string maThoiGian = dong["MaThoiGian"].ToString() ?? "";
                if (maThoiGian != "" && thuTheoMau.TryGetValue(maThoiGian, out var dsThu))
                    item.ThuUuTien = new List<int>(dsThu);

                if (hocVienTheoLop.TryGetValue(maLop, out var dsHocVien))
                    item.HocVien = new List<string>(dsHocVien);

                duLieu.Lop.Add(item);
            }

            return duLieu;
        }

        //Ghi đè toàn bộ lịch trong một transaction
        public void LuuLich(IEnumerable<BuoiHoc> danhSach)
        {
            DbHelper.ExecuteTransaction((conn, tran) =>
            {
                using (var xoa = new SqlCommand("DELETE FROM BuoiHoc", conn, tran))
                    xoa.ExecuteNonQuery();

                const string sql = @"INSERT INTO BuoiHoc (MaLop, MaGV, Ngay, MaCa, MaPhong)
                                     VALUES (@MaLop, @MaGV, @Ngay, @MaCa, @MaPhong)";

                foreach (BuoiHoc buoi in danhSach)
                {
                    using var cmd = new SqlCommand(sql, conn, tran);
                    cmd.Parameters.Add(new SqlParameter("@MaLop", SqlDbType.VarChar, 20) { Value = buoi.MaLop });
                    cmd.Parameters.Add(new SqlParameter("@MaGV", SqlDbType.VarChar, 20) { Value = buoi.MaGV });
                    cmd.Parameters.Add(new SqlParameter("@Ngay", SqlDbType.Date) { Value = buoi.Ngay });
                    cmd.Parameters.Add(new SqlParameter("@MaCa", SqlDbType.VarChar, 20) { Value = buoi.MaCa });
                    cmd.Parameters.Add(new SqlParameter("@MaPhong", SqlDbType.VarChar, 20) { Value = buoi.MaPhong });
                    cmd.ExecuteNonQuery();
                }
            });
        }

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
