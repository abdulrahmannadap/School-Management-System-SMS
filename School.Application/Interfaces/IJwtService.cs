using School.Domain.Entities;

namespace School.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user, IReadOnlyList<string>? permissionKeys = null);
}
