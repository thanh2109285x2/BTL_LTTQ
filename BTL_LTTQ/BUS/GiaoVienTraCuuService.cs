using BTL_LTTQ.Models;
using BTL_LTTQ.Repositories;
using System.Data;

namespace BTL_LTTQ.Services
{
    public class GiaoVienTraCuuService
    {
        private readonly GiaoVienTraCuuRepository _repo = new GiaoVienTraCuuRepository();

        public DataTable TimKiem(TimKiemGiaoVien tieuChi)
        {
            tieuChi.TenGV = (tieuChi.TenGV ?? "").Trim();
            return _repo.TimKiem(tieuChi);
        }
    }
}
