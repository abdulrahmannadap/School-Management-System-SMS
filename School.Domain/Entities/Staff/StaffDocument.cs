using School.Domain.Common;

namespace School.Domain.Entities.Staff;

public class StaffDocument : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int    Id           { get; set; }
    public int    StaffId      { get; set; }
    public string DocumentName { get; set; } = string.Empty;
    public string FilePath     { get; set; } = string.Empty;
}
