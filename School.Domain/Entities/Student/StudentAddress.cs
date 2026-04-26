namespace School.Domain.Entities.Student;

public class StudentAddress
{
    public int    Id        { get; set; }
    public int    StudentId { get; set; }
    public string FlatNo    { get; set; } = string.Empty;
    public string Building  { get; set; } = string.Empty;
    public string Area      { get; set; } = string.Empty;
    public string City      { get; set; } = string.Empty;
    public string Landmark  { get; set; } = string.Empty;
    public string District  { get; set; } = string.Empty;
    public string State     { get; set; } = string.Empty;
    public string PinCode   { get; set; } = string.Empty;
}
