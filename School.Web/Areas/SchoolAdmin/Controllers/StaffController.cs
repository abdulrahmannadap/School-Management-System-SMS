using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.DTOs.Staff;
using School.Application.Interfaces;
using School.Web.Models.Staff;

namespace School.Web.Areas.SchoolAdmin.Controllers;

[Area("SchoolAdmin")]
[Authorize(Policy = "SchoolAdminAccess")]
public class StaffController(IStaffService staffSvc) : Controller
{
    public async Task<IActionResult> Index([FromQuery] StaffSearchDto search, CancellationToken ct)
    {
        ViewData["Title"] = "Staff";
        return View(new StaffIndexViewModel { Items = await staffSvc.SearchAsync(search, ct), Search = search });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(StaffFormModel form, CancellationToken ct)
    {
        if (form.Id == 0 && form.JoiningDate == default)
            ModelState.AddModelError(nameof(form.JoiningDate), "Joining date required");

        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Staff";
            return View("Index", new StaffIndexViewModel
            {
                Items = await staffSvc.SearchAsync(new StaffSearchDto(), ct), Form = form, ShowModal = true
            });
        }

        if (form.Id == 0)
            await staffSvc.CreateAsync(new CreateStaffDto
            {
                FullName    = form.FullName,
                Mobile      = form.Mobile,
                Designation = form.Designation,
                JoiningDate = form.JoiningDate,
                LoginRole   = form.LoginRole
            }, ct);
        else
            await staffSvc.UpdateAsync(new EditStaffDto
            {
                Id          = form.Id,
                FullName    = form.FullName,
                Mobile      = form.Mobile,
                Email       = form.Email ?? string.Empty,
                Designation = form.Designation,
                IsActive    = form.IsActive
            }, ct);

        TempData["Success"] = "Staff saved.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await staffSvc.DeleteAsync(id, ct);
        TempData["Success"] = "Staff deactivated.";
        return RedirectToAction(nameof(Index));
    }
}
