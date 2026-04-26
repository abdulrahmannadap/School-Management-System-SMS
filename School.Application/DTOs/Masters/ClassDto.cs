namespace School.Application.DTOs.Masters;

public class ClassDto
{
    public int    Id      { get; set; }
    public string Name    { get; set; } = string.Empty;
    public int    OrderNo { get; set; }
    public bool   IsActive { get; set; }
}

public class CreateClassDto
{
    public string Name    { get; set; } = string.Empty;
    public int    OrderNo { get; set; }
    public bool   IsActive { get; set; }
}

public class UpdateClassDto
{
    public int    Id      { get; set; }
    public string Name    { get; set; } = string.Empty;
    public int    OrderNo { get; set; }
    public bool   IsActive { get; set; }
}
