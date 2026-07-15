using School.Domain.Common;

namespace School.Domain.Entities.Staff;

public class StaffPhoto : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int    Id        { get; set; }
    public int    StaffId   { get; set; }
    public string PhotoPath { get; set; } = string.Empty;
}
