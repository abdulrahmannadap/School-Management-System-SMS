using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace School.Web.Areas.SchoolAdmin.Controllers;

[Area("SchoolAdmin")]
[Authorize(Roles = "SchoolAdmin")]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "School Admin Dashboard";
        return View();
    }
}
