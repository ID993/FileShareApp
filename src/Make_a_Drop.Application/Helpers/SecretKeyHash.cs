using System.Security.Cryptography;
using System.Text;

namespace Make_a_Drop.Application.Helpers
{
    public static class SecretKeyHash
    {
        public static string Hash(string secretKey)
        {
            var hashedKey = SHA256.HashData(Encoding.UTF8.GetBytes(secretKey));
            return Convert.ToBase64String(hashedKey);
        }

        public static bool MatchingSecretKey(string hashedSecretKey, string rawSecretKey)
        {
            var newHashKey = Hash(rawSecretKey);
            return newHashKey == hashedSecretKey;
        }
    }
}
