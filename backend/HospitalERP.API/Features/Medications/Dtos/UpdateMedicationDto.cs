namespace HospitalERP.API.Features.Medications.Dtos;

public record UpdateMedicationDto : CreateMedicationDto
{
    public int MedicationID { get; init; }
}

