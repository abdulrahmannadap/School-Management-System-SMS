using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace School.Web.Areas.Accountant.Controllers;

[Area("Accountant")]
[Authorize(Roles = "Accountant")]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Accountant Dashboard";
        return View();
    }
}
