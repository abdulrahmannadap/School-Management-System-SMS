namespace School.Domain.Entities.Fees;

public class DepositMaster
{
    public int     Id          { get; set; }
    public string  DepositName { get; set; } = string.Empty;
    public decimal Amount      { get; set; }
}
