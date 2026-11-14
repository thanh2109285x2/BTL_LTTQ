using System.Security.Cryptography;
using System.Text;

namespace BTL_LTTQ.Common
{
    public static class MatKhauHelper
    {
        private const int SoVongLap = 600000;
        private const int DoDaiHash = 32;
        private const int DoDaiSalt = 16;

        public static string TaoSalt()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(DoDaiSalt));
        }

        public static string BamMatKhau(string matKhau, string saltBase64)
        {
            byte[] matKhauBytes = Encoding.UTF8.GetBytes(matKhau);
            byte[] saltBytes = Convert.FromBase64String(saltBase64);
            byte[] hashBytes = Rfc2898DeriveBytes.Pbkdf2(
                matKhauBytes,
                saltBytes,
                SoVongLap,
                HashAlgorithmName.SHA256,
                DoDaiHash);

            return Convert.ToBase64String(hashBytes);
        }

        public static bool KiemTra(string matKhau, string saltBase64, string hashDaLuu)
        {
            try
            {
                byte[] hashMoi = Convert.FromBase64String(BamMatKhau(matKhau, saltBase64));
                byte[] hashCu = Convert.FromBase64String(hashDaLuu);
                return CryptographicOperations.FixedTimeEquals(hashMoi, hashCu);
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
