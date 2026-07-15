using School.Application.DTOs.Library;
using School.Application.Interfaces;
using School.Domain.Entities.Library;

namespace School.Application.Services.Library;

public class LibraryService(
    IGenericRepository<BookCategory> categoryRepo,
    IGenericRepository<LibraryBook>  bookRepo,
    IGenericRepository<BookIssue>    issueRepo,
    IGenericRepository<BookReturn>   returnRepo,
    IGenericRepository<BookLedger>   ledgerRepo) : ILibraryService
{
    // ── Book Category ────────────────────────────────────────

    public async Task<BookCategoryDto> CreateCategoryAsync(BookCategoryDto dto, CancellationToken ct = default)
    {
        var entity = new BookCategory { CategoryName = dto.CategoryName };
        await categoryRepo.AddAsync(entity, ct);
        await categoryRepo.SaveChangesAsync(ct);
        return new BookCategoryDto { Id = entity.Id, CategoryName = entity.CategoryName };
    }

    public async Task<BookCategoryDto> UpdateCategoryAsync(BookCategoryDto dto, CancellationToken ct = default)
    {
        var entity = await categoryRepo.FirstOrDefaultAsync(c => c.Id == dto.Id, ct)
            ?? throw new KeyNotFoundException($"BookCategory {dto.Id} not found.");
        entity.CategoryName = dto.CategoryName;
        categoryRepo.Update(entity);
        await categoryRepo.SaveChangesAsync(ct);
        return new BookCategoryDto { Id = entity.Id, CategoryName = entity.CategoryName };
    }

    public async Task DeleteCategoryAsync(int id, CancellationToken ct = default)
    {
        var entity = await categoryRepo.FirstOrDefaultAsync(c => c.Id == id, ct)
            ?? throw new KeyNotFoundException($"BookCategory {id} not found.");
        categoryRepo.Delete(entity);
        await categoryRepo.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<BookCategoryDto>> GetCategoriesAsync(CancellationToken ct = default)
    {
        var list = await categoryRepo.GetAllAsync(ct);
        return list.OrderBy(c => c.CategoryName)
                   .Select(c => new BookCategoryDto { Id = c.Id, CategoryName = c.CategoryName })
                   .ToList();
    }

    // ── Books ────────────────────────────────────────────────

    public async Task<LibraryBookDto> AddBookAsync(LibraryBookDto dto, CancellationToken ct = default)
    {
        var entity = new LibraryBook
        {
            Title             = dto.Title,
            Author            = dto.Author,
            ISBN              = dto.ISBN,
            CategoryId        = dto.CategoryId,
            TotalQuantity     = dto.TotalQuantity,
            AvailableQuantity = dto.TotalQuantity   // all copies available on add
        };
        await bookRepo.AddAsync(entity, ct);
        await bookRepo.SaveChangesAsync(ct);
        return MapBook(entity);
    }

    public async Task<LibraryBookDto> UpdateBookAsync(LibraryBookDto dto, CancellationToken ct = default)
    {
        var entity = await bookRepo.FirstOrDefaultAsync(b => b.Id == dto.Id, ct)
            ?? throw new KeyNotFoundException($"Book {dto.Id} not found.");

        var quantityDiff        = dto.TotalQuantity - entity.TotalQuantity;
        entity.Title             = dto.Title;
        entity.Author            = dto.Author;
        entity.ISBN              = dto.ISBN;
        entity.CategoryId        = dto.CategoryId;
        entity.TotalQuantity     = dto.TotalQuantity;
        entity.AvailableQuantity = Math.Max(0, entity.AvailableQuantity + quantityDiff);

        bookRepo.Update(entity);
        await bookRepo.SaveChangesAsync(ct);
        return MapBook(entity);
    }

    public async Task DeleteBookAsync(int id, CancellationToken ct = default)
    {
        var entity = await bookRepo.FirstOrDefaultAsync(b => b.Id == id, ct)
            ?? throw new KeyNotFoundException($"Book {id} not found.");
        bookRepo.Delete(entity);
        await bookRepo.SaveChangesAsync(ct);
    }

    public async Task<LibraryBookDto?> GetBookAsync(int id, CancellationToken ct = default)
    {
        var entity = await bookRepo.FirstOrDefaultAsync(b => b.Id == id, ct);
        return entity is null ? null : MapBook(entity);
    }

    public async Task<IReadOnlyList<LibraryBookDto>> SearchBooksAsync(BookSearchDto search, CancellationToken ct = default)
    {
        var query = bookRepo.QueryNoTracking();

        if (!string.IsNullOrWhiteSpace(search.Title))
            query = query.Where(b => b.Title.Contains(search.Title));

        if (!string.IsNullOrWhiteSpace(search.Author))
            query = query.Where(b => b.Author.Contains(search.Author));

        if (search.CategoryId.HasValue)
            query = query.Where(b => b.CategoryId == search.CategoryId.Value);

        return query.OrderBy(b => b.Title).Select(b => MapBook(b)).ToList();
    }

    // ── Issue & Return ───────────────────────────────────────

    public async Task<int> IssueBookAsync(IssueBookDto dto, CancellationToken ct = default)
    {
        var book = await bookRepo.FirstOrDefaultAsync(b => b.Id == dto.BookId, ct)
            ?? throw new KeyNotFoundException($"Book {dto.BookId} not found.");

        if (book.AvailableQuantity <= 0)
            throw new InvalidOperationException($"Book '{book.Title}' has no available copies.");

        var issue = new BookIssue
        {
            BookId     = dto.BookId,
            StudentId  = dto.StudentId,
            StaffId    = dto.StaffId,
            IssueDate  = dto.IssueDate,
            DueDate    = dto.DueDate,
            IsReturned = false
        };
        await issueRepo.AddAsync(issue, ct);

        // ledger entry
        await ledgerRepo.AddAsync(new BookLedger
        {
            BookId    = dto.BookId,
            StudentId = dto.StudentId,
            StaffId   = dto.StaffId,
            Date      = dto.IssueDate,
            Type      = "Issued",
            DueDate   = dto.DueDate
        }, ct);

        // decrement available
        book.AvailableQuantity--;
        bookRepo.Update(book);

        await issueRepo.SaveChangesAsync(ct);
        return issue.Id;
    }

    public async Task ReturnBookAsync(ReturnBookDto dto, CancellationToken ct = default)
    {
        var issue = await issueRepo.FirstOrDefaultAsync(i => i.Id == dto.IssueId, ct)
            ?? throw new KeyNotFoundException($"Issue {dto.IssueId} not found.");

        if (issue.IsReturned)
            throw new InvalidOperationException("This book has already been returned.");

        // create return record
        await returnRepo.AddAsync(new BookReturn
        {
            IssueId    = dto.IssueId,
            ReturnDate = dto.ReturnDate,
            FineAmount = dto.FineAmount
        }, ct);

        // mark issue as returned
        issue.IsReturned = true;
        issueRepo.Update(issue);

        // ledger entry
        await ledgerRepo.AddAsync(new BookLedger
        {
            BookId    = issue.BookId,
            StudentId = issue.StudentId,
            StaffId   = issue.StaffId,
            Date      = dto.ReturnDate,
            Type      = "Returned",
            DueDate   = issue.DueDate
        }, ct);

        // increment available
        var book = await bookRepo.FirstOrDefaultAsync(b => b.Id == issue.BookId, ct);
        if (book is not null)
        {
            book.AvailableQuantity = Math.Min(book.AvailableQuantity + 1, book.TotalQuantity);
            bookRepo.Update(book);
        }

        await returnRepo.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<BookIssueReportDto>> GetActiveIssuesAsync(
        int? studentId, int? staffId, CancellationToken ct = default)
    {
        var list = await issueRepo.FindAsync(i =>
            !i.IsReturned &&
            (!studentId.HasValue || i.StudentId == studentId) &&
            (!staffId.HasValue   || i.StaffId   == staffId), ct);

        var titles = await GetBookTitlesAsync(list.Select(i => i.BookId), ct);
        return list.OrderBy(i => i.DueDate).Select(i => MapIssueReport(i, titles)).ToList();
    }

    public async Task<IReadOnlyList<BookIssueReportDto>> GetIssueHistoryAsync(
        int? studentId, int? staffId, CancellationToken ct = default)
    {
        var list = await issueRepo.FindAsync(i =>
            (!studentId.HasValue || i.StudentId == studentId) &&
            (!staffId.HasValue   || i.StaffId   == staffId), ct);

        var titles = await GetBookTitlesAsync(list.Select(i => i.BookId), ct);
        return list.OrderByDescending(i => i.IssueDate).Select(i => MapIssueReport(i, titles)).ToList();
    }

    // ── Ledger ───────────────────────────────────────────────

    public async Task<IReadOnlyList<BookLedgerDto>> GetLedgerAsync(int bookId, CancellationToken ct = default)
    {
        var list = await ledgerRepo.FindAsync(l => l.BookId == bookId, ct);
        return list.OrderByDescending(l => l.Date)
                   .Select(l => new BookLedgerDto
                   {
                       Id        = l.Id,
                       BookId    = l.BookId,
                       StudentId = l.StudentId,
                       StaffId   = l.StaffId,
                       Date      = l.Date,
                       Type      = l.Type,
                       DueDate   = l.DueDate
                   }).ToList();
    }

    // ── Reports ──────────────────────────────────────────────

    public async Task<IReadOnlyList<BookPendingDto>> GetPendingReturnsAsync(CancellationToken ct = default)
    {
        var today   = DateTime.UtcNow.Date;
        var overdue = await issueRepo.FindAsync(i => !i.IsReturned && i.DueDate.Date < today, ct);

        return overdue.OrderBy(i => i.DueDate)
                      .Select(i => new BookPendingDto
                      {
                          BookId    = i.BookId,
                          StudentId = i.StudentId,
                          StaffId   = i.StaffId,
                          IssueDate = i.IssueDate,
                          DueDate   = i.DueDate,
                          DaysLate  = (today - i.DueDate.Date).Days
                      }).ToList();
    }

    public async Task<IReadOnlyList<BookIssueReportDto>> GetIssuedReportAsync(
        DateTime from, DateTime to, CancellationToken ct = default)
    {
        var list = await issueRepo.FindAsync(
            i => i.IssueDate.Date >= from.Date && i.IssueDate.Date <= to.Date, ct);

        var titles = await GetBookTitlesAsync(list.Select(i => i.BookId), ct);
        return list.OrderBy(i => i.IssueDate).Select(i => MapIssueReport(i, titles)).ToList();
    }

    // ── Private mappers ──────────────────────────────────────

    private static LibraryBookDto MapBook(LibraryBook b) => new()
    {
        Id                = b.Id,
        Title             = b.Title,
        Author            = b.Author,
        ISBN              = b.ISBN,
        CategoryId        = b.CategoryId,
        TotalQuantity     = b.TotalQuantity,
        AvailableQuantity = b.AvailableQuantity
    };

    private async Task<Dictionary<int, string>> GetBookTitlesAsync(IEnumerable<int> bookIds, CancellationToken ct)
    {
        var ids = bookIds.Distinct().ToList();
        var books = await bookRepo.FindAsync(b => ids.Contains(b.Id), ct);
        return books.ToDictionary(b => b.Id, b => b.Title);
    }

    private static BookIssueReportDto MapIssueReport(BookIssue i, IReadOnlyDictionary<int, string> titles) => new()
    {
        Id         = i.Id,
        BookId     = i.BookId,
        BookTitle  = titles.GetValueOrDefault(i.BookId, "—"),
        StudentId  = i.StudentId,
        StaffId    = i.StaffId,
        IssueDate  = i.IssueDate,
        DueDate    = i.DueDate,
        IsReturned = i.IsReturned
    };
}
