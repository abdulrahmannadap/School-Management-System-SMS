using School.Application.DTOs.Role;
using School.Application.Interfaces;
using School.Domain.Entities;

namespace School.Application.Services.Role;

public class RoleService(
    IGenericRepository<Domain.Entities.Role> roleRepo,
    IGenericRepository<Permission> permissionRepo,
    IGenericRepository<RolePermission> rolePermissionRepo,
    IGenericRepository<User> userRepo) : IRoleService
{
    public Task<IReadOnlyList<PermissionDto>> GetPermissionCatalogAsync(CancellationToken ct = default)
    {
        var permissions = permissionRepo.QueryNoTracking()
            .OrderBy(p => p.Module).ThenBy(p => p.Key)
            .Select(p => new PermissionDto { Id = p.Id, Key = p.Key, Module = p.Module, Description = p.Description })
            .ToList();

        return Task.FromResult<IReadOnlyList<PermissionDto>>(permissions);
    }

    public async Task<IReadOnlyList<RoleDto>> GetRolesAsync(Guid schoolId, CancellationToken ct = default)
    {
        var roles = await roleRepo.FindAsync(r => r.SchoolId == schoolId, ct);
        var permissionKeysByPermissionId = await GetAllPermissionKeysAsync(ct);
        var linksByRole = (await rolePermissionRepo.FindAsync(rp => true, ct))
            .GroupBy(rp => rp.RoleId)
            .ToDictionary(g => g.Key, g => g.ToList());

        return roles
            .OrderBy(r => r.Name)
            .Select(r => ToDto(r, linksByRole.GetValueOrDefault(r.Id, []), permissionKeysByPermissionId))
            .ToList();
    }

    public async Task<RoleDto> GetRoleAsync(Guid roleId, CancellationToken ct = default)
    {
        var role = await roleRepo.FirstOrDefaultAsync(r => r.Id == roleId, ct)
            ?? throw new KeyNotFoundException($"Role {roleId} not found.");

        var links = await rolePermissionRepo.FindAsync(rp => rp.RoleId == roleId, ct);
        var permissionKeysByPermissionId = await GetAllPermissionKeysAsync(ct);

        return ToDto(role, links, permissionKeysByPermissionId);
    }

    public async Task<RoleDto> CreateRoleAsync(Guid schoolId, RoleFormDto form, CancellationToken ct = default)
    {
        var nameTaken = await roleRepo.AnyAsync(r => r.SchoolId == schoolId && r.Name == form.Name, ct);
        if (nameTaken)
            throw new InvalidOperationException($"A role named '{form.Name}' already exists for this school.");

        var entity = new Domain.Entities.Role
        {
            SchoolId    = schoolId,
            Name        = form.Name,
            Description = form.Description
        };
        await roleRepo.AddAsync(entity, ct);
        await roleRepo.SaveChangesAsync(ct);

        await SyncPermissionsAsync(entity.Id, form.PermissionKeys, ct);

        return await GetRoleAsync(entity.Id, ct);
    }

    public async Task<RoleDto> UpdateRoleAsync(Guid roleId, RoleFormDto form, CancellationToken ct = default)
    {
        var entity = await roleRepo.FirstOrDefaultAsync(r => r.Id == roleId, ct)
            ?? throw new KeyNotFoundException($"Role {roleId} not found.");

        var nameTaken = await roleRepo.AnyAsync(r => r.SchoolId == entity.SchoolId && r.Name == form.Name && r.Id != roleId, ct);
        if (nameTaken)
            throw new InvalidOperationException($"A role named '{form.Name}' already exists for this school.");

        entity.Name        = form.Name;
        entity.Description = form.Description;
        roleRepo.Update(entity);
        await roleRepo.SaveChangesAsync(ct);

        await SyncPermissionsAsync(roleId, form.PermissionKeys, ct);

        return await GetRoleAsync(roleId, ct);
    }

    public async Task DeleteRoleAsync(Guid roleId, CancellationToken ct = default)
    {
        var entity = await roleRepo.FirstOrDefaultAsync(r => r.Id == roleId, ct)
            ?? throw new KeyNotFoundException($"Role {roleId} not found.");

        var usersOnRole = await userRepo.FindAsync(u => u.RoleId == roleId, ct);
        foreach (var user in usersOnRole)
        {
            user.RoleId = null;
            userRepo.Update(user);
        }

        var links = await rolePermissionRepo.FindAsync(rp => rp.RoleId == roleId, ct);
        rolePermissionRepo.DeleteRange(links);

        roleRepo.Delete(entity);
        await roleRepo.SaveChangesAsync(ct);
    }

    public async Task AssignRoleToUserAsync(Guid userId, Guid roleId, CancellationToken ct = default)
    {
        var user = await userRepo.FirstOrDefaultAsync(u => u.Id == userId, ct)
            ?? throw new KeyNotFoundException($"User {userId} not found.");

        var role = await roleRepo.FirstOrDefaultAsync(r => r.Id == roleId, ct)
            ?? throw new KeyNotFoundException($"Role {roleId} not found.");

        if (user.SchoolId != role.SchoolId)
            throw new InvalidOperationException("Role and user must belong to the same school.");

        user.RoleId = roleId;
        userRepo.Update(user);
        await userRepo.SaveChangesAsync(ct);
    }

    public async Task UnassignRoleFromUserAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await userRepo.FirstOrDefaultAsync(u => u.Id == userId, ct)
            ?? throw new KeyNotFoundException($"User {userId} not found.");

        user.RoleId = null;
        userRepo.Update(user);
        await userRepo.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<string>> GetUserPermissionKeysAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await userRepo.FirstOrDefaultAsync(u => u.Id == userId, ct);
        if (user?.RoleId is null) return [];

        var links = await rolePermissionRepo.FindAsync(rp => rp.RoleId == user.RoleId, ct);
        var permissionKeysByPermissionId = await GetAllPermissionKeysAsync(ct);

        return links
            .Select(l => permissionKeysByPermissionId.GetValueOrDefault(l.PermissionId))
            .Where(k => k is not null)
            .Select(k => k!)
            .ToList();
    }

    private async Task SyncPermissionsAsync(Guid roleId, List<string> permissionKeys, CancellationToken ct)
    {
        var existing = await rolePermissionRepo.FindAsync(rp => rp.RoleId == roleId, ct);
        rolePermissionRepo.DeleteRange(existing);

        if (permissionKeys.Count > 0)
        {
            var permissions = await permissionRepo.FindAsync(p => permissionKeys.Contains(p.Key), ct);
            await rolePermissionRepo.AddRangeAsync(
                permissions.Select(p => new RolePermission { RoleId = roleId, PermissionId = p.Id }), ct);
        }

        await rolePermissionRepo.SaveChangesAsync(ct);
    }

    private async Task<Dictionary<Guid, string>> GetAllPermissionKeysAsync(CancellationToken ct) =>
        (await permissionRepo.GetAllAsync(ct)).ToDictionary(p => p.Id, p => p.Key);

    private static RoleDto ToDto(Domain.Entities.Role r, IReadOnlyList<RolePermission> links, Dictionary<Guid, string> permissionKeysByPermissionId) => new()
    {
        Id             = r.Id,
        Name           = r.Name,
        Description    = r.Description,
        CreatedAt      = r.CreatedAt,
        PermissionKeys = links
            .Select(l => permissionKeysByPermissionId.GetValueOrDefault(l.PermissionId))
            .Where(k => k is not null)
            .Select(k => k!)
            .ToList()
    };
}
