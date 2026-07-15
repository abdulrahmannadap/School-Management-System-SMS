using School.Domain.Common;

namespace School.Domain.Entities.Staff;

public class StaffHoliday : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int      Id   { get; set; }
    public DateTime Date { get; set; }
    public string   Name { get; set; } = string.Empty;
}
