using System.Security.Cryptography;
using System.Text;

namespace backend.Helpers
{
    public class TokenHelper
    {
        public static string GenerateToken(int size = 32)
        {
            var bytes = new byte[size];
            RandomNumberGenerator.Fill(bytes);
            return Convert.ToBase64String(bytes);
        }

        public static byte[] ComputeSha256HashBytes(string input)
        {
            using var sha = SHA256.Create();
            return sha.ComputeHash(Encoding.UTF8.GetBytes(input));
        }
    }
}
