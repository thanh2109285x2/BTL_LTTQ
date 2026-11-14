using BTL_LTTQ.DataAccessLayer;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;

namespace BTL_LTTQ.Forms.BaoCao
{
    public class frmBaoCaoBase : Form
    {
        private readonly BaoCaoRepository _repo = new();
        private readonly bool _laBaoCaoDiem;

        public frmBaoCaoBase() : this(false) { }

        protected frmBaoCaoBase(bool laBaoCaoDiem)
        {
            _laBaoCaoDiem = laBaoCaoDiem;
            GiaoDien.ApDung(this);
            Load += FrmBaoCao_Load;
        }

        protected virtual ComboBox cboLop => null!;
        protected virtual DataGridView dgvBaoCao => null!;
        protected virtual Label lblTong => null!;

        private void FrmBaoCao_Load(object? sender, EventArgs e)
        {
            try
            {
                var lop = _repo.LayDanhSachLop();
                var tatCa = lop.NewRow();
                tatCa["MaLop"] = DBNull.Value;
                tatCa["TenLop"] = "-- Tất cả lớp --";
                lop.Rows.InsertAt(tatCa, 0);
                cboLop.DataSource = lop;
                cboLop.DisplayMember = "TenLop";
                cboLop.ValueMember = "MaLop";
                NapDuLieu();
            }
            catch (Exception ex) { HienLoi(ex); }
        }

        protected void NapDuLieu()
        {
            try
            {
                string? maLop = cboLop.SelectedValue == null || cboLop.SelectedValue == DBNull.Value
                    ? null : cboLop.SelectedValue.ToString();
                var data = _laBaoCaoDiem ? _repo.LayBangDiem(maLop) : _repo.LayDoanhThu(maLop);
                dgvBaoCao.DataSource = data;
                DinhDangCot(data);
                if (_laBaoCaoDiem)
                {
                    var diem = data.AsEnumerable().Where(r => !r.IsNull("Diem")).Select(r => Convert.ToDouble(r["Diem"])).ToList();
                    lblTong.Text = $"Số học viên có điểm: {diem.Count}     Điểm trung bình: {(diem.Count == 0 ? "—" : diem.Average().ToString("0.##"))}";
                }
                else
                {
                    decimal tong = data.AsEnumerable().Sum(r => r.Field<decimal>("DoanhThu"));
                    lblTong.Text = $"Tổng doanh thu: {tong:N0} VNĐ";
                }
            }
            catch (Exception ex) { HienLoi(ex); }
        }

        private void DinhDangCot(DataTable data)
        {
            string[] headers = _laBaoCaoDiem
                ? ["Mã lớp", "Tên lớp", "Mã học viên", "Họ tên", "Điểm", "Xếp loại"]
                : ["Mã lớp", "Tên lớp", "Môn học", "Số học viên đã nộp", "Học phí / học viên", "Doanh thu"];
            for (int i = 0; i < data.Columns.Count; i++)
            {
                var col = dgvBaoCao.Columns[i];
                col.HeaderText = headers[i];
                if (_laBaoCaoDiem && col.Name == "Diem") col.DefaultCellStyle.Format = "0.##";
                if (!_laBaoCaoDiem && (col.Name == "DoanhThu" || col.Name == "HocPhiMoiHocVien")) col.DefaultCellStyle.Format = "N0";
            }
        }

        private static void HienLoi(Exception ex) => MessageBox.Show(
            ex is SqlException ? "Không tải được báo cáo: " + ex.Message : "Có lỗi khi tải báo cáo: " + ex.Message,
            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}
