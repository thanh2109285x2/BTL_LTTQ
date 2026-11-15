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

        public bool TonTai(string maHocVien) => _repo.TonTai(maHocVien);

        public void Them(string ma, string ten, DateTime? ngaySinh, string? gioiTinh, string diaChi,
            string? maNghe, string? dienThoai, DateTime? ngayNopHocPhi, double? diem, string? maXepLoai, string? maLop) =>
            _repo.Them(ma, ten, ngaySinh, gioiTinh, diaChi, maNghe, dienThoai, ngayNopHocPhi, diem, maXepLoai, maLop);

        public void Sua(string ma, string ten, DateTime? ngaySinh, string? gioiTinh, string diaChi,
            string? maNghe, string? dienThoai, DateTime? ngayNopHocPhi, double? diem, string? maXepLoai, string? maLop) =>
            _repo.Sua(ma, ten, ngaySinh, gioiTinh, diaChi, maNghe, dienThoai, ngayNopHocPhi, diem, maXepLoai, maLop);

        public void Xoa(string ma) => _repo.Xoa(ma);
    }
}
