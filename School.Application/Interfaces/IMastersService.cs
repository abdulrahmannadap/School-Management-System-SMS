using School.Application.DTOs.Masters;

namespace School.Application.Interfaces;

public interface IMastersService
{
    // ── Academic Year ────────────────────────────────────────
    Task<IReadOnlyList<AcademicYearDto>> GetAcademicYearsAsync(CancellationToken ct = default);
    Task<AcademicYearDto?>              GetAcademicYearAsync(int id, CancellationToken ct = default);
    Task<AcademicYearDto>               CreateAcademicYearAsync(CreateAcademicYearDto dto, CancellationToken ct = default);
    Task<AcademicYearDto>               UpdateAcademicYearAsync(UpdateAcademicYearDto dto, CancellationToken ct = default);
    Task                                DeleteAcademicYearAsync(int id, CancellationToken ct = default);

    // ── Financial Year ───────────────────────────────────────
    Task<IReadOnlyList<FinancialYearDto>> GetFinancialYearsAsync(CancellationToken ct = default);
    Task<FinancialYearDto?>              GetFinancialYearAsync(int id, CancellationToken ct = default);
    Task<FinancialYearDto>               CreateFinancialYearAsync(CreateFinancialYearDto dto, CancellationToken ct = default);
    Task<FinancialYearDto>               UpdateFinancialYearAsync(UpdateFinancialYearDto dto, CancellationToken ct = default);
    Task                                 DeleteFinancialYearAsync(int id, CancellationToken ct = default);

    // ── Class ────────────────────────────────────────────────
    Task<IReadOnlyList<ClassDto>> GetClassesAsync(CancellationToken ct = default);
    Task<ClassDto?>              GetClassAsync(int id, CancellationToken ct = default);
    Task<ClassDto>               CreateClassAsync(CreateClassDto dto, CancellationToken ct = default);
    Task<ClassDto>               UpdateClassAsync(UpdateClassDto dto, CancellationToken ct = default);
    Task                         DeleteClassAsync(int id, CancellationToken ct = default);

    // ── Division ─────────────────────────────────────────────
    Task<IReadOnlyList<DivisionDto>> GetDivisionsAsync(int? classId = null, CancellationToken ct = default);
    Task<DivisionDto?>              GetDivisionAsync(int id, CancellationToken ct = default);
    Task<DivisionDto>               CreateDivisionAsync(CreateDivisionDto dto, CancellationToken ct = default);
    Task<DivisionDto>               UpdateDivisionAsync(UpdateDivisionDto dto, CancellationToken ct = default);
    Task                            DeleteDivisionAsync(int id, CancellationToken ct = default);

    // ── Batch ────────────────────────────────────────────────
    Task<IReadOnlyList<BatchDto>> GetBatchesAsync(CancellationToken ct = default);
    Task<BatchDto?>              GetBatchAsync(int id, CancellationToken ct = default);
    Task<BatchDto>               CreateBatchAsync(CreateBatchDto dto, CancellationToken ct = default);
    Task<BatchDto>               UpdateBatchAsync(UpdateBatchDto dto, CancellationToken ct = default);
    Task                         DeleteBatchAsync(int id, CancellationToken ct = default);

    // ── Subject ──────────────────────────────────────────────
    Task<IReadOnlyList<SubjectDto>> GetSubjectsAsync(int? classId = null, CancellationToken ct = default);
    Task<SubjectDto?>              GetSubjectAsync(int id, CancellationToken ct = default);
    Task<SubjectDto>               CreateSubjectAsync(CreateSubjectDto dto, CancellationToken ct = default);
    Task<SubjectDto>               UpdateSubjectAsync(UpdateSubjectDto dto, CancellationToken ct = default);
    Task                           DeleteSubjectAsync(int id, CancellationToken ct = default);
}
