using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.Interfaces;

namespace School.Web.Areas.SuperAdmin.Controllers;

[Area("SuperAdmin")]
[Authorize(Roles = "SuperAdmin")]
public class ReportsController(ISystemReportService reportSvc) : Controller
{
    public async Task<IActionResult> SystemReport(CancellationToken ct)
    {
        ViewData["Title"] = "System Report";
        return View(await reportSvc.GetAsync(ct));
    }
}
