using School.Domain.Common;

namespace School.Domain.Entities.Exam;

public class ExamGroupMap : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int    Id        { get; set; }
    public int    ExamId    { get; set; }
    public string GroupName { get; set; } = string.Empty;
}
