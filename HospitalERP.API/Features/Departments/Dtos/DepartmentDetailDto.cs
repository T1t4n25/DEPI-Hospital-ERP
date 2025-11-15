namespace HospitalERP.API.Features.Departments.Dtos;

public record DepartmentDetailDto
{
    public int DepartmentID { get; init; }
    public string DepartmentName { get; init; } = string.Empty;
    public int? ManagerID { get; init; }
    public string? ManagerName { get; init; }
}

