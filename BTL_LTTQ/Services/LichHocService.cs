using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace BTL_LTTQ.Services
{
    /// <summary>
    /// Sinh lịch học (bảng BuoiHoc) cho một lớp dựa trên Ca / Mẫu thời gian / Phòng đã gán.
    ///
    /// Lịch được tạo tự động khi lớp có học viên đăng ký (đăng ký tay trong form "Đăng ký lớp"
    /// hoặc nhập hàng loạt), nên thời khóa biểu luôn khớp với dữ liệu lớp học đang quản lý.
    ///
    /// Các hàm nhận (SqlConnection, SqlTransaction) để chạy chung transaction với thao tác
    /// đăng ký: hoặc cả hai thành công, hoặc cả hai được rollback.
    /// </summary>
    public static class LichHocService
    {
        /// <summary>Số tuần tối đa sinh lịch cho một lớp (tránh sinh hàng nghìn dòng cho lớp kéo dài nhiều năm).</summary>
        private const int SoTuanSinhLichToiDa = 12;

        /// <summary>
        /// Tạo các buổi học còn thiếu cho lớp. Trả về số buổi đã tạo.
        /// Bỏ qua các ngày bị trùng giáo viên/phòng với lớp khác (đúng ràng buộc unique của DB).
        /// </summary>
        public static int TaoBuoiHocChoLop(SqlConnection conn, SqlTransaction tran, string maLop)
        {
            if (conn == null || tran == null || string.IsNullOrWhiteSpace(maLop))
                return 0;

            // 1. Thông tin lớp: cần giáo viên, ca, phòng và khoảng ngày để sinh lịch.
            string maGv = "";
            string maCa = "";
            string maPhong = "";
            string maThoiGian = "";
            int soBuoiTuan = 3;
            DateTime ngayBd = DateTime.Today;
            DateTime ngayKt = DateTime.Today;

            using (var cmd = new SqlCommand(@"
                SELECT MaGV, MaCa, MaPhong, MaThoiGian, SoBuoiTuan, NgayBD, NgayKT
                FROM dbo.LopHoc WITH (UPDLOCK, HOLDLOCK)
                WHERE MaLop = @MaLop", conn, tran))
            {
                cmd.Parameters.Add(new SqlParameter("@MaLop", SqlDbType.VarChar, 20) { Value = maLop });

                using var reader = cmd.ExecuteReader();
                if (!reader.Read())
                    return 0;

                maGv = reader.IsDBNull(0) ? "" : reader.GetString(0);
                maCa = reader.IsDBNull(1) ? "" : reader.GetString(1);
                maPhong = reader.IsDBNull(2) ? "" : reader.GetString(2);
                maThoiGian = reader.IsDBNull(3) ? "" : reader.GetString(3);
                soBuoiTuan = reader.IsDBNull(4) ? 3 : Convert.ToInt32(reader.GetValue(4));
                ngayBd = reader.IsDBNull(5) ? DateTime.Today : Convert.ToDateTime(reader.GetValue(5)).Date;
                ngayKt = reader.IsDBNull(6) ? DateTime.Today : Convert.ToDateTime(reader.GetValue(6)).Date;
            }

            // Lớp chưa đủ thông tin (chưa có giáo viên / ca / phòng) thì chưa sinh được lịch.
            if (maGv == "" || maCa == "" || maPhong == "")
                return 0;

            if (ngayKt < ngayBd)
                return 0;

            // 2. Các thứ trong tuần của mẫu thời gian (2 = T2 ... 8 = CN).
            var thuCuaMau = new List<int>();
            using (var cmd = new SqlCommand(
                "SELECT Thu FROM dbo.ThoiGian_Thu WHERE MaThoiGian = @MaThoiGian ORDER BY Thu", conn, tran))
            {
                cmd.Parameters.Add(new SqlParameter("@MaThoiGian", SqlDbType.VarChar, 20) { Value = maThoiGian });

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.IsDBNull(0)) continue;
                    int thu = Convert.ToInt32(reader.GetValue(0));
                    if (thu >= 2 && thu <= 8)
                        thuCuaMau.Add(thu);
                }
            }

            // 3. Số buổi mỗi tuần: không vượt quá số buổi/tuần khai trong lớp.
            int soBuoiTuanThucTe = soBuoiTuan <= 0 ? 3 : soBuoiTuan;
            if (thuCuaMau.Count > 0 && thuCuaMau.Count < soBuoiTuanThucTe)
                soBuoiTuanThucTe = thuCuaMau.Count;

            // 4. Các buổi đã có của lớp (tránh tạo trùng khi đăng ký nhiều lần).
            var ngayDaCo = new HashSet<DateTime>();
            using (var cmd = new SqlCommand(
                "SELECT Ngay FROM dbo.BuoiHoc WHERE MaLop = @MaLop", conn, tran))
            {
                cmd.Parameters.Add(new SqlParameter("@MaLop", SqlDbType.VarChar, 20) { Value = maLop });

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                    ngayDaCo.Add(Convert.ToDateTime(reader.GetValue(0)).Date);
            }

            // 5. Duyệt các ngày trong khoảng lớp, sinh buổi theo các thứ đã chọn.
            DateTime ngayKetThuc = ngayBd.AddDays(SoTuanSinhLichToiDa * 7);
            if (ngayKetThuc > ngayKt)
                ngayKetThuc = ngayKt;

            int soBuoiDaTao = 0;
            var soBuoiTheoThu = new Dictionary<int, int>();

            for (DateTime ngay = ngayBd; ngay <= ngayKetThuc; ngay = ngay.AddDays(1))
            {
                int thu = ThuTrongTuan(ngay);

                bool thuDuocChon = thuCuaMau.Count > 0
                    ? thuCuaMau.Contains(thu)
                    : soBuoiTuanThucTe > 0;

                if (!thuDuocChon)
                    continue;

                // Không sinh quá số buổi mỗi tuần đã khai.
                soBuoiTheoThu.TryGetValue(thu, out int daTaoTrongTuan);
                if (daTaoTrongTuan >= soBuoiTuanThucTe)
                    continue;

                if (ngayDaCo.Contains(ngay))
                    continue;

                // Bỏ qua nếu giáo viên hoặc phòng đã bị lớp khác chiếm trong khung giờ này.
                if (BiTrungKhungGio(conn, tran, maLop, ngay, maCa, maGv, maPhong))
                    continue;

                using (var cmd = new SqlCommand(@"
                    INSERT INTO dbo.BuoiHoc (MaLop, MaGV, Ngay, MaCa, MaPhong)
                    VALUES (@MaLop, @MaGV, @Ngay, @MaCa, @MaPhong)", conn, tran))
                {
                    cmd.Parameters.Add(new SqlParameter("@MaLop", SqlDbType.VarChar, 20) { Value = maLop });
                    cmd.Parameters.Add(new SqlParameter("@MaGV", SqlDbType.VarChar, 20) { Value = maGv });
                    cmd.Parameters.Add(new SqlParameter("@Ngay", SqlDbType.Date) { Value = ngay });
                    cmd.Parameters.Add(new SqlParameter("@MaCa", SqlDbType.VarChar, 20) { Value = maCa });
                    cmd.Parameters.Add(new SqlParameter("@MaPhong", SqlDbType.VarChar, 20) { Value = maPhong });
                    cmd.ExecuteNonQuery();
                }

                ngayDaCo.Add(ngay);
                soBuoiTheoThu[thu] = daTaoTrongTuan + 1;
                soBuoiDaTao++;
            }

            return soBuoiDaTao;
        }

        /// <summary>Xóa toàn bộ buổi học của lớp (dùng khi lớp không còn học viên nào).</summary>
        public static void XoaBuoiHocCuaLop(SqlConnection conn, SqlTransaction tran, string maLop)
        {
            using var cmd = new SqlCommand("DELETE FROM dbo.BuoiHoc WHERE MaLop = @MaLop", conn, tran);
            cmd.Parameters.Add(new SqlParameter("@MaLop", SqlDbType.VarChar, 20) { Value = maLop });
            cmd.ExecuteNonQuery();
        }

        /// <summary>Kiểm tra giáo viên hoặc phòng đã bị lớp khác chiếm trong khung (ngày, ca).</summary>
        private static bool BiTrungKhungGio(SqlConnection conn, SqlTransaction tran, string maLop,
            DateTime ngay, string maCa, string maGv, string maPhong)
        {
            using var cmd = new SqlCommand(@"
                SELECT COUNT(*) FROM dbo.BuoiHoc
                WHERE Ngay = @Ngay AND MaCa = @MaCa AND MaLop <> @MaLop
                  AND (MaGV = @MaGV OR MaPhong = @MaPhong)", conn, tran);

            cmd.Parameters.Add(new SqlParameter("@Ngay", SqlDbType.Date) { Value = ngay });
            cmd.Parameters.Add(new SqlParameter("@MaCa", SqlDbType.VarChar, 20) { Value = maCa });
            cmd.Parameters.Add(new SqlParameter("@MaLop", SqlDbType.VarChar, 20) { Value = maLop });
            cmd.Parameters.Add(new SqlParameter("@MaGV", SqlDbType.VarChar, 20) { Value = maGv });
            cmd.Parameters.Add(new SqlParameter("@MaPhong", SqlDbType.VarChar, 20) { Value = maPhong });

            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        /// <summary>Đổi ngày trong tuần sang số theo quy ước của bảng ThoiGian_Thu (2 = T2 ... 8 = CN).</summary>
        private static int ThuTrongTuan(DateTime ngay)
        {
            return ((int)ngay.DayOfWeek + 6) % 7 + 2;
        }
    }
}
