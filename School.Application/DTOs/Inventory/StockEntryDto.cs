namespace School.Application.DTOs.Inventory;

public class StockEntryDto
{
    public int      ProductId   { get; set; }
    public decimal  InQty       { get; set; }
    public decimal  OutQty      { get; set; }
    public DateTime Date        { get; set; }
    public string   Type        { get; set; } = string.Empty; // Purchase | Adjustment | Return
    public string   ReferenceNo { get; set; } = string.Empty;
}

public class StockBalanceDto
{
    public int     ProductId       { get; set; }
    public string  ProductName     { get; set; } = string.Empty;
    public decimal TotalIn         { get; set; }
    public decimal TotalOut        { get; set; }
    public decimal CurrentStock    { get; set; }
}
