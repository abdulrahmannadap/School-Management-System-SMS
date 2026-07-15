using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.DTOs;
using School.Application.Interfaces;
using School.Web.Models.SuperAdmin;

namespace School.Web.Areas.SuperAdmin.Controllers;

[Area("SuperAdmin")]
[Authorize(Roles = "SuperAdmin")]
public class SchoolsController(ISchoolService schoolSvc) : Controller
{
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        ViewData["Title"] = "Schools";
        return View(new SchoolsIndexViewModel { Items = await schoolSvc.GetAllAsync(ct) });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(SchoolFormModel form, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Schools";
            var vm = new SchoolsIndexViewModel { Items = await schoolSvc.GetAllAsync(ct), Form = form, ShowModal = true };
            return View("Index", vm);
        }

        var dto = new SchoolDto
        {
            Id           = form.Id,
            Name         = form.Name,
            Address      = form.Address,
            ContactEmail = form.ContactEmail,
            ContactPhone = form.ContactPhone,
            IsActive     = form.IsActive
        };

        if (form.Id == Guid.Empty)
            await schoolSvc.CreateAsync(dto, ct);
        else
            await schoolSvc.UpdateAsync(dto, ct);

        TempData["Success"] = "School saved.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateSchoolAdmin(SchoolAdminFormModel form, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Schools";
            var vm = new SchoolsIndexViewModel { Items = await schoolSvc.GetAllAsync(ct), AdminForm = form, ShowAdminModal = true };
            return View("Index", vm);
        }

        try
        {
            await schoolSvc.CreateSchoolAdminAsync(form.SchoolId, form.Email, form.Password, form.FullName, ct);
            TempData["Success"] = $"School admin login created for {form.Email}.";
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}
