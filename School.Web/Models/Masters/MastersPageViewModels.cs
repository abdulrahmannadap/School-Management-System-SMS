using School.Application.DTOs.Masters;

namespace School.Web.Models.Masters;

public class AcademicYearsViewModel
{
    public IReadOnlyList<AcademicYearDto> Items { get; set; } = [];
    public AcademicYearFormModel Form { get; set; } = new();
    public bool ShowModal { get; set; }
}

public class FinancialYearsViewModel
{
    public IReadOnlyList<FinancialYearDto> Items { get; set; } = [];
    public FinancialYearFormModel Form { get; set; } = new();
    public bool ShowModal { get; set; }
}

public class ClassesViewModel
{
    public IReadOnlyList<ClassDto> Items { get; set; } = [];
    public ClassFormModel Form { get; set; } = new();
    public bool ShowModal { get; set; }
}

public class DivisionsViewModel
{
    public IReadOnlyList<DivisionDto> Items { get; set; } = [];
    public IReadOnlyList<ClassDto> Classes { get; set; } = [];
    public DivisionFormModel Form { get; set; } = new();
    public bool ShowModal { get; set; }
}

public class BatchesViewModel
{
    public IReadOnlyList<BatchDto> Items { get; set; } = [];
    public BatchFormModel Form { get; set; } = new();
    public bool ShowModal { get; set; }
}

public class SubjectsViewModel
{
    public IReadOnlyList<SubjectDto> Items { get; set; } = [];
    public IReadOnlyList<ClassDto> Classes { get; set; } = [];
    public SubjectFormModel Form { get; set; } = new();
    public bool ShowModal { get; set; }
}
