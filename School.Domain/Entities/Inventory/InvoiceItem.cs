namespace School.Domain.Entities.Inventory;

public class InvoiceItem
{
    public int     Id        { get; set; }
    public int     InvoiceId { get; set; }
    public int     ProductId { get; set; }
    public decimal Quantity  { get; set; }
    public decimal Rate      { get; set; }
    public decimal Amount    { get; set; }
}
