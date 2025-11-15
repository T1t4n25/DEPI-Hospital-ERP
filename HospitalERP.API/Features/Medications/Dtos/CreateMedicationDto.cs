namespace HospitalERP.API.Features.Medications.Dtos;

public record CreateMedicationDto
{
    public string BarCode { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal Cost { get; init; }
}

