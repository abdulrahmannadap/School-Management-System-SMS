namespace School.Application.DTOs.Student;

public class PromoteStudentDto
{
    public int NewFinancialYearId { get; set; }
    public int StudentId         { get; set; }
    public int NewClassId        { get; set; }
    public int NewDivisionId     { get; set; }
}
