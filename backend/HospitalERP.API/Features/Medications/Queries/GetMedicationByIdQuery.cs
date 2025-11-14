using HospitalERP.API.Features.Medications.Dtos;
using MediatR;

namespace HospitalERP.API.Features.Medications.Queries;

public record GetMedicationByIdQuery(int MedicationID) : IRequest<MedicationDetailDto>;

