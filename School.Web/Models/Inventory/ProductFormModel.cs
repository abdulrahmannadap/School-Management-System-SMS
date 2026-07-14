using System.ComponentModel.DataAnnotations;

namespace School.Web.Models.Inventory;

public class ProductFormModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name required")]
    [StringLength(150)]
    public string Name { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Category required")]
    public int CategoryId { get; set; }

    [Range(0.01, 100000000, ErrorMessage = "Enter a valid purchase price")]
    public decimal PurchasePrice { get; set; }

    [Range(0.01, 100000000, ErrorMessage = "Enter a valid selling price")]
    public decimal SellingPrice { get; set; }

    public bool IsActive { get; set; } = true;
}
