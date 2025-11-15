namespace HospitalERP.API.Features.Employees.Dtos;

public record CreateEmployeeDto
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public int GenderID { get; init; }
    public int RoleID { get; init; }
    public int DepartmentID { get; init; }
    public string ContactNumber { get; init; } = string.Empty;
    public DateOnly HireDate { get; init; }
}

