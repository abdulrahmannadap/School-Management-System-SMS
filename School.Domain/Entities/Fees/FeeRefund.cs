namespace School.Domain.Entities.Fees;

public class FeeRefund
{
    public int      Id        { get; set; }
    public int      StudentId { get; set; }
    public decimal  Amount    { get; set; }
    public string   Reason    { get; set; } = string.Empty;
    public DateTime Date      { get; set; } = DateTime.UtcNow;
}
