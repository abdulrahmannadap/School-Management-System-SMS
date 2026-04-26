using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.DTOs.Student;
using School.Application.Interfaces;

namespace School.API.Controllers;

[ApiController]
[Route("api/students")]
[Authorize]
public class StudentController(IStudentService svc) : ControllerBase
{
    // ── Core ─────────────────────────────────────────────────

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] StudentSearchDto search, CancellationToken ct)
        => Ok(await svc.SearchAsync(search, ct));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id, CancellationToken ct)
    {
        var result = await svc.GetAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStudentDto dto, CancellationToken ct)
    {
        var result = await svc.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] EditStudentDto dto, CancellationToken ct)
    {
        if (id != dto.Id) return BadRequest("Id mismatch.");
        return Ok(await svc.UpdateAsync(dto, ct));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await svc.DeleteAsync(id, ct);
        return NoContent();
    }

    // ── Parent ───────────────────────────────────────────────

    [HttpGet("{studentId:int}/parent")]
    public async Task<IActionResult> GetParent(int studentId, CancellationToken ct)
    {
        var result = await svc.GetParentAsync(studentId, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("{studentId:int}/parent")]
    public async Task<IActionResult> SaveParent(int studentId, [FromBody] StudentParentDto dto, CancellationToken ct)
    {
        dto.StudentId = studentId;
        return Ok(await svc.SaveParentAsync(dto, ct));
    }

    // ── Address ──────────────────────────────────────────────

    [HttpGet("{studentId:int}/address")]
    public async Task<IActionResult> GetAddress(int studentId, CancellationToken ct)
    {
        var result = await svc.GetAddressAsync(studentId, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("{studentId:int}/address")]
    public async Task<IActionResult> SaveAddress(int studentId, [FromBody] StudentAddressDto dto, CancellationToken ct)
    {
        dto.StudentId = studentId;
        return Ok(await svc.SaveAddressAsync(dto, ct));
    }

    // ── Contact ──────────────────────────────────────────────

    [HttpGet("{studentId:int}/contact")]
    public async Task<IActionResult> GetContact(int studentId, CancellationToken ct)
    {
        var result = await svc.GetContactAsync(studentId, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("{studentId:int}/contact")]
    public async Task<IActionResult> SaveContact(int studentId, [FromBody] StudentContactDto dto, CancellationToken ct)
    {
        dto.StudentId = studentId;
        return Ok(await svc.SaveContactAsync(dto, ct));
    }

    // ── Admission ────────────────────────────────────────────

    [HttpGet("{studentId:int}/admission")]
    public async Task<IActionResult> GetAdmission(int studentId, CancellationToken ct)
    {
        var result = await svc.GetAdmissionAsync(studentId, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("{studentId:int}/admission")]
    public async Task<IActionResult> SaveAdmission(int studentId, [FromBody] StudentAdmissionDto dto, CancellationToken ct)
    {
        dto.StudentId = studentId;
        return Ok(await svc.SaveAdmissionAsync(dto, ct));
    }

    // ── Attendance ───────────────────────────────────────────

    [HttpPost("attendance")]
    public async Task<IActionResult> MarkAttendance([FromBody] List<AttendanceEntryDto> entries, CancellationToken ct)
    {
        await svc.MarkAttendanceAsync(entries, ct);
        return NoContent();
    }

    [HttpGet("{studentId:int}/attendance")]
    public async Task<IActionResult> GetAttendance(
        int studentId,
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        CancellationToken ct)
        => Ok(await svc.GetAttendanceAsync(studentId, from, to, ct));

    [HttpGet("{studentId:int}/attendance/summary")]
    public async Task<IActionResult> GetAttendanceSummary(
        int studentId,
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        CancellationToken ct)
        => Ok(await svc.GetAttendanceSummaryAsync(studentId, from, to, ct));

    // ── Remarks ──────────────────────────────────────────────

    [HttpGet("{studentId:int}/remarks")]
    public async Task<IActionResult> GetRemarks(int studentId, CancellationToken ct)
        => Ok(await svc.GetRemarksAsync(studentId, ct));

    [HttpPost("{studentId:int}/remarks")]
    public async Task<IActionResult> AddRemark(int studentId, [FromBody] StudentRemarkDto dto, CancellationToken ct)
    {
        dto.StudentId = studentId;
        await svc.AddRemarkAsync(dto, ct);
        return NoContent();
    }

    // ── Documents ────────────────────────────────────────────

    [HttpGet("{studentId:int}/documents")]
    public async Task<IActionResult> GetDocuments(int studentId, CancellationToken ct)
        => Ok(await svc.GetDocumentsAsync(studentId, ct));

    [HttpPost("{studentId:int}/documents")]
    public async Task<IActionResult> AddDocument(int studentId, [FromBody] StudentDocumentDto dto, CancellationToken ct)
    {
        dto.StudentId = studentId;
        await svc.AddDocumentAsync(dto, ct);
        return NoContent();
    }

    [HttpDelete("documents/{id:int}")]
    public async Task<IActionResult> DeleteDocument(int id, CancellationToken ct)
    {
        await svc.DeleteDocumentAsync(id, ct);
        return NoContent();
    }

    // ── Leave ────────────────────────────────────────────────

    [HttpGet("{studentId:int}/leaves")]
    public async Task<IActionResult> GetLeaves(int studentId, CancellationToken ct)
        => Ok(await svc.GetLeavesAsync(studentId, ct));

    [HttpPost("{studentId:int}/leaves")]
    public async Task<IActionResult> RequestLeave(int studentId, [FromBody] LeaveRequestDto dto, CancellationToken ct)
    {
        dto.StudentId = studentId;
        var leaveId = await svc.RequestLeaveAsync(dto, ct);
        return Ok(new { leaveId });
    }

    [HttpPatch("leaves/{leaveId:int}/approve")]
    public async Task<IActionResult> ApproveLeave(int leaveId, [FromQuery] bool approve, CancellationToken ct)
    {
        await svc.ApproveLeaveAsync(leaveId, approve, ct);
        return NoContent();
    }

    // ── Promote ──────────────────────────────────────────────

    [HttpPost("promote")]
    public async Task<IActionResult> Promote([FromBody] PromoteStudentDto dto, CancellationToken ct)
    {
        await svc.PromoteAsync(dto, ct);
        return NoContent();
    }

    // ── RFID ─────────────────────────────────────────────────

    [HttpGet("{studentId:int}/rfid")]
    public async Task<IActionResult> GetRfid(int studentId, CancellationToken ct)
    {
        var result = await svc.GetRfidAsync(studentId, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("{studentId:int}/rfid")]
    public async Task<IActionResult> AssignRfid(int studentId, [FromBody] StudentRfidDto dto, CancellationToken ct)
    {
        dto.StudentId = studentId;
        await svc.AssignRfidAsync(dto, ct);
        return NoContent();
    }

    // ── Parent App Status ────────────────────────────────────

    [HttpGet("{studentId:int}/parent-app")]
    public async Task<IActionResult> GetParentAppStatus(int studentId, CancellationToken ct)
    {
        var result = await svc.GetParentAppStatusAsync(studentId, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("{studentId:int}/parent-app")]
    public async Task<IActionResult> UpdateParentAppStatus(int studentId, [FromBody] ParentAppStatusDto dto, CancellationToken ct)
    {
        dto.StudentId = studentId;
        await svc.UpdateParentAppStatusAsync(dto, ct);
        return NoContent();
    }
}
