namespace School.Application.DTOs.Fees;

public class ReceiptDto
{
    public int      ReceiptId   { get; set; }
    public int      StudentId   { get; set; }
    public decimal  Amount      { get; set; }
    public DateTime PaymentDate { get; set; }
    public string   PaymentMode { get; set; } = string.Empty;
    public string   ReferenceNo { get; set; } = string.Empty;
}
