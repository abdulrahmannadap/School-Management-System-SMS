using School.Application.DTOs.Library;

namespace School.Web.Models.Library;

public class SearchOnlineViewModel
{
    public string? Query { get; set; }
    public IReadOnlyList<ExternalBookResultDto> Results { get; set; } = [];
    public IReadOnlyList<string> Warnings { get; set; } = [];
}
