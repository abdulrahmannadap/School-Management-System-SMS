# SMS — School Management System

A multi-role school management system built with **ASP.NET Core (.NET 10)**, following clean-architecture layering. It ships two host apps against a shared domain/data layer:

- **School.Web** — MVC + Razor Areas web app (cookie auth), the primary UI.
- **School.API** — REST API (JWT bearer auth) for programmatic/external access.

## Solution layout (Clean Architecture)

```
School.Domain          Entities & enums only, no dependencies
  Entities/             Exam, Fees, Inventory, Library, Masters, Staff, Student, User
  Enums/                UserRole (SuperAdmin, SchoolAdmin, Teacher, Accountant, Staff, Parent, Student)

School.Application     Interfaces + business services (DTOs, use cases)
  DTOs/, Interfaces/, Services/ (Masters, Exam, Inventory, Library, Fees, Staff, Student)

School.Infrastructure   Cross-cutting service implementations (e.g. JwtService)

School.Persistence      EF Core: AppDbContext, Migrations, Repositories, DependencyInjection, SeedData

School.API              JWT-secured REST controllers (Auth, Exam, Fees, Inventory, Library, Masters, Staff, Student)

School.Web              Cookie-authenticated MVC app
  Areas/                SuperAdmin, SchoolAdmin, Teacher, Accountant, Staff, Parent, Student
  Controllers/           Account (login/logout), Home
```

Dependency direction: `Domain ← Application ← Infrastructure/Persistence ← Web/API`. Both hosts call `AddPersistence()` (in `School.Persistence/DependencyInjection.cs`) to register EF Core (SQL Server) and a generic repository (`IGenericRepository<T>`).

## Auth model

- **School.Web** uses cookie auth (`SMS.Auth`, 8h sliding expiration). On login it also mints a JWT (via `IJwtService`) and stores it as a claim, presumably for calling the API on the user's behalf.
- **School.API** validates that JWT with `JwtBearerDefaults` against `Jwt:Issuer` / `Jwt:Audience` / `Jwt:SecretKey` in `appsettings.json`.
- Role-based redirect after login sends each `UserRole` to its own Area (`AccountController.cs`), e.g. Teacher → `/Teacher`, Accountant → `/Accountant`, etc.

## Database

SQL Server via EF Core. Connection string lives in `appsettings.json` per project (`ConnectionStrings:Default`) — currently points at a local named instance, so update it per machine. `School.API`'s `Program.cs` runs `db.Database.Migrate()` and `SeedData.Seed(db)` automatically on startup, so **the API is the one that should be run first** to provision the database.

### Seeded demo accounts (`School.Persistence/SeedData.cs`)

| Role | Email | Password |
|---|---|---|
| SuperAdmin | superadmin@sms.com | SuperAdmin@123 |
| SchoolAdmin | schooladmin@school.com | SchoolAdmin@123 |
| Teacher | teacher@school.com | Teacher@123 |
| Accountant | accounts@school.com | Accounts@123 |
| Staff | staff@school.com | Staff@123 |
| Parent | parent@school.com | Parent@123 |
| Student | student@school.com | Student@123 |

Note: seed passwords are hashed with plain SHA-256 (no salt) — fine for seed/demo data, not production-grade.

## Running locally

1. Update `ConnectionStrings:Default` in both `School.API/appsettings.json` and `School.Web/appsettings.json` to point at your SQL Server instance.
2. Run `School.API` first — it applies migrations and seeds demo users automatically.
3. Run `School.Web` and log in with one of the seeded accounts above; you'll land in that role's Area.

## Recent / in-progress work (uncommitted as of 2026-07-14)

- Branding: sidebar icon replaced with an actual logo image (`School.Web/wwwroot/images/logo_ii.jpeg`, `logo_i.png`), styled via new `.brand-logo` CSS in `_Layout.cshtml`.
- Client-side validation scripts (`jquery.validate`, `jquery.validate.unobtrusive`) wired into `_Layout.cshtml` and the Accountant Home view.
- Logout form fixed to use `asp-area=""` so it posts to the root `AccountController` instead of resolving inside the current Area.
- `School.API.csproj` gained an explicit `Microsoft.OpenApi` package reference.
- Minor formatting cleanup in `AccountController.cs` (using order, switch-expression alignment).

These changes are staged in the working tree but not yet committed.
