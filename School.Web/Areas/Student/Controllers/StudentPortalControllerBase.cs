using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace School.Web.Areas.Student.Controllers;

[Area("Student")]
[Authorize(Roles = "Student")]
public abstract class StudentPortalControllerBase : Controller
{
    protected int CurrentStudentId => int.Parse(User.FindFirst("StudentId")?.Value ?? "0");
}
