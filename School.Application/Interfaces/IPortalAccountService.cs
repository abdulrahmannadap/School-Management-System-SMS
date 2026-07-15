using School.Application.DTOs.Portal;

namespace School.Application.Interfaces;

public interface IPortalAccountService
{
    Task<bool> HasStudentLoginAsync(int studentId, CancellationToken ct = default);
    Task<bool> CreateStudentLoginAsync(int studentId, CancellationToken ct = default);

    Task<IReadOnlyList<LinkedStudentDto>> GetLinkedStudentsAsync(Guid parentUserId, CancellationToken ct = default);
    Task<bool>                            IsStudentLinkedToParentAsync(Guid parentUserId, int studentId, CancellationToken ct = default);

    Task<IReadOnlyList<ParentAccountLinkDto>> GetAllParentLinksAsync(CancellationToken ct = default);
    Task                                      LinkParentAsync(string email, string password, string fullName, int studentId, CancellationToken ct = default);
    Task                                      UnlinkParentAsync(int linkId, CancellationToken ct = default);
}
