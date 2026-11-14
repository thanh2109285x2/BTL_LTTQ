using BTL_LTTQ.DataAccessLayer;
using BTL_LTTQ.Models.Import;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace BTL_LTTQ.Repositories.Import
{
    /// <summary>
    /// Nạp một lần các dữ liệu tham chiếu cần cho việc kiểm tra trước khi nhập hàng loạt.
    /// Chỉ đọc dữ liệu, không ghi. Dữ liệu này chỉ dùng để báo lỗi sớm cho người dùng;
    /// khi ghi thật luôn kiểm tra lại trong transaction.
    /// </summary>
    public class ImportLookupRepository
    {
        private readonly string _connectionString;

        public ImportLookupRepository()
        {
            _connectionString = DbHelper.ConnectionString;
        }

        public ImportLookupRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public HashSet<string> MaHocVien { get; } = new(StringComparer.OrdinalIgnoreCase);
        public HashSet<string> MaGiaoVien { get; } = new(StringComparer.OrdinalIgnoreCase);
        public HashSet<string> MaNghe { get; } = new(StringComparer.OrdinalIgnoreCase);
        public HashSet<string> MaXepLoai { get; } = new(StringComparer.OrdinalIgnoreCase);
        public HashSet<string> MaChucDanh { get; } = new(StringComparer.OrdinalIgnoreCase);
        public HashSet<string> MaChuyenMon { get; } = new(StringComparer.OrdinalIgnoreCase);
        public HashSet<string> MaCoQuan { get; } = new(StringComparer.OrdinalIgnoreCase);
        public HashSet<string> MaTrinhDo { get; } = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>Thông tin lớp (kèm sĩ số hiện tại) theo mã lớp.</summary>
        public Dictionary<string, LopThongTinImport> LopHoc { get; } =
            new(StringComparer.OrdinalIgnoreCase);

        /// <summary>Các cặp (mã học viên, mã lớp) đã đăng ký trong database.</summary>
        public HashSet<string> CapDangKy { get; } = new(StringComparer.OrdinalIgnoreCase);

        public void Nap()
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            NapTap(conn, "SELECT MaHocVien FROM dbo.HocVien", MaHocVien);
            NapTap(conn, "SELECT MaGV FROM dbo.GiaoVien", MaGiaoVien);
            NapTap(conn, "SELECT MaNghe FROM dbo.NgheNghiep", MaNghe);
            NapTap(conn, "SELECT MaXepLoai FROM dbo.XepLoai", MaXepLoai);
            NapTap(conn, "SELECT MaChucDanh FROM dbo.ChucDanh", MaChucDanh);
            NapTap(conn, "SELECT MaChuyenMon FROM dbo.ChuyenMon", MaChuyenMon);
            NapTap(conn, "SELECT MaCQ FROM dbo.CoQuan", MaCoQuan);
            NapTap(conn, "SELECT MaTrinhDo FROM dbo.TrinhDo", MaTrinhDo);

            const string sqlLop = @"
                SELECT l.MaLop, l.TenLop, l.SiSoToiDa, l.TrangThai,
                       ISNULL((SELECT COUNT(*) FROM dbo.DangKy d WHERE d.MaLop = l.MaLop), 0) AS SiSo
                FROM dbo.LopHoc l";

            using (var cmd = new SqlCommand(sqlLop, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var lop = new LopThongTinImport
                    {
                        MaLop = reader.GetString(0),
                        TenLop = reader.IsDBNull(1) ? "" : reader.GetString(1),
                        SiSoToiDa = reader.IsDBNull(2) ? 0 : Convert.ToInt32(reader.GetValue(2)),
                        TrangThai = reader.IsDBNull(3) ? "" : reader.GetString(3),
                        SiSoHienTai = reader.IsDBNull(4) ? 0 : Convert.ToInt32(reader.GetValue(4))
                    };

                    if (!LopHoc.ContainsKey(lop.MaLop))
                        LopHoc[lop.MaLop] = lop;
                }
            }

            using (var cmd = new SqlCommand("SELECT MaHocVien, MaLop FROM dbo.DangKy", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (reader.IsDBNull(0) || reader.IsDBNull(1)) continue;
                    CapDangKy.Add(TaoKhoaCap(reader.GetString(0), reader.GetString(1)));
                }
            }
        }

        public static string TaoKhoaCap(string maHocVien, string maLop)
        {
            return maHocVien.Trim() + "\u001F" + maLop.Trim();
        }

        private static void NapTap(SqlConnection conn, string sql, HashSet<string> dich)
        {
            using var cmd = new SqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                if (reader.IsDBNull(0)) continue;
                string giaTri = Convert.ToString(reader.GetValue(0))?.Trim() ?? "";
                if (giaTri.Length > 0)
                    dich.Add(giaTri);
            }
        }

        /// <summary>Đếm sĩ số hiện tại của một lớp (dùng khi kiểm tra trong transaction).</summary>
        public static int DemSiSo(SqlConnection conn, SqlTransaction tran, string maLop)
        {
            using var cmd = new SqlCommand(
                "SELECT COUNT(*) FROM dbo.DangKy WHERE MaLop = @MaLop", conn, tran);
            cmd.Parameters.Add(new SqlParameter("@MaLop", SqlDbType.VarChar, 20) { Value = maLop });
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        /// <summary>Đọc thông tin lớp và khóa dòng để tránh hai phiên cùng vượt sĩ số.</summary>
        public static LopThongTinImport? DocLopDangKhoa(SqlConnection conn, SqlTransaction tran, string maLop)
        {
            using var cmd = new SqlCommand(@"
                SELECT l.MaLop, l.TenLop, l.SiSoToiDa, l.TrangThai,
                       (SELECT COUNT(*) FROM dbo.DangKy d WHERE d.MaLop = l.MaLop) AS SiSo
                FROM dbo.LopHoc l WITH (UPDLOCK, HOLDLOCK)
                WHERE l.MaLop = @MaLop", conn, tran);

            cmd.Parameters.Add(new SqlParameter("@MaLop", SqlDbType.VarChar, 20) { Value = maLop });

            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
                return null;

            return new LopThongTinImport
            {
                MaLop = reader.GetString(0),
                TenLop = reader.IsDBNull(1) ? "" : reader.GetString(1),
                SiSoToiDa = reader.IsDBNull(2) ? 0 : Convert.ToInt32(reader.GetValue(2)),
                TrangThai = reader.IsDBNull(3) ? "" : reader.GetString(3),
                SiSoHienTai = reader.IsDBNull(4) ? 0 : Convert.ToInt32(reader.GetValue(4))
            };
        }

        /// <summary>Kiểm tra một mã tồn tại trong bảng tham chiếu (dùng trong transaction).</summary>
        public static bool TonTai(SqlConnection conn, SqlTransaction tran, string bang, string cot, string giaTri)
        {
            // Tên bảng/cột do code nội bộ truyền vào, không lấy từ tệp người dùng.
            using var cmd = new SqlCommand(
                $"SELECT TOP 1 1 FROM dbo.{bang} WHERE {cot} = @GiaTri", conn, tran);
            cmd.Parameters.Add(new SqlParameter("@GiaTri", SqlDbType.VarChar, 20) { Value = giaTri });
            object? kq = cmd.ExecuteScalar();
            return kq != null && kq != DBNull.Value;
        }
    }
}
