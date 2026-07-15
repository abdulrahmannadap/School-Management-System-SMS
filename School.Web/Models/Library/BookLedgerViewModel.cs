using School.Application.DTOs.Library;

namespace School.Web.Models.Library;

public class BookLedgerViewModel
{
    public IReadOnlyList<LibraryBookDto> Books { get; set; } = [];
    public int? SelectedBookId { get; set; }
    public LibraryBookDto? SelectedBook { get; set; }
    public IReadOnlyList<BookLedgerDto> Ledger { get; set; } = [];
}
