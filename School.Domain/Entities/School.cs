namespace School.Domain.Entities;

public class School
{
    public Guid     Id            { get; set; } = Guid.NewGuid();
    public string   Name          { get; set; } = string.Empty;
    public string   Address       { get; set; } = string.Empty;
    public string   ContactEmail  { get; set; } = string.Empty;
    public string   ContactPhone  { get; set; } = string.Empty;
    public bool     IsActive      { get; set; } = true;
    public DateTime CreatedAt     { get; set; } = DateTime.UtcNow;
}
