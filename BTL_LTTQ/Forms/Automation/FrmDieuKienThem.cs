using BTL_LTTQ.Common;
using BTL_LTTQ.Models.Automation;
using BTL_LTTQ.Services.Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.Automation
{
    public partial class FrmDieuKienThem : Form
    {
        private readonly string _fieldCode;
        private readonly IReadOnlyList<string> _toanTuCho;
        private readonly AutomationCondition? _hienTai;
        private AutomationCondition? _ketQua;

        public FrmDieuKienThem(string fieldCode, IEnumerable<string> toanTuCho,
                               AutomationCondition? hienTai = null)
        {
            InitializeComponent();
            _fieldCode = fieldCode ?? "";
            _toanTuCho = (toanTuCho ?? Array.Empty<string>()).ToList();
            _hienTai = hienTai;
            Load += FrmDieuKienThem_Load;
        }

        /// <summary>Điều kiện người dùng đã nhập (chỉ dùng khi ShowDialog trả về OK).</summary>
        public AutomationCondition LayDieuKien()
        {
            return _ketQua ?? new AutomationCondition { FieldCode = _fieldCode };
        }

        private void FrmDieuKienThem_Load(object? sender, EventArgs e)
        {
            DinhNghiaTruong? dinhNghia = AutomationFieldCatalog.Tim(_fieldCode);
            lblTenTruong.Text = dinhNghia?.Ten ?? _fieldCode;
            lblMoTaTruong.Text = dinhNghia?.MoTa ?? "";
            Text = $"Điều kiện: {lblTenTruong.Text}";

            CboToanTu.DisplayMember = "Ten";
            CboToanTu.ValueMember = "Code";
            CboToanTu.DataSource = _toanTuCho
                .Select(o => new { Code = o, Ten = AutomationConst.Operator.TenHienThi(o) })
                .ToList();

            bool laSo = AutomationFieldCatalog.LaSoNguyen(_fieldCode);
            if (laSo)
            {
                numGiaTriSo.Minimum = -1_000_000;
                numGiaTriSo.Maximum = 1_000_000;
                numGiaTriSo.Value = 0;
            }
            else
            {
                cboGiaTriLuanLy.DisplayMember = "Ten";
                cboGiaTriLuanLy.ValueMember = "Code";
                cboGiaTriLuanLy.DataSource = new List<object>
                {
                    new { Code = true, Ten = "Đúng" },
                    new { Code = false, Ten = "Sai" }
                };
                cboGiaTriLuanLy.SelectedValue = true;
            }

            if (_hienTai != null)
            {
                if (!string.IsNullOrWhiteSpace(_hienTai.Operator))
                    CboToanTu.SelectedValue = _hienTai.Operator;

                if (laSo)
                {
                    decimal giaTri = _hienTai.ValueNumber ?? 0;
                    if (giaTri < numGiaTriSo.Minimum) giaTri = numGiaTriSo.Minimum;
                    if (giaTri > numGiaTriSo.Maximum) giaTri = numGiaTriSo.Maximum;
                    numGiaTriSo.Value = giaTri;
                }
                else if (bool.TryParse(_hienTai.ValueText, out bool giaTriLuanLy))
                {
                    cboGiaTriLuanLy.SelectedValue = giaTriLuanLy;
                }
            }

            CapNhatHienThiGiaTri();
        }

        private void CboToanTu_SelectedIndexChanged(object? sender, EventArgs e)
        {
            CapNhatHienThiGiaTri();
        }

        private void CapNhatHienThiGiaTri()
        {
            string toanTu = CboToanTu.SelectedValue?.ToString() ?? "";
            bool laSo = AutomationFieldCatalog.LaSoNguyen(_fieldCode);
            bool canGiaTri = toanTu != AutomationConst.Operator.IsTrue &&
                             toanTu != AutomationConst.Operator.IsFalse;

            lblGiaTri.Visible = canGiaTri;
            numGiaTriSo.Visible = laSo && canGiaTri;
            cboGiaTriLuanLy.Visible = !laSo && canGiaTri;
        }

        private void BtnOK_Click(object? sender, EventArgs e)
        {
            string toanTu = CboToanTu.SelectedValue?.ToString() ?? "";
            if (string.IsNullOrWhiteSpace(toanTu))
            {
                MessageBox.Show("Vui lòng chọn toán tử.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dk = new AutomationCondition
            {
                FieldCode = _fieldCode,
                Operator = toanTu
            };

            if (AutomationFieldCatalog.LaSoNguyen(_fieldCode))
            {
                dk.ValueNumber = numGiaTriSo.Value;
                dk.ValueText = null;
            }
            else if (toanTu == AutomationConst.Operator.IsTrue ||
                     toanTu == AutomationConst.Operator.IsFalse)
            {
                dk.ValueText = null;
                dk.ValueNumber = null;
            }
            else
            {
                bool giaTri = cboGiaTriLuanLy.SelectedValue is bool b && b;
                dk.ValueText = giaTri ? "true" : "false";
                dk.ValueNumber = null;
            }

            string? loi = ConditionEvaluator.KiemTra(dk);
            if (loi != null)
            {
                MessageBox.Show(loi, "Điều kiện không hợp lệ",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            dk.SortOrder = _hienTai?.SortOrder ?? 1;
            _ketQua = dk;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnHuy_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
