using BTL_LTTQ.Models.Import;
using ClosedXML.Excel;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace BTL_LTTQ.Services.Import
{
    /// <summary>Định nghĩa một cột chuẩn của bảng đích và các tiêu đề được chấp nhận.</summary>
    public sealed class DinhNghiaCot
    {
        public DinhNghiaCot(string cot, string tenHienThi, bool batBuoc, IReadOnlyList<string> biDanh)
        {
            Cot = cot;
            TenHienThi = tenHienThi;
            BatBuoc = batBuoc;
            BiDanh = biDanh;
        }

        /// <summary>Tên cột thật trong database / khóa dùng nội bộ.</summary>
        public string Cot { get; }

        public string TenHienThi { get; }
        public bool BatBuoc { get; }
        public IReadOnlyList<string> BiDanh { get; }
    }

    public sealed class KetQuaDocTep
    {
        public List<ImportDong> DanhSachDong { get; } = new();
        public List<string> LoiFile { get; } = new();
        public List<string> CotBatBuocThieu { get; } = new();
        public List<string> CotKhongNhanDien { get; } = new();
    }

    /// <summary>
    /// Đọc dữ liệu từ .xlsx (ClosedXML) hoặc .csv (CsvHelper). Chỉ đọc, không ghi database.
    /// Tiêu đề cột được chấp nhận cả tên database (MaHocVien) và tên tiếng Việt (Mã học viên),
    /// không phân biệt hoa/thường, khoảng trắng hay dấu.
    /// </summary>
    public static class BangTinhReader
    {
        private static readonly IReadOnlyList<DinhNghiaCot> _cotHocVien = new List<DinhNghiaCot>
        {
            Truong("MaHocVien", "Mã học viên", true, "mahv", "mahs", "masinhvien", "mahocsinh"),
            Truong("TenHocVien", "Tên học viên", true, "tenhv", "hoten", "hotenhocvien", "tensinhvien", "tenhocsinh"),
            Truong("NgaySinh", "Ngày sinh", false),
            Truong("GioiTinh", "Giới tính", false),
            Truong("DiaChi", "Địa chỉ", false),
            Truong("MaNghe", "Mã nghề", false, "manguyenghe", "nghe"),
            Truong("DienThoai", "Điện thoại", false, "sdt", "dienthoai"),
            Truong("NgayNopHocPhi", "Ngày nộp học phí", false, "ngaynophp"),
            Truong("Diem", "Điểm", false, "diemtb", "diemtrungbinh"),
            Truong("MaXepLoai", "Mã xếp loại", false, "xeploai"),
            Truong("MaLop", "Mã lớp", false, "lop", "malophoc")
        }.AsReadOnly();

        private static readonly IReadOnlyList<DinhNghiaCot> _cotGiaoVien = new List<DinhNghiaCot>
        {
            Truong("MaGV", "Mã giáo viên", true, "magv", "magiaovien"),
            Truong("TenGV", "Tên giáo viên", true, "tengv", "tengiaovien", "hoten", "hotengiaovien"),
            Truong("NgaySinh", "Ngày sinh", false),
            Truong("GioiTinh", "Giới tính", false),
            Truong("DiaChi", "Địa chỉ", false),
            Truong("DienThoai", "Điện thoại", false, "sdt"),
            Truong("MaChucDanh", "Mã chức danh", false, "chucdanh"),
            Truong("MaChuyenMon", "Mã chuyên môn", false, "chuyenmon"),
            Truong("MaCQ", "Mã cơ quan", false, "macq", "macoquan", "coquan", "madonvi"),
            Truong("MaTrinhDo", "Mã trình độ", false, "trinhdo"),
            Truong("Anh", "Ảnh", false, "hinhanh", "duongdananh")
        }.AsReadOnly();

        private static readonly IReadOnlyList<DinhNghiaCot> _cotDangKy = new List<DinhNghiaCot>
        {
            Truong("MaHocVien", "Mã học viên", true, "mahv", "mahs", "masinhvien"),
            Truong("MaLop", "Mã lớp", true, "lop", "malophoc"),
            Truong("NgayDangKy", "Ngày đăng ký", false, "ngaydk")
        }.AsReadOnly();

        public static IReadOnlyList<DinhNghiaCot> LayDinhNghiaCot(ImportLoai loai)
        {
            return loai switch
            {
                ImportLoai.HocVien => _cotHocVien,
                ImportLoai.GiaoVien => _cotGiaoVien,
                ImportLoai.DangKy => _cotDangKy,
                _ => Array.Empty<DinhNghiaCot>()
            };
        }

        public static KetQuaDocTep Doc(string duongDan, ImportLoai loai)
        {
            var ketQua = new KetQuaDocTep();

            try
            {
                if (string.IsNullOrWhiteSpace(duongDan) || !File.Exists(duongDan))
                {
                    ketQua.LoiFile.Add("Tệp dữ liệu không tồn tại.");
                    return ketQua;
                }

                string duoi = Path.GetExtension(duongDan).ToLowerInvariant();
                switch (duoi)
                {
                    case ".xlsx":
                    case ".xlsm":
                        DocExcel(duongDan, loai, ketQua);
                        break;
                    case ".csv":
                        DocCsv(duongDan, loai, ketQua);
                        break;
                    default:
                        ketQua.LoiFile.Add("Chỉ hỗ trợ tệp Excel (.xlsx) hoặc CSV (.csv).");
                        break;
                }
            }
            catch (Exception ex)
            {
                ketQua.LoiFile.Add("Không đọc được tệp: " + ex.Message);
            }

            return ketQua;
        }

        private static void DocExcel(string duongDan, ImportLoai loai, KetQuaDocTep ketQua)
        {
            using var workbook = new XLWorkbook(duongDan);
            IXLWorksheet? worksheet = workbook.Worksheets.FirstOrDefault();
            if (worksheet == null)
            {
                ketQua.LoiFile.Add("Tệp Excel không có sheet dữ liệu nào.");
                return;
            }

            int dongCuoi = worksheet.LastRowUsed()?.RowNumber() ?? 0;
            int cotCuoi = worksheet.LastColumnUsed()?.ColumnNumber() ?? 0;

            if (dongCuoi < 1 || cotCuoi < 1)
            {
                ketQua.LoiFile.Add("Tệp Excel không có dữ liệu.");
                return;
            }

            int dongTieuDe = 1;
            while (dongTieuDe <= dongCuoi && !DongCoDuLieu(worksheet, dongTieuDe, cotCuoi))
                dongTieuDe++;

            if (dongTieuDe > dongCuoi)
            {
                ketQua.LoiFile.Add("Không tìm thấy dòng tiêu đề trong tệp Excel.");
                return;
            }

            var tieuDe = new string[cotCuoi];
            for (int c = 1; c <= cotCuoi; c++)
                tieuDe[c - 1] = DocO(worksheet.Cell(dongTieuDe, c));

            Dictionary<string, int> anhXa = XacDinhCot(tieuDe, loai, ketQua);
            if (ketQua.CotBatBuocThieu.Count > 0)
                return;

            for (int r = dongTieuDe + 1; r <= dongCuoi; r++)
            {
                var dong = new ImportDong { SoDong = r };
                bool coDuLieu = false;

                foreach (var cap in anhXa)
                {
                    string giaTri = DocO(worksheet.Cell(r, cap.Value + 1));
                    dong.DuLieu[cap.Key] = giaTri;
                    if (giaTri.Length > 0) coDuLieu = true;
                }

                if (coDuLieu)
                    ketQua.DanhSachDong.Add(dong);
            }

            if (ketQua.DanhSachDong.Count == 0)
                ketQua.LoiFile.Add("Tệp Excel không có dòng dữ liệu nào.");
        }

        private static void DocCsv(string duongDan, ImportLoai loai, KetQuaDocTep ketQua)
        {
            using StreamReader reader = MoReader(duongDan);

            var cauHinh = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                MissingFieldFound = null,
                HeaderValidated = null,
                BadDataFound = null,
                TrimOptions = TrimOptions.Trim,
                DetectDelimiter = true,
                DetectDelimiterValues = new[] { ",", ";", "\t", "|" }
            };

            using var csv = new CsvReader(reader, cauHinh);

            if (!csv.Read())
            {
                ketQua.LoiFile.Add("Tệp CSV không có dữ liệu.");
                return;
            }

            csv.ReadHeader();
            string[] tieuDe = csv.HeaderRecord ?? Array.Empty<string>();

            if (tieuDe.Length == 0)
            {
                ketQua.LoiFile.Add("Tệp CSV không có dòng tiêu đề.");
                return;
            }

            Dictionary<string, int> anhXa = XacDinhCot(tieuDe, loai, ketQua);
            if (ketQua.CotBatBuocThieu.Count > 0)
                return;

            while (csv.Read())
            {
                int soDong = Convert.ToInt32(csv.Parser.Row);
                var dong = new ImportDong { SoDong = soDong };
                bool coDuLieu = false;

                foreach (var cap in anhXa)
                {
                    string giaTri = (csv.GetField(cap.Value) ?? "").Trim();
                    dong.DuLieu[cap.Key] = giaTri;
                    if (giaTri.Length > 0) coDuLieu = true;
                }

                if (coDuLieu)
                    ketQua.DanhSachDong.Add(dong);
            }

            if (ketQua.DanhSachDong.Count == 0)
                ketQua.LoiFile.Add("Tệp CSV không có dòng dữ liệu nào.");
        }

        private static Dictionary<string, int> XacDinhCot(IReadOnlyList<string> tieuDe,
            ImportLoai loai, KetQuaDocTep ketQua)
        {
            var anhXa = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            var daChuanHoa = tieuDe.Select(ChuanHoa).ToList();
            var daDung = new HashSet<int>();

            foreach (DinhNghiaCot dinhNghia in LayDinhNghiaCot(loai))
            {
                int viTri = -1;

                for (int i = 0; i < daChuanHoa.Count; i++)
                {
                    if (daChuanHoa[i].Length == 0 || daDung.Contains(i)) continue;

                    if (dinhNghia.BiDanh.Contains(daChuanHoa[i], StringComparer.Ordinal))
                    {
                        viTri = i;
                        break;
                    }
                }

                if (viTri >= 0)
                {
                    anhXa[dinhNghia.Cot] = viTri;
                    daDung.Add(viTri);
                }
                else if (dinhNghia.BatBuoc)
                {
                    ketQua.CotBatBuocThieu.Add(dinhNghia.TenHienThi);
                }
            }

            for (int i = 0; i < tieuDe.Count; i++)
            {
                if (daChuanHoa[i].Length == 0 || daDung.Contains(i)) continue;

                string tenGoc = tieuDe[i].Trim();
                if (tenGoc.Length > 0)
                    ketQua.CotKhongNhanDien.Add(tenGoc);
            }

            return anhXa;
        }

        private static bool DongCoDuLieu(IXLWorksheet worksheet, int dong, int cotCuoi)
        {
            for (int c = 1; c <= cotCuoi; c++)
            {
                if (!worksheet.Cell(dong, c).IsEmpty())
                    return true;
            }

            return false;
        }

        private static string DocO(IXLCell o)
        {
            if (o == null || o.IsEmpty())
                return "";

            try
            {
                XLCellValue giaTri = o.Value;

                if (giaTri.IsDateTime)
                    return giaTri.GetDateTime().ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

                if (giaTri.IsNumber)
                    return giaTri.GetNumber().ToString(CultureInfo.InvariantCulture);

                if (giaTri.IsBoolean)
                    return giaTri.GetBoolean() ? "true" : "false";

                return o.GetString().Trim();
            }
            catch
            {
                return o.GetString().Trim();
            }
        }

        private static StreamReader MoReader(string duongDan)
        {
            byte[] duLieu = File.ReadAllBytes(duongDan);
            Encoding encoding = Encoding.UTF8;

            try
            {
                // Kiểm tra UTF-8 nghiêm ngặt để tránh đọc sai file CSV do Excel lưu theo bảng mã ANSI.
                _ = new UTF8Encoding(false, true).GetString(duLieu);
            }
            catch (DecoderFallbackException)
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                encoding = Encoding.GetEncoding(1258);
            }

            return new StreamReader(new MemoryStream(duLieu), encoding,
                detectEncodingFromByteOrderMarks: true);
        }

        /// <summary>Chuẩn hóa tiêu đề: bỏ dấu, khoảng trắng, ký tự đặc biệt và chuyển về chữ thường.</summary>
        public static string ChuanHoa(string? giaTri)
        {
            if (string.IsNullOrWhiteSpace(giaTri))
                return "";

            string chuThuong = giaTri.Trim().ToLowerInvariant().Replace('đ', 'd');
            string dangFormD = chuThuong.Normalize(NormalizationForm.FormD);

            var sb = new StringBuilder(dangFormD.Length);
            foreach (char c in dangFormD)
            {
                if (char.IsLetterOrDigit(c))
                    sb.Append(c);
            }

            return sb.ToString();
        }

        private static DinhNghiaCot Truong(string cot, string tenHienThi, bool batBuoc,
            params string[] biDanhThem)
        {
            var ds = new List<string> { ChuanHoa(cot), ChuanHoa(tenHienThi) };

            foreach (string ten in biDanhThem)
            {
                string chuanHoa = ChuanHoa(ten);
                if (chuanHoa.Length > 0 && !ds.Contains(chuanHoa))
                    ds.Add(chuanHoa);
            }

            return new DinhNghiaCot(cot, tenHienThi, batBuoc, ds);
        }
    }
}
