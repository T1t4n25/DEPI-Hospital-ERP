namespace HospitalERP.API.Features.Patients.Dtos;

public record PatientDetailDto
{
    public int PatientID { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public DateOnly DateOfBirth { get; init; }
    public int GenderID { get; init; }
    public string GenderName { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public int BloodTypeID { get; init; }
    public string BloodTypeName { get; init; } = string.Empty;
    public string ContactNumber { get; init; } = string.Empty;
}

