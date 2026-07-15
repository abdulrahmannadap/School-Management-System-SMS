using School.Domain.Common;

namespace School.Domain.Entities.Masters;

public class Batch : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int    Id       { get; set; }
    public string Name     { get; set; } = string.Empty;
    public bool   IsActive { get; set; }
}
