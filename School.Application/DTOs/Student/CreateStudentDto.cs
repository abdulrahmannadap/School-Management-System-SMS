namespace School.Application.DTOs.Student;

public class CreateStudentDto
{
    public int      FinancialYearId { get; set; }
    public int      ClassId         { get; set; }
    public int      DivisionId      { get; set; }
    public string   FullName        { get; set; } = string.Empty;
    public string   Gender          { get; set; } = string.Empty;
    public DateTime DateOfBirth     { get; set; }
    public string   FatherName      { get; set; } = string.Empty;
    public string   FatherMobile    { get; set; } = string.Empty;
}
