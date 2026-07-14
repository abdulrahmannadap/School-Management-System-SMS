using System.ComponentModel.DataAnnotations;

namespace School.Web.Models.Library;

public class BookCategoryFormModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Category name required")]
    [StringLength(100)]
    public string CategoryName { get; set; } = string.Empty;
}
