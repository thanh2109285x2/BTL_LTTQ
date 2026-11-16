using System.Data;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using BTL_LTTQ.Common;
using BTL_LTTQ.DAL;
using Microsoft.Data.SqlClient;
namespace BTL_LTTQ.Services.Ai
{
    public static class AiCongCu
    {
        private const int ToiDaDong = 30;
        public const string DinhNghiaJson = """
        [
          {
            "type": "function",
            "function": {
              "name": "tim_lop",
              "description": "Tìm lớp theo môn, giáo viên, ca học và số học viên đã ghi danh.",
              "parameters": {
                "type": "object",
                "properties": {
                  "ten_mon": { "type": ["string", "null"], "description": "Một phần tên môn." },
                  "ten_giao_vien": { "type": ["string", "null"], "description": "Một phần tên giáo viên." },
                  "cua_toi": { "type": "boolean", "description": "Đặt true khi người dùng hỏi lớp của mình/lớp tôi đang dạy. Công cụ sẽ tự lọc theo giáo viên đang đăng nhập; không cần hỏi tên giáo viên." },
                  "ten_ca": { "type": ["string", "null"], "description": "Một phần tên ca, ví dụ sáng hoặc tối." },
                  "si_so_tu": { "type": ["integer", "null"] },
                  "si_so_den": { "type": ["integer", "null"] }
                }
              }
            }
          },
          {
            "type": "function",
            "function": {
              "name": "tim_giao_vien",
              "description": "Tìm giáo viên theo tên, chuyên môn hoặc môn đang dạy.",
              "parameters": {
                "type": "object",
                "properties": {
                  "ten": { "type": ["string", "null"] },
                  "chuyen_mon": { "type": ["string", "null"] },
                  "ten_mon": { "type": ["string", "null"] }
                }
              }
            }
          },
          {
            "type": "function",
            "function": {
              "name": "xep_hang_hoc_vien",
              "description": "Xếp hạng học viên trong lớp theo điểm.",
              "parameters": {
                "type": "object",
                "properties": {
                  "ten_lop": { "type": ["string", "null"] },
                  "top": { "type": ["integer", "null"], "description": "Số học viên đứng đầu mỗi lớp. Mặc định 3." }
                }
              }
            }
          },
          {
            "type": "function",
            "function": {
              "name": "doanh_thu_mon",
              "description": "Ước tính doanh thu bằng học phí nhân số học viên đã ghi danh.",
              "parameters": {
                "type": "object",
                "properties": {
                  "ten_mon": { "type": ["string", "null"] }
                }
              }
            }
          },
          {
            "type": "function",
            "function": {
              "name": "lop_theo_phong",
              "description": "Liệt kê lớp học theo phòng.",
              "parameters": {
                "type": "object",
                "properties": {
                  "ten_phong": { "type": ["string", "null"] }
                }
              }
            }
          }
        ]
        """;

        public static string LayDinhNghiaTheoQuyen()
        {
            JsonArray dinhNghia = JsonNode.Parse(DinhNghiaJson)!.AsArray();
            var duocPhep = new JsonArray();
            foreach (JsonNode? congCu in dinhNghia)
            {
                string ten = congCu?["function"]?["name"]?.ToString() ?? "";
                if (CoQuyenCongCu(ten))
                    duocPhep.Add(congCu!.DeepClone());
            }
            return duocPhep.ToJsonString();
        }

        public static List<string> LayMoTaCongCuDuocPhep()
        {
            JsonArray dinhNghia = JsonNode.Parse(DinhNghiaJson)!.AsArray();
            var moTa = new List<string>();
            foreach (JsonNode? congCu in dinhNghia)
            {
                string ten = congCu?["function"]?["name"]?.ToString() ?? "";
                string? noiDung = MoTaCongCu(ten, congCu?["function"]?["description"]?.ToString());
                if (CoQuyenCongCu(ten) && !string.IsNullOrWhiteSpace(noiDung))
                    moTa.Add(noiDung);
            }
            return moTa;
        }

        private static string? MoTaCongCu(string tenHam, string? moTaMacDinh) => tenHam switch
        {
            "xep_hang_hoc_vien" when !PermissionService.Can(ChucNangConst.BC_XEPHANG)
                && PermissionService.ChiCuaMinh(ChucNangConst.BC_DIEM) => "Xem điểm và thứ hạng của chính học viên đang đăng nhập.",
            "xep_hang_hoc_vien" when PermissionService.ChiCuaMinh(ChucNangConst.BC_XEPHANG)
                && CurrentUser.IsGiaoVien => "Xem xếp hạng trong các lớp do giáo viên đang đăng nhập phụ trách.",
            "lop_theo_phong" when PermissionService.ChiCuaMinh(ChucNangConst.BC_LOP_PHONG)
                && CurrentUser.IsGiaoVien => "Liệt kê các lớp do giáo viên đang đăng nhập phụ trách theo phòng.",
            "lop_theo_phong" when PermissionService.ChiCuaMinh(ChucNangConst.BC_LOP_PHONG)
                && CurrentUser.IsHocVien => "Xem phòng học của lớp mà học viên đang đăng nhập tham gia.",
            "doanh_thu_mon" when PermissionService.ChiCuaMinh(ChucNangConst.BC_DOANHTHU)
                && CurrentUser.IsGiaoVien => "Ước tính doanh thu theo môn trong các lớp do giáo viên đang đăng nhập phụ trách.",
            _ => moTaMacDinh
        };

        private static bool CoQuyenCongCu(string tenHam) => tenHam switch
        {
            "tim_lop" => PermissionService.Can(ChucNangConst.TIM_LOP) || PermissionService.Can(ChucNangConst.LOPHOC_XEM),
            "tim_giao_vien" => PermissionService.Can(ChucNangConst.TIM_GIAOVIEN) || PermissionService.Can(ChucNangConst.GIAOVIEN_XEM),
            "xep_hang_hoc_vien" => PermissionService.Can(ChucNangConst.BC_XEPHANG) || PermissionService.Can(ChucNangConst.BC_DIEM),
            "doanh_thu_mon" => !CurrentUser.IsHocVien && PermissionService.Can(ChucNangConst.BC_DOANHTHU),
            "lop_theo_phong" => PermissionService.Can(ChucNangConst.BC_LOP_PHONG),
            _ => false
        };

        public static string ThucThi(string tenHam, string thamSoJson, out DataTable? bangKetQua)
        {
            bangKetQua = null;
            try
            {
                using JsonDocument taiLieu = JsonDocument.Parse(
                    string.IsNullOrWhiteSpace(thamSoJson) ? "{}" : thamSoJson);
                JsonElement thamSo = taiLieu.RootElement;
                return tenHam switch
                {
                    "tim_lop" => TimLop(thamSo, out bangKetQua),
                    "tim_giao_vien" => TimGiaoVien(thamSo, out bangKetQua),
                    "xep_hang_hoc_vien" => XepHangHocVien(thamSo, out bangKetQua),
                    "doanh_thu_mon" => DoanhThuMon(thamSo, out bangKetQua),
                    "lop_theo_phong" => LopTheoPhong(thamSo, out bangKetQua),
                    _ => Loi("Trợ lý AI hiện chưa thể xử lý yêu cầu này. Bạn vui lòng thử lại sau.")
                };
            }
            catch (Exception)
            {
                bangKetQua = null;
                return Loi("Không thể truy vấn dữ liệu lúc này.");
            }
        }
        private static string TimLop(JsonElement thamSo, out DataTable? bangKetQua)
        {
            bangKetQua = null;
            string? quyen = QuyenDauTien(ChucNangConst.TIM_LOP, ChucNangConst.LOPHOC_XEM);
            if (quyen == null)
                return Loi("Tài khoản đang đăng nhập không được tìm lớp.");
            var phamVi = PhamViSqlBuilder.TaoDieuKienLopHoc(PhamViDuLieu.ChoLopHoc(quyen), "l");
            string locCuaToi = "";
            var thamSoPhamVi = phamVi.ThamSo.ToList();
            if (DocBoolean(thamSo, "cua_toi"))
            {
                if (!CurrentUser.IsGiaoVien || string.IsNullOrWhiteSpace(CurrentUser.MaGV))
                    return Loi("Chỉ tài khoản giáo viên đã liên kết mã giáo viên mới tra cứu được lớp mình đang dạy.");
                locCuaToi = " AND l.MaGV = @MaGVCuaToi";
                thamSoPhamVi.Add(new SqlParameter("@MaGVCuaToi", SqlDbType.VarChar, 20) { Value = CurrentUser.MaGV });
            }
            string dieuKienPhamVi = phamVi.DieuKienSql == "" ? locCuaToi : " AND " + phamVi.DieuKienSql + locCuaToi;
            string sql = $"""
                SELECT l.MaLop AS [Mã lớp], l.TenLop AS [Tên lớp], m.TenMon AS [Môn học],
                       g.TenGV AS [Giáo viên], c.TenCa AS [Ca học], tg.TenThoiGian AS [Lịch học],
                       p.TenPhong AS [Phòng], l.NgayBD AS [Ngày bắt đầu], l.NgayKT AS [Ngày kết thúc],
                       (SELECT COUNT(*) FROM DangKy d WHERE d.MaLop = l.MaLop) AS [Sĩ số]
                FROM LopHoc l
                LEFT JOIN MonHoc m ON l.MaMon = m.MaMon
                LEFT JOIN GiaoVien g ON l.MaGV = g.MaGV
                LEFT JOIN CaHoc c ON l.MaCa = c.MaCa
                LEFT JOIN ThoiGian tg ON l.MaThoiGian = tg.MaThoiGian
                LEFT JOIN PhongHoc p ON l.MaPhong = p.MaPhong
                WHERE (@TenMon IS NULL OR m.TenMon COLLATE Vietnamese_CI_AI LIKE '%' + @TenMon + '%')
                  AND (@TenGV IS NULL OR g.TenGV COLLATE Vietnamese_CI_AI LIKE '%' + @TenGV + '%')
                  AND (@TenCa IS NULL OR c.TenCa COLLATE Vietnamese_CI_AI LIKE '%' + @TenCa + '%')
                  AND (SELECT COUNT(*) FROM DangKy d WHERE d.MaLop = l.MaLop)
                      BETWEEN @SiSoTu AND @SiSoDen
                {dieuKienPhamVi}
                ORDER BY l.NgayBD DESC
                """;
            var dsThamSo = new List<SqlParameter>
            {
                ThamSoChuoi("@TenMon", DocChuoi(thamSo, "ten_mon")),
                ThamSoChuoi("@TenGV", DocChuoi(thamSo, "ten_giao_vien")),
                ThamSoChuoi("@TenCa", DocChuoi(thamSo, "ten_ca")),
                new("@SiSoTu", DocSo(thamSo, "si_so_tu", 0)),
                new("@SiSoDen", DocSo(thamSo, "si_so_den", 1000))
            };
            dsThamSo.AddRange(thamSoPhamVi);
            bangKetQua = DbHelper.ExecuteQuery(sql, dsThamSo.ToArray());
            return DinhDangBang(bangKetQua);
        }
        private static string TimGiaoVien(JsonElement thamSo, out DataTable? bangKetQua)
        {
            bangKetQua = null;
            if (QuyenDauTien(ChucNangConst.TIM_GIAOVIEN, ChucNangConst.GIAOVIEN_XEM) == null)
                return Loi("Tài khoản đang đăng nhập không được tìm giáo viên.");
            const string sql = """
                SELECT g.MaGV AS [Mã giáo viên], g.TenGV AS [Tên giáo viên],
                       t.TenTrinhDo AS [Trình độ], c.TenChuyenMon AS [Chuyên môn],
                       (SELECT STRING_AGG(m.TenMon, ', ')
                        FROM GiaoVien_MonHoc gm
                        JOIN MonHoc m ON gm.MaMon = m.MaMon
                        WHERE gm.MaGV = g.MaGV) AS [Môn dạy]
                FROM GiaoVien g
                LEFT JOIN TrinhDo t ON g.MaTrinhDo = t.MaTrinhDo
                LEFT JOIN ChuyenMon c ON g.MaChuyenMon = c.MaChuyenMon
                WHERE (@Ten IS NULL OR g.TenGV COLLATE Vietnamese_CI_AI LIKE '%' + @Ten + '%')
                  AND (@ChuyenMon IS NULL OR c.TenChuyenMon COLLATE Vietnamese_CI_AI LIKE '%' + @ChuyenMon + '%')
                  AND (@TenMon IS NULL OR EXISTS (
                        SELECT 1
                        FROM GiaoVien_MonHoc gm
                        JOIN MonHoc m ON gm.MaMon = m.MaMon
                        WHERE gm.MaGV = g.MaGV
                          AND m.TenMon COLLATE Vietnamese_CI_AI LIKE '%' + @TenMon + '%'))
                ORDER BY g.TenGV
                """;
            bangKetQua = DbHelper.ExecuteQuery(sql,
                ThamSoChuoi("@Ten", DocChuoi(thamSo, "ten")),
                ThamSoChuoi("@ChuyenMon", DocChuoi(thamSo, "chuyen_mon")),
                ThamSoChuoi("@TenMon", DocChuoi(thamSo, "ten_mon")));
            return DinhDangBang(bangKetQua);
        }
        private static string XepHangHocVien(JsonElement thamSo, out DataTable? bangKetQua)
        {
            bangKetQua = null;
            string? quyen = QuyenDauTien(ChucNangConst.BC_XEPHANG, ChucNangConst.BC_DIEM);
            if (quyen == null)
                return Loi("Tài khoản đang đăng nhập không được xem điểm.");
            var phamVi = PhamViHocVien(quyen);
            string dieuKienPhamVi = phamVi.DieuKienSql == "" ? "" : " AND " + phamVi.DieuKienSql;
            int top = Math.Clamp(DocSo(thamSo, "top", 3), 1, 20);
            string sql = $"""
                SELECT [Tên lớp], [Mã học viên], [Tên học viên], [Điểm], [Hạng]
                FROM (
                    SELECT l.TenLop AS [Tên lớp], hv.MaHocVien AS [Mã học viên],
                           hv.TenHocVien AS [Tên học viên], hv.Diem AS [Điểm],
                           ROW_NUMBER() OVER (PARTITION BY l.MaLop ORDER BY hv.Diem DESC, hv.MaHocVien) AS [Hạng]
                    FROM HocVien hv
                    JOIN LopHoc l ON hv.MaLop = l.MaLop
                    WHERE hv.Diem IS NOT NULL
                      AND (@TenLop IS NULL OR l.TenLop COLLATE Vietnamese_CI_AI LIKE '%' + @TenLop + '%')
                      {dieuKienPhamVi}
                ) x
                WHERE [Hạng] <= @Top
                ORDER BY [Tên lớp], [Hạng]
                """;
            var dsThamSo = new List<SqlParameter>
            {
                ThamSoChuoi("@TenLop", DocChuoi(thamSo, "ten_lop")),
                new("@Top", top)
            };
            dsThamSo.AddRange(phamVi.ThamSo);
            bangKetQua = DbHelper.ExecuteQuery(sql, dsThamSo.ToArray());
            return DinhDangBang(bangKetQua);
        }
        private static string DoanhThuMon(JsonElement thamSo, out DataTable? bangKetQua)
        {
            bangKetQua = null;
            if (CurrentUser.IsHocVien)
                return Loi("Học viên không được xem doanh thu.");
            if (!PermissionService.Can(ChucNangConst.BC_DOANHTHU))
                return Loi("Tài khoản đang đăng nhập không được xem doanh thu.");
            string locGiaoVien = "";
            var dsThamSo = new List<SqlParameter>
            {
                ThamSoChuoi("@TenMon", DocChuoi(thamSo, "ten_mon"))
            };
            if (PermissionService.ChiCuaMinh(ChucNangConst.BC_DOANHTHU) && CurrentUser.IsGiaoVien)
            {
                locGiaoVien = " AND l.MaGV = @MaGVCuaToi";
                dsThamSo.Add(new SqlParameter("@MaGVCuaToi", CurrentUser.MaGV ?? ""));
            }
            string sql = $"""
                SELECT m.TenMon AS [Môn học], m.HocPhi AS [Học phí],
                       COUNT(hv.MaHocVien) AS [Sĩ số],
                       COUNT(hv.MaHocVien) * ISNULL(m.HocPhi, 0) AS [Doanh thu ước tính]
                FROM MonHoc m
                LEFT JOIN LopHoc l ON l.MaMon = m.MaMon
                LEFT JOIN HocVien hv ON hv.MaLop = l.MaLop
                WHERE (@TenMon IS NULL OR m.TenMon COLLATE Vietnamese_CI_AI LIKE '%' + @TenMon + '%')
                {locGiaoVien}
                GROUP BY m.MaMon, m.TenMon, m.HocPhi
                ORDER BY [Doanh thu ước tính] DESC
                """;
            bangKetQua = DbHelper.ExecuteQuery(sql, dsThamSo.ToArray());
            return DinhDangBang(bangKetQua);
        }
        private static string LopTheoPhong(JsonElement thamSo, out DataTable? bangKetQua)
        {
            bangKetQua = null;
            if (!PermissionService.Can(ChucNangConst.BC_LOP_PHONG))
                return Loi("Tài khoản đang đăng nhập không được xem lớp theo phòng.");
            var phamVi = PhamViBaoCaoLop();
            string dieuKienPhamVi = phamVi.DieuKienSql == "" ? "" : " AND " + phamVi.DieuKienSql;
            string sql = $"""
                SELECT p.TenPhong AS [Phòng], l.TenLop AS [Tên lớp],
                       m.TenMon AS [Môn học], g.TenGV AS [Giáo viên], c.TenCa AS [Ca học]
                FROM PhongHoc p
                LEFT JOIN LopHoc l ON l.MaPhong = p.MaPhong
                LEFT JOIN MonHoc m ON m.MaMon = l.MaMon
                LEFT JOIN GiaoVien g ON g.MaGV = l.MaGV
                LEFT JOIN CaHoc c ON c.MaCa = l.MaCa
                WHERE (@TenPhong IS NULL OR p.TenPhong COLLATE Vietnamese_CI_AI LIKE '%' + @TenPhong + '%')
                {dieuKienPhamVi}
                ORDER BY p.TenPhong, l.TenLop
                """;
            var dsThamSo = new List<SqlParameter>
            {
                ThamSoChuoi("@TenPhong", DocChuoi(thamSo, "ten_phong"))
            };
            dsThamSo.AddRange(phamVi.ThamSo);
            bangKetQua = DbHelper.ExecuteQuery(sql, dsThamSo.ToArray());
            return DinhDangBang(bangKetQua);
        }
        // ChoLopHoc chỉ nhận TIM_LOP và LOPHOC_XEM. Báo cáo phòng phải tự lọc khi chỉ có BC_LOP_PHONG.
        private static (string DieuKienSql, SqlParameter[] ThamSo) PhamViBaoCaoLop()
        {
            if (PermissionService.ChiCuaMinh(ChucNangConst.BC_LOP_PHONG))
            {
                if (CurrentUser.IsGiaoVien && !string.IsNullOrWhiteSpace(CurrentUser.MaGV))
                {
                    return ("l.MaGV = @PV_MaGV", new[]
                    {
                        new SqlParameter("@PV_MaGV", SqlDbType.VarChar, 20) { Value = CurrentUser.MaGV }
                    });
                }
                if (CurrentUser.IsHocVien && !string.IsNullOrWhiteSpace(CurrentUser.MaHocVien))
                {
                    return ("EXISTS (SELECT 1 FROM HocVien pv_h WHERE pv_h.MaHocVien = @PV_MaHocVien AND pv_h.MaLop = l.MaLop)", new[]
                    {
                        new SqlParameter("@PV_MaHocVien", SqlDbType.VarChar, 20) { Value = CurrentUser.MaHocVien }
                    });
                }
                return ("1 = 0", Array.Empty<SqlParameter>());
            }

            string? quyen = QuyenDauTien(ChucNangConst.TIM_LOP, ChucNangConst.LOPHOC_XEM);
            if (quyen != null)
                return PhamViSqlBuilder.TaoDieuKienLopHoc(PhamViDuLieu.ChoLopHoc(quyen), "l");
            return ("", Array.Empty<SqlParameter>());
        }
        private static (string DieuKienSql, SqlParameter[] ThamSo) PhamViHocVien(string maQuyen)
        {
            if (maQuyen == ChucNangConst.BC_DIEM)
                return PhamViDuLieu.ChoHocVien(maQuyen, "hv");
            if (!PermissionService.ChiCuaMinh(maQuyen))
                return ("", Array.Empty<SqlParameter>());
            if (CurrentUser.IsGiaoVien && !string.IsNullOrWhiteSpace(CurrentUser.MaGV))
            {
                return ("l.MaGV = @PV_MaGV", new[]
                {
                    new SqlParameter("@PV_MaGV", SqlDbType.VarChar, 20) { Value = CurrentUser.MaGV }
                });
            }
            if (CurrentUser.IsHocVien && !string.IsNullOrWhiteSpace(CurrentUser.MaHocVien))
            {
                return ("hv.MaHocVien = @PV_MaHocVien", new[]
                {
                    new SqlParameter("@PV_MaHocVien", SqlDbType.VarChar, 20) { Value = CurrentUser.MaHocVien }
                });
            }
            return ("1 = 0", Array.Empty<SqlParameter>());
        }
        private static string? QuyenDauTien(params string[] danhSach)
        {
            foreach (string ma in danhSach)
            {
                if (PermissionService.Can(ma))
                    return ma;
            }
            return null;
        }
        private static string? DocChuoi(JsonElement thamSo, string ten)
        {
            if (!thamSo.TryGetProperty(ten, out JsonElement nut))
                return null;
            if (nut.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
                return null;
            string text = nut.ToString().Trim();
            return text.Length == 0 ? null : text;
        }
        private static int DocSo(JsonElement thamSo, string ten, int macDinh)
        {
            if (!thamSo.TryGetProperty(ten, out JsonElement nut) || nut.ValueKind != JsonValueKind.Number)
                return macDinh;
            return nut.TryGetInt32(out int so) ? so : macDinh;
        }
        private static bool DocBoolean(JsonElement thamSo, string ten) =>
            thamSo.TryGetProperty(ten, out JsonElement nut) &&
            (nut.ValueKind == JsonValueKind.True ||
             (nut.ValueKind == JsonValueKind.String && bool.TryParse(nut.GetString(), out bool giaTri) && giaTri));
        private static SqlParameter ThamSoChuoi(string ten, string? giaTri)
        {
            return new SqlParameter(ten, (object?)giaTri ?? DBNull.Value);
        }
        private static string Loi(string noiDung) => "KHONG_CO_DU_LIEU: " + noiDung;
        private static string DinhDangBang(DataTable bang)
        {
            if (bang.Rows.Count == 0)
                return "Không có dòng nào.";
            var viet = new StringBuilder();
            viet.AppendLine("Số dòng: " + bang.Rows.Count);
            int soDong = Math.Min(bang.Rows.Count, ToiDaDong);
            for (int i = 0; i < soDong; i++)
            {
                var cot = new List<string>();
                foreach (DataColumn column in bang.Columns)
                    cot.Add(column.ColumnName + "=" + GiaTriO(bang.Rows[i][column]));
                viet.AppendLine(string.Join(" | ", cot));
            }
            if (bang.Rows.Count > ToiDaDong)
                viet.AppendLine("Còn " + (bang.Rows.Count - ToiDaDong) + " dòng chưa gửi.");
            return viet.ToString();
        }
        private static string GiaTriO(object giaTri)
        {
            if (giaTri == DBNull.Value)
                return "";
            CultureInfo vietNam = CultureInfo.GetCultureInfo("vi-VN");
            return giaTri switch
            {
                DateTime ngay => ngay.ToString("dd/MM/yyyy"),
                decimal tien => tien.ToString("N0", vietNam),
                double diem => diem.ToString("0.##", vietNam),
                float diem => diem.ToString("0.##", vietNam),
                _ => Convert.ToString(giaTri, vietNam) ?? ""
            };
        }
    }
}
