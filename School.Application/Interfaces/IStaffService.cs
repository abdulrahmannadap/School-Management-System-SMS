using School.Application.DTOs.Staff;

namespace School.Application.Interfaces;

public interface IStaffService
{
    // ── Core ─────────────────────────────────────────────────
    Task<IReadOnlyList<StaffBaseDto>> SearchAsync(StaffSearchDto search, CancellationToken ct = default);
    Task<StaffBaseDto?>               GetAsync(int id, CancellationToken ct = default);
    Task<StaffBaseDto>                CreateAsync(CreateStaffDto dto, CancellationToken ct = default);
    Task<StaffBaseDto>                UpdateAsync(EditStaffDto dto, CancellationToken ct = default);
    Task                              DeleteAsync(int id, CancellationToken ct = default);

    // ── Attendance ───────────────────────────────────────────
    Task                                       MarkAttendanceAsync(IEnumerable<StaffAttendanceDto> entries, CancellationToken ct = default);
    Task                                       UpdateAttendanceAsync(UpdateStaffAttendanceDto dto, CancellationToken ct = default);
    Task<IReadOnlyList<StaffAttendanceDto>>    GetAttendanceAsync(int staffId, DateTime from, DateTime to, CancellationToken ct = default);
    Task<StaffAttendanceReportDto>             GetAttendanceSummaryAsync(int staffId, DateTime from, DateTime to, CancellationToken ct = default);

    // ── Photo & Signature ────────────────────────────────────
    Task              SavePhotoAsync(StaffPhotoDto dto, CancellationToken ct = default);
    Task<StaffPhotoDto?> GetPhotoAsync(int staffId, CancellationToken ct = default);
    Task              SaveSignatureAsync(StaffSignatureDto dto, CancellationToken ct = default);
    Task<StaffSignatureDto?> GetSignatureAsync(int staffId, CancellationToken ct = default);

    // ── Salary ───────────────────────────────────────────────
    Task<SalaryMasterDto>               SaveSalaryMasterAsync(SalaryMasterDto dto, CancellationToken ct = default);
    Task<SalaryMasterDto?>              GetSalaryMasterAsync(int staffId, CancellationToken ct = default);
    Task                                GenerateSalaryAsync(GenerateSalaryDto dto, CancellationToken ct = default);
    Task<IReadOnlyList<GenerateSalaryDto>> GetSalaryHistoryAsync(int staffId, CancellationToken ct = default);

    // ── Class Teacher Mapping ────────────────────────────────
    Task                                    AssignClassTeacherAsync(ClassTeacherMapDto dto, CancellationToken ct = default);
    Task<ClassTeacherMapDto?>               GetClassTeacherAsync(int classId, int divisionId, CancellationToken ct = default);
    Task<IReadOnlyList<ClassTeacherMapDto>> GetClassesByTeacherAsync(int staffId, CancellationToken ct = default);
    Task                                    RemoveClassTeacherAsync(int classId, int divisionId, CancellationToken ct = default);

    // ── Teacher Subject Mapping ──────────────────────────────
    Task                                       AssignSubjectAsync(TeacherSubjectMapDto dto, CancellationToken ct = default);
    Task<IReadOnlyList<TeacherSubjectMapDto>>  GetSubjectsByTeacherAsync(int staffId, CancellationToken ct = default);
    Task<IReadOnlyList<TeacherSubjectMapDto>>  GetTeachersBySubjectAsync(int subjectId, int classId, CancellationToken ct = default);
    Task                                       RemoveSubjectMapAsync(int staffId, int subjectId, int classId, CancellationToken ct = default);

    // ── Groups ───────────────────────────────────────────────
    Task<StaffGroupDto>               CreateGroupAsync(StaffGroupDto dto, CancellationToken ct = default);
    Task<IReadOnlyList<StaffGroupDto>> GetGroupsAsync(CancellationToken ct = default);
    Task                              AssignToGroupAsync(StaffGroupMapDto dto, CancellationToken ct = default);
    Task                              RemoveFromGroupAsync(int staffId, int groupId, CancellationToken ct = default);
    Task<IReadOnlyList<StaffBaseDto>> GetGroupMembersAsync(int groupId, CancellationToken ct = default);

    // ── Leave Types ──────────────────────────────────────────
    Task<LeaveTypeDto>               CreateLeaveTypeAsync(LeaveTypeDto dto, CancellationToken ct = default);
    Task<IReadOnlyList<LeaveTypeDto>> GetLeaveTypesAsync(CancellationToken ct = default);
    Task<LeaveTypeDto>               UpdateLeaveTypeAsync(LeaveTypeDto dto, CancellationToken ct = default);
    Task                             DeleteLeaveTypeAsync(int id, CancellationToken ct = default);

    // ── Leave ────────────────────────────────────────────────
    Task<int>                              RequestLeaveAsync(StaffLeaveDto dto, CancellationToken ct = default);
    Task                                   ApproveLeaveAsync(int leaveId, bool approve, CancellationToken ct = default);
    Task<IReadOnlyList<StaffLeaveDetailDto>> GetLeavesAsync(int staffId, CancellationToken ct = default);
    Task<LeaveReportDto>                   GetLeaveReportAsync(int staffId, CancellationToken ct = default);

    // ── Leave Balance ────────────────────────────────────────
    Task<IReadOnlyList<LeaveBalanceDto>> GetLeaveBalancesAsync(int staffId, CancellationToken ct = default);
    Task                                 SetLeaveBalanceAsync(LeaveBalanceDto dto, CancellationToken ct = default);

    // ── Holidays ─────────────────────────────────────────────
    Task                                  AddHolidayAsync(StaffHolidayDto dto, CancellationToken ct = default);
    Task<IReadOnlyList<StaffHolidayDto>>  GetHolidaysAsync(CancellationToken ct = default);
    Task                                  DeleteHolidayAsync(int id, CancellationToken ct = default);

    // ── Documents ────────────────────────────────────────────
    Task                                    AddDocumentAsync(StaffDocumentDto dto, CancellationToken ct = default);
    Task<IReadOnlyList<StaffDocumentDto>>   GetDocumentsAsync(int staffId, CancellationToken ct = default);
    Task                                    DeleteDocumentAsync(int id, CancellationToken ct = default);

    // ── Supervisor ───────────────────────────────────────────
    Task                     AssignSupervisorAsync(StaffSupervisorDto dto, CancellationToken ct = default);
    Task<StaffSupervisorDto?> GetSupervisorAsync(int staffId, CancellationToken ct = default);

    // ── RFID ─────────────────────────────────────────────────
    Task               AssignRfidAsync(StaffRfidDto dto, CancellationToken ct = default);
    Task<StaffRfidDto?> GetRfidAsync(int staffId, CancellationToken ct = default);
    Task               LogRfidScanAsync(StaffRfidLogDto dto, CancellationToken ct = default);
    Task<IReadOnlyList<StaffRfidLogDto>> GetRfidLogsAsync(int staffId, CancellationToken ct = default);
}
