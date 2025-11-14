using MediatR;

namespace HospitalERP.API.Features.Employees.Commands;

public record DeleteEmployeeCommand(int EmployeeID) : IRequest<Unit>;

