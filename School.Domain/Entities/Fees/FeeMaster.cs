using School.Domain.Common;

namespace School.Domain.Entities.Fees;

public class FeeMaster : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int     Id              { get; set; }
    public string  FeeName         { get; set; } = string.Empty;
    public decimal Amount          { get; set; }
    public int     ClassId         { get; set; }
    public int     FinancialYearId { get; set; }
    public bool    IsRecurring     { get; set; }
}
