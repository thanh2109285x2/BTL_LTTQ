using BTL_LTTQ.Common;
using BTL_LTTQ.Models.Import;
using BTL_LTTQ.Services.Automation;
using BTL_LTTQ.Services.Import;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.Import
{
    /// <summary>
    /// Nhập hàng loạt Học viên / Giáo viên / Đăng ký lớp từ Excel hoặc CSV.
    /// Chỉ ghi database sau khi người dùng xem trước và xác nhận; toàn bộ tệp là một transaction.
    /// </summary>
    public partial class FrmBulkImport : Form
    {
        private readonly ImportService _service = new();

        private KetQuaPreview? _preview;
        private KetQuaImport? _ketQua;
        private List<ImportLoi> _loiHienThi = new();
        private List<string> _loiFileHienThi = new();
        private bool _dangXuLy;
        private bool _dangKhoiTao;

        public FrmBulkImport()
        {
            InitializeComponent();
            GiaoDien.ApDung(this);
        }

        private ImportLoai LoaiDangChon =>
            cboLoai.SelectedItem is LoaiItem item ? item.Loai : ImportLoai.HocVien;

        private void FrmBulkImport_Load(object sender, EventArgs e)
        {
            _dangKhoiTao = true;

            foreach (ImportLoai loai in ImportLoaiMoTa.TatCa)
                cboLoai.Items.Add(new LoaiItem(loai, ImportLoaiMoTa.TenHienThi(loai)));

            cboLoai.SelectedIndex = 0;
            _dangKhoiTao = false;

            lblTieuDe.ForeColor = GiaoDien.MauChinh;
            btnXacNhan.BackColor = GiaoDien.MauChinh;
            btnXacNhan.ForeColor = Color.White;
            btnXacNhan.FlatStyle = FlatStyle.Flat;
            btnXacNhan.FlatAppearance.BorderColor = GiaoDien.MauChinhDam;
            btnXacNhan.UseVisualStyleBackColor = false;

            DatLaiTrangThai();
        }

        private void cboLoai_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_dangKhoiTao) return;

            _preview = null;
            _ketQua = null;
            _loiHienThi = new List<ImportLoi>();
            _loiFileHienThi = new List<string>();
            txtTep.Clear();
            dgvDuLieu.Rows.Clear();
            dgvDuLieu.Columns.Clear();

            DatLaiTrangThai();

            if (!CoQuyenLoai(LoaiDangChon))
                lblCanhBao.Text = "Bạn không có quyền nhập loại dữ liệu này.";
        }

        private void btnChonTep_Click(object sender, EventArgs e)
        {
            if (!KiemTraQuyenTruocKhiLam())
                return;

            using var dlg = new OpenFileDialog
            {
                Title = "Chọn tệp dữ liệu cần nhập",
                Filter = "Tệp dữ liệu (*.xlsx;*.csv)|*.xlsx;*.csv|Excel (*.xlsx)|*.xlsx|CSV (*.csv)|*.csv",
                CheckFileExists = true,
                Multiselect = false
            };

            if (dlg.ShowDialog(this) != DialogResult.OK)
                return;

            txtTep.Text = dlg.FileName;
            ThucHienXemTruoc();
        }

        private void btnTaiMau_Click(object sender, EventArgs e)
        {
            ImportLoai loai = LoaiDangChon;

            using var dlg = new SaveFileDialog
            {
                Title = "Lưu tệp mẫu nhập dữ liệu",
                Filter = "Excel (*.xlsx)|*.xlsx",
                FileName = ImportLoaiMoTa.TenFileMau(loai),
                OverwritePrompt = true
            };

            if (dlg.ShowDialog(this) != DialogResult.OK)
                return;

            try
            {
                ImportTemplateWriter.Luu(loai, dlg.FileName);
                MessageBox.Show(this,
                    $"Đã tạo tệp mẫu:\n{dlg.FileName}\n\nTệp mẫu chỉ có tiêu đề và sheet hướng dẫn, không có dữ liệu mẫu.",
                    "Tạo tệp mẫu", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Không tạo được tệp mẫu: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXemTruoc_Click(object sender, EventArgs e)
        {
            if (!KiemTraQuyenTruocKhiLam())
                return;

            ThucHienXemTruoc();
        }

        private void ThucHienXemTruoc()
        {
            if (string.IsNullOrWhiteSpace(txtTep.Text) || !File.Exists(txtTep.Text))
            {
                MessageBox.Show(this, "Vui lòng chọn tệp dữ liệu .xlsx hoặc .csv.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                UseWaitCursor = true;
                _ketQua = null;
                _preview = _service.XemTruoc(txtTep.Text, LoaiDangChon);

                _loiHienThi = _preview.TatCaLoi.ToList();
                _loiFileHienThi = new List<string>(_preview.LoiFile);
                if (_preview.CotBatBuocThieu.Count > 0)
                {
                    _loiFileHienThi.Add("Thiếu cột bắt buộc: " + string.Join(", ", _preview.CotBatBuocThieu));
                }

                HienThiDuLieu();
                CapNhatTomTat();
                CapNhatCanhBao();
                CapNhatNut();

                if (_preview.LoiFile.Count > 0 || _preview.CotBatBuocThieu.Count > 0)
                {
                    MessageBox.Show(this, string.Join(Environment.NewLine, _loiFileHienThi),
                        "Không đọc được dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (_preview.SoDongLoi > 0)
                {
                    MessageBox.Show(this,
                        $"Tệp có {_preview.SoDongLoi} dòng lỗi trên tổng {_preview.TongSoDong} dòng.\n" +
                        "Bạn có thể xuất báo cáo lỗi để xem chi tiết. Nút Xác nhận nhập đang bị khóa.",
                        "Dữ liệu chưa hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                _preview = null;
                _loiHienThi = new List<ImportLoi>();
                _loiFileHienThi = new List<string> { "Không xem trước được dữ liệu: " + ex.Message };
                dgvDuLieu.Rows.Clear();
                dgvDuLieu.Columns.Clear();
                CapNhatTomTat();
                CapNhatCanhBao();
                CapNhatNut();
                MessageBox.Show(this, _loiFileHienThi[0],
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                UseWaitCursor = false;
            }
        }

        private async void btnXacNhan_Click(object sender, EventArgs e)
        {
            if (_dangXuLy || _preview == null)
                return;

            if (!_preview.CoTheImport)
            {
                MessageBox.Show(this, "Dữ liệu chưa hợp lệ nên không thể nhập.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!KiemTraQuyenTruocKhiLam())
                return;

            DialogResult xacNhan = MessageBox.Show(this,
                $"Nhập {_preview.TongSoDong} dòng {ImportLoaiMoTa.TenHienThi(_preview.Loai).ToLowerInvariant()}?\n\n" +
                "Toàn bộ tệp được ghi trong một lần. Nếu có bất kỳ lỗi nào, không dòng nào được ghi.",
                "Xác nhận nhập dữ liệu", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (xacNhan != DialogResult.Yes)
                return;

            KetQuaImport ketQua;

            try
            {
                _dangXuLy = true;
                progressNhap.Visible = true;
                CapNhatNut();
                UseWaitCursor = true;

                ketQua = _service.ThucHien(_preview);
            }
            catch (Exception ex)
            {
                ketQua = new KetQuaImport
                {
                    OperationId = _preview.OperationId,
                    Loai = _preview.Loai,
                    ThanhCong = false,
                    ThongDiep = "Không nhập được dữ liệu: " + ex.Message
                };
            }
            finally
            {
                _dangXuLy = false;
                progressNhap.Visible = false;
                UseWaitCursor = false;
            }

            _ketQua = ketQua;

            if (ketQua.ThanhCong)
            {
                _loiHienThi = new List<ImportLoi>();
                _loiFileHienThi = new List<string>();

                MessageBox.Show(this,
                    $"Đã nhập thành công {ketQua.SoDongDaThem} dòng {ImportLoaiMoTa.TenHienThi(ketQua.Loai).ToLowerInvariant()}.",
                    "Nhập dữ liệu thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Sự kiện Automation chỉ phát sau khi transaction nghiệp vụ đã commit.
                await PhatSuKienSauCommitAsync(ketQua);

                _preview = null;
                _ketQua = null;
                txtTep.Clear();
                dgvDuLieu.Rows.Clear();
                dgvDuLieu.Columns.Clear();
                DatLaiTrangThai();
                lblTrangThai.Text = "Đã nhập xong";
                lblTrangThai.ForeColor = Color.FromArgb(24, 128, 56);
                return;
            }

            _loiHienThi = ketQua.DanhSachLoi.Where(l => l.SoDong > 0).ToList();
            _loiFileHienThi = ketQua.DanhSachLoi
                .Where(l => l.SoDong <= 0 && !string.IsNullOrWhiteSpace(l.ThongDiep))
                .Select(l => l.ThongDiep)
                .ToList();

            if (_preview != null)
                HienThiDuLieu();

            CapNhatTomTat();
            CapNhatCanhBao();
            CapNhatNut();

            string chiTiet = _loiHienThi
                .Select(l => $"Dòng {l.SoDong}: {l.ThongDiep}")
                .Take(8)
                .ToList()
                .Count > 0
                ? string.Join(Environment.NewLine, _loiHienThi
                    .Select(l => $"Dòng {l.SoDong}: {l.ThongDiep}")
                    .Take(8))
                : "";

            string thongBao = ketQua.ThongDiep;
            if (chiTiet.Length > 0)
                thongBao += Environment.NewLine + Environment.NewLine + chiTiet;
            if (_loiHienThi.Count > 8)
                thongBao += Environment.NewLine + $"... và {_loiHienThi.Count - 8} lỗi khác. Xuất báo cáo lỗi để xem đầy đủ.";

            MessageBox.Show(this, thongBao, "Không nhập được dữ liệu",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private async Task PhatSuKienSauCommitAsync(KetQuaImport ketQua)
        {
            try
            {
                var publisher = new AutomationEventPublisher();
                string nguoiThucHien = CurrentUser.TenDangNhap;

                if (ketQua.Loai == ImportLoai.DangKy)
                {
                    foreach (string maLop in ketQua.LopBiAnhHuong)
                    {
                        await publisher.PhatSuKienAsync(
                            AutomationConst.Trigger.ClassEnrollmentChanged,
                            AutomationConst.EntityType.Class,
                            maLop,
                            $"BULK_IMPORT:{ketQua.OperationId:N}:{maLop}",
                            context: null,
                            triggeredBy: nguoiThucHien);
                    }
                }

                string entityId = "BI" + ketQua.OperationId.ToString("N").Substring(0, 18);
                var context = new AutomationContext
                {
                    EntityType = AutomationConst.EntityType.Import,
                    EntityID = entityId
                };

                await publisher.PhatSuKienAsync(
                    AutomationConst.Trigger.BulkImportCompleted,
                    AutomationConst.EntityType.Import,
                    entityId,
                    $"BULK_IMPORT_COMPLETED:{ketQua.OperationId:N}",
                    context,
                    nguoiThucHien);
            }
            catch
            {
                // Automation không được ảnh hưởng kết quả nhập dữ liệu đã commit.
            }
        }

        private void btnXuatBaoCao_Click(object sender, EventArgs e)
        {
            if (_loiHienThi.Count == 0 && _loiFileHienThi.Count == 0)
            {
                MessageBox.Show(this, "Hiện không có lỗi nào để xuất báo cáo.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ImportLoai loai = _ketQua?.Loai ?? _preview?.Loai ?? LoaiDangChon;
            string tenFile = _preview?.TenFile ?? Path.GetFileName(txtTep.Text);
            string tenMacDinh = $"BaoCaoLoi_Nhap_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

            using var dlg = new SaveFileDialog
            {
                Title = "Lưu báo cáo lỗi",
                Filter = "Excel (*.xlsx)|*.xlsx|CSV (*.csv)|*.csv",
                FileName = tenMacDinh,
                OverwritePrompt = true
            };

            if (dlg.ShowDialog(this) != DialogResult.OK)
                return;

            try
            {
                ImportErrorReportWriter.Luu(loai, tenFile, _loiFileHienThi, _loiHienThi, dlg.FileName);
                MessageBox.Show(this, "Đã lưu báo cáo lỗi:\n" + dlg.FileName,
                    "Xuất báo cáo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Không lưu được báo cáo: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            Close();
        }

        // ---------------------------------------------------------------- Kéo thả tệp

        private static readonly string[] _duoiKeoThaChoPhep = { ".xlsx", ".csv" };
        private static readonly Color _mauGoiYBinhThuong = Color.FromArgb(90, 98, 110);

        private void FrmBulkImport_DragEnter(object sender, DragEventArgs e)
        {
            bool coThe = !_dangXuLy && CoQuyenLoai(LoaiDangChon) && LayTepTha(e).Length > 0;
            e.Effect = coThe ? DragDropEffects.Copy : DragDropEffects.None;

            if (coThe)
                lblGoiY.ForeColor = GiaoDien.MauChinh;
        }

        private void FrmBulkImport_DragLeave(object sender, EventArgs e)
        {
            lblGoiY.ForeColor = _mauGoiYBinhThuong;
        }

        private void FrmBulkImport_DragDrop(object sender, DragEventArgs e)
        {
            lblGoiY.ForeColor = _mauGoiYBinhThuong;

            if (_dangXuLy)
                return;

            string[] tep = LayTepTha(e);

            if (tep.Length == 0)
            {
                MessageBox.Show(this,
                    "Không nhận được tệp .xlsx hoặc .csv hợp lệ từ thao tác kéo thả.",
                    "Không nhận được tệp", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!KiemTraQuyenTruocKhiLam())
                return;

            if (tep.Length > 1)
            {
                MessageBox.Show(this,
                    $"Bạn đã thả {tep.Length} tệp. Mỗi lần chỉ nhập một tệp, nên hệ thống dùng tệp đầu tiên:\n{tep[0]}",
                    "Chọn tệp đầu tiên", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            txtTep.Text = tep[0];
            ThucHienXemTruoc();
        }

        /// <summary>Lọc các tệp hợp lệ kéo vào: chỉ .xlsx/.csv và phải tồn tại.</summary>
        private static string[] LayTepTha(DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                return Array.Empty<string>();

            if (e.Data.GetData(DataFormats.FileDrop) is not string[] danhSach)
                return Array.Empty<string>();

            var hopLe = new List<string>();

            foreach (string duongDan in danhSach)
            {
                if (string.IsNullOrWhiteSpace(duongDan))
                    continue;

                string duoi = Path.GetExtension(duongDan).ToLowerInvariant();
                if (Array.IndexOf(_duoiKeoThaChoPhep, duoi) >= 0 && File.Exists(duongDan))
                    hopLe.Add(duongDan);
            }

            return hopLe.ToArray();
        }

        private void HienThiDuLieu()
        {
            dgvDuLieu.Rows.Clear();
            dgvDuLieu.Columns.Clear();

            if (_preview == null)
                return;

            dgvDuLieu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            dgvDuLieu.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CotSoDong",
                HeaderText = "Dòng",
                Width = 60,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            dgvDuLieu.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CotTrangThai",
                HeaderText = "Trạng thái",
                Width = 100,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            foreach (DinhNghiaCot cot in BangTinhReader.LayDinhNghiaCot(_preview.Loai))
            {
                dgvDuLieu.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Cot_" + cot.Cot,
                    HeaderText = cot.TenHienThi,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    MinimumWidth = 90,
                    FillWeight = cot.Cot is "MaHocVien" or "MaGV" or "MaLop" ? 80 : 110
                });
            }

            dgvDuLieu.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CotLoi",
                HeaderText = "Lỗi",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                MinimumWidth = 160,
                FillWeight = 180
            });

            var loiTheoDong = _loiHienThi
                .Where(l => l.SoDong > 0)
                .GroupBy(l => l.SoDong)
                .ToDictionary(g => g.Key, g => g.Select(x => x.ThongDiep).Distinct().ToList());

            foreach (ImportDong dong in _preview.DanhSachDong)
            {
                loiTheoDong.TryGetValue(dong.SoDong, out List<string>? loiCuaDong);
                bool coLoi = loiCuaDong != null && loiCuaDong.Count > 0;

                var giaTri = new List<object>
                {
                    dong.SoDong,
                    coLoi ? "Lỗi" : "Hợp lệ"
                };

                foreach (DinhNghiaCot cot in BangTinhReader.LayDinhNghiaCot(_preview.Loai))
                    giaTri.Add(dong.Lay(cot.Cot) ?? "");

                giaTri.Add(coLoi ? string.Join("; ", loiCuaDong!) : "");

                int chiSo = dgvDuLieu.Rows.Add(giaTri.ToArray());
                if (coLoi)
                {
                    dgvDuLieu.Rows[chiSo].DefaultCellStyle.BackColor = Color.FromArgb(253, 231, 231);
                    dgvDuLieu.Rows[chiSo].DefaultCellStyle.ForeColor = Color.FromArgb(150, 40, 30);
                }
            }
        }

        private void CapNhatTomTat()
        {
            if (_preview == null)
            {
                lblTongDong.Text = "0";
                lblHopLe.Text = "0";
                lblLoi.Text = "0";
                lblTrangThai.Text = "Chưa chọn tệp dữ liệu";
                lblTrangThai.ForeColor = GiaoDien.MauChu;
                return;
            }

            int tong = _preview.TongSoDong;
            int soDongLoi = _loiHienThi
                .Where(l => l.SoDong > 0)
                .Select(l => l.SoDong)
                .Distinct()
                .Count();
            soDongLoi = Math.Max(soDongLoi, _preview.SoDongLoi);

            lblTongDong.Text = tong.ToString();
            lblLoi.Text = soDongLoi.ToString();
            lblHopLe.Text = Math.Max(0, tong - soDongLoi).ToString();

            if (_preview.LoiFile.Count > 0 || _preview.CotBatBuocThieu.Count > 0)
            {
                lblTrangThai.Text = "Không đọc được tệp";
                lblTrangThai.ForeColor = GiaoDien.MauNguyHiem;
            }
            else if (soDongLoi > 0)
            {
                lblTrangThai.Text = "Có lỗi - chưa thể nhập";
                lblTrangThai.ForeColor = GiaoDien.MauNguyHiem;
            }
            else
            {
                lblTrangThai.Text = "Sẵn sàng nhập";
                lblTrangThai.ForeColor = Color.FromArgb(24, 128, 56);
            }
        }

        private void CapNhatCanhBao()
        {
            if (_loiFileHienThi.Count > 0)
            {
                lblCanhBao.Text = _loiFileHienThi[0];
                return;
            }

            if (_preview != null && _preview.CotKhongNhanDien.Count > 0)
            {
                lblCanhBao.Text = "Bỏ qua cột không nhận diện: " + string.Join(", ", _preview.CotKhongNhanDien);
                return;
            }

            lblCanhBao.Text = "";
        }

        private void CapNhatNut()
        {
            bool coQuyen = CoQuyenLoai(LoaiDangChon);

            btnXemTruoc.Enabled = !_dangXuLy && coQuyen;
            btnChonTep.Enabled = !_dangXuLy && coQuyen;
            btnXacNhan.Enabled = !_dangXuLy && coQuyen &&
                                 _preview != null && _preview.CoTheImport;
            btnXuatBaoCao.Enabled = !_dangXuLy &&
                                    (_loiHienThi.Count > 0 || _loiFileHienThi.Count > 0);
            cboLoai.Enabled = !_dangXuLy;
            btnTaiMau.Enabled = !_dangXuLy;
            btnDong.Enabled = !_dangXuLy;
        }

        private void DatLaiTrangThai()
        {
            lblTongDong.Text = "0";
            lblHopLe.Text = "0";
            lblLoi.Text = "0";
            lblTrangThai.Text = "Chưa chọn tệp dữ liệu";
            lblTrangThai.ForeColor = GiaoDien.MauChu;
            lblCanhBao.Text = "";
            CapNhatNut();
        }

        private bool KiemTraQuyenTruocKhiLam()
        {
            if (CoQuyenLoai(LoaiDangChon))
                return true;

            MessageBox.Show(this, "Bạn không có quyền nhập loại dữ liệu này.",
                "Không có quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        private static bool CoQuyenLoai(ImportLoai loai)
        {
            // Học viên không được nhập dữ liệu hàng loạt: học viên tự đăng ký lớp
            // bằng chức năng "Đăng ký lớp", không phải bằng nhập tệp.
            if (CurrentUser.IsHocVien)
                return false;

            return loai switch
            {
                ImportLoai.HocVien => PermissionService.Can(ChucNangConst.HOCVIEN_THEM),
                ImportLoai.GiaoVien => PermissionService.Can(ChucNangConst.GIAOVIEN_THEM),
                ImportLoai.DangKy => PermissionService.Can(ChucNangConst.DANGKYLOP),
                _ => false
            };
        }

        private sealed class LoaiItem
        {
            public LoaiItem(ImportLoai loai, string ten)
            {
                Loai = loai;
                Ten = ten;
            }

            public ImportLoai Loai { get; }
            public string Ten { get; }

            public override string ToString() => Ten;
        }
    }
}
