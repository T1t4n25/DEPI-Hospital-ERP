using HospitalERP.API.Features.Medications.Dtos;
using MediatR;

namespace HospitalERP.API.Features.Medications.Commands;

public record CreateMedicationCommand(CreateMedicationDto Dto) : IRequest<MedicationDetailDto>;

