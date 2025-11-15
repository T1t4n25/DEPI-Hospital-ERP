using MediatR;

namespace HospitalERP.API.Features.Departments.Commands;

public record DeleteDepartmentCommand(int DepartmentID) : IRequest<Unit>;

