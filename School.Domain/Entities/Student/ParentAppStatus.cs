using School.Domain.Common;

namespace School.Domain.Entities.Student;

public class ParentAppStatus : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int       Id          { get; set; }
    public int       StudentId   { get; set; }
    public bool      IsInstalled { get; set; }
    public DateTime? InstalledOn { get; set; }
}
