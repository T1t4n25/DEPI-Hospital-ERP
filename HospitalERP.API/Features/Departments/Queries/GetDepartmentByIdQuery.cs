using HospitalERP.API.Features.Departments.Dtos;
using MediatR;

namespace HospitalERP.API.Features.Departments.Queries;

public record GetDepartmentByIdQuery(int DepartmentID) : IRequest<DepartmentDetailDto>;

