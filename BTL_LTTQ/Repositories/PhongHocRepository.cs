using BTL_LTTQ.Common;
using BTL_LTTQ.DataAccessLayer;
using BTL_LTTQ.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace BTL_LTTQ.Repositories
{
    public class PhongHocRepository
    {
        public DataTable LayDanhSach(string tuKhoa = "")
        {
            //Không có quyền xem thì trả bảng rỗng thay vì lộ dữ liệu
            if (!PermissionService.Can(ChucNangConst.DANHMUC_KHAC))
                return new DataTable();

            const string sql = @"
                SELECT MaPhong, TenPhong, SucChua
                FROM PhongHoc
                WHERE @TuKhoa = ''
                   OR MaPhong COLLATE Vietnamese_CI_AI LIKE '%' + @TuKhoa + '%'
                   OR TenPhong COLLATE Vietnamese_CI_AI LIKE '%' + @TuKhoa + '%'
                ORDER BY MaPhong";

            return DbHelper.ExecuteQuery(sql, new SqlParameter("@TuKhoa", tuKhoa ?? ""));
        }

        public bool TonTai(string maPhong)
        {
            DataTable dt = DbHelper.ExecuteQuery(
                "SELECT 1 FROM PhongHoc WHERE MaPhong = @MaPhong",
                new SqlParameter("@MaPhong", maPhong));
            return dt.Rows.Count > 0;
        }

        public void Them(PhongHoc p)
        {
            PermissionService.YeuCau(ChucNangConst.DANHMUC_KHAC, "Bạn không có quyền thêm phòng học.");

            const string sql = @"
                INSERT INTO PhongHoc (MaPhong, TenPhong, SucChua)
                VALUES (@MaPhong, @TenPhong, @SucChua)";

            DbHelper.ExecuteNonQuery(sql, new[]
            {
                new SqlParameter("@MaPhong", SqlDbType.VarChar, 20) { Value = p.MaPhong },
                new SqlParameter("@TenPhong", SqlDbType.NVarChar, 100) { Value = p.TenPhong },
                new SqlParameter("@SucChua", SqlDbType.Int) { Value = (object?)p.SucChua ?? DBNull.Value }
            });
        }

        public void Sua(PhongHoc p)
        {
            PermissionService.YeuCau(ChucNangConst.DANHMUC_KHAC, "Bạn không có quyền sửa phòng học.");

            const string sql = @"
                UPDATE PhongHoc
                SET TenPhong = @TenPhong, SucChua = @SucChua
                WHERE MaPhong = @MaPhong";

            DbHelper.ExecuteNonQuery(sql, new[]
            {
                new SqlParameter("@MaPhong", SqlDbType.VarChar, 20) { Value = p.MaPhong },
                new SqlParameter("@TenPhong", SqlDbType.NVarChar, 100) { Value = p.TenPhong },
                new SqlParameter("@SucChua", SqlDbType.Int) { Value = (object?)p.SucChua ?? DBNull.Value }
            });
        }

        public void Xoa(string maPhong)
        {
            PermissionService.YeuCau(ChucNangConst.DANHMUC_KHAC, "Bạn không có quyền xóa phòng học.");

            DbHelper.ExecuteNonQuery("DELETE FROM PhongHoc WHERE MaPhong = @MaPhong",
                                     new SqlParameter("@MaPhong", SqlDbType.VarChar, 20) { Value = maPhong });
        }
    }
}
