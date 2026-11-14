using BTL_LTTQ.Models;
using BTL_LTTQ.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace BTL_LTTQ.Services
{
    public class PhongHocService
    {
        private readonly PhongHocRepository _repo = new PhongHocRepository();

        public DataTable LayDanhSach(string tuKhoa = "")
        {
            return _repo.LayDanhSach((tuKhoa ?? "").Trim());
        }

        public string Them(PhongHoc p)
        {
            string loi = KiemTra(p);
            if (loi != "") return loi;

            if (_repo.TonTai(p.MaPhong)) return "Mã phòng này đã tồn tại";

            try
            {
                _repo.Them(p);
                return "";
            }
            catch (Exception ex) when (ex is SqlException || ex is UnauthorizedAccessException)
            {
                return ex is SqlException ? "Không thêm được: " + ex.Message : ex.Message;
            }
        }

        public string Sua(PhongHoc p)
        {
            string loi = KiemTra(p);
            if (loi != "") return loi;

            try
            {
                _repo.Sua(p);
                return "";
            }
            catch (Exception ex) when (ex is SqlException || ex is UnauthorizedAccessException)
            {
                return ex is SqlException ? "Không sửa được: " + ex.Message : ex.Message;
            }
        }

        public string Xoa(string maPhong)
        {
            if (string.IsNullOrWhiteSpace(maPhong)) return "Vui lòng chọn một phòng để xóa.";

            try
            {
                _repo.Xoa(maPhong);
                return "";
            }
            catch (UnauthorizedAccessException ex)
            {
                return ex.Message;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                    return "Phòng này đang được sử dụng (lớp học / buổi học), không thể xóa";
                return "Không xóa được: " + ex.Message;
            }
        }

        private static string KiemTra(PhongHoc p)
        {
            var loi = new List<string>();

            if (p.MaPhong == "") loi.Add("Vui lòng nhập mã phòng");
            else if (p.MaPhong.Length > 20) loi.Add("Mã phòng không được vượt quá 20 ký tự");

            if (p.TenPhong == "") loi.Add("Vui lòng nhập tên phòng");
            else if (p.TenPhong.Length > 100) loi.Add("Tên phòng không được vượt quá 100 ký tự");

            if (p.SucChua.HasValue && p.SucChua.Value < 0) loi.Add("Sức chứa không được âm");

            return string.Join("\n", loi);
        }
    }
}
