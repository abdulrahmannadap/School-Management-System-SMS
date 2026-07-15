using School.Application.DTOs;

namespace School.Application.Interfaces;

public interface ISchoolService
{
    Task<SchoolDto>                CreateAsync(SchoolDto dto, CancellationToken ct = default);
    Task<SchoolDto>                UpdateAsync(SchoolDto dto, CancellationToken ct = default);
    Task<SchoolDto?>               GetAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<SchoolDto>> GetAllAsync(CancellationToken ct = default);

    Task CreateSchoolAdminAsync(Guid schoolId, string email, string password, string fullName, CancellationToken ct = default);
}
