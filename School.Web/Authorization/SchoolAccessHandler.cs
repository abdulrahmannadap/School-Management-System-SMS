using Microsoft.AspNetCore.Authorization;
using School.Application.Interfaces;

namespace School.Web.Authorization;

public class SchoolAccessHandler(ICurrentSchoolContext currentSchool, ISchoolService schoolSvc)
    : AuthorizationHandler<SchoolAccessRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, SchoolAccessRequirement requirement)
    {
        if (requirement.AllowedRoles.Any(context.User.IsInRole))
        {
            context.Succeed(requirement);
            return;
        }

        if (context.User.IsInRole("SuperAdmin") && currentSchool.IsImpersonating && currentSchool.SchoolId.HasValue)
        {
            var school = await schoolSvc.GetAsync(currentSchool.SchoolId.Value);
            if (school is { IsActive: true })
                context.Succeed(requirement);
        }
    }
}
