using Microsoft.AspNetCore.Authorization;

namespace School.Web.Authorization;

public class SchoolAccessRequirement(params string[] allowedRoles) : IAuthorizationRequirement
{
    public string[] AllowedRoles { get; } = allowedRoles;
}
