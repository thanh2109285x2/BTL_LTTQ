using System;
using System.Collections.Generic;
using System.Linq;

namespace BTL_LTTQ.Models.Import
{
    /// <summary>
    /// Kết quả đọc + kiểm tra dữ liệu trước khi ghi. Không chứa dữ liệu nào đã được ghi vào database.
    /// OperationId chỉ dùng trong bộ nhớ để đối chiếu kết quả với lần xem trước.
    /// </summary>
    public class KetQuaPreview
    {
        public Guid OperationId { get; } = Guid.NewGuid();
        public ImportLoai Loai { get; set; }
        public string TenFile { get; set; } = "";

        /// <summary>Lỗi cấp tệp: không mở được, sai định dạng, không có dữ liệu...</summary>
        public List<string> LoiFile { get; } = new();

        /// <summary>Các cột bắt buộc bị thiếu trong dòng tiêu đề.</summary>
        public List<string> CotBatBuocThieu { get; } = new();

        /// <summary>Các cột có trong tệp nhưng không được dùng (cảnh báo, không chặn nhập).</summary>
        public List<string> CotKhongNhanDien { get; } = new();

        public List<ImportDong> DanhSachDong { get; } = new();

        public int TongSoDong => DanhSachDong.Count;
        public int SoDongLoi => DanhSachDong.Count(d => d.CoLoi);
        public int SoDongHopLe => DanhSachDong.Count(d => !d.CoLoi);

        public bool CoTheImport =>
            LoiFile.Count == 0 &&
            CotBatBuocThieu.Count == 0 &&
            DanhSachDong.Count > 0 &&
            SoDongLoi == 0;

        public IEnumerable<ImportLoi> TatCaLoi => DanhSachDong.SelectMany(d => d.DanhSachLoi);
    }
}
