using MediatR;
using HospitalERP.API.Models.Entities;

namespace HospitalERP.API.Features.Genders.Queries;

public record GetAllGendersQuery : IRequest<IEnumerable<Gender>>;
