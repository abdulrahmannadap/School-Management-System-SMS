using Microsoft.AspNetCore.Authorization;

namespace School.API.Authorization;

public class PermissionRequirement(string permissionKey) : IAuthorizationRequirement
{
    public string PermissionKey { get; } = permissionKey;
}
