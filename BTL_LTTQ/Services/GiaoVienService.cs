using BTL_LTTQ.Models;
using BTL_LTTQ.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BTL_LTTQ.Services
{
    public class GiaoVienService
    {
        private readonly GiaoVienRepository _repo = new();

        public DataTable LayDanhSach(string tuKhoa = "") => _repo.LayDanhSach((tuKhoa ?? "").Trim());
        public DataTable LayDanhMuc(string bang, string cotMa, string cotTen) => _repo.LayDanhMuc(bang, cotMa, cotTen);
        public List<string> LayMonDay(string maGV) => _repo.LayMonDay(maGV);

        public string Them(GiaoVien g)
        {
            ChuanHoa(g);
            string loi = KiemTra(g);
            if (loi != "") return loi;
            if (_repo.TonTai(g.MaGV)) return "Mã giáo viên đã tồn tại.";
            try { _repo.Them(g); return ""; }
            catch (SqlException ex) { return LoiSql(ex, "thêm"); }
        }

        public string Sua(GiaoVien g)
        {
            ChuanHoa(g);
            string loi = KiemTra(g);
            if (loi != "") return loi;
            try { _repo.Sua(g); return ""; }
            catch (SqlException ex) { return LoiSql(ex, "sửa"); }
            catch (InvalidOperationException ex) { return ex.Message; }
        }

        public string Xoa(string maGV)
        {
            if (string.IsNullOrWhiteSpace(maGV)) return "Vui lòng chọn giáo viên cần xóa.";
            try { _repo.Xoa(maGV); return ""; }
            catch (SqlException ex) when (ex.Number == 547)
            { return "Giáo viên đang được lớp học hoặc tài khoản sử dụng, không thể xóa."; }
            catch (SqlException ex) { return "Không xóa được giáo viên: " + ex.Message; }
            catch (InvalidOperationException ex) { return ex.Message; }
        }

        private static void ChuanHoa(GiaoVien g)
        {
            g.MaGV = (g.MaGV ?? "").Trim();
            g.TenGV = (g.TenGV ?? "").Trim();
            g.DiaChi = BlankToNull(g.DiaChi);
            g.DienThoai = BlankToNull(g.DienThoai);
            g.GioiTinh = BlankToNull(g.GioiTinh);
            g.Anh = BlankToNull(g.Anh);
            g.MaMonDay = g.MaMonDay.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        }

        private static string KiemTra(GiaoVien g)
        {
            if (g.MaGV == "") return "Vui lòng nhập mã giáo viên.";
            if (g.MaGV.Length > 20) return "Mã giáo viên tối đa 20 ký tự.";
            if (g.TenGV == "") return "Vui lòng nhập họ tên giáo viên.";
            if (g.TenGV.Length > 100) return "Họ tên tối đa 100 ký tự.";
            if (!g.NgaySinh.HasValue) return "Vui lòng chọn ngày sinh.";
            if (g.NgaySinh.Value.Date > DateTime.Today) return "Ngày sinh không được ở tương lai.";
            if (g.NgaySinh.Value.Date > DateTime.Today.AddYears(-18)) return "Giáo viên phải đủ 18 tuổi.";
            if (g.GioiTinh is not ("Nam" or "Nữ")) return "Vui lòng chọn giới tính.";
            if (string.IsNullOrWhiteSpace(g.MaChucDanh)) return "Vui lòng chọn chức danh.";
            if (string.IsNullOrWhiteSpace(g.MaCQ)) return "Vui lòng chọn cơ quan.";
            if (string.IsNullOrWhiteSpace(g.MaChuyenMon)) return "Vui lòng chọn chuyên môn.";
            if (g.DienThoai?.Length > 20) return "Số điện thoại tối đa 20 ký tự.";
            if (!string.IsNullOrWhiteSpace(g.DienThoai) &&
                (g.DienThoai.Any(c => !char.IsDigit(c)) || g.DienThoai.Length is < 9 or > 11))
                return "Số điện thoại phải gồm 9 đến 11 chữ số.";
            if (g.DiaChi?.Length > 200) return "Địa chỉ tối đa 200 ký tự.";
            if (g.Anh?.Length > 255) return "Đường dẫn ảnh tối đa 255 ký tự.";
            if (g.MaMonDay.Count == 0) return "Vui lòng chọn ít nhất một môn giảng dạy.";
            return "";
        }

        private static string? BlankToNull(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        private static string LoiSql(SqlException ex, string thaoTac) => ex.Number == 547
            ? "Dữ liệu danh mục hoặc môn học không còn hợp lệ. Vui lòng tải lại danh sách."
            : $"Không {thaoTac} được giáo viên: {ex.Message}";
    }
}
