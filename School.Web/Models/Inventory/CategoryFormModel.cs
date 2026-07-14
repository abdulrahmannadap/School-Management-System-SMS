using System.ComponentModel.DataAnnotations;

namespace School.Web.Models.Inventory;

public class CategoryFormModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Category name required")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
}
