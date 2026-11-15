using BTL_LTTQ.Common;
using BTL_LTTQ.DAL;
using BTL_LTTQ.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BTL_LTTQ.BUS
{
    public sealed class NhapDiemService
    {
        private static readonly (string Khoa, string Ten)[] LoaiBatBuoc =
        {
            ("xuatsac", "Xuất sắc"),
            ("gioi", "Giỏi"),
            ("kha", "Khá"),
            ("trungbinh", "Trung bình")
        };

        private readonly NhapDiemRepository _repository = new();

        public string LayDanhSachLop(out DataTable danhSach)
        {
            danhSach = new DataTable();
            string loi = LayPhamVi(out PhamViInfo phamVi);
            if (loi != "") return loi;

            try
            {
                danhSach = _repository.LayDanhSachLop(phamVi);
                return "";
            }
            catch (Exception ex)
            {
                return "Không tải được danh sách lớp: " + ex.Message;
            }
        }

        public string LayDanhSachHocVien(string maLop, out DataTable danhSach)
        {
            danhSach = new DataTable();
            if (string.IsNullOrWhiteSpace(maLop))
                return "Vui lòng chọn lớp học.";

            string loi = LayPhamVi(out PhamViInfo phamVi);
            if (loi != "") return loi;

            try
            {
                DataTable cacLop = _repository.LayDanhSachLop(phamVi);
                if (!cacLop.AsEnumerable().Any(dong =>
                        string.Equals(dong.Field<string>("MaLop"), maLop, StringComparison.OrdinalIgnoreCase)))
                    return "Lớp học không nằm trong phạm vi được phép.";

                danhSach = _repository.LayDanhSachHocVien(maLop, phamVi);
                return "";
            }
            catch (Exception ex)
            {
                return "Không tải được danh sách học viên: " + ex.Message;
            }
        }

        public string LayDanhSachXepLoai(out DataTable danhSach)
        {
            danhSach = new DataTable();
            string loi = LayPhamVi(out _);
            if (loi != "") return loi;

            try
            {
                danhSach = _repository.LayDanhSachXepLoai();
                return TaoMaXepLoai(danhSach, out _);
            }
            catch (Exception ex)
            {
                return "Không tải được danh sách xếp loại: " + ex.Message;
            }
        }

        public string TinhTenXepLoai(decimal? diem, DataTable danhSachXepLoai, out string tenXepLoai)
        {
            tenXepLoai = "";
            string loi = TaoMaXepLoai(danhSachXepLoai, out Dictionary<string, string> maTheoTen);
            if (loi != "") return loi;

            loi = TinhMaXepLoai(diem, maTheoTen, out string? maXepLoai);
            if (loi != "") return loi;
            if (diem == null) return "";

            if (diem < 5m && maXepLoai == null)
            {
                tenXepLoai = "Chưa đạt";
                return "";
            }

            DataRow? dong = danhSachXepLoai.AsEnumerable().FirstOrDefault(r =>
                string.Equals(r.Field<string>("MaXepLoai"), maXepLoai, StringComparison.OrdinalIgnoreCase));
            tenXepLoai = dong?.Field<string>("TenLoai") ?? "";
            return "";
        }

        public string LuuDiem(string maLop, IReadOnlyCollection<DiemNhapDangKy> danhSach)
        {
            if (string.IsNullOrWhiteSpace(maLop))
                return "Vui lòng chọn lớp học.";
            if (danhSach == null)
                return "Danh sách điểm không hợp lệ.";
            if (danhSach.Count == 0)
                return "";

            string loi = LayPhamVi(out PhamViInfo phamVi);
            if (loi != "") return loi;

            try
            {
                DataTable danhSachXepLoai = _repository.LayDanhSachXepLoai();
                loi = TaoMaXepLoai(danhSachXepLoai, out Dictionary<string, string> maTheoTen);
                if (loi != "") return loi;

                var daGap = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                var capNhat = new List<CapNhatDiemDangKy>(danhSach.Count);
                foreach (DiemNhapDangKy dong in danhSach)
                {
                    if (string.IsNullOrWhiteSpace(dong.MaHocVien) || !daGap.Add(dong.MaHocVien))
                        return "Danh sách có mã học viên trống hoặc bị trùng.";
                    if (dong.Diem.HasValue && (dong.Diem.Value < 0m || dong.Diem.Value > 10m))
                        return $"Điểm của học viên {dong.MaHocVien} phải từ 0 đến 10.";

                    loi = TinhMaXepLoai(dong.Diem, maTheoTen, out string? maXepLoai);
                    if (loi != "") return loi;
                    capNhat.Add(new CapNhatDiemDangKy(dong.MaHocVien, maLop, dong.Diem, maXepLoai));
                }

                _repository.CapNhatDiem(phamVi, capNhat);
                return "";
            }
            catch (SqlException ex)
            {
                return "Không lưu được điểm: " + ex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private static string LayPhamVi(out PhamViInfo phamVi)
        {
            phamVi = new PhamViInfo(LoaiPhamVi.TuChoi);
            if (!PermissionService.Can(ChucNangConst.HOCVIEN_NHAPDIEM))
                return "Bạn không có quyền sử dụng chức năng nhập điểm.";
            if (CurrentUser.IsHocVien)
                return "Học viên không được nhập điểm.";

            phamVi = PhamViDuLieu.ChoNhapDiem();
            if (phamVi.Loai == LoaiPhamVi.TuChoi)
                return "Không xác định được phạm vi lớp học được phép.";
            if (phamVi.Loai == LoaiPhamVi.GiaoVien && string.IsNullOrWhiteSpace(phamVi.MaGiaoVien))
            {
                phamVi = new PhamViInfo(LoaiPhamVi.TuChoi);
                return "Không xác định được mã giáo viên.";
            }

            return "";
        }

        private static string TaoMaXepLoai(DataTable danhSach, out Dictionary<string, string> maTheoTen)
        {
            var ketQua = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (DataRow dong in danhSach.Rows)
            {
                string ten = dong.Field<string>("TenLoai") ?? "";
                string ma = dong.Field<string>("MaXepLoai") ?? "";
                string khoa = ChuanHoaTenLoai(ten);
                if (khoa != "" && ma != "" && !ketQua.ContainsKey(khoa))
                    ketQua.Add(khoa, ma);
            }

            var thieu = new List<string>();
            foreach (var loai in LoaiBatBuoc)
                if (!ketQua.ContainsKey(loai.Khoa))
                    thieu.Add(loai.Ten);
            maTheoTen = ketQua;
            return thieu.Count == 0
                ? ""
                : "Thiếu các loại bắt buộc trong bảng XepLoai: " + string.Join(", ", thieu) + ".";
        }

        private static string TinhMaXepLoai(decimal? diem, Dictionary<string, string> maTheoTen, out string? maXepLoai)
        {
            maXepLoai = null;
            if (!diem.HasValue) return "";
            if (diem.Value < 0m || diem.Value > 10m)
                return "Điểm phải nằm trong khoảng từ 0 đến 10.";

            string khoa = diem.Value >= 9.5m ? "xuatsac"
                : diem.Value >= 8.0m ? "gioi"
                : diem.Value >= 7.0m ? "kha"
                : diem.Value >= 5.0m ? "trungbinh"
                : maTheoTen.ContainsKey("yeu") ? "yeu"
                : maTheoTen.ContainsKey("kem") ? "kem"
                : "";

            if (khoa != "")
                maXepLoai = maTheoTen[khoa];
            return "";
        }

        private static string ChuanHoaTenLoai(string ten)
        {
            string chuanHoa = ten.Trim().Normalize(NormalizationForm.FormD);
            var ketQua = new StringBuilder(chuanHoa.Length);
            foreach (char kyTu in chuanHoa)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(kyTu) == UnicodeCategory.NonSpacingMark || char.IsWhiteSpace(kyTu))
                    continue;
                ketQua.Append(char.ToLowerInvariant(kyTu == 'đ' || kyTu == 'Đ' ? 'd' : kyTu));
            }
            return ketQua.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
