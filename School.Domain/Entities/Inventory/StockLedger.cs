namespace School.Domain.Entities.Inventory;

public class StockLedger
{
    public int      Id          { get; set; }
    public int      ProductId   { get; set; }
    public decimal  InQty       { get; set; }
    public decimal  OutQty      { get; set; }
    public DateTime Date        { get; set; }
    public string   Type        { get; set; } = string.Empty;
    public string   ReferenceNo { get; set; } = string.Empty;
}
