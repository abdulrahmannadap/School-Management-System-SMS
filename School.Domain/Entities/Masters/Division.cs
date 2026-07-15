using School.Domain.Common;

namespace School.Domain.Entities.Masters;

public class Division : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int    Id      { get; set; }
    public string Name    { get; set; } = string.Empty;
    public int    ClassId { get; set; }
}
