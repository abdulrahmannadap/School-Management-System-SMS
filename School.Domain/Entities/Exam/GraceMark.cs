using School.Domain.Common;

namespace School.Domain.Entities.Exam;

public class GraceMark : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int     Id         { get; set; }
    public int     StudentId  { get; set; }
    public int     ExamId     { get; set; }
    public int     SubjectId  { get; set; }
    public decimal GraceMarks { get; set; }
}
