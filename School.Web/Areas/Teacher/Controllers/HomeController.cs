using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace School.Web.Areas.Teacher.Controllers;

[Area("Teacher")]
[Authorize(Roles = "Teacher")]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Teacher Dashboard";
        return View();
    }
}
