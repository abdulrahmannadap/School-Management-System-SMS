using School.Domain.Common;

namespace School.Domain.Entities.Inventory;

public class Product : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int     Id            { get; set; }
    public string  Name          { get; set; } = string.Empty;
    public int     CategoryId    { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal SellingPrice  { get; set; }
    public bool    IsActive      { get; set; } = true;
}
