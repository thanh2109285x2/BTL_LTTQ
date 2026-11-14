using BTL_LTTQ.Repositories;
using System.Data;

namespace BTL_LTTQ.Services
{
    public class LookupService
    {
        private readonly LookupRepository _repo = new LookupRepository();

        public DataTable LayMonHoc() => _repo.LayMonHoc();
        public DataTable LayGiaoVien() => _repo.LayGiaoVien();
        public DataTable LayTrinhDo() => _repo.LayTrinhDo();
        public DataTable LayChuyenMon() => _repo.LayChuyenMon();
    }
}
