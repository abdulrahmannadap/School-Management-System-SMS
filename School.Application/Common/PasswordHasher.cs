using System.Security.Cryptography;
using System.Text;

namespace School.Application.Common;

public static class PasswordHasher
{
    public static string Hash(string input) =>
        Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(input)));
}
