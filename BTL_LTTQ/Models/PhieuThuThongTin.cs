namespace BTL_LTTQ.Models
{
    public sealed record PhieuThuThongTin(
        int MaPhieu,
        string MaHocVien,
        string TenHocVien,
        DateTime NgayLap,
        decimal TongTien,
        string NoiDungCK,
        byte TrangThai,
        DateTime? NgayThu,
        string? NguoiThu);
}
