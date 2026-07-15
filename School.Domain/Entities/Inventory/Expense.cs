using School.Domain.Common;

namespace School.Domain.Entities.Inventory;

public class Expense : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int      Id          { get; set; }
    public string   ExpenseName { get; set; } = string.Empty;
    public decimal  Amount      { get; set; }
    public DateTime Date        { get; set; }
    public string   Category    { get; set; } = string.Empty;
}
