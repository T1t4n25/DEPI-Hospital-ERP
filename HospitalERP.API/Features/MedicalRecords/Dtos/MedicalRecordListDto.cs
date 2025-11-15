namespace HospitalERP.API.Features.MedicalRecords.Dtos;

public record MedicalRecordListDto
{
    public int RecordID { get; init; }
    public string PatientName { get; init; } = string.Empty;
    public string DoctorName { get; init; } = string.Empty;
    public string Diagnosis { get; init; } = string.Empty;
    public DateOnly DiagnoseDate { get; init; }
}

