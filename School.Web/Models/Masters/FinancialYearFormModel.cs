using System.ComponentModel.DataAnnotations;

namespace School.Web.Models.Masters;

public class FinancialYearFormModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name required")]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Start date required")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "End date required")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }

    public bool IsActive { get; set; }
}
