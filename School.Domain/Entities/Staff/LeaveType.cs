using School.Domain.Common;

namespace School.Domain.Entities.Staff;

public class LeaveType : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int    Id      { get; set; }
    public string Name    { get; set; } = string.Empty;
    public int    MaxDays { get; set; }
}
