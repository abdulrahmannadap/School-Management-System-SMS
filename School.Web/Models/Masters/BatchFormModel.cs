using System.ComponentModel.DataAnnotations;

namespace School.Web.Models.Masters;

public class BatchFormModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name required")]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}
