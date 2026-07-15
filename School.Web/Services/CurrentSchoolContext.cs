using School.Application.Interfaces;

namespace School.Web.Services;

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
}
