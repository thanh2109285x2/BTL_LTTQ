namespace BTL_LTTQ.Models
{
    public enum LoaiPhamVi
    {
        TatCa,
        GiaoVien,
        HocVien,
        TuChoi
    }

    public sealed record PhamViInfo(
        LoaiPhamVi Loai,
        string? MaGiaoVien = null,
        string? MaHocVien = null);
}
