namespace HospitalERP.API.Features.Appointments.Dtos;

public record UpdateAppointmentDto : CreateAppointmentDto
{
    public int AppointmentID { get; init; }
}

