namespace School.Domain.Entities.Masters;

public class Subject
{
    public int    Id       { get; set; }
    public string Name     { get; set; } = string.Empty;
    public int    ClassId  { get; set; }
    public bool   IsActive { get; set; }
}
