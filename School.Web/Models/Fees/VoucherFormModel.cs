using System.ComponentModel.DataAnnotations;

namespace School.Web.Models.Fees;

public class VoucherFormModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Date required")]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Type required")]
    public string Type { get; set; } = "Receipt";

    [Range(0.01, 100000000, ErrorMessage = "Enter a valid amount")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Description required")]
    [StringLength(200)]
    public string Description { get; set; } = string.Empty;
}
