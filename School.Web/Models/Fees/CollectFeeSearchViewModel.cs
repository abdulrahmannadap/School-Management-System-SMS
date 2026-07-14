using School.Application.DTOs.Student;

namespace School.Web.Models.Fees;

public class CollectFeeSearchViewModel
{
    public StudentSearchDto Search { get; set; } = new();
    public IReadOnlyList<StudentBaseDto> Results { get; set; } = [];
}
