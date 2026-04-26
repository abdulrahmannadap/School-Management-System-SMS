namespace School.Domain.Entities.Fees;

public class Voucher
{
    public int      Id          { get; set; }
    public DateTime Date        { get; set; }
    public string   Type        { get; set; } = string.Empty;
    public decimal  Amount      { get; set; }
    public string   Description { get; set; } = string.Empty;
}
