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
    public async Task<IReadOnlyList<SystemReportRowDto>> GetAsync(CancellationToken ct = default)
    {
        var schools = await Task.FromResult(schoolRepo.QueryNoTracking().OrderBy(s => s.Name).ToList());

        // Tenant-scoped entities are filtered to the current school by EF's global query filter,
        // so cross-school aggregation must deliberately bypass it here.
        var studentCounts = studentRepo.QueryIgnoreFilters()
            .GroupBy(s => s.SchoolId)
            .ToDictionary(g => g.Key, g => g.Count());

        var staffCounts = staffRepo.QueryIgnoreFilters()
            .GroupBy(s => s.SchoolId)
            .ToDictionary(g => g.Key, g => g.Count());

        var feeTotals = feePaymentRepo.QueryIgnoreFilters()
            .GroupBy(p => p.SchoolId)
            .ToDictionary(g => g.Key, g => g.Sum(p => p.Amount));

        return schools.Select(s => new SystemReportRowDto
        {
            Id                = s.Id,
            Name              = s.Name,
            IsActive          = s.IsActive,
            StudentCount      = studentCounts.GetValueOrDefault(s.Id),
            StaffCount        = staffCounts.GetValueOrDefault(s.Id),
            TotalFeeCollected = feeTotals.GetValueOrDefault(s.Id)
        }).ToList();
    }
}
