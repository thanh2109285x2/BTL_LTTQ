using BTL_LTTQ.Forms.HeThong;

namespace BTL_LTTQ
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // Đăng ký handler Automation một lần cho toàn ứng dụng.
            Services.Automation.ActionHandlerRegistry.EnsureInitialized();

            using var formDangNhap = new frmDangNhap();
            if (formDangNhap.ShowDialog() == DialogResult.OK)
                Application.Run(new frmMain());
        }
    }
}