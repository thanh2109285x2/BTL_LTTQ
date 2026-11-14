using System;
using System.Collections.Generic;
using System.Text;
using BTL_LTTQ.DataAccessLayer;
using System.Data;

namespace BTL_LTTQ.Repositories
{
    public class LookupRepository
    {
        public DataTable LayMonHoc() => DbHelper.ExecuteQuery("SELECT MaMon, TenMon FROM MonHoc");
        public DataTable LayGiaoVien() => DbHelper.ExecuteQuery("SELECT MaGV, TenGV FROM GiaoVien");
        public DataTable LayGiaoVienTheoMon(string? maMon) => DbHelper.ExecuteQuery(
            @"SELECT g.MaGV, g.TenGV
              FROM dbo.GiaoVien g
              WHERE @MaMon IS NOT NULL AND EXISTS
                    (SELECT 1 FROM dbo.GiaoVien_MonHoc gm WHERE gm.MaGV = g.MaGV AND gm.MaMon = @MaMon)
              ORDER BY g.TenGV",
            new Microsoft.Data.SqlClient.SqlParameter("@MaMon", (object?)maMon ?? DBNull.Value));
        public DataTable LayTrinhDo() => DbHelper.ExecuteQuery("SELECT MaTrinhDo, TenTrinhDo FROM TrinhDo");
        public DataTable LayChuyenMon() => DbHelper.ExecuteQuery("SELECT MaChuyenMon, TenChuyenMon FROM ChuyenMon");
        public DataTable LayCaHoc() => DbHelper.ExecuteQuery("SELECT MaCa, TenCa FROM CaHoc");
        public DataTable LayThoiGian() => DbHelper.ExecuteQuery("SELECT MaThoiGian, TenThoiGian FROM ThoiGian");
        public DataTable LayPhongHoc() => DbHelper.ExecuteQuery("SELECT MaPhong, TenPhong FROM PhongHoc");
    }
}
