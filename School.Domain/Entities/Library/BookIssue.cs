using School.Domain.Common;

namespace School.Domain.Entities.Library;

public class BookIssue : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int      Id        { get; set; }
    public int      BookId    { get; set; }
    public int?     StudentId { get; set; }
    public int?     StaffId   { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime DueDate   { get; set; }
    public bool     IsReturned{ get; set; }
}
