namespace HospitalERP.API.Features.MedicalRecords.Dtos;

public record CreateMedicalRecordDto
{
    public int PatientID { get; init; }
    public int DoctorID { get; init; }
    public int Diagnosesid { get; init; }
    public DateOnly DiagnoseDate { get; init; }
}

