using School.Application.DTOs.Library;

namespace School.Web.Models.Library;

public class BorrowerDetailViewModel
{
    public string BorrowerType { get; set; } = "Student";
    public int PersonId { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public string PersonCode { get; set; } = string.Empty;

    public IReadOnlyList<LibraryBookDto> AvailableBooks { get; set; } = [];
    public IReadOnlyList<BookIssueReportDto> ActiveIssues { get; set; } = [];

    public IssueBookFormModel IssueForm { get; set; } = new();
    public ReturnBookFormModel ReturnForm { get; set; } = new();
}
