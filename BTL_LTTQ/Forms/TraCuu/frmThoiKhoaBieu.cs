
using BTL_LTTQ.BUS;
using BTL_LTTQ.Common;
using BTL_LTTQ.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.TraCuu
{
    // Xem thời khóa biểu nhiều tuần liên tiếp và danh sách buổi học chi tiết
    public partial class frmThoiKhoaBieu : Form, IXuatExcel
    {
        private static readonly string[] TenCacThu = ThuTrongTuanConst.CacThu
            .Select(ThuTrongTuanConst.TenHienThi).ToArray();

        private const int SoTuanMacDinh = 1;

        // Kích thước giao diện DataGridView
        private const int ChieuCaoDong = 65;
        private const int ChieuCaoHeader = 55;

        private readonly ThoiKhoaBieuService _service = new ThoiKhoaBieuService();

        private string? _maGV;
        private string? _maHocVien;

        private List<BuoiHoc> _tatCaBuoi = new List<BuoiHoc>();

        // Ngày Thứ 2 của tuần đang xem
        private DateTime _tuanBatDau = TinhThuHai(DateTime.Today);
        private int _soTuan = SoTuanMacDinh;
        private bool _dangCapNhatLich;

        public DataGridView LuoiXuat => dgvChiTiet;
        public string TieuDeXuat => "Thời khóa biểu";

        public frmThoiKhoaBieu()
        {
            InitializeComponent();
            GiaoDien.ApDung(this);
        }

        // Thứ 2 của tuần chứa ngày truyền vào
        private static DateTime TinhThuHai(DateTime ngay)
        {
            int lech = ThuTrongTuanConst.TuNgay(ngay) - ThuTrongTuanConst.ThuHai;
            return ngay.Date.AddDays(-lech);
        }

        private void frmThoiKhoaBieu_Load(object sender, EventArgs e)
        {
            if (!PermissionService.Can(ChucNangConst.THOIKHOA_XEM))
            {
                MessageBox.Show(
                    "Bạn không có quyền xem thời khóa biểu.",
                    "Không có quyền",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                lblMoTa.Text = "Bạn không có quyền xem thời khóa biểu.";
                return;
            }

            // Giáo viên và học viên chỉ thấy lịch của mình,
            // Admin thấy tất cả.
            if (CurrentUser.IsGiaoVien)
                _maGV = CurrentUser.MaGV;
            else if (CurrentUser.IsHocVien)
                _maHocVien = CurrentUser.MaHocVien;

            _soTuan = SoTuanMacDinh;

            // Nới panel điều hướng đủ chỗ chứa MonthCalendar
            panelDieuHuong.Height = Math.Max(
                panelDieuHuong.Height,
                calThang.Bottom + panelDieuHuong.Padding.Bottom + 4);

            NapLich();
            NhayVeTuanGanNhat();
        }

        // =====================================================
        // NẠP DỮ LIỆU
        // =====================================================

        private void NapLich()
        {
            _tatCaBuoi = _service.LayLich(_maGV, _maHocVien);

            lblMoTa.Text = TaoMoTa(_tatCaBuoi);

            dgvChiTiet.DataSource = TaoBangChiTiet(_tatCaBuoi);

            DinhDangChiTiet();

            VeLichTuan();
        }

        private string TaoMoTa(List<BuoiHoc> ds)
        {
            string phamVi = CurrentUser.IsGiaoVien
                ? "Lịch giảng dạy của bạn"
                : CurrentUser.IsHocVien
                    ? "Lịch học của bạn"
                    : "Toàn bộ thời khóa biểu";

            if (ds.Count == 0)
            {
                return CurrentUser.IsAdmin
                    ? phamVi + "  Chưa có lịch. Lịch được tạo tự động khi học viên đăng ký lớp."
                    : phamVi + " • Bạn chưa có buổi học nào.";
            }

            return $"{phamVi} • {ds.Count} buổi • " +
                   $"{ds.Min(b => b.Ngay):dd/MM/yyyy} - {ds.Max(b => b.Ngay):dd/MM/yyyy}";
        }

        private static DataTable TaoBangChiTiet(List<BuoiHoc> ds)
        {
            var dt = new DataTable();

            dt.Columns.Add("Ngay", typeof(DateTime));
            dt.Columns.Add("Thu", typeof(string));
            dt.Columns.Add("Ca", typeof(string));
            dt.Columns.Add("Gio", typeof(string));
            dt.Columns.Add("MaLop", typeof(string));
            dt.Columns.Add("TenLop", typeof(string));
            dt.Columns.Add("MonHoc", typeof(string));
            dt.Columns.Add("GiaoVien", typeof(string));
            dt.Columns.Add("Phong", typeof(string));

            foreach (BuoiHoc buoi in ds)
            {
                dt.Rows.Add(
                    buoi.Ngay,
                    buoi.TenThu,
                    buoi.TenCa,
                    buoi.KhungGio,
                    buoi.MaLop,
                    buoi.TenLop,
                    buoi.TenMon,
                    buoi.TenGV,
                    buoi.TenPhong);
            }

            return dt;
        }

        // =====================================================
        // TẠO BẢNG THỜI KHÓA BIỂU THEO TUẦN
        // =====================================================

        private DataTable TaoBangTuan(
            List<BuoiHoc> ds,
            DateTime tuanBatDau,
            int soTuan)
        {
            var dt = new DataTable();

            dt.Columns.Add("Tuan", typeof(string));
            dt.Columns.Add("Ca", typeof(string));

            for (int t = 0; t < TenCacThu.Length; t++)
                dt.Columns.Add("Thu" + t, typeof(string));

            if (soTuan <= 0)
                return dt;

            // Danh sách ca lấy từ toàn bộ lịch
            List<BuoiHoc> dsCa = ds
                .GroupBy(b => new
                {
                    b.MaCa,
                    b.TenCa,
                    b.KhungGio,
                    Gio = b.GioBatDau ?? TimeSpan.MaxValue
                })
                .OrderBy(g => g.Key.Gio)
                .ThenBy(g => g.Key.TenCa)
                .Select(g => g.First())
                .ToList();

            if (dsCa.Count == 0)
                return dt;

            for (int i = 0; i < soTuan; i++)
            {
                DateTime dauTuan = tuanBatDau.AddDays(i * 7);
                DateTime cuoiTuan = dauTuan.AddDays(6);

                // Lọc các buổi học trong tuần
                List<BuoiHoc> buoiTrongTuan = ds
                    .Where(b =>
                        b.Ngay.Date >= dauTuan &&
                        b.Ngay.Date <= cuoiTuan)
                    .ToList();

                bool daGanNhanTuan = false;

                foreach (BuoiHoc ca in dsCa)
                {
                    DataRow dong = dt.NewRow();

                    dong["Tuan"] = daGanNhanTuan
                        ? ""
                        : TaoNhanTuan(dauTuan, cuoiTuan);

                    dong["Ca"] = TaoNhanCa(ca);

                    for (int t = 0; t < TenCacThu.Length; t++)
                    {
                        DateTime ngay = dauTuan.AddDays(t);

                        var noiDung = buoiTrongTuan
                            .Where(b =>
                                b.Ngay.Date == ngay &&
                                b.MaCa == ca.MaCa)
                            .GroupBy(b => new
                            {
                                b.MaLop,
                                b.MaPhong,
                                b.MaGV
                            })
                            .Select(g => TaoNoiDungO(g.First()))
                            .ToList();

                        dong["Thu" + t] = string.Join("\n", noiDung);
                    }

                    dt.Rows.Add(dong);
                    daGanNhanTuan = true;
                }
            }

            return dt;
        }

        private static string TaoNhanTuan(
            DateTime dauTuan,
            DateTime cuoiTuan)
        {
            return $"Tuần {ISOWeek.GetWeekOfYear(dauTuan)}\n" +
                   $"{dauTuan:dd/MM} - {cuoiTuan:dd/MM}";
        }

        // Tên ca đã có sẵn giờ thì không chèn thêm khung giờ
        private static string TaoNhanCa(BuoiHoc ca)
        {
            string tenCa = string.IsNullOrWhiteSpace(ca.TenCa)
                ? ca.MaCa
                : ca.TenCa;

            if (string.IsNullOrWhiteSpace(ca.KhungGio) ||
                tenCa.Any(char.IsDigit))
            {
                return tenCa;
            }

            return $"{tenCa}\n({ca.KhungGio})";
        }

        private string TaoNoiDungO(BuoiHoc buoi)
        {
            string dongDau = string.IsNullOrWhiteSpace(buoi.TenMon)
                ? buoi.MaLop
                : $"{buoi.MaLop} - {buoi.TenMon}";

            string dongSau = $"{buoi.TenPhong} • {buoi.Ngay:dd/MM}";

            if (CurrentUser.IsAdmin)
                dongSau += " • " + buoi.TenGV;

            return dongDau + "\n" + dongSau;
        }

        // =====================================================
        // ĐỊNH DẠNG BẢNG CHI TIẾT
        // =====================================================

        private void DinhDangChiTiet()
        {
            if (dgvChiTiet.Columns.Count == 0)
                return;

            dgvChiTiet.Columns["Ngay"].HeaderText = "Ngày";
            dgvChiTiet.Columns["Thu"].HeaderText = "Thứ";
            dgvChiTiet.Columns["Ca"].HeaderText = "Ca học";
            dgvChiTiet.Columns["Gio"].HeaderText = "Giờ";
            dgvChiTiet.Columns["MaLop"].HeaderText = "Mã lớp";
            dgvChiTiet.Columns["TenLop"].HeaderText = "Tên lớp";
            dgvChiTiet.Columns["MonHoc"].HeaderText = "Môn học";
            dgvChiTiet.Columns["GiaoVien"].HeaderText = "Giáo viên";
            dgvChiTiet.Columns["Phong"].HeaderText = "Phòng học";

            dgvChiTiet.Columns["Ngay"].DefaultCellStyle.Format = "dd/MM/yyyy";

            dgvChiTiet.Columns["Ngay"].DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            dgvChiTiet.Columns["Thu"].DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
        }

        // =====================================================
        // ĐỊNH DẠNG BẢNG THỜI KHÓA BIỂU
        // =====================================================

        private void DinhDangTuan()
        {
            // Cấu hình cơ bản
            dgvTuan.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvTuan.DefaultCellStyle.WrapMode =
                DataGridViewTriState.True;

            // Không cho DataGridView tự động thay đổi chiều cao dòng
            dgvTuan.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.None;

            // Thiết lập chiều cao dòng mặc định
            dgvTuan.RowTemplate.Height = ChieuCaoDong;

            // Áp dụng cho tất cả dòng dữ liệu hiện tại
            foreach (DataGridViewRow row in dgvTuan.Rows)
            {
                if (!row.IsNewRow)
                {
                    row.Height = ChieuCaoDong;
                }
            }

            // ==============================
            // CẤU HÌNH HEADER
            // ==============================

            dgvTuan.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgvTuan.ColumnHeadersHeight = ChieuCaoHeader;

            dgvTuan.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            dgvTuan.ColumnHeadersDefaultCellStyle.WrapMode =
                DataGridViewTriState.True;

            if (dgvTuan.Columns.Count == 0)
                return;

            // ==============================
            // CỘT TUẦN
            // ==============================

            if (dgvTuan.Columns.Contains("Tuan"))
            {
                dgvTuan.Columns["Tuan"].HeaderText = "Tuần";

                dgvTuan.Columns["Tuan"].FillWeight = 70;

                dgvTuan.Columns["Tuan"].Frozen = true;

                dgvTuan.Columns["Tuan"].DefaultCellStyle.Font =
                    new Font(dgvTuan.Font, FontStyle.Bold);
            }

            // ==============================
            // CỘT CA
            // ==============================

            if (dgvTuan.Columns.Contains("Ca"))
            {
                dgvTuan.Columns["Ca"].HeaderText = "Ca";

                dgvTuan.Columns["Ca"].FillWeight = 90;

                dgvTuan.Columns["Ca"].Frozen = true;

                dgvTuan.Columns["Ca"].DefaultCellStyle.Font =
                    new Font(dgvTuan.Font, FontStyle.Bold);
            }

            // ==============================
            // CÁC CỘT THỨ TRONG TUẦN
            // ==============================

            for (int t = 0; t < TenCacThu.Length; t++)
            {
                string col = "Thu" + t;

                if (!dgvTuan.Columns.Contains(col))
                    continue;

                DateTime ngay = _tuanBatDau.AddDays(t);

                dgvTuan.Columns[col].HeaderText =
                    $"{TenCacThu[t]}\n{ngay:dd/MM}";

                dgvTuan.Columns[col].FillWeight = 130;
            }

            // ==============================
            // ĐỊNH DẠNG TẤT CẢ CỘT
            // ==============================

            foreach (DataGridViewColumn cot in dgvTuan.Columns)
            {
                cot.SortMode =
                    DataGridViewColumnSortMode.NotSortable;

                cot.DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;
            }
        }

        // =====================================================
        // VẼ LỊCH TUẦN
        // =====================================================

        private void VeLichTuan()
        {
            _soTuan = 1;

            dgvTuan.DataSource = TaoBangTuan(
                _tatCaBuoi,
                _tuanBatDau,
                _soTuan);

            // Cấu hình lại giao diện sau mỗi lần nạp dữ liệu
            DinhDangTuan();

            CapNhatKhoang();
        }

        private void CapNhatKhoang()
        {
            DateTime cuoi = _tuanBatDau.AddDays(6);

            int soBuoi = _tatCaBuoi.Count(b =>
                b.Ngay.Date >= _tuanBatDau &&
                b.Ngay.Date <= cuoi);

            string thongTin = soBuoi > 0
                ? $"{soBuoi} buổi học trong tuần."
                : "Không có buổi học trong tuần này.";

            int tuanISO = ISOWeek.GetWeekOfYear(_tuanBatDau);

            lblKhoang.Text =
                $"Tuần {tuanISO}: {_tuanBatDau:dd/MM/yyyy} - {cuoi:dd/MM/yyyy}\r\n" +
                $"{thongTin}";
        }

        // =====================================================
        // ĐỒNG BỘ CALENDAR
        // =====================================================

        private void DongBoLich()
        {
            _dangCapNhatLich = true;

            try
            {
                DateTime ngay = _tuanBatDau;

                if (ngay < calThang.MinDate)
                    ngay = calThang.MinDate;

                if (ngay > calThang.MaxDate)
                    ngay = calThang.MaxDate;

                calThang.SetDate(ngay);
            }
            finally
            {
                _dangCapNhatLich = false;
            }

            VeLichTuan();
        }

        // Chọn tuần hiển thị ban đầu:
        // Tuần hiện tại nếu lịch bao quanh hôm nay,
        // ngược lại là tuần của buổi học gần nhất.
        private void NhayVeTuanGanNhat()
        {
            DateTime moc = DateTime.Today;

            if (_tatCaBuoi.Count > 0)
            {
                DateTime somNhat =
                    _tatCaBuoi.Min(b => b.Ngay.Date);

                DateTime muonNhat =
                    _tatCaBuoi.Max(b => b.Ngay.Date);

                if (moc < somNhat || moc > muonNhat)
                {
                    moc = _tatCaBuoi
                        .OrderBy(b =>
                            Math.Abs((b.Ngay.Date - DateTime.Today).Days))
                        .First()
                        .Ngay;
                }
            }

            _tuanBatDau = TinhThuHai(moc);

            DongBoLich();
        }

        // Sau khi xếp lịch: nhảy về tuần có buổi học sớm nhất
        private void NhayVeTuanDauTien()
        {
            DateTime moc = _tatCaBuoi.Count > 0
                ? _tatCaBuoi.Min(b => b.Ngay.Date)
                : DateTime.Today;

            _tuanBatDau = TinhThuHai(moc);

            DongBoLich();
        }

        // =====================================================
        // CÁC SỰ KIỆN
        // =====================================================

        private void calThang_DateChanged(
            object sender,
            DateRangeEventArgs e)
        {
            if (_dangCapNhatLich)
                return;

            DateTime thuHai = TinhThuHai(e.Start);

            if (thuHai == _tuanBatDau)
                return;

            _tuanBatDau = thuHai;

            VeLichTuan();
        }

        private void btnTuanTruoc_Click(object sender, EventArgs e)
        {
            _tuanBatDau = _tuanBatDau.AddDays(-7);

            DongBoLich();
        }

        private void btnTuanNay_Click(object sender, EventArgs e)
        {
            _tuanBatDau = TinhThuHai(DateTime.Today);

            DongBoLich();
        }

        private void btnTuanSau_Click(object sender, EventArgs e)
        {
            _tuanBatDau = _tuanBatDau.AddDays(7);

            DongBoLich();
        }

        private void numSoTuan_ValueChanged(object sender, EventArgs e)
        {
            // Đã bỏ chọn số tuần hiển thị; mặc định 1 tuần.
        }
    }
}
