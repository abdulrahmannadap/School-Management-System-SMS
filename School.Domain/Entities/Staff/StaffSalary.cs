using School.Domain.Common;

namespace School.Domain.Entities.Staff;

public class StaffSalary : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int     Id        { get; set; }
    public int     StaffId   { get; set; }
    public int     Month     { get; set; }
    public int     Year      { get; set; }
    public decimal NetSalary { get; set; }
    public DateTime GeneratedOn { get; set; } = DateTime.UtcNow;
}
