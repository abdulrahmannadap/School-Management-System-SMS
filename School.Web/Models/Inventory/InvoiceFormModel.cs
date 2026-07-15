using System.ComponentModel.DataAnnotations;

namespace School.Web.Models.Inventory;

public class InvoiceFormModel
{
    [Required(ErrorMessage = "Date required")]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Customer name required")]
    [StringLength(100)]
    public string CustomerName { get; set; } = string.Empty;

    [Range(0, 1000000000, ErrorMessage = "Enter a valid amount")]
    public decimal PaidAmount { get; set; }

    public List<InvoiceItemFormModel> Items { get; set; } = [];
}
