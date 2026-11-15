using BTL_LTTQ.Common;
using BTL_LTTQ.DAL;
using BTL_LTTQ.Models;
using System.Collections.Generic;

namespace BTL_LTTQ.BUS
{
    public class ThoiKhoaBieuService
    {
        private readonly ThoiKhoaBieuRepository _repo = new ThoiKhoaBieuRepository();

        public List<BuoiHoc> LayLich(string? maGV, string? maHocVien)
        {
            if (!PermissionService.Can(ChucNangConst.THOIKHOA_XEM))
                return new List<BuoiHoc>();
            return _repo.LayLich(maGV, maHocVien);
        }
    }
}
