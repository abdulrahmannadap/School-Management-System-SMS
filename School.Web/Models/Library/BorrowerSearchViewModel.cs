namespace School.Web.Models.Library;

public class BorrowerResult
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}

public class BorrowerSearchViewModel
{
    public string BorrowerType { get; set; } = "Student";
    public string? Name { get; set; }
    public string? Code { get; set; }
    public IReadOnlyList<BorrowerResult> Results { get; set; } = [];
}
