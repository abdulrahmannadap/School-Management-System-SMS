using School.Domain.Common;

namespace School.Domain.Entities.Masters;

public class Class : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int    Id       { get; set; }
    public string Name     { get; set; } = string.Empty;
    public int    OrderNo  { get; set; }
    public bool   IsActive { get; set; }
}
