using HospitalERP.API.Features.Appointments.Dtos;
using MediatR;

namespace HospitalERP.API.Features.Appointments.Commands;

public record UpdateAppointmentCommand(UpdateAppointmentDto Dto) : IRequest<AppointmentDetailDto>;

