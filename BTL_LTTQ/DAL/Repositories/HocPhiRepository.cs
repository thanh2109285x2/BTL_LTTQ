using BTL_LTTQ.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BTL_LTTQ.DAL
{
    public sealed class HocPhiRepository
    {
        public DataTable LayLopChuaLapPhieu(string maHocVien)
        {
            const string sql = @"
                SELECT d.MaHocVien, d.MaLop, l.TenLop, m.TenMon, m.HocPhi
                FROM dbo.DangKy d
                INNER JOIN dbo.LopHoc l ON l.MaLop = d.MaLop
                INNER JOIN dbo.MonHoc m ON m.MaMon = l.MaMon
                WHERE d.MaHocVien = @MaHocVien AND d.MaPhieu IS NULL
                ORDER BY l.TenLop, d.MaLop";
            return DbHelper.ExecuteQuery(sql, new SqlParameter("@MaHocVien", SqlDbType.VarChar, 20) { Value = maHocVien });
        }

        public DataTable TimHocVien(string tuKhoa)
        {
            const string sql = @"
                SELECT MaHocVien, TenHocVien
                FROM dbo.HocVien
                WHERE @TuKhoa = ''
                   OR MaHocVien COLLATE Vietnamese_CI_AI LIKE '%' + @TuKhoa + '%'
                   OR TenHocVien COLLATE Vietnamese_CI_AI LIKE '%' + @TuKhoa + '%'
                ORDER BY TenHocVien, MaHocVien";
            return DbHelper.ExecuteQuery(sql, new SqlParameter("@TuKhoa", SqlDbType.NVarChar, 200) { Value = tuKhoa ?? "" });
        }

        public DataTable LayPhieuThu(string? maHocVien, bool chiChoThanhToan = false)
        {
            const string sql = @"
                SELECT p.MaPhieu, p.MaHocVien, h.TenHocVien, p.NgayLap, p.TongTien,
                       p.NoiDungCK, p.TrangThai, p.NgayThu, p.NguoiThu
                FROM dbo.PhieuThu p
                INNER JOIN dbo.HocVien h ON h.MaHocVien = p.MaHocVien
                WHERE (@MaHocVien IS NULL OR p.MaHocVien = @MaHocVien)
                  AND (@ChiChoThanhToan = 0 OR p.TrangThai = 0)
                ORDER BY p.NgayLap DESC, p.MaPhieu DESC";
            return DbHelper.ExecuteQuery(sql,
                new SqlParameter("@MaHocVien", SqlDbType.VarChar, 20) { Value = (object?)maHocVien ?? DBNull.Value },
                new SqlParameter("@ChiChoThanhToan", SqlDbType.Bit) { Value = chiChoThanhToan });
        }

        public PhieuThuMoi TaoPhieuThu(string maHocVien, IReadOnlyCollection<string> cacMaLop)
        {
            if (cacMaLop.Count == 0) throw new InvalidOperationException("Vui lòng chọn ít nhất một lớp.");
            PhieuThuMoi? ketQua = null;
            DbHelper.ExecuteTransaction((conn, tran) =>
            {
                var maLopDaGap = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                decimal tongTien = 0m;
                foreach (string maLop in cacMaLop)
                {
                    if (string.IsNullOrWhiteSpace(maLop) || !maLopDaGap.Add(maLop))
                        throw new InvalidOperationException("Danh sách lớp không hợp lệ hoặc có lớp bị lặp.");
                    using var cmd = new SqlCommand(@"
                        SELECT d.MaPhieu, m.HocPhi
                        FROM dbo.DangKy d WITH (UPDLOCK, HOLDLOCK)
                        INNER JOIN dbo.LopHoc l ON l.MaLop = d.MaLop
                        INNER JOIN dbo.MonHoc m ON m.MaMon = l.MaMon
                        WHERE d.MaHocVien = @MaHocVien AND d.MaLop = @MaLop", conn, tran);
                    cmd.Parameters.Add(new SqlParameter("@MaHocVien", SqlDbType.VarChar, 20) { Value = maHocVien });
                    cmd.Parameters.Add(new SqlParameter("@MaLop", SqlDbType.VarChar, 20) { Value = maLop });
                    using SqlDataReader reader = cmd.ExecuteReader();
                    if (!reader.Read()) throw new InvalidOperationException($"Không tìm thấy đăng ký lớp {maLop} của học viên.");
                    if (!reader.IsDBNull(0)) throw new InvalidOperationException($"Lớp {maLop} đã được lập phiếu thu.");
                    if (reader.IsDBNull(1)) throw new InvalidOperationException($"Lớp {maLop} chưa có học phí cấu hình.");
                    decimal hocPhi = reader.GetDecimal(1);
                    reader.Close();
                    tongTien = checked(tongTien + hocPhi);
                }
                if (tongTien <= 0m) throw new InvalidOperationException("Tổng học phí bằng 0; không thể tạo phiếu thu.");

                int maPhieu;
                using (var cmd = new SqlCommand(@"
                    INSERT INTO dbo.PhieuThu (MaHocVien, TongTien, TrangThai)
                    VALUES (@MaHocVien, @TongTien, 0);
                    SELECT CAST(SCOPE_IDENTITY() AS int);", conn, tran))
                {
                    cmd.Parameters.Add(new SqlParameter("@MaHocVien", SqlDbType.VarChar, 20) { Value = maHocVien });
                    cmd.Parameters.Add(new SqlParameter("@TongTien", SqlDbType.Decimal) { Precision = 18, Scale = 0, Value = tongTien });
                    maPhieu = Convert.ToInt32(cmd.ExecuteScalar());
                }

                foreach (string maLop in maLopDaGap)
                {
                    using var cmd = new SqlCommand(@"
                        UPDATE dbo.DangKy SET MaPhieu = @MaPhieu
                        WHERE MaHocVien = @MaHocVien AND MaLop = @MaLop AND MaPhieu IS NULL", conn, tran);
                    cmd.Parameters.Add(new SqlParameter("@MaPhieu", SqlDbType.Int) { Value = maPhieu });
                    cmd.Parameters.Add(new SqlParameter("@MaHocVien", SqlDbType.VarChar, 20) { Value = maHocVien });
                    cmd.Parameters.Add(new SqlParameter("@MaLop", SqlDbType.VarChar, 20) { Value = maLop });
                    if (cmd.ExecuteNonQuery() != 1) throw new InvalidOperationException("Đăng ký đã được lập phiếu đồng thời; thao tác đã được hoàn tác.");
                }

                using var read = new SqlCommand("SELECT NoiDungCK FROM dbo.PhieuThu WHERE MaPhieu = @MaPhieu", conn, tran);
                read.Parameters.Add(new SqlParameter("@MaPhieu", SqlDbType.Int) { Value = maPhieu });
                string noiDung = Convert.ToString(read.ExecuteScalar()) ?? "";
                if (noiDung == "") throw new InvalidOperationException("Không đọc được nội dung chuyển khoản của phiếu.");
                ketQua = new PhieuThuMoi(maPhieu, noiDung, tongTien);
            });
            return ketQua!;
        }

        public void XacNhanDaThu(int maPhieu, string nguoiThu)
        {
            int affected = DbHelper.ExecuteNonQuery(@"
                UPDATE dbo.PhieuThu
                SET TrangThai = 1, NgayThu = GETDATE(), NguoiThu = @NguoiThu
                WHERE MaPhieu = @MaPhieu AND TrangThai = 0",
                new SqlParameter("@NguoiThu", SqlDbType.VarChar, 50) { Value = nguoiThu },
                new SqlParameter("@MaPhieu", SqlDbType.Int) { Value = maPhieu });
            if (affected != 1) throw new InvalidOperationException("Phiếu không còn ở trạng thái chờ thanh toán.");
        }

        public void HuyPhieu(int maPhieu, string maHocVien)
        {
            DbHelper.ExecuteTransaction((conn, tran) =>
            {
                using var cmd = new SqlCommand(@"
                    UPDATE dbo.PhieuThu SET TrangThai = 2
                    WHERE MaPhieu = @MaPhieu AND MaHocVien = @MaHocVien AND TrangThai = 0", conn, tran);
                cmd.Parameters.Add(new SqlParameter("@MaPhieu", SqlDbType.Int) { Value = maPhieu });
                cmd.Parameters.Add(new SqlParameter("@MaHocVien", SqlDbType.VarChar, 20) { Value = maHocVien });
                if (cmd.ExecuteNonQuery() != 1) throw new InvalidOperationException("Chỉ được hủy phiếu đang chờ thanh toán của chính học viên.");

                using var reset = new SqlCommand(@"
                    UPDATE dbo.DangKy SET MaPhieu = NULL
                    WHERE MaPhieu = @MaPhieu AND MaHocVien = @MaHocVien", conn, tran);
                reset.Parameters.Add(new SqlParameter("@MaPhieu", SqlDbType.Int) { Value = maPhieu });
                reset.Parameters.Add(new SqlParameter("@MaHocVien", SqlDbType.VarChar, 20) { Value = maHocVien });
                reset.ExecuteNonQuery();
            });
        }
    }
}
