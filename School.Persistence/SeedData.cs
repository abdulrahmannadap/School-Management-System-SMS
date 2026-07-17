using System.Security.Cryptography;
using System.Text;
using School.Domain.Entities;
using School.Domain.Enums;

namespace School.Persistence;

public static class SeedData
{
    public static void Seed(AppDbContext db)
    {
        if (db.Users.Any()) return;

        db.Users.AddRange(
            Make("superadmin@sms.com",      "SuperAdmin@123",  "Super Admin",     UserRole.SuperAdmin),
            Make("schooladmin@school.com",  "SchoolAdmin@123", "School Admin",    UserRole.SchoolAdmin),
            Make("student@school.com",      "Student@123",     "Sara Student",    UserRole.Student)
        );

        db.SaveChanges();
    }

    private static User Make(string email, string password, string fullName, UserRole role) => new()
    {
        Email        = email,
        PasswordHash = Hash(password),
        FullName     = fullName,
        Role         = role
    };

    public static string Hash(string input) =>
        Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(input)));
}
