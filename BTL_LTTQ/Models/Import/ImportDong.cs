using System;
using System.Collections.Generic;
using System.Globalization;

namespace BTL_LTTQ.Models.Import
{
    /// <summary>
    /// Một dòng dữ liệu đọc từ Excel/CSV. DuLieu lưu giá trị theo tên cột chuẩn của database
    /// (ví dụ MaHocVien, TenHocVien...), không phụ thuộc tiêu đề người dùng đặt trong tệp.
    /// </summary>
    public class ImportDong
    {
        public int SoDong { get; set; }

        public Dictionary<string, string> DuLieu { get; } =
            new(StringComparer.OrdinalIgnoreCase);

        public List<ImportLoi> DanhSachLoi { get; } = new();

        public bool CoLoi => DanhSachLoi.Count > 0;

        public string? Lay(string cot)
        {
            return DuLieu.TryGetValue(cot, out string? giaTri) ? giaTri : null;
        }

        public string LayChuoi(string cot)
        {
            return (Lay(cot) ?? "").Trim();
        }

        /// <summary>Đọc ngày theo các định dạng thường gặp; trả null nếu rỗng hoặc không hợp lệ.</summary>
        public DateTime? LayNgay(string cot)
        {
            string s = LayChuoi(cot);
            if (s.Length == 0) return null;

            string[] dinhDang =
            {
                "yyyy-MM-dd", "yyyy-M-d", "yyyy/MM/dd", "yyyy/M/d",
                "dd/MM/yyyy", "d/M/yyyy", "dd-MM-yyyy", "d-M-yyyy",
                "dd/MM/yy", "d/M/yy", "dd.MM.yyyy", "d.M.yyyy"
            };

            if (DateTime.TryParseExact(s, dinhDang, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime ketQua))
                return ketQua.Date;

            if (DateTime.TryParse(s, CultureInfo.GetCultureInfo("vi-VN"),
                    DateTimeStyles.None, out ketQua))
                return ketQua.Date;

            if (DateTime.TryParse(s, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out ketQua))
                return ketQua.Date;

            return null;
        }

        /// <summary>Đọc số thực; hỗ trợ cả dấu phẩy thập phân (8,5) và dấu chấm (8.5).</summary>
        public decimal? LaySo(string cot)
        {
            string s = LayChuoi(cot);
            if (s.Length == 0) return null;

            s = s.Replace(" ", "");

            // "1.234,5" hoặc "8,5": bỏ dấu chấm phân cách nghìn, đổi dấu phẩy thành dấu chấm.
            if (s.Contains(','))
                s = s.Replace(".", "").Replace(',', '.');

            if (decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture,
                    out decimal ketQua))
                return ketQua;

            return null;
        }
    }
}
