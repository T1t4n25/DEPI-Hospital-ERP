using HospitalERP.API.Common.Pagination;
using HospitalERP.API.Features.Patients.Dtos;
using MediatR;

namespace HospitalERP.API.Features.Patients.Queries;

public record GetAllPatientsQuery(QueryParams QueryParams) : IRequest<PaginatedResponse<PatientListDto>>;

