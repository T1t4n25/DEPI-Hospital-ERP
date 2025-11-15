namespace HospitalERP.API.Features.Departments.Dtos;

public record UpdateDepartmentDto : CreateDepartmentDto
{
    public int DepartmentID { get; init; }
}

