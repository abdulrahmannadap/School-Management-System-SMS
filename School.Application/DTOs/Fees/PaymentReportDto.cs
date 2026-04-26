namespace School.Application.DTOs.Fees;

public class PaymentReportDto
{
    public int      StudentId   { get; set; }
    public decimal  Amount      { get; set; }
    public DateTime PaymentDate { get; set; }
    public string   PaymentMode { get; set; } = string.Empty;
}
