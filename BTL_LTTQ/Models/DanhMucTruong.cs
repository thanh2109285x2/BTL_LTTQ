namespace BTL_LTTQ.Models
{
    public enum DanhMucKieuTruong
    {
        Text,
        So,
        Gio
    }

    public sealed record DanhMucTruong(
        string Ten,
        string Nhan,
        DanhMucKieuTruong Kieu,
        bool BatBuoc,
        int? MaxLength = null,
        bool LaKhoa = false);

    public sealed record DanhMucMoTa(
        DanhMucLoai Loai,
        string TenHienThi,
        IReadOnlyList<DanhMucTruong> Truong,
        bool CoThuTrongTuan = false);
}
