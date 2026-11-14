using BTL_LTTQ.Models;
using BTL_LTTQ.Repositories;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace BTL_LTTQ.Services
{
    public class LopHocService
    {
        private readonly LopHocRepository _repo = new LopHocRepository();

        public DataTable LayDanhSach(string tuKhoa = "")
        {
            return _repo.LayDanhSach((tuKhoa ?? "").Trim());
        }

        public string Them(LopHoc l)
        {
            ChuanHoa(l);

            string loi = KiemTra(l);
            if (loi != "") return loi;

            if (_repo.TonTai(l.MaLop)) return "Mã lớp này đã tồn tại";

            try
            {
                _repo.Them(l);
                return "";
            }
            catch (SqlException ex)
            {
                return "Không thêm được: " + ex.Message;
            }
        }

        public string Sua(LopHoc l)
        {
            ChuanHoa(l);

            string loi = KiemTra(l);
            if (loi != "") return loi;

            try
            {
                _repo.Sua(l);
                return "";
            }
            catch (Exception ex) when (ex is SqlException || ex is UnauthorizedAccessException || ex is InvalidOperationException)
            {
                return ex is SqlException ? "Không sửa được: " + ex.Message : ex.Message;
            }
        }

        public string Xoa(string maLop)
        {
            if (string.IsNullOrWhiteSpace(maLop)) return "Vui lòng chọn một lớp để xóa.";

            try
            {
                _repo.Xoa(maLop);
                return "";
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                    return "Lớp này đang có học viên (hoặc dữ liệu liên quan), không thể xóa";
                return "Không xóa được: " + ex.Message;
            }
        }

        private static void ChuanHoa(LopHoc l)
        {
            l.MaLop = (l.MaLop ?? "").Trim();
            l.TenLop = (l.TenLop ?? "").Trim();
        }

        private static string KiemTra(LopHoc l)
        {
            var loi = new List<string>();

            if (l.MaLop == "") loi.Add("Vui lòng nhập mã lớp");
            if (l.TenLop == "") loi.Add("Vui lòng nhập tên lớp");

            if (l.MaMon == null || l.MaGV == null || l.MaCa == null ||
                l.MaThoiGian == null || l.MaPhong == null)
                loi.Add("Vui lòng chọn đầy đủ môn học, giáo viên, ca học, thời gian và phòng học");

            if (l.NgayKT.Date < l.NgayBD.Date)
                loi.Add("Ngày kết thúc không được trước ngày bắt đầu");

            return string.Join("\n", loi);
        }
    }
}