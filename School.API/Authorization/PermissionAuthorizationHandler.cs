using Microsoft.AspNetCore.Authorization;

namespace School.API.Authorization;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var user = context.User;

        if (user.IsInRole("SuperAdmin") || user.IsInRole("SchoolAdmin"))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        if (user.HasClaim("perm", requirement.PermissionKey))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
