using HospitalERP.API.Features.Appointments.Dtos;
using MediatR;

namespace HospitalERP.API.Features.Appointments.Queries;

public record GetAppointmentByIdQuery(int AppointmentID) : IRequest<AppointmentDetailDto>;

