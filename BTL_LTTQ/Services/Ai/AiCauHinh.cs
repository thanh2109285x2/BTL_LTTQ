using System.Configuration;

namespace BTL_LTTQ.Services.Ai
{
    public static class AiCauHinh
    {
        public static string BaseUrl =>
            (ConfigurationManager.AppSettings["AiBaseUrl"] ?? "").Trim().TrimEnd('/');

        public static string Model =>
            string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["AiModel"])
                ? "openai/gpt-oss-120b"
                : ConfigurationManager.AppSettings["AiModel"]!.Trim();

        public static string ApiKey
        {
            get
            {
                string tuMoiTruong = Environment.GetEnvironmentVariable("AI_API_KEY") ?? "";
                if (!string.IsNullOrWhiteSpace(tuMoiTruong))
                    return tuMoiTruong.Trim();

                return (ConfigurationManager.AppSettings["AiApiKey"] ?? "").Trim();
            }
        }

        public static string? LoiThieuCauHinh()
        {
            if (ApiKey.Length == 0)
                return "Chưa có API key. Đặt biến môi trường AI_API_KEY hoặc điền AiApiKey trong App.config, rồi build lại.";
            if (BaseUrl.Length == 0)
                return "Chưa có AiBaseUrl trong App.config.";
            return null;
        }
    }
}
