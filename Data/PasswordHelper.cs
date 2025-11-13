using System.Security.Cryptography;
using System.Text;

namespace HotelAPI.Helpers
{
    public static class PasswordHelper
    {
        public static string CreatePasswordHash(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public static bool VerifyPassword(string password, string hash)
        {
            var computedHash = CreatePasswordHash(password);
            return computedHash == hash;
        }
    }
}
