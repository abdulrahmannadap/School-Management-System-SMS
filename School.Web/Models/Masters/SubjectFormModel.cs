using System.ComponentModel.DataAnnotations;

namespace School.Web.Models.Masters;

public class SubjectFormModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name required")]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Class required")]
    [Range(1, int.MaxValue, ErrorMessage = "Class required")]
    public int ClassId { get; set; }

    public bool IsActive { get; set; }
}
