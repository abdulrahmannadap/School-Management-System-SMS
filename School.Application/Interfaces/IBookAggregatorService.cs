using School.Application.DTOs.Library;

namespace School.Application.Interfaces;

public interface IBookAggregatorService
{
    Task<BookSearchResultDto> SearchAsync(string query, CancellationToken ct = default);
}
