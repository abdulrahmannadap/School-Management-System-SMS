namespace School.Application.DTOs.Masters;

public class BatchDto
{
    public int    Id       { get; set; }
    public string Name     { get; set; } = string.Empty;
    public bool   IsActive { get; set; }
}

public class CreateBatchDto
{
    public string Name     { get; set; } = string.Empty;
    public bool   IsActive { get; set; }
}

public class UpdateBatchDto
{
    public int    Id       { get; set; }
    public string Name     { get; set; } = string.Empty;
    public bool   IsActive { get; set; }
}
