using System.ComponentModel.DataAnnotations;

namespace School.Web.Models.SuperAdmin;

public class SchoolFormModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "School name required")]
    [StringLength(150)]
    public string Name { get; set; } = string.Empty;

    [StringLength(250)]
    public string Address { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Enter a valid email")]
    public string ContactEmail { get; set; } = string.Empty;

    [StringLength(20)]
    public string ContactPhone { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
