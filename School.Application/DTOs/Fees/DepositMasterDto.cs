namespace School.Application.DTOs.Fees;

public class DepositMasterDto
{
    public int     Id          { get; set; }
    public string  DepositName { get; set; } = string.Empty;
    public decimal Amount      { get; set; }
}
