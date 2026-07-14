using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.DTOs.Exam;
using School.Application.DTOs.Student;
using School.Application.Interfaces;
using School.Web.Models.Marks;

namespace School.Web.Areas.Teacher.Controllers;

[Area("Teacher")]
[Authorize(Roles = "Teacher")]
public class MarksController(IExamService examSvc, IMastersService mastersSvc, IStudentService studentSvc) : Controller
{
    public async Task<IActionResult> Entry(int? financialYearId, int? examId, int? classId, int? subjectId, CancellationToken ct)
    {
        ViewData["Title"] = "Marks Entry";

        var vm = new MarksEntryViewModel
        {
            FinancialYearId = financialYearId,
            ExamId          = examId,
            ClassId         = classId,
            SubjectId       = subjectId,
            FinancialYears  = await mastersSvc.GetFinancialYearsAsync(ct),
            Classes         = await mastersSvc.GetClassesAsync(ct)
        };

        if (financialYearId.HasValue)
            vm.Exams = await examSvc.GetExamsAsync(financialYearId.Value, ct);

        if (classId.HasValue)
            vm.Subjects = await mastersSvc.GetSubjectsAsync(classId, ct);

        if (examId.HasValue && classId.HasValue && subjectId.HasValue)
        {
            var details = await examSvc.GetExamDetailsAsync(examId.Value, classId.Value, ct);
            var detail = details.FirstOrDefault(d => d.SubjectId == subjectId.Value);
            vm.MaxMarks = detail?.MaxMarks;
            vm.PassingMarks = detail?.PassingMarks;

            var students = await studentSvc.SearchAsync(new StudentSearchDto { ClassId = classId, DivisionId = null }, ct);

            foreach (var student in students)
            {
                var marks = await examSvc.GetMarksAsync(student.Id, examId.Value, ct);
                var entry = marks.FirstOrDefault(m => m.SubjectId == subjectId.Value);

                vm.Rows.Add(new MarksRowFormModel
                {
                    StudentId     = student.Id,
                    StudentName   = student.FullName,
                    GRNumber      = student.GRNumber,
                    MarksObtained = entry?.MarksObtained ?? 0,
                    IsAbsent      = entry?.IsAbsent ?? false
                });
            }
        }

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(MarksEntryViewModel form, CancellationToken ct)
    {
        if (form.ExamId.HasValue && form.SubjectId.HasValue && form.Rows.Count > 0)
        {
            var bulk = new BulkMarksEntryDto
            {
                ExamId    = form.ExamId.Value,
                SubjectId = form.SubjectId.Value,
                Students  = form.Rows.Select(r => new MarksEntryDto
                {
                    StudentId     = r.StudentId,
                    MarksObtained = r.MarksObtained,
                    IsAbsent      = r.IsAbsent
                }).ToList()
            };

            await examSvc.BulkSaveMarksAsync(bulk, ct);
            TempData["Success"] = "Marks saved.";
        }

        return RedirectToAction(nameof(Entry), new
        {
            financialYearId = form.FinancialYearId,
            examId          = form.ExamId,
            classId         = form.ClassId,
            subjectId       = form.SubjectId
        });
    }
}
