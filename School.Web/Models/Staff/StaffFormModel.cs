using System.ComponentModel.DataAnnotations;
using School.Domain.Enums;

namespace School.Web.Models.Staff;

public class StaffFormModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Full name required")]
    [StringLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mobile required")]
    [StringLength(20)]
    public string Mobile { get; set; } = string.Empty;

    [Required(ErrorMessage = "Designation required")]
    [StringLength(100)]
    public string Designation { get; set; } = string.Empty;

    // Only required on create (Id == 0); validated manually in the controller since
    // Update (edit) doesn't accept/need a joining date and the field is hidden then.
    [DataType(DataType.Date)]
    public DateTime JoiningDate { get; set; }

    // Only used on create (Id == 0) to pick the login role for the auto-created account.
    public UserRole LoginRole { get; set; } = UserRole.Staff;

    [EmailAddress(ErrorMessage = "Invalid email")]
    public string? Email { get; set; }

    public bool IsActive { get; set; }
}
