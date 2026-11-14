using BTL_LTTQ.DataAccessLayer;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BTL_LTTQ.Forms.BaoCao
{
    internal sealed class BaoCaoRepository
    {
        public DataTable LayDanhSachLop() => DbHelper.ExecuteQuery(
            "SELECT MaLop, TenLop FROM LopHoc ORDER BY TenLop");

        public DataTable LayBangDiem(string? maLop) => DbHelper.ExecuteQuery(@"
            SELECT l.MaLop, l.TenLop, hv.MaHocVien, hv.TenHocVien,
                   hv.Diem, xl.TenLoai AS XepLoai
            FROM LopHoc l
            LEFT JOIN HocVien hv ON hv.MaLop = l.MaLop
            LEFT JOIN XepLoai xl ON xl.MaXepLoai = hv.MaXepLoai
            WHERE (@MaLop IS NULL OR l.MaLop = @MaLop)
            ORDER BY l.TenLop, hv.TenHocVien",
            new SqlParameter("@MaLop", (object?)maLop ?? DBNull.Value));

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
    }
}
