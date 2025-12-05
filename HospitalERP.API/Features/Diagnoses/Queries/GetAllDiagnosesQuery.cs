using MediatR;
using HospitalERP.API.Models.Entities;

namespace HospitalERP.API.Features.Diagnoses.Queries;

public record GetAllDiagnosesQuery : IRequest<IEnumerable<Diagnosis>>;
