using System.ComponentModel.DataAnnotations;

namespace School.Web.Models.Library;

public class ReturnBookFormModel
{
    public int IssueId { get; set; }
    public string BorrowerType { get; set; } = "Student";
    public int PersonId { get; set; }

    [Required(ErrorMessage = "Return date required")]
    [DataType(DataType.Date)]
    public DateTime ReturnDate { get; set; } = DateTime.Today;

    [Range(0, 100000, ErrorMessage = "Enter a valid fine amount")]
    public decimal FineAmount { get; set; }
}
