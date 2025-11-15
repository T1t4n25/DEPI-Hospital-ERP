namespace HospitalERP.API.Features.Appointments.Dtos;

public record CreateAppointmentDto
{
    public int PatientID { get; init; }
    public int DoctorID { get; init; }
    public int ServiceID { get; init; }
    public DateTime AppointmentDateTime { get; init; }
    public string Status { get; init; } = "Scheduled";
}

