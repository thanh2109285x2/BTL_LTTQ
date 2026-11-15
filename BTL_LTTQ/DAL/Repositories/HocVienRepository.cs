using BTL_LTTQ.Common;
using BTL_LTTQ.DAL;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace BTL_LTTQ.DAL
{
    public class HocVienRepository
    {
        public DataTable TimKiem(string tuKhoa = "", string? maLop = null,
            string? maNghe = null, string? maXepLoai = null)
        {
            _ = maXepLoai;
            var phamVi = PhamViDuLieu.ChoHocVien(ChucNangConst.TIM_HOCVIEN, "hv");
            string dieuKienPhamVi = phamVi.DieuKienSql.Length == 0 ? "" : " AND " + phamVi.DieuKienSql;
            string sql = $@"SELECT hv.MaHocVien, hv.TenHocVien, hv.NgaySinh, hv.GioiTinh,
                                   hv.DienThoai, hv.DiaChi, n.TenNghe
                            FROM HocVien hv
                            LEFT JOIN NgheNghiep n ON hv.MaNghe = n.MaNghe
                            WHERE (@TuKhoa = '' OR hv.MaHocVien COLLATE Vietnamese_CI_AI LIKE '%' + @TuKhoa + '%'
                               OR hv.TenHocVien COLLATE Vietnamese_CI_AI LIKE '%' + @TuKhoa + '%'
                               OR hv.DienThoai LIKE '%' + @TuKhoa + '%')
                              AND (@MaLop IS NULL OR EXISTS (
                                    SELECT 1 FROM DangKy dk
                                    WHERE dk.MaHocVien = hv.MaHocVien AND dk.MaLop = @MaLop))
                              AND (@MaNghe IS NULL OR hv.MaNghe = @MaNghe){dieuKienPhamVi}
                            ORDER BY hv.TenHocVien";
            var thamSo = new List<SqlParameter>
            {
                new SqlParameter("@TuKhoa", tuKhoa.Trim()),
                new SqlParameter("@MaLop", (object?)maLop ?? DBNull.Value),
                new SqlParameter("@MaNghe", (object?)maNghe ?? DBNull.Value)
            };
            thamSo.AddRange(phamVi.ThamSo);
            return DbHelper.ExecuteQuery(sql, thamSo.ToArray());
        }

        public DataTable LayDanhSach(string tuKhoa = "")
        {
            var phamVi = PhamViDuLieu.ChoHocVien(ChucNangConst.HOCVIEN_XEM, "hv");
            string dieuKienPhamVi = phamVi.DieuKienSql.Length == 0 ? "" : " AND " + phamVi.DieuKienSql;
            string sql = $@"SELECT hv.MaHocVien, hv.TenHocVien, hv.NgaySinh, hv.GioiTinh,
                                   hv.DienThoai, hv.DiaChi, hv.MaNghe, n.TenNghe
                            FROM HocVien hv
                            LEFT JOIN NgheNghiep n ON hv.MaNghe = n.MaNghe
                            WHERE (@TuKhoa = ''
                               OR hv.MaHocVien COLLATE Vietnamese_CI_AI LIKE '%' + @TuKhoa + '%'
                               OR hv.TenHocVien COLLATE Vietnamese_CI_AI LIKE '%' + @TuKhoa + '%'
                               OR hv.DienThoai LIKE '%' + @TuKhoa + '%'){dieuKienPhamVi}
                            ORDER BY hv.TenHocVien";
            var thamSo = new List<SqlParameter> { new SqlParameter("@TuKhoa", tuKhoa) };
            thamSo.AddRange(phamVi.ThamSo);
            return DbHelper.ExecuteQuery(sql, thamSo.ToArray());
        }

        public DataTable LayDanhSachLop() => DbHelper.ExecuteQuery("SELECT MaLop, TenLop FROM LopHoc ORDER BY TenLop");
        public DataTable LayDanhSachNghe() => DbHelper.ExecuteQuery("SELECT MaNghe, TenNghe FROM NgheNghiep ORDER BY TenNghe");
        public DataTable LayDanhSachXepLoai() => DbHelper.ExecuteQuery("SELECT MaXepLoai, TenLoai FROM XepLoai ORDER BY MaXepLoai");

        public bool TonTai(string maHocVien) => DbHelper.ExecuteQuery(
            "SELECT 1 FROM HocVien WHERE MaHocVien=@MaHocVien", new SqlParameter("@MaHocVien", maHocVien)).Rows.Count > 0;

        public void Them(string ma, string ten, DateTime? ngaySinh, string? gioiTinh, string diaChi,
            string? maNghe, string? dienThoai)
        {
            const string sql = @"INSERT INTO HocVien (MaHocVien, TenHocVien, NgaySinh, GioiTinh, DiaChi, MaNghe, DienThoai)
                                 VALUES (@Ma, @Ten, @NgaySinh, @GioiTinh, @DiaChi, @MaNghe, @DienThoai)";
            DbHelper.ExecuteNonQuery(sql, ThamSo(ma, ten, ngaySinh, gioiTinh, diaChi, maNghe, dienThoai));
        }

        public void Sua(string ma, string ten, DateTime? ngaySinh, string? gioiTinh, string diaChi,
            string? maNghe, string? dienThoai)
        {
            const string sql = @"UPDATE HocVien SET TenHocVien=@Ten, NgaySinh=@NgaySinh,
                                 GioiTinh=@GioiTinh, DiaChi=@DiaChi, MaNghe=@MaNghe, DienThoai=@DienThoai
                                 WHERE MaHocVien=@Ma";
            DbHelper.ExecuteNonQuery(sql, ThamSo(ma, ten, ngaySinh, gioiTinh, diaChi, maNghe, dienThoai));
        }

        public void Xoa(string ma) => DbHelper.ExecuteNonQuery(
            "DELETE FROM HocVien WHERE MaHocVien=@Ma", new SqlParameter("@Ma", ma));

        private static SqlParameter[] ThamSo(string ma, string ten, DateTime? ngaySinh, string? gioiTinh,
            string diaChi, string? maNghe, string? dienThoai) => new[]
        {
            P("@Ma", ma), P("@Ten", ten), P("@NgaySinh", ngaySinh),
            P("@GioiTinh", gioiTinh), P("@DiaChi", diaChi), P("@MaNghe", maNghe),
            P("@DienThoai", dienThoai)
        };

        private static SqlParameter P(string name, object? value) =>
            new(name, value ?? DBNull.Value);
    }
}
