using School.Domain.Common;

namespace School.Domain.Entities.Fees;

public class FeePayment : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int      Id          { get; set; }
    public int      StudentId   { get; set; }
    public decimal  Amount      { get; set; }
    public DateTime PaymentDate { get; set; }
    public string   PaymentMode { get; set; } = string.Empty;
    public string   ReferenceNo { get; set; } = string.Empty;
}
