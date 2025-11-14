namespace HospitalERP.API.Features.Services.Dtos;

public record UpdateServiceDto : CreateServiceDto
{
    public int ServiceID { get; init; }
}

