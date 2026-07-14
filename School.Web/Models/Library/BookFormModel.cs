using System.ComponentModel.DataAnnotations;

namespace School.Web.Models.Library;

public class BookFormModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title required")]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Author required")]
    [StringLength(150)]
    public string Author { get; set; } = string.Empty;

    [StringLength(30)]
    public string ISBN { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Category required")]
    public int CategoryId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Enter a valid quantity")]
    public int TotalQuantity { get; set; }
}
