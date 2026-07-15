using School.Domain.Common;

namespace School.Domain.Entities.Library;

public class LibraryBook : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int    Id                { get; set; }
    public string Title             { get; set; } = string.Empty;
    public string Author            { get; set; } = string.Empty;
    public string ISBN              { get; set; } = string.Empty;
    public int    CategoryId        { get; set; }
    public int    TotalQuantity     { get; set; }
    public int    AvailableQuantity { get; set; }
}
