using HospitalERP.API.Common.Pagination;
using HospitalERP.API.Features.Services.Dtos;
using MediatR;

namespace HospitalERP.API.Features.Services.Queries;

public record GetAllServicesQuery(QueryParams QueryParams) : IRequest<PaginatedResponse<ServiceListDto>>;

