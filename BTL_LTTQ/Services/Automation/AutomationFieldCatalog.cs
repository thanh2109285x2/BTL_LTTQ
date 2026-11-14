using BTL_LTTQ.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BTL_LTTQ.Services.Automation
{
    public enum KieuDuLieuTruong
    {
        SoNguyen,
        LuanLy
    }

    public sealed class DinhNghiaTruong
    {
        public DinhNghiaTruong(string code, string ten, KieuDuLieuTruong kieu, string moTa, params string[] toanTu)
        {
            Code = code;
            Ten = ten;
            Kieu = kieu;
            MoTa = moTa;
            ToanTu = toanTu;
        }

        public string Code { get; }
        public string Ten { get; }
        public KieuDuLieuTruong Kieu { get; }
        public string MoTa { get; }
        public IReadOnlyList<string> ToanTu { get; }
    }

    /// <summary>
    /// Danh sách trắng các trường được phép dùng trong điều kiện.
    /// Expression chỉ được sinh từ đây + Operator + CompareValue, không nhận text tự do từ Admin.
    /// </summary>
    public static class AutomationFieldCatalog
    {
        private static readonly IReadOnlyList<DinhNghiaTruong> _tatCa = new List<DinhNghiaTruong>
        {
            new(AutomationConst.FieldCode.CurrentEnrollment, "Số học viên hiện tại",
                KieuDuLieuTruong.SoNguyen, "Tổng số học viên đã đăng ký lớp",
                AutomationConst.Operator.SoHoc),
            new(AutomationConst.FieldCode.ClassCapacity, "Sĩ số tối đa",
                KieuDuLieuTruong.SoNguyen, "Giới hạn đăng ký của lớp",
                AutomationConst.Operator.SoHoc),
            new(AutomationConst.FieldCode.MinimumEnrollment, "Sĩ số tối thiểu",
                KieuDuLieuTruong.SoNguyen, "Sĩ số tối thiểu để lớp có thể khai giảng",
                AutomationConst.Operator.SoHoc),
            new(AutomationConst.FieldCode.HasTeacher, "Đã có giáo viên",
                KieuDuLieuTruong.LuanLy, "Lớp đã được phân công giáo viên",
                AutomationConst.Operator.IsTrue, AutomationConst.Operator.IsFalse,
                AutomationConst.Operator.Equals_, AutomationConst.Operator.NotEquals),
            new(AutomationConst.FieldCode.HasSchedule, "Đã có lịch học",
                KieuDuLieuTruong.LuanLy, "Lớp đã có ít nhất một buổi học trong thời khóa biểu",
                AutomationConst.Operator.IsTrue, AutomationConst.Operator.IsFalse,
                AutomationConst.Operator.Equals_, AutomationConst.Operator.NotEquals),
            new(AutomationConst.FieldCode.DaysUntilStart, "Số ngày đến ngày bắt đầu",
                KieuDuLieuTruong.SoNguyen, "Âm nếu ngày bắt đầu đã qua",
                AutomationConst.Operator.SoHoc),
            new(AutomationConst.FieldCode.DaysUntilEnd, "Số ngày đến ngày kết thúc",
                KieuDuLieuTruong.SoNguyen, "Âm nếu ngày kết thúc đã qua",
                AutomationConst.Operator.SoHoc)
        };

        public static IReadOnlyList<DinhNghiaTruong> TatCa => _tatCa;

        public static DinhNghiaTruong? Tim(string? code)
        {
            if (string.IsNullOrWhiteSpace(code)) return null;
            return _tatCa.FirstOrDefault(t => string.Equals(t.Code, code, StringComparison.Ordinal));
        }

        public static bool LaSoNguyen(string? code) => Tim(code)?.Kieu == KieuDuLieuTruong.SoNguyen;

        public static bool LaLuanLy(string? code) => Tim(code)?.Kieu == KieuDuLieuTruong.LuanLy;

        public static IReadOnlyList<string> ToanTuCho(string? code)
        {
            return Tim(code)?.ToanTu ?? Array.Empty<string>();
        }

        /// <summary>Lấy giá trị thực tế của trường từ context để ghi log (chỉ trường trong whitelist).</summary>
        public static string? LayGiaTri(AutomationContext? ctx, string fieldCode)
        {
            if (ctx == null || Tim(fieldCode) == null) return null;

            object? giaTri = typeof(AutomationContext).GetProperty(fieldCode)?.GetValue(ctx);
            return giaTri switch
            {
                null => null,
                bool b => b ? "true" : "false",
                int i => i.ToString(System.Globalization.CultureInfo.InvariantCulture),
                _ => Convert.ToString(giaTri, System.Globalization.CultureInfo.InvariantCulture)
            };
        }
    }
}
