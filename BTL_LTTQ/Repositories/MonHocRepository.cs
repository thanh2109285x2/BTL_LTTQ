using BTL_LTTQ.Common;
using BTL_LTTQ.DataAccessLayer;
using BTL_LTTQ.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace BTL_LTTQ.Repositories
{
    public class MonHocRepository
    {
        public DataTable LayDanhSach(string tuKhoa = "")
        {
            //Không có quyền xem thì trả bảng rỗng thay vì lộ dữ liệu
            if (!PermissionService.Can(ChucNangConst.MONHOC_XEM))
                return new DataTable();

            string sql = @"SELECT MaMon, TenMon, HocPhi
                           FROM MonHoc
                           WHERE @TuKhoa = ''
                              OR MaMon COLLATE Vietnamese_CI_AI LIKE '%' + @TuKhoa + '%'
                              OR TenMon COLLATE Vietnamese_CI_AI LIKE '%' + @TuKhoa + '%'
                           ORDER BY MaMon";

            return DbHelper.ExecuteQuery(sql, new SqlParameter("@TuKhoa", tuKhoa));
        }

        public bool TonTai(string maMon)
        {
            DataTable dt = DbHelper.ExecuteQuery(
                "SELECT 1 FROM MonHoc WHERE MaMon = @MaMon",
                new SqlParameter("@MaMon", maMon));
            return dt.Rows.Count > 0;
        }

        public void Them(MonHoc m)
        {
            PermissionService.YeuCau(ChucNangConst.MONHOC_THEM, "Bạn không có quyền thêm môn học.");

            string sql = @"INSERT INTO MonHoc (MaMon, TenMon, HocPhi)
                           VALUES (@MaMon, @TenMon, @HocPhi)";
            DbHelper.ExecuteNonQuery(sql, ThamSo(m));
        }

        public void Sua(MonHoc m)
        {
            PermissionService.YeuCau(ChucNangConst.MONHOC_SUA, "Bạn không có quyền sửa môn học.");

            string sql = @"UPDATE MonHoc
                           SET TenMon = @TenMon, HocPhi = @HocPhi
                           WHERE MaMon = @MaMon";
            DbHelper.ExecuteNonQuery(sql, ThamSo(m));
        }

        public void Xoa(string maMon)
        {
            PermissionService.YeuCau(ChucNangConst.MONHOC_XOA, "Bạn không có quyền xóa môn học.");

            DbHelper.ExecuteNonQuery("DELETE FROM MonHoc WHERE MaMon = @MaMon",
                                     new SqlParameter("@MaMon", maMon));
        }

        private static SqlParameter[] ThamSo(MonHoc m)
        {
            return new[]
            {
                new SqlParameter("@MaMon", m.MaMon),
                new SqlParameter("@TenMon", m.TenMon),
                new SqlParameter("@HocPhi", (object?)m.HocPhi ?? DBNull.Value)
            };
        }
    }
}
