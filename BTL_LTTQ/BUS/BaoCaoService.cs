using BTL_LTTQ.Common;
using System.Data;

namespace BTL_LTTQ.BUS
{
    /// <summary>
    /// BUS cho báo cáo (mở rộng theo yêu cầu 3 tầng).
    /// </summary>
    public class BaoCaoService
    {
        // Có thể mở rộng: gọi DAL/Repositories/BaoCaoRepository khi UI báo cáo được bổ sung.
        public DataTable LayDuLieuBaoCao(string maChucNang)
        {
            if (!PermissionService.Can(maChucNang))
                return new DataTable();
            return new DataTable();
        }
    }
}
