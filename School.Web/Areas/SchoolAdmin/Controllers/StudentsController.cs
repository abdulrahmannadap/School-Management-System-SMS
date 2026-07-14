using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.DTOs.Student;
using School.Application.Interfaces;
using School.Web.Models.Students;

namespace School.Web.Areas.SchoolAdmin.Controllers;

[Area("SchoolAdmin")]
[Authorize(Roles = "SchoolAdmin")]
public class StudentsController(IStudentService studentSvc, IMastersService mastersSvc) : Controller
{
    public async Task<IActionResult> Index([FromQuery] StudentSearchDto search, CancellationToken ct)
    {
        ViewData["Title"] = "Students";
        return View(await BuildViewModel(search, ct));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(StudentFormModel form, CancellationToken ct)
    {
        if (form.Id == 0 && form.FinancialYearId < 1)
            ModelState.AddModelError(nameof(form.FinancialYearId), "Financial year required");

        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Students";
            var vm = await BuildViewModel(new StudentSearchDto(), ct);
            vm.Form = form;
            vm.ShowModal = true;
            return View("Index", vm);
        }

        if (form.Id == 0)
            await studentSvc.CreateAsync(new CreateStudentDto
            {
                FinancialYearId = form.FinancialYearId,
                ClassId         = form.ClassId,
                DivisionId      = form.DivisionId,
                FullName        = form.FullName,
                Gender          = form.Gender,
                DateOfBirth     = form.DateOfBirth,
                FatherName      = form.FatherName,
                FatherMobile    = form.FatherMobile
            }, ct);
        else
            await studentSvc.UpdateAsync(new EditStudentDto
            {
                Id          = form.Id,
                ClassId     = form.ClassId,
                DivisionId  = form.DivisionId,
                FullName    = form.FullName,
                Gender      = form.Gender,
                DateOfBirth = form.DateOfBirth,
                Email       = form.Email ?? string.Empty,
                IsActive    = form.IsActive
            }, ct);

        TempData["Success"] = "Student saved.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await studentSvc.DeleteAsync(id, ct);
        TempData["Success"] = "Student deactivated.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<StudentsIndexViewModel> BuildViewModel(StudentSearchDto search, CancellationToken ct)
    {
        return new StudentsIndexViewModel
        {
            Items          = await studentSvc.SearchAsync(search, ct),
            Classes        = await mastersSvc.GetClassesAsync(ct),
            Divisions      = await mastersSvc.GetDivisionsAsync(null, ct),
            FinancialYears = await mastersSvc.GetFinancialYearsAsync(ct),
            Search         = search
        };
    }
}
