namespace School.Application.DTOs.Inventory;

public class CreditPaymentDto
{
    public int      Id          { get; set; }
    public int      InvoiceId   { get; set; }
    public decimal  Amount      { get; set; }
    public DateTime PaymentDate { get; set; }
    public string   PaymentMode { get; set; } = string.Empty; // Cash | Card | Bank | Online
}
