using BTL_LTTQ.Models;
using BTL_LTTQ.Repositories;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace BTL_LTTQ.Services
{
    public class MonHocService
    {
        private readonly MonHocRepository _repo = new MonHocRepository();

        public DataTable LayDanhSach(string tuKhoa = "")
        {
            return _repo.LayDanhSach((tuKhoa ?? "").Trim());
        }

        public string Them(MonHoc m)
        {
            ChuanHoa(m);

            string loi = KiemTra(m);
            if (loi != "") return loi;

            if (_repo.TonTai(m.MaMon)) return "Mã môn này đã tồn tại";

            try
            {
                _repo.Them(m);
                return "";
            }
            catch (Exception ex) when (ex is SqlException || ex is UnauthorizedAccessException)
            {
                return ex is SqlException ? "Không thêm được: " + ex.Message : ex.Message;
            }
        }

        public string Sua(MonHoc m)
        {
            ChuanHoa(m);

            string loi = KiemTra(m);
            if (loi != "") return loi;

            try
            {
                _repo.Sua(m);
                return "";
            }
            catch (Exception ex) when (ex is SqlException || ex is UnauthorizedAccessException)
            {
                return ex is SqlException ? "Không sửa được: " + ex.Message : ex.Message;
            }
        }

        public string Xoa(string maMon)
        {
            if (string.IsNullOrWhiteSpace(maMon)) return "Vui lòng chọn một môn học để xóa.";

            try
            {
                _repo.Xoa(maMon);
                return "";
            }
            catch (UnauthorizedAccessException ex)
            {
                return ex.Message;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                    return "Môn học này đang được sử dụng (lớp học hoặc giáo viên), không thể xóa";
                return "Không xóa được: " + ex.Message;
            }
        }

        private static void ChuanHoa(MonHoc m)
        {
            m.MaMon = (m.MaMon ?? "").Trim();
            m.TenMon = (m.TenMon ?? "").Trim();
        }

        private static string KiemTra(MonHoc m)
        {
            var loi = new List<string>();

            if (m.MaMon == "") loi.Add("Vui lòng nhập mã môn");
            else if (m.MaMon.Length > 20) loi.Add("Mã môn không được vượt quá 20 ký tự");

            if (m.TenMon == "") loi.Add("Vui lòng nhập tên môn");
            else if (m.TenMon.Length > 100) loi.Add("Tên môn không được vượt quá 100 ký tự");

            if (m.HocPhi.HasValue && m.HocPhi.Value < 0) loi.Add("Học phí không được âm");

            return string.Join("\n", loi);
        }
    }
}
