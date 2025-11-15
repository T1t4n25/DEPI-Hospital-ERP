namespace HospitalERP.API.Features.Departments.Dtos;

public record DepartmentListDto
{
    public int DepartmentID { get; init; }
    public string DepartmentName { get; init; } = string.Empty;
    public string? ManagerName { get; init; }
}

