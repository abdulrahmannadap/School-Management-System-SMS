using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.Interfaces;

namespace School.Web.Controllers;

[Authorize(Roles = "SuperAdmin")]
public class ImpersonationController(ISchoolService schoolSvc) : Controller
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Start(Guid schoolId, CancellationToken ct)
    {
        var school = await schoolSvc.GetAsync(schoolId, ct)
            ?? throw new KeyNotFoundException($"School {schoolId} not found.");

        HttpContext.Session.SetString("ImpersonatedSchoolId", school.Id.ToString());
        return RedirectToAction("Index", "Home", new { area = "SchoolAdmin" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Stop()
    {
        HttpContext.Session.Remove("ImpersonatedSchoolId");
        return RedirectToAction("Index", "Home", new { area = "SuperAdmin" });
    }
}
