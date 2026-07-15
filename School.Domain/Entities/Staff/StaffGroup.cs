using School.Domain.Common;

namespace School.Domain.Entities.Staff;

public class StaffGroup : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int    Id        { get; set; }
    public string GroupName { get; set; } = string.Empty;
}
