using School.Domain.Common;

namespace School.Domain.Entities.Staff;

public class StaffSignature : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int    Id            { get; set; }
    public int    StaffId       { get; set; }
    public string SignaturePath { get; set; } = string.Empty;
}
