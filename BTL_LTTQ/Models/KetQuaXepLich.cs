namespace BTL_LTTQ.Models
{
    //Kết quả chạy bộ xếp lịch
    public class KetQuaXepLich
    {
        public bool ThanhCong { get; set; }
        public string ThongBao { get; set; } = "";
        public int SoLopDaXep { get; set; }
        public int SoBuoi { get; set; }

        public static KetQuaXepLich Loi(string thongBao)
        {
            return new KetQuaXepLich { ThanhCong = false, ThongBao = thongBao };
        }
    }
}
