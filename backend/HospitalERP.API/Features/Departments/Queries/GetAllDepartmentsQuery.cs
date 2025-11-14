using HospitalERP.API.Common.Pagination;
using HospitalERP.API.Features.Departments.Dtos;
using MediatR;

namespace HospitalERP.API.Features.Departments.Queries;

public record GetAllDepartmentsQuery(QueryParams QueryParams) : IRequest<PaginatedResponse<DepartmentListDto>>;

