namespace School.Application.DTOs.Inventory;

public class StockReportDto
{
    public int     ProductId { get; set; }
    public decimal TotalIn   { get; set; }
    public decimal TotalOut  { get; set; }
    public decimal Balance   { get; set; }
}
