using System;
using System.Collections.Generic;

namespace BTL_LTTQ.Models.Import
{
    /// <summary>Kết quả ghi dữ liệu vào database. ThanhCong = false thì không có dòng nào được ghi.</summary>
    public class KetQuaImport
    {
        public Guid OperationId { get; set; }
        public ImportLoai Loai { get; set; }
        public bool ThanhCong { get; set; }
        public int SoDongDaThem { get; set; }

        /// <summary>Danh sách lớp bị ảnh hưởng (chỉ có ý nghĩa với ImportLoai.DangKy).</summary>
        public List<string> LopBiAnhHuong { get; } = new();

        public string ThongDiep { get; set; } = "";
        public List<ImportLoi> DanhSachLoi { get; } = new();
    }
}
