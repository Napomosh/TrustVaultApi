using System.Security.Cryptography;
using System.Text;

namespace TrustDrop.Common.Crypto;

public static class PasswordHasher
{
    private const int ITERATIONS = 100000;
    
    public static byte[] HashPassword(string password, byte[] salt)
    {
        var hash = Rfc2898DeriveBytes.Pbkdf2
        (
            Encoding.UTF8.GetBytes(password),
            salt,
            ITERATIONS,
            HashAlgorithmName.SHA256,
            32
        );

        return hash;
    }

    public static bool VerifyPassword(string password, byte[] salt, byte[] hash)
    {
        return CryptographicOperations.FixedTimeEquals(
            HashPassword(password, salt), hash);
    }
}