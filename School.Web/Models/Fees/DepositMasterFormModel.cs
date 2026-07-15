using System.ComponentModel.DataAnnotations;

namespace School.Web.Models.Fees;

public class DepositMasterFormModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Deposit name required")]
    [StringLength(100)]
    public string DepositName { get; set; } = string.Empty;

    [Range(0.01, 100000000, ErrorMessage = "Enter a valid amount")]
    public decimal Amount { get; set; }
}
