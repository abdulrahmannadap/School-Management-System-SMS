using School.Domain.Common;

namespace School.Domain.Entities.Staff;

public class LeaveBalance : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int Id          { get; set; }
    public int StaffId     { get; set; }
    public int LeaveTypeId { get; set; }
    public int Total       { get; set; }
    public int Used        { get; set; }
}
