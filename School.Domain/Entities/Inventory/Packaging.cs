using School.Domain.Common;

namespace School.Domain.Entities.Inventory;

public class Packaging : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int     Id   { get; set; }
    public string  Name { get; set; } = string.Empty;
    public decimal Cost { get; set; }
}
