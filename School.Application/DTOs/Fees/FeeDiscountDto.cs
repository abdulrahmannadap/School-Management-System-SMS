namespace School.Application.DTOs.Fees;

public class FeeDiscountDto
{
    public int     StudentId { get; set; }
    public decimal Amount    { get; set; }
    public string  Reason    { get; set; } = string.Empty;
}
