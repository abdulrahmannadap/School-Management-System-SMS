using Microsoft.EntityFrameworkCore;
using School.Domain.Entities;
using School.Domain.Enums;

namespace School.Persistence;

public static partial class MenuSeedData
{
    /// <summary>Idempotent — safe to call on every startup. Seeds the SuperAdmin
    /// global tree and the per-role "template" rows (SchoolId = null) once, then
    /// clones a per-role tree into every School that doesn't have one yet.</summary>
    public static async Task SeedDefaultsAsync(AppDbContext db)
    {
        foreach (var (role, items) in Templates)
        {
            if (!await db.MenuItems.AnyAsync(m => m.SchoolId == null && m.Role == role))
                await InsertTreeAsync(db, null, role, items, null);
        }

        var schoolIds = await db.Schools.Select(s => s.Id).ToListAsync();
        foreach (var schoolId in schoolIds)
            await CloneTemplatesForSchoolAsync(db, schoolId);
    }

    /// <summary>Clones every non-SuperAdmin role's template tree for one school,
    /// skipping roles that already have a tree for that school.</summary>
    public static async Task CloneTemplatesForSchoolAsync(AppDbContext db, Guid schoolId)
    {
        foreach (var (role, items) in Templates)
        {
            if (role == UserRole.SuperAdmin) continue;
            if (await db.MenuItems.AnyAsync(m => m.SchoolId == schoolId && m.Role == role)) continue;

            await InsertTreeAsync(db, schoolId, role, items, null);
        }
    }

    /// <summary>Idempotent patch for Teacher trees seeded before the "Admission" link
    /// existed: ensures every "My Classes" parent has exactly one link to the Students
    /// page, labeled "Admission" — dropping the old separate "My Students" entry, since
    /// both pointed at the same page and showed as active together, which read as a bug.
    /// Safe to call on every startup; a no-op once every tree has been patched.</summary>
    public static async Task PatchTeacherAdmissionMenuAsync(AppDbContext db)
    {
        var parents = await db.MenuItems
            .Where(m => m.Role == UserRole.Teacher && m.Label == "My Classes")
            .ToListAsync();

        foreach (var parent in parents)
        {
            var children = await db.MenuItems
                .Where(m => m.ParentId == parent.Id)
                .ToListAsync();

            var admission  = children.FirstOrDefault(c => c.Label == "Admission");
            var myStudents = children.FirstOrDefault(c => c.Label == "My Students");

            if (admission is null && myStudents is not null)
            {
                // No "Admission" row yet — repurpose the old "My Students" row in place
                // rather than adding a new one, so its SortOrder/position is preserved.
                myStudents.Label        = "Admission";
                myStudents.IconCssClass = "bi-person-plus";
                myStudents.Area         = "Teacher";
                myStudents.Controller   = "Students";
                myStudents.Action       = "Index";
            }
            else if (admission is not null && myStudents is not null)
            {
                db.MenuItems.Remove(myStudents);
            }
            else if (admission is null && myStudents is null)
            {
                var sortOrder = children.Count > 0 ? children.Max(c => c.SortOrder) + 1 : 0;
                db.MenuItems.Add(new MenuItem
                {
                    SchoolId     = parent.SchoolId,
                    Role         = UserRole.Teacher,
                    ParentId     = parent.Id,
                    SectionLabel = null,
                    Label        = "Admission",
                    IconCssClass = "bi-person-plus",
                    Area         = "Teacher",
                    Controller   = "Students",
                    Action       = "Index",
                    SortOrder    = sortOrder,
                    IsEnabled    = true
                });
            }

            await db.SaveChangesAsync();
        }
    }

    /// <summary>Idempotent patch for Accountant trees seeded before the Fee Collection /
    /// Banking sub-pages existed: "Pending Fees" and "Fee Ledger" had no link at all, and
    /// "Refunds", "Discounts", "Cheques", "Deposits" all pointed at the same "Collect Fee"
    /// page as a placeholder. Repoints each label to its now-real dedicated action. Safe to
    /// call on every startup; a no-op once every tree has been patched.</summary>
    public static async Task PatchAccountantFeeMenuAsync(AppDbContext db)
    {
        var targets = new Dictionary<string, string>
        {
            ["Pending Fees"] = "PendingFees",
            ["Fee Ledger"]   = "Ledger",
            ["Refunds"]      = "Refunds",
            ["Discounts"]    = "Discounts",
            ["Cheques"]      = "Cheques",
            ["Deposits"]     = "Deposits",
        };

        var items = await db.MenuItems
            .Where(m => m.Role == UserRole.Accountant && targets.Keys.Contains(m.Label))
            .ToListAsync();

        foreach (var item in items)
        {
            var action = targets[item.Label];
            if (item.Area == "Accountant" && item.Controller == "Fees" && item.Action == action)
                continue;

            item.Area       = "Accountant";
            item.Controller = "Fees";
            item.Action     = action;
        }

        await db.SaveChangesAsync();
    }

    private static async Task InsertTreeAsync(
        AppDbContext db, Guid? schoolId, UserRole role, List<TemplateItem> items, int? parentId)
    {
        var sortOrder = 0;
        foreach (var item in items)
        {
            var entity = new MenuItem
            {
                SchoolId     = schoolId,
                Role         = role,
                ParentId     = parentId,
                SectionLabel = item.Section,
                Label        = item.Label,
                IconCssClass = item.Icon,
                Area         = item.Area,
                Controller   = item.Controller,
                Action       = item.Action,
                SortOrder    = sortOrder++,
                IsEnabled    = true
            };
            db.MenuItems.Add(entity);
            await db.SaveChangesAsync(); // need the generated Id before inserting children

            if (item.Children.Count > 0)
                await InsertTreeAsync(db, schoolId, role, item.Children, entity.Id);
        }
    }
}
