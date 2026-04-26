namespace School.Application.DTOs.Student;

public class StudentSearchDto
{
    public string? Name            { get; set; }
    public string? GRNumber        { get; set; }
    public int?    ClassId         { get; set; }
    public int?    DivisionId      { get; set; }
    public int?    FinancialYearId { get; set; }
}
