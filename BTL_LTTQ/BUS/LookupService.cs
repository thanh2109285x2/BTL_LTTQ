using BTL_LTTQ.DAL;
using System.Data;

namespace BTL_LTTQ.BUS
{
    public class LookupService
    {
        private readonly LookupRepository _repo = new LookupRepository();

        public DataTable LayMonHoc() => _repo.LayMonHoc();
        public DataTable LayGiaoVien() => _repo.LayGiaoVien();
        public DataTable LayTrinhDo() => _repo.LayTrinhDo();
        public DataTable LayChuyenMon() => _repo.LayChuyenMon();
        public DataTable LayCaHoc() => _repo.LayCaHoc();
        public DataTable LayThoiGian() => _repo.LayThoiGian();
        public DataTable LayPhongHoc() => _repo.LayPhongHoc();
        public DataTable LayGiaoVienTheoMon(string? maMon) => _repo.LayGiaoVienTheoMon(maMon);
    }
}
