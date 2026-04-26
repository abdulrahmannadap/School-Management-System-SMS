namespace School.Application.DTOs.Inventory;

public class CancelInvoiceDto
{
    public int    InvoiceId { get; set; }
    public string Reason    { get; set; } = string.Empty;
}
