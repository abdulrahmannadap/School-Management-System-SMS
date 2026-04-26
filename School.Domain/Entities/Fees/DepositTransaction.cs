namespace School.Domain.Entities.Fees;

public class DepositTransaction
{
    public int      Id              { get; set; }
    public int      StudentId       { get; set; }
    public int      DepositMasterId { get; set; }
    public decimal  Amount          { get; set; }
    public DateTime Date            { get; set; }
    public string   Type            { get; set; } = string.Empty;
}
