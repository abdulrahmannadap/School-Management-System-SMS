using System.ComponentModel.DataAnnotations;

namespace School.Web.Models.Library;

public class IssueBookFormModel
{
    public string BorrowerType { get; set; } = "Student";
    public int PersonId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Book required")]
    public int BookId { get; set; }

    [Required(ErrorMessage = "Issue date required")]
    [DataType(DataType.Date)]
    public DateTime IssueDate { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Due date required")]
    [DataType(DataType.Date)]
    public DateTime DueDate { get; set; } = DateTime.Today.AddDays(14);
}
