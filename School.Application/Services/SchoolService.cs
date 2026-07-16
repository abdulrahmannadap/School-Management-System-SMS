using School.Application.Common;
using School.Application.DTOs;
using School.Application.Interfaces;
using School.Domain.Entities;
using School.Domain.Enums;

namespace School.Application.Services;

public class SchoolService(
    IGenericRepository<Domain.Entities.School> schoolRepo,
    IGenericRepository<User>                   userRepo,
    IMenuService                                menuSvc) : ISchoolService
{
    public async Task<SchoolDto> CreateAsync(SchoolDto dto, CancellationToken ct = default)
    {
        var entity = new Domain.Entities.School
        {
            Name         = dto.Name,
            Address      = dto.Address,
            ContactEmail = dto.ContactEmail,
            ContactPhone = dto.ContactPhone,
            IsActive     = dto.IsActive
        };
        await schoolRepo.AddAsync(entity, ct);
        await schoolRepo.SaveChangesAsync(ct);

        await menuSvc.CloneDefaultsForSchoolAsync(entity.Id, ct);

        return Map(entity);
    }

    public async Task<SchoolDto> UpdateAsync(SchoolDto dto, CancellationToken ct = default)
    {
        var entity = await schoolRepo.FirstOrDefaultAsync(s => s.Id == dto.Id, ct)
            ?? throw new KeyNotFoundException($"School {dto.Id} not found.");

        entity.Name         = dto.Name;
        entity.Address      = dto.Address;
        entity.ContactEmail = dto.ContactEmail;
        entity.ContactPhone = dto.ContactPhone;
        entity.IsActive     = dto.IsActive;

        schoolRepo.Update(entity);
        await schoolRepo.SaveChangesAsync(ct);
        return Map(entity);
    }

    public async Task<SchoolDto?> GetAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await schoolRepo.FirstOrDefaultAsync(s => s.Id == id, ct);
        return entity is null ? null : Map(entity);
    }

    public async Task<IReadOnlyList<SchoolDto>> GetAllAsync(CancellationToken ct = default)
    {
        var list = await schoolRepo.GetAllAsync(ct);
        return list.OrderBy(s => s.Name).Select(Map).ToList();
    }

    public async Task CreateSchoolAdminAsync(Guid schoolId, string email, string password, string fullName, CancellationToken ct = default)
    {
        if (await userRepo.AnyAsync(u => u.Email == email, ct))
            throw new InvalidOperationException($"Email '{email}' is already in use.");

        await userRepo.AddAsync(new User
        {
            FullName     = fullName,
            Email        = email,
            PasswordHash = PasswordHasher.Hash(password),
            Role         = UserRole.SchoolAdmin,
            IsActive     = true,
            SchoolId     = schoolId
        }, ct);
        await userRepo.SaveChangesAsync(ct);
    }

    private static SchoolDto Map(Domain.Entities.School s) => new()
    {
        Id           = s.Id,
        Name         = s.Name,
        Address      = s.Address,
        ContactEmail = s.ContactEmail,
        ContactPhone = s.ContactPhone,
        IsActive     = s.IsActive
    };
}
