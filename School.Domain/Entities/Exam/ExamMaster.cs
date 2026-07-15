using School.Domain.Common;

namespace School.Domain.Entities.Exam;

public class ExamMaster : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int      Id              { get; set; }
    public string   ExamName        { get; set; } = string.Empty;
    public int      FinancialYearId { get; set; }
    public DateTime StartDate       { get; set; }
    public DateTime EndDate         { get; set; }
    public bool     IsPublished     { get; set; }
}
