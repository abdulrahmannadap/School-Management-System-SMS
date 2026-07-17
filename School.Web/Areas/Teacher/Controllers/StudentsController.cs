using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.DTOs.Student;
using School.Application.Interfaces;
using School.Web.Models.Students;

namespace School.Web.Areas.Teacher.Controllers;

[Area("Teacher")]
[Authorize(Roles = "Teacher")]
public class StudentsController(IStudentService studentSvc, IMastersService mastersSvc) : Controller
{
    public async Task<IActionResult> Index([FromQuery] StudentSearchDto search, CancellationToken ct)
    {
        ViewData["Title"] = "Admission";
        return View(await BuildViewModel(search, ct));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(StudentFormModel form, CancellationToken ct)
    {
        if (form.Id == 0 && form.FinancialYearId is null or < 1)
            ModelState.AddModelError(nameof(form.FinancialYearId), "Financial year required");

        // Class/Division are optional at first admission but required once editing an
        // already-placed student.
        if (form.Id != 0 && form.ClassId is null or <= 0)
            ModelState.AddModelError(nameof(form.ClassId), "Class required");
        if (form.Id != 0 && form.DivisionId is null or <= 0)
            ModelState.AddModelError(nameof(form.DivisionId), "Division required");

        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Admission";
            var vm = await BuildViewModel(new StudentSearchDto(), ct);
            vm.Form = form;
            vm.ShowModal = true;
            return View("Index", vm);
        }

        if (form.Id == 0)
            await studentSvc.CreateAsync(StudentFormMapper.ToCreateDto(form), ct);
        else
            await studentSvc.UpdateAsync(StudentFormMapper.ToEditDto(form), ct);

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

    /// <summary>Feeds the Edit modal every field the Create form also collects.</summary>
    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken ct)
    {
        var details = await studentSvc.GetFullDetailsAsync(id, ct);
        return details is null ? NotFound() : Json(details);
    }

    private async Task<StudentsIndexViewModel> BuildViewModel(StudentSearchDto search, CancellationToken ct)
    {
        return new StudentsIndexViewModel
        {
            Items          = await studentSvc.SearchAsync(search, ct),
            Classes        = await mastersSvc.GetClassesAsync(ct),
            Divisions      = await mastersSvc.GetDivisionsAsync(null, ct),
            FinancialYears = await mastersSvc.GetFinancialYearsAsync(ct),
            AcademicYears  = await mastersSvc.GetAcademicYearsAsync(ct),
            Search         = search
        };
    }
}
