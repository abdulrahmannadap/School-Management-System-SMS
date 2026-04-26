using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.DTOs.Staff;
using School.Application.Interfaces;

namespace School.API.Controllers;

[ApiController]
[Route("api/staff")]
[Authorize]
public class StaffController(IStaffService svc) : ControllerBase
{
    // ── Core ─────────────────────────────────────────────────

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] StaffSearchDto search, CancellationToken ct)
        => Ok(await svc.SearchAsync(search, ct));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id, CancellationToken ct)
    {
        var result = await svc.GetAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStaffDto dto, CancellationToken ct)
    {
        var result = await svc.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] EditStaffDto dto, CancellationToken ct)
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

    // ── Attendance ───────────────────────────────────────────

    [HttpPost("attendance")]
    public async Task<IActionResult> MarkAttendance([FromBody] List<StaffAttendanceDto> entries, CancellationToken ct)
    {
        await svc.MarkAttendanceAsync(entries, ct);
        return NoContent();
    }

    [HttpPatch("attendance")]
    public async Task<IActionResult> UpdateAttendance([FromBody] UpdateStaffAttendanceDto dto, CancellationToken ct)
    {
        await svc.UpdateAttendanceAsync(dto, ct);
        return NoContent();
    }

    [HttpGet("{staffId:int}/attendance")]
    public async Task<IActionResult> GetAttendance(
        int staffId, [FromQuery] DateTime from, [FromQuery] DateTime to, CancellationToken ct)
        => Ok(await svc.GetAttendanceAsync(staffId, from, to, ct));

    [HttpGet("{staffId:int}/attendance/summary")]
    public async Task<IActionResult> GetAttendanceSummary(
        int staffId, [FromQuery] DateTime from, [FromQuery] DateTime to, CancellationToken ct)
        => Ok(await svc.GetAttendanceSummaryAsync(staffId, from, to, ct));

    // ── Photo & Signature ────────────────────────────────────

    [HttpGet("{staffId:int}/photo")]
    public async Task<IActionResult> GetPhoto(int staffId, CancellationToken ct)
    {
        var result = await svc.GetPhotoAsync(staffId, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("{staffId:int}/photo")]
    public async Task<IActionResult> SavePhoto(int staffId, [FromBody] StaffPhotoDto dto, CancellationToken ct)
    {
        dto.StaffId = staffId;
        await svc.SavePhotoAsync(dto, ct);
        return NoContent();
    }

    [HttpGet("{staffId:int}/signature")]
    public async Task<IActionResult> GetSignature(int staffId, CancellationToken ct)
    {
        var result = await svc.GetSignatureAsync(staffId, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("{staffId:int}/signature")]
    public async Task<IActionResult> SaveSignature(int staffId, [FromBody] StaffSignatureDto dto, CancellationToken ct)
    {
        dto.StaffId = staffId;
        await svc.SaveSignatureAsync(dto, ct);
        return NoContent();
    }

    // ── Salary ───────────────────────────────────────────────

    [HttpGet("{staffId:int}/salary-master")]
    public async Task<IActionResult> GetSalaryMaster(int staffId, CancellationToken ct)
    {
        var result = await svc.GetSalaryMasterAsync(staffId, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("{staffId:int}/salary-master")]
    public async Task<IActionResult> SaveSalaryMaster(int staffId, [FromBody] SalaryMasterDto dto, CancellationToken ct)
    {
        dto.StaffId = staffId;
        return Ok(await svc.SaveSalaryMasterAsync(dto, ct));
    }

    [HttpPost("{staffId:int}/salary/generate")]
    public async Task<IActionResult> GenerateSalary(int staffId, [FromBody] GenerateSalaryDto dto, CancellationToken ct)
    {
        dto.StaffId = staffId;
        await svc.GenerateSalaryAsync(dto, ct);
        return NoContent();
    }

    [HttpGet("{staffId:int}/salary/history")]
    public async Task<IActionResult> GetSalaryHistory(int staffId, CancellationToken ct)
        => Ok(await svc.GetSalaryHistoryAsync(staffId, ct));

    // ── Class Teacher Mapping ────────────────────────────────

    [HttpGet("class-teacher")]
    public async Task<IActionResult> GetClassTeacher([FromQuery] int classId, [FromQuery] int divisionId, CancellationToken ct)
    {
        var result = await svc.GetClassTeacherAsync(classId, divisionId, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("class-teacher")]
    public async Task<IActionResult> AssignClassTeacher([FromBody] ClassTeacherMapDto dto, CancellationToken ct)
    {
        await svc.AssignClassTeacherAsync(dto, ct);
        return NoContent();
    }

    [HttpGet("{staffId:int}/classes")]
    public async Task<IActionResult> GetClassesByTeacher(int staffId, CancellationToken ct)
        => Ok(await svc.GetClassesByTeacherAsync(staffId, ct));

    [HttpDelete("class-teacher")]
    public async Task<IActionResult> RemoveClassTeacher([FromQuery] int classId, [FromQuery] int divisionId, CancellationToken ct)
    {
        await svc.RemoveClassTeacherAsync(classId, divisionId, ct);
        return NoContent();
    }

    // ── Teacher Subject Mapping ──────────────────────────────

    [HttpPost("subject-map")]
    public async Task<IActionResult> AssignSubject([FromBody] TeacherSubjectMapDto dto, CancellationToken ct)
    {
        await svc.AssignSubjectAsync(dto, ct);
        return NoContent();
    }

    [HttpGet("{staffId:int}/subjects")]
    public async Task<IActionResult> GetSubjectsByTeacher(int staffId, CancellationToken ct)
        => Ok(await svc.GetSubjectsByTeacherAsync(staffId, ct));

    [HttpGet("subject-map/teachers")]
    public async Task<IActionResult> GetTeachersBySubject([FromQuery] int subjectId, [FromQuery] int classId, CancellationToken ct)
        => Ok(await svc.GetTeachersBySubjectAsync(subjectId, classId, ct));

    [HttpDelete("subject-map")]
    public async Task<IActionResult> RemoveSubjectMap([FromQuery] int staffId, [FromQuery] int subjectId, [FromQuery] int classId, CancellationToken ct)
    {
        await svc.RemoveSubjectMapAsync(staffId, subjectId, classId, ct);
        return NoContent();
    }

    // ── Groups ───────────────────────────────────────────────

    [HttpGet("groups")]
    public async Task<IActionResult> GetGroups(CancellationToken ct)
        => Ok(await svc.GetGroupsAsync(ct));

    [HttpPost("groups")]
    public async Task<IActionResult> CreateGroup([FromBody] StaffGroupDto dto, CancellationToken ct)
        => Ok(await svc.CreateGroupAsync(dto, ct));

    [HttpPost("groups/assign")]
    public async Task<IActionResult> AssignToGroup([FromBody] StaffGroupMapDto dto, CancellationToken ct)
    {
        await svc.AssignToGroupAsync(dto, ct);
        return NoContent();
    }

    [HttpDelete("groups/{groupId:int}/members/{staffId:int}")]
    public async Task<IActionResult> RemoveFromGroup(int groupId, int staffId, CancellationToken ct)
    {
        await svc.RemoveFromGroupAsync(staffId, groupId, ct);
        return NoContent();
    }

    [HttpGet("groups/{groupId:int}/members")]
    public async Task<IActionResult> GetGroupMembers(int groupId, CancellationToken ct)
        => Ok(await svc.GetGroupMembersAsync(groupId, ct));

    // ── Leave Types ──────────────────────────────────────────

    [HttpGet("leave-types")]
    public async Task<IActionResult> GetLeaveTypes(CancellationToken ct)
        => Ok(await svc.GetLeaveTypesAsync(ct));

    [HttpPost("leave-types")]
    public async Task<IActionResult> CreateLeaveType([FromBody] LeaveTypeDto dto, CancellationToken ct)
        => Ok(await svc.CreateLeaveTypeAsync(dto, ct));

    [HttpPut("leave-types/{id:int}")]
    public async Task<IActionResult> UpdateLeaveType(int id, [FromBody] LeaveTypeDto dto, CancellationToken ct)
    {
        if (id != dto.Id) return BadRequest("Id mismatch.");
        return Ok(await svc.UpdateLeaveTypeAsync(dto, ct));
    }

    [HttpDelete("leave-types/{id:int}")]
    public async Task<IActionResult> DeleteLeaveType(int id, CancellationToken ct)
    {
        await svc.DeleteLeaveTypeAsync(id, ct);
        return NoContent();
    }

    // ── Leave ────────────────────────────────────────────────

    [HttpGet("{staffId:int}/leaves")]
    public async Task<IActionResult> GetLeaves(int staffId, CancellationToken ct)
        => Ok(await svc.GetLeavesAsync(staffId, ct));

    [HttpPost("{staffId:int}/leaves")]
    public async Task<IActionResult> RequestLeave(int staffId, [FromBody] StaffLeaveDto dto, CancellationToken ct)
    {
        dto.StaffId = staffId;
        var leaveId = await svc.RequestLeaveAsync(dto, ct);
        return Ok(new { leaveId });
    }

    [HttpPatch("leaves/{leaveId:int}/approve")]
    public async Task<IActionResult> ApproveLeave(int leaveId, [FromQuery] bool approve, CancellationToken ct)
    {
        await svc.ApproveLeaveAsync(leaveId, approve, ct);
        return NoContent();
    }

    [HttpGet("{staffId:int}/leaves/report")]
    public async Task<IActionResult> GetLeaveReport(int staffId, CancellationToken ct)
        => Ok(await svc.GetLeaveReportAsync(staffId, ct));

    // ── Leave Balance ────────────────────────────────────────

    [HttpGet("{staffId:int}/leave-balance")]
    public async Task<IActionResult> GetLeaveBalances(int staffId, CancellationToken ct)
        => Ok(await svc.GetLeaveBalancesAsync(staffId, ct));

    [HttpPost("{staffId:int}/leave-balance")]
    public async Task<IActionResult> SetLeaveBalance(int staffId, [FromBody] LeaveBalanceDto dto, CancellationToken ct)
    {
        dto.StaffId = staffId;
        await svc.SetLeaveBalanceAsync(dto, ct);
        return NoContent();
    }

    // ── Holidays ─────────────────────────────────────────────

    [HttpGet("holidays")]
    public async Task<IActionResult> GetHolidays(CancellationToken ct)
        => Ok(await svc.GetHolidaysAsync(ct));

    [HttpPost("holidays")]
    public async Task<IActionResult> AddHoliday([FromBody] StaffHolidayDto dto, CancellationToken ct)
    {
        await svc.AddHolidayAsync(dto, ct);
        return NoContent();
    }

    [HttpDelete("holidays/{id:int}")]
    public async Task<IActionResult> DeleteHoliday(int id, CancellationToken ct)
    {
        await svc.DeleteHolidayAsync(id, ct);
        return NoContent();
    }

    // ── Documents ────────────────────────────────────────────

    [HttpGet("{staffId:int}/documents")]
    public async Task<IActionResult> GetDocuments(int staffId, CancellationToken ct)
        => Ok(await svc.GetDocumentsAsync(staffId, ct));

    [HttpPost("{staffId:int}/documents")]
    public async Task<IActionResult> AddDocument(int staffId, [FromBody] StaffDocumentDto dto, CancellationToken ct)
    {
        dto.StaffId = staffId;
        await svc.AddDocumentAsync(dto, ct);
        return NoContent();
    }

    [HttpDelete("documents/{id:int}")]
    public async Task<IActionResult> DeleteDocument(int id, CancellationToken ct)
    {
        await svc.DeleteDocumentAsync(id, ct);
        return NoContent();
    }

    // ── Supervisor ───────────────────────────────────────────

    [HttpGet("{staffId:int}/supervisor")]
    public async Task<IActionResult> GetSupervisor(int staffId, CancellationToken ct)
    {
        var result = await svc.GetSupervisorAsync(staffId, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("{staffId:int}/supervisor")]
    public async Task<IActionResult> AssignSupervisor(int staffId, [FromBody] StaffSupervisorDto dto, CancellationToken ct)
    {
        dto.StaffId = staffId;
        await svc.AssignSupervisorAsync(dto, ct);
        return NoContent();
    }

    // ── RFID ─────────────────────────────────────────────────

    [HttpGet("{staffId:int}/rfid")]
    public async Task<IActionResult> GetRfid(int staffId, CancellationToken ct)
    {
        var result = await svc.GetRfidAsync(staffId, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("{staffId:int}/rfid")]
    public async Task<IActionResult> AssignRfid(int staffId, [FromBody] StaffRfidDto dto, CancellationToken ct)
    {
        dto.StaffId = staffId;
        await svc.AssignRfidAsync(dto, ct);
        return NoContent();
    }

    [HttpPost("{staffId:int}/rfid/log")]
    public async Task<IActionResult> LogRfidScan(int staffId, [FromBody] StaffRfidLogDto dto, CancellationToken ct)
    {
        dto.StaffId = staffId;
        await svc.LogRfidScanAsync(dto, ct);
        return NoContent();
    }

    [HttpGet("{staffId:int}/rfid/logs")]
    public async Task<IActionResult> GetRfidLogs(int staffId, CancellationToken ct)
        => Ok(await svc.GetRfidLogsAsync(staffId, ct));
}
