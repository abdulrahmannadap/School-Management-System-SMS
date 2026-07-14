using School.Application.DTOs.Masters;
using School.Application.DTOs.Student;

namespace School.Web.Models.Students;

public class StudentsIndexViewModel
{
    public IReadOnlyList<StudentBaseDto> Items { get; set; } = [];
    public IReadOnlyList<ClassDto> Classes { get; set; } = [];
    public IReadOnlyList<DivisionDto> Divisions { get; set; } = [];
    public IReadOnlyList<FinancialYearDto> FinancialYears { get; set; } = [];
    public StudentSearchDto Search { get; set; } = new();
    public StudentFormModel Form { get; set; } = new();
    public bool ShowModal { get; set; }
}
