using BTL_LTTQ.BUS.Reports;
using BTL_LTTQ.Common;
using BTL_LTTQ.Common.Reports;
using BTL_LTTQ.Forms.Reports.Controls;
using BTL_LTTQ.Models.Reports;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.Reports.Screens
{
    /// <summary>
    /// Màn hình đầu: thống kê nhanh, danh sách báo cáo đã lưu và mẫu dựng sẵn.
    /// Là điểm xuất phát để tạo báo cáo mới hoặc mở báo cáo đã có.
    /// </summary>
    public sealed class UcReportDashboard : ReportScreenBase
    {
        private readonly TextBox _txtTim;
        private readonly DataGridView _luoi;
        private readonly UcStateOverlay _overlay;
        private readonly FlowLayoutPanel _theMau;
        private readonly List<ReportListItem> _danhSach = new();
        private readonly bool _chiCuaMinh;

        public UcReportDashboard(ReportEditorState trangThai, ReportDefinitionService dichVu)
            : base(trangThai, dichVu)
        {
            _chiCuaMinh = PermissionService.ChiCuaMinh(ChucNangConst.BAOCAO_TAO);

            var goc = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                BackColor = ReportDesignTokens.NenPanel,
                Padding = new Padding(ReportDesignTokens.Khoang20)
            };
            goc.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            goc.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            goc.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            goc.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // ----- Tiêu đề -----
            var dau = new Panel { AutoSize = true, Dock = DockStyle.Fill, Margin = new Padding(0, 0, 0, ReportDesignTokens.Khoang16) };
            var tieuDe = ReportUiFactory.TieuDeTrang("Trình tạo báo cáo");
            tieuDe.Location = new Point(0, 0);
            var phuDe = ReportUiFactory.PhuDeTrang("Tự dựng báo cáo từ dữ liệu có sẵn, không cần viết truy vấn.");
            phuDe.Location = new Point(2, 34);
            dau.Controls.Add(phuDe);
            dau.Controls.Add(tieuDe);
            dau.Height = 62;

            // ----- Thẻ thống kê -----
            var theThongKe = new FlowLayoutPanel
            {
                AutoSize = true,
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Margin = new Padding(0, 0, 0, ReportDesignTokens.Khoang16)
            };

            // ----- Thân: danh sách + mẫu -----
            var than = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = ReportDesignTokens.NenPanel
            };
            than.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62F));
            than.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38F));

            // Danh sách báo cáo đã lưu
            var theDs = ReportUiFactory.The("Báo cáo đã lưu", "Mở lại hoặc xóa báo cáo");
            theDs.Dock = DockStyle.Fill;
            theDs.Margin = new Padding(0, 0, ReportDesignTokens.Khoang12, 0);

            _txtTim = ReportUiFactory.OTimKiem("Tìm theo tên báo cáo…", 240);
            _txtTim.TextChanged += (_, _) => NapDanhSach();

            var nutTai = ReportUiFactory.Nut("↻ Tải lại", KieuNut.Phu, 96);
            nutTai.Click += (_, _) => NapDanhSach();

            var thanh = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(0, 0, 0, ReportDesignTokens.Khoang8)
            };
            thanh.Controls.Add(_txtTim);
            thanh.Controls.Add(nutTai);

            _luoi = ReportUiFactory.Luoi();
            _luoi.Columns.Add("Ten", "Tên báo cáo");
            _luoi.Columns.Add("Nguon", "Nguồn dữ liệu");
            _luoi.Columns.Add("Cot", "Cột");
            _luoi.Columns.Add("Loc", "Bộ lọc");
            _luoi.Columns.Add("NguoiTao", "Người tạo");
            _luoi.Columns.Add("CapNhat", "Cập nhật");
            _luoi.Columns["Ten"]!.FillWeight = 220;
            _luoi.Columns["Nguon"]!.FillWeight = 130;
            _luoi.Columns["Cot"]!.FillWeight = 50;
            _luoi.Columns["Loc"]!.FillWeight = 50;
            _luoi.Columns["NguoiTao"]!.FillWeight = 100;
            _luoi.Columns["CapNhat"]!.FillWeight = 110;
            _luoi.CellDoubleClick += (_, e) => { if (e.RowIndex >= 0) MoBaoCao(); };

            var nutMo = ReportUiFactory.Nut("Mở báo cáo", KieuNut.Chinh, 118);
            nutMo.Click += (_, _) => MoBaoCao();
            var nutNhanBan = ReportUiFactory.Nut("Nhân bản", KieuNut.Phu, 96);
            nutNhanBan.Click += (_, _) => NhanBanBaoCao();
            var nutXoa = ReportUiFactory.Nut("Xóa", KieuNut.NguyHiem, 80);
            nutXoa.Click += (_, _) => XoaBaoCao();
            var nutMoi = ReportUiFactory.Nut("＋ Tạo báo cáo mới", KieuNut.Phu, 156);
            nutMoi.Click += (_, _) => Chuyen(BuocBaoCao.NguonDuLieu);

            var nutCuoi = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(0, ReportDesignTokens.Khoang8, 0, 0)
            };
            nutCuoi.Controls.Add(nutMo);
            nutCuoi.Controls.Add(nutNhanBan);
            nutCuoi.Controls.Add(nutXoa);
            nutCuoi.Controls.Add(nutMoi);

            var khungLuoi = new Panel { Dock = DockStyle.Fill, BackColor = ReportDesignTokens.NenTrang };
            _overlay = new UcStateOverlay();
            khungLuoi.Controls.Add(_overlay);
            khungLuoi.Controls.Add(_luoi);

            theDs.Than.Controls.Add(khungLuoi);
            theDs.Than.Controls.Add(nutCuoi);
            theDs.Than.Controls.Add(thanh);

            // Mẫu dựng sẵn
            var theMauCard = ReportUiFactory.The("Mẫu báo cáo dựng sẵn", "Chọn nhanh một cấu hình gợi ý");
            theMauCard.Dock = DockStyle.Fill;
            theMauCard.Margin = new Padding(ReportDesignTokens.Khoang12, 0, 0, 0);

            _theMau = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = ReportDesignTokens.NenTrang
            };
            _theMau.Resize += (_, _) => CanChinhTheMau();
            theMauCard.Than.Controls.Add(_theMau);

            than.Controls.Add(theDs, 0, 0);
            than.Controls.Add(theMauCard, 1, 0);

            goc.Controls.Add(dau, 0, 0);
            goc.Controls.Add(theThongKe, 0, 1);
            goc.Controls.Add(than, 0, 2);
            Controls.Add(goc);

            DungTheThongKe(theThongKe);
            DungTheMau();
        }

        public override void Nap()
        {
            NapDanhSach();
        }

        private void DungTheThongKe(FlowLayoutPanel cha)
        {
            int soBaoCao = DichVu.Dem(TrangThai.NguoiDung, _chiCuaMinh);
            cha.Controls.Add(TheThongKe("📊", soBaoCao.ToString("N0"), "Báo cáo khả dụng"));
            cha.Controls.Add(TheThongKe("🗄️", DanhMuc.NguonDuLieu.Count.ToString(), "Nguồn dữ liệu"));
            cha.Controls.Add(TheThongKe("🧾", DanhMuc.TongSoTruong.ToString("N0"), "Trường dữ liệu"));
            cha.Controls.Add(TheThongKe("🧩", DanhMuc.MauDungSan.Count.ToString(), "Mẫu dựng sẵn"));
        }

        private static Control TheThongKe(string bieuTuong, string giaTri, string nhan)
        {
            var card = ReportUiFactory.The();
            card.AnHeader();
            card.Size = new Size(200, 94);
            card.Margin = new Padding(0, 0, ReportDesignTokens.Khoang12, ReportDesignTokens.Khoang12);

            var icon = new Label
            {
                Text = bieuTuong,
                Font = new Font("Segoe UI Emoji", 20F),
                AutoSize = true,
                Location = new Point(ReportDesignTokens.Khoang12, ReportDesignTokens.Khoang12)
            };
            var so = new Label
            {
                Text = giaTri,
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = ReportDesignTokens.Nhan,
                AutoSize = true,
                Location = new Point(ReportDesignTokens.Khoang12, 14)
            };
            var cap = new Label
            {
                Text = nhan,
                Font = ReportDesignTokens.ChuThuong,
                ForeColor = ReportDesignTokens.ChuPhu,
                AutoSize = true,
                Location = new Point(ReportDesignTokens.Khoang12, 56)
            };

            card.Than.Padding = new Padding(0);
            card.Than.Controls.Add(cap);
            card.Than.Controls.Add(so);
            card.Than.Controls.Add(icon);
            icon.Left = 150;
            icon.Top = 10;
            return card;
        }

        private void DungTheMau()
        {
            _theMau.Controls.Clear();
            foreach (var mau in DanhMuc.MauDungSan)
            {
                var the = new Panel
                {
                    Width = Math.Max(220, _theMau.ClientSize.Width - 26),
                    Height = 78,
                    BackColor = ReportDesignTokens.NenPanel,
                    Margin = new Padding(0, 0, 0, ReportDesignTokens.Khoang8),
                    Padding = new Padding(ReportDesignTokens.Khoang8)
                };

                var icon = new Label
                {
                    Text = mau.BieuTuong,
                    Font = new Font("Segoe UI Emoji", 15F),
                    AutoSize = true,
                    Location = new Point(ReportDesignTokens.Khoang8, 20)
                };
                var ten = new Label
                {
                    Text = mau.Ten,
                    Font = ReportDesignTokens.ChuTieuDeThe,
                    ForeColor = ReportDesignTokens.ChuChinh,
                    AutoSize = true,
                    Location = new Point(48, 12)
                };
                var moTa = new Label
                {
                    Text = mau.MoTa,
                    Font = ReportDesignTokens.ChuNho,
                    ForeColor = ReportDesignTokens.ChuPhu,
                    AutoSize = false,
                    Size = new Size(220, 32),
                    Location = new Point(48, 34)
                };
                var nut = ReportUiFactory.Nut("Dùng mẫu", KieuNut.Phu, 88, 28);
                nut.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                nut.Location = new Point(the.Width - 96, 24);
                nut.Click += (_, _) => DungMau(mau);

                the.Controls.Add(nut);
                the.Controls.Add(moTa);
                the.Controls.Add(ten);
                the.Controls.Add(icon);
                _theMau.Controls.Add(the);
            }
        }

        private void CanChinhTheMau()
        {
            int rong = Math.Max(220, _theMau.ClientSize.Width - 26);
            foreach (Control c in _theMau.Controls)
            {
                c.Width = rong;
                if (c.Controls["btnDungMau"] is Control nut)
                    nut.Left = rong - 96;
                else if (c.Controls.Count > 0 && c.Controls[0] is AppButton b)
                    b.Left = rong - 96;
            }
        }

        private void NapDanhSach()
        {
            _danhSach.Clear();
            _luoi.Rows.Clear();

            try
            {
                _danhSach.AddRange(DichVu.DanhSach(_txtTim.Text.Trim(), _chiCuaMinh, TrangThai.NguoiDung));
            }
            catch (Exception ex)
            {
                _overlay.Hien(TrangThaiXemTruoc.Loi, "Không tải được danh sách",
                    "Chi tiết: " + ex.Message, "Thử lại", NapDanhSach);
                return;
            }

            foreach (var item in _danhSach)
            {
                int i = _luoi.Rows.Add(
                    item.Ten,
                    item.TenNguon,
                    item.SoCot.ToString(),
                    item.SoBoLoc.ToString(),
                    item.NguoiTao,
                    item.NgayCapNhat == DateTime.MinValue ? "—" : item.NgayCapNhat.ToString("dd/MM/yyyy HH:mm"));
                _luoi.Rows[i].Tag = item;
            }

            if (_danhSach.Count == 0)
            {
                _overlay.Hien(TrangThaiXemTruoc.Trong,
                    string.IsNullOrWhiteSpace(_txtTim.Text) ? "Chưa có báo cáo nào" : "Không tìm thấy báo cáo",
                    "Nhấn “Tạo báo cáo mới” hoặc chọn một mẫu dựng sẵn để bắt đầu.",
                    "Tạo báo cáo mới", () => Chuyen(BuocBaoCao.NguonDuLieu));
            }
            else
            {
                _overlay.An();
            }
        }

        private ReportListItem? DongChon()
        {
            int idx = _luoi.CurrentRow?.Index ?? -1;
            return idx >= 0 && idx < _danhSach.Count ? _danhSach[idx] : null;
        }

        private void MoBaoCao()
        {
            var item = DongChon();
            if (item == null)
            {
                HienThongBao("Vui lòng chọn một báo cáo để mở.", "Chưa chọn báo cáo", MessageBoxIcon.Information);
                return;
            }

            var dinhNghia = DichVu.Lay(item.MaBaoCao);
            if (dinhNghia == null)
            {
                HienThongBao("Không đọc được cấu hình báo cáo này.", "Lỗi", MessageBoxIcon.Error);
                return;
            }

            TrangThai.DatDinhNghia(dinhNghia);
            Chuyen(BuocBaoCao.CauHinh);
        }

        private void NhanBanBaoCao()
        {
            var item = DongChon();
            if (item == null)
            {
                HienThongBao("Vui lòng chọn một báo cáo để nhân bản.", "Chưa chọn báo cáo", MessageBoxIcon.Information);
                return;
            }

            var dinhNghia = DichVu.Lay(item.MaBaoCao);
            if (dinhNghia == null)
            {
                HienThongBao("Không đọc được cấu hình báo cáo này.", "Lỗi", MessageBoxIcon.Error);
                return;
            }

            var banSao = dinhNghia.Clone();
            banSao.MaBaoCao = 0;
            banSao.Ten = $"{dinhNghia.Ten} (Bản sao)";
            banSao.NguoiTao = TrangThai.NguoiDung;
            banSao.NgayTao = DateTime.Now;
            banSao.NgayCapNhat = DateTime.Now;

            TrangThai.DatDinhNghia(banSao);
            Chuyen(BuocBaoCao.CauHinh);
        }

        private void XoaBaoCao()
        {
            var item = DongChon();
            if (item == null)
            {
                HienThongBao("Vui lòng chọn một báo cáo để xóa.", "Chưa chọn báo cáo", MessageBoxIcon.Information);
                return;
            }

            var traLoi = MessageBox.Show($"Bạn có chắc muốn xóa báo cáo “{item.Ten}”?\nThao tác này không thể hoàn tác.",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (traLoi != DialogResult.Yes)
                return;

            if (DichVu.Xoa(item.MaBaoCao, out string loi))
            {
                NapDanhSach();
                HienThongBao("Đã xóa báo cáo.", "Thành công", MessageBoxIcon.Information);
            }
            else
            {
                HienThongBao(loi, "Không xóa được", MessageBoxIcon.Error);
            }
        }

        private void DungMau(ReportTemplate mau)
        {
            var dinhNghia = DichVu.TaoTuMau(mau);
            TrangThai.DatDinhNghia(dinhNghia, true);
            Chuyen(BuocBaoCao.CauHinh);
        }
    }
}
