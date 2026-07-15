using System.ComponentModel.DataAnnotations;

namespace School.Web.Models.Inventory;

public class CreditPaymentFormModel
{
    public int InvoiceId { get; set; }

    [Range(0.01, 1000000000, ErrorMessage = "Enter a valid amount")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Payment date required")]
    [DataType(DataType.Date)]
    public DateTime PaymentDate { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Payment mode required")]
    public string PaymentMode { get; set; } = string.Empty;
}
