namespace School.Application.DTOs.Library;

public class BookSearchDto
{
    public string? Title      { get; set; }
    public string? Author     { get; set; }
    public int?    CategoryId { get; set; }
}
