using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using School.Application.Interfaces;
using School.Application.Services.Masters;
using School.Application.Services.Exam;
using School.Application.Services.Inventory;
using School.Application.Services.Library;
using School.Application.Services.Fees;
using School.Application.Services.Staff;
using School.Application.Services.Student;
using School.Infrastructure.Services;
using School.Persistence;
using School.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();

builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddScoped<ICurrentSchoolContext, CurrentSchoolContext>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IMastersService, MastersService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<IFeesService, FeesService>();
builder.Services.AddScoped<IExamService, ExamService>();
builder.Services.AddScoped<ILibraryService, LibraryService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = builder.Configuration["Jwt:Issuer"],
            ValidAudience            = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey         = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    SeedData.Seed(db);
}

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
