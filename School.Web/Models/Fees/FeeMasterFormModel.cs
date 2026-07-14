using System.ComponentModel.DataAnnotations;

namespace School.Web.Models.Fees;

public class FeeMasterFormModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Fee name required")]
    [StringLength(100)]
    public string FeeName { get; set; } = string.Empty;

    [Range(0.01, 100000000, ErrorMessage = "Enter a valid amount")]
    public decimal Amount { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Class required")]
    public int ClassId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Financial year required")]
    public int FinancialYearId { get; set; }

    public bool IsRecurring { get; set; }
}
