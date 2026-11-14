using System;

namespace BTL_LTTQ.Models
{
    public enum LoaiPhamVi
    {
        TatCa = 0,
        TuChoi = 1,
        GiaoVien = 2,
        HocVien = 3
    }

    /// <summary>
    /// Thông tin phạm vi dữ liệu do BUS quyết định; DAL chuyển thành điều kiện SQL.
    /// </summary>
    public class PhamViInfo
    {
        public LoaiPhamVi Loai { get; set; } = LoaiPhamVi.TatCa;
        public string? Ma { get; set; }
    }
}
