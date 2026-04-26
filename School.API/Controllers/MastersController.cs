using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.DTOs.Masters;
using School.Application.Interfaces;

namespace School.API.Controllers;

[ApiController]
[Route("api/masters")]
[Authorize]
public class MastersController(IMastersService svc) : ControllerBase
{
    // ── Academic Year ────────────────────────────────────────

    [HttpGet("academic-years")]
    public async Task<IActionResult> GetAcademicYears(CancellationToken ct)
        => Ok(await svc.GetAcademicYearsAsync(ct));

    [HttpGet("academic-years/{id:int}")]
    public async Task<IActionResult> GetAcademicYear(int id, CancellationToken ct)
    {
        var result = await svc.GetAcademicYearAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("academic-years")]
    public async Task<IActionResult> CreateAcademicYear([FromBody] CreateAcademicYearDto dto, CancellationToken ct)
    {
        var result = await svc.CreateAcademicYearAsync(dto, ct);
        return CreatedAtAction(nameof(GetAcademicYear), new { id = result.Id }, result);
    }

    [HttpPut("academic-years/{id:int}")]
    public async Task<IActionResult> UpdateAcademicYear(int id, [FromBody] UpdateAcademicYearDto dto, CancellationToken ct)
    {
        if (id != dto.Id) return BadRequest("Id mismatch.");
        return Ok(await svc.UpdateAcademicYearAsync(dto, ct));
    }

    [HttpDelete("academic-years/{id:int}")]
    public async Task<IActionResult> DeleteAcademicYear(int id, CancellationToken ct)
    {
        await svc.DeleteAcademicYearAsync(id, ct);
        return NoContent();
    }

    // ── Financial Year ───────────────────────────────────────

    [HttpGet("financial-years")]
    public async Task<IActionResult> GetFinancialYears(CancellationToken ct)
        => Ok(await svc.GetFinancialYearsAsync(ct));

    [HttpGet("financial-years/{id:int}")]
    public async Task<IActionResult> GetFinancialYear(int id, CancellationToken ct)
    {
        var result = await svc.GetFinancialYearAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("financial-years")]
    public async Task<IActionResult> CreateFinancialYear([FromBody] CreateFinancialYearDto dto, CancellationToken ct)
    {
        var result = await svc.CreateFinancialYearAsync(dto, ct);
        return CreatedAtAction(nameof(GetFinancialYear), new { id = result.Id }, result);
    }

    [HttpPut("financial-years/{id:int}")]
    public async Task<IActionResult> UpdateFinancialYear(int id, [FromBody] UpdateFinancialYearDto dto, CancellationToken ct)
    {
        if (id != dto.Id) return BadRequest("Id mismatch.");
        return Ok(await svc.UpdateFinancialYearAsync(dto, ct));
    }

    [HttpDelete("financial-years/{id:int}")]
    public async Task<IActionResult> DeleteFinancialYear(int id, CancellationToken ct)
    {
        await svc.DeleteFinancialYearAsync(id, ct);
        return NoContent();
    }

    // ── Class ────────────────────────────────────────────────

    [HttpGet("classes")]
    public async Task<IActionResult> GetClasses(CancellationToken ct)
        => Ok(await svc.GetClassesAsync(ct));

    [HttpGet("classes/{id:int}")]
    public async Task<IActionResult> GetClass(int id, CancellationToken ct)
    {
        var result = await svc.GetClassAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("classes")]
    public async Task<IActionResult> CreateClass([FromBody] CreateClassDto dto, CancellationToken ct)
    {
        var result = await svc.CreateClassAsync(dto, ct);
        return CreatedAtAction(nameof(GetClass), new { id = result.Id }, result);
    }

    [HttpPut("classes/{id:int}")]
    public async Task<IActionResult> UpdateClass(int id, [FromBody] UpdateClassDto dto, CancellationToken ct)
    {
        if (id != dto.Id) return BadRequest("Id mismatch.");
        return Ok(await svc.UpdateClassAsync(dto, ct));
    }

    [HttpDelete("classes/{id:int}")]
    public async Task<IActionResult> DeleteClass(int id, CancellationToken ct)
    {
        await svc.DeleteClassAsync(id, ct);
        return NoContent();
    }

    // ── Division ─────────────────────────────────────────────

    [HttpGet("divisions")]
    public async Task<IActionResult> GetDivisions([FromQuery] int? classId, CancellationToken ct)
        => Ok(await svc.GetDivisionsAsync(classId, ct));

    [HttpGet("divisions/{id:int}")]
    public async Task<IActionResult> GetDivision(int id, CancellationToken ct)
    {
        var result = await svc.GetDivisionAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("divisions")]
    public async Task<IActionResult> CreateDivision([FromBody] CreateDivisionDto dto, CancellationToken ct)
    {
        var result = await svc.CreateDivisionAsync(dto, ct);
        return CreatedAtAction(nameof(GetDivision), new { id = result.Id }, result);
    }

    [HttpPut("divisions/{id:int}")]
    public async Task<IActionResult> UpdateDivision(int id, [FromBody] UpdateDivisionDto dto, CancellationToken ct)
    {
        if (id != dto.Id) return BadRequest("Id mismatch.");
        return Ok(await svc.UpdateDivisionAsync(dto, ct));
    }

    [HttpDelete("divisions/{id:int}")]
    public async Task<IActionResult> DeleteDivision(int id, CancellationToken ct)
    {
        await svc.DeleteDivisionAsync(id, ct);
        return NoContent();
    }

    // ── Batch ────────────────────────────────────────────────

    [HttpGet("batches")]
    public async Task<IActionResult> GetBatches(CancellationToken ct)
        => Ok(await svc.GetBatchesAsync(ct));

    [HttpGet("batches/{id:int}")]
    public async Task<IActionResult> GetBatch(int id, CancellationToken ct)
    {
        var result = await svc.GetBatchAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("batches")]
    public async Task<IActionResult> CreateBatch([FromBody] CreateBatchDto dto, CancellationToken ct)
    {
        var result = await svc.CreateBatchAsync(dto, ct);
        return CreatedAtAction(nameof(GetBatch), new { id = result.Id }, result);
    }

    [HttpPut("batches/{id:int}")]
    public async Task<IActionResult> UpdateBatch(int id, [FromBody] UpdateBatchDto dto, CancellationToken ct)
    {
        if (id != dto.Id) return BadRequest("Id mismatch.");
        return Ok(await svc.UpdateBatchAsync(dto, ct));
    }

    [HttpDelete("batches/{id:int}")]
    public async Task<IActionResult> DeleteBatch(int id, CancellationToken ct)
    {
        await svc.DeleteBatchAsync(id, ct);
        return NoContent();
    }

    // ── Subject ──────────────────────────────────────────────

    [HttpGet("subjects")]
    public async Task<IActionResult> GetSubjects([FromQuery] int? classId, CancellationToken ct)
        => Ok(await svc.GetSubjectsAsync(classId, ct));

    [HttpGet("subjects/{id:int}")]
    public async Task<IActionResult> GetSubject(int id, CancellationToken ct)
    {
        var result = await svc.GetSubjectAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("subjects")]
    public async Task<IActionResult> CreateSubject([FromBody] CreateSubjectDto dto, CancellationToken ct)
    {
        var result = await svc.CreateSubjectAsync(dto, ct);
        return CreatedAtAction(nameof(GetSubject), new { id = result.Id }, result);
    }

    [HttpPut("subjects/{id:int}")]
    public async Task<IActionResult> UpdateSubject(int id, [FromBody] UpdateSubjectDto dto, CancellationToken ct)
    {
        if (id != dto.Id) return BadRequest("Id mismatch.");
        return Ok(await svc.UpdateSubjectAsync(dto, ct));
    }

    [HttpDelete("subjects/{id:int}")]
    public async Task<IActionResult> DeleteSubject(int id, CancellationToken ct)
    {
        await svc.DeleteSubjectAsync(id, ct);
        return NoContent();
    }
}
