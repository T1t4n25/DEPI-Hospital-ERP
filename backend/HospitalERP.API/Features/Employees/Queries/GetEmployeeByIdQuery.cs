using HospitalERP.API.Features.Employees.Dtos;
using MediatR;

namespace HospitalERP.API.Features.Employees.Queries;

public record GetEmployeeByIdQuery(int EmployeeID) : IRequest<EmployeeDetailDto>;

