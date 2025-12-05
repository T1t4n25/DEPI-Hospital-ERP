using MediatR;
using HospitalERP.API.Models.Entities;

namespace HospitalERP.API.Features.Roles.Queries;

public record GetAllRolesQuery : IRequest<IEnumerable<Role>>;
