using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.DTOs.Student;
using School.Application.Interfaces;
using School.Web.Models.Attendance;

namespace School.Web.Areas.Teacher.Controllers;

[Area("Teacher")]
[Authorize(Roles = "Teacher")]
public class AttendanceController(IStudentService studentSvc, IMastersService mastersSvc) : Controller
{
    public async Task<IActionResult> Mark(int? classId, int? divisionId, DateTime? date, CancellationToken ct)
    {
        ViewData["Title"] = "Mark Attendance";
        var effectiveDate = date ?? DateTime.Today;

        var vm = new MarkAttendanceViewModel
        {
            ClassId    = classId,
            DivisionId = divisionId,
            Date       = effectiveDate,
            Classes    = await mastersSvc.GetClassesAsync(ct),
            Divisions  = await mastersSvc.GetDivisionsAsync(classId, ct)
        };

        if (classId.HasValue && divisionId.HasValue)
        {
            var students = await studentSvc.SearchAsync(
                new StudentSearchDto { ClassId = classId, DivisionId = divisionId }, ct);

            foreach (var student in students)
            {
                var existing = await studentSvc.GetAttendanceAsync(student.Id, effectiveDate, effectiveDate, ct);
                var entry = existing.FirstOrDefault();

                vm.Rows.Add(new AttendanceRowFormModel
                {
                    StudentId   = student.Id,
                    StudentName = student.FullName,
                    GRNumber    = student.GRNumber,
                    IsPresent   = entry?.IsPresent ?? true,
                    IsHalfDay   = entry?.IsHalfDay ?? false,
                    IsLate      = entry?.IsLate ?? false,
                    Remark      = entry?.Remark ?? string.Empty
                });
            }
        }

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(MarkAttendanceViewModel form, CancellationToken ct)
    {
        if (form.Date.HasValue && form.Rows.Count > 0)
        {
            var entries = form.Rows.Select(r => new AttendanceEntryDto
            {
                StudentId = r.StudentId,
                Date      = form.Date.Value,
                IsPresent = r.IsPresent,
                IsHalfDay = r.IsHalfDay,
                IsLate    = r.IsLate,
                Remark    = r.Remark
            });

            await studentSvc.MarkAttendanceAsync(entries, ct);
            TempData["Success"] = "Attendance saved.";
        }

        return RedirectToAction(nameof(Mark), new { classId = form.ClassId, divisionId = form.DivisionId, date = form.Date });
    }

    public async Task<IActionResult> View(int? studentId, string? name, string? grNumber, DateTime? from, DateTime? to, CancellationToken ct)
    {
        ViewData["Title"] = "Attendance Records";

        var vm = new ViewAttendanceViewModel
        {
            Search = new StudentSearchDto { Name = name, GRNumber = grNumber },
            From   = from ?? DateTime.Today.AddDays(-30),
            To     = to ?? DateTime.Today
        };

        if (studentId.HasValue)
        {
            vm.SelectedStudent = await studentSvc.GetAsync(studentId.Value, ct);
            if (vm.SelectedStudent is not null)
            {
                vm.Summary = await studentSvc.GetAttendanceSummaryAsync(studentId.Value, vm.From, vm.To, ct);
                vm.Entries = await studentSvc.GetAttendanceAsync(studentId.Value, vm.From, vm.To, ct);
            }
        }
        else if (!string.IsNullOrWhiteSpace(name) || !string.IsNullOrWhiteSpace(grNumber))
        {
            vm.SearchResults = await studentSvc.SearchAsync(vm.Search, ct);
        }

        return View(vm);
    }
}
