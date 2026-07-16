using Microsoft.AspNetCore.Authorization;

namespace School.Web.Authorization;

public class SchoolAccessHandler(IHttpContextAccessor httpContextAccessor)
    : AuthorizationHandler<SchoolAccessRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, SchoolAccessRequirement requirement)
    {
        if (requirement.AllowedRoles.Any(context.User.IsInRole))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        var impersonating = httpContextAccessor.HttpContext?.Session.GetString("ImpersonatedSchoolId");
        if (context.User.IsInRole("SuperAdmin") && Guid.TryParse(impersonating, out _))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
