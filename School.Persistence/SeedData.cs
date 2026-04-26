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
            Make("teacher@school.com",      "Teacher@123",     "Ali Teacher",     UserRole.Teacher),
            Make("accounts@school.com",     "Accounts@123",    "Zara Accountant", UserRole.Accountant),
            Make("staff@school.com",        "Staff@123",       "Omar Staff",      UserRole.Staff),
            Make("parent@school.com",       "Parent@123",      "Ahmed Parent",    UserRole.Parent),
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
