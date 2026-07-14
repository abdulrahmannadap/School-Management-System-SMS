using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using School.Application.DTOs.Library;
using School.Application.Interfaces;

namespace School.Infrastructure.Services;

public class BookAggregatorService(HttpClient http, IConfiguration configuration) : IBookAggregatorService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<BookSearchResultDto> SearchAsync(string query, CancellationToken ct = default)
    {
        var results = new List<ExternalBookResultDto>();
        var warnings = new List<string>();

        var tasks = new List<Task>
        {
            RunSource("Open Library", results, warnings, ct, c => SearchOpenLibraryAsync(query, c)),
            RunSource("Gutenberg", results, warnings, ct, c => SearchGutenbergAsync(query, c))
        };

        var apiKey = configuration["GoogleBooks:ApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
            warnings.Add("Google Books: no API key configured in appsettings.json (GoogleBooks:ApiKey).");
        else
            tasks.Add(RunSource("Google Books", results, warnings, ct, c => SearchGoogleBooksAsync(query, apiKey, c)));

        await Task.WhenAll(tasks);

        return new BookSearchResultDto { Results = results, Warnings = warnings };
    }

    private static async Task RunSource(
        string sourceName,
        List<ExternalBookResultDto> results,
        List<string> warnings,
        CancellationToken ct,
        Func<CancellationToken, Task<IReadOnlyList<ExternalBookResultDto>>> search)
    {
        try
        {
            var sourceResults = await search(ct);
            lock (results) results.AddRange(sourceResults);
        }
        catch (Exception ex)
        {
            lock (warnings) warnings.Add($"{sourceName}: {ex.Message}");
        }
    }

    private async Task<IReadOnlyList<ExternalBookResultDto>> SearchOpenLibraryAsync(string query, CancellationToken ct)
    {
        var url = $"https://openlibrary.org/search.json?q={Uri.EscapeDataString(query)}&limit=10";
        var response = await http.GetFromJsonAsync<OpenLibraryResponse>(url, JsonOptions, ct);

        return response?.Docs?.Select(d => new ExternalBookResultDto
        {
            Title    = d.Title ?? string.Empty,
            Author   = d.AuthorName?.FirstOrDefault() ?? string.Empty,
            CoverUrl = d.CoverId.HasValue ? $"https://covers.openlibrary.org/b/id/{d.CoverId}-M.jpg" : string.Empty,
            Source   = "Open Library",
            InfoLink = d.Key is not null ? $"https://openlibrary.org{d.Key}" : string.Empty
        }).ToList() ?? [];
    }

    private async Task<IReadOnlyList<ExternalBookResultDto>> SearchGoogleBooksAsync(string query, string apiKey, CancellationToken ct)
    {
        var url = $"https://www.googleapis.com/books/v1/volumes?q={Uri.EscapeDataString(query)}&maxResults=10&key={Uri.EscapeDataString(apiKey)}";
        var response = await http.GetFromJsonAsync<GoogleBooksResponse>(url, JsonOptions, ct);

        return response?.Items?.Select(i => new ExternalBookResultDto
        {
            Title    = i.VolumeInfo?.Title ?? string.Empty,
            Author   = i.VolumeInfo?.Authors?.FirstOrDefault() ?? string.Empty,
            CoverUrl = i.VolumeInfo?.ImageLinks?.Thumbnail ?? string.Empty,
            Source   = "Google Books",
            InfoLink = i.VolumeInfo?.InfoLink ?? string.Empty
        }).ToList() ?? [];
    }

    private async Task<IReadOnlyList<ExternalBookResultDto>> SearchGutenbergAsync(string query, CancellationToken ct)
    {
        var url = $"https://gutendex.com/books/?search={Uri.EscapeDataString(query)}";
        var response = await http.GetFromJsonAsync<GutendexResponse>(url, JsonOptions, ct);

        return response?.Results?.Select(r => new ExternalBookResultDto
        {
            Title    = r.Title ?? string.Empty,
            Author   = r.Authors?.FirstOrDefault()?.Name ?? string.Empty,
            CoverUrl = r.Formats?.GetValueOrDefault("image/jpeg") ?? string.Empty,
            Source   = "Gutenberg",
            InfoLink = $"https://www.gutenberg.org/ebooks/{r.Id}"
        }).ToList() ?? [];
    }

    // ── Response shapes ──────────────────────────────────────

    private class OpenLibraryResponse
    {
        [JsonPropertyName("docs")] public List<OpenLibraryDoc>? Docs { get; set; }
    }

    private class OpenLibraryDoc
    {
        public string? Title { get; set; }
        [JsonPropertyName("author_name")] public List<string>? AuthorName { get; set; }
        [JsonPropertyName("cover_i")] public int? CoverId { get; set; }
        public string? Key { get; set; }
    }

    private class GoogleBooksResponse
    {
        public List<GoogleBookItem>? Items { get; set; }
    }

    private class GoogleBookItem
    {
        [JsonPropertyName("volumeInfo")] public GoogleVolumeInfo? VolumeInfo { get; set; }
    }

    private class GoogleVolumeInfo
    {
        public string? Title { get; set; }
        public List<string>? Authors { get; set; }
        [JsonPropertyName("imageLinks")] public GoogleImageLinks? ImageLinks { get; set; }
        [JsonPropertyName("infoLink")] public string? InfoLink { get; set; }
    }

    private class GoogleImageLinks
    {
        public string? Thumbnail { get; set; }
    }

    private class GutendexResponse
    {
        public List<GutendexBook>? Results { get; set; }
    }

    private class GutendexBook
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public List<GutendexAuthor>? Authors { get; set; }
        public Dictionary<string, string>? Formats { get; set; }
    }

    private class GutendexAuthor
    {
        public string? Name { get; set; }
    }
}
