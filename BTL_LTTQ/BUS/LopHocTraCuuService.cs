using BTL_LTTQ.Common;
using BTL_LTTQ.Models;
using BTL_LTTQ.DAL;
using System.Data;

namespace BTL_LTTQ.BUS
{
    public class LopHocTraCuuService
    {
        private readonly LopHocTraCuuRepository _repo = new LopHocTraCuuRepository();

        public string TimKiem(TimKiemLopHoc tieuChi, out DataTable ketQua)
        {
            ketQua = new DataTable();

            if (tieuChi.SiSoTu > tieuChi.SiSoDen)
                return "Sĩ số 'từ' không được lớn hơn sĩ số 'đến'.";

            PhamViInfo phamVi = PhamViDuLieu.ChoLopHoc(ChucNangConst.TIM_LOP);
            ketQua = _repo.TimKiem(tieuChi, phamVi);
            return "";
        }
    }
}
