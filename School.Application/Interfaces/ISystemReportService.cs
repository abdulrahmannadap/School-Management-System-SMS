using School.Application.DTOs;

namespace School.Application.Interfaces;

public interface ISystemReportService
{
    Task<IReadOnlyList<SystemReportRowDto>> GetAsync(CancellationToken ct = default);
}
