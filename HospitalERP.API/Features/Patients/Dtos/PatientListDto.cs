namespace HospitalERP.API.Features.Patients.Dtos;

public record PatientListDto
{
    public int PatientID { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public DateOnly DateOfBirth { get; init; }
    public string GenderName { get; init; } = string.Empty;
    public string BloodTypeName { get; init; } = string.Empty;
    public string ContactNumber { get; init; } = string.Empty;
}

