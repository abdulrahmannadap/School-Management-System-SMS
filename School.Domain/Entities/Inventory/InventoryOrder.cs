namespace School.Domain.Entities.Inventory;

public class InventoryOrder
{
    public int      Id           { get; set; }
    public string   OrderNo      { get; set; } = string.Empty;
    public DateTime Date         { get; set; }
    public string   CustomerName { get; set; } = string.Empty;
    public string   Status       { get; set; } = string.Empty;
}
