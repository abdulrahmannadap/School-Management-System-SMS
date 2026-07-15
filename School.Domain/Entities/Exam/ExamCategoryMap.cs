using School.Domain.Common;

namespace School.Domain.Entities.Exam;

public class ExamCategoryMap : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int    Id       { get; set; }
    public int    ExamId   { get; set; }
    public string Category { get; set; } = string.Empty;
}
