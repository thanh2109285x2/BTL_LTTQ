using BTL_LTTQ.Common.Reports;
using BTL_LTTQ.DAL;
using BTL_LTTQ.Models;
using BTL_LTTQ.Models.Reports;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BTL_LTTQ.BUS.Reports
{
    /// <summary>Lỗi khi dựng truy vấn báo cáo (cấu hình chưa hợp lệ).</summary>
    public sealed class ReportBuildException : Exception
    {
        public ReportBuildException(string message) : base(message) { }
    }

    /// <summary>
    /// Dựng câu truy vấn SELECT an toàn từ một <see cref="ReportDefinition"/>.
    ///
    /// AN TOÀN:
    ///  - Cột/nguồn lấy từ danh sách trắng (<see cref="ReportCatalogService"/>), không nối chuỗi người dùng.
    ///  - Mọi giá trị lọc đi qua tham số <see cref="SqlParameter"/>.
    ///  - Có TOP giới hạn số dòng để tránh truy vấn quá nặng.
    /// </summary>
    public sealed class ReportQueryBuilder
    {
        public sealed class KetQua
        {
            public string Sql { get; set; } = "";
            public SqlParameter[] ThamSo { get; set; } = Array.Empty<SqlParameter>();
            public List<string> CotHienThi { get; set; } = new();
            public List<string> CotPhu { get; set; } = new();
            public bool ChamGioiHanDuKien { get; set; }
        }

        private const string Collation = "Vietnamese_CI_AI";

        public KetQua Dung(ReportDefinition dinhNghia, DataSourceDefinition nguon, PhamViInfo? phamVi)
        {
            ArgumentNullException.ThrowIfNull(dinhNghia);
            ArgumentNullException.ThrowIfNull(nguon);

            var cotHienThi = dinhNghia.CotHienThi.ToList();
            if (cotHienThi.Count == 0)
                throw new ReportBuildException("Báo cáo cần có ít nhất một cột hiển thị.");

            // Kiểm tra mọi trường tham chiếu đều nằm trong danh sách trắng.
            foreach (var cot in cotHienThi)
                YeuCauTruong(nguon, cot.MaTruong, "cột hiển thị");

            var dsPhu = new List<string>();
            foreach (var nhom in dinhNghia.Nhom)
            {
                YeuCauTruong(nguon, nhom.MaTruong, "nhóm");
                if (!cotHienThi.Any(c => Trung(c.MaTruong, nhom.MaTruong)) && !dsPhu.Any(x => Trung(x, nhom.MaTruong)))
                    dsPhu.Add(nhom.MaTruong);
            }

            foreach (var th in dinhNghia.TongHop.Where(t => t.HienThi && t.Ham != HamTongHop.Khong))
            {
                YeuCauTruong(nguon, th.MaTruong, "tổng hợp");
                if (!cotHienThi.Any(c => Trung(c.MaTruong, th.MaTruong)) && !dsPhu.Any(x => Trung(x, th.MaTruong)))
                    dsPhu.Add(th.MaTruong);
            }

            foreach (var sx in dinhNghia.SapXep)
                YeuCauTruong(nguon, sx.MaTruong, "sắp xếp");

            var thamSo = new List<SqlParameter>();
            var select = new List<string>();

            foreach (var cot in cotHienThi)
            {
                var truong = nguon.TimTruong(cot.MaTruong)!;
                select.Add($"{truong.BieuThuc} AS [{truong.Ma}]");
            }

            foreach (string ma in dsPhu)
            {
                var truong = nguon.TimTruong(ma)!;
                select.Add($"{truong.BieuThuc} AS [{truong.Ma}]");
            }

            // ----- WHERE -----
            var dieuKien = new List<string>();

            var (dieuKienPhamVi, thamSoPhamVi) = DungPhamVi(nguon, phamVi);
            if (dieuKienPhamVi.Length > 0)
                dieuKien.Add(dieuKienPhamVi);
            thamSo.AddRange(thamSoPhamVi);

            int chiSoLoc = 0;
            foreach (var loc in dinhNghia.Loc.Where(l => l.Bat))
            {
                if (string.IsNullOrWhiteSpace(loc.MaTruong))
                    continue;

                YeuCauTruong(nguon, loc.MaTruong, "bộ lọc");
                var truong = nguon.TimTruong(loc.MaTruong)!;
                string? dieuKienMot = DungDieuKienLoc(truong, loc, chiSoLoc, thamSo);
                if (dieuKienMot != null)
                    dieuKien.Add(dieuKienMot);

                chiSoLoc++;
            }

            // ----- ORDER BY: nhóm trước để các dòng cùng nhóm nằm liền nhau -----
            var order = new List<string>();
            foreach (var nhom in dinhNghia.Nhom)
            {
                var truong = nguon.TimTruong(nhom.MaTruong);
                if (truong != null)
                    order.Add($"{truong.BieuThuc} ASC");
            }

            foreach (var sx in dinhNghia.SapXep)
            {
                var truong = nguon.TimTruong(sx.MaTruong);
                if (truong == null) continue;

                string huong = sx.Huong == HuongSapXep.GiamDan ? "DESC" : "ASC";
                string menhDe = $"{truong.BieuThuc} {huong}";
                if (!order.Any(o => o.StartsWith(truong.BieuThuc + " ", StringComparison.OrdinalIgnoreCase)))
                    order.Add(menhDe);
            }

            int gioiHan = dinhNghia.GioiHanDong <= 0 ? 1000 : Math.Min(dinhNghia.GioiHanDong, 20000);
            thamSo.Add(new SqlParameter("@GioiHanDong", SqlDbType.Int) { Value = gioiHan + 1 });

            var sql = new StringBuilder();
            sql.AppendLine($"SELECT TOP (@GioiHanDong) {string.Join(", ", select)}");
            sql.AppendLine(nguon.FromSql);

            if (dieuKien.Count > 0)
                sql.AppendLine("WHERE " + string.Join(" AND ", dieuKien));

            if (order.Count > 0)
                sql.AppendLine("ORDER BY " + string.Join(", ", order));

            return new KetQua
            {
                Sql = sql.ToString(),
                ThamSo = thamSo.ToArray(),
                CotHienThi = cotHienThi.Select(c => c.MaTruong).ToList(),
                CotPhu = dsPhu,
                ChamGioiHanDuKien = true
            };
        }

        // -------------------------------------------------------------- Phạm vi

        private static (string DieuKien, SqlParameter[] ThamSo) DungPhamVi(
            DataSourceDefinition nguon, PhamViInfo? phamVi)
        {
            return nguon.PhamVi switch
            {
                PhamViNguon.ChoHocVien => PhamViSqlBuilder.ChoHocVien(phamVi, nguon.AliasChinh),
                PhamViNguon.ChoLopHoc => PhamViSqlBuilder.ChoLopHoc(phamVi, nguon.AliasChinh),
                _ => ("", Array.Empty<SqlParameter>())
            };
        }

        // -------------------------------------------------------------- Bộ lọc

        private static string? DungDieuKienLoc(FieldDefinition truong, ReportFilterDef loc,
            int chiSo, List<SqlParameter> thamSo)
        {
            string expr = truong.BieuThuc;

            if (ReportEnumText.LaDanhSach(loc.ToanTu))
            {
                var phanTu = ReportFormatters.TachDanhSach(loc.GiaTri1);
                if (phanTu.Count == 0)
                    throw new ReportBuildException($"Bộ lọc '{truong.TieuDe}': danh sách giá trị đang trống.");

                var ten = new List<string>();
                for (int i = 0; i < phanTu.Count; i++)
                {
                    string thamSoTen = $"@f{chiSo}_{i}";
                    thamSo.Add(TaoThamSo(thamSoTen, truong, phanTu[i]));
                    ten.Add(thamSoTen);
                }

                string toanTu = loc.ToanTu == ToanTuLoc.TrongDanhSach ? "IN" : "NOT IN";
                return $"{expr} {toanTu} ({string.Join(", ", ten)})";
            }

            if (ReportEnumText.CanHaiGiaTri(loc.ToanTu))
            {
                string t1 = $"@f{chiSo}a";
                string t2 = $"@f{chiSo}b";
                thamSo.Add(TaoThamSo(t1, truong, loc.GiaTri1));
                thamSo.Add(TaoThamSo(t2, truong, loc.GiaTri2));
                return $"{expr} BETWEEN {t1} AND {t2}";
            }

            if (loc.ToanTu == ToanTuLoc.Bang && loc.GiaTri1.Trim().Length == 0 &&
                truong.Kieu == KieuDuLieu.Chuoi)
            {
                return $"({expr} IS NULL OR {expr} = N'')";
            }

            string tham = $"@f{chiSo}";
            string giaTriLoc = ReportEnumText.LaLike(loc.ToanTu) ? EscapeLike(loc.GiaTri1) : loc.GiaTri1;
            thamSo.Add(TaoThamSo(tham, truong, giaTriLoc));

            return loc.ToanTu switch
            {
                ToanTuLoc.Bang => $"{expr} = {tham}",
                ToanTuLoc.Khac => $"({expr} <> {tham} OR {expr} IS NULL)",
                ToanTuLoc.Chua => $"{expr} COLLATE {Collation} LIKE N'%' + {tham} + N'%'",
                ToanTuLoc.KhongChua => $"({expr} IS NULL OR {expr} COLLATE {Collation} NOT LIKE N'%' + {tham} + N'%')",
                ToanTuLoc.BatDauBang => $"{expr} COLLATE {Collation} LIKE {tham} + N'%'",
                ToanTuLoc.KetThucBang => $"{expr} COLLATE {Collation} LIKE N'%' + {tham}",
                ToanTuLoc.LonHon => $"{expr} > {tham}",
                ToanTuLoc.LonHonBang => $"{expr} >= {tham}",
                ToanTuLoc.NhoHon => $"{expr} < {tham}",
                ToanTuLoc.NhoHonBang => $"{expr} <= {tham}",
                _ => $"{expr} = {tham}"
            };
        }

        private static SqlParameter TaoThamSo(string ten, FieldDefinition truong, string? chuoi)
        {
            if (!ReportFormatters.ThuPhanTich(chuoi, truong.Kieu, out object? giaTri))
                throw new ReportBuildException(
                    $"Bộ lọc '{truong.TieuDe}': giá trị '{chuoi}' không đúng kiểu {ReportEnumText.Ten(truong.Kieu)}.");

            return truong.Kieu switch
            {
                KieuDuLieu.SoNguyen => new SqlParameter(ten, SqlDbType.Int) { Value = Convert.ToInt32(giaTri, CultureInfo.InvariantCulture) },
                KieuDuLieu.SoThuc or KieuDuLieu.PhanTram => new SqlParameter(ten, SqlDbType.Float) { Value = Convert.ToDouble(giaTri, CultureInfo.InvariantCulture) },
                KieuDuLieu.TienTe => new SqlParameter(ten, SqlDbType.Decimal) { Value = Convert.ToDecimal(giaTri, CultureInfo.InvariantCulture) },
                KieuDuLieu.Ngay => new SqlParameter(ten, SqlDbType.Date) { Value = ((DateTime)giaTri!).Date },
                KieuDuLieu.Gio => new SqlParameter(ten, SqlDbType.DateTime2) { Value = (DateTime)giaTri! },
                KieuDuLieu.Logic => new SqlParameter(ten, SqlDbType.Bit) { Value = Convert.ToBoolean(giaTri) },
                _ => new SqlParameter(ten, SqlDbType.NVarChar, 4000) { Value = chuoi ?? "" }
            };
        }

        // -------------------------------------------------------------- Tiện ích

        private static void YeuCauTruong(DataSourceDefinition nguon, string ma, string vaiTro)
        {
            if (string.IsNullOrWhiteSpace(ma))
                throw new ReportBuildException($"Cấu hình {vaiTro} đang thiếu trường dữ liệu.");

            if (!nguon.CoTruong(ma))
                throw new ReportBuildException(
                    $"Trường '{ma}' không thuộc nguồn dữ liệu '{nguon.Ten}' (danh sách trắng).");
        }

        private static bool Trung(string a, string b) =>
            string.Equals(a, b, StringComparison.OrdinalIgnoreCase);

        /// <summary>Escape ký tự đại diện của LIKE để người dùng không vô tình tạo mẫu rộng.</summary>
        public static string EscapeLike(string? giaTri)
        {
            if (string.IsNullOrEmpty(giaTri))
                return "";

            return giaTri.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]");
        }
    }
}
