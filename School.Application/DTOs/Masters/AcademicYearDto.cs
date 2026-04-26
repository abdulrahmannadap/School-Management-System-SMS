namespace School.Application.DTOs.Masters;

public class AcademicYearDto
{
    public int      Id        { get; set; }
    public string   Name      { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate   { get; set; }
    public bool     IsActive  { get; set; }
}

public class CreateAcademicYearDto
{
    public string   Name      { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate   { get; set; }
    public bool     IsActive  { get; set; }
}

public class UpdateAcademicYearDto
{
    public int      Id        { get; set; }
    public string   Name      { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate   { get; set; }
    public bool     IsActive  { get; set; }
}
