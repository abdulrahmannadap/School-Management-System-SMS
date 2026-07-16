using School.Application.DTOs.Menu;
using School.Application.Interfaces;
using School.Domain.Entities;
using School.Domain.Enums;

namespace School.Application.Services;

public class MenuService(IGenericRepository<MenuItem> menuRepo) : IMenuService
{
    public async Task<IReadOnlyList<MenuItemDto>> GetMenuTreeAsync(Guid? schoolId, UserRole role, CancellationToken ct = default)
    {
        var flat = menuRepo.QueryNoTracking()
            .Where(m => m.SchoolId == schoolId && m.Role == role && m.IsEnabled)
            .OrderBy(m => m.SortOrder)
            .ToList();

        return BuildTree(flat, null);
    }

    public async Task<IReadOnlyList<MenuItemDto>> GetAllForManagementAsync(Guid? schoolId, UserRole role, CancellationToken ct = default)
    {
        var flat = menuRepo.QueryNoTracking()
            .Where(m => m.SchoolId == schoolId && m.Role == role)
            .OrderBy(m => m.SortOrder)
            .ToList();

        return BuildTree(flat, null);
    }

    private static List<MenuItemDto> BuildTree(List<MenuItem> flat, int? parentId) =>
        flat.Where(m => m.ParentId == parentId)
            .Select(m => new MenuItemDto
            {
                Id           = m.Id,
                ParentId     = m.ParentId,
                SectionLabel = m.SectionLabel,
                Label        = m.Label,
                IconCssClass = m.IconCssClass,
                Area         = m.Area,
                Controller   = m.Controller,
                Action       = m.Action,
                SortOrder    = m.SortOrder,
                IsEnabled    = m.IsEnabled,
                ModuleKey    = m.ModuleKey,
                Children     = BuildTree(flat, m.Id)
            })
            .ToList();

    public async Task<MenuItemFormDto> CreateAsync(MenuItemFormDto form, CancellationToken ct = default)
    {
        var maxOrder = menuRepo.QueryNoTracking()
            .Where(m => m.SchoolId == form.SchoolId && m.Role == form.Role && m.ParentId == form.ParentId)
            .Select(m => (int?)m.SortOrder)
            .Max() ?? -1;

        var entity = new MenuItem
        {
            SchoolId     = form.SchoolId,
            Role         = form.Role,
            ParentId     = form.ParentId,
            SectionLabel = form.SectionLabel,
            Label        = form.Label,
            IconCssClass = form.IconCssClass,
            Area         = form.Area,
            Controller   = form.Controller,
            Action       = form.Action,
            SortOrder    = maxOrder + 1,
            IsEnabled    = form.IsEnabled,
            ModuleKey    = form.ModuleKey
        };
        await menuRepo.AddAsync(entity, ct);
        await menuRepo.SaveChangesAsync(ct);

        form.Id = entity.Id;
        return form;
    }

    public async Task<MenuItemFormDto> UpdateAsync(MenuItemFormDto form, CancellationToken ct = default)
    {
        var entity = await menuRepo.FirstOrDefaultAsync(m => m.Id == form.Id, ct)
            ?? throw new KeyNotFoundException($"MenuItem {form.Id} not found.");

        entity.SectionLabel = form.SectionLabel;
        entity.Label        = form.Label;
        entity.IconCssClass = form.IconCssClass;
        entity.Area         = form.Area;
        entity.Controller   = form.Controller;
        entity.Action       = form.Action;
        entity.IsEnabled    = form.IsEnabled;
        entity.ModuleKey    = form.ModuleKey;
        entity.UpdatedAt    = DateTime.UtcNow;

        menuRepo.Update(entity);
        await menuRepo.SaveChangesAsync(ct);
        return form;
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await menuRepo.FirstOrDefaultAsync(m => m.Id == id, ct)
            ?? throw new KeyNotFoundException($"MenuItem {id} not found.");

        var children = await menuRepo.FindAsync(m => m.ParentId == id, ct);
        if (children.Count > 0)
            menuRepo.DeleteRange(children);

        menuRepo.Delete(entity);
        await menuRepo.SaveChangesAsync(ct);
    }

    public async Task SetEnabledAsync(int id, bool enabled, CancellationToken ct = default)
    {
        var entity = await menuRepo.FirstOrDefaultAsync(m => m.Id == id, ct)
            ?? throw new KeyNotFoundException($"MenuItem {id} not found.");

        entity.IsEnabled = enabled;
        entity.UpdatedAt = DateTime.UtcNow;
        menuRepo.Update(entity);
        await menuRepo.SaveChangesAsync(ct);
    }

    public async Task ReorderAsync(IReadOnlyList<int> orderedIds, CancellationToken ct = default)
    {
        for (var i = 0; i < orderedIds.Count; i++)
        {
            var entity = await menuRepo.FirstOrDefaultAsync(m => m.Id == orderedIds[i], ct);
            if (entity is null) continue;
            entity.SortOrder = i;
            menuRepo.Update(entity);
        }
        await menuRepo.SaveChangesAsync(ct);
    }

    public async Task CloneDefaultsForSchoolAsync(Guid schoolId, CancellationToken ct = default)
    {
        foreach (var role in Enum.GetValues<UserRole>())
        {
            if (role == UserRole.SuperAdmin) continue;

            var templates = menuRepo.QueryNoTracking()
                .Where(m => m.SchoolId == null && m.Role == role)
                .OrderBy(m => m.Id)
                .ToList();
            if (templates.Count == 0) continue;

            // old template Id -> new cloned entity, so children can be re-parented correctly.
            var idMap = new Dictionary<int, MenuItem>();
            foreach (var template in templates)
            {
                var clone = new MenuItem
                {
                    SchoolId     = schoolId,
                    Role         = role,
                    SectionLabel = template.SectionLabel,
                    Label        = template.Label,
                    IconCssClass = template.IconCssClass,
                    Area         = template.Area,
                    Controller   = template.Controller,
                    Action       = template.Action,
                    SortOrder    = template.SortOrder,
                    IsEnabled    = template.IsEnabled,
                    ModuleKey    = template.ModuleKey
                };
                idMap[template.Id] = clone;
            }
            foreach (var template in templates)
            {
                if (template.ParentId.HasValue && idMap.TryGetValue(template.ParentId.Value, out var parentClone))
                    idMap[template.Id].ParentId = null; // set below once parent has an Id

                await menuRepo.AddAsync(idMap[template.Id], ct);
            }
            await menuRepo.SaveChangesAsync(ct); // flush to get generated Ids for the parent-child fixup below

            foreach (var template in templates)
            {
                if (template.ParentId.HasValue && idMap.TryGetValue(template.ParentId.Value, out var parentClone))
                {
                    idMap[template.Id].ParentId = parentClone.Id;
                    menuRepo.Update(idMap[template.Id]);
                }
            }
            await menuRepo.SaveChangesAsync(ct);
        }
    }
}
