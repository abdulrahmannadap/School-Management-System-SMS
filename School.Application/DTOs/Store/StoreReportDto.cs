namespace School.Application.DTOs.Inventory;

public class StoreReportDto
{
    public int     ProductId    { get; set; }
    public decimal CurrentStock { get; set; }
    public decimal StockValue   { get; set; }
}
