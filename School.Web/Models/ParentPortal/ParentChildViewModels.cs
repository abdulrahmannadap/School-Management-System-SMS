using School.Application.DTOs.Student;
using School.Web.Models.StudentPortal;

namespace School.Web.Models.ParentPortal;

public class ParentChildAttendanceViewModel : StudentAttendanceViewModel
{
    public StudentBaseDto Student { get; set; } = new();
}

public class ParentChildResultsViewModel : StudentResultsViewModel
{
    public StudentBaseDto Student { get; set; } = new();
}

public class ParentChildResultDetailViewModel : StudentResultDetailViewModel
{
    public StudentBaseDto Student { get; set; } = new();
}

public class ParentChildFeesViewModel : StudentFeesViewModel
{
    public StudentBaseDto Student { get; set; } = new();
}

public class ParentChildLibraryViewModel : StudentLibraryViewModel
{
    public StudentBaseDto Student { get; set; } = new();
}
