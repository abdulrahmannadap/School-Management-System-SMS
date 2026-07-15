using Microsoft.AspNetCore.Mvc;
using School.Application.Interfaces;
using School.Web.Models.StudentPortal;

namespace School.Web.Areas.Student.Controllers;

public class LibraryController(ILibraryService librarySvc) : StudentPortalControllerBase
{
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        ViewData["Title"] = "Library";

        var history = await librarySvc.GetIssueHistoryAsync(CurrentStudentId, null, ct);

        return View(new StudentLibraryViewModel
        {
            ActiveIssues = await librarySvc.GetActiveIssuesAsync(CurrentStudentId, null, ct),
            History      = history.Where(h => h.IsReturned).ToList()
        });
    }
}
