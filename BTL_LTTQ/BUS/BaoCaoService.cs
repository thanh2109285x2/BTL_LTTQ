using BTL_LTTQ.DAL;
using System.Data;

namespace BTL_LTTQ.BUS
{
    public class BaoCaoService
    {
        private readonly BaoCaoRepository _repo = new();

        public DataTable LayDanhSachLop() => _repo.LayDanhSachLop();

        public DataTable LayBangDiem(string? maLop) => _repo.LayBangDiem(maLop);

        public DataTable LayDoanhThu(string? maLop) => _repo.LayDoanhThu(maLop);
    }
}
