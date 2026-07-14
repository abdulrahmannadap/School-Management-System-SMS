using System.ComponentModel.DataAnnotations;
using School.Application.DTOs.Student;

namespace School.Web.Models.Fees;

public class ApplyFeeViewModel
{
    public int FeeMasterId { get; set; }
    public string FeeName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int ClassId { get; set; }
    public string ClassName { get; set; } = string.Empty;

    public IReadOnlyList<StudentBaseDto> Students { get; set; } = [];

    [Required(ErrorMessage = "Due date required")]
    [DataType(DataType.Date)]
    public DateTime DueDate { get; set; }

    public List<int> SelectedStudentIds { get; set; } = [];
}
