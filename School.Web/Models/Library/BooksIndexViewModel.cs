using School.Application.DTOs.Library;

namespace School.Web.Models.Library;

public class BooksIndexViewModel
{
    public IReadOnlyList<LibraryBookDto> Items { get; set; } = [];
    public IReadOnlyList<BookCategoryDto> Categories { get; set; } = [];
    public BookSearchDto Search { get; set; } = new();
    public BookFormModel Form { get; set; } = new();
    public BookCategoryFormModel CategoryForm { get; set; } = new();
    public bool ShowModal { get; set; }
    public bool ShowCategoryModal { get; set; }
}
