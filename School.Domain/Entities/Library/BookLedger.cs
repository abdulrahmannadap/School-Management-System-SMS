using School.Domain.Common;

namespace School.Domain.Entities.Library;

public class BookLedger : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int      Id        { get; set; }
    public int      BookId    { get; set; }
    public int?     StudentId { get; set; }
    public int?     StaffId   { get; set; }
    public DateTime Date      { get; set; }
    public string   Type      { get; set; } = string.Empty; // Issued | Returned
    public DateTime DueDate   { get; set; }
}
