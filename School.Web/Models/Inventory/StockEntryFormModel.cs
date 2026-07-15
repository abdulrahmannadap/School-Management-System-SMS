using System.ComponentModel.DataAnnotations;

namespace School.Web.Models.Inventory;

public class StockEntryFormModel
{
    [Range(1, int.MaxValue, ErrorMessage = "Product required")]
    public int ProductId { get; set; }

    [Range(0, 1000000, ErrorMessage = "Enter a valid quantity")]
    public decimal InQty { get; set; }

    [Range(0, 1000000, ErrorMessage = "Enter a valid quantity")]
    public decimal OutQty { get; set; }

    [Required(ErrorMessage = "Date required")]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Type required")]
    public string Type { get; set; } = "Purchase";

    public string ReferenceNo { get; set; } = string.Empty;
}
