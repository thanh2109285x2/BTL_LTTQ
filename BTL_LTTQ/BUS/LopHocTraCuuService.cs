using BTL_LTTQ.Models;
using BTL_LTTQ.Repositories;
using System.Data;

namespace BTL_LTTQ.Services
{
    public class LopHocTraCuuService
    {
        private readonly LopHocTraCuuRepository _repo = new LopHocTraCuuRepository();

        public string TimKiem(TimKiemLopHoc tieuChi, out DataTable ketQua)
        {
            ketQua = new DataTable();

            if (tieuChi.SiSoTu > tieuChi.SiSoDen)
                return "Sĩ số 'từ' không được lớn hơn sĩ số 'đến'.";

            ketQua = _repo.TimKiem(tieuChi);
            return "";
        }
    }
}
