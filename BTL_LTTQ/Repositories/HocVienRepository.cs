using BTL_LTTQ.DataAccessLayer;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BTL_LTTQ.Repositories
{
    public class HocVienRepository
    {
        public DataTable TimKiem(string tuKhoa = "", string? maLop = null,
            string? maNghe = null, string? maXepLoai = null)
        {
            const string sql = @"SELECT hv.MaHocVien, hv.TenHocVien, hv.NgaySinh, hv.GioiTinh,
                                        hv.DienThoai, hv.DiaChi, l.TenLop, n.TenNghe,
                                        hv.NgayNopHocPhi, hv.Diem, x.TenLoai
                                 FROM HocVien hv
                                 LEFT JOIN LopHoc l ON hv.MaLop = l.MaLop
                                 LEFT JOIN NgheNghiep n ON hv.MaNghe = n.MaNghe
                                 LEFT JOIN XepLoai x ON hv.MaXepLoai = x.MaXepLoai
                                 WHERE (@TuKhoa = '' OR hv.MaHocVien COLLATE Vietnamese_CI_AI LIKE '%' + @TuKhoa + '%'
                                    OR hv.TenHocVien COLLATE Vietnamese_CI_AI LIKE '%' + @TuKhoa + '%'
                                    OR hv.DienThoai LIKE '%' + @TuKhoa + '%')
                                   AND (@MaLop IS NULL OR hv.MaLop = @MaLop)
                                   AND (@MaNghe IS NULL OR hv.MaNghe = @MaNghe)
                                   AND (@MaXepLoai IS NULL OR hv.MaXepLoai = @MaXepLoai)
                                 ORDER BY hv.TenHocVien";
            return DbHelper.ExecuteQuery(sql,
                new SqlParameter("@TuKhoa", tuKhoa.Trim()),
                new SqlParameter("@MaLop", (object?)maLop ?? DBNull.Value),
                new SqlParameter("@MaNghe", (object?)maNghe ?? DBNull.Value),
                new SqlParameter("@MaXepLoai", (object?)maXepLoai ?? DBNull.Value));
        }

        public DataTable LayDanhSach(string tuKhoa = "")
        {
            const string sql = @"SELECT hv.MaHocVien, hv.TenHocVien, hv.NgaySinh, hv.GioiTinh,
                                        hv.DienThoai, hv.DiaChi, hv.MaLop, l.TenLop,
                                        hv.MaNghe, n.TenNghe, hv.NgayNopHocPhi, hv.Diem,
                                        hv.MaXepLoai, x.TenLoai
                                 FROM HocVien hv
                                 LEFT JOIN LopHoc l ON hv.MaLop = l.MaLop
                                 LEFT JOIN NgheNghiep n ON hv.MaNghe = n.MaNghe
                                 LEFT JOIN XepLoai x ON hv.MaXepLoai = x.MaXepLoai
                                 WHERE @TuKhoa = ''
                                    OR hv.MaHocVien COLLATE Vietnamese_CI_AI LIKE '%' + @TuKhoa + '%'
                                    OR hv.TenHocVien COLLATE Vietnamese_CI_AI LIKE '%' + @TuKhoa + '%'
                                    OR hv.DienThoai LIKE '%' + @TuKhoa + '%'
                                 ORDER BY hv.TenHocVien";
            return DbHelper.ExecuteQuery(sql, new SqlParameter("@TuKhoa", tuKhoa));
        }

        public DataTable LayDanhSachLop() => DbHelper.ExecuteQuery("SELECT MaLop, TenLop FROM LopHoc ORDER BY TenLop");
        public DataTable LayDanhSachNghe() => DbHelper.ExecuteQuery("SELECT MaNghe, TenNghe FROM NgheNghiep ORDER BY TenNghe");
        public DataTable LayDanhSachXepLoai() => DbHelper.ExecuteQuery("SELECT MaXepLoai, TenLoai FROM XepLoai ORDER BY MaXepLoai");

        public bool TonTai(string maHocVien) => DbHelper.ExecuteQuery(
            "SELECT 1 FROM HocVien WHERE MaHocVien=@MaHocVien", new SqlParameter("@MaHocVien", maHocVien)).Rows.Count > 0;

        public void Them(string ma, string ten, DateTime? ngaySinh, string? gioiTinh, string diaChi,
            string? maNghe, string? dienThoai, DateTime? ngayNopHocPhi, double? diem, string? maXepLoai, string? maLop)
        {
            const string sql = @"INSERT INTO HocVien (MaHocVien, TenHocVien, MaLop, NgaySinh, GioiTinh, DiaChi,
                                 MaNghe, DienThoai, NgayNopHocPhi, Diem, MaXepLoai)
                                 VALUES (@Ma, @Ten, @MaLop, @NgaySinh, @GioiTinh, @DiaChi,
                                 @MaNghe, @DienThoai, @NgayNopHocPhi, @Diem, @MaXepLoai)";
            DbHelper.ExecuteNonQuery(sql, ThamSo(ma, ten, ngaySinh, gioiTinh, diaChi, maNghe, dienThoai, ngayNopHocPhi, diem, maXepLoai, maLop));
        }

        public void Sua(string ma, string ten, DateTime? ngaySinh, string? gioiTinh, string diaChi,
            string? maNghe, string? dienThoai, DateTime? ngayNopHocPhi, double? diem, string? maXepLoai, string? maLop)
        {
            const string sql = @"UPDATE HocVien SET TenHocVien=@Ten, MaLop=@MaLop, NgaySinh=@NgaySinh,
                                 GioiTinh=@GioiTinh, DiaChi=@DiaChi, MaNghe=@MaNghe, DienThoai=@DienThoai,
                                 NgayNopHocPhi=@NgayNopHocPhi, Diem=@Diem, MaXepLoai=@MaXepLoai
                                 WHERE MaHocVien=@Ma";
            DbHelper.ExecuteNonQuery(sql, ThamSo(ma, ten, ngaySinh, gioiTinh, diaChi, maNghe, dienThoai, ngayNopHocPhi, diem, maXepLoai, maLop));
        }

        public void Xoa(string ma) => DbHelper.ExecuteNonQuery(
            "DELETE FROM HocVien WHERE MaHocVien=@Ma", new SqlParameter("@Ma", ma));

        private static SqlParameter[] ThamSo(string ma, string ten, DateTime? ngaySinh, string? gioiTinh,
            string diaChi, string? maNghe, string? dienThoai, DateTime? ngayNopHocPhi,
            double? diem, string? maXepLoai, string? maLop) => new[]
        {
            P("@Ma", ma), P("@Ten", ten), P("@MaLop", maLop), P("@NgaySinh", ngaySinh),
            P("@GioiTinh", gioiTinh), P("@DiaChi", diaChi), P("@MaNghe", maNghe),
            P("@DienThoai", dienThoai), P("@NgayNopHocPhi", ngayNopHocPhi), P("@Diem", diem),
            P("@MaXepLoai", maXepLoai)
        };

        private static SqlParameter P(string name, object? value) =>
            new(name, value ?? DBNull.Value);
    }
}
