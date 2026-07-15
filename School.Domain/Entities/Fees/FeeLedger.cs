using School.Domain.Common;

namespace School.Domain.Entities.Fees;

public class FeeLedger : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int      Id          { get; set; }
    public int      StudentId   { get; set; }
    public int      FeeMasterId { get; set; }
    public decimal  Debit       { get; set; }
    public decimal  Credit      { get; set; }
    public DateTime Date        { get; set; }
    public string   Type        { get; set; } = string.Empty;
    public string   ReferenceNo { get; set; } = string.Empty;
}
