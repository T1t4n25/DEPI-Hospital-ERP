using HospitalERP.API.Common.Pagination;
using HospitalERP.API.Features.Appointments.Dtos;
using MediatR;

namespace HospitalERP.API.Features.Appointments.Queries;

public record GetAllAppointmentsQuery(QueryParams QueryParams) : IRequest<PaginatedResponse<AppointmentListDto>>;

