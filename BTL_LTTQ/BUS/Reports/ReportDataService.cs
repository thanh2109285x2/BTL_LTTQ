using BTL_LTTQ.Common.Reports;
using BTL_LTTQ.Models;
using BTL_LTTQ.Models.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace BTL_LTTQ.BUS.Reports
{
    /// <summary>
    /// Chạy báo cáo: dựng truy vấn, lấy dữ liệu, rồi nhóm và tính tổng phía máy khách.
    ///
    /// Vì sao nhóm/tổng ở máy khách? Để không phải sinh GROUP BY động (dễ lỗi và khó kiểm soát),
    /// trong khi vẫn giữ mọi giá trị lọc là tham số hóa. Số dòng đã bị giới hạn bởi TOP.
    /// </summary>
    public sealed class ReportDataService
    {
        private readonly ReportCatalogService _danhMuc;

        public ReportDataService(ReportCatalogService danhMuc)
        {
            _danhMuc = danhMuc;
        }

        public ReportResult Chay(ReportDefinition dinhNghia, IReportDataProvider nhaCungCap, PhamViInfo? phamVi)
        {
            ArgumentNullException.ThrowIfNull(dinhNghia);
            ArgumentNullException.ThrowIfNull(nhaCungCap);

            var nguon = _danhMuc.YeuCauNguon(dinhNghia.MaNguon);
            var truyVan = new ReportQueryBuilder().Dung(dinhNghia, nguon, phamVi);

            var dongHo = Stopwatch.StartNew();
            DataTable bang = nhaCungCap.LayDuLieu(truyVan, nguon);
            dongHo.Stop();

            bool chamGioiHan = false;
            if (bang.Rows.Count > dinhNghia.GioiHanDong && dinhNghia.GioiHanDong > 0)
            {
                chamGioiHan = true;
                while (bang.Rows.Count > dinhNghia.GioiHanDong)
                    bang.Rows.RemoveAt(bang.Rows.Count - 1);
            }

            var ketQua = new ReportResult
            {
                TieuDe = string.IsNullOrWhiteSpace(dinhNghia.TieuDe) ? dinhNghia.Ten : dinhNghia.TieuDe,
                PhuDe = dinhNghia.PhuDe,
                ThoiGianChayMs = dongHo.ElapsedMilliseconds,
                ChamGioiHan = chamGioiHan,
                DungDuLieuMau = nhaCungCap.LaDuLieuMau,
                ThoiDiemChay = DateTime.Now,
                SoDongMoiTrang = ReportLayoutService.SoDongMoiTrang(dinhNghia.BoCuc)
            };

            ketQua.Cot.AddRange(DungCot(dinhNghia, nguon));
            ketQua.SoDongDuLieu = bang.Rows.Count;

            var dongDuLieu = bang.Rows.Cast<DataRow>().ToList();
            SapXepDong(dongDuLieu, dinhNghia, nguon);
            DungDong(ketQua, dinhNghia, nguon, dongDuLieu);

            return ketQua;
        }

        // ------------------------------------------------------------ Cột kết quả

        private static IEnumerable<ReportResultColumn> DungCot(ReportDefinition dinhNghia, DataSourceDefinition nguon)
        {
            foreach (var cot in dinhNghia.CotHienThi)
            {
                var truong = nguon.TimTruong(cot.MaTruong);
                if (truong == null) continue;

                yield return new ReportResultColumn
                {
                    MaTruong = truong.Ma,
                    TieuDe = string.IsNullOrWhiteSpace(cot.TieuDe) ? truong.TieuDe : cot.TieuDe,
                    Kieu = truong.Kieu,
                    CanLe = cot.CanLe,
                    DoRong = cot.DoRong <= 0 ? truong.DoRong : cot.DoRong,
                    DinhDang = string.IsNullOrWhiteSpace(cot.DinhDang) ? truong.DinhDangMacDinh : cot.DinhDang,
                    TienTo = cot.TienTo,
                    HauTo = cot.HauTo,
                    InDam = cot.InDam
                };
            }
        }

        // ------------------------------------------------------------ Sắp xếp

        private static void SapXepDong(List<DataRow> dong, ReportDefinition dinhNghia, DataSourceDefinition nguon)
        {
            var tieuChi = new List<(string Ma, bool Giam)>();

            foreach (var nhom in dinhNghia.Nhom)
                if (nguon.CoTruong(nhom.MaTruong))
                    tieuChi.Add((nhom.MaTruong, false));

            foreach (var sx in dinhNghia.SapXep)
                if (nguon.CoTruong(sx.MaTruong) && !tieuChi.Any(t => t.Ma.Equals(sx.MaTruong, StringComparison.OrdinalIgnoreCase)))
                    tieuChi.Add((sx.MaTruong, sx.Huong == HuongSapXep.GiamDan));

            if (tieuChi.Count == 0 || dong.Count < 2)
                return;

            var kieu = tieuChi.ToDictionary(
                t => t.Ma,
                t => nguon.TimTruong(t.Ma)?.Kieu ?? KieuDuLieu.Chuoi,
                StringComparer.OrdinalIgnoreCase);

            dong.Sort((a, b) =>
            {
                foreach (var (ma, giam) in tieuChi)
                {
                    int soSanh = SoSanh(a[ma], b[ma], kieu[ma]);
                    if (soSanh != 0)
                        return giam ? -soSanh : soSanh;
                }
                return 0;
            });
        }

        private static int SoSanh(object? a, object? b, KieuDuLieu kieu)
        {
            bool aRong = a == null || a == DBNull.Value;
            bool bRong = b == null || b == DBNull.Value;
            if (aRong && bRong) return 0;
            if (aRong) return 1;   // giá trị rỗng xuống cuối
            if (bRong) return -1;

            switch (kieu)
            {
                case KieuDuLieu.SoNguyen:
                case KieuDuLieu.SoThuc:
                case KieuDuLieu.TienTe:
                case KieuDuLieu.PhanTram:
                    if (ReportFormatters.LaSo(a, out double da) && ReportFormatters.LaSo(b, out double db))
                        return da.CompareTo(db);
                    break;

                case KieuDuLieu.Ngay:
                case KieuDuLieu.Gio:
                    if (ReportFormatters.LaNgay(a, out DateTime nga) && ReportFormatters.LaNgay(b, out DateTime ngb))
                        return nga.CompareTo(ngb);
                    break;

                case KieuDuLieu.Logic:
                    if (ReportFormatters.LaBool(a, out bool ba) && ReportFormatters.LaBool(b, out bool bb))
                        return ba.CompareTo(bb);
                    break;
            }

            return string.Compare(Convert.ToString(a), Convert.ToString(b),
                CultureInfo.GetCultureInfo("vi-VN"), CompareOptions.IgnoreCase);
        }

        // ------------------------------------------------------------ Dựng dòng

        private void DungDong(ReportResult kq, ReportDefinition dinhNghia, DataSourceDefinition nguon, List<DataRow> dong)
        {
            var cotHienThi = kq.Cot.Select(c => c.MaTruong).ToList();
            var nhom = dinhNghia.Nhom.Where(n => nguon.CoTruong(n.MaTruong)).ToList();
            var tongTheoNhom = dinhNghia.TongHop.Where(t => t.HienThi && t.Ham != HamTongHop.Khong && t.PhamVi == PhamViTongHop.TheoNhom).ToList();
            var tongBaoCao = dinhNghia.TongHop.Where(t => t.HienThi && t.Ham != HamTongHop.Khong && t.PhamVi == PhamViTongHop.ToanBaoCao).ToList();

            if (nhom.Count == 0)
            {
                foreach (var r in dong)
                    kq.Dong.Add(DongDuLieu(r, cotHienThi, 0));

                if (dinhNghia.BoCuc.HienTongCong && tongBaoCao.Count > 0)
                    kq.Dong.Add(DongTong("Tổng cộng", dong, cotHienThi, tongBaoCao, LoaiDongBaoCao.TongBaoCao, 0));

                return;
            }

            XuatNhom(kq, dong, 0, dong.Count, 0, cotHienThi, nhom, nguon, tongTheoNhom);

            if (dinhNghia.BoCuc.HienTongCong && tongBaoCao.Count > 0)
                kq.Dong.Add(DongTong("Tổng cộng", dong, cotHienThi, tongBaoCao, LoaiDongBaoCao.TongBaoCao, 0));
        }

        private void XuatNhom(ReportResult kq, List<DataRow> dong, int bat, int ket, int cap,
            List<string> cotHienThi, List<ReportGroupDef> nhom, DataSourceDefinition nguon, List<ReportSummaryDef> tongTheoNhom)
        {
            if (cap >= nhom.Count)
            {
                for (int k = bat; k < ket; k++)
                    kq.Dong.Add(DongDuLieu(dong[k], cotHienThi, cap));
                return;
            }

            var nhomHienTai = nhom[cap];
            var truongNhom = nguon.TimTruong(nhomHienTai.MaTruong);
            if (truongNhom == null)
            {
                for (int k = bat; k < ket; k++)
                    kq.Dong.Add(DongDuLieu(dong[k], cotHienThi, cap));
                return;
            }

            int i = bat;
            while (i < ket)
            {
                string khoa = ReportFormatters.Khoa(dong[i][truongNhom.Ma]);
                int j = i;
                while (j < ket && ReportFormatters.Khoa(dong[j][truongNhom.Ma]) == khoa)
                    j++;

                // Tiêu đề nhóm
                var dongTieuDe = new ReportResultRow
                {
                    Loai = LoaiDongBaoCao.TieuDeNhom,
                    CapNhom = cap,
                    NhanNhom = ReportFormatters.Hien(dong[i][truongNhom.Ma], truongNhom.Kieu, truongNhom.DinhDangMacDinh),
                    GiaTri = new object?[cotHienThi.Count],
                    InDam = true
                };
                DatNhanNhom(dongTieuDe, truongNhom.Ma, cotHienThi, cap);
                kq.Dong.Add(dongTieuDe);

                // Thân nhóm
                if (cap + 1 < nhom.Count)
                    XuatNhom(kq, dong, i, j, cap + 1, cotHienThi, nhom, nguon, tongTheoNhom);
                else
                    for (int k = i; k < j; k++)
                        kq.Dong.Add(DongDuLieu(dong[k], cotHienThi, cap + 1));

                // Chân nhóm (tổng theo nhóm)
                if (nhomHienTai.HienThiTongNhom && tongTheoNhom.Count > 0)
                {
                    var phanDoan = dong.GetRange(i, j - i);
                    kq.Dong.Add(DongTong($"Tổng {dongTieuDe.NhanNhom}", phanDoan, cotHienThi,
                        tongTheoNhom, LoaiDongBaoCao.TongNhom, cap + 1));
                }

                i = j;
            }
        }

        private static void DatNhanNhom(ReportResultRow dong, string maTruong, List<string> cotHienThi, int cap)
        {
            int chiSo = cotHienThi.FindIndex(c => c.Equals(maTruong, StringComparison.OrdinalIgnoreCase));
            if (chiSo < 0) chiSo = 0;

            string nhan = new string(' ', cap * 3) + dong.NhanNhom;
            dong.GiaTri[chiSo] = nhan;
        }

        private static ReportResultRow DongDuLieu(DataRow r, List<string> cotHienThi, int cap)
        {
            var giaTri = new object?[cotHienThi.Count];
            for (int i = 0; i < cotHienThi.Count; i++)
            {
                string ma = cotHienThi[i];
                giaTri[i] = r.Table.Columns.Contains(ma) ? r[ma] : null;
            }

            return new ReportResultRow
            {
                Loai = LoaiDongBaoCao.DuLieu,
                GiaTri = giaTri,
                CapNhom = cap
            };
        }

        private static ReportResultRow DongTong(string nhan, List<DataRow> phanDoan, List<string> cotHienThi,
            List<ReportSummaryDef> tongHop, LoaiDongBaoCao loai, int cap)
        {
            var giaTri = new object?[cotHienThi.Count];
            bool daDatNhan = false;

            foreach (var th in tongHop)
            {
                int chiSo = cotHienThi.FindIndex(c => c.Equals(th.MaTruong, StringComparison.OrdinalIgnoreCase));
                object? giaTriTong = TinhTong(th.Ham, phanDoan, th.MaTruong);

                if (chiSo >= 0)
                {
                    giaTri[chiSo] = giaTriTong;
                    if (chiSo == 0) daDatNhan = true;
                }
            }

            if (!daDatNhan && cotHienThi.Count > 0)
                giaTri[0] = nhan;

            return new ReportResultRow
            {
                Loai = loai,
                GiaTri = giaTri,
                CapNhom = cap,
                NhanNhom = nhan,
                InDam = true
            };
        }

        private static object? TinhTong(HamTongHop ham, List<DataRow> dong, string maTruong)
        {
            if (dong.Count == 0) return null;

            var giaTri = dong
                .Select(r => r.Table.Columns.Contains(maTruong) ? r[maTruong] : null)
                .Where(v => v != null && v != DBNull.Value)
                .ToList();

            if (ham == HamTongHop.Dem)
                return giaTri.Count;

            if (giaTri.Count == 0) return null;

            switch (ham)
            {
                case HamTongHop.Tong:
                    {
                        double tong = 0;
                        bool co = false;
                        foreach (var v in giaTri)
                            if (ReportFormatters.LaSo(v, out double s)) { tong += s; co = true; }
                        return co ? tong : null;
                    }
                case HamTongHop.TrungBinh:
                    {
                        double tong = 0;
                        int dem = 0;
                        foreach (var v in giaTri)
                            if (ReportFormatters.LaSo(v, out double s)) { tong += s; dem++; }
                        return dem > 0 ? tong / dem : null;
                    }
                case HamTongHop.NhoNhat:
                case HamTongHop.LonNhat:
                    {
                        var soDuoc = giaTri.Where(v => ReportFormatters.LaSo(v, out _)).ToList();
                        if (soDuoc.Count > 0)
                        {
                            double ket = soDuoc
                                .Select(v => { ReportFormatters.LaSo(v, out double s); return s; })
                                .Aggregate(ham == HamTongHop.NhoNhat ? Math.Min : Math.Max);
                            return ket;
                        }

                        var ngayDuoc = giaTri.Where(v => ReportFormatters.LaNgay(v, out _)).ToList();
                        if (ngayDuoc.Count > 0)
                        {
                            DateTime ket = ngayDuoc
                                .Select(v => { ReportFormatters.LaNgay(v, out DateTime d); return d; })
                                .Aggregate(ham == HamTongHop.NhoNhat ? (a, b) => a < b ? a : b : (a, b) => a > b ? a : b);
                            return ket;
                        }

                        var chuoi = giaTri.Select(Convert.ToString).Where(s => s != null).Cast<string>().ToList();
                        if (chuoi.Count > 0)
                            return ham == HamTongHop.NhoNhat
                                ? chuoi.OrderBy(s => s, StringComparer.OrdinalIgnoreCase).First()
                                : chuoi.OrderByDescending(s => s, StringComparer.OrdinalIgnoreCase).First();
                        return null;
                    }
            }

            return null;
        }
    }
}
