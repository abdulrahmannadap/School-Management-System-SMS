using School.Application.DTOs.Student;

namespace School.Application.Interfaces;

public interface IStudentService
{
    // ── Core ─────────────────────────────────────────────────
    Task<IReadOnlyList<StudentBaseDto>> SearchAsync(StudentSearchDto search, CancellationToken ct = default);
    Task<StudentBaseDto?>               GetAsync(int id, CancellationToken ct = default);
    Task<StudentBaseDto>                CreateAsync(CreateStudentDto dto, CancellationToken ct = default);
    Task<StudentBaseDto>                UpdateAsync(EditStudentDto dto, CancellationToken ct = default);
    Task                                DeleteAsync(int id, CancellationToken ct = default);

    // ── Parent ───────────────────────────────────────────────
    Task<StudentParentDto?>  GetParentAsync(int studentId, CancellationToken ct = default);
    Task<StudentParentDto>   SaveParentAsync(StudentParentDto dto, CancellationToken ct = default);

    // ── Address ──────────────────────────────────────────────
    Task<StudentAddressDto?> GetAddressAsync(int studentId, CancellationToken ct = default);
    Task<StudentAddressDto>  SaveAddressAsync(StudentAddressDto dto, CancellationToken ct = default);

    // ── Contact ──────────────────────────────────────────────
    Task<StudentContactDto?> GetContactAsync(int studentId, CancellationToken ct = default);
    Task<StudentContactDto>  SaveContactAsync(StudentContactDto dto, CancellationToken ct = default);

    // ── Admission ────────────────────────────────────────────
    Task<StudentAdmissionDto?> GetAdmissionAsync(int studentId, CancellationToken ct = default);
    Task<StudentAdmissionDto>  SaveAdmissionAsync(StudentAdmissionDto dto, CancellationToken ct = default);

    // ── Attendance ───────────────────────────────────────────
    Task                                    MarkAttendanceAsync(IEnumerable<AttendanceEntryDto> entries, CancellationToken ct = default);
    Task<IReadOnlyList<AttendanceEntryDto>> GetAttendanceAsync(int studentId, DateTime from, DateTime to, CancellationToken ct = default);
    Task<AttendanceReportDto>               GetAttendanceSummaryAsync(int studentId, DateTime from, DateTime to, CancellationToken ct = default);

    // ── Remarks ──────────────────────────────────────────────
    Task                                    AddRemarkAsync(StudentRemarkDto dto, CancellationToken ct = default);
    Task<IReadOnlyList<StudentRemarkDto>>   GetRemarksAsync(int studentId, CancellationToken ct = default);

    // ── Documents ────────────────────────────────────────────
    Task                                    AddDocumentAsync(StudentDocumentDto dto, CancellationToken ct = default);
    Task<IReadOnlyList<StudentDocumentDto>> GetDocumentsAsync(int studentId, CancellationToken ct = default);
    Task                                    DeleteDocumentAsync(int id, CancellationToken ct = default);

    // ── Leave ────────────────────────────────────────────────
    Task<int>                              RequestLeaveAsync(LeaveRequestDto dto, CancellationToken ct = default);
    Task                                   ApproveLeaveAsync(int leaveId, bool approve, CancellationToken ct = default);
    Task<IReadOnlyList<LeaveRequestDto>>   GetLeavesAsync(int studentId, CancellationToken ct = default);

    // ── Promote ──────────────────────────────────────────────
    Task PromoteAsync(PromoteStudentDto dto, CancellationToken ct = default);

    // ── RFID ─────────────────────────────────────────────────
    Task              AssignRfidAsync(StudentRfidDto dto, CancellationToken ct = default);
    Task<StudentRfidDto?> GetRfidAsync(int studentId, CancellationToken ct = default);

    // ── Parent App Status ────────────────────────────────────
    Task                   UpdateParentAppStatusAsync(ParentAppStatusDto dto, CancellationToken ct = default);
    Task<ParentAppStatusDto?> GetParentAppStatusAsync(int studentId, CancellationToken ct = default);
}
