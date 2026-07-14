using System.ComponentModel.DataAnnotations;

namespace School.Web.Models.Fees;

public class ReceivePaymentFormModel
{
    public int StudentId { get; set; }

    [Range(0.01, 100000000, ErrorMessage = "Enter a valid amount")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Payment date required")]
    [DataType(DataType.Date)]
    public DateTime PaymentDate { get; set; }

    [Required(ErrorMessage = "Payment mode required")]
    public string PaymentMode { get; set; } = string.Empty;

    public string ReferenceNo { get; set; } = string.Empty;
}
