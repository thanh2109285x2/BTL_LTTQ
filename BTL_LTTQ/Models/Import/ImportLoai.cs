using System;

namespace BTL_LTTQ.Models.Import
{
    /// <summary>Ba loại dữ liệu được phép nhập hàng loạt. Mỗi lần chỉ nhập một loại.</summary>
    public enum ImportLoai
    {
        HocVien = 1,
        GiaoVien = 2,
        DangKy = 3
    }

    public static class ImportLoaiMoTa
    {
        public static readonly ImportLoai[] TatCa =
        {
            ImportLoai.HocVien, ImportLoai.GiaoVien, ImportLoai.DangKy
        };

        public static string TenHienThi(ImportLoai loai)
        {
            return loai switch
            {
                ImportLoai.HocVien => "Học viên",
                ImportLoai.GiaoVien => "Giáo viên",
                ImportLoai.DangKy => "Đăng ký lớp",
                _ => loai.ToString()
            };
        }

        public static string TenBang(ImportLoai loai)
        {
            return loai switch
            {
                ImportLoai.HocVien => "HocVien",
                ImportLoai.GiaoVien => "GiaoVien",
                ImportLoai.DangKy => "DangKy",
                _ => loai.ToString()
            };
        }

        public static string TenFileMau(ImportLoai loai)
        {
            string ten = loai switch
            {
                ImportLoai.HocVien => "HocVien",
                ImportLoai.GiaoVien => "GiaoVien",
                ImportLoai.DangKy => "DangKy",
                _ => "DuLieu"
            };
            return $"MauNhap_{ten}.xlsx";
        }
    }
}
