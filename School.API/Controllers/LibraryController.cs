using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.DTOs.Library;
using School.Application.Interfaces;

namespace School.API.Controllers;

[ApiController]
[Route("api/library")]
[Authorize]
public class LibraryController(ILibraryService svc) : ControllerBase
{
    // ── Book Category ────────────────────────────────────────

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories(CancellationToken ct)
        => Ok(await svc.GetCategoriesAsync(ct));

    [HttpPost("categories")]
    public async Task<IActionResult> CreateCategory([FromBody] BookCategoryDto dto, CancellationToken ct)
        => Ok(await svc.CreateCategoryAsync(dto, ct));

    [HttpPut("categories/{id:int}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] BookCategoryDto dto, CancellationToken ct)
    {
        if (id != dto.Id) return BadRequest("Id mismatch.");
        return Ok(await svc.UpdateCategoryAsync(dto, ct));
    }

    [HttpDelete("categories/{id:int}")]
    public async Task<IActionResult> DeleteCategory(int id, CancellationToken ct)
    {
        await svc.DeleteCategoryAsync(id, ct);
        return NoContent();
    }

    // ── Books ────────────────────────────────────────────────

    [HttpGet("books")]
    public async Task<IActionResult> SearchBooks([FromQuery] BookSearchDto search, CancellationToken ct)
        => Ok(await svc.SearchBooksAsync(search, ct));

    [HttpGet("books/{id:int}")]
    public async Task<IActionResult> GetBook(int id, CancellationToken ct)
    {
        var result = await svc.GetBookAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("books")]
    public async Task<IActionResult> AddBook([FromBody] LibraryBookDto dto, CancellationToken ct)
    {
        var result = await svc.AddBookAsync(dto, ct);
        return CreatedAtAction(nameof(GetBook), new { id = result.Id }, result);
    }

    [HttpPut("books/{id:int}")]
    public async Task<IActionResult> UpdateBook(int id, [FromBody] LibraryBookDto dto, CancellationToken ct)
    {
        if (id != dto.Id) return BadRequest("Id mismatch.");
        return Ok(await svc.UpdateBookAsync(dto, ct));
    }

    [HttpDelete("books/{id:int}")]
    public async Task<IActionResult> DeleteBook(int id, CancellationToken ct)
    {
        await svc.DeleteBookAsync(id, ct);
        return NoContent();
    }

    // ── Issue & Return ───────────────────────────────────────

    [HttpPost("issue")]
    public async Task<IActionResult> IssueBook([FromBody] IssueBookDto dto, CancellationToken ct)
    {
        var issueId = await svc.IssueBookAsync(dto, ct);
        return Ok(new { issueId });
    }

    [HttpPost("return")]
    public async Task<IActionResult> ReturnBook([FromBody] ReturnBookDto dto, CancellationToken ct)
    {
        await svc.ReturnBookAsync(dto, ct);
        return NoContent();
    }

    [HttpGet("issues/active")]
    public async Task<IActionResult> GetActiveIssues(
        [FromQuery] int? studentId,
        [FromQuery] int? staffId,
        CancellationToken ct)
        => Ok(await svc.GetActiveIssuesAsync(studentId, staffId, ct));

    [HttpGet("issues/history")]
    public async Task<IActionResult> GetIssueHistory(
        [FromQuery] int? studentId,
        [FromQuery] int? staffId,
        CancellationToken ct)
        => Ok(await svc.GetIssueHistoryAsync(studentId, staffId, ct));

    // ── Ledger ───────────────────────────────────────────────

    [HttpGet("books/{bookId:int}/ledger")]
    public async Task<IActionResult> GetLedger(int bookId, CancellationToken ct)
        => Ok(await svc.GetLedgerAsync(bookId, ct));

    // ── Reports ──────────────────────────────────────────────

    [HttpGet("reports/pending")]
    public async Task<IActionResult> GetPendingReturns(CancellationToken ct)
        => Ok(await svc.GetPendingReturnsAsync(ct));

    [HttpGet("reports/issued")]
    public async Task<IActionResult> GetIssuedReport(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        CancellationToken ct)
        => Ok(await svc.GetIssuedReportAsync(from, to, ct));
}
