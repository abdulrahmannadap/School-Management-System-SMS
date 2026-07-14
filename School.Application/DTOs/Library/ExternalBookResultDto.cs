namespace School.Application.DTOs.Library;

public class ExternalBookResultDto
{
    public string Title    { get; set; } = string.Empty;
    public string Author   { get; set; } = string.Empty;
    public string CoverUrl { get; set; } = string.Empty;
    public string Source   { get; set; } = string.Empty;
    public string InfoLink { get; set; } = string.Empty;
}

public class BookSearchResultDto
{
    public IReadOnlyList<ExternalBookResultDto> Results { get; set; } = [];
    public IReadOnlyList<string> Warnings { get; set; } = [];
}
