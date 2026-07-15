using School.Domain.Common;

namespace School.Domain.Entities.Staff;

public class StaffSupervisor : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int Id           { get; set; }
    public int StaffId      { get; set; }
    public int SupervisorId { get; set; }
}
