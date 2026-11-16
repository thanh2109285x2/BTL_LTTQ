using BTL_LTTQ.Common.Reports;
using BTL_LTTQ.Models.Reports;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BTL_LTTQ.BUS.Reports
{
    /// <summary>
    /// Danh mục nguồn dữ liệu và trường dữ liệu được phép dùng để dựng báo cáo.
    ///
    /// NGUYÊN TẮC AN TOÀN:
    ///  - Mọi nguồn/trường đều được khai báo cứng tại đây (danh sách trắng).
    ///  - Người dùng chỉ chọn theo mã (<c>Ma</c>), không bao giờ nhập SQL.
    ///  - Mệnh đề FROM/JOIN và biểu thức cột là hằng số do hệ thống cung cấp.
    ///  - Giá trị lọc luôn đi qua tham số hóa ở <see cref="ReportQueryBuilder"/>.
    /// </summary>
    public sealed class ReportCatalogService
    {
        private readonly List<DataSourceDefinition> _nguon = new();
        private readonly List<ReportTemplate> _mau = new();

        public ReportCatalogService()
        {
            DungNguon();
            DungMau();
        }

        public IReadOnlyList<DataSourceDefinition> NguonDuLieu => _nguon;

        public IReadOnlyList<ReportTemplate> MauDungSan => _mau;

        public DataSourceDefinition? TimNguon(string ma) =>
            _nguon.FirstOrDefault(n => string.Equals(n.Ma, ma, StringComparison.OrdinalIgnoreCase));

        public DataSourceDefinition YeuCauNguon(string ma) =>
            TimNguon(ma) ?? throw new InvalidOperationException($"Nguồn dữ liệu '{ma}' không tồn tại trong danh mục.");

        public int TongSoTruong => _nguon.Sum(n => n.Truong.Count);

        // ------------------------------------------------------------------ Dữ liệu

        private void Them(DataSourceDefinition nguon) => _nguon.Add(nguon);

        private static FieldDefinition T(string ma, string bieuThuc, string tieuDe, KieuDuLieu kieu,
            string nhom, bool nhomDuoc = false, bool tongHopDuoc = false, bool macDinh = false,
            int doRong = 0, string ghiChu = "")
        {
            return new FieldDefinition
            {
                Ma = ma,
                BieuThuc = bieuThuc,
                TieuDe = tieuDe,
                Kieu = kieu,
                Nhom = nhom,
                CoTheNhom = nhomDuoc,
                CoTheTongHop = tongHopDuoc,
                MacDinh = macDinh,
                DoRong = doRong > 0 ? doRong : DoRongGoiY(kieu, tieuDe),
                CanLeMacDinh = ReportFormatters.CanLeMacDinh(kieu),
                DinhDangMacDinh = ReportFormatters.DinhDangMacDinh(kieu),
                GhiChu = ghiChu
            };
        }

        private static int DoRongGoiY(KieuDuLieu kieu, string tieuDe)
        {
            int theoChu = Math.Max(90, Math.Min(240, tieuDe.Length * 9 + 40));
            return kieu switch
            {
                KieuDuLieu.Ngay => 110,
                KieuDuLieu.Gio => 80,
                KieuDuLieu.Logic => 80,
                KieuDuLieu.SoNguyen => 90,
                KieuDuLieu.SoThuc => 80,
                KieuDuLieu.TienTe => 120,
                KieuDuLieu.PhanTram => 90,
                _ => theoChu
            };
        }

        private void DungNguon()
        {
            Them(NguonHocVien());
            Them(NguonLopHoc());
            Them(NguonMonHoc());
            Them(NguonGiaoVien());
            Them(NguonHocPhi());
            Them(NguonDiem());
            Them(NguonDangKy());
        }

        private static DataSourceDefinition NguonHocVien() => new()
        {
            Ma = "HOCVIEN",
            Ten = "Học viên",
            MoTa = "Hồ sơ học viên kèm lớp, môn học và giáo viên phụ trách.",
            BieuTuong = "🎓",
            Nhom = "Đào tạo",
            AliasChinh = "h",
            KhoaChinh = "MaHocVien",
            PhamVi = PhamViNguon.ChoHocVien,
            FromSql = @"FROM HocVien h
                        LEFT JOIN LopHoc l ON h.MaLop = l.MaLop
                        LEFT JOIN MonHoc m ON l.MaMon = m.MaMon
                        LEFT JOIN GiaoVien g ON l.MaGV = g.MaGV",
            Truong =
            {
                T("MaHocVien", "h.MaHocVien", "Mã học viên", KieuDuLieu.Chuoi, "Định danh", true, false, true, 100),
                T("TenHocVien", "h.TenHocVien", "Tên học viên", KieuDuLieu.Chuoi, "Định danh", true, false, true, 190),
                T("NgaySinh", "h.NgaySinh", "Ngày sinh", KieuDuLieu.Ngay, "Thông tin cá nhân", false, false, false, 110),
                T("GioiTinh", "h.GioiTinh", "Giới tính", KieuDuLieu.Chuoi, "Thông tin cá nhân", true, false, false, 90),
                T("DiaChi", "h.DiaChi", "Địa chỉ", KieuDuLieu.Chuoi, "Thông tin cá nhân", false, false, false, 200),
                T("DienThoai", "h.DienThoai", "Điện thoại", KieuDuLieu.Chuoi, "Thông tin cá nhân", false, false, false, 110),
                T("MaNghe", "h.MaNghe", "Mã nghề", KieuDuLieu.Chuoi, "Thông tin cá nhân", true, false, false, 90),
                T("MaLop", "h.MaLop", "Mã lớp", KieuDuLieu.Chuoi, "Lớp học", true, false, true, 90),
                T("TenLop", "l.TenLop", "Tên lớp", KieuDuLieu.Chuoi, "Lớp học", true, false, true, 150),
                T("TenMon", "m.TenMon", "Môn học", KieuDuLieu.Chuoi, "Lớp học", true, false, true, 140),
                T("TenGV", "g.TenGV", "Giáo viên", KieuDuLieu.Chuoi, "Lớp học", true, false, false, 150),
                T("Diem", "h.Diem", "Điểm", KieuDuLieu.SoThuc, "Kết quả", false, true, false, 80),
                T("MaXepLoai", "h.MaXepLoai", "Mã xếp loại", KieuDuLieu.Chuoi, "Kết quả", true, false, false, 100),
                T("NgayNopHocPhi", "h.NgayNopHocPhi", "Ngày nộp học phí", KieuDuLieu.Ngay, "Học phí", false, false, false, 130)
            }
        };

        private static DataSourceDefinition NguonLopHoc() => new()
        {
            Ma = "LOPHOC",
            Ten = "Lớp học",
            MoTa = "Thông tin lớp học: môn, giáo viên, ca, phòng, sĩ số.",
            BieuTuong = "🏫",
            Nhom = "Đào tạo",
            AliasChinh = "l",
            KhoaChinh = "MaLop",
            PhamVi = PhamViNguon.ChoLopHoc,
            FromSql = @"FROM LopHoc l
                        LEFT JOIN MonHoc m ON l.MaMon = m.MaMon
                        LEFT JOIN GiaoVien g ON l.MaGV = g.MaGV
                        LEFT JOIN CaHoc c ON l.MaCa = c.MaCa
                        LEFT JOIN ThoiGian t ON l.MaThoiGian = t.MaThoiGian
                        LEFT JOIN PhongHoc p ON l.MaPhong = p.MaPhong",
            Truong =
            {
                T("MaLop", "l.MaLop", "Mã lớp", KieuDuLieu.Chuoi, "Định danh", true, false, true, 90),
                T("TenLop", "l.TenLop", "Tên lớp", KieuDuLieu.Chuoi, "Định danh", true, false, true, 180),
                T("TenMon", "m.TenMon", "Môn học", KieuDuLieu.Chuoi, "Nội dung", true, false, true, 150),
                T("TenGV", "g.TenGV", "Giáo viên", KieuDuLieu.Chuoi, "Nội dung", true, false, true, 160),
                T("NgayBD", "l.NgayBD", "Ngày bắt đầu", KieuDuLieu.Ngay, "Thời gian", false, false, false, 120),
                T("NgayKT", "l.NgayKT", "Ngày kết thúc", KieuDuLieu.Ngay, "Thời gian", false, false, false, 120),
                T("TenCa", "c.TenCa", "Ca học", KieuDuLieu.Chuoi, "Thời gian", true, false, false, 100),
                T("TenThoiGian", "t.TenThoiGian", "Thời gian", KieuDuLieu.Chuoi, "Thời gian", true, false, false, 130),
                T("TenPhong", "p.TenPhong", "Phòng học", KieuDuLieu.Chuoi, "Cơ sở vật chất", true, false, false, 110),
                T("SiSo", "(SELECT COUNT(*) FROM DangKy dk WHERE dk.MaLop = l.MaLop)", "Sĩ số hiện tại", KieuDuLieu.SoNguyen, "Sĩ số", false, true, true, 110),
                T("SiSoToiDa", "l.SiSoToiDa", "Sĩ số tối đa", KieuDuLieu.SoNguyen, "Sĩ số", false, true, false, 110),
                T("SoBuoiTuan", "l.SoBuoiTuan", "Số buổi/tuần", KieuDuLieu.SoNguyen, "Sĩ số", false, true, false, 110),
                T("TrangThai", "l.TrangThai", "Trạng thái", KieuDuLieu.Chuoi, "Trạng thái", true, false, false, 120)
            }
        };

        private static DataSourceDefinition NguonMonHoc() => new()
        {
            Ma = "MONHOC",
            Ten = "Môn học",
            MoTa = "Danh mục môn học và học phí niêm yết.",
            BieuTuong = "📚",
            Nhom = "Danh mục",
            AliasChinh = "m",
            KhoaChinh = "MaMon",
            PhamVi = PhamViNguon.Khong,
            FromSql = "FROM MonHoc m",
            Truong =
            {
                T("MaMon", "m.MaMon", "Mã môn", KieuDuLieu.Chuoi, "Định danh", true, false, true, 100),
                T("TenMon", "m.TenMon", "Tên môn học", KieuDuLieu.Chuoi, "Định danh", true, false, true, 200),
                T("HocPhi", "m.HocPhi", "Học phí", KieuDuLieu.TienTe, "Học phí", false, true, true, 130),
                T("SoLop", "(SELECT COUNT(*) FROM LopHoc l WHERE l.MaMon = m.MaMon)", "Số lớp", KieuDuLieu.SoNguyen, "Thống kê", false, true, false, 90)
            }
        };

        private static DataSourceDefinition NguonGiaoVien() => new()
        {
            Ma = "GIAOVIEN",
            Ten = "Giáo viên",
            MoTa = "Hồ sơ giáo viên, trình độ, chuyên môn.",
            BieuTuong = "👩‍🏫",
            Nhom = "Danh mục",
            AliasChinh = "g",
            KhoaChinh = "MaGV",
            PhamVi = PhamViNguon.Khong,
            FromSql = @"FROM GiaoVien g
                        LEFT JOIN TrinhDo t ON g.MaTrinhDo = t.MaTrinhDo
                        LEFT JOIN ChuyenMon c ON g.MaChuyenMon = c.MaChuyenMon",
            Truong =
            {
                T("MaGV", "g.MaGV", "Mã giáo viên", KieuDuLieu.Chuoi, "Định danh", true, false, true, 110),
                T("TenGV", "g.TenGV", "Tên giáo viên", KieuDuLieu.Chuoi, "Định danh", true, false, true, 190),
                T("NgaySinh", "g.NgaySinh", "Ngày sinh", KieuDuLieu.Ngay, "Thông tin cá nhân", false, false, false, 110),
                T("GioiTinh", "g.GioiTinh", "Giới tính", KieuDuLieu.Chuoi, "Thông tin cá nhân", true, false, false, 90),
                T("DiaChi", "g.DiaChi", "Địa chỉ", KieuDuLieu.Chuoi, "Thông tin cá nhân", false, false, false, 200),
                T("DienThoai", "g.DienThoai", "Điện thoại", KieuDuLieu.Chuoi, "Thông tin cá nhân", false, false, false, 110),
                T("TenTrinhDo", "t.TenTrinhDo", "Trình độ", KieuDuLieu.Chuoi, "Chuyên môn", true, false, false, 150),
                T("TenChuyenMon", "c.TenChuyenMon", "Chuyên môn", KieuDuLieu.Chuoi, "Chuyên môn", true, false, false, 150),
                T("MaChucDanh", "g.MaChucDanh", "Mã chức danh", KieuDuLieu.Chuoi, "Chuyên môn", true, false, false, 110),
                T("SoLop", "(SELECT COUNT(*) FROM LopHoc l WHERE l.MaGV = g.MaGV)", "Số lớp phụ trách", KieuDuLieu.SoNguyen, "Thống kê", false, true, false, 130)
            }
        };

        private static DataSourceDefinition NguonHocPhi() => new()
        {
            Ma = "HOCPHI",
            Ten = "Học phí",
            MoTa = "Tình trạng nộp học phí của học viên theo lớp và môn.",
            BieuTuong = "💰",
            Nhom = "Tài chính",
            AliasChinh = "h",
            KhoaChinh = "MaHocVien",
            PhamVi = PhamViNguon.ChoHocVien,
            FromSql = @"FROM HocVien h
                        JOIN LopHoc l ON h.MaLop = l.MaLop
                        LEFT JOIN MonHoc m ON l.MaMon = m.MaMon",
            Truong =
            {
                T("MaHocVien", "h.MaHocVien", "Mã học viên", KieuDuLieu.Chuoi, "Định danh", true, false, true, 100),
                T("TenHocVien", "h.TenHocVien", "Tên học viên", KieuDuLieu.Chuoi, "Định danh", true, false, true, 190),
                T("TenLop", "l.TenLop", "Tên lớp", KieuDuLieu.Chuoi, "Lớp học", true, false, true, 160),
                T("TenMon", "m.TenMon", "Môn học", KieuDuLieu.Chuoi, "Lớp học", true, false, true, 150),
                T("HocPhi", "m.HocPhi", "Học phí", KieuDuLieu.TienTe, "Học phí", false, true, true, 130),
                T("NgayNopHocPhi", "h.NgayNopHocPhi", "Ngày nộp", KieuDuLieu.Ngay, "Học phí", false, false, true, 120),
                T("TrangThaiThanhToan",
                  "CASE WHEN h.NgayNopHocPhi IS NULL THEN N'Chưa nộp' ELSE N'Đã nộp' END",
                  "Trạng thái nộp", KieuDuLieu.Chuoi, "Học phí", true, false, true, 130)
            }
        };

        private static DataSourceDefinition NguonDiem() => new()
        {
            Ma = "DIEM",
            Ten = "Điểm số",
            MoTa = "Điểm và kết quả học tập của học viên theo lớp, môn.",
            BieuTuong = "📝",
            Nhom = "Đào tạo",
            AliasChinh = "h",
            KhoaChinh = "MaHocVien",
            PhamVi = PhamViNguon.ChoHocVien,
            FromSql = @"FROM HocVien h
                        LEFT JOIN LopHoc l ON h.MaLop = l.MaLop
                        LEFT JOIN MonHoc m ON l.MaMon = m.MaMon",
            Truong =
            {
                T("MaHocVien", "h.MaHocVien", "Mã học viên", KieuDuLieu.Chuoi, "Định danh", true, false, true, 100),
                T("TenHocVien", "h.TenHocVien", "Tên học viên", KieuDuLieu.Chuoi, "Định danh", true, false, true, 190),
                T("TenLop", "l.TenLop", "Tên lớp", KieuDuLieu.Chuoi, "Lớp học", true, false, true, 160),
                T("TenMon", "m.TenMon", "Môn học", KieuDuLieu.Chuoi, "Lớp học", true, false, true, 150),
                T("Diem", "h.Diem", "Điểm", KieuDuLieu.SoThuc, "Kết quả", false, true, true, 80),
                T("MaXepLoai", "h.MaXepLoai", "Mã xếp loại", KieuDuLieu.Chuoi, "Kết quả", true, false, false, 110),
                T("KetQua",
                  "CASE WHEN h.Diem IS NULL THEN N'Chưa có điểm' WHEN h.Diem >= 5 THEN N'Đạt' ELSE N'Chưa đạt' END",
                  "Kết quả", KieuDuLieu.Chuoi, "Kết quả", true, false, true, 130)
            }
        };

        private static DataSourceDefinition NguonDangKy() => new()
        {
            Ma = "DANGKY",
            Ten = "Đăng ký lớp",
            MoTa = "Danh sách học viên đăng ký lớp kèm ngày đăng ký.",
            BieuTuong = "🗂️",
            Nhom = "Đào tạo",
            AliasChinh = "d",
            KhoaChinh = "MaLop",
            PhamVi = PhamViNguon.ChoHocVien,
            FromSql = @"FROM DangKy d
                        JOIN HocVien h ON d.MaHocVien = h.MaHocVien
                        LEFT JOIN LopHoc l ON d.MaLop = l.MaLop
                        LEFT JOIN MonHoc m ON l.MaMon = m.MaMon",
            Truong =
            {
                T("MaHocVien", "d.MaHocVien", "Mã học viên", KieuDuLieu.Chuoi, "Định danh", true, false, true, 100),
                T("TenHocVien", "h.TenHocVien", "Tên học viên", KieuDuLieu.Chuoi, "Định danh", true, false, true, 190),
                T("MaLop", "d.MaLop", "Mã lớp", KieuDuLieu.Chuoi, "Lớp học", true, false, true, 90),
                T("TenLop", "l.TenLop", "Tên lớp", KieuDuLieu.Chuoi, "Lớp học", true, false, true, 160),
                T("TenMon", "m.TenMon", "Môn học", KieuDuLieu.Chuoi, "Lớp học", true, false, true, 150),
                T("NgayDangKy", "d.NgayDangKy", "Ngày đăng ký", KieuDuLieu.Ngay, "Thời gian", false, false, true, 120)
            }
        };

        // ------------------------------------------------------------------ Mẫu

        private void DungMau()
        {
            _mau.Add(new ReportTemplate
            {
                Ma = "MAU_HOCVIEN",
                Ten = "Danh sách học viên",
                MoTa = "Danh sách học viên theo lớp, có tổng số lượng.",
                BieuTuong = "🎓",
                MaNguon = "HOCVIEN",
                Nhom = "Phổ biến",
                Tao = () => TaoMau("HOCVIEN",
                    new[] { "MaHocVien", "TenHocVien", "GioiTinh", "MaLop", "TenLop", "TenMon", "DienThoai" },
                    new[] { "TenLop" })
            });

            _mau.Add(new ReportTemplate
            {
                Ma = "MAU_DIEM",
                Ten = "Điểm học viên theo lớp",
                MoTa = "Bảng điểm kèm xếp loại và tổng hợp điểm trung bình.",
                BieuTuong = "📝",
                MaNguon = "DIEM",
                Nhom = "Phổ biến",
                Tao = () => TaoMau("DIEM",
                    new[] { "MaHocVien", "TenHocVien", "TenLop", "TenMon", "Diem", "KetQua" },
                    new[] { "TenLop" },
                    new[] { ("Diem", HamTongHop.TrungBinh, PhamViTongHop.ToanBaoCao),
                            ("Diem", HamTongHop.LonNhat, PhamViTongHop.ToanBaoCao) })
            });

            _mau.Add(new ReportTemplate
            {
                Ma = "MAU_DOANHTHU",
                Ten = "Học phí theo môn",
                MoTa = "Tổng học phí dự kiến theo từng môn học.",
                BieuTuong = "💰",
                MaNguon = "HOCPHI",
                Nhom = "Tài chính",
                Tao = () => TaoMau("HOCPHI",
                    new[] { "TenMon", "TrangThaiThanhToan", "HocPhi", "NgayNopHocPhi" },
                    new[] { "TenMon" },
                    new[] { ("HocPhi", HamTongHop.Tong, PhamViTongHop.ToanBaoCao),
                            ("MaHocVien", HamTongHop.Dem, PhamViTongHop.ToanBaoCao) })
            });

            _mau.Add(new ReportTemplate
            {
                Ma = "MAU_SISO",
                Ten = "Sĩ số lớp học",
                MoTa = "Sĩ số hiện tại / tối đa của từng lớp.",
                BieuTuong = "🏫",
                MaNguon = "LOPHOC",
                Nhom = "Vận hành",
                Tao = () => TaoMau("LOPHOC",
                    new[] { "MaLop", "TenLop", "TenMon", "TenGV", "TenPhong", "SiSo", "SiSoToiDa" },
                    new[] { "TenMon" },
                    new[] { ("SiSo", HamTongHop.Tong, PhamViTongHop.ToanBaoCao) })
            });

            _mau.Add(new ReportTemplate
            {
                Ma = "MAU_GIAOVIEN",
                Ten = "Danh sách giáo viên",
                MoTa = "Giáo viên kèm trình độ, chuyên môn và số lớp phụ trách.",
                BieuTuong = "👩‍🏫",
                MaNguon = "GIAOVIEN",
                Nhom = "Danh mục",
                Tao = () => TaoMau("GIAOVIEN",
                    new[] { "MaGV", "TenGV", "GioiTinh", "DienThoai", "TenTrinhDo", "TenChuyenMon", "SoLop" },
                    Array.Empty<string>())
            });

            _mau.Add(new ReportTemplate
            {
                Ma = "MAU_DANGKY",
                Ten = "Đăng ký lớp theo lớp học",
                MoTa = "Số lượt đăng ký theo lớp và môn.",
                BieuTuong = "🗂️",
                MaNguon = "DANGKY",
                Nhom = "Vận hành",
                Tao = () => TaoMau("DANGKY",
                    new[] { "TenHocVien", "MaLop", "TenLop", "TenMon", "NgayDangKy" },
                    new[] { "TenLop" },
                    new[] { ("MaHocVien", HamTongHop.Dem, PhamViTongHop.ToanBaoCao) })
            });
        }

        private static ReportDefinition TaoMau(string maNguon, string[] cot,
            string[] nhom,
            (string Truong, HamTongHop Ham, PhamViTongHop PhamVi)[]? tongHop = null)
        {
            var dinhNghia = new ReportDefinition
            {
                MaNguon = maNguon,
                TieuDe = "",
                BoCuc = new ReportLayoutDef()
            };

            int thuTu = 0;
            foreach (string ma in cot)
            {
                dinhNghia.Cot.Add(new ReportColumnDef
                {
                    MaTruong = ma,
                    HienThi = true,
                    ThuTu = thuTu++,
                    CanLe = CanLeCot.Trai,
                    DoRong = 120
                });
            }

            foreach (string ma in nhom)
                dinhNghia.Nhom.Add(new ReportGroupDef { MaTruong = ma });

            if (tongHop != null)
            {
                foreach (var th in tongHop)
                {
                    dinhNghia.TongHop.Add(new ReportSummaryDef
                    {
                        MaTruong = th.Truong,
                        Ham = th.Ham,
                        PhamVi = th.PhamVi,
                        HienThi = true
                    });
                }
            }

            return dinhNghia;
        }
    }
}
