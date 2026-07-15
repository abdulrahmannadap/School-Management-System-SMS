using School.Domain.Common;

namespace School.Domain.Entities.Exam;

public class ExamRemark : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int    Id        { get; set; }
    public int    StudentId { get; set; }
    public int    ExamId    { get; set; }
    public string Remark    { get; set; } = string.Empty;
}
