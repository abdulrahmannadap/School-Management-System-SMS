using School.Domain.Common;

namespace School.Domain.Entities.Exam;

public class MarksEntry : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int     Id            { get; set; }
    public int     StudentId     { get; set; }
    public int     ExamId        { get; set; }
    public int     SubjectId     { get; set; }
    public decimal MarksObtained { get; set; }
    public bool    IsAbsent      { get; set; }
}
