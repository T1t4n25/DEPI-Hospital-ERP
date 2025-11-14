namespace HospitalERP.API.Features.Employees.Dtos;

public record EmployeeListDto
{
    public int EmployeeID { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string RoleName { get; init; } = string.Empty;
    public string DepartmentName { get; init; } = string.Empty;
    public string ContactNumber { get; init; } = string.Empty;
}

