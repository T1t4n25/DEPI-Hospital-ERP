using MediatR;

namespace HospitalERP.API.Features.Appointments.Commands;

public record DeleteAppointmentCommand(int AppointmentID) : IRequest<Unit>;

