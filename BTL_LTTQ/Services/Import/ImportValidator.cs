using BTL_LTTQ.Models.Import;
using BTL_LTTQ.Repositories.Import;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BTL_LTTQ.Services.Import
{
    /// <summary>
    /// Kiểm tra dữ liệu đã đọc từ tệp dựa trên dữ liệu tham chiếu trong database.
    /// Chỉ báo lỗi, không sửa dữ liệu và không ghi database.
    /// </summary>
    public class ImportValidator
    {
        private static readonly string[] _trangThaiChoDangKy = { "SapKhaiGiang", "DangHoc" };

        private readonly ImportLookupRepository _lookup;

        public ImportValidator(ImportLookupRepository lookup)
        {
            _lookup = lookup ?? throw new ArgumentNullException(nameof(lookup));
        }

        public void KiemTra(KetQuaPreview preview)
        {
            ArgumentNullException.ThrowIfNull(preview);

            if (preview.LoiFile.Count > 0 || preview.CotBatBuocThieu.Count > 0)
                return;

            switch (preview.Loai)
            {
                case ImportLoai.HocVien:
                    KiemTraHocVien(preview);
                    break;
                case ImportLoai.GiaoVien:
                    KiemTraGiaoVien(preview);
                    break;
                case ImportLoai.DangKy:
                    KiemTraDangKy(preview);
                    break;
            }
        }

        private void KiemTraHocVien(KetQuaPreview preview)
        {
            var daGap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            foreach (ImportDong dong in preview.DanhSachDong)
            {
                KiemTraMa(dong, "MaHocVien", "Mã học viên", true, daGap, _lookup.MaHocVien);
                KiemTraChuoiBatBuoc(dong, "TenHocVien", "Tên học viên", 100);

                KiemTraNgay(dong, "NgaySinh", "Ngày sinh");
                KiemTraDoDai(dong, "GioiTinh", "Giới tính", 10);
                KiemTraDoDai(dong, "DiaChi", "Địa chỉ", 200);
                KiemTraMaThamChieu(dong, "MaNghe", "Mã nghề", _lookup.MaNghe, "Nghề nghiệp");
                KiemTraDoDai(dong, "DienThoai", "Điện thoại", 20);
                KiemTraNgay(dong, "NgayNopHocPhi", "Ngày nộp học phí");
                KiemTraSo(dong, "Diem", "Điểm");
                KiemTraMaThamChieu(dong, "MaXepLoai", "Mã xếp loại", _lookup.MaXepLoai, "Xếp loại");
                KiemTraMaLopKhongBatBuoc(dong);
            }
        }

        private void KiemTraGiaoVien(KetQuaPreview preview)
        {
            var daGap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            foreach (ImportDong dong in preview.DanhSachDong)
            {
                KiemTraMa(dong, "MaGV", "Mã giáo viên", true, daGap, _lookup.MaGiaoVien);
                KiemTraChuoiBatBuoc(dong, "TenGV", "Tên giáo viên", 100);

                KiemTraNgay(dong, "NgaySinh", "Ngày sinh");
                KiemTraDoDai(dong, "GioiTinh", "Giới tính", 10);
                KiemTraDoDai(dong, "DiaChi", "Địa chỉ", 200);
                KiemTraDoDai(dong, "DienThoai", "Điện thoại", 20);
                KiemTraMaThamChieu(dong, "MaChucDanh", "Mã chức danh", _lookup.MaChucDanh, "Chức danh");
                KiemTraMaThamChieu(dong, "MaChuyenMon", "Mã chuyên môn", _lookup.MaChuyenMon, "Chuyên môn");
                KiemTraMaThamChieu(dong, "MaCQ", "Mã cơ quan", _lookup.MaCoQuan, "Cơ quan");
                KiemTraMaThamChieu(dong, "MaTrinhDo", "Mã trình độ", _lookup.MaTrinhDo, "Trình độ");
                KiemTraDoDai(dong, "Anh", "Ảnh", 255);
            }
        }

        private void KiemTraDangKy(KetQuaPreview preview)
        {
            var daGap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            var soDongTheoLop = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            foreach (ImportDong dong in preview.DanhSachDong)
            {
                string maHocVien = dong.LayChuoi("MaHocVien");
                string maLop = dong.LayChuoi("MaLop");

                KiemTraMaLienKet(dong, "MaHocVien", "Mã học viên", _lookup.MaHocVien, "Học viên");
                KiemTraMaLienKet(dong, "MaLop", "Mã lớp", _lookup.LopHoc.Keys, "Lớp học");
                KiemTraNgay(dong, "NgayDangKy", "Ngày đăng ký");

                if (maHocVien.Length > 0 && maLop.Length > 0)
                {
                    string khoa = ImportLookupRepository.TaoKhoaCap(maHocVien, maLop);

                    if (daGap.TryGetValue(khoa, out int dongDau))
                    {
                        dong.DanhSachLoi.Add(new ImportLoi
                        {
                            SoDong = dong.SoDong,
                            Cot = "MaLop",
                            GiaTri = maLop,
                            ThongDiep = $"Học viên '{maHocVien}' đã được đăng ký lớp '{maLop}' ở dòng {dongDau} trong tệp."
                        });
                    }
                    else
                    {
                        daGap[khoa] = dong.SoDong;

                        if (_lookup.CapDangKy.Contains(khoa))
                        {
                            dong.DanhSachLoi.Add(new ImportLoi
                            {
                                SoDong = dong.SoDong,
                                Cot = "MaLop",
                                GiaTri = maLop,
                                ThongDiep = $"Học viên '{maHocVien}' đã đăng ký lớp '{maLop}' trong hệ thống."
                            });
                        }
                    }

                    if (_lookup.LopHoc.TryGetValue(maLop, out LopThongTinImport? lop))
                    {
                        if (Array.IndexOf(_trangThaiChoDangKy, lop.TrangThai) < 0)
                        {
                            dong.DanhSachLoi.Add(new ImportLoi
                            {
                                SoDong = dong.SoDong,
                                Cot = "MaLop",
                                GiaTri = maLop,
                                ThongDiep = $"Lớp '{maLop}' đang ở trạng thái '{lop.TrangThai}', không nhận đăng ký."
                            });
                        }
                    }

                    if (soDongTheoLop.TryGetValue(maLop, out int soDong))
                        soDongTheoLop[maLop] = soDong + 1;
                    else
                        soDongTheoLop[maLop] = 1;
                }
            }

            // Kiểm tra sức chứa theo tổng số dòng trong tệp, không chỉ từng dòng riêng lẻ.
            foreach (var cap in soDongTheoLop)
            {
                if (!_lookup.LopHoc.TryGetValue(cap.Key, out LopThongTinImport? lop))
                    continue;

                int conLai = lop.SiSoToiDa - lop.SiSoHienTai;
                if (cap.Value > conLai)
                {
                    string thongDiep = conLai <= 0
                        ? $"Lớp '{lop.MaLop}' đã đủ sĩ số ({lop.SiSoHienTai}/{lop.SiSoToiDa})."
                        : $"Lớp '{lop.MaLop}' chỉ còn nhận {conLai} học viên nhưng tệp có {cap.Value} đăng ký.";

                    foreach (ImportDong dong in preview.DanhSachDong.Where(d =>
                                 string.Equals(d.LayChuoi("MaLop"), lop.MaLop, StringComparison.OrdinalIgnoreCase)))
                    {
                        dong.DanhSachLoi.Add(new ImportLoi
                        {
                            SoDong = dong.SoDong,
                            Cot = "MaLop",
                            GiaTri = lop.MaLop,
                            ThongDiep = thongDiep
                        });
                    }
                }
            }
        }

        private static void KiemTraMa(ImportDong dong, string cot, string tenCot, bool batBuoc,
            Dictionary<string, int> daGap, HashSet<string> daCoTrongDb)
        {
            string giaTri = dong.LayChuoi(cot);

            if (giaTri.Length == 0)
            {
                if (batBuoc)
                    ThemLoi(dong, cot, "", $"{tenCot} không được để trống.");
                return;
            }

            if (giaTri.Length > 20)
                ThemLoi(dong, cot, giaTri, $"{tenCot} tối đa 20 ký tự.");

            if (!ChiGomKyTuMa(giaTri))
                ThemLoi(dong, cot, giaTri, $"{tenCot} chỉ được chứa chữ, số và các ký tự _ . -");

            if (daGap.TryGetValue(giaTri, out int dongTruoc))
            {
                ThemLoi(dong, cot, giaTri, $"{tenCot} '{giaTri}' bị trùng với dòng {dongTruoc} trong tệp.");
            }
            else
            {
                daGap[giaTri] = dong.SoDong;
            }

            if (daCoTrongDb.Contains(giaTri))
                ThemLoi(dong, cot, giaTri, $"{tenCot} '{giaTri}' đã tồn tại trong hệ thống.");
        }

        private static void KiemTraMaLienKet(ImportDong dong, string cot, string tenCot,
            IEnumerable<string> tapHop, string tenBang)
        {
            string giaTri = dong.LayChuoi(cot);

            if (giaTri.Length == 0)
            {
                ThemLoi(dong, cot, "", $"{tenCot} không được để trống.");
                return;
            }

            if (giaTri.Length > 20)
                ThemLoi(dong, cot, giaTri, $"{tenCot} tối đa 20 ký tự.");

            if (!tapHop.Contains(giaTri))
                ThemLoi(dong, cot, giaTri, $"{tenCot} '{giaTri}' không tồn tại trong {tenBang}.");
        }

        private static void KiemTraMaThamChieu(ImportDong dong, string cot, string tenCot,
            HashSet<string> tapHop, string tenBang)
        {
            string giaTri = dong.LayChuoi(cot);

            if (giaTri.Length == 0)
                return;

            if (giaTri.Length > 20)
                ThemLoi(dong, cot, giaTri, $"{tenCot} tối đa 20 ký tự.");
            else if (!tapHop.Contains(giaTri))
                ThemLoi(dong, cot, giaTri, $"{tenCot} '{giaTri}' không tồn tại trong {tenBang}.");
        }

        private void KiemTraMaLopKhongBatBuoc(ImportDong dong)
        {
            string maLop = dong.LayChuoi("MaLop");

            if (maLop.Length == 0)
                return;

            if (maLop.Length > 20)
                ThemLoi(dong, "MaLop", maLop, "Mã lớp tối đa 20 ký tự.");
            else if (!_lookup.LopHoc.ContainsKey(maLop))
                ThemLoi(dong, "MaLop", maLop, $"Mã lớp '{maLop}' không tồn tại trong Lớp học.");
        }

        private static void KiemTraChuoiBatBuoc(ImportDong dong, string cot, string tenCot, int doDaiToiDa)
        {
            string giaTri = dong.LayChuoi(cot);

            if (giaTri.Length == 0)
            {
                ThemLoi(dong, cot, "", $"{tenCot} không được để trống.");
                return;
            }

            if (giaTri.Length > doDaiToiDa)
                ThemLoi(dong, cot, giaTri, $"{tenCot} tối đa {doDaiToiDa} ký tự.");
        }

        private static void KiemTraDoDai(ImportDong dong, string cot, string tenCot, int doDaiToiDa)
        {
            string giaTri = dong.LayChuoi(cot);

            if (giaTri.Length > doDaiToiDa)
                ThemLoi(dong, cot, giaTri, $"{tenCot} tối đa {doDaiToiDa} ký tự.");
        }

        private static void KiemTraNgay(ImportDong dong, string cot, string tenCot)
        {
            string giaTri = dong.LayChuoi(cot);

            if (giaTri.Length == 0)
                return;

            if (dong.LayNgay(cot) == null)
                ThemLoi(dong, cot, giaTri, $"{tenCot} '{giaTri}' không phải ngày hợp lệ (yyyy-MM-dd hoặc dd/MM/yyyy).");
        }

        private static void KiemTraSo(ImportDong dong, string cot, string tenCot)
        {
            string giaTri = dong.LayChuoi(cot);

            if (giaTri.Length == 0)
                return;

            if (dong.LaySo(cot) == null)
                ThemLoi(dong, cot, giaTri, $"{tenCot} '{giaTri}' không phải số hợp lệ.");
        }

        private static bool ChiGomKyTuMa(string giaTri)
        {
            foreach (char c in giaTri)
            {
                if (!char.IsLetterOrDigit(c) && c != '_' && c != '.' && c != '-')
                    return false;
            }

            return true;
        }

        private static void ThemLoi(ImportDong dong, string cot, string? giaTri, string thongDiep)
        {
            dong.DanhSachLoi.Add(new ImportLoi
            {
                SoDong = dong.SoDong,
                Cot = cot,
                GiaTri = string.IsNullOrWhiteSpace(giaTri) ? null : giaTri,
                ThongDiep = thongDiep
            });
        }

        /// <summary>Định dạng số dùng khi ghi báo cáo lỗi.</summary>
        public static string HienThiSo(decimal giaTri)
        {
            return giaTri.ToString(CultureInfo.InvariantCulture);
        }
    }
}
