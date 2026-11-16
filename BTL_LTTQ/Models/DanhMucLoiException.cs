namespace BTL_LTTQ.Models
{
    public enum DanhMucLoi
    {
        TrungKhoa,
        DangDuocSuDung,
        KhongTimThay,
        KhongTheDoiThuKhiDaDung,
        TrungTapThu
    }

    public sealed class DanhMucLoiException : Exception
    {
        public DanhMucLoi Loai { get; }

        public DanhMucLoiException(DanhMucLoi loai, string message, Exception? innerException = null)
            : base(message, innerException)
        {
            Loai = loai;
        }
    }
}
