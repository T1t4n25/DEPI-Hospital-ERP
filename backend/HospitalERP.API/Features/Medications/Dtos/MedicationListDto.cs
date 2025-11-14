namespace HospitalERP.API.Features.Medications.Dtos;

public record MedicationListDto
{
    public int MedicationID { get; init; }
    public string BarCode { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public decimal Cost { get; init; }
    public int? Quantity { get; init; }
}

