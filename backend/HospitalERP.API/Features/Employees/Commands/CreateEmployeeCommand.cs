using HospitalERP.API.Features.Employees.Dtos;
using MediatR;

namespace HospitalERP.API.Features.Employees.Commands;

public record CreateEmployeeCommand(CreateEmployeeDto Dto) : IRequest<EmployeeDetailDto>;

