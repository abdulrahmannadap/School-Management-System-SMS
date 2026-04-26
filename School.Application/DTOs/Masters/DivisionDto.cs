namespace School.Application.DTOs.Masters;

public class DivisionDto
{
    public int    Id      { get; set; }
    public string Name    { get; set; } = string.Empty;
    public int    ClassId { get; set; }
}

public class CreateDivisionDto
{
    public string Name    { get; set; } = string.Empty;
    public int    ClassId { get; set; }
}

public class UpdateDivisionDto
{
    public int    Id      { get; set; }
    public string Name    { get; set; } = string.Empty;
    public int    ClassId { get; set; }
}
