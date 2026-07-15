using School.Domain.Common;

namespace School.Domain.Entities.Fees;

public class FeeApplication : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int      Id          { get; set; }
    public int      StudentId   { get; set; }
    public int      FeeMasterId { get; set; }
    public decimal  Amount      { get; set; }
    public DateTime DueDate     { get; set; }
}
