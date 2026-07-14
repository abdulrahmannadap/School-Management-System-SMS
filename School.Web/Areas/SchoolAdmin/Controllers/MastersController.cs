using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.DTOs.Masters;
using School.Application.Interfaces;
using School.Web.Models.Masters;

namespace School.Web.Areas.SchoolAdmin.Controllers;

[Area("SchoolAdmin")]
[Authorize(Roles = "SchoolAdmin")]
public class MastersController(IMastersService svc) : Controller
{
    // ── Academic Year ────────────────────────────────────────

    public async Task<IActionResult> AcademicYears(CancellationToken ct)
    {
        ViewData["Title"] = "Academic Years";
        return View(new AcademicYearsViewModel { Items = await svc.GetAcademicYearsAsync(ct) });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveAcademicYear(AcademicYearFormModel form, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Academic Years";
            return View("AcademicYears", new AcademicYearsViewModel
            {
                Items = await svc.GetAcademicYearsAsync(ct), Form = form, ShowModal = true
            });
        }

        if (form.Id == 0)
            await svc.CreateAcademicYearAsync(new CreateAcademicYearDto
            {
                Name = form.Name, StartDate = form.StartDate, EndDate = form.EndDate, IsActive = form.IsActive
            }, ct);
        else
            await svc.UpdateAcademicYearAsync(new UpdateAcademicYearDto
            {
                Id = form.Id, Name = form.Name, StartDate = form.StartDate, EndDate = form.EndDate, IsActive = form.IsActive
            }, ct);

        TempData["Success"] = "Academic year saved.";
        return RedirectToAction(nameof(AcademicYears));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAcademicYear(int id, CancellationToken ct)
    {
        await svc.DeleteAcademicYearAsync(id, ct);
        TempData["Success"] = "Academic year deleted.";
        return RedirectToAction(nameof(AcademicYears));
    }

    // ── Financial Year ───────────────────────────────────────

    public async Task<IActionResult> FinancialYears(CancellationToken ct)
    {
        ViewData["Title"] = "Financial Years";
        return View(new FinancialYearsViewModel { Items = await svc.GetFinancialYearsAsync(ct) });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveFinancialYear(FinancialYearFormModel form, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Financial Years";
            return View("FinancialYears", new FinancialYearsViewModel
            {
                Items = await svc.GetFinancialYearsAsync(ct), Form = form, ShowModal = true
            });
        }

        if (form.Id == 0)
            await svc.CreateFinancialYearAsync(new CreateFinancialYearDto
            {
                Name = form.Name, StartDate = form.StartDate, EndDate = form.EndDate, IsActive = form.IsActive
            }, ct);
        else
            await svc.UpdateFinancialYearAsync(new UpdateFinancialYearDto
            {
                Id = form.Id, Name = form.Name, StartDate = form.StartDate, EndDate = form.EndDate, IsActive = form.IsActive
            }, ct);

        TempData["Success"] = "Financial year saved.";
        return RedirectToAction(nameof(FinancialYears));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteFinancialYear(int id, CancellationToken ct)
    {
        await svc.DeleteFinancialYearAsync(id, ct);
        TempData["Success"] = "Financial year deleted.";
        return RedirectToAction(nameof(FinancialYears));
    }

    // ── Class ────────────────────────────────────────────────

    public async Task<IActionResult> Classes(CancellationToken ct)
    {
        ViewData["Title"] = "Classes";
        return View(new ClassesViewModel { Items = await svc.GetClassesAsync(ct) });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveClass(ClassFormModel form, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Classes";
            return View("Classes", new ClassesViewModel
            {
                Items = await svc.GetClassesAsync(ct), Form = form, ShowModal = true
            });
        }

        if (form.Id == 0)
            await svc.CreateClassAsync(new CreateClassDto { Name = form.Name, OrderNo = form.OrderNo, IsActive = form.IsActive }, ct);
        else
            await svc.UpdateClassAsync(new UpdateClassDto { Id = form.Id, Name = form.Name, OrderNo = form.OrderNo, IsActive = form.IsActive }, ct);

        TempData["Success"] = "Class saved.";
        return RedirectToAction(nameof(Classes));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteClass(int id, CancellationToken ct)
    {
        await svc.DeleteClassAsync(id, ct);
        TempData["Success"] = "Class deleted.";
        return RedirectToAction(nameof(Classes));
    }

    // ── Division ─────────────────────────────────────────────

    public async Task<IActionResult> Divisions(CancellationToken ct)
    {
        ViewData["Title"] = "Divisions";
        return View(new DivisionsViewModel
        {
            Items = await svc.GetDivisionsAsync(null, ct),
            Classes = await svc.GetClassesAsync(ct)
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveDivision(DivisionFormModel form, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Divisions";
            return View("Divisions", new DivisionsViewModel
            {
                Items = await svc.GetDivisionsAsync(null, ct), Classes = await svc.GetClassesAsync(ct),
                Form = form, ShowModal = true
            });
        }

        if (form.Id == 0)
            await svc.CreateDivisionAsync(new CreateDivisionDto { Name = form.Name, ClassId = form.ClassId }, ct);
        else
            await svc.UpdateDivisionAsync(new UpdateDivisionDto { Id = form.Id, Name = form.Name, ClassId = form.ClassId }, ct);

        TempData["Success"] = "Division saved.";
        return RedirectToAction(nameof(Divisions));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteDivision(int id, CancellationToken ct)
    {
        await svc.DeleteDivisionAsync(id, ct);
        TempData["Success"] = "Division deleted.";
        return RedirectToAction(nameof(Divisions));
    }

    // ── Batch ────────────────────────────────────────────────

    public async Task<IActionResult> Batches(CancellationToken ct)
    {
        ViewData["Title"] = "Batches";
        return View(new BatchesViewModel { Items = await svc.GetBatchesAsync(ct) });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveBatch(BatchFormModel form, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Batches";
            return View("Batches", new BatchesViewModel
            {
                Items = await svc.GetBatchesAsync(ct), Form = form, ShowModal = true
            });
        }

        if (form.Id == 0)
            await svc.CreateBatchAsync(new CreateBatchDto { Name = form.Name, IsActive = form.IsActive }, ct);
        else
            await svc.UpdateBatchAsync(new UpdateBatchDto { Id = form.Id, Name = form.Name, IsActive = form.IsActive }, ct);

        TempData["Success"] = "Batch saved.";
        return RedirectToAction(nameof(Batches));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteBatch(int id, CancellationToken ct)
    {
        await svc.DeleteBatchAsync(id, ct);
        TempData["Success"] = "Batch deleted.";
        return RedirectToAction(nameof(Batches));
    }

    // ── Subject ──────────────────────────────────────────────

    public async Task<IActionResult> Subjects(CancellationToken ct)
    {
        ViewData["Title"] = "Subjects";
        return View(new SubjectsViewModel
        {
            Items = await svc.GetSubjectsAsync(null, ct),
            Classes = await svc.GetClassesAsync(ct)
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveSubject(SubjectFormModel form, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Subjects";
            return View("Subjects", new SubjectsViewModel
            {
                Items = await svc.GetSubjectsAsync(null, ct), Classes = await svc.GetClassesAsync(ct),
                Form = form, ShowModal = true
            });
        }

        if (form.Id == 0)
            await svc.CreateSubjectAsync(new CreateSubjectDto { Name = form.Name, ClassId = form.ClassId, IsActive = form.IsActive }, ct);
        else
            await svc.UpdateSubjectAsync(new UpdateSubjectDto { Id = form.Id, Name = form.Name, ClassId = form.ClassId, IsActive = form.IsActive }, ct);

        TempData["Success"] = "Subject saved.";
        return RedirectToAction(nameof(Subjects));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteSubject(int id, CancellationToken ct)
    {
        await svc.DeleteSubjectAsync(id, ct);
        TempData["Success"] = "Subject deleted.";
        return RedirectToAction(nameof(Subjects));
    }
}
