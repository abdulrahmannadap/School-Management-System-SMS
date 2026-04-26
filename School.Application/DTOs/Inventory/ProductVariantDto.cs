namespace School.Application.DTOs.Inventory;

public class ProductVariantDto
{
    public int    Id          { get; set; }
    public int    ProductId   { get; set; }
    public string VariantType { get; set; } = string.Empty; // Size | Color | Unit
    public string Value       { get; set; } = string.Empty;
}
