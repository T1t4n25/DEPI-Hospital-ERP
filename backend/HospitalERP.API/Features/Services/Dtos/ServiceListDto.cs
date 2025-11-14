namespace HospitalERP.API.Features.Services.Dtos;

public record ServiceListDto
{
    public int ServiceID { get; init; }
    public string ServiceName { get; init; } = string.Empty;
    public decimal Cost { get; init; }
    public string DepartmentName { get; init; } = string.Empty;
}

