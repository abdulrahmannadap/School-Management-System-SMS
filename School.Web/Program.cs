using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using School.Application.Interfaces;
using School.Application.Services.Exam;
using School.Application.Services.Fees;
using School.Application.Services.Inventory;
using School.Application.Services.Library;
using School.Application.Services.Masters;
using School.Application.Services;
using School.Application.Services.Portal;
using School.Application.Services.Staff;
using School.Application.Services.Student;
using School.Infrastructure.Services;
using School.Persistence;
using School.Web.Authorization;
using School.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddScoped<ICurrentSchoolContext, CurrentSchoolContext>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IMastersService, MastersService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<IFeesService, FeesService>();
builder.Services.AddScoped<ILibraryService, LibraryService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IExamService, ExamService>();
builder.Services.AddScoped<IPortalAccountService, PortalAccountService>();
builder.Services.AddScoped<ISchoolService, SchoolService>();
builder.Services.AddScoped<School.Application.Services.MenuService>();
builder.Services.AddScoped<IMenuService, School.Web.Services.CachedMenuService>();
builder.Services.AddScoped<ISystemReportService, SystemReportService>();
builder.Services.AddHttpClient<IBookAggregatorService, BookAggregatorService>();

builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(o =>
{
    o.IdleTimeout    = TimeSpan.FromHours(2);
    o.Cookie.HttpOnly = true;
    o.Cookie.Name    = "SMS.Session";
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.LoginPath        = "/Account/Login";
        o.AccessDeniedPath = "/Account/AccessDenied";
        o.ExpireTimeSpan   = TimeSpan.FromHours(8);
        o.SlidingExpiration = true;
        o.Cookie.HttpOnly  = true;
        o.Cookie.Name      = "SMS.Auth";
    });

builder.Services.AddScoped<IAuthorizationHandler, SchoolAccessHandler>();
builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("SchoolAdminAccess",
        p => p.RequireAuthenticatedUser().AddRequirements(new SchoolAccessRequirement("SchoolAdmin")));
    o.AddPolicy("SchoolAdminOrAccountantAccess",
        p => p.RequireAuthenticatedUser().AddRequirements(new SchoolAccessRequirement("SchoolAdmin", "Accountant")));
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapStaticAssets();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(name: "areas",   pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(name: "default", pattern: "{controller=Account}/{action=Login}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<School.Persistence.AppDbContext>();
    await School.Persistence.MenuSeedData.SeedDefaultsAsync(db);
    await School.Persistence.MenuSeedData.PatchTeacherAdmissionMenuAsync(db);
    await School.Persistence.MenuSeedData.PatchAccountantFeeMenuAsync(db);
}

app.Run();
