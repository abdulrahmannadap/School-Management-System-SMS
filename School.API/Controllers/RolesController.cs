using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.DTOs.Role;
using School.Application.Interfaces;

namespace School.API.Controllers;

[ApiController]
[Route("api/roles")]
[Authorize(Roles = "SchoolAdmin")]
public class RolesController(IRoleService roleService, ICurrentSchoolContext currentSchool) : ControllerBase
{
    private Guid SchoolId => currentSchool.SchoolId
        ?? throw new InvalidOperationException("No school context for the current user.");

    [HttpGet("permissions")]
    public async Task<IActionResult> GetPermissionCatalog(CancellationToken ct)
        => Ok(await roleService.GetPermissionCatalogAsync(ct));

    [HttpGet]
    public async Task<IActionResult> GetRoles(CancellationToken ct)
        => Ok(await roleService.GetRolesAsync(SchoolId, ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetRole(Guid id, CancellationToken ct)
        => Ok(await roleService.GetRoleAsync(id, ct));

    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] RoleFormDto form, CancellationToken ct)
    {
        var result = await roleService.CreateRoleAsync(SchoolId, form, ct);
        return CreatedAtAction(nameof(GetRole), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateRole(Guid id, [FromBody] RoleFormDto form, CancellationToken ct)
        => Ok(await roleService.UpdateRoleAsync(id, form, ct));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteRole(Guid id, CancellationToken ct)
    {
        await roleService.DeleteRoleAsync(id, ct);
        return NoContent();
    }

    [HttpPost("{roleId:guid}/assign/{userId:guid}")]
    public async Task<IActionResult> AssignRole(Guid roleId, Guid userId, CancellationToken ct)
    {
        await roleService.AssignRoleToUserAsync(userId, roleId, ct);
        return NoContent();
    }

    [HttpPost("unassign/{userId:guid}")]
    public async Task<IActionResult> UnassignRole(Guid userId, CancellationToken ct)
    {
        await roleService.UnassignRoleFromUserAsync(userId, ct);
        return NoContent();
    }
}
