namespace School.Application.DTOs.Fees;

public class FeePendingDto
{
    public int     StudentId     { get; set; }
    public decimal TotalFees     { get; set; }
    public decimal PaidAmount    { get; set; }
    public decimal PendingAmount { get; set; }
}
