namespace HospitalERP.API.Features.MedicalRecords.Dtos;

public record MedicalRecordDetailDto
{
    public int RecordID { get; init; }
    public int PatientID { get; init; }
    public string PatientName { get; init; } = string.Empty;
    public int DoctorID { get; init; }
    public string DoctorName { get; init; } = string.Empty;
    public int Diagnosesid { get; init; }
    public string Diagnosis { get; init; } = string.Empty;
    public DateOnly DiagnoseDate { get; init; }
}

