using School.Domain.Common;

namespace School.Domain.Entities.Inventory;

public class CreditPayment : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int      Id          { get; set; }
    public int      InvoiceId   { get; set; }
    public decimal  Amount      { get; set; }
    public DateTime PaymentDate { get; set; }
    public string   PaymentMode { get; set; } = string.Empty;
}
