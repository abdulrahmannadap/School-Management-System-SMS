using System.ComponentModel.DataAnnotations;

namespace School.Web.Models.Exams;

public class ExamFormModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Exam name required")]
    [StringLength(150)]
    public string ExamName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Start date required")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "End date required")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }

    // Always supplied from the page's current Financial Year selection via a hidden
    // field, never user-typed — no [Required]/[Range] needed here.
    public int FinancialYearId { get; set; }
}
