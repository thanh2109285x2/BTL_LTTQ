using BTL_LTTQ.Common;
using BTL_LTTQ.Models;
using BTL_LTTQ.Repositories;
using Google.OrTools.Sat;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BTL_LTTQ.Services
{
    //Xếp thời khóa biểu tự động bằng Google OR-Tools (CP-SAT)
    public class XepLichService
    {
        private const int SoNgayTrongTuan = 7;

        //Trọng số phạt cho ràng buộc mềm (càng lớn càng ưu tiên tránh)
        private const int PhatLechThu = 4;
        private const int PhatLechCa = 3;
        private const int PhatLechPhong = 1;
        private const int PhatCaMuon = 1;
        private const int PhatNgayLienKe = 10;

        private readonly ThoiKhoaBieuRepository _repo = new ThoiKhoaBieuRepository();

        public KetQuaXepLich XepVaLuu()
        {
            if (!PermissionService.Can(ChucNangConst.THOIKHOA_XEP))
                return KetQuaXepLich.Loi("Bạn không có quyền tạo thời khóa biểu.");

            return XepVaLuu(_repo.LayDuLieuXepLich());
        }

        public KetQuaXepLich XepVaLuu(DuLieuXepLich duLieu)
        {
            var lopCanXep = duLieu.Lop.Where(l => l.SiSo > 0).ToList();
            int soLopBoQua = duLieu.Lop.Count - lopCanXep.Count;

            if (lopCanXep.Count == 0)
                return KetQuaXepLich.Loi("Chưa có lớp nào có học viên đăng ký nên không có gì để xếp.");
            if (duLieu.Ca.Count == 0)
                return KetQuaXepLich.Loi("Hệ thống chưa có ca học nào.");
            if (duLieu.Phong.Count == 0)
                return KetQuaXepLich.Loi("Hệ thống chưa có phòng học nào.");

            //Kiểm tra trước để báo lỗi rõ ràng thay vì để solver trả "không tìm được lịch"
            var loi = new List<string>();
            foreach (LopXepLich lop in lopCanXep)
            {
                if (lop.SoBuoiTuan < 1 || lop.SoBuoiTuan > SoNgayTrongTuan)
                    loi.Add($"{lop.MaLop}: số buổi/tuần phải từ 1 đến {SoNgayTrongTuan}");

                if (!duLieu.Phong.Any(p => p.SucChua >= lop.SiSo))
                    loi.Add($"{lop.MaLop}: sĩ số {lop.SiSo} nhưng không phòng nào đủ chỗ");

                if (lop.NgayKT.Date < lop.NgayBD.Date)
                    loi.Add($"{lop.MaLop}: ngày kết thúc trước ngày bắt đầu");

                if (string.IsNullOrWhiteSpace(lop.MaGV))
                    loi.Add($"{lop.MaLop}: chưa gán giáo viên");
            }

            if (loi.Count > 0)
                return KetQuaXepLich.Loi(string.Join("\n", loi));

            var model = new CpModel();
            var bien = new List<BienXep>();
            var chiPhi = new List<IntVar>();
            var trongSo = new List<long>();

            var theoBuoi = new Dictionary<(int Lop, int Buoi), List<IntVar>>();
            var theoPhong = new Dictionary<(int Ngay, int Ca, int Phong), List<IntVar>>();
            var theoGiaoVien = new Dictionary<(string MaGV, int Ngay, int Ca), List<IntVar>>();
            var theoNgayCuaLop = new Dictionary<(int Lop, int Ngay), List<IntVar>>();
            var theoLopNgayCa = new Dictionary<(int Lop, int Ngay, int Ca), List<IntVar>>();
            var lopCuaHocVien = new Dictionary<string, HashSet<int>>(StringComparer.OrdinalIgnoreCase);

            for (int c = 0; c < lopCanXep.Count; c++)
            {
                LopXepLich lop = lopCanXep[c];
                foreach (string maHocVien in lop.HocVien)
                {
                    if (!lopCuaHocVien.TryGetValue(maHocVien, out var dsLop))
                    {
                        dsLop = new HashSet<int>();
                        lopCuaHocVien[maHocVien] = dsLop;
                    }
                    dsLop.Add(c);
                }
            }

            //Tạo biến quyết định: buổi s của lớp c học vào thứ d, ca k, phòng r
            for (int c = 0; c < lopCanXep.Count; c++)
            {
                LopXepLich lop = lopCanXep[c];
                for (int s = 0; s < lop.SoBuoiTuan; s++)
                {
                    for (int d = 0; d < SoNgayTrongTuan; d++)
                    {
                        for (int k = 0; k < duLieu.Ca.Count; k++)
                        {
                            for (int r = 0; r < duLieu.Phong.Count; r++)
                            {
                                PhongXepLich phong = duLieu.Phong[r];
                                if (phong.SucChua < lop.SiSo) continue; //phòng không đủ chỗ

                                IntVar v = model.NewBoolVar($"x_{c}_{s}_{d}_{k}_{r}");
                                bien.Add(new BienXep(v, c, s, d, k, r));

                                Them(theoBuoi, (c, s), v);
                                Them(theoPhong, (d, k, r), v);
                                Them(theoGiaoVien, (lop.MaGV, d, k), v);
                                Them(theoNgayCuaLop, (c, d), v);
                                Them(theoLopNgayCa, (c, d, k), v);

                                long phi = 0;
                                if (lop.ThuUuTien.Count > 0 && !lop.ThuUuTien.Contains(d))
                                    phi += PhatLechThu;
                                if (lop.MaCaUuTien != "" && duLieu.Ca[k] != lop.MaCaUuTien)
                                    phi += PhatLechCa;
                                if (lop.MaPhongUuTien != "" && phong.MaPhong != lop.MaPhongUuTien)
                                    phi += PhatLechPhong;
                                phi += k * PhatCaMuon;

                                chiPhi.Add(v);
                                trongSo.Add(phi);
                            }
                        }
                    }
                }
            }

            //Mỗi buổi phải được xếp đúng 1 chỗ
            foreach (var muc in theoBuoi.Values)
                model.Add(LinearExpr.Sum(muc) == 1);

            //1 phòng / 1 giáo viên chỉ dạy 1 lớp trong cùng (thứ, ca)
            foreach (var muc in theoPhong.Values)
                model.Add(LinearExpr.Sum(muc) <= 1);
            foreach (var muc in theoGiaoVien.Values)
                model.Add(LinearExpr.Sum(muc) <= 1);

            //1 lớp không học 2 buổi trong cùng 1 ngày
            foreach (var muc in theoNgayCuaLop.Values)
                model.Add(LinearExpr.Sum(muc) <= 1);

            //1 học viên không ở 2 lớp trong cùng (thứ, ca)
            foreach (var cap in lopCuaHocVien)
            {
                if (cap.Value.Count < 2) continue; //chỉ 1 lớp thì không thể trùng

                for (int d = 0; d < SoNgayTrongTuan; d++)
                {
                    for (int k = 0; k < duLieu.Ca.Count; k++)
                    {
                        var ds = new List<IntVar>();
                        foreach (int c in cap.Value)
                        {
                            if (theoLopNgayCa.TryGetValue((c, d, k), out var muc))
                                ds.AddRange(muc);
                        }

                        if (ds.Count > 1)
                            model.Add(LinearExpr.Sum(ds) <= 1);
                    }
                }
            }

            //Hạn chế 2 buổi liền ngày (mềm: vi phạm thì bị phạt)
            for (int c = 0; c < lopCanXep.Count; c++)
            {
                for (int d = 0; d < SoNgayTrongTuan - 1; d++)
                {
                    if (!theoNgayCuaLop.TryGetValue((c, d), out var homNay) ||
                        !theoNgayCuaLop.TryGetValue((c, d + 1), out var homSau))
                        continue;

                    IntVar viPham = model.NewBoolVar($"lienke_{c}_{d}");
                    model.Add(viPham >= LinearExpr.Sum(homNay) + LinearExpr.Sum(homSau) - 1);

                    chiPhi.Add(viPham);
                    trongSo.Add(PhatNgayLienKe);
                }
            }

            model.Minimize(LinearExpr.WeightedSum(chiPhi, trongSo));

            var solver = new CpSolver();
            solver.StringParameters = "max_time_in_seconds:20.0 num_search_workers:4 random_seed:42";
            CpSolverStatus trangThai = solver.Solve(model);

            if (trangThai != CpSolverStatus.Optimal && trangThai != CpSolverStatus.Feasible)
                return KetQuaXepLich.Loi(
                    "Không tìm được thời khóa biểu thỏa tất cả ràng buộc.\n" +
                    "Hãy thử: giảm số buổi/tuần, thêm phòng học, hoặc giảm số lớp mở trong cùng khung giờ.");

            var ketQua = new List<BuoiHoc>();

            foreach (BienXep b in bien)
            {
                if (solver.Value(b.Var) != 1) continue;

                LopXepLich lop = lopCanXep[b.Lop];
                PhongXepLich phong = duLieu.Phong[b.Phong];
                string maCa = duLieu.Ca[b.Ca];

                for (DateTime ngay = lop.NgayBD.Date; ngay <= lop.NgayKT.Date; ngay = ngay.AddDays(1))
                {
                    if (ChiSoThu(ngay) != b.Ngay) continue;

                    ketQua.Add(new BuoiHoc
                    {
                        MaLop = lop.MaLop,
                        MaGV = lop.MaGV,
                        Ngay = ngay,
                        MaCa = maCa,
                        MaPhong = phong.MaPhong
                    });
                }
            }

            if (ketQua.Count == 0)
                return KetQuaXepLich.Loi(
                    "Đã tìm được khung lịch nhưng khoảng ngày của các lớp quá ngắn nên không sinh được buổi học nào.");

            try
            {
                _repo.LuuLich(ketQua);
            }
            catch (SqlException ex)
            {
                return KetQuaXepLich.Loi("Không lưu được thời khóa biểu: " + ex.Message);
            }

            string thongBao = $"Đã xếp {ketQua.Count} buổi học cho {lopCanXep.Count} lớp.";
            if (soLopBoQua > 0)
                thongBao += $"\nBỏ qua {soLopBoQua} lớp chưa có học viên đăng ký.";

            return new KetQuaXepLich
            {
                ThanhCong = true,
                ThongBao = thongBao,
                SoLopDaXep = lopCanXep.Count,
                SoBuoi = ketQua.Count
            };
        }

        private static int ChiSoThu(DateTime ngay)
        {
            return ngay.DayOfWeek == DayOfWeek.Sunday ? 6 : (int)ngay.DayOfWeek - 1;
        }

        private static void Them<TKhoa>(Dictionary<TKhoa, List<IntVar>> dict, TKhoa khoa, IntVar bien)
            where TKhoa : notnull
        {
            if (!dict.TryGetValue(khoa, out var ds))
            {
                ds = new List<IntVar>();
                dict[khoa] = ds;
            }
            ds.Add(bien);
        }

        private sealed class BienXep
        {
            public BienXep(IntVar bien, int lop, int buoi, int ngay, int ca, int phong)
            {
                Var = bien;
                Lop = lop;
                Buoi = buoi;
                Ngay = ngay;
                Ca = ca;
                Phong = phong;
            }

            public IntVar Var { get; }
            public int Lop { get; }
            public int Buoi { get; }
            public int Ngay { get; }
            public int Ca { get; }
            public int Phong { get; }
        }
    }
}
