using BTL_LTTQ.Common;
using BTL_LTTQ.Models.Automation;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace BTL_LTTQ.Services.Automation
{
    /// <summary>
    /// Kiểm tra và sinh expression từ FieldCode + Operator + CompareValue.
    /// Đây là lớp duy nhất được phép tạo chuỗi expression; mọi giá trị đều đi qua whitelist.
    /// </summary>
    public static class ConditionEvaluator
    {
        private const string TienToThamSo = "ctx";

        /// <summary>Trả về null nếu điều kiện hợp lệ, ngược lại là thông báo lỗi.</summary>
        public static string? KiemTra(AutomationCondition? c)
        {
            if (c == null) return "Điều kiện không được để trống.";

            DinhNghiaTruong? dinhNghia = AutomationFieldCatalog.Tim(c.FieldCode);
            if (dinhNghia == null)
                return $"Trường '{c.FieldCode}' không được hỗ trợ.";

            if (!ChuaToanTu(dinhNghia.ToanTu, c.Operator))
                return $"Toán tử '{c.Operator}' không hợp lệ cho trường '{dinhNghia.Ten}'.";

            if (dinhNghia.Kieu == KieuDuLieuTruong.SoNguyen)
            {
                if (c.ValueNumber == null)
                    return $"Trường '{dinhNghia.Ten}' phải có giá trị số.";

                if (!string.IsNullOrEmpty(c.ValueText))
                    return $"Trường '{dinhNghia.Ten}' không được có giá trị văn bản.";

                if (decimal.Truncate(c.ValueNumber.Value) != c.ValueNumber.Value)
                    return $"Giá trị của '{dinhNghia.Ten}' phải là số nguyên.";
            }
            else
            {
                if (c.ValueNumber != null)
                    return $"Trường '{dinhNghia.Ten}' không được có giá trị số.";

                bool laDungSai = c.Operator == AutomationConst.Operator.IsTrue ||
                                 c.Operator == AutomationConst.Operator.IsFalse;

                if (laDungSai)
                {
                    if (!string.IsNullOrEmpty(c.ValueText))
                        return $"Toán tử '{AutomationConst.Operator.TenHienThi(c.Operator)}' không cần giá trị so sánh.";
                }
                else if (!bool.TryParse(c.ValueText, out _))
                {
                    return $"Giá trị của '{dinhNghia.Ten}' phải là true hoặc false.";
                }
            }

            return null;
        }

        /// <summary>Sinh expression an toàn. Ném InvalidOperationException nếu điều kiện không hợp lệ.</summary>
        public static string TaoExpression(AutomationCondition c)
        {
            string? loi = KiemTra(c);
            if (loi != null)
                throw new InvalidOperationException(loi);

            string truong = TienToThamSo + "." + c.FieldCode;

            if (AutomationFieldCatalog.LaSoNguyen(c.FieldCode))
            {
                string giaTri = c.ValueNumber!.Value.ToString("0", CultureInfo.InvariantCulture);
                return c.Operator switch
                {
                    AutomationConst.Operator.Equals_ => $"{truong} == {giaTri}",
                    AutomationConst.Operator.NotEquals => $"{truong} != {giaTri}",
                    AutomationConst.Operator.GreaterThan => $"{truong} > {giaTri}",
                    AutomationConst.Operator.GreaterThanOrEqual => $"{truong} >= {giaTri}",
                    AutomationConst.Operator.LessThan => $"{truong} < {giaTri}",
                    AutomationConst.Operator.LessThanOrEqual => $"{truong} <= {giaTri}",
                    _ => throw new InvalidOperationException($"Toán tử '{c.Operator}' không hỗ trợ cho trường số.")
                };
            }

            bool giaTriLuanLy = c.Operator == AutomationConst.Operator.IsTrue ||
                                (c.Operator != AutomationConst.Operator.IsFalse && bool.Parse(c.ValueText!));

            string bieuThuc = giaTriLuanLy ? "true" : "false";
            string toanTu = c.Operator == AutomationConst.Operator.NotEquals ? "!=" : "==";
            return $"{truong} {toanTu} {bieuThuc}";
        }

        /// <summary>Mô tả điều kiện để hiển thị trên lịch sử thực thi.</summary>
        public static string MoTa(AutomationCondition c)
        {
            DinhNghiaTruong? dinhNghia = AutomationFieldCatalog.Tim(c.FieldCode);
            string tenTruong = dinhNghia?.Ten ?? c.FieldCode;

            if (c.Operator == AutomationConst.Operator.IsTrue)
                return $"{tenTruong} là đúng";

            if (c.Operator == AutomationConst.Operator.IsFalse)
                return $"{tenTruong} là sai";

            return $"{tenTruong} {KyHieuToanTu(c.Operator)} {GiaTriMongDoi(c)}".TrimEnd();
        }

        /// <summary>Giá trị mong đợi dạng text ngắn gọn (dùng cho log).</summary>
        public static string? GiaTriMongDoi(AutomationCondition c)
        {
            if (AutomationFieldCatalog.LaSoNguyen(c.FieldCode))
            {
                return c.ValueNumber?.ToString("0", CultureInfo.InvariantCulture);
            }

            bool.TryParse(c.ValueText, out bool giaTri);
            return giaTri ? "true" : "false";
        }

        public static string KyHieuToanTu(string toanTu)
        {
            return toanTu switch
            {
                AutomationConst.Operator.Equals_ => "=",
                AutomationConst.Operator.NotEquals => "<>",
                AutomationConst.Operator.GreaterThan => ">",
                AutomationConst.Operator.GreaterThanOrEqual => ">=",
                AutomationConst.Operator.LessThan => "<",
                AutomationConst.Operator.LessThanOrEqual => "<=",
                AutomationConst.Operator.IsTrue => "đúng",
                AutomationConst.Operator.IsFalse => "sai",
                _ => toanTu
            };
        }

        /// <summary>Danh sách lỗi của toàn bộ Rule (rỗng nếu hợp lệ).</summary>
        public static List<string> KiemTraDanhSach(IReadOnlyList<AutomationCondition> ds)
        {
            var loi = new List<string>();

            if (ds.Count == 0)
            {
                loi.Add("Rule phải có ít nhất một điều kiện.");
                return loi;
            }

            for (int i = 0; i < ds.Count; i++)
            {
                string? thongBao = KiemTra(ds[i]);
                if (thongBao != null)
                    loi.Add($"Điều kiện #{i + 1}: {thongBao}");
            }

            return loi;
        }

        private static bool ChuaToanTu(IReadOnlyList<string> ds, string toanTu)
        {
            foreach (string item in ds)
            {
                if (string.Equals(item, toanTu, StringComparison.Ordinal))
                    return true;
            }
            return false;
        }
    }
}
