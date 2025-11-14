namespace HospitalERP.API.Features.Appointments.Dtos;

public record AppointmentListDto
{
    public int AppointmentID { get; init; }
    public string PatientName { get; init; } = string.Empty;
    public string DoctorName { get; init; } = string.Empty;
    public string ServiceName { get; init; } = string.Empty;
    public DateTime AppointmentDateTime { get; init; }
    public string Status { get; init; } = string.Empty;
}

