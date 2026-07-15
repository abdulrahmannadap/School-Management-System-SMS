using School.Application.DTOs.Inventory;

namespace School.Web.Models.Inventory;

public class NewInvoiceViewModel
{
    public IReadOnlyList<ProductDto> Products { get; set; } = [];
    public InvoiceFormModel Form { get; set; } = new();
}
