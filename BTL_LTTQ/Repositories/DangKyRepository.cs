using BTL_LTTQ.DataAccessLayer;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace BTL_LTTQ.Repositories
{
    public class DangKyRepository
    {
        //Danh sách lớp kèm sĩ số và trạng thái đăng ký của học viên
        public DataTable LayLopChoHocVien(string maHocVien, string tuKhoa = "")
        {
            const string sql = @"
                SELECT l.MaLop, l.TenLop, m.TenMon, g.TenGV,
                       l.NgayBD, l.NgayKT, l.SiSoToiDa,
                       (SELECT COUNT(*) FROM DangKy d WHERE d.MaLop = l.MaLop) AS SiSo,
                       CASE WHEN EXISTS (SELECT 1 FROM DangKy d2
                                         WHERE d2.MaLop = l.MaLop AND d2.MaHocVien = @MaHocVien)
                            THEN 1 ELSE 0 END AS DaDangKy
                FROM LopHoc l
                LEFT JOIN MonHoc m ON l.MaMon = m.MaMon
                LEFT JOIN GiaoVien g ON l.MaGV = g.MaGV
                WHERE @TuKhoa = ''
                   OR l.TenLop COLLATE Vietnamese_CI_AI LIKE '%' + @TuKhoa + '%'
                   OR m.TenMon COLLATE Vietnamese_CI_AI LIKE '%' + @TuKhoa + '%'
                   OR g.TenGV COLLATE Vietnamese_CI_AI LIKE '%' + @TuKhoa + '%'
                ORDER BY l.MaLop";

            return DbHelper.ExecuteQuery(sql,
                new SqlParameter("@MaHocVien", SqlDbType.VarChar, 20) { Value = maHocVien },
                new SqlParameter("@TuKhoa", SqlDbType.NVarChar, 200) { Value = tuKhoa ?? "" });
        }

        public DataTable LayLopCuaHocVien(string maHocVien)
        {
            const string sql = @"
                SELECT d.MaLop, l.TenLop, m.TenMon, g.TenGV,
                       l.NgayBD, l.NgayKT, d.NgayDangKy
                FROM DangKy d
                JOIN LopHoc l ON l.MaLop = d.MaLop
                LEFT JOIN MonHoc m ON m.MaMon = l.MaMon
                LEFT JOIN GiaoVien g ON g.MaGV = l.MaGV
                WHERE d.MaHocVien = @MaHocVien
                ORDER BY d.NgayDangKy DESC, d.MaLop";

            return DbHelper.ExecuteQuery(sql,
                new SqlParameter("@MaHocVien", SqlDbType.VarChar, 20) { Value = maHocVien });
        }

        public int DemSiSo(string maLop)
        {
            object? ketQua = DbHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM DangKy WHERE MaLop = @MaLop",
                new SqlParameter("@MaLop", SqlDbType.VarChar, 20) { Value = maLop });

            return Convert.ToInt32(ketQua);
        }

        //Đăng ký lớp, chặn vượt sĩ số bằng khóa dòng LopHoc trong transaction
        public string Them(string maHocVien, string maLop)
        {
            try
            {
                DbHelper.ExecuteTransaction((conn, tran) =>
                {
                    int siSoToiDa;

                    using (var cmd = new SqlCommand(
                        "SELECT SiSoToiDa FROM LopHoc WITH (UPDLOCK, HOLDLOCK) WHERE MaLop = @MaLop",
                        conn, tran))
                    {
                        cmd.Parameters.Add(new SqlParameter("@MaLop", SqlDbType.VarChar, 20) { Value = maLop });
                        object? kq = cmd.ExecuteScalar();
                        if (kq == null || kq == DBNull.Value)
                            throw new InvalidOperationException("Lớp học không tồn tại.");
                        siSoToiDa = Convert.ToInt32(kq);
                    }

                    int siSo;

                    using (var cmd = new SqlCommand(
                        "SELECT COUNT(*) FROM DangKy WHERE MaLop = @MaLop", conn, tran))
                    {
                        cmd.Parameters.Add(new SqlParameter("@MaLop", SqlDbType.VarChar, 20) { Value = maLop });
                        siSo = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    if (siSo >= siSoToiDa)
                        throw new InvalidOperationException($"Lớp đã đủ sĩ số ({siSo}/{siSoToiDa}).");

                    using (var cmd = new SqlCommand(
                        "INSERT INTO DangKy (MaHocVien, MaLop) VALUES (@MaHocVien, @MaLop)", conn, tran))
                    {
                        cmd.Parameters.Add(new SqlParameter("@MaHocVien", SqlDbType.VarChar, 20) { Value = maHocVien });
                        cmd.Parameters.Add(new SqlParameter("@MaLop", SqlDbType.VarChar, 20) { Value = maLop });
                        cmd.ExecuteNonQuery();
                    }
                });

                return "";
            }
            catch (InvalidOperationException ex)
            {
                return ex.Message;
            }
            catch (SqlException ex) when (ex.Number is 2627 or 2601)
            {
                return "Bạn đã đăng ký lớp này rồi.";
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                return "Không đăng ký được: lớp học hoặc học viên không hợp lệ.";
            }
            catch (Exception ex)
            {
                return "Không đăng ký được: " + ex.Message;
            }
        }

        public string Huy(string maHocVien, string maLop)
        {
            try
            {
                int soDong = DbHelper.ExecuteNonQuery(
                    "DELETE FROM DangKy WHERE MaHocVien = @MaHocVien AND MaLop = @MaLop",
                    new SqlParameter("@MaHocVien", SqlDbType.VarChar, 20) { Value = maHocVien },
                    new SqlParameter("@MaLop", SqlDbType.VarChar, 20) { Value = maLop });

                return soDong == 0 ? "Bạn chưa đăng ký lớp này." : "";
            }
            catch (Exception ex)
            {
                return "Không hủy được: " + ex.Message;
            }
        }
    }
}
