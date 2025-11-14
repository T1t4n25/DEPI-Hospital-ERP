namespace HospitalERP.API.Features.Services.Dtos;

public record ServiceDetailDto
{
    public int ServiceID { get; init; }
    public string ServiceName { get; init; } = string.Empty;
    public decimal Cost { get; init; }
    public int DepartmentID { get; init; }
    public string DepartmentName { get; init; } = string.Empty;
}

