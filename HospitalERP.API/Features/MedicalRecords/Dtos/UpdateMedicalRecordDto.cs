namespace HospitalERP.API.Features.MedicalRecords.Dtos;

public record UpdateMedicalRecordDto : CreateMedicalRecordDto
{
    public int RecordID { get; init; }
}

