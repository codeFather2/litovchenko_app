using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace LitovchenkoApp.Utils;

public static class PasswordUtils
{
    public static string HashPassword(string original)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: original!,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
    }

    public static bool IsPasswordSafe(string password)
    {
        var regex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d).{2,}$", RegexOptions.Compiled);
        return regex.IsMatch(password);
    }
}