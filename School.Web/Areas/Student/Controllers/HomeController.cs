using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace School.Web.Areas.Student.Controllers;

[Area("Student")]
[Authorize(Roles = "Student")]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Student Portal";
        return View();
    }
}
