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
        // Có thể mở rộng: CRUD học viên qua DAL khi UI được bổ sung.
        public DataTable LayDanhSachTheoPhamVi(string maChucNang = ChucNangConst.HOCVIEN_XEM)
        {
            if (!PermissionService.Can(maChucNang))
                return new DataTable();
            // Chưa có UI/repo học viên đầy đủ trong nhánh hiện tại.
            return new DataTable();
        }
    }
}
