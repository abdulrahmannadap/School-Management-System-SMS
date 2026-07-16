using School.Application.DTOs.Menu;
using School.Domain.Enums;

namespace School.Application.Interfaces;

public interface IMenuService
{
    Task<IReadOnlyList<MenuItemDto>> GetMenuTreeAsync(Guid? schoolId, UserRole role, CancellationToken ct = default);

    /// <summary>Management-only: returns every item (including disabled ones), uncached.</summary>
    Task<IReadOnlyList<MenuItemDto>> GetAllForManagementAsync(Guid? schoolId, UserRole role, CancellationToken ct = default);

    Task<MenuItemFormDto> CreateAsync(MenuItemFormDto form, CancellationToken ct = default);
    Task<MenuItemFormDto> UpdateAsync(MenuItemFormDto form, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
    Task SetEnabledAsync(int id, bool enabled, CancellationToken ct = default);

    /// <summary>orderedIds is the full set of sibling ids (same ParentId) in their new order.</summary>
    Task ReorderAsync(IReadOnlyList<int> orderedIds, CancellationToken ct = default);

    /// <summary>Clones the given role's default template (SchoolId = null rows) for a newly created school.</summary>
    Task CloneDefaultsForSchoolAsync(Guid schoolId, CancellationToken ct = default);
}
