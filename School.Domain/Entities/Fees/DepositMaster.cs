using School.Domain.Common;

namespace School.Domain.Entities.Fees;

public class DepositMaster : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int     Id          { get; set; }
    public string  DepositName { get; set; } = string.Empty;
    public decimal Amount      { get; set; }
}
