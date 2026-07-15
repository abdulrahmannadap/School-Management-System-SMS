using School.Domain.Common;

namespace School.Domain.Entities.Inventory;

public class Category : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int    Id   { get; set; }
    public string Name { get; set; } = string.Empty;
}
