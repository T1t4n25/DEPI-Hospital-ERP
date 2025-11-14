namespace HospitalERP.API.Features.Services.Dtos;

public record CreateServiceDto
{
    public string ServiceName { get; init; } = string.Empty;
    public decimal Cost { get; init; }
    public int DepartmentID { get; init; }
}

