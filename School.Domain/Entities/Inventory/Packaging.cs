namespace School.Domain.Entities.Inventory;

public class Packaging
{
    public int     Id   { get; set; }
    public string  Name { get; set; } = string.Empty;
    public decimal Cost { get; set; }
}
