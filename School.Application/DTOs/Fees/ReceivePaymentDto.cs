namespace School.Application.DTOs.Fees;

public class ReceivePaymentDto
{
    public int      StudentId   { get; set; }
    public decimal  Amount      { get; set; }
    public DateTime PaymentDate { get; set; }
    public string   PaymentMode { get; set; } = string.Empty;
    public string   ReferenceNo { get; set; } = string.Empty;
}
