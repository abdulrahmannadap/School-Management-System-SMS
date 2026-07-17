using School.Application.DTOs.Role;

namespace School.Application.Interfaces;

public interface IRoleService
{
    Task<IReadOnlyList<PermissionDto>> GetPermissionCatalogAsync(CancellationToken ct = default);

    Task<IReadOnlyList<RoleDto>> GetRolesAsync(Guid schoolId, CancellationToken ct = default);
    Task<RoleDto> GetRoleAsync(Guid roleId, CancellationToken ct = default);
    Task<RoleDto> CreateRoleAsync(Guid schoolId, RoleFormDto form, CancellationToken ct = default);
    Task<RoleDto> UpdateRoleAsync(Guid roleId, RoleFormDto form, CancellationToken ct = default);
    Task DeleteRoleAsync(Guid roleId, CancellationToken ct = default);

    /// <summary>Assigns a custom role to a user. Throws if the role and user belong to different schools.</summary>
    Task AssignRoleToUserAsync(Guid userId, Guid roleId, CancellationToken ct = default);

    /// <summary>Removes any custom role assignment from a user.</summary>
    Task UnassignRoleFromUserAsync(Guid userId, CancellationToken ct = default);

    /// <summary>Permission keys granted to a user through their assigned custom role, if any.</summary>
    Task<IReadOnlyList<string>> GetUserPermissionKeysAsync(Guid userId, CancellationToken ct = default);
}
