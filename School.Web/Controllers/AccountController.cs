using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Application.Interfaces;
using School.Domain.Enums;
using School.Persistence;
using School.Web.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace School.Web.Controllers;

public class AccountController(AppDbContext db, IJwtService jwtService) : Controller
{
    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToRoleArea();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var hash = Hash(model.Password);
        var user = await db.Users.FirstOrDefaultAsync(u =>
            u.Email == model.Email && u.PasswordHash == hash && u.IsActive);

        if (user is null)
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View(model);
        }

        var jwt = jwtService.GenerateToken(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name,           user.FullName),
            new(ClaimTypes.Email,          user.Email),
            new(ClaimTypes.Role,           user.Role.ToString()),
            new("jwt",                     jwt),
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
            new AuthenticationProperties { IsPersistent = false });

        return RedirectToRoleArea(user.Role);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }

    public IActionResult AccessDenied() => View();

    // ── helpers ──────────────────────────────────────────────────────────────

    private IActionResult RedirectToRoleArea(UserRole? role = null)
    {
        var r = role?.ToString()
             ?? User.FindFirstValue(ClaimTypes.Role)
             ?? string.Empty;

        return r switch
        {
            nameof(UserRole.SuperAdmin) => RedirectToAction("Index", "Home", new { area = "SuperAdmin" }),
            nameof(UserRole.SchoolAdmin) => RedirectToAction("Index", "Home", new { area = "SchoolAdmin" }),
            nameof(UserRole.Teacher) => RedirectToAction("Index", "Home", new { area = "Teacher" }),
            nameof(UserRole.Accountant) => RedirectToAction("Index", "Home", new { area = "Accountant" }),
            nameof(UserRole.Staff) => RedirectToAction("Index", "Home", new { area = "Staff" }),
            nameof(UserRole.Parent) => RedirectToAction("Index", "Home", new { area = "Parent" }),
            nameof(UserRole.Student) => RedirectToAction("Index", "Home", new { area = "Student" }),
            _ => RedirectToAction(nameof(Login)),
        };
    }

    private static string Hash(string input) =>
        Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(input)));
}
