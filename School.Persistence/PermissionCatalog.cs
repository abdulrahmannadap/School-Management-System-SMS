using School.Domain.Entities;

namespace School.Persistence;

public static class PermissionCatalog
{
    public static readonly (string Key, string Module, string Description)[] All =
    [
        ("Student.View",       "Student",   "View student records"),
        ("Student.Manage",     "Student",   "Create, edit, and admit students"),
        ("Staff.View",         "Staff",     "View staff records"),
        ("Staff.Manage",       "Staff",     "Create, edit, and manage staff"),
        ("Attendance.View",    "Attendance","View attendance records"),
        ("Attendance.Mark",    "Attendance","Mark student or staff attendance"),
        ("Fees.View",          "Fees",      "View fee ledgers and pending fees"),
        ("Fees.Collect",       "Fees",      "Collect fee payments"),
        ("Fees.Refund",        "Fees",      "Issue fee refunds"),
        ("Exam.View",          "Exam",      "View exams and results"),
        ("Exam.ManageResults", "Exam",      "Enter and publish exam results"),
        ("Library.View",       "Library",   "View library catalog and issues"),
        ("Library.Manage",     "Library",   "Issue/return books and manage catalog"),
        ("Inventory.View",     "Inventory", "View inventory stock and invoices"),
        ("Inventory.Manage",   "Inventory", "Manage stock, invoices, and orders"),
    ];

    public static void Seed(AppDbContext db)
    {
        var existingKeys = db.Permissions.Select(p => p.Key).ToHashSet();

        var missing = All
            .Where(p => !existingKeys.Contains(p.Key))
            .Select(p => new Permission { Key = p.Key, Module = p.Module, Description = p.Description })
            .ToList();

        if (missing.Count == 0) return;

        db.Permissions.AddRange(missing);
        db.SaveChanges();
    }
}
