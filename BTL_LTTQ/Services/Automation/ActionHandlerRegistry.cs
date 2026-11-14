using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace BTL_LTTQ.Services.Automation
{
    /// <summary>
    /// Registry định nghĩa và quản lý tất cả AutomationAction Handler.
    /// Handler được đăng ký bằng ActionType, cho phép dễ dàng thêm/xóa handler mà không cần sửa code cốt lõi.
    /// </summary>
    public static class ActionHandlerRegistry
    {
        // Dictionary lưu tráng Handler theo ActionType
        private static readonly ConcurrentDictionary<string, IAutomationActionHandler> _handlers =
            new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Đăng ký handler cho một ActionType cụ thể.
        /// Nếu đã có handler cho ActionType này, sẽ ném exception.
        /// </summary>
        public static void Register(IAutomationActionHandler handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            if (_handlers.TryAdd(handler.ActionType, handler))
            {
                Console.WriteLine($"[ActionHandlerRegistry] Đăng ký handler cho ActionType: {handler.ActionType}");
            }
            else
            {
                throw new InvalidOperationException(
                    $"ActionType '{handler.ActionType}' đã được đăng ký bởi handler khác.");
            }
        }

        /// <summary>
        /// Lấy handler theo ActionType.
        /// </summary>
        public static IAutomationActionHandler? GetHandler(string actionType)
        {
            _handlers.TryGetValue(actionType, out var handler);
            return handler;
        }

        /// <summary>
        /// Lấy tất cả handler đã đăng ký.
        /// </summary>
        public static IReadOnlyList<IAutomationActionHandler> GetAllHandlers() =>
            _handlers.Values.ToList().AsReadOnly();

        private static readonly object _khoiTaoLock = new();
        private static bool _daKhoiTao;

        /// <summary>
        /// Khởi tạo các handler mặc định. Gọi nhiều lần cũng an toàn.
        /// </summary>
        public static void InitializeDefaultHandlers()
        {
            lock (_khoiTaoLock)
            {
                if (_daKhoiTao)
                    return;

                // Đăng ký các handler MVP
                Register(new CreateNotificationHandler());
                Register(new CreateTaskHandler());
                Register(new UpdateClassStatusHandler());
                Register(new WriteAuditLogHandler());

                _daKhoiTao = true;
            }
        }

        /// <summary>
        /// Bảo đảm handler mặc định đã được đăng ký trước khi dùng.
        /// An toàn khi gọi từ nhiều nơi và nhiều luồng.
        /// </summary>
        public static void EnsureInitialized()
        {
            if (_daKhoiTao)
                return;

            InitializeDefaultHandlers();
        }
    }
}