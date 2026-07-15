using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.Interfaces;
using System.Security.Claims;

namespace School.Web.Areas.Parent.Controllers;

[Area("Parent")]
[Authorize(Roles = "Parent")]
public abstract class ParentPortalControllerBase(IPortalAccountService portalSvc) : Controller
{
    protected Guid CurrentParentId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    protected async Task<IActionResult?> EnsureLinkedAsync(int studentId, CancellationToken ct)
    {
        if (!await portalSvc.IsStudentLinkedToParentAsync(CurrentParentId, studentId, ct))
            return Forbid();
        return null;
    }
}
