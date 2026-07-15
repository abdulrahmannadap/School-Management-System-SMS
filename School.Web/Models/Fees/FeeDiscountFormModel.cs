using System.ComponentModel.DataAnnotations;

namespace School.Web.Models.Fees;

public class FeeDiscountFormModel
{
    public int StudentId { get; set; }

    [Range(0.01, 100000000, ErrorMessage = "Enter a valid amount")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Reason required")]
    [StringLength(200)]
    public string Reason { get; set; } = string.Empty;
}
