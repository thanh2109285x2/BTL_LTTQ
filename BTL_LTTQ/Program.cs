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
<<<<<<< Updated upstream
            Application.Run(new frmMain());
=======

            // Đăng ký handler Automation một lần cho toàn ứng dụng.
            Services.Automation.ActionHandlerRegistry.EnsureInitialized();

            using var formDangNhap = new frmDangNhap();
            if (formDangNhap.ShowDialog() == DialogResult.OK)
                Application.Run(new frmMain());
>>>>>>> Stashed changes
        }
    }
}