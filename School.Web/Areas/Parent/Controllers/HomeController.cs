using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace School.Web.Areas.Parent.Controllers;

[Area("Parent")]
[Authorize(Roles = "Parent")]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Parent Portal";
        return View();
    }
}
