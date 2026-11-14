using BTL_LTTQ.Common;
using BTL_LTTQ.DataAccessLayer;
using BTL_LTTQ.Models.Import;
using BTL_LTTQ.Repositories.Import;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace BTL_LTTQ.Services.Import
{
    /// <summary>
    /// Điều phối đọc tệp, kiểm tra và ghi dữ liệu hàng loạt.
    /// Ghi theo nguyên tắc tất cả hoặc không: mọi dòng nằm trong một transaction,
    /// có bất kỳ lỗi nào thì rollback toàn bộ.
    /// </summary>
    public class ImportService
    {
        private static readonly string[] _trangThaiChoDangKy = { "SapKhaiGiang", "DangHoc" };

        public KetQuaPreview XemTruoc(string duongDan, ImportLoai loai)
        {
            var preview = new KetQuaPreview
            {
                Loai = loai,
                TenFile = string.IsNullOrWhiteSpace(duongDan) ? "" : Path.GetFileName(duongDan)
            };

            try
            {
                KetQuaDocTep doc = BangTinhReader.Doc(duongDan, loai);
                preview.LoiFile.AddRange(doc.LoiFile);
                preview.CotBatBuocThieu.AddRange(doc.CotBatBuocThieu);
                preview.CotKhongNhanDien.AddRange(doc.CotKhongNhanDien);
                preview.DanhSachDong.AddRange(doc.DanhSachDong);

                if (preview.LoiFile.Count > 0 || preview.CotBatBuocThieu.Count > 0)
                    return preview;

                var lookup = new ImportLookupRepository();
                lookup.Nap();
                new ImportValidator(lookup).KiemTra(preview);
            }
            catch (Exception ex)
            {
                preview.LoiFile.Add("Không kiểm tra được dữ liệu: " + ex.Message);
            }

            return preview;
        }

        public KetQuaImport ThucHien(KetQuaPreview preview)
        {
            ArgumentNullException.ThrowIfNull(preview);

            var ketQua = new KetQuaImport
            {
                OperationId = preview.OperationId,
                Loai = preview.Loai
            };

            if (!preview.CoTheImport)
            {
                ketQua.ThanhCong = false;
                ketQua.ThongDiep = "Dữ liệu chưa hợp lệ, không có dòng nào được ghi.";
                ketQua.DanhSachLoi.AddRange(preview.TatCaLoi);
                ketQua.DanhSachLoi.AddRange(preview.LoiFile.Select(m => new ImportLoi
                {
                    SoDong = 0,
                    ThongDiep = m
                }));
                return ketQua;
            }

            var lopBiAnhHuong = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            try
            {
                KiemTraQuyen(preview.Loai);

                DbHelper.ExecuteTransaction((conn, tran) =>
                {
                    var loi = new List<ImportLoi>();
                    HashSet<string> lopThayDoi = KiemTraLai(conn, tran, preview, loi);

                    if (loi.Count > 0)
                        throw new ImportValidationException(loi);

                    int soDong = preview.Loai switch
                    {
                        ImportLoai.HocVien => GhiHocVien(conn, tran, preview.DanhSachDong),
                        ImportLoai.GiaoVien => GhiGiaoVien(conn, tran, preview.DanhSachDong),
                        ImportLoai.DangKy => GhiDangKy(conn, tran, preview.DanhSachDong),
                        _ => 0
                    };

                    ketQua.SoDongDaThem = soDong;
                    lopBiAnhHuong = lopThayDoi;
                });

                ketQua.ThanhCong = true;
                ketQua.LopBiAnhHuong.AddRange(lopBiAnhHuong.OrderBy(x => x, StringComparer.OrdinalIgnoreCase));
                ketQua.ThongDiep = $"Đã nhập {ketQua.SoDongDaThem} dòng {ImportLoaiMoTa.TenHienThi(preview.Loai).ToLowerInvariant()}.";
            }
            catch (UnauthorizedAccessException ex)
            {
                ketQua.ThanhCong = false;
                ketQua.SoDongDaThem = 0;
                ketQua.ThongDiep = ex.Message;
            }
            catch (ImportValidationException ex)
            {
                ketQua.ThanhCong = false;
                ketQua.SoDongDaThem = 0;
                ketQua.DanhSachLoi.AddRange(ex.Loi);
                ketQua.ThongDiep = "Dữ liệu thay đổi trong lúc chờ xác nhận. Không có dòng nào được ghi.";
            }
            catch (SqlException ex) when (ex.Number is 2627 or 2601)
            {
                ketQua.ThanhCong = false;
                ketQua.SoDongDaThem = 0;
                ketQua.ThongDiep = "Dữ liệu đã tồn tại hoặc bị trùng khóa. Không có dòng nào được ghi.";
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                ketQua.ThanhCong = false;
                ketQua.SoDongDaThem = 0;
                ketQua.ThongDiep = "Dữ liệu tham chiếu không hợp lệ. Không có dòng nào được ghi.";
            }
            catch (Exception ex)
            {
                ketQua.ThanhCong = false;
                ketQua.SoDongDaThem = 0;
                ketQua.ThongDiep = "Không nhập được dữ liệu: " + ex.Message;
            }

            return ketQua;
        }

        private static void KiemTraQuyen(ImportLoai loai)
        {
            // Lớp bảo vệ cuối: học viên không được nhập dữ liệu hàng loạt.
            if (CurrentUser.IsHocVien)
                throw new UnauthorizedAccessException("Học viên không có quyền nhập dữ liệu hàng loạt.");

            switch (loai)
            {
                case ImportLoai.HocVien:
                    PermissionService.YeuCau(ChucNangConst.HOCVIEN_THEM,
                        "Bạn không có quyền thêm học viên.");
                    break;
                case ImportLoai.GiaoVien:
                    PermissionService.YeuCau(ChucNangConst.GIAOVIEN_THEM,
                        "Bạn không có quyền thêm giáo viên.");
                    break;
                case ImportLoai.DangKy:
                    PermissionService.YeuCau(ChucNangConst.DANGKYLOP,
                        "Bạn không có quyền đăng ký lớp.");
                    break;
            }
        }

        /// <summary>
        /// Kiểm tra lại toàn bộ ràng buộc ngay trước khi ghi, trong cùng transaction.
        /// Với đăng ký, khóa dòng lớp để hai phiên không cùng vượt sĩ số.
        /// </summary>
        private static HashSet<string> KiemTraLai(SqlConnection conn, SqlTransaction tran,
            KetQuaPreview preview, List<ImportLoi> loi)
        {
            var lopThayDoi = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            switch (preview.Loai)
            {
                case ImportLoai.HocVien:
                    KiemTraLaiHocVien(conn, tran, preview.DanhSachDong, loi);
                    break;
                case ImportLoai.GiaoVien:
                    KiemTraLaiGiaoVien(conn, tran, preview.DanhSachDong, loi);
                    break;
                case ImportLoai.DangKy:
                    KiemTraLaiDangKy(conn, tran, preview.DanhSachDong, loi, lopThayDoi);
                    break;
            }

            return lopThayDoi;
        }

        private static void KiemTraLaiHocVien(SqlConnection conn, SqlTransaction tran,
            List<ImportDong> dongList, List<ImportLoi> loi)
        {
            var daGap = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (ImportDong dong in dongList)
            {
                string ma = dong.LayChuoi("MaHocVien");

                if (!daGap.Add(ma))
                    ThemLoi(loi, dong, "MaHocVien", ma, "Mã học viên bị trùng trong tệp.");
                else if (DaTonTai(conn, tran, "HocVien", "MaHocVien", ma))
                    ThemLoi(loi, dong, "MaHocVien", ma, $"Mã học viên '{ma}' đã tồn tại trong hệ thống.");

                KiemTraThamChieuTrongDb(conn, tran, dong, "MaNghe", "NgheNghiep", "MaNghe", loi);
                KiemTraThamChieuTrongDb(conn, tran, dong, "MaXepLoai", "XepLoai", "MaXepLoai", loi);
                KiemTraThamChieuTrongDb(conn, tran, dong, "MaLop", "LopHoc", "MaLop", loi);
            }
        }

        private static void KiemTraLaiGiaoVien(SqlConnection conn, SqlTransaction tran,
            List<ImportDong> dongList, List<ImportLoi> loi)
        {
            var daGap = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (ImportDong dong in dongList)
            {
                string ma = dong.LayChuoi("MaGV");

                if (!daGap.Add(ma))
                    ThemLoi(loi, dong, "MaGV", ma, "Mã giáo viên bị trùng trong tệp.");
                else if (DaTonTai(conn, tran, "GiaoVien", "MaGV", ma))
                    ThemLoi(loi, dong, "MaGV", ma, $"Mã giáo viên '{ma}' đã tồn tại trong hệ thống.");

                KiemTraThamChieuTrongDb(conn, tran, dong, "MaChucDanh", "ChucDanh", "MaChucDanh", loi);
                KiemTraThamChieuTrongDb(conn, tran, dong, "MaChuyenMon", "ChuyenMon", "MaChuyenMon", loi);
                KiemTraThamChieuTrongDb(conn, tran, dong, "MaCQ", "CoQuan", "MaCQ", loi);
                KiemTraThamChieuTrongDb(conn, tran, dong, "MaTrinhDo", "TrinhDo", "MaTrinhDo", loi);
            }
        }

        private static void KiemTraLaiDangKy(SqlConnection conn, SqlTransaction tran,
            List<ImportDong> dongList, List<ImportLoi> loi, HashSet<string> lopThayDoi)
        {
            var soDongTheoLop = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            var daGap = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (ImportDong dong in dongList)
            {
                string maHocVien = dong.LayChuoi("MaHocVien");
                string maLop = dong.LayChuoi("MaLop");
                string khoa = ImportLookupRepository.TaoKhoaCap(maHocVien, maLop);

                if (!daGap.Add(khoa))
                    ThemLoi(loi, dong, "MaLop", maLop,
                        $"Học viên '{maHocVien}' đăng ký lớp '{maLop}' bị trùng trong tệp.");

                if (!DaTonTai(conn, tran, "HocVien", "MaHocVien", maHocVien))
                    ThemLoi(loi, dong, "MaHocVien", maHocVien,
                        $"Mã học viên '{maHocVien}' không tồn tại trong hệ thống.");

                if (DaTonTaiCapDangKy(conn, tran, maHocVien, maLop))
                    ThemLoi(loi, dong, "MaLop", maLop,
                        $"Học viên '{maHocVien}' đã đăng ký lớp '{maLop}' trong hệ thống.");

                if (soDongTheoLop.TryGetValue(maLop, out int soDong))
                    soDongTheoLop[maLop] = soDong + 1;
                else
                    soDongTheoLop[maLop] = 1;
            }

            // Khóa từng lớp theo thứ tự cố định để tránh deadlock giữa các phiên nhập.
            foreach (string maLop in soDongTheoLop.Keys.OrderBy(x => x, StringComparer.OrdinalIgnoreCase))
            {
                LopThongTinImport? lop = ImportLookupRepository.DocLopDangKhoa(conn, tran, maLop);

                if (lop == null)
                {
                    foreach (ImportDong dong in dongList.Where(d =>
                                 string.Equals(d.LayChuoi("MaLop"), maLop, StringComparison.OrdinalIgnoreCase)))
                    {
                        ThemLoi(loi, dong, "MaLop", maLop, $"Mã lớp '{maLop}' không tồn tại trong hệ thống.");
                    }

                    continue;
                }

                int soDongTrongTep = soDongTheoLop[maLop];
                int conLai = lop.SiSoToiDa - lop.SiSoHienTai;

                if (Array.IndexOf(_trangThaiChoDangKy, lop.TrangThai) < 0)
                {
                    foreach (ImportDong dong in dongList.Where(d =>
                                 string.Equals(d.LayChuoi("MaLop"), maLop, StringComparison.OrdinalIgnoreCase)))
                    {
                        ThemLoi(loi, dong, "MaLop", maLop,
                            $"Lớp '{maLop}' đang ở trạng thái '{lop.TrangThai}', không nhận đăng ký.");
                    }
                }

                if (soDongTrongTep > conLai)
                {
                    string thongDiep = conLai <= 0
                        ? $"Lớp '{maLop}' đã đủ sĩ số ({lop.SiSoHienTai}/{lop.SiSoToiDa})."
                        : $"Lớp '{maLop}' chỉ còn nhận {conLai} học viên nhưng tệp có {soDongTrongTep} đăng ký.";

                    foreach (ImportDong dong in dongList.Where(d =>
                                 string.Equals(d.LayChuoi("MaLop"), maLop, StringComparison.OrdinalIgnoreCase)))
                    {
                        ThemLoi(loi, dong, "MaLop", maLop, thongDiep);
                    }
                }

                lopThayDoi.Add(maLop);
            }
        }

        private static int GhiHocVien(SqlConnection conn, SqlTransaction tran, List<ImportDong> dongList)
        {
            const string sql = @"
                INSERT INTO dbo.HocVien
                    (MaHocVien, TenHocVien, MaLop, NgaySinh, GioiTinh, DiaChi,
                     MaNghe, DienThoai, NgayNopHocPhi, Diem, MaXepLoai)
                VALUES
                    (@MaHocVien, @TenHocVien, @MaLop, @NgaySinh, @GioiTinh, @DiaChi,
                     @MaNghe, @DienThoai, @NgayNopHocPhi, @Diem, @MaXepLoai)";

            foreach (ImportDong dong in dongList)
            {
                using var cmd = new SqlCommand(sql, conn, tran);

                cmd.Parameters.Add(new SqlParameter("@MaHocVien", SqlDbType.VarChar, 20) { Value = dong.LayChuoi("MaHocVien") });
                cmd.Parameters.Add(new SqlParameter("@TenHocVien", SqlDbType.NVarChar, 100) { Value = dong.LayChuoi("TenHocVien") });
                cmd.Parameters.Add(GiaTriHoacNull("@MaLop", SqlDbType.VarChar, 20, dong.LayChuoi("MaLop")));
                cmd.Parameters.Add(new SqlParameter("@NgaySinh", SqlDbType.Date) { Value = (object?)dong.LayNgay("NgaySinh") ?? DBNull.Value });
                cmd.Parameters.Add(GiaTriHoacNull("@GioiTinh", SqlDbType.NVarChar, 10, dong.LayChuoi("GioiTinh")));
                cmd.Parameters.Add(GiaTriHoacNull("@DiaChi", SqlDbType.NVarChar, 200, dong.LayChuoi("DiaChi")));
                cmd.Parameters.Add(GiaTriHoacNull("@MaNghe", SqlDbType.VarChar, 20, dong.LayChuoi("MaNghe")));
                cmd.Parameters.Add(GiaTriHoacNull("@DienThoai", SqlDbType.VarChar, 20, dong.LayChuoi("DienThoai")));
                cmd.Parameters.Add(new SqlParameter("@NgayNopHocPhi", SqlDbType.Date) { Value = (object?)dong.LayNgay("NgayNopHocPhi") ?? DBNull.Value });
                cmd.Parameters.Add(new SqlParameter("@Diem", SqlDbType.Float) { Value = (object?)dong.LaySo("Diem") is decimal diem ? (double)diem : DBNull.Value });
                cmd.Parameters.Add(GiaTriHoacNull("@MaXepLoai", SqlDbType.VarChar, 20, dong.LayChuoi("MaXepLoai")));

                cmd.ExecuteNonQuery();
            }

            return dongList.Count;
        }

        private static int GhiGiaoVien(SqlConnection conn, SqlTransaction tran, List<ImportDong> dongList)
        {
            const string sql = @"
                INSERT INTO dbo.GiaoVien
                    (MaGV, TenGV, NgaySinh, GioiTinh, DiaChi, DienThoai,
                     MaChucDanh, MaChuyenMon, MaCQ, MaTrinhDo, Anh)
                VALUES
                    (@MaGV, @TenGV, @NgaySinh, @GioiTinh, @DiaChi, @DienThoai,
                     @MaChucDanh, @MaChuyenMon, @MaCQ, @MaTrinhDo, @Anh)";

            foreach (ImportDong dong in dongList)
            {
                using var cmd = new SqlCommand(sql, conn, tran);

                cmd.Parameters.Add(new SqlParameter("@MaGV", SqlDbType.VarChar, 20) { Value = dong.LayChuoi("MaGV") });
                cmd.Parameters.Add(new SqlParameter("@TenGV", SqlDbType.NVarChar, 100) { Value = dong.LayChuoi("TenGV") });
                cmd.Parameters.Add(new SqlParameter("@NgaySinh", SqlDbType.Date) { Value = (object?)dong.LayNgay("NgaySinh") ?? DBNull.Value });
                cmd.Parameters.Add(GiaTriHoacNull("@GioiTinh", SqlDbType.NVarChar, 10, dong.LayChuoi("GioiTinh")));
                cmd.Parameters.Add(GiaTriHoacNull("@DiaChi", SqlDbType.NVarChar, 200, dong.LayChuoi("DiaChi")));
                cmd.Parameters.Add(GiaTriHoacNull("@DienThoai", SqlDbType.VarChar, 20, dong.LayChuoi("DienThoai")));
                cmd.Parameters.Add(GiaTriHoacNull("@MaChucDanh", SqlDbType.VarChar, 20, dong.LayChuoi("MaChucDanh")));
                cmd.Parameters.Add(GiaTriHoacNull("@MaChuyenMon", SqlDbType.VarChar, 20, dong.LayChuoi("MaChuyenMon")));
                cmd.Parameters.Add(GiaTriHoacNull("@MaCQ", SqlDbType.VarChar, 20, dong.LayChuoi("MaCQ")));
                cmd.Parameters.Add(GiaTriHoacNull("@MaTrinhDo", SqlDbType.VarChar, 20, dong.LayChuoi("MaTrinhDo")));
                cmd.Parameters.Add(GiaTriHoacNull("@Anh", SqlDbType.NVarChar, 255, dong.LayChuoi("Anh")));

                cmd.ExecuteNonQuery();
            }

            return dongList.Count;
        }

        private static int GhiDangKy(SqlConnection conn, SqlTransaction tran, List<ImportDong> dongList)
        {
            foreach (ImportDong dong in dongList)
            {
                DateTime? ngay = dong.LayNgay("NgayDangKy");

                string sql = ngay.HasValue
                    ? "INSERT INTO dbo.DangKy (MaHocVien, MaLop, NgayDangKy) VALUES (@MaHocVien, @MaLop, @NgayDangKy)"
                    : "INSERT INTO dbo.DangKy (MaHocVien, MaLop) VALUES (@MaHocVien, @MaLop)";

                using var cmd = new SqlCommand(sql, conn, tran);
                cmd.Parameters.Add(new SqlParameter("@MaHocVien", SqlDbType.VarChar, 20) { Value = dong.LayChuoi("MaHocVien") });
                cmd.Parameters.Add(new SqlParameter("@MaLop", SqlDbType.VarChar, 20) { Value = dong.LayChuoi("MaLop") });

                if (ngay.HasValue)
                    cmd.Parameters.Add(new SqlParameter("@NgayDangKy", SqlDbType.Date) { Value = ngay.Value });

                cmd.ExecuteNonQuery();
            }

            return dongList.Count;
        }

        private static bool DaTonTai(SqlConnection conn, SqlTransaction tran, string bang, string cot, string giaTri)
        {
            using var cmd = new SqlCommand(
                $"SELECT TOP 1 1 FROM dbo.{bang} WITH (HOLDLOCK) WHERE {cot} = @GiaTri", conn, tran);
            cmd.Parameters.Add(new SqlParameter("@GiaTri", SqlDbType.VarChar, 20) { Value = giaTri });
            object? kq = cmd.ExecuteScalar();
            return kq != null && kq != DBNull.Value;
        }

        private static bool DaTonTaiCapDangKy(SqlConnection conn, SqlTransaction tran,
            string maHocVien, string maLop)
        {
            using var cmd = new SqlCommand(@"
                SELECT TOP 1 1 FROM dbo.DangKy WITH (UPDLOCK, HOLDLOCK)
                WHERE MaHocVien = @MaHocVien AND MaLop = @MaLop", conn, tran);
            cmd.Parameters.Add(new SqlParameter("@MaHocVien", SqlDbType.VarChar, 20) { Value = maHocVien });
            cmd.Parameters.Add(new SqlParameter("@MaLop", SqlDbType.VarChar, 20) { Value = maLop });
            object? kq = cmd.ExecuteScalar();
            return kq != null && kq != DBNull.Value;
        }

        private static void KiemTraThamChieuTrongDb(SqlConnection conn, SqlTransaction tran,
            ImportDong dong, string cot, string bang, string cotBang, List<ImportLoi> loi)
        {
            string giaTri = dong.LayChuoi(cot);

            if (giaTri.Length == 0)
                return;

            if (!ImportLookupRepository.TonTai(conn, tran, bang, cotBang, giaTri))
                ThemLoi(loi, dong, cot, giaTri, $"Mã '{giaTri}' không tồn tại trong {bang}.");
        }

        private static SqlParameter GiaTriHoacNull(string ten, SqlDbType kieu, int doDai, string? giaTri)
        {
            return string.IsNullOrWhiteSpace(giaTri)
                ? new SqlParameter(ten, kieu, doDai) { Value = DBNull.Value }
                : new SqlParameter(ten, kieu, doDai) { Value = giaTri.Trim() };
        }

        private static void ThemLoi(List<ImportLoi> loi, ImportDong dong, string cot, string? giaTri, string thongDiep)
        {
            loi.Add(new ImportLoi
            {
                SoDong = dong.SoDong,
                Cot = cot,
                GiaTri = giaTri,
                ThongDiep = thongDiep
            });
        }

        private sealed class ImportValidationException : Exception
        {
            public ImportValidationException(List<ImportLoi> loi)
                : base("Dữ liệu không còn hợp lệ tại thời điểm ghi.")
            {
                Loi = loi;
            }

            public List<ImportLoi> Loi { get; }
        }
    }
}
