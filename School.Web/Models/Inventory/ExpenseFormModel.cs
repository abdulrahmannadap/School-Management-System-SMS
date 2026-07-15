using System.ComponentModel.DataAnnotations;

namespace School.Web.Models.Inventory;

public class ExpenseFormModel
{
    [Required(ErrorMessage = "Expense name required")]
    [StringLength(100)]
    public string ExpenseName { get; set; } = string.Empty;

    [Range(0.01, 1000000000, ErrorMessage = "Enter a valid amount")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Date required")]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Category required")]
    [StringLength(50)]
    public string Category { get; set; } = string.Empty;
}
