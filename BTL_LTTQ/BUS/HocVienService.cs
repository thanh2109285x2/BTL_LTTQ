using BTL_LTTQ.Common;
using BTL_LTTQ.DAL;
using BTL_LTTQ.Models;
using System.Data;

namespace BTL_LTTQ.BUS
{
    /// <summary>
    /// BUS cho nghiệp vụ học viên (mở rộng theo yêu cầu 3 tầng).
    /// </summary>
    public class HocVienService
    {
        private readonly HocVienRepository _repo = new();

        public DataTable TimKiem(string tuKhoa = "", string? maLop = null,
            string? maNghe = null, string? maXepLoai = null) =>
            _repo.TimKiem(tuKhoa, maLop, maNghe, maXepLoai);

        public DataTable LayDanhSach(string tuKhoa = "") => _repo.LayDanhSach(tuKhoa);

        public DataTable LayDanhSachLop() => _repo.LayDanhSachLop();

        public DataTable LayDanhSachNghe() => _repo.LayDanhSachNghe();

        public DataTable LayDanhSachXepLoai() => _repo.LayDanhSachXepLoai();

        public DataTable LayDanhSachDiaChi()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaDiaChi", typeof(string));
            dt.Columns.Add("TenDiaChi", typeof(string));

            string[] danhSachTinh = {
                "Hà Nội", "Hải Phòng", "Đà Nẵng", "Huế", "Thành phố Hồ Chí Minh", "Cần Thơ",
                "An Giang", "Bắc Ninh", "Cà Mau", "Cao Bằng", "Đắk Lắk",
                "Điện Biên", "Đồng Nai", "Đồng Tháp", "Gia Lai", "Hà Tĩnh",
                "Hưng Yên", "Khánh Hòa", "Lai Châu", "Lạng Sơn", "Lào Cai",
                "Nghệ An", "Ninh Bình", "Phú Thọ", "Quảng Ngãi", "Quảng Ninh",
                "Quảng Trị", "Sơn La", "Thái Nguyên", "Thanh Hóa", "Tuyên Quang",
                "Tây Ninh", "Vĩnh Long", "Lâm Đồng"
            };

            foreach (var tinh in danhSachTinh)
            {
                dt.Rows.Add(tinh, tinh);
            }

            return dt;
        }

        public bool TonTai(string maHocVien) => _repo.TonTai(maHocVien);

        public void Them(string ma, string ten, DateTime? ngaySinh, string? gioiTinh, string diaChi,
            string? maNghe, string? dienThoai) =>
            _repo.Them(ma, ten, ngaySinh, gioiTinh, diaChi, maNghe, dienThoai);

        public void Sua(string ma, string ten, DateTime? ngaySinh, string? gioiTinh, string diaChi,
            string? maNghe, string? dienThoai) =>
            _repo.Sua(ma, ten, ngaySinh, gioiTinh, diaChi, maNghe, dienThoai);

        public void Xoa(string ma) => _repo.Xoa(ma);
    }
}
