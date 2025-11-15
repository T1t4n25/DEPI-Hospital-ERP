namespace HospitalERP.API.Features.Patients.Dtos;

public record UpdatePatientDto : CreatePatientDto
{
    public int PatientID { get; init; }
}

