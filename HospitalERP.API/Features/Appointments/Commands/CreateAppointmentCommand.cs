using HospitalERP.API.Features.Appointments.Dtos;
using MediatR;

namespace HospitalERP.API.Features.Appointments.Commands;

public record CreateAppointmentCommand(CreateAppointmentDto Dto) : IRequest<AppointmentDetailDto>;

