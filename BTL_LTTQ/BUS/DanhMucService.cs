using BTL_LTTQ.Common;
using BTL_LTTQ.DAL;
using BTL_LTTQ.Models;
using System.Data;

namespace BTL_LTTQ.BUS
{
    public sealed class DanhMucService
    {
        private readonly DanhMucKhacRepository _repository = new();

        private static readonly IReadOnlyDictionary<DanhMucLoai, DanhMucMoTa> MoTa =
            new Dictionary<DanhMucLoai, DanhMucMoTa>
            {
                [DanhMucLoai.NgheNghiep] = new(DanhMucLoai.NgheNghiep, "Nghề nghiệp", new[] { Khoa("MaNghe", "Mã nghề"), Text("TenNghe", "Tên nghề", true, 100) }),
                [DanhMucLoai.ChuyenMon] = new(DanhMucLoai.ChuyenMon, "Chuyên môn", new[] { Khoa("MaChuyenMon", "Mã chuyên môn"), Text("TenChuyenMon", "Tên chuyên môn", true, 100) }),
                [DanhMucLoai.TrinhDo] = new(DanhMucLoai.TrinhDo, "Trình độ", new[] { Khoa("MaTrinhDo", "Mã trình độ"), Text("TenTrinhDo", "Tên trình độ", true, 100) }),
                [DanhMucLoai.CoQuan] = new(DanhMucLoai.CoQuan, "Cơ quan", new[] { Khoa("MaCQ", "Mã cơ quan"), Text("TenCQ", "Tên cơ quan", true, 100), Text("DiaChi", "Địa chỉ", false, 200), Text("DienThoai", "Điện thoại", false, 20) }),
                [DanhMucLoai.PhongHoc] = new(DanhMucLoai.PhongHoc, "Phòng học", new[] { Khoa("MaPhong", "Mã phòng"), Text("TenPhong", "Tên phòng", true, 100), Text("DiaChi", "Địa chỉ", false, 200), new DanhMucTruong("SucChua", "Sức chứa", DanhMucKieuTruong.So, false) }),
                [DanhMucLoai.CaHoc] = new(DanhMucLoai.CaHoc, "Ca học", new[] { Khoa("MaCa", "Mã ca"), Text("TenCa", "Tên ca", true, 50), new DanhMucTruong("GioBatDau", "Giờ bắt đầu", DanhMucKieuTruong.Gio, true), new DanhMucTruong("GioKetThuc", "Giờ kết thúc", DanhMucKieuTruong.Gio, true) }),
                [DanhMucLoai.ThoiGian] = new(DanhMucLoai.ThoiGian, "Thời gian", new[] { Khoa("MaThoiGian", "Mã thời gian"), Text("TenThoiGian", "Tên thời gian", true, 100) }, true)
            };

        public bool CoQuyen => PermissionService.Can(ChucNangConst.DANHMUC_KHAC);

        public IReadOnlyList<DanhMucMoTa> LayMoTa()
        {
            YeuCauQuyen();
            return Enum.GetValues<DanhMucLoai>().Select(loai => MoTa[loai]).ToArray();
        }

        public DanhMucMoTa LayMoTa(DanhMucLoai loai)
        {
            YeuCauQuyen();
            return MoTa.TryGetValue(loai, out DanhMucMoTa? moTa) ? moTa : throw new ArgumentOutOfRangeException(nameof(loai));
        }

        public string LayDanhSach(DanhMucLoai loai, out DataTable danhSach)
        {
            danhSach = new DataTable();
            if (!CoQuyen) return "Bạn không có quyền sử dụng chức năng Danh mục khác.";
            try { danhSach = _repository.LayDanhSach(loai); return ""; }
            catch (Exception ex) { return Loi(ex); }
        }

        public string LayMaThoiGianKeTiep(out string ma)
        {
            ma = "";
            if (!CoQuyen) return "Bạn không có quyền sử dụng chức năng Danh mục khác.";
            try { ma = _repository.LayMaThoiGianKeTiep(); return ""; }
            catch (Exception ex) { return Loi(ex); }
        }

        public string Them(DanhMucLoai loai, IReadOnlyDictionary<string, object?> giaTri, IReadOnlyCollection<byte>? cacThu = null)
        {
            if (!CoQuyen) return "Bạn không có quyền sử dụng chức năng Danh mục khác.";
            IReadOnlyDictionary<string, object?> giaTriChuanHoa = ChuanHoaGiaTri(loai, giaTri, cacThu);
            string loi = KiemTra(loai, giaTriChuanHoa, cacThu);
            if (loi != "") return loi;
            try
            {
                string ma = Convert.ToString(giaTriChuanHoa[MoTa[loai].Truong[0].Ten])!;
                if (_repository.TonTai(loai, ma)) return "Mã đã tồn tại.";
                _repository.Them(loai, giaTriChuanHoa, cacThu);
                return "";
            }
            catch (Exception ex) { return Loi(ex); }
        }

        public string Sua(DanhMucLoai loai, string maGoc, IReadOnlyDictionary<string, object?> giaTri, IReadOnlyCollection<byte>? cacThu = null)
        {
            if (!CoQuyen) return "Bạn không có quyền sử dụng chức năng Danh mục khác.";
            IReadOnlyDictionary<string, object?> giaTriChuanHoa = ChuanHoaGiaTri(loai, giaTri, cacThu);
            string loi = KiemTra(loai, giaTriChuanHoa, cacThu);
            if (loi != "") return loi;
            if (string.IsNullOrWhiteSpace(maGoc)) return "Vui lòng chọn dòng cần sửa.";
            try { _repository.Sua(loai, maGoc, giaTriChuanHoa, cacThu); return ""; }
            catch (Exception ex) { return Loi(ex); }
        }

        public string Xoa(DanhMucLoai loai, string ma)
        {
            if (!CoQuyen) return "Bạn không có quyền sử dụng chức năng Danh mục khác.";
            if (string.IsNullOrWhiteSpace(ma)) return "Vui lòng chọn dòng cần xóa.";
            try { _repository.Xoa(loai, ma); return ""; }
            catch (Exception ex) { return Loi(ex); }
        }

        private static string KiemTra(DanhMucLoai loai, IReadOnlyDictionary<string, object?> values, IReadOnlyCollection<byte>? days)
        {
            DanhMucMoTa moTa = MoTa[loai];
            foreach (DanhMucTruong field in moTa.Truong)
            {
                values.TryGetValue(field.Ten, out object? value);
                bool empty = value == null || value == DBNull.Value || (value is string text && string.IsNullOrWhiteSpace(text));
                if (field.BatBuoc && empty) return $"Vui lòng nhập {field.Nhan.ToLowerInvariant()}.";
                if (empty) continue;
                if (field.MaxLength.HasValue && Convert.ToString(value)!.Length > field.MaxLength.Value)
                    return $"{field.Nhan} không được vượt quá {field.MaxLength.Value} ký tự.";
                if (field.LaKhoa && value is string code && code != code.Trim())
                    return $"{field.Nhan} không được có khoảng trắng đầu hoặc cuối.";
            }

            if (loai == DanhMucLoai.PhongHoc && values.TryGetValue("SucChua", out object? capacity) && capacity != null && capacity != DBNull.Value)
            {
                if (!int.TryParse(Convert.ToString(capacity), out int parsedCapacity)) return "Sức chứa phải là số nguyên.";
                if (parsedCapacity < 0) return "Sức chứa phải lớn hơn hoặc bằng 0.";
            }
            if (loai == DanhMucLoai.CaHoc)
            {
                if (!values.TryGetValue("GioBatDau", out object? start) || start == null || start == DBNull.Value ||
                    !values.TryGetValue("GioKetThuc", out object? end) || end == null || end == DBNull.Value)
                    return "Ca học bắt buộc nhập cả giờ bắt đầu và giờ kết thúc.";
                if (start is not TimeSpan gioBatDau || end is not TimeSpan gioKetThuc)
                    return "Giờ bắt đầu và giờ kết thúc không hợp lệ.";
                if (gioKetThuc <= gioBatDau) return "Giờ kết thúc phải sau giờ bắt đầu.";
            }
            if (loai == DanhMucLoai.ThoiGian && (days == null || days.Count == 0))
                return "Thời gian phải tích ít nhất một thứ.";
            if (days?.Any(day => !ThuTrongTuanConst.CacThu.Contains(day)) == true) return "Ngày trong tuần không hợp lệ.";
            return "";
        }

        private static IReadOnlyDictionary<string, object?> ChuanHoaGiaTri(
            DanhMucLoai loai, IReadOnlyDictionary<string, object?> giaTri, IReadOnlyCollection<byte>? cacThu)
        {
            var ketQua = new Dictionary<string, object?>(giaTri, StringComparer.Ordinal);
            if (loai == DanhMucLoai.ThoiGian)
                ketQua["TenThoiGian"] = ThuTrongTuanConst.TaoTenThoiGian(cacThu ?? Array.Empty<byte>());
            return ketQua;
        }

        private static string Loi(Exception ex)
        {
            if (ex is DanhMucLoiException business)
                return business.Loai switch
                {
                    DanhMucLoi.TrungKhoa => "Mã đã tồn tại.",
                    DanhMucLoi.DangDuocSuDung when business.InnerException is Microsoft.Data.SqlClient.SqlException sql && sql.Number == 547 => "Đang được sử dụng nên không thể xóa.",
                    DanhMucLoi.DangDuocSuDung => business.Message,
                    DanhMucLoi.KhongTheDoiThuKhiDaDung => business.Message,
                    DanhMucLoi.TrungTapThu => business.Message,
                    DanhMucLoi.KhongTimThay => business.Message,
                    _ => business.Message
                };
            return ex.Message;
        }

        private static void YeuCauQuyen()
        {
            if (!PermissionService.Can(ChucNangConst.DANHMUC_KHAC))
                throw new UnauthorizedAccessException("Bạn không có quyền sử dụng chức năng Danh mục khác.");
        }

        private static DanhMucTruong Khoa(string ten, string nhan) => new(ten, nhan, DanhMucKieuTruong.Text, true, 20, true);
        private static DanhMucTruong Text(string ten, string nhan, bool required, int length, bool isKey = false) => new(ten, nhan, DanhMucKieuTruong.Text, required, length, isKey);
    }
}
