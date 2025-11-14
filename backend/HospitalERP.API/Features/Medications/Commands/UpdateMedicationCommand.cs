using HospitalERP.API.Features.Medications.Dtos;
using MediatR;

namespace HospitalERP.API.Features.Medications.Commands;

public record UpdateMedicationCommand(UpdateMedicationDto Dto) : IRequest<MedicationDetailDto>;

