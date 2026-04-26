namespace School.Domain.Entities.Masters;

public class Batch
{
    public int    Id       { get; set; }
    public string Name     { get; set; } = string.Empty;
    public bool   IsActive { get; set; }
}
