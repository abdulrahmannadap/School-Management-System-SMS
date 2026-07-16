namespace School.Application.Interfaces;

public interface ICurrentSchoolContext
{
    Guid? SchoolId { get; }
    bool  IsImpersonating { get; }
}
