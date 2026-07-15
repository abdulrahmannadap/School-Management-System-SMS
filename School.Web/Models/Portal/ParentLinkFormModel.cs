using System.ComponentModel.DataAnnotations;

namespace School.Web.Models.Portal;

public class ParentLinkFormModel
{
    [Required(ErrorMessage = "Email required")]
    [EmailAddress(ErrorMessage = "Enter a valid email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password required")]
    [StringLength(100, MinimumLength = 4)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Name required")]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Student GR Number required")]
    public string GRNumber { get; set; } = string.Empty;
}
