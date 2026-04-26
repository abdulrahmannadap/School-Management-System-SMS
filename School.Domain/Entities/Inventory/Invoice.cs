namespace School.Domain.Entities.Inventory;

public class Invoice
{
    public int      Id            { get; set; }
    public string   InvoiceNo     { get; set; } = string.Empty;
    public DateTime Date          { get; set; }
    public string   CustomerName  { get; set; } = string.Empty;
    public decimal  TotalAmount   { get; set; }
    public decimal  PaidAmount    { get; set; }
    public decimal  PendingAmount { get; set; }
    public string   Status        { get; set; } = string.Empty;
}
