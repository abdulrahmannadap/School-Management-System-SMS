using School.Domain.Common;

namespace School.Domain.Entities.Staff;

public class StaffRfid : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int    Id       { get; set; }
    public int    StaffId  { get; set; }
    public string RfidCode { get; set; } = string.Empty;
}
