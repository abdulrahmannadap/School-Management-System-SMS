namespace School.Application.DTOs.Fees;

public class FeeRefundDto
{
    public int     StudentId { get; set; }
    public decimal Amount    { get; set; }
    public string  Reason    { get; set; } = string.Empty;
}
