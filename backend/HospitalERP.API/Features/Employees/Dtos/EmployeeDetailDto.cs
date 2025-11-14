namespace HospitalERP.API.Features.Employees.Dtos;

public record EmployeeDetailDto
{
    public int EmployeeID { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public int GenderID { get; init; }
    public string GenderName { get; init; } = string.Empty;
    public int RoleID { get; init; }
    public string RoleName { get; init; } = string.Empty;
    public int DepartmentID { get; init; }
    public string DepartmentName { get; init; } = string.Empty;
    public string ContactNumber { get; init; } = string.Empty;
    public DateOnly HireDate { get; init; }
}

