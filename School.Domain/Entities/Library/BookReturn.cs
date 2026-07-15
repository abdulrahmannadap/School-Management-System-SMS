using School.Domain.Common;

namespace School.Domain.Entities.Library;

public class BookReturn : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int      Id         { get; set; }
    public int      IssueId    { get; set; }
    public DateTime ReturnDate { get; set; }
    public decimal  FineAmount { get; set; }
}
