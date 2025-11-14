namespace HospitalERP.API.Features.Employees.Dtos;

public record UpdateEmployeeDto : CreateEmployeeDto
{
    public int EmployeeID { get; init; }
}

