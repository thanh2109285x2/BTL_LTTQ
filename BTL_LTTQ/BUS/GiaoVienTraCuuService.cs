using BTL_LTTQ.Models;
using BTL_LTTQ.DAL;
using System.Data;

namespace BTL_LTTQ.BUS
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
