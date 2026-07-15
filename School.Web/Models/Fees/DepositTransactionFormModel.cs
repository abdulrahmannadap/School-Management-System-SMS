using System.ComponentModel.DataAnnotations;

namespace School.Web.Models.Fees;

public class DepositTransactionFormModel
{
    public int StudentId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Deposit type required")]
    public int DepositMasterId { get; set; }

    [Range(0.01, 100000000, ErrorMessage = "Enter a valid amount")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Date required")]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Type required")]
    public string Type { get; set; } = "Paid";
}
