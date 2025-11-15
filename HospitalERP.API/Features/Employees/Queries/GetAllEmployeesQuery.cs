using HospitalERP.API.Common.Pagination;
using HospitalERP.API.Features.Employees.Dtos;
using MediatR;

namespace HospitalERP.API.Features.Employees.Queries;

public record GetAllEmployeesQuery(QueryParams QueryParams) : IRequest<PaginatedResponse<EmployeeListDto>>;

