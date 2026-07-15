using Microsoft.AspNetCore.Mvc;
using School.Application.Interfaces;
using School.Web.Models.ParentPortal;

namespace School.Web.Areas.Parent.Controllers;

public class LibraryController(IPortalAccountService portalSvc, IStudentService studentSvc, ILibraryService librarySvc) : ParentPortalControllerBase(portalSvc)
{
    public async Task<IActionResult> Index(int studentId, CancellationToken ct)
    {
        var deny = await EnsureLinkedAsync(studentId, ct);
        if (deny is not null) return deny;

        var student = await studentSvc.GetAsync(studentId, ct);
        if (student is null) return NotFound();

        ViewData["Title"] = "Library";

        var history = await librarySvc.GetIssueHistoryAsync(studentId, null, ct);

        return View(new ParentChildLibraryViewModel
        {
            Student      = student,
            ActiveIssues = await librarySvc.GetActiveIssuesAsync(studentId, null, ct),
            History      = history.Where(h => h.IsReturned).ToList()
        });
    }
}
