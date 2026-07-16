using School.Application.Interfaces;

namespace School.API.Services;

public class CurrentSchoolContext(IHttpContextAccessor httpContextAccessor) : ICurrentSchoolContext
{
    public Guid? SchoolId
    {
        get
        {
            var value = httpContextAccessor.HttpContext?.User.FindFirst("SchoolId")?.Value;
            return Guid.TryParse(value, out var schoolId) ? schoolId : null;
        }
    }

    // The API has no session-based SuperAdmin impersonation (that's a Web-only feature).
    public bool IsImpersonating => false;
}
