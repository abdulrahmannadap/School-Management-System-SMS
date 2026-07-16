using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.DTOs.Staff;
using School.Application.DTOs.Student;
using School.Application.Interfaces;
using School.Domain.Enums;
using School.Web.Models.Portal;

namespace School.Web.Areas.SchoolAdmin.Controllers;

[Area("SchoolAdmin")]
[Authorize(Policy = "SchoolAdminAccess")]
public class PortalAccountsController(IPortalAccountService portalSvc, IStudentService studentSvc, IStaffService staffSvc) : Controller
{
    public async Task<IActionResult> Index(StudentSearchDto search, string? staffName, string? staffCode, CancellationToken ct)
    {
        ViewData["Title"] = "Portal Accounts";
        return View(await BuildViewModel(search, staffName, staffCode, ct));
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
    public async Task<IActionResult> CreateStaffLogin(int staffId, UserRole role, CancellationToken ct)
    {
        try
        {
            var created = await portalSvc.CreateStaffLoginAsync(staffId, role, ct);
            TempData["Success"] = created ? "Staff login created." : "Staff member already has a login.";
        }
        catch (ArgumentOutOfRangeException)
        {
            TempData["Error"] = "Invalid login role.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LinkParent(ParentLinkFormModel form, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Portal Accounts";
            var vm = await BuildViewModel(new StudentSearchDto(), null, null, ct);
            vm.LinkForm = form;
            return View("Index", vm);
        }

        var student = (await studentSvc.SearchAsync(new StudentSearchDto { GRNumber = form.GRNumber }, ct))
            .FirstOrDefault();

        if (student is null)
        {
            ModelState.AddModelError(nameof(form.GRNumber), "No student found with this GR Number.");
            ViewData["Title"] = "Portal Accounts";
            var vm = await BuildViewModel(new StudentSearchDto(), null, null, ct);
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

    private async Task<PortalAccountsViewModel> BuildViewModel(
        StudentSearchDto search, string? staffName, string? staffCode, CancellationToken ct)
    {
        var vm = new PortalAccountsViewModel
        {
            Search      = search,
            StaffName   = staffName,
            StaffCode   = staffCode,
            ParentLinks = await portalSvc.GetAllParentLinksAsync(ct)
        };

        if (!string.IsNullOrWhiteSpace(search.Name) || !string.IsNullOrWhiteSpace(search.GRNumber))
        {
            var students = await studentSvc.SearchAsync(search, ct);
            var withLogin = await portalSvc.GetStudentIdsWithLoginAsync(students.Select(s => s.Id), ct);
            vm.StudentResults = students
                .Select(s => new StudentLoginRow { Student = s, HasLogin = withLogin.Contains(s.Id) })
                .ToList();
        }

        if (!string.IsNullOrWhiteSpace(staffName) || !string.IsNullOrWhiteSpace(staffCode))
        {
            var staff = await staffSvc.SearchAsync(new StaffSearchDto { Name = staffName, EmployeeCode = staffCode }, ct);
            var withLogin = await portalSvc.GetStaffIdsWithLoginAsync(staff.Select(s => s.Id), ct);
            vm.StaffResults = staff
                .Select(s => new StaffLoginRow { Staff = s, HasLogin = withLogin.Contains(s.Id) })
                .ToList();
        }

        return vm;
    }
}
