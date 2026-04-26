using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.DTOs.Exam;
using School.Application.Interfaces;

namespace School.API.Controllers;

[ApiController]
[Route("api/exam")]
[Authorize]
public class ExamController(IExamService svc) : ControllerBase
{
    // ── Exam Master ──────────────────────────────────────────

    [HttpGet]
    public async Task<IActionResult> GetExams([FromQuery] int financialYearId, CancellationToken ct)
        => Ok(await svc.GetExamsAsync(financialYearId, ct));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetExam(int id, CancellationToken ct)
    {
        var result = await svc.GetExamAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateExam([FromBody] ExamMasterDto dto, CancellationToken ct)
    {
        var result = await svc.CreateExamAsync(dto, ct);
        return CreatedAtAction(nameof(GetExam), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateExam(int id, [FromBody] ExamMasterDto dto, CancellationToken ct)
    {
        if (id != dto.Id) return BadRequest("Id mismatch.");
        return Ok(await svc.UpdateExamAsync(dto, ct));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteExam(int id, CancellationToken ct)
    {
        await svc.DeleteExamAsync(id, ct);
        return NoContent();
    }

    [HttpPatch("{id:int}/publish")]
    public async Task<IActionResult> PublishExam(int id, [FromBody] ExamPublishDto dto, CancellationToken ct)
    {
        dto.ExamId = id;
        await svc.PublishExamAsync(dto, ct);
        return NoContent();
    }

    // ── Exam Detail ──────────────────────────────────────────

    [HttpGet("{examId:int}/details")]
    public async Task<IActionResult> GetExamDetails(int examId, [FromQuery] int classId, CancellationToken ct)
        => Ok(await svc.GetExamDetailsAsync(examId, classId, ct));

    [HttpPost("{examId:int}/details")]
    public async Task<IActionResult> AddExamDetail(int examId, [FromBody] ExamDetailDto dto, CancellationToken ct)
    {
        dto.ExamId = examId;
        var result = await svc.AddExamDetailAsync(dto, ct);
        return Ok(result);
    }

    [HttpPut("{examId:int}/details/{id:int}")]
    public async Task<IActionResult> UpdateExamDetail(int examId, int id, [FromBody] ExamDetailDto dto, CancellationToken ct)
    {
        dto.ExamId = examId;
        dto.Id     = id;
        return Ok(await svc.UpdateExamDetailAsync(dto, ct));
    }

    [HttpDelete("{examId:int}/details/{id:int}")]
    public async Task<IActionResult> DeleteExamDetail(int examId, int id, CancellationToken ct)
    {
        await svc.DeleteExamDetailAsync(id, ct);
        return NoContent();
    }

    // ── Category ─────────────────────────────────────────────

    [HttpGet("{examId:int}/categories")]
    public async Task<IActionResult> GetCategories(int examId, CancellationToken ct)
        => Ok(await svc.GetCategoriesAsync(examId, ct));

    [HttpPost("{examId:int}/categories")]
    public async Task<IActionResult> AddCategory(int examId, [FromBody] ExamCategoryMapDto dto, CancellationToken ct)
    {
        dto.ExamId = examId;
        await svc.AddCategoryAsync(dto, ct);
        return NoContent();
    }

    [HttpDelete("{examId:int}/categories/{category}")]
    public async Task<IActionResult> RemoveCategory(int examId, string category, CancellationToken ct)
    {
        await svc.RemoveCategoryAsync(examId, category, ct);
        return NoContent();
    }

    // ── Group ────────────────────────────────────────────────

    [HttpGet("{examId:int}/groups")]
    public async Task<IActionResult> GetGroups(int examId, CancellationToken ct)
        => Ok(await svc.GetGroupsAsync(examId, ct));

    [HttpPost("{examId:int}/groups")]
    public async Task<IActionResult> AddGroup(int examId, [FromBody] ExamGroupMapDto dto, CancellationToken ct)
    {
        dto.ExamId = examId;
        await svc.AddGroupAsync(dto, ct);
        return NoContent();
    }

    [HttpDelete("{examId:int}/groups/{groupName}")]
    public async Task<IActionResult> RemoveGroup(int examId, string groupName, CancellationToken ct)
    {
        await svc.RemoveGroupAsync(examId, groupName, ct);
        return NoContent();
    }

    // ── Marks Entry ──────────────────────────────────────────

    [HttpPost("marks")]
    public async Task<IActionResult> SaveMarks([FromBody] MarksEntryDto dto, CancellationToken ct)
    {
        await svc.SaveMarksAsync(dto, ct);
        return NoContent();
    }

    [HttpPost("marks/bulk")]
    public async Task<IActionResult> BulkSaveMarks([FromBody] BulkMarksEntryDto dto, CancellationToken ct)
    {
        await svc.BulkSaveMarksAsync(dto, ct);
        return NoContent();
    }

    [HttpGet("{examId:int}/marks/student/{studentId:int}")]
    public async Task<IActionResult> GetStudentMarks(int examId, int studentId, CancellationToken ct)
        => Ok(await svc.GetMarksAsync(studentId, examId, ct));

    [HttpGet("{examId:int}/marks/class/{classId:int}")]
    public async Task<IActionResult> GetMarksReport(int examId, int classId, CancellationToken ct)
        => Ok(await svc.GetMarksReportAsync(examId, classId, ct));

    // ── Grace Marks ──────────────────────────────────────────

    [HttpPost("grace-marks")]
    public async Task<IActionResult> ApplyGraceMark([FromBody] GraceMarkDto dto, CancellationToken ct)
    {
        await svc.ApplyGraceMarkAsync(dto, ct);
        return NoContent();
    }

    [HttpGet("{examId:int}/grace-marks/student/{studentId:int}")]
    public async Task<IActionResult> GetGraceMarks(int examId, int studentId, CancellationToken ct)
        => Ok(await svc.GetGraceMarksAsync(studentId, examId, ct));

    // ── Result ───────────────────────────────────────────────

    [HttpPost("result/generate")]
    public async Task<IActionResult> GenerateResult([FromBody] GenerateResultDto dto, CancellationToken ct)
    {
        await svc.GenerateResultAsync(dto, ct);
        return NoContent();
    }

    [HttpGet("{examId:int}/result/student/{studentId:int}")]
    public async Task<IActionResult> GetStudentResult(int examId, int studentId, CancellationToken ct)
    {
        var result = await svc.GetStudentResultAsync(studentId, examId, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("{examId:int}/result/class/{classId:int}")]
    public async Task<IActionResult> GetClassResult(int examId, int classId, CancellationToken ct)
        => Ok(await svc.GetClassResultAsync(examId, classId, ct));

    [HttpGet("{examId:int}/result/class/{classId:int}/all")]
    public async Task<IActionResult> GetAllResults(int examId, int classId, CancellationToken ct)
        => Ok(await svc.GetAllResultsAsync(examId, classId, ct));

    // ── Remarks ──────────────────────────────────────────────

    [HttpPost("{examId:int}/remarks")]
    public async Task<IActionResult> SaveRemark(int examId, [FromBody] ExamRemarkDto dto, CancellationToken ct)
    {
        dto.ExamId = examId;
        await svc.SaveRemarkAsync(dto, ct);
        return NoContent();
    }

    [HttpGet("{examId:int}/remarks/student/{studentId:int}")]
    public async Task<IActionResult> GetRemark(int examId, int studentId, CancellationToken ct)
    {
        var result = await svc.GetRemarkAsync(studentId, examId, ct);
        return result is null ? NotFound() : Ok(result);
    }

    // ── Seat Numbers ─────────────────────────────────────────

    [HttpPost("{examId:int}/seats")]
    public async Task<IActionResult> AssignSeatNo(int examId, [FromBody] ExamSeatNoDto dto, CancellationToken ct)
    {
        dto.ExamId = examId;
        await svc.AssignSeatNoAsync(dto, ct);
        return NoContent();
    }

    [HttpGet("{examId:int}/seats/student/{studentId:int}")]
    public async Task<IActionResult> GetSeatNo(int examId, int studentId, CancellationToken ct)
    {
        var result = await svc.GetSeatNoAsync(studentId, examId, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("{examId:int}/seats")]
    public async Task<IActionResult> GetAllSeats(int examId, CancellationToken ct)
        => Ok(await svc.GetExamSeatNosAsync(examId, ct));

    // ── MCQ ──────────────────────────────────────────────────

    [HttpGet("{examId:int}/mcq/{subjectId:int}")]
    public async Task<IActionResult> GetMcqs(int examId, int subjectId, CancellationToken ct)
        => Ok(await svc.GetMcqsAsync(examId, subjectId, ct));

    [HttpPost("{examId:int}/mcq")]
    public async Task<IActionResult> AddMcq(int examId, [FromBody] McqDto dto, CancellationToken ct)
    {
        dto.ExamId = examId;
        return Ok(await svc.AddMcqAsync(dto, ct));
    }

    [HttpPut("{examId:int}/mcq/{id:int}")]
    public async Task<IActionResult> UpdateMcq(int examId, int id, [FromBody] McqDto dto, CancellationToken ct)
    {
        dto.ExamId = examId;
        dto.Id     = id;
        return Ok(await svc.UpdateMcqAsync(dto, ct));
    }

    [HttpDelete("{examId:int}/mcq/{id:int}")]
    public async Task<IActionResult> DeleteMcq(int examId, int id, CancellationToken ct)
    {
        await svc.DeleteMcqAsync(id, ct);
        return NoContent();
    }

    // ── MCQ Answers ──────────────────────────────────────────

    [HttpPost("mcq/submit")]
    public async Task<IActionResult> SubmitAnswers([FromBody] List<McqAnswerDto> answers, CancellationToken ct)
    {
        await svc.SubmitMcqAnswersAsync(answers, ct);
        return NoContent();
    }

    [HttpGet("{examId:int}/mcq/summary/{studentId:int}")]
    public async Task<IActionResult> GetMcqSummary(int examId, int studentId, CancellationToken ct)
        => Ok(await svc.GetMcqSummaryAsync(studentId, examId, ct));
}
