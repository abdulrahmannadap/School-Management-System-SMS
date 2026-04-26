using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace School.Web.Areas.Staff.Controllers;

[Area("Staff")]
[Authorize(Roles = "Staff")]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Staff Dashboard";
        return View();
    }
}
