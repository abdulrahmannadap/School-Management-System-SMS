namespace School.Application.DTOs.Inventory;

public class ProductDto
{
    public int     Id            { get; set; }
    public string  Name          { get; set; } = string.Empty;
    public int     CategoryId    { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal SellingPrice  { get; set; }
    public bool    IsActive      { get; set; }
}
