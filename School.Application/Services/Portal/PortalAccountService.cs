using School.Application.Common;
using School.Application.DTOs.Portal;
using School.Application.Interfaces;
using School.Domain.Entities;
using School.Domain.Entities.Student;
using School.Domain.Enums;

namespace School.Application.Services.Portal;

public class PortalAccountService(
    IGenericRepository<User>              userRepo,
    IGenericRepository<ParentStudentLink> linkRepo,
    IGenericRepository<Domain.Entities.Student.Student> studentRepo,
    IGenericRepository<Domain.Entities.Staff.Staff> staffRepo) : IPortalAccountService
{
    public async Task<bool> HasStudentLoginAsync(int studentId, CancellationToken ct = default) =>
        await userRepo.AnyAsync(u => u.StudentId == studentId, ct);

    public Task<HashSet<int>> GetStudentIdsWithLoginAsync(IEnumerable<int> studentIds, CancellationToken ct = default)
    {
        var ids = studentIds.ToList();
        var withLogin = userRepo.QueryNoTracking()
            .Where(u => u.StudentId != null && ids.Contains(u.StudentId.Value))
            .Select(u => u.StudentId!.Value)
            .ToList();
        return Task.FromResult(withLogin.ToHashSet());
    }

    public Task<HashSet<int>> GetStaffIdsWithLoginAsync(IEnumerable<int> staffIds, CancellationToken ct = default)
    {
        var ids = staffIds.ToList();
        var withLogin = userRepo.QueryNoTracking()
            .Where(u => u.StaffId != null && ids.Contains(u.StaffId.Value))
            .Select(u => u.StaffId!.Value)
            .ToList();
        return Task.FromResult(withLogin.ToHashSet());
    }

    public async Task<bool> CreateStudentLoginAsync(int studentId, CancellationToken ct = default)
    {
        if (await HasStudentLoginAsync(studentId, ct))
            return false;

        var student = await studentRepo.FirstOrDefaultAsync(s => s.Id == studentId, ct)
            ?? throw new KeyNotFoundException($"Student {studentId} not found.");

        await userRepo.AddAsync(new User
        {
            FullName     = student.FullName,
            Email        = $"student{student.Id}@student.local",
            PasswordHash = PasswordHasher.Hash(student.GRNumber),
            Role         = UserRole.Student,
            IsActive     = true,
            StudentId    = student.Id,
            SchoolId     = student.SchoolId
        }, ct);
        await userRepo.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> HasStaffLoginAsync(int staffId, CancellationToken ct = default) =>
        await userRepo.AnyAsync(u => u.StaffId == staffId, ct);

    private static readonly UserRole[] StaffLoginRoles = [UserRole.Teacher, UserRole.Accountant, UserRole.Staff];

    public async Task<bool> CreateStaffLoginAsync(int staffId, UserRole role, CancellationToken ct = default)
    {
        if (!StaffLoginRoles.Contains(role))
            throw new ArgumentOutOfRangeException(nameof(role), role, "Staff logins may only be Teacher, Accountant, or Staff.");

        if (await HasStaffLoginAsync(staffId, ct))
            return false;

        var staff = await staffRepo.FirstOrDefaultAsync(s => s.Id == staffId, ct)
            ?? throw new KeyNotFoundException($"Staff {staffId} not found.");

        await userRepo.AddAsync(new User
        {
            FullName     = staff.FullName,
            Email        = $"staff{staff.Id}@staff.local",
            PasswordHash = PasswordHasher.Hash(staff.EmployeeCode),
            Role         = role,
            IsActive     = true,
            StaffId      = staff.Id,
            SchoolId     = staff.SchoolId
        }, ct);
        await userRepo.SaveChangesAsync(ct);
        return true;
    }

    public async Task<IReadOnlyList<LinkedStudentDto>> GetLinkedStudentsAsync(Guid parentUserId, CancellationToken ct = default)
    {
        var links = await linkRepo.FindAsync(l => l.UserId == parentUserId, ct);
        var result = new List<LinkedStudentDto>();

        foreach (var link in links)
        {
            var student = await studentRepo.FirstOrDefaultAsync(s => s.Id == link.StudentId, ct);
            if (student is not null)
                result.Add(new LinkedStudentDto { StudentId = student.Id, FullName = student.FullName, GRNumber = student.GRNumber });
        }

        return result;
    }

    public async Task<bool> IsStudentLinkedToParentAsync(Guid parentUserId, int studentId, CancellationToken ct = default) =>
        await linkRepo.AnyAsync(l => l.UserId == parentUserId && l.StudentId == studentId, ct);

    public async Task<IReadOnlyList<ParentAccountLinkDto>> GetAllParentLinksAsync(CancellationToken ct = default)
    {
        var links = await linkRepo.GetAllAsync(ct);
        var result = new List<ParentAccountLinkDto>();

        foreach (var link in links)
        {
            var user    = await userRepo.FirstOrDefaultAsync(u => u.Id == link.UserId, ct);
            var student = await studentRepo.FirstOrDefaultAsync(s => s.Id == link.StudentId, ct);

            result.Add(new ParentAccountLinkDto
            {
                LinkId      = link.Id,
                UserId      = link.UserId,
                ParentName  = user?.FullName ?? "—",
                ParentEmail = user?.Email ?? "—",
                StudentId   = link.StudentId,
                StudentName = student?.FullName ?? "—",
                GRNumber    = student?.GRNumber ?? "—"
            });
        }

        return result.OrderBy(r => r.ParentEmail).ToList();
    }

    public async Task LinkParentAsync(string email, string password, string fullName, int studentId, CancellationToken ct = default)
    {
        var student = await studentRepo.FirstOrDefaultAsync(s => s.Id == studentId, ct)
            ?? throw new KeyNotFoundException($"Student {studentId} not found.");

        var user = await userRepo.FirstOrDefaultAsync(u => u.Email == email, ct);
        if (user is null)
        {
            user = new User
            {
                FullName     = fullName,
                Email        = email,
                PasswordHash = PasswordHasher.Hash(password),
                Role         = UserRole.Parent,
                IsActive     = true,
                SchoolId     = student.SchoolId
            };
            await userRepo.AddAsync(user, ct);
            await userRepo.SaveChangesAsync(ct);
        }
        else if (user.Role != UserRole.Parent)
        {
            throw new InvalidOperationException($"Email '{email}' is already used by a non-parent account.");
        }

        if (await linkRepo.AnyAsync(l => l.UserId == user.Id && l.StudentId == studentId, ct))
            return;

        await linkRepo.AddAsync(new ParentStudentLink { UserId = user.Id, StudentId = studentId }, ct);
        await linkRepo.SaveChangesAsync(ct);
    }

    public async Task UnlinkParentAsync(int linkId, CancellationToken ct = default)
    {
        var link = await linkRepo.FirstOrDefaultAsync(l => l.Id == linkId, ct)
            ?? throw new KeyNotFoundException($"Link {linkId} not found.");
        linkRepo.Delete(link);
        await linkRepo.SaveChangesAsync(ct);
    }
}
