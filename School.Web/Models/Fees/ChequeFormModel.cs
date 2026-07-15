using System.ComponentModel.DataAnnotations;

namespace School.Web.Models.Fees;

public class ChequeFormModel
{
    public int StudentId { get; set; }

    [Required(ErrorMessage = "Cheque number required")]
    [StringLength(30)]
    public string ChequeNo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Cheque date required")]
    [DataType(DataType.Date)]
    public DateTime ChequeDate { get; set; } = DateTime.Today;

    [Range(0.01, 100000000, ErrorMessage = "Enter a valid amount")]
    public decimal Amount { get; set; }
}
