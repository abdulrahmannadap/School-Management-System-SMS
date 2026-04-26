namespace School.Domain.Entities.Masters;

public class Division
{
    public int    Id      { get; set; }
    public string Name    { get; set; } = string.Empty;
    public int    ClassId { get; set; }
}
