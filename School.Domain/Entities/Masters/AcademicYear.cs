using School.Domain.Common;

namespace School.Domain.Entities.Masters;

public class AcademicYear : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int    Id       { get; set; }
    public string Name     { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate   { get; set; }
    public bool   IsActive { get; set; }
}
