using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.DTOs.Student;
using School.Application.Interfaces;
using School.Web.Models.Portal;

namespace School.Web.Areas.SchoolAdmin.Controllers;

[Area("SchoolAdmin")]
[Authorize(Roles = "SchoolAdmin")]
public class PortalAccountsController(IPortalAccountService portalSvc, IStudentService studentSvc) : Controller
{
    public async Task<IActionResult> Index(StudentSearchDto search, CancellationToken ct)
    {
        ViewData["Title"] = "Portal Accounts";
        return View(await BuildViewModel(search, ct));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateStudentLogin(int studentId, CancellationToken ct)
    {
        var created = await portalSvc.CreateStudentLoginAsync(studentId, ct);
        TempData["Success"] = created ? "Student login created." : "Student already has a login.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LinkParent(ParentLinkFormModel form, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Portal Accounts";
            var vm = await BuildViewModel(new StudentSearchDto(), ct);
            vm.LinkForm = form;
            return View("Index", vm);
        }

        var student = (await studentSvc.SearchAsync(new StudentSearchDto { GRNumber = form.GRNumber }, ct))
            .FirstOrDefault();

        if (student is null)
        {
            ModelState.AddModelError(nameof(form.GRNumber), "No student found with this GR Number.");
            ViewData["Title"] = "Portal Accounts";
            var vm = await BuildViewModel(new StudentSearchDto(), ct);
            vm.LinkForm = form;
            return View("Index", vm);
        }

        try
        {
            await portalSvc.LinkParentAsync(form.Email, form.Password, form.FullName, student.Id, ct);
            TempData["Success"] = $"Parent account linked to {student.FullName}.";
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UnlinkParent(int linkId, CancellationToken ct)
    {
        await portalSvc.UnlinkParentAsync(linkId, ct);
        TempData["Success"] = "Parent link removed.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<PortalAccountsViewModel> BuildViewModel(StudentSearchDto search, CancellationToken ct)
    {
        var vm = new PortalAccountsViewModel
        {
            Search      = search,
            ParentLinks = await portalSvc.GetAllParentLinksAsync(ct)
        };

        if (!string.IsNullOrWhiteSpace(search.Name) || !string.IsNullOrWhiteSpace(search.GRNumber))
        {
            var students = await studentSvc.SearchAsync(search, ct);
            var rows = new List<StudentLoginRow>();
            foreach (var s in students)
                rows.Add(new StudentLoginRow { Student = s, HasLogin = await portalSvc.HasStudentLoginAsync(s.Id, ct) });
            vm.StudentResults = rows;
        }

        return vm;
    }
}
