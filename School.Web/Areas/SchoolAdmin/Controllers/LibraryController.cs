using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.DTOs.Library;
using School.Application.Interfaces;
using School.Web.Models.Library;

namespace School.Web.Areas.SchoolAdmin.Controllers;

[Area("SchoolAdmin")]
[Authorize(Roles = "SchoolAdmin")]
public class LibraryController(ILibraryService librarySvc) : Controller
{
    public async Task<IActionResult> Books([FromQuery] BookSearchDto search, CancellationToken ct)
    {
        ViewData["Title"] = "Books";
        return View(await BuildViewModel(search, ct));
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
