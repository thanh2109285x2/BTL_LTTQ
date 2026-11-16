using BTL_LTTQ.BUS.Reports;
using BTL_LTTQ.Common;
using BTL_LTTQ.Common.Reports;
using BTL_LTTQ.Models.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Microsoft.Reporting.WinForms;

namespace BTL_LTTQ.Forms.Reports
{
    public partial class frmReportBuilder : Form, IXuatExcel
    {
        private readonly ReportViewer _reportViewer;
        private readonly ReportCatalogService _danhMuc = new();
        private readonly ReportDataService _duLieu;

        // IXuatExcel interface cho nút xuất Excel ở thanh menu sidebar của frmMain
        public DataGridView LuoiXuat => dgvKetQua;
        public string TieuDeXuat => string.IsNullOrWhiteSpace(txtTenBaoCao.Text) ? "Báo cáo động" : txtTenBaoCao.Text.Trim();

        public frmReportBuilder()
        {
            InitializeComponent();
            _reportViewer = new ReportViewer
            {
                Dock = DockStyle.Fill,
                ProcessingMode = ProcessingMode.Local,
                ShowPrintButton = true,
                ShowExportButton = true
            };
            grpKetQua.Text = "Xem trước RDLC";
            grpKetQua.Controls.Add(_reportViewer);
            _reportViewer.BringToFront();
            dgvKetQua.Visible = false;
            _duLieu = new ReportDataService(_danhMuc);
            GiaoDien.ApDung(this);
        }

        private void frmReportBuilder_Load(object? sender, EventArgs e)
        {
            // 1. Nạp danh sách nguồn dữ liệu
            cboNguon.DisplayMember = "Ten";
            cboNguon.ValueMember = "Ma";
            cboNguon.DataSource = _danhMuc.NguonDuLieu.ToList();

            // 2. Nạp các phép toán lọc
            cboPhepToan.Items.Clear();
            cboPhepToan.Items.AddRange(new object[] {
                "--Không lọc--",
                "= (Bằng)",
                "Chứa (LIKE)",
                "> (Lớn hơn)",
                "< (Nhỏ hơn)",
                ">= (Lớn hơn hoặc bằng)",
                "<= (Nhỏ hơn hoặc bằng)"
            });
            cboPhepToan.SelectedIndex = 0;

            // 3. Nạp danh sách mẫu có sẵn
            cboMau.Items.Clear();
            cboMau.Items.Add("--Tự thiết kế tùy ý--");
            foreach (var mau in _danhMuc.MauDungSan)
            {
                cboMau.Items.Add(mau.Ten);
            }
            cboMau.SelectedIndex = 0;

            // Nạp các cột cho nguồn đầu tiên
            if (cboNguon.Items.Count > 0)
            {
                cboNguon.SelectedIndex = 0;
            }
        }

        private void cboNguon_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cboNguon.SelectedItem is not DataSourceDefinition nguon)
                return;

            txtTenBaoCao.Text = $"Báo cáo {nguon.Ten} ({DateTime.Now:dd/MM/yyyy})";

            // Nạp CheckedListBox chọn cột
            clbCot.Items.Clear();
            foreach (var f in nguon.Truong)
            {
                clbCot.Items.Add(new LuaChonTruong(f), true); // Mặc định tích chọn tất cả
            }

            // Nạp ComboBox chọn cột lọc
            cboCotLoc.Items.Clear();
            cboCotLoc.Items.Add("--Chọn cột cần lọc--");
            foreach (var f in nguon.Truong)
            {
                cboCotLoc.Items.Add(new LuaChonTruong(f));
            }
            cboCotLoc.SelectedIndex = 0;

            // Nạp ComboBox chọn cột sắp xếp
            cboSapXep.Items.Clear();
            cboSapXep.Items.Add("--Mặc định--");
            foreach (var f in nguon.Truong)
            {
                cboSapXep.Items.Add(new LuaChonTruong(f));
            }
            cboSapXep.SelectedIndex = 0;
        }

        private void btnChonTatCaCot_Click(object? sender, EventArgs e)
        {
            for (int i = 0; i < clbCot.Items.Count; i++)
            {
                clbCot.SetItemChecked(i, true);
            }
        }

        private void btnBoChonTatCaCot_Click(object? sender, EventArgs e)
        {
            for (int i = 0; i < clbCot.Items.Count; i++)
            {
                clbCot.SetItemChecked(i, false);
            }
        }

        private void cboMau_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cboMau.SelectedIndex <= 0)
                return;

            var mau = _danhMuc.MauDungSan.FirstOrDefault(m => m.Ten == cboMau.SelectedItem?.ToString());
            if (mau == null)
                return;

            // Chọn nguồn tương ứng (dùng mau.MaNguon trực tiếp, không có mau.DinhNghia)
            for (int i = 0; i < cboNguon.Items.Count; i++)
            {
                if (cboNguon.Items[i] is DataSourceDefinition src && src.Ma == mau.MaNguon)
                {
                    cboNguon.SelectedIndex = i;
                    break;
                }
            }

            txtTenBaoCao.Text = mau.Ten;

            // Tích chọn đúng các cột của mẫu (gọi mau.Tao() để lấy ReportDefinition mẫu)
            var dinhNghiaMau = mau.Tao();
            var cotMau = new HashSet<string>(dinhNghiaMau.Cot.Select(c => c.MaTruong), StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < clbCot.Items.Count; i++)
            {
                if (clbCot.Items[i] is LuaChonTruong item)
                {
                    clbCot.SetItemChecked(i, cotMau.Contains(item.Truong.Ma));
                }
            }
        }

        private void btnXemBaoCao_Click(object? sender, EventArgs e)
        {
            if (cboNguon.SelectedItem is not DataSourceDefinition nguon)
            {
                MessageBox.Show("Vui lòng chọn nguồn dữ liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var cotDaChon = clbCot.CheckedItems.OfType<LuaChonTruong>().Select(x => x.Truong).ToList();
            if (cotDaChon.Count == 0)
            {
                MessageBox.Show("Vui lòng tích chọn ít nhất một cột cần hiển thị trên báo cáo.", "Chưa chọn cột", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var def = new ReportDefinition
            {
                Ten = string.IsNullOrWhiteSpace(txtTenBaoCao.Text) ? nguon.Ten : txtTenBaoCao.Text.Trim(),
                MaNguon = nguon.Ma,
                NguoiTao = CurrentUser.TenDangNhap ?? "admin",
                Cot = cotDaChon.Select((f, idx) => new ReportColumnDef
                {
                    MaTruong = f.Ma,
                    TieuDe = f.TieuDe,
                    CanLe = f.Kieu == KieuDuLieu.SoNguyen || f.Kieu == KieuDuLieu.SoThuc || f.Kieu == KieuDuLieu.TienTe
                        ? CanLeCot.Phai
                        : (f.Kieu == KieuDuLieu.Ngay ? CanLeCot.Giua : CanLeCot.Trai),
                    DinhDang = f.Kieu == KieuDuLieu.TienTe ? "#,##0 ₫" : "",
                    ThuTu = idx,
                    HienThi = true
                }).ToList()
            };

            // Thêm bộ lọc nếu có nhập
            if (cboCotLoc.SelectedIndex > 0 && cboCotLoc.SelectedItem is LuaChonTruong cotLoc && cboPhepToan.SelectedIndex > 0)
            {
                string val = txtGiaTriLoc.Text.Trim();
                if (!string.IsNullOrWhiteSpace(val))
                {
                    ToanTuLoc op = cboPhepToan.SelectedIndex switch
                    {
                        1 => ToanTuLoc.Bang,
                        2 => ToanTuLoc.Chua,
                        3 => ToanTuLoc.LonHon,
                        4 => ToanTuLoc.NhoHon,
                        5 => ToanTuLoc.LonHonBang,
                        6 => ToanTuLoc.NhoHonBang,
                        _ => ToanTuLoc.Bang
                    };

                    def.Loc.Add(new ReportFilterDef
                    {
                        MaTruong = cotLoc.Truong.Ma,
                        ToanTu = op,
                        GiaTri1 = val,
                        Bat = true
                    });
                }
            }

            // Thêm sắp xếp nếu có chọn
            if (cboSapXep.SelectedIndex > 0 && cboSapXep.SelectedItem is LuaChonTruong cotSapXep)
            {
                def.SapXep.Add(new ReportSortDef
                {
                    MaTruong = cotSapXep.Truong.Ma,
                    Huong = chkGiamDan.Checked ? HuongSapXep.GiamDan : HuongSapXep.TangDan
                });
            }

            Cursor = Cursors.WaitCursor;
            lblThongKe.Text = "Đang truy vấn dữ liệu từ CSDL...";
            Application.DoEvents();

            try
            {
                var nguonDuLieu = _danhMuc.YeuCauNguon(def.MaNguon);
                var phamVi = ReportScopeService.ChoNguon(nguonDuLieu);
                ReportResult result;
                try
                {
                    result = _duLieu.Chay(def, new ReportSqlDataProvider(), phamVi);
                    lblThongKe.Text = $"Truy vấn thành công: {result.SoDongDuLieu} bản ghi ({result.ThoiGianChayMs} ms)";
                }
                catch (Exception loiTruyVan)
                {
                    result = _duLieu.Chay(def, new MockReportDataProvider(), phamVi);
                    lblThongKe.Text = $"Không kết nối được CSDL, đang hiển thị dữ liệu mẫu: {result.SoDongDuLieu} dòng ({result.ThoiGianChayMs} ms). {loiTruyVan.Message}";
                }

                var bangRdlc = new DataTable("ReportData");
                var bangExcel = new DataTable();
                foreach (var (cot, i) in result.Cot.Select((cot, i) => (cot, i)))
                {
                    bangRdlc.Columns.Add($"F{i}", typeof(string));
                    bangExcel.Columns.Add(cot.MaTruong, typeof(string)).Caption = cot.TieuDe;
                }

                foreach (var dong in result.Dong.Where(d => d.Loai == LoaiDongBaoCao.DuLieu))
                {
                    var giaTriHienThi = result.Cot.Select((cot, i) =>
                        ReportFormatters.Hien(i < dong.GiaTri.Length ? dong.GiaTri[i] : null,
                            cot.Kieu, cot.DinhDang, cot.TienTo, cot.HauTo)).ToArray();
                    bangRdlc.Rows.Add(giaTriHienThi);
                    bangExcel.Rows.Add(giaTriHienThi.Cast<object>().ToArray());
                }

                dgvKetQua.DataSource = bangExcel;
                foreach (DataGridViewColumn col in dgvKetQua.Columns)
                    col.HeaderText = bangExcel.Columns[col.Name]?.Caption ?? col.HeaderText;

                _reportViewer.LocalReport.DataSources.Clear();
                using (var rdlc = new MemoryStream(RdlcRuntimeBuilder.Tao(
                    string.IsNullOrWhiteSpace(def.TieuDe) ? def.Ten : def.TieuDe,
                    result.Cot.Select(c => c.TieuDe).ToArray())))
                {
                    _reportViewer.LocalReport.LoadReportDefinition(rdlc);
                }
                _reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ReportData", bangRdlc));
                _reportViewer.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi tạo báo cáo: " + ex.Message, "Lỗi truy vấn", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblThongKe.Text = "Lỗi khi truy vấn.";
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void btnXuatExcel_Click(object? sender, EventArgs e)
        {
            if (_reportViewer.LocalReport.DataSources.Count == 0)
            {
                MessageBox.Show("Chưa có dữ liệu để xuất Excel. Vui lòng nhấn 'Xem báo cáo' trước.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Title = "Xuất RDLC ra Excel",
                Filter = "Excel (*.xlsx)|*.xlsx",
                FileName = $"{TenFile(txtTenBaoCao.Text)}_{DateTime.Now:yyyyMMdd_HHmm}.xlsx"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;
            try
            {
                File.WriteAllBytes(sfd.FileName, _reportViewer.LocalReport.Render("EXCELOPENXML"));
                MessageBox.Show("Đã xuất báo cáo RDLC ra Excel.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xuất báo cáo thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string TenFile(string ten) => string.Concat((ten ?? "BaoCao").Select(c =>
            Path.GetInvalidFileNameChars().Contains(c) ? '_' : c));

        private void btnLamMoi_Click(object? sender, EventArgs e)
        {
            txtGiaTriLoc.Clear();
            cboPhepToan.SelectedIndex = 0;
            cboCotLoc.SelectedIndex = 0;
            cboSapXep.SelectedIndex = 0;
            chkGiamDan.Checked = false;
            btnChonTatCaCot_Click(sender, e);
            dgvKetQua.DataSource = null;
            lblThongKe.Text = "Đã làm mới thiết lập.";
        }

        // Lớp helper bọc hiển thị trường trong CheckListBox và ComboBox
        private sealed class LuaChonTruong
        {
            public FieldDefinition Truong { get; }
            public LuaChonTruong(FieldDefinition truong) => Truong = truong;
            public override string ToString() => Truong.TieuDe;
        }
    }
}
