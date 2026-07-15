using School.Domain.Common;

namespace School.Domain.Entities.Library;

public class BookCategory : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int    Id           { get; set; }
    public string CategoryName { get; set; } = string.Empty;
}
