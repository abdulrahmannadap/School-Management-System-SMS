using School.Application.DTOs.Masters;
using School.Application.Interfaces;
using School.Domain.Entities.Masters;

namespace School.Application.Services.Masters;

public class MastersService(
    IGenericRepository<AcademicYear>  academicYearRepo,
    IGenericRepository<FinancialYear> financialYearRepo,
    IGenericRepository<Class>         classRepo,
    IGenericRepository<Division>      divisionRepo,
    IGenericRepository<Batch>         batchRepo,
    IGenericRepository<Subject>       subjectRepo) : IMastersService
{
    // ── Academic Year ────────────────────────────────────────

    public async Task<IReadOnlyList<AcademicYearDto>> GetAcademicYearsAsync(CancellationToken ct = default)
    {
        var list = await academicYearRepo.GetAllAsync(ct);
        return list.Select(MapAcademicYear).ToList();
    }

    public async Task<AcademicYearDto?> GetAcademicYearAsync(int id, CancellationToken ct = default)
    {
        var entity = await academicYearRepo.FirstOrDefaultAsync(x => x.Id == id, ct);
        return entity is null ? null : MapAcademicYear(entity);
    }

    public async Task<AcademicYearDto> CreateAcademicYearAsync(CreateAcademicYearDto dto, CancellationToken ct = default)
    {
        var entity = new AcademicYear
        {
            Name      = dto.Name,
            StartDate = dto.StartDate,
            EndDate   = dto.EndDate,
            IsActive  = dto.IsActive
        };
        await academicYearRepo.AddAsync(entity, ct);
        await academicYearRepo.SaveChangesAsync(ct);
        return MapAcademicYear(entity);
    }

    public async Task<AcademicYearDto> UpdateAcademicYearAsync(UpdateAcademicYearDto dto, CancellationToken ct = default)
    {
        var entity = await academicYearRepo.FirstOrDefaultAsync(x => x.Id == dto.Id, ct)
            ?? throw new KeyNotFoundException($"AcademicYear {dto.Id} not found.");

        entity.Name      = dto.Name;
        entity.StartDate = dto.StartDate;
        entity.EndDate   = dto.EndDate;
        entity.IsActive  = dto.IsActive;

        academicYearRepo.Update(entity);
        await academicYearRepo.SaveChangesAsync(ct);
        return MapAcademicYear(entity);
    }

    public async Task DeleteAcademicYearAsync(int id, CancellationToken ct = default)
    {
        var entity = await academicYearRepo.FirstOrDefaultAsync(x => x.Id == id, ct)
            ?? throw new KeyNotFoundException($"AcademicYear {id} not found.");
        academicYearRepo.Delete(entity);
        await academicYearRepo.SaveChangesAsync(ct);
    }

    // ── Financial Year ───────────────────────────────────────

    public async Task<IReadOnlyList<FinancialYearDto>> GetFinancialYearsAsync(CancellationToken ct = default)
    {
        var list = await financialYearRepo.GetAllAsync(ct);
        return list.Select(MapFinancialYear).ToList();
    }

    public async Task<FinancialYearDto?> GetFinancialYearAsync(int id, CancellationToken ct = default)
    {
        var entity = await financialYearRepo.FirstOrDefaultAsync(x => x.Id == id, ct);
        return entity is null ? null : MapFinancialYear(entity);
    }

    public async Task<FinancialYearDto> CreateFinancialYearAsync(CreateFinancialYearDto dto, CancellationToken ct = default)
    {
        var entity = new FinancialYear
        {
            Name      = dto.Name,
            StartDate = dto.StartDate,
            EndDate   = dto.EndDate,
            IsActive  = dto.IsActive
        };
        await financialYearRepo.AddAsync(entity, ct);
        await financialYearRepo.SaveChangesAsync(ct);
        return MapFinancialYear(entity);
    }

    public async Task<FinancialYearDto> UpdateFinancialYearAsync(UpdateFinancialYearDto dto, CancellationToken ct = default)
    {
        var entity = await financialYearRepo.FirstOrDefaultAsync(x => x.Id == dto.Id, ct)
            ?? throw new KeyNotFoundException($"FinancialYear {dto.Id} not found.");

        entity.Name      = dto.Name;
        entity.StartDate = dto.StartDate;
        entity.EndDate   = dto.EndDate;
        entity.IsActive  = dto.IsActive;

        financialYearRepo.Update(entity);
        await financialYearRepo.SaveChangesAsync(ct);
        return MapFinancialYear(entity);
    }

    public async Task DeleteFinancialYearAsync(int id, CancellationToken ct = default)
    {
        var entity = await financialYearRepo.FirstOrDefaultAsync(x => x.Id == id, ct)
            ?? throw new KeyNotFoundException($"FinancialYear {id} not found.");
        financialYearRepo.Delete(entity);
        await financialYearRepo.SaveChangesAsync(ct);
    }

    // ── Class ────────────────────────────────────────────────

    public async Task<IReadOnlyList<ClassDto>> GetClassesAsync(CancellationToken ct = default)
    {
        var list = await classRepo.FindAsync(null, x => (object)x.OrderNo, true, ct: ct);
        return list.Select(MapClass).ToList();
    }

    public async Task<ClassDto?> GetClassAsync(int id, CancellationToken ct = default)
    {
        var entity = await classRepo.FirstOrDefaultAsync(x => x.Id == id, ct);
        return entity is null ? null : MapClass(entity);
    }

    public async Task<ClassDto> CreateClassAsync(CreateClassDto dto, CancellationToken ct = default)
    {
        var entity = new Class
        {
            Name     = dto.Name,
            OrderNo  = dto.OrderNo,
            IsActive = dto.IsActive
        };
        await classRepo.AddAsync(entity, ct);
        await classRepo.SaveChangesAsync(ct);
        return MapClass(entity);
    }

    public async Task<ClassDto> UpdateClassAsync(UpdateClassDto dto, CancellationToken ct = default)
    {
        var entity = await classRepo.FirstOrDefaultAsync(x => x.Id == dto.Id, ct)
            ?? throw new KeyNotFoundException($"Class {dto.Id} not found.");

        entity.Name     = dto.Name;
        entity.OrderNo  = dto.OrderNo;
        entity.IsActive = dto.IsActive;

        classRepo.Update(entity);
        await classRepo.SaveChangesAsync(ct);
        return MapClass(entity);
    }

    public async Task DeleteClassAsync(int id, CancellationToken ct = default)
    {
        var entity = await classRepo.FirstOrDefaultAsync(x => x.Id == id, ct)
            ?? throw new KeyNotFoundException($"Class {id} not found.");
        classRepo.Delete(entity);
        await classRepo.SaveChangesAsync(ct);
    }

    // ── Division ─────────────────────────────────────────────

    public async Task<IReadOnlyList<DivisionDto>> GetDivisionsAsync(int? classId = null, CancellationToken ct = default)
    {
        var list = classId.HasValue
            ? await divisionRepo.FindAsync(x => x.ClassId == classId.Value, ct)
            : await divisionRepo.GetAllAsync(ct);
        return list.Select(MapDivision).ToList();
    }

    public async Task<DivisionDto?> GetDivisionAsync(int id, CancellationToken ct = default)
    {
        var entity = await divisionRepo.FirstOrDefaultAsync(x => x.Id == id, ct);
        return entity is null ? null : MapDivision(entity);
    }

    public async Task<DivisionDto> CreateDivisionAsync(CreateDivisionDto dto, CancellationToken ct = default)
    {
        var entity = new Division
        {
            Name    = dto.Name,
            ClassId = dto.ClassId
        };
        await divisionRepo.AddAsync(entity, ct);
        await divisionRepo.SaveChangesAsync(ct);
        return MapDivision(entity);
    }

    public async Task<DivisionDto> UpdateDivisionAsync(UpdateDivisionDto dto, CancellationToken ct = default)
    {
        var entity = await divisionRepo.FirstOrDefaultAsync(x => x.Id == dto.Id, ct)
            ?? throw new KeyNotFoundException($"Division {dto.Id} not found.");

        entity.Name    = dto.Name;
        entity.ClassId = dto.ClassId;

        divisionRepo.Update(entity);
        await divisionRepo.SaveChangesAsync(ct);
        return MapDivision(entity);
    }

    public async Task DeleteDivisionAsync(int id, CancellationToken ct = default)
    {
        var entity = await divisionRepo.FirstOrDefaultAsync(x => x.Id == id, ct)
            ?? throw new KeyNotFoundException($"Division {id} not found.");
        divisionRepo.Delete(entity);
        await divisionRepo.SaveChangesAsync(ct);
    }

    // ── Batch ────────────────────────────────────────────────

    public async Task<IReadOnlyList<BatchDto>> GetBatchesAsync(CancellationToken ct = default)
    {
        var list = await batchRepo.GetAllAsync(ct);
        return list.Select(MapBatch).ToList();
    }

    public async Task<BatchDto?> GetBatchAsync(int id, CancellationToken ct = default)
    {
        var entity = await batchRepo.FirstOrDefaultAsync(x => x.Id == id, ct);
        return entity is null ? null : MapBatch(entity);
    }

    public async Task<BatchDto> CreateBatchAsync(CreateBatchDto dto, CancellationToken ct = default)
    {
        var entity = new Batch
        {
            Name     = dto.Name,
            IsActive = dto.IsActive
        };
        await batchRepo.AddAsync(entity, ct);
        await batchRepo.SaveChangesAsync(ct);
        return MapBatch(entity);
    }

    public async Task<BatchDto> UpdateBatchAsync(UpdateBatchDto dto, CancellationToken ct = default)
    {
        var entity = await batchRepo.FirstOrDefaultAsync(x => x.Id == dto.Id, ct)
            ?? throw new KeyNotFoundException($"Batch {dto.Id} not found.");

        entity.Name     = dto.Name;
        entity.IsActive = dto.IsActive;

        batchRepo.Update(entity);
        await batchRepo.SaveChangesAsync(ct);
        return MapBatch(entity);
    }

    public async Task DeleteBatchAsync(int id, CancellationToken ct = default)
    {
        var entity = await batchRepo.FirstOrDefaultAsync(x => x.Id == id, ct)
            ?? throw new KeyNotFoundException($"Batch {id} not found.");
        batchRepo.Delete(entity);
        await batchRepo.SaveChangesAsync(ct);
    }

    // ── Subject ──────────────────────────────────────────────

    public async Task<IReadOnlyList<SubjectDto>> GetSubjectsAsync(int? classId = null, CancellationToken ct = default)
    {
        var list = classId.HasValue
            ? await subjectRepo.FindAsync(x => x.ClassId == classId.Value, ct)
            : await subjectRepo.GetAllAsync(ct);
        return list.Select(MapSubject).ToList();
    }

    public async Task<SubjectDto?> GetSubjectAsync(int id, CancellationToken ct = default)
    {
        var entity = await subjectRepo.FirstOrDefaultAsync(x => x.Id == id, ct);
        return entity is null ? null : MapSubject(entity);
    }

    public async Task<SubjectDto> CreateSubjectAsync(CreateSubjectDto dto, CancellationToken ct = default)
    {
        var entity = new Subject
        {
            Name     = dto.Name,
            ClassId  = dto.ClassId,
            IsActive = dto.IsActive
        };
        await subjectRepo.AddAsync(entity, ct);
        await subjectRepo.SaveChangesAsync(ct);
        return MapSubject(entity);
    }

    public async Task<SubjectDto> UpdateSubjectAsync(UpdateSubjectDto dto, CancellationToken ct = default)
    {
        var entity = await subjectRepo.FirstOrDefaultAsync(x => x.Id == dto.Id, ct)
            ?? throw new KeyNotFoundException($"Subject {dto.Id} not found.");

        entity.Name     = dto.Name;
        entity.ClassId  = dto.ClassId;
        entity.IsActive = dto.IsActive;

        subjectRepo.Update(entity);
        await subjectRepo.SaveChangesAsync(ct);
        return MapSubject(entity);
    }

    public async Task DeleteSubjectAsync(int id, CancellationToken ct = default)
    {
        var entity = await subjectRepo.FirstOrDefaultAsync(x => x.Id == id, ct)
            ?? throw new KeyNotFoundException($"Subject {id} not found.");
        subjectRepo.Delete(entity);
        await subjectRepo.SaveChangesAsync(ct);
    }

    // ── Private mappers ──────────────────────────────────────

    private static AcademicYearDto  MapAcademicYear(AcademicYear e)   => new() { Id = e.Id, Name = e.Name, StartDate = e.StartDate, EndDate = e.EndDate, IsActive = e.IsActive };
    private static FinancialYearDto MapFinancialYear(FinancialYear e) => new() { Id = e.Id, Name = e.Name, StartDate = e.StartDate, EndDate = e.EndDate, IsActive = e.IsActive };
    private static ClassDto         MapClass(Class e)                 => new() { Id = e.Id, Name = e.Name, OrderNo = e.OrderNo, IsActive = e.IsActive };
    private static DivisionDto      MapDivision(Division e)           => new() { Id = e.Id, Name = e.Name, ClassId = e.ClassId };
    private static BatchDto         MapBatch(Batch e)                 => new() { Id = e.Id, Name = e.Name, IsActive = e.IsActive };
    private static SubjectDto       MapSubject(Subject e)             => new() { Id = e.Id, Name = e.Name, ClassId = e.ClassId, IsActive = e.IsActive };
}
