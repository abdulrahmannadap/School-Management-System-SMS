using System.ComponentModel.DataAnnotations;

namespace School.Web.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email required")]
    [EmailAddress(ErrorMessage = "Invalid email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password required")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
