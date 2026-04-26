namespace School.Domain.Entities.Exam;

public class ExamMaster
{
    public int      Id              { get; set; }
    public string   ExamName        { get; set; } = string.Empty;
    public int      FinancialYearId { get; set; }
    public DateTime StartDate       { get; set; }
    public DateTime EndDate         { get; set; }
    public bool     IsPublished     { get; set; }
}
