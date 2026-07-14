using System.ComponentModel.DataAnnotations;

namespace School.Web.Models.Masters;

public class ClassFormModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name required")]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Range(0, 999, ErrorMessage = "Order must be a positive number")]
    public int OrderNo { get; set; }

    public bool IsActive { get; set; }
}
