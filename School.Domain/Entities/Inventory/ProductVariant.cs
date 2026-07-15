using School.Domain.Common;

namespace School.Domain.Entities.Inventory;

public class ProductVariant : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int    Id          { get; set; }
    public int    ProductId   { get; set; }
    public string VariantType { get; set; } = string.Empty;
    public string Value       { get; set; } = string.Empty;
}
