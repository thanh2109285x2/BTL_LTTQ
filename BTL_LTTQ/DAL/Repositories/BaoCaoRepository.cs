using BTL_LTTQ.DAL;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BTL_LTTQ.DAL
{
    internal sealed class BaoCaoRepository
    {
        public DataTable LayDanhSachLop(string? dieuKienPhamVi, SqlParameter[] thamSo)
        {
            string locPhamVi = string.IsNullOrWhiteSpace(dieuKienPhamVi) ? "" : " AND " + dieuKienPhamVi;
            return DbHelper.ExecuteQuery($@"
                SELECT DISTINCT l.MaLop, l.TenLop
                FROM LopHoc l
                LEFT JOIN DangKy d ON d.MaLop = l.MaLop
                WHERE 1 = 1 {locPhamVi}
                ORDER BY l.TenLop", thamSo);
        }

        public DataTable LayBangDiem(string? maLop, string? dieuKienPhamVi, SqlParameter[] thamSo)
        {
            string locPhamVi = string.IsNullOrWhiteSpace(dieuKienPhamVi) ? "" : " AND " + dieuKienPhamVi;
            var thamSoTruyVan = thamSo.ToList();
            thamSoTruyVan.Add(new SqlParameter("@MaLop", (object?)maLop ?? DBNull.Value));
            return DbHelper.ExecuteQuery($@"
                SELECT l.MaLop, l.TenLop, hv.MaHocVien, hv.TenHocVien,
                       d.Diem, xl.TenLoai AS XepLoai
                FROM LopHoc l
                INNER JOIN DangKy d ON d.MaLop = l.MaLop
                INNER JOIN HocVien hv ON hv.MaHocVien = d.MaHocVien
                LEFT JOIN XepLoai xl ON xl.MaXepLoai = d.MaXepLoai
                WHERE (@MaLop IS NULL OR l.MaLop = @MaLop) {locPhamVi}
                ORDER BY l.TenLop, hv.TenHocVien", thamSoTruyVan.ToArray());
        }

        public DataTable LayDoanhThu(string? maLop) => DbHelper.ExecuteQuery(@"
            SELECT l.MaLop, l.TenLop, m.TenMon,
                   COUNT(hv.MaHocVien) AS SoHocVienDaNop,
                   CAST(COALESCE(m.HocPhi, 0) AS decimal(18,2)) AS HocPhiMoiHocVien,
                   CAST(COUNT(hv.MaHocVien) * COALESCE(m.HocPhi, 0) AS decimal(18,2)) AS DoanhThu
            FROM LopHoc l
            LEFT JOIN MonHoc m ON m.MaMon = l.MaMon
            LEFT JOIN HocVien hv ON hv.MaLop = l.MaLop AND hv.NgayNopHocPhi IS NOT NULL
            WHERE (@MaLop IS NULL OR l.MaLop = @MaLop)
            GROUP BY l.MaLop, l.TenLop, m.TenMon, m.HocPhi
            ORDER BY l.TenLop",
            new SqlParameter("@MaLop", (object?)maLop ?? DBNull.Value));

        public DataTable LayTop3HocVienTheoLop(string? dieuKienPhamVi, SqlParameter[] thamSo)
        {
            string locPhamVi = string.IsNullOrWhiteSpace(dieuKienPhamVi) ? "" : " AND " + dieuKienPhamVi;
            var thamSoTruyVan = thamSo.ToList();
            thamSoTruyVan.Add(new SqlParameter("@Top", 3));
            return DbHelper.ExecuteQuery($@"
                SELECT MaLop, TenLop, MaHocVien, TenHocVien, Diem, Hang
                FROM (
                    SELECT l.MaLop, l.TenLop, hv.MaHocVien, hv.TenHocVien, hv.Diem,
                           ROW_NUMBER() OVER (PARTITION BY l.MaLop ORDER BY hv.Diem DESC, hv.MaHocVien) AS Hang
                    FROM LopHoc l
                    JOIN HocVien hv ON hv.MaLop = l.MaLop
                    WHERE hv.Diem IS NOT NULL {locPhamVi}
                ) x
                WHERE Hang <= @Top
                ORDER BY TenLop, Hang", thamSoTruyVan.ToArray());
        }

        public DataTable LayLopTheoPhong(string? maPhong, string? dieuKienPhamVi, SqlParameter[] thamSo)
        {
            string locPhamVi = string.IsNullOrWhiteSpace(dieuKienPhamVi) ? "" : " AND " + dieuKienPhamVi;
            var thamSoTruyVan = thamSo.ToList();
            thamSoTruyVan.Add(new SqlParameter("@MaPhong", SqlDbType.VarChar, 20) { Value = (object?)maPhong ?? DBNull.Value });
            return DbHelper.ExecuteQuery($@"
                SELECT p.MaPhong, p.TenPhong, l.MaLop, l.TenLop,
                       m.TenMon, g.TenGV, c.TenCa, tg.TenThoiGian
                FROM PhongHoc p
                LEFT JOIN LopHoc l ON l.MaPhong = p.MaPhong
                LEFT JOIN MonHoc m ON m.MaMon = l.MaMon
                LEFT JOIN GiaoVien g ON g.MaGV = l.MaGV
                LEFT JOIN CaHoc c ON c.MaCa = l.MaCa
                LEFT JOIN ThoiGian tg ON tg.MaThoiGian = l.MaThoiGian
                WHERE (@MaPhong IS NULL OR p.MaPhong = @MaPhong) {locPhamVi}
                ORDER BY p.TenPhong, l.TenLop", thamSoTruyVan.ToArray());
        }
    }
}
