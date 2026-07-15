using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.DTOs.Library;
using School.Application.DTOs.Staff;
using School.Application.DTOs.Student;
using School.Application.Interfaces;
using School.Web.Models.Library;

namespace School.Web.Areas.SchoolAdmin.Controllers;

[Area("SchoolAdmin")]
[Authorize(Roles = "SchoolAdmin")]
public class LibraryController(
    ILibraryService librarySvc,
    IBookAggregatorService aggregatorSvc,
    IStudentService studentSvc,
    IStaffService staffSvc) : Controller
{
    public async Task<IActionResult> Books([FromQuery] BookSearchDto search, CancellationToken ct)
    {
        ViewData["Title"] = "Books";
        return View(await BuildViewModel(search, ct));
    }

    public async Task<IActionResult> SearchOnline(string? query, CancellationToken ct)
    {
        ViewData["Title"] = "Search Online";

        var vm = new SearchOnlineViewModel { Query = query };

        if (!string.IsNullOrWhiteSpace(query))
        {
            var result = await aggregatorSvc.SearchAsync(query, ct);
            vm.Results = result.Results;
            vm.Warnings = result.Warnings;
        }

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveBook(BookFormModel form, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Books";
            var vm = await BuildViewModel(new BookSearchDto(), ct);
            vm.Form = form;
            vm.ShowModal = true;
            return View("Books", vm);
        }

        var dto = new LibraryBookDto
        {
            Id            = form.Id,
            Title         = form.Title,
            Author        = form.Author,
            ISBN          = form.ISBN,
            CategoryId    = form.CategoryId,
            TotalQuantity = form.TotalQuantity
        };

        if (form.Id == 0)
            await librarySvc.AddBookAsync(dto, ct);
        else
            await librarySvc.UpdateBookAsync(dto, ct);

        TempData["Success"] = "Book saved.";
        return RedirectToAction(nameof(Books));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteBook(int id, CancellationToken ct)
    {
        await librarySvc.DeleteBookAsync(id, ct);
        TempData["Success"] = "Book deleted.";
        return RedirectToAction(nameof(Books));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveCategory(BookCategoryFormModel categoryForm, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Books";
            var vm = await BuildViewModel(new BookSearchDto(), ct);
            vm.CategoryForm = categoryForm;
            vm.ShowCategoryModal = true;
            return View("Books", vm);
        }

        var dto = new BookCategoryDto { Id = categoryForm.Id, CategoryName = categoryForm.CategoryName };

        if (categoryForm.Id == 0)
            await librarySvc.CreateCategoryAsync(dto, ct);
        else
            await librarySvc.UpdateCategoryAsync(dto, ct);

        TempData["Success"] = "Category saved.";
        return RedirectToAction(nameof(Books));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCategory(int id, CancellationToken ct)
    {
        await librarySvc.DeleteCategoryAsync(id, ct);
        TempData["Success"] = "Category deleted.";
        return RedirectToAction(nameof(Books));
    }

    public async Task<IActionResult> IssueReturn(string? borrowerType, string? name, string? code, CancellationToken ct)
    {
        ViewData["Title"] = "Issue / Return";

        var vm = new BorrowerSearchViewModel
        {
            BorrowerType = string.IsNullOrWhiteSpace(borrowerType) ? "Student" : borrowerType,
            Name         = name,
            Code         = code
        };

        if (!string.IsNullOrWhiteSpace(name) || !string.IsNullOrWhiteSpace(code))
        {
            if (vm.BorrowerType == "Staff")
            {
                var staff = await staffSvc.SearchAsync(new StaffSearchDto { Name = name, EmployeeCode = code }, ct);
                vm.Results = staff.Select(s => new BorrowerResult { Id = s.Id, Name = s.FullName, Code = s.EmployeeCode }).ToList();
            }
            else
            {
                var students = await studentSvc.SearchAsync(new StudentSearchDto { Name = name, GRNumber = code }, ct);
                vm.Results = students.Select(s => new BorrowerResult { Id = s.Id, Name = s.FullName, Code = s.GRNumber }).ToList();
            }
        }

        return View(vm);
    }

    public async Task<IActionResult> BorrowerDetail(int personId, string borrowerType, CancellationToken ct)
    {
        string personName;
        string personCode;

        if (borrowerType == "Staff")
        {
            var staff = await staffSvc.GetAsync(personId, ct);
            if (staff is null) return NotFound();
            personName = staff.FullName;
            personCode = staff.EmployeeCode;
        }
        else
        {
            var student = await studentSvc.GetAsync(personId, ct);
            if (student is null) return NotFound();
            personName = student.FullName;
            personCode = student.GRNumber;
        }

        ViewData["Title"] = "Issue / Return";
        return View(await BuildBorrowerDetailViewModel(personId, borrowerType, personName, personCode, ct));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> IssueBook(IssueBookFormModel form, CancellationToken ct)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await librarySvc.IssueBookAsync(new IssueBookDto
                {
                    BookId    = form.BookId,
                    StudentId = form.BorrowerType == "Student" ? form.PersonId : null,
                    StaffId   = form.BorrowerType == "Staff" ? form.PersonId : null,
                    IssueDate = form.IssueDate,
                    DueDate   = form.DueDate
                }, ct);
                TempData["Success"] = "Book issued.";
            }
            catch (Exception ex) when (ex is KeyNotFoundException or InvalidOperationException)
            {
                TempData["Error"] = ex.Message;
            }
        }

        return RedirectToAction(nameof(BorrowerDetail), new { personId = form.PersonId, borrowerType = form.BorrowerType });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ReturnBook(ReturnBookFormModel form, CancellationToken ct)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await librarySvc.ReturnBookAsync(new ReturnBookDto
                {
                    IssueId    = form.IssueId,
                    ReturnDate = form.ReturnDate,
                    FineAmount = form.FineAmount
                }, ct);
                TempData["Success"] = "Book returned.";
            }
            catch (Exception ex) when (ex is KeyNotFoundException or InvalidOperationException)
            {
                TempData["Error"] = ex.Message;
            }
        }

        return RedirectToAction(nameof(BorrowerDetail), new { personId = form.PersonId, borrowerType = form.BorrowerType });
    }

    public async Task<IActionResult> BookLedger(int? bookId, CancellationToken ct)
    {
        ViewData["Title"] = "Book Ledger";

        var vm = new BookLedgerViewModel
        {
            Books          = await librarySvc.SearchBooksAsync(new BookSearchDto(), ct),
            SelectedBookId = bookId
        };

        if (bookId.HasValue)
        {
            vm.SelectedBook = await librarySvc.GetBookAsync(bookId.Value, ct);
            vm.Ledger       = await librarySvc.GetLedgerAsync(bookId.Value, ct);
        }

        return View(vm);
    }

    private async Task<BorrowerDetailViewModel> BuildBorrowerDetailViewModel(
        int personId, string borrowerType, string personName, string personCode, CancellationToken ct)
    {
        var books = await librarySvc.SearchBooksAsync(new BookSearchDto(), ct);

        return new BorrowerDetailViewModel
        {
            BorrowerType   = borrowerType,
            PersonId       = personId,
            PersonName     = personName,
            PersonCode     = personCode,
            AvailableBooks = books.Where(b => b.AvailableQuantity > 0).ToList(),
            ActiveIssues   = borrowerType == "Staff"
                ? await librarySvc.GetActiveIssuesAsync(null, personId, ct)
                : await librarySvc.GetActiveIssuesAsync(personId, null, ct),
            IssueForm  = new IssueBookFormModel { PersonId = personId, BorrowerType = borrowerType },
            ReturnForm = new ReturnBookFormModel { PersonId = personId, BorrowerType = borrowerType }
        };
    }

    private async Task<BooksIndexViewModel> BuildViewModel(BookSearchDto search, CancellationToken ct)
    {
        return new BooksIndexViewModel
        {
            Items      = await librarySvc.SearchBooksAsync(search, ct),
            Categories = await librarySvc.GetCategoriesAsync(ct),
            Search     = search
        };
    }
}
