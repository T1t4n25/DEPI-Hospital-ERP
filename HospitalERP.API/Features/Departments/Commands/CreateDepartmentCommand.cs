using HospitalERP.API.Features.Departments.Dtos;
using MediatR;

namespace HospitalERP.API.Features.Departments.Commands;

public record CreateDepartmentCommand(CreateDepartmentDto Dto) : IRequest<DepartmentDetailDto>;

