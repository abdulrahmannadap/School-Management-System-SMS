using System.ComponentModel.DataAnnotations;

namespace School.Web.Models.Inventory;

public class InvoiceItemFormModel
{
    [Range(1, int.MaxValue, ErrorMessage = "Product required")]
    public int ProductId { get; set; }

    [Range(0.01, 1000000, ErrorMessage = "Enter a valid quantity")]
    public decimal Quantity { get; set; }

    [Range(0.01, 1000000, ErrorMessage = "Enter a valid rate")]
    public decimal Rate { get; set; }
}
