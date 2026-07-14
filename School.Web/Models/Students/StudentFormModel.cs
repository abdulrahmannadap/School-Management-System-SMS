using System.ComponentModel.DataAnnotations;

namespace School.Web.Models.Students;

public class StudentFormModel
{
    public int Id { get; set; }

    // Only required on create (Id == 0); validated manually in the controller since
    // Update (edit) doesn't accept/need a financial year and the field is hidden then.
    public int FinancialYearId { get; set; }

    [Required(ErrorMessage = "Class required")]
    [Range(1, int.MaxValue, ErrorMessage = "Class required")]
    public int ClassId { get; set; }

    [Required(ErrorMessage = "Division required")]
    [Range(1, int.MaxValue, ErrorMessage = "Division required")]
    public int DivisionId { get; set; }

    [Required(ErrorMessage = "Full name required")]
    [StringLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Gender required")]
    public string Gender { get; set; } = string.Empty;

    [Required(ErrorMessage = "Date of birth required")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email")]
    public string? Email { get; set; }

    [StringLength(150)]
    public string FatherName { get; set; } = string.Empty;

    [StringLength(20)]
    public string FatherMobile { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}
