using School.Application.DTOs.Library;

namespace School.Application.Interfaces;

public interface ILibraryService
{
    // ── Book Category ────────────────────────────────────────
    Task<BookCategoryDto>               CreateCategoryAsync(BookCategoryDto dto, CancellationToken ct = default);
    Task<BookCategoryDto>               UpdateCategoryAsync(BookCategoryDto dto, CancellationToken ct = default);
    Task                                DeleteCategoryAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<BookCategoryDto>> GetCategoriesAsync(CancellationToken ct = default);

    // ── Books ────────────────────────────────────────────────
    Task<LibraryBookDto>               AddBookAsync(LibraryBookDto dto, CancellationToken ct = default);
    Task<LibraryBookDto>               UpdateBookAsync(LibraryBookDto dto, CancellationToken ct = default);
    Task                               DeleteBookAsync(int id, CancellationToken ct = default);
    Task<LibraryBookDto?>              GetBookAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<LibraryBookDto>> SearchBooksAsync(BookSearchDto search, CancellationToken ct = default);

    // ── Issue & Return ───────────────────────────────────────
    Task<int>                            IssueBookAsync(IssueBookDto dto, CancellationToken ct = default);
    Task                                 ReturnBookAsync(ReturnBookDto dto, CancellationToken ct = default);
    Task<IReadOnlyList<BookIssueReportDto>> GetActiveIssuesAsync(int? studentId, int? staffId, CancellationToken ct = default);
    Task<IReadOnlyList<BookIssueReportDto>> GetIssueHistoryAsync(int? studentId, int? staffId, CancellationToken ct = default);

    // ── Ledger ───────────────────────────────────────────────
    Task<IReadOnlyList<BookLedgerDto>> GetLedgerAsync(int bookId, CancellationToken ct = default);

    // ── Reports ──────────────────────────────────────────────
    Task<IReadOnlyList<BookPendingDto>>     GetPendingReturnsAsync(CancellationToken ct = default);
    Task<IReadOnlyList<BookIssueReportDto>> GetIssuedReportAsync(DateTime from, DateTime to, CancellationToken ct = default);
}
