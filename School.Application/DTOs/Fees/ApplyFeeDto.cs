namespace School.Application.DTOs.Fees;

public class ApplyFeeDto
{
    public List<int> StudentIds  { get; set; } = [];
    public int       FeeMasterId { get; set; }
    public decimal   Amount      { get; set; }
    public DateTime  DueDate     { get; set; }
}
