namespace HospitalERP.API.Features.Departments.Dtos;

public record CreateDepartmentDto
{
    public string DepartmentName { get; init; } = string.Empty;
    public int? ManagerID { get; init; }
}

