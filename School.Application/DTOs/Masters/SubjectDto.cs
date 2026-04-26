namespace School.Application.DTOs.Masters;

public class SubjectDto
{
    public int    Id       { get; set; }
    public string Name     { get; set; } = string.Empty;
    public int    ClassId  { get; set; }
    public bool   IsActive { get; set; }
}

public class CreateSubjectDto
{
    public string Name     { get; set; } = string.Empty;
    public int    ClassId  { get; set; }
    public bool   IsActive { get; set; }
}

public class UpdateSubjectDto
{
    public int    Id       { get; set; }
    public string Name     { get; set; } = string.Empty;
    public int    ClassId  { get; set; }
    public bool   IsActive { get; set; }
}
