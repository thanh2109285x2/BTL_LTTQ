using BTL_LTTQ.Models;
using BTL_LTTQ.Common;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace BTL_LTTQ.DAL
{
    public sealed class DanhMucKhacRepository
    {
        private sealed record CauHinhBang(string Bang, string Khoa, string[] Cot);

        private static readonly IReadOnlyDictionary<DanhMucLoai, CauHinhBang> DanhSachTrang =
            new Dictionary<DanhMucLoai, CauHinhBang>
            {
                [DanhMucLoai.NgheNghiep] = new("NgheNghiep", "MaNghe", new[] { "MaNghe", "TenNghe" }),
                [DanhMucLoai.ChuyenMon] = new("ChuyenMon", "MaChuyenMon", new[] { "MaChuyenMon", "TenChuyenMon" }),
                [DanhMucLoai.TrinhDo] = new("TrinhDo", "MaTrinhDo", new[] { "MaTrinhDo", "TenTrinhDo" }),
                [DanhMucLoai.CoQuan] = new("CoQuan", "MaCQ", new[] { "MaCQ", "TenCQ", "DiaChi", "DienThoai" }),
                [DanhMucLoai.PhongHoc] = new("PhongHoc", "MaPhong", new[] { "MaPhong", "TenPhong", "DiaChi", "SucChua" }),
                [DanhMucLoai.CaHoc] = new("CaHoc", "MaCa", new[] { "MaCa", "TenCa", "GioBatDau", "GioKetThuc" }),
                [DanhMucLoai.ThoiGian] = new("ThoiGian", "MaThoiGian", new[] { "MaThoiGian", "TenThoiGian" })
            };

        public DataTable LayDanhSach(DanhMucLoai loai)
        {
            CauHinhBang bang = LayCauHinh(loai);
            DataTable dt = DbHelper.ExecuteQuery($"SELECT {string.Join(", ", bang.Cot)} FROM dbo.{bang.Bang} ORDER BY {bang.Khoa}");
            if (loai == DanhMucLoai.ThoiGian) BatauCotCacThu(dt);
            return dt;
        }

        public bool TonTai(DanhMucLoai loai, string ma)
        {
            CauHinhBang bang = LayCauHinh(loai);
            return DbHelper.ExecuteScalar($"SELECT 1 FROM dbo.{bang.Bang} WHERE {bang.Khoa} = @Ma",
                new SqlParameter("@Ma", SqlDbType.VarChar, 20) { Value = ma }) != null;
        }

        public string LayMaThoiGianKeTiep()
        {
            using var conn = new SqlConnection(DbHelper.ConnectionString);
            conn.Open();
            using var cmd = new SqlCommand("SELECT MaThoiGian FROM dbo.ThoiGian WHERE MaThoiGian LIKE @Prefix", conn);
            cmd.Parameters.Add(new SqlParameter("@Prefix", SqlDbType.VarChar, 20) { Value = "TG%" });
            using SqlDataReader reader = cmd.ExecuteReader();
            int maLonNhat = 0;
            while (reader.Read())
            {
                string ma = reader.GetString(0);
                if (ma.StartsWith("TG", StringComparison.OrdinalIgnoreCase) &&
                    int.TryParse(ma.AsSpan(2), NumberStyles.None, CultureInfo.InvariantCulture, out int so))
                    maLonNhat = Math.Max(maLonNhat, so);
            }
            return "TG" + checked(maLonNhat + 1).ToString(CultureInfo.InvariantCulture);
        }

        public void Them(DanhMucLoai loai, IReadOnlyDictionary<string, object?> giaTri, IReadOnlyCollection<byte>? cacThu = null)
        {
            CauHinhBang bang = LayCauHinh(loai);
            try
            {
                if (loai == DanhMucLoai.ThoiGian)
                {
                    DbHelper.ExecuteTransaction((conn, tran) =>
                    {
                        string maThoiGian = Convert.ToString(giaTri[bang.Khoa])!;
                        KiemTraTapThuTrung(conn, tran, null, cacThu ?? Array.Empty<byte>());
                        ThemDong(conn, tran, bang, giaTri);
                        LuuCacThu(conn, tran, maThoiGian, cacThu ?? Array.Empty<byte>());
                    });
                    return;
                }
                using var conn = new SqlConnection(DbHelper.ConnectionString);
                conn.Open();
                string parameters = string.Join(", ", Enumerable.Range(0, bang.Cot.Length).Select(i => "@v" + i));
                using var cmd = new SqlCommand($"INSERT INTO dbo.{bang.Bang} ({string.Join(", ", bang.Cot)}) VALUES ({parameters})", conn);
                ThemThamSo(cmd, bang, giaTri);
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex) { throw DichLoiSql(ex); }
        }

        public void Sua(DanhMucLoai loai, string maGoc, IReadOnlyDictionary<string, object?> giaTri, IReadOnlyCollection<byte>? cacThu = null)
        {
            CauHinhBang bang = LayCauHinh(loai);
            try
            {
                if (loai == DanhMucLoai.ThoiGian)
                {
                    DbHelper.ExecuteTransaction((conn, tran) =>
                    {
                        IReadOnlyCollection<byte> thuMoi = cacThu ?? Array.Empty<byte>();
                        KiemTraDoiThuDuocPhep(conn, tran, maGoc, thuMoi);
                        KiemTraTapThuTrung(conn, tran, maGoc, thuMoi);
                        using var update = new SqlCommand("UPDATE dbo.ThoiGian SET TenThoiGian = @Ten WHERE MaThoiGian = @Ma", conn, tran);
                        update.Parameters.Add(new SqlParameter("@Ten", SqlDbType.NVarChar, 100) { Value = giaTri["TenThoiGian"] ?? DBNull.Value });
                        update.Parameters.Add(new SqlParameter("@Ma", SqlDbType.VarChar, 20) { Value = maGoc });
                        if (update.ExecuteNonQuery() != 1) throw new DanhMucLoiException(DanhMucLoi.KhongTimThay, "Không tìm thấy danh mục cần sửa.");
                        using var delete = new SqlCommand("DELETE FROM dbo.ThoiGian_Thu WHERE MaThoiGian = @Ma", conn, tran);
                        delete.Parameters.Add(new SqlParameter("@Ma", SqlDbType.VarChar, 20) { Value = maGoc });
                        delete.ExecuteNonQuery();
                        LuuCacThu(conn, tran, maGoc, cacThu ?? Array.Empty<byte>());
                    });
                    return;
                }

                using var connection = new SqlConnection(DbHelper.ConnectionString);
                connection.Open();
                string[] columns = bang.Cot.Skip(1).ToArray();
                string set = string.Join(", ", columns.Select((column, i) => $"{column} = @v{i}"));
                using var command = new SqlCommand($"UPDATE dbo.{bang.Bang} SET {set} WHERE {bang.Khoa} = @Ma", connection);
                for (int i = 0; i < columns.Length; i++) ThemThamSo(command, columns[i], "@v" + i, giaTri);
                command.Parameters.Add(new SqlParameter("@Ma", SqlDbType.VarChar, 20) { Value = maGoc });
                if (command.ExecuteNonQuery() != 1) throw new DanhMucLoiException(DanhMucLoi.KhongTimThay, "Không tìm thấy danh mục cần sửa.");
            }
            catch (SqlException ex) { throw DichLoiSql(ex); }
        }

        public void Xoa(DanhMucLoai loai, string ma)
        {
            CauHinhBang bang = LayCauHinh(loai);
            try
            {
                if (loai == DanhMucLoai.ThoiGian)
                {
                    DbHelper.ExecuteTransaction((conn, tran) =>
                    {
                        using var deleteDays = new SqlCommand("DELETE FROM dbo.ThoiGian_Thu WHERE MaThoiGian = @Ma", conn, tran);
                        deleteDays.Parameters.Add(new SqlParameter("@Ma", SqlDbType.VarChar, 20) { Value = ma });
                        deleteDays.ExecuteNonQuery();
                        using var deleteTime = new SqlCommand("DELETE FROM dbo.ThoiGian WHERE MaThoiGian = @Ma", conn, tran);
                        deleteTime.Parameters.Add(new SqlParameter("@Ma", SqlDbType.VarChar, 20) { Value = ma });
                        if (deleteTime.ExecuteNonQuery() != 1) throw new DanhMucLoiException(DanhMucLoi.KhongTimThay, "Không tìm thấy danh mục cần xóa.");
                    });
                    return;
                }
                using var conn = new SqlConnection(DbHelper.ConnectionString);
                conn.Open();
                using var cmd = new SqlCommand($"DELETE FROM dbo.{bang.Bang} WHERE {bang.Khoa} = @Ma", conn);
                cmd.Parameters.Add(new SqlParameter("@Ma", SqlDbType.VarChar, 20) { Value = ma });
                if (cmd.ExecuteNonQuery() != 1) throw new DanhMucLoiException(DanhMucLoi.KhongTimThay, "Không tìm thấy danh mục cần xóa.");
            }
            catch (SqlException ex) { throw DichLoiSql(ex); }
        }

        private static void KiemTraDoiThuDuocPhep(SqlConnection conn, SqlTransaction tran, string ma, IReadOnlyCollection<byte> cacThuMoi)
        {
            using var lop = new SqlCommand("SELECT COUNT(*) FROM dbo.LopHoc WITH (UPDLOCK, HOLDLOCK) WHERE MaThoiGian = @Ma", conn, tran);
            lop.Parameters.Add(new SqlParameter("@Ma", SqlDbType.VarChar, 20) { Value = ma });
            if (Convert.ToInt32(lop.ExecuteScalar()) == 0) return;
            using var oldCmd = new SqlCommand("SELECT Thu FROM dbo.ThoiGian_Thu WITH (UPDLOCK, HOLDLOCK) WHERE MaThoiGian = @Ma ORDER BY Thu", conn, tran);
            oldCmd.Parameters.Add(new SqlParameter("@Ma", SqlDbType.VarChar, 20) { Value = ma });
            using SqlDataReader reader = oldCmd.ExecuteReader();
            var old = new List<byte>();
            while (reader.Read()) old.Add(reader.GetByte(0));
            reader.Close();
            if (!old.ToHashSet().SetEquals(cacThuMoi))
                throw new DanhMucLoiException(DanhMucLoi.KhongTheDoiThuKhiDaDung, "Thời gian đã được lớp học sử dụng nên không thể thay đổi các thứ trong tuần; chỉ được sửa tên.");
        }

        private static void KiemTraTapThuTrung(
            SqlConnection conn, SqlTransaction tran, string? maBoQua, IReadOnlyCollection<byte> cacThuMoi)
        {
            var thuMoi = cacThuMoi.ToHashSet();
            using var cmd = new SqlCommand(@"
                SELECT tg.MaThoiGian, tt.Thu
                FROM dbo.ThoiGian tg WITH (UPDLOCK, HOLDLOCK)
                LEFT JOIN dbo.ThoiGian_Thu tt WITH (UPDLOCK, HOLDLOCK) ON tt.MaThoiGian = tg.MaThoiGian
                WHERE @MaBoQua IS NULL OR tg.MaThoiGian <> @MaBoQua
                ORDER BY tg.MaThoiGian, tt.Thu", conn, tran);
            cmd.Parameters.Add(new SqlParameter("@MaBoQua", SqlDbType.VarChar, 20) { Value = (object?)maBoQua ?? DBNull.Value });
            using SqlDataReader reader = cmd.ExecuteReader();
            var thuTheoMa = new Dictionary<string, HashSet<byte>>(StringComparer.OrdinalIgnoreCase);
            while (reader.Read())
            {
                string ma = reader.GetString(0);
                if (!thuTheoMa.TryGetValue(ma, out HashSet<byte>? thu))
                    thuTheoMa[ma] = thu = new HashSet<byte>();
                if (!reader.IsDBNull(1)) thu.Add(reader.GetByte(1));
            }
            reader.Close();

            string? maTrung = thuTheoMa.FirstOrDefault(x => x.Value.SetEquals(thuMoi)).Key;
            if (maTrung != null)
                throw new DanhMucLoiException(DanhMucLoi.TrungTapThu, $"Đã có mục này (Mã {maTrung})");
        }

        private static void LuuCacThu(SqlConnection conn, SqlTransaction tran, string ma, IEnumerable<byte> cacThu)
        {
            foreach (byte thu in cacThu.Distinct().OrderBy(x => x))
            {
                using var cmd = new SqlCommand("INSERT INTO dbo.ThoiGian_Thu (MaThoiGian, Thu) VALUES (@Ma, @Thu)", conn, tran);
                cmd.Parameters.Add(new SqlParameter("@Ma", SqlDbType.VarChar, 20) { Value = ma });
                cmd.Parameters.Add(new SqlParameter("@Thu", SqlDbType.TinyInt) { Value = thu });
                cmd.ExecuteNonQuery();
            }
        }

        private static void ThemDong(SqlConnection conn, SqlTransaction tran, CauHinhBang bang, IReadOnlyDictionary<string, object?> giaTri)
        {
            string parameters = string.Join(", ", Enumerable.Range(0, bang.Cot.Length).Select(i => "@v" + i));
            using var cmd = new SqlCommand($"INSERT INTO dbo.{bang.Bang} ({string.Join(", ", bang.Cot)}) VALUES ({parameters})", conn, tran);
            ThemThamSo(cmd, bang, giaTri);
            cmd.ExecuteNonQuery();
        }

        private static void ThemThamSo(SqlCommand cmd, CauHinhBang bang, IReadOnlyDictionary<string, object?> giaTri)
        {
            for (int i = 0; i < bang.Cot.Length; i++) ThemThamSo(cmd, bang.Cot[i], "@v" + i, giaTri);
        }

        private static void ThemThamSo(SqlCommand cmd, string cot, string parameter, IReadOnlyDictionary<string, object?> giaTri)
        {
            object value = giaTri.TryGetValue(cot, out object? raw) && raw != null ? raw : DBNull.Value;
            SqlParameter p = cot switch
            {
                "MaNghe" or "MaChuyenMon" or "MaTrinhDo" or "MaCQ" or "MaPhong" or "MaCa" or "MaThoiGian" => new SqlParameter(parameter, SqlDbType.VarChar, 20),
                "DienThoai" => new SqlParameter(parameter, SqlDbType.VarChar, 20),
                "TenCa" => new SqlParameter(parameter, SqlDbType.NVarChar, 50),
                "TenNghe" or "TenChuyenMon" or "TenTrinhDo" or "TenCQ" or "TenPhong" or "TenThoiGian" => new SqlParameter(parameter, SqlDbType.NVarChar, 100),
                "DiaChi" => new SqlParameter(parameter, SqlDbType.NVarChar, 200),
                "SucChua" => new SqlParameter(parameter, SqlDbType.Int),
                "GioBatDau" or "GioKetThuc" => new SqlParameter(parameter, SqlDbType.Time),
                _ => throw new InvalidOperationException("Cột danh mục không nằm trong allowlist.")
            };
            p.Value = value;
            cmd.Parameters.Add(p);
        }

        private static void BatauCotCacThu(DataTable dt)
        {
            dt.Columns.Add(DanhMucKhacConst.CotCacThu, typeof(string));
            dt.Columns.Add(DanhMucKhacConst.CotCacThuMa, typeof(string));
            dt.Columns.Add(DanhMucKhacConst.CotDaDuocSuDung, typeof(bool));
            using var conn = new SqlConnection(DbHelper.ConnectionString);
            conn.Open();
            using var cmd = new SqlCommand(@"
                SELECT tg.MaThoiGian, tt.Thu,
                    CASE WHEN EXISTS (SELECT 1 FROM dbo.LopHoc l WHERE l.MaThoiGian = tg.MaThoiGian) THEN 1 ELSE 0 END
                FROM dbo.ThoiGian tg
                LEFT JOIN dbo.ThoiGian_Thu tt ON tt.MaThoiGian = tg.MaThoiGian
                ORDER BY tg.MaThoiGian, tt.Thu", conn);
            using SqlDataReader reader = cmd.ExecuteReader();
            var thuTheoMa = new Dictionary<string, List<byte>>(StringComparer.OrdinalIgnoreCase);
            var duocSuDung = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            while (reader.Read())
            {
                string ma = reader.GetString(0);
                if (!thuTheoMa.TryGetValue(ma, out List<byte>? thu)) thuTheoMa[ma] = thu = new List<byte>();
                if (!reader.IsDBNull(1)) thu.Add(reader.GetByte(1));
                duocSuDung[ma] = Convert.ToBoolean(reader.GetValue(2));
            }
            foreach (DataRow row in dt.Rows)
            {
                string ma = Convert.ToString(row["MaThoiGian"]) ?? "";
                List<byte> thu = thuTheoMa.TryGetValue(ma, out List<byte>? daLuu) ? daLuu : new List<byte>();
                row[DanhMucKhacConst.CotCacThu] = ThuTrongTuanConst.TaoTenThoiGian(thu);
                row[DanhMucKhacConst.CotCacThuMa] = string.Join(",", thu);
                row[DanhMucKhacConst.CotDaDuocSuDung] = duocSuDung.TryGetValue(ma, out bool dangDung) && dangDung;
            }
        }

        private static CauHinhBang LayCauHinh(DanhMucLoai loai) =>
            DanhSachTrang.TryGetValue(loai, out CauHinhBang? bang) ? bang : throw new ArgumentOutOfRangeException(nameof(loai));

        private static DanhMucLoiException DichLoiSql(SqlException ex)
        {
            if (ex.Number is 2601 or 2627) return new DanhMucLoiException(DanhMucLoi.TrungKhoa, "Mã đã tồn tại.", ex);
            if (ex.Number == 547) return new DanhMucLoiException(DanhMucLoi.DangDuocSuDung, "Đang được sử dụng nên không thể xóa.", ex);
            return new DanhMucLoiException(DanhMucLoi.DangDuocSuDung, "Không thể hoàn tất thao tác danh mục.", ex);
        }
    }
}
