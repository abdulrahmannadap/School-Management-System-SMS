using School.Application.Interfaces;

namespace School.Web.Services;

public class CurrentSchoolContext(IHttpContextAccessor httpContextAccessor) : ICurrentSchoolContext
{
    public Guid? SchoolId
    {
        get
        {
            var impersonated = httpContextAccessor.HttpContext?.Session.GetString("ImpersonatedSchoolId");
            if (Guid.TryParse(impersonated, out var overrideId))
                return overrideId;

            var value = httpContextAccessor.HttpContext?.User.FindFirst("SchoolId")?.Value;
            return Guid.TryParse(value, out var schoolId) ? schoolId : null;
        }
    }

    public bool IsImpersonating =>
        Guid.TryParse(httpContextAccessor.HttpContext?.Session.GetString("ImpersonatedSchoolId"), out _);
}
