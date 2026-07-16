using School.Application.DTOs;
using School.Application.Interfaces;
using School.Domain.Entities.Fees;

namespace School.Application.Services;

public class SystemReportService(
    IGenericRepository<Domain.Entities.School>          schoolRepo,
    IGenericRepository<Domain.Entities.Student.Student>  studentRepo,
    IGenericRepository<Domain.Entities.Staff.Staff>      staffRepo,
    IGenericRepository<FeePayment>                       feePaymentRepo) : ISystemReportService
{
    public Task<IReadOnlyList<SystemReportRowDto>> GetAsync(CancellationToken ct = default)
    {
        var schools = schoolRepo.QueryNoTracking().OrderBy(s => s.Name).ToList();

        // Tenant-scoped entities are filtered to the current school by EF's global query filter,
        // so cross-school aggregation must deliberately bypass it here. GroupBy is followed
        // immediately by Select so EF Core can push the grouping/count/sum down to SQL instead
        // of pulling every row into memory and aggregating client-side.
        var studentCounts = studentRepo.QueryIgnoreFilters()
            .GroupBy(s => s.SchoolId)
            .Select(g => new { SchoolId = g.Key, Count = g.Count() })
            .ToList()
            .ToDictionary(x => x.SchoolId, x => x.Count);

        var staffCounts = staffRepo.QueryIgnoreFilters()
            .GroupBy(s => s.SchoolId)
            .Select(g => new { SchoolId = g.Key, Count = g.Count() })
            .ToList()
            .ToDictionary(x => x.SchoolId, x => x.Count);

        var feeTotals = feePaymentRepo.QueryIgnoreFilters()
            .GroupBy(p => p.SchoolId)
            .Select(g => new { SchoolId = g.Key, Total = g.Sum(p => p.Amount) })
            .ToList()
            .ToDictionary(x => x.SchoolId, x => x.Total);

        IReadOnlyList<SystemReportRowDto> rows = schools.Select(s => new SystemReportRowDto
        {
            Id                = s.Id,
            Name              = s.Name,
            IsActive          = s.IsActive,
            StudentCount      = studentCounts.GetValueOrDefault(s.Id),
            StaffCount        = staffCounts.GetValueOrDefault(s.Id),
            TotalFeeCollected = feeTotals.GetValueOrDefault(s.Id)
        }).ToList();

        return Task.FromResult(rows);
    }
}
