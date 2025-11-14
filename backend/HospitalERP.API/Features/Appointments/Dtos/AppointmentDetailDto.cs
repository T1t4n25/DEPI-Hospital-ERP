namespace HospitalERP.API.Features.Appointments.Dtos;

public record AppointmentDetailDto
{
    public int AppointmentID { get; init; }
    public int PatientID { get; init; }
    public string PatientName { get; init; } = string.Empty;
    public int DoctorID { get; init; }
    public string DoctorName { get; init; } = string.Empty;
    public int ServiceID { get; init; }
    public string ServiceName { get; init; } = string.Empty;
    public DateTime AppointmentDateTime { get; init; }
    public string Status { get; init; } = string.Empty;
}

