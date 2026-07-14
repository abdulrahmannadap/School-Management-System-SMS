using School.Application.DTOs.Inventory;

namespace School.Web.Models.Inventory;

public class ProductsIndexViewModel
{
    public IReadOnlyList<ProductDto> Items { get; set; } = [];
    public IReadOnlyList<CategoryDto> Categories { get; set; } = [];
    public int? SelectedCategoryId { get; set; }
    public ProductFormModel Form { get; set; } = new();
    public CategoryFormModel CategoryForm { get; set; } = new();
    public bool ShowModal { get; set; }
    public bool ShowCategoryModal { get; set; }
}
