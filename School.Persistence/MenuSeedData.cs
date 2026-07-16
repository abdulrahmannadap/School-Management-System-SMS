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
