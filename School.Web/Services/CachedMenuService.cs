using Microsoft.Extensions.Caching.Memory;
using School.Application.DTOs.Menu;
using School.Application.Interfaces;
using School.Application.Services;
using School.Domain.Enums;

namespace School.Web.Services;

/// <summary>
/// Caches menu trees (IMemoryCache lives at the Web layer, not Application,
/// matching this codebase's existing convention). Every write invalidates
/// only the affected school+role's cache entry.
/// </summary>
public class CachedMenuService(MenuService inner, IMemoryCache cache) : IMenuService
{
    private static string Key(Guid? schoolId, UserRole role) => $"menu:{schoolId}:{role}";

    public async Task<IReadOnlyList<MenuItemDto>> GetMenuTreeAsync(Guid? schoolId, UserRole role, CancellationToken ct = default)
    {
        var cached = await cache.GetOrCreateAsync(Key(schoolId, role), async entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromMinutes(30);
            return await inner.GetMenuTreeAsync(schoolId, role, ct);
        });
        return cached ?? [];
    }

    public Task<IReadOnlyList<MenuItemDto>> GetAllForManagementAsync(Guid? schoolId, UserRole role, CancellationToken ct = default) =>
        inner.GetAllForManagementAsync(schoolId, role, ct);

    public async Task<MenuItemFormDto> CreateAsync(MenuItemFormDto form, CancellationToken ct = default)
    {
        var result = await inner.CreateAsync(form, ct);
        cache.Remove(Key(form.SchoolId, form.Role));
        return result;
    }

    public async Task<MenuItemFormDto> UpdateAsync(MenuItemFormDto form, CancellationToken ct = default)
    {
        var result = await inner.UpdateAsync(form, ct);
        cache.Remove(Key(form.SchoolId, form.Role));
        return result;
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        await inner.DeleteAsync(id, ct);
        InvalidateAll(); // deletion doesn't carry schoolId/role context here; safe, cheap fallback
    }

    public async Task SetEnabledAsync(int id, bool enabled, CancellationToken ct = default)
    {
        await inner.SetEnabledAsync(id, enabled, ct);
        InvalidateAll();
    }

    public async Task ReorderAsync(IReadOnlyList<int> orderedIds, CancellationToken ct = default)
    {
        await inner.ReorderAsync(orderedIds, ct);
        InvalidateAll();
    }

    public async Task CloneDefaultsForSchoolAsync(Guid schoolId, CancellationToken ct = default)
    {
        await inner.CloneDefaultsForSchoolAsync(schoolId, ct);
    }

    // MemoryCache has no built-in "clear all" API; a Compact(1.0) evicts everything
    // it can, which is an acceptable, infrequent cost for an admin-only write path.
    private void InvalidateAll()
    {
        if (cache is MemoryCache mc) mc.Compact(1.0);
    }
}
