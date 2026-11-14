namespace BTL_LTTQ.Common
{
    /// <summary>
    /// Hằng số dùng chung cho Workflow Automation Center.
    /// Giá trị phải khớp chính xác với CHECK constraint trong Database/Automation.sql.
    /// </summary>
    public static class AutomationConst
    {
        public static class Trigger
        {
            public const string ClassCreated = "CLASS_CREATED";
            public const string ClassEnrollmentChanged = "CLASS_ENROLLMENT_CHANGED";
            public const string ClassStatusChanged = "CLASS_STATUS_CHANGED";

            /// <summary>
            /// Sự kiện sau khi nhập hàng loạt hoàn tất (phát sau commit).
            /// Chưa đưa vào TatCa vì chưa có loại điều kiện/hành động phù hợp cho phạm vi toàn hệ thống.
            /// </summary>
            public const string BulkImportCompleted = "BULK_IMPORT_COMPLETED";

            public static readonly string[] TatCa =
            {
                ClassCreated, ClassEnrollmentChanged, ClassStatusChanged
            };

            public static string TenHienThi(string triggerType)
            {
                return triggerType switch
                {
                    ClassCreated => "Tạo lớp học",
                    ClassEnrollmentChanged => "Thay đổi đăng ký",
                    ClassStatusChanged => "Đổi trạng thái lớp",
                    BulkImportCompleted => "Nhập dữ liệu hàng loạt hoàn tất",
                    _ => triggerType
                };
            }
        }

        public static class ActionType
        {
            public const string CreateNotification = "CREATE_NOTIFICATION";
            public const string CreateTask = "CREATE_TASK";
            public const string UpdateClassStatus = "UPDATE_CLASS_STATUS";
            public const string WriteAuditLog = "WRITE_AUDIT_LOG";

            public static readonly string[] TatCa =
            {
                CreateNotification, CreateTask, UpdateClassStatus, WriteAuditLog
            };

            public static string TenHienThi(string actionType)
            {
                return actionType switch
                {
                    CreateNotification => "Tạo thông báo",
                    CreateTask => "Tạo nhiệm vụ",
                    UpdateClassStatus => "Cập nhật trạng thái lớp",
                    WriteAuditLog => "Ghi nhật ký",
                    _ => actionType
                };
            }
        }

        public static class FieldCode
        {
            public const string CurrentEnrollment = "CurrentEnrollment";
            public const string ClassCapacity = "ClassCapacity";
            public const string MinimumEnrollment = "MinimumEnrollment";
            public const string HasTeacher = "HasTeacher";
            public const string HasSchedule = "HasSchedule";
            public const string DaysUntilStart = "DaysUntilStart";
            public const string DaysUntilEnd = "DaysUntilEnd";

            public static readonly string[] TatCa =
            {
                CurrentEnrollment, ClassCapacity, MinimumEnrollment,
                HasTeacher, HasSchedule, DaysUntilStart, DaysUntilEnd
            };
        }

        public static class Operator
        {
            public const string Equals_ = "EQUALS";
            public const string NotEquals = "NOT_EQUALS";
            public const string GreaterThan = "GREATER_THAN";
            public const string GreaterThanOrEqual = "GREATER_THAN_OR_EQUAL";
            public const string LessThan = "LESS_THAN";
            public const string LessThanOrEqual = "LESS_THAN_OR_EQUAL";
            public const string IsTrue = "IS_TRUE";
            public const string IsFalse = "IS_FALSE";

            public static readonly string[] SoHoc =
            {
                Equals_, NotEquals, GreaterThan, GreaterThanOrEqual, LessThan, LessThanOrEqual
            };

            public static readonly string[] LuanLy =
            {
                IsTrue, IsFalse
            };

            public static readonly string[] TatCa =
            {
                Equals_, NotEquals, GreaterThan, GreaterThanOrEqual,
                LessThan, LessThanOrEqual, IsTrue, IsFalse
            };

            public static string TenHienThi(string op)
            {
                return op switch
                {
                    Equals_ => "Bằng",
                    NotEquals => "Khác",
                    GreaterThan => "Lớn hơn",
                    GreaterThanOrEqual => "Lớn hơn hoặc bằng",
                    LessThan => "Nhỏ hơn",
                    LessThanOrEqual => "Nhỏ hơn hoặc bằng",
                    IsTrue => "Đúng",
                    IsFalse => "Sai",
                    _ => op
                };
            }
        }

        public static class ExecutionStatus
        {
            public const string Running = "RUNNING";
            public const string Success = "SUCCESS";
            public const string Partial = "PARTIAL";
            public const string Failed = "FAILED";
            public const string Skipped = "SKIPPED";
        }

        public static class StepType
        {
            public const string Condition = "CONDITION";
            public const string Action = "ACTION";
        }

        public static class TaskStatus
        {
            public const string Pending = "PENDING";
            public const string InProgress = "IN_PROGRESS";
            public const string Completed = "COMPLETED";
            public const string Cancelled = "CANCELLED";

            public static readonly string[] TatCa = { Pending, InProgress, Completed, Cancelled };

            public static string TenHienThi(string status)
            {
                return status switch
                {
                    Pending => "Chờ xử lý",
                    InProgress => "Đang xử lý",
                    Completed => "Hoàn thành",
                    Cancelled => "Đã hủy",
                    _ => status
                };
            }
        }

        public static class TaskPriority
        {
            public const string Low = "LOW";
            public const string Normal = "NORMAL";
            public const string High = "HIGH";

            public static readonly string[] TatCa = { Low, Normal, High };
        }

        public static class MucDoThongBao
        {
            public const string Info = "INFO";
            public const string Warning = "WARNING";
            public const string Error = "ERROR";

            public static readonly string[] TatCa = { Info, Warning, Error };
        }

        public static class ClassStatus
        {
            public const string SapKhaiGiang = "SapKhaiGiang";
            public const string DangHoc = "DangHoc";
            public const string DaKetThuc = "DaKetThuc";
            public const string Huy = "Huy";

            public static readonly string[] TatCa = { SapKhaiGiang, DangHoc, DaKetThuc, Huy };

            public static string TenHienThi(string status)
            {
                return status switch
                {
                    SapKhaiGiang => "Sắp khai giảng",
                    DangHoc => "Đang học",
                    DaKetThuc => "Đã kết thúc",
                    Huy => "Đã hủy",
                    _ => status
                };
            }
        }

        public static class EntityType
        {
            public const string Class = "CLASS";
            public const string Import = "IMPORT";
        }

        /// <summary>Mã quyền của module Automation.</summary>
        public const string QuyenXem = ChucNangConst.AUTOMATION_XEM;
        public const string QuyenQuanLy = ChucNangConst.AUTOMATION_QUANLY;
        public const string QuyenTask = ChucNangConst.AUTOMATION_TASK;
    }
}
