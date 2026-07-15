using School.Domain.Common;

namespace School.Domain.Entities.Exam;

public class ExamDetail : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int      Id           { get; set; }
    public int      ExamId       { get; set; }
    public int      SubjectId    { get; set; }
    public int      ClassId      { get; set; }
    public decimal  MaxMarks     { get; set; }
    public decimal  PassingMarks { get; set; }
    public DateTime ExamDate     { get; set; }
}
