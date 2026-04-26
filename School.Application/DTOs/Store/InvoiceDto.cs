namespace School.Application.DTOs.Inventory;

public class InvoiceDto
{
    public int     Id             { get; set; }
    public string  InvoiceNo      { get; set; } = string.Empty;
    public DateTime Date          { get; set; }
    public string  CustomerName   { get; set; } = string.Empty;
    public decimal TotalAmount    { get; set; }
    public decimal PaidAmount     { get; set; }
    public decimal PendingAmount  { get; set; }
    public string  Status         { get; set; } = string.Empty; // Paid | Partial | Unpaid | Cancelled
    public List<InvoiceItemDto> Items { get; set; } = [];
}
